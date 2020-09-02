using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SawanSecsDll
{
    public enum PROCESS_MSG_RESULT
    {
        PROCESS_FINISH = 0,         //AP無需回覆
        PROCESS_NOT_FINISH = 1,     //AP需回覆與執行後續動作
        ALREADY_REPLIED = 2,        //AP無需回覆但可能須執行後續動作
    }

    public enum CONTROL_STATE
    {
        EQUIPMENT_OFF_LINE = 0,
        ATTEMPT_ON_LINE = 1,
        HOST_OFF_LINE = 2,
        ON_LINE_LOCATE = 3,
        ON_LINE_REMOTE = 4
    }

    public partial class SanwaBaseExec
    {
        public string _mdln;
        public string _softRev;
        public int _dataID;        //sent report

        public SecsGem _secsGem;


        //提供Interface給外部紀錄Log
        public ISecsGemLogger _logger;

        public CONTROL_STATE _currentState { get; set; }
        public SecsMessageList _secsMessages;
        public Dictionary<string, SanwaSV> _svList;
        public Dictionary<string, SanwaEC> _ecList;
        public Dictionary<string, SanwaAlarm> _alarmList;
        public List<string> _terminalMSGList;

        public SanwaBaseExec(SecsMessageList secsMessages)
        {
            _secsMessages = secsMessages;

            _mdln = "TestMDLN";
            _softRev = "Test_SOFTREV";

            _currentState = CONTROL_STATE.ON_LINE_LOCATE;

            _dataID = 0;

            _secsGem = null;
        }
        public void Initialize()
        {
            _ecList.TryGetValue(ECName.GEM_INIT_CONTROL_STATE, out SanwaEC sanwaEC);
            if (sanwaEC == null)
            {
                _logger.Error("GEM_INIT_CONTROL_STATE_NO_DEFINED");
            }
            else
            {
                /// 1:OffLine, 2:OnLine
                if ("1" == sanwaEC._defaultValue.ToString())
                {
                    _ecList.TryGetValue(ECName.GEM_OFF_LINE_SUBSTATE, out sanwaEC);
                    if (sanwaEC == null)
                    {
                        _logger.Error("GEM_ON_LINE_SUBSTATE NO DEFINED");
                    }
                    else
                    {
                        /// 1:Eqp. OFF-line , 2:Attempt On-line , 3:Host Off-line
                        if ("1" == sanwaEC._defaultValue.ToString())
                        {
                            _currentState = CONTROL_STATE.EQUIPMENT_OFF_LINE;
                        }
                        else if ("2" == sanwaEC._defaultValue.ToString())
                        {
                            _currentState = CONTROL_STATE.ATTEMPT_ON_LINE;
                        }
                        else if ("3" == sanwaEC._defaultValue.ToString())
                        {
                            _currentState = CONTROL_STATE.HOST_OFF_LINE;
                        }
                    }
                }
                else
                {
                    _ecList.TryGetValue(ECName.GEM_ON_LINE_SUBSTATE, out sanwaEC);
                    if (sanwaEC == null)
                    {
                        _logger.Error("GEM_ON_LINE_SUBSTATE NO DEFINED");
                    }
                    else
                    {
                        //Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
                        if ("4" == sanwaEC._defaultValue.ToString())
                        {
                            _currentState = CONTROL_STATE.ON_LINE_LOCATE;
                        }
                        else if ("5" == sanwaEC._defaultValue.ToString())
                        {
                            _currentState = CONTROL_STATE.ON_LINE_REMOTE;
                        }
                    }
                }
            }
        }
        private void ReplyUnrecognizedStreamType(PrimaryMessageWrapper e)
        {
            e.ReplyAsync(_secsMessages[9, 3].FirstOrDefault());
        }
        private void ReplyUnrecognizedFunctionType(PrimaryMessageWrapper e)
        {
            e.ReplyAsync(_secsMessages[9, 5].FirstOrDefault());
        }
        private void ReplyIllegalData(PrimaryMessageWrapper e)
        {
            e.ReplyAsync(_secsMessages[9, 7].FirstOrDefault());
        }
        private bool IsOfflineState()
        {
            bool bRet = false;

            if (_currentState == CONTROL_STATE.EQUIPMENT_OFF_LINE ||
                    _currentState == CONTROL_STATE.ATTEMPT_ON_LINE ||
                    _currentState == CONTROL_STATE.HOST_OFF_LINE)
            {
                bRet = true;
            }

            return bRet;
        }
        public  Task <PROCESS_MSG_RESULT> ProcessMessage(PrimaryMessageWrapper e)
        {
            Task<PROCESS_MSG_RESULT> task =
                Task.Factory.StartNew(() =>
                {
                    PROCESS_MSG_RESULT lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;

                    SecsMessage receiveSecsmsg;
                    SecsMessage replySecsmsg;
                    try
                    {
                        receiveSecsmsg = e.Message;
                        replySecsmsg = _secsMessages[e.Message.S, (byte)(e.Message.F + 1)].FirstOrDefault();

                        if (replySecsmsg == null || receiveSecsmsg == null)
                        {
                            //無相關訊息
                            ReplyIllegalData(e);
                            lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;
                            return lResult;
                        }

                        lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;
                        //DateTimeRequest

                        SecsMessage tempReplySecsmsg = new SecsMessage(replySecsmsg.S, replySecsmsg.F, replySecsmsg.Name);

                        if (replySecsmsg.F % 2 == 0)
                            tempReplySecsmsg.ReplyExpected = false;

                        if (1 == replySecsmsg.S)
                        {
                            if (2 == replySecsmsg.F)
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                }
                                else
                                {
                                    ReplyMDLNAndSOFTREV(tempReplySecsmsg, replySecsmsg, e);
                                }
                            }
                            else if (4 == replySecsmsg.F)//Selected Equipment Status Data (SSD) M,H<-E
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                ReplyS1F4(e, receiveSecsmsg, tempReplySecsmsg);

                            }
                            else if (12 == replySecsmsg.F)// Status Variable Namelist Reply(SVNRR) M,H<-E
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                //1.複製"收到訊息格式"給"回覆訊息的格式"
                                //2.根據訊息長度決定根據SVID回覆，還是全部回覆
                                //2.a.將個別SVID存入List裡面
                                //2.b.將所有SVID存入List裡面   
                                //3.根據SVID開始寫檔

                                //1.複製"收到訊息格式"給"回覆訊息的格式"
                                CopyReceiveMsgToReply(ref receiveSecsmsg, ref tempReplySecsmsg);

                                List<string> svIDList = new List<string>();
                                if (tempReplySecsmsg.SecsItem.Count > 0)//2.根據訊息長度決定根據SVID回覆，還是全部回覆
                                {
                                    //a.將個別SVID存入List裡面
                                    foreach (Item items in receiveSecsmsg.SecsItem.Items)
                                    {
                                        //回覆U4格式
                                        if (items.Format == SecsFormat.U4)
                                        {
                                            var key = items.GetValue<uint>();
                                            string id = key.ToString();
                                            svIDList.Add(id);
                                        }
                                    }
                                }
                                else
                                {
                                    //2.b.將所有SVID存入List裡面 
                                    foreach (KeyValuePair<string, SanwaSV> item in _svList)
                                    {
                                        svIDList.Add(item.Key);
                                    }
                                }

                                //3.根據SVID開始寫檔
                                string smlFormat = tempReplySecsmsg.ToSml();

                                //4.編輯SML檔
                                string NewsmlFormat = ParseSVNRRToSML(svIDList, smlFormat);

                                //5.轉換為SECSMessage
                                tempReplySecsmsg = NewsmlFormat.ToSecsMessage();

                                e.ReplyAsync(tempReplySecsmsg);
                            }
                            else if (14 == replySecsmsg.F)//Establish Communications Request Acknowledge (CRA)
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                ReplyMDLNAndSOFTREV(tempReplySecsmsg, replySecsmsg, e);
                            }
                            else if (16 == replySecsmsg.F)  //OFF-LINE ACKNOWLEDGE(OFLA)
                            {
                                ChangeControlState(CONTROL_STATE.HOST_OFF_LINE, e, tempReplySecsmsg, ref lResult);
                            }
                            else if (18 == replySecsmsg.F)  //ON-LINE ACKNOWLEDGE(OFLA)
                            {
                                //先取出收到On command的預設值
                                _ecList.TryGetValue(ECName.GEM_ON_LINE_SUBSTATE, out SanwaEC sanwaEC);
                                if (sanwaEC == null)
                                {
                                    _logger.Error("GEM_ON_LINE_SUBSTATE NO DEFINED");
                                }
                                else
                                {
                                    //Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
                                    if ("4" == sanwaEC._defaultValue.ToString())
                                    {
                                        ChangeControlState(CONTROL_STATE.ON_LINE_LOCATE, e, tempReplySecsmsg, ref lResult);
                                    }
                                    else if ("5" == sanwaEC._defaultValue.ToString())
                                    {
                                        ChangeControlState(CONTROL_STATE.ON_LINE_REMOTE, e, tempReplySecsmsg, ref lResult);
                                    }
                                }
                            }
                            else
                            {
                                //Replay S9F5
                                //ReplyUnrecognizedFunctionType(e);
                                //由應用層回復
                                lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                            }
                        }
                        else if (2 == replySecsmsg.S)
                        {
                            if (IsOfflineState())
                            {
                                ReplyInOffLineState(e);
                                return lResult;
                            }
                            if (14 == replySecsmsg.F)
                            {
                                ReplyS2F14(e, receiveSecsmsg, tempReplySecsmsg);
                            }
                            else if (16 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.EAC_ACK };
                                ReceiveS2F15(e, ref ack);

                                ReplyACK(e, tempReplySecsmsg, ack);
                            }
                            else if (18 == replySecsmsg.F)
                            {
                                ReplyS2F18(e, tempReplySecsmsg);
                            }
                            else if (24 == replySecsmsg.F)  //Trace Initialize Acknowledge (TIA) 
                            {
                                byte[] ack = { SanwaACK.TIAACK_ACK };

                                SanwaTIS sanwaTIS = new SanwaTIS();

                                ReceiveS2F23(e, ref ack, ref sanwaTIS);


                                ReplyACK(e, tempReplySecsmsg, ack);

                                SanwaTISThread sanwaTISThread = new SanwaTISThread
                                {
                                    _sanwaTIS = sanwaTIS
                                };

                                ExecuteTraceData(sanwaTISThread);

                            }
                            else if (30 == replySecsmsg.F)  //Equipment Constant Namelist (ECN)
                            {
                                ReplyS2F30(e, receiveSecsmsg, tempReplySecsmsg);
                            }
                            else if (32 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.TIACK_ACK };
                                ReceiveS2F31(e, ref ack);
                                ReplyACK(e, tempReplySecsmsg, ack);
                            }
                            else
                            {
                                //Replay S9F5
                                ReplyUnrecognizedFunctionType(e);
                            }
                        }
                        else if (5 == replySecsmsg.S)
                        {
                            if (IsOfflineState())
                            {
                                ReplyInOffLineState(e);
                                return lResult;
                            }

                            if (4 == replySecsmsg.F) //Enable/Disable Alarm Acknowledge (EAA)
                            {
                                byte[] ack = { SanwaACK.ACKC5_ACK };
                                ReceiveS5F3(e, ref ack);
                                ReplyACK(e, tempReplySecsmsg, ack);
                            }
                            else if (6 == replySecsmsg.F)   //List Alarms Request (LAR)
                            {
                                ReplyS5F6(e, receiveSecsmsg, tempReplySecsmsg);
                            }
                        }
                        else if (10 == replySecsmsg.S)
                        {
                            if (IsOfflineState())
                            {
                                ReplyInOffLineState(e);
                                return lResult;
                            }

                            if (4 == replySecsmsg.F || 6 == replySecsmsg.F)    // Terminal Display, Single (VTN)
                            {
                                byte[] ack = { SanwaACK.ACKC10_ACK };
                                ReceiveS10F3(e, ref ack);
                                ReplyACK(e, tempReplySecsmsg, ack);

                                lResult = PROCESS_MSG_RESULT.ALREADY_REPLIED;
                            }

                        }
                        else
                        {
                            //Replay S9F3
                            ReplyUnrecognizedStreamType(e);
                        }
                    }
                    catch (Exception ex)
                    {
                        //無相關訊息
                        replySecsmsg = new SecsMessage(9, 7, "IDN",
                                         Item.B());
                        e.ReplyAsync(replySecsmsg);
                        lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;

                        _logger.Error("SanwaBaseExec_ProcessMessage" + ex.Message);
                    }

                    return lResult;
                });

            return task;
        }
        private string GetTypeStringValue(SecsFormat ecsFormat, object value)
        {
            string strRet = "";

            switch (ecsFormat)
            {
                case SecsFormat.JIS8:
                    strRet += "<J[0] " + value.ToString() + ">\r\n";
                    break;
                case SecsFormat.ASCII:
                    strRet += "<A[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.Boolean:
                    strRet += "<Boolean[0] " + ((bool)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.Binary:
                    strRet += "<B[0] ";// + ((byte)Obj._value).ToString() + ">\r\n";
                    foreach (byte bValue in (byte[])value)
                        strRet += "0x" + bValue.ToString("X2") + " ";

                    strRet += ">\r\n";
                    break;

                case SecsFormat.I1:
                    strRet += "<I1[0] " + ((sbyte)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.I2:
                    strRet += "<I2[0] " + ((short)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.I4:
                    strRet += "<I4[0] " + ((int)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.I8:
                    strRet += "<I8[0] " + ((long)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.F8:
                    strRet += "<F8[0] " + ((double)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.F4:
                    strRet += "<F4[0] " + ((float)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.U8:
                    strRet += "<U8[0] " + ((ulong)value).ToString() + ">\r\n";
                    break;

                case SecsFormat.U1:
                    strRet += "<U1[0] " + ((byte)value).ToString() + ">\r\n";
                    break;
                case SecsFormat.U2:
                    strRet += "<U2[0] " + ((ushort)value).ToString() + ">\r\n";
                    break;
                case SecsFormat.U4:
                    strRet += "<U4[0] " + ((uint)value).ToString() + ">\r\n";
                    break;
                default:
                    break;
            }

            return strRet;
        }
        private void CheckReceiveIDList<T>(SecsMessage receiveMsg, ref List<string> idlist, Dictionary<string, T> list)
        {
            idlist.Clear();
            if (receiveMsg.SecsItem.Count > 0)
            {
                //針對個別ID回覆
                //解析送來的訊息並回填相對應的結果(並將異常的SVID寫入)

                if (receiveMsg.SecsItem.Format == SecsFormat.List)
                {
                    for (int i = 0; i < receiveMsg.SecsItem.Count; i++)
                    {
                        Item iItems = receiveMsg.SecsItem.Items[i];

                        if (CheckFomart3x5x20(iItems))
                        {
                            SetItemToStringType(iItems, out string id);

                            idlist.Add(id);
                        }
                    }
                }
                else
                {
                    if (receiveMsg.SecsItem.Format == SecsFormat.I1)
                    {
                        sbyte[] array = new sbyte[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<sbyte>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.I2)
                    {
                        short[] array = new short[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<short>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.I4)
                    {
                        int[] array = new int[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<int>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.I8)
                    {
                        long[] array = new long[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<long>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U1)
                    {
                        byte[] array = new byte[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<byte>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U2)
                    {
                        ushort[] array = new ushort[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<ushort>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U4)
                    {
                        uint[] array = new uint[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<uint>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U8)
                    {
                        ulong[] array = new ulong[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<ulong>();
                        for (int i = 0; i < receiveMsg.SecsItem.Count; ++i)
                        {
                            idlist.Add(array[i].ToString());
                        }
                    }
                }

                
            }
            else
            {
                //全部回覆
                foreach (var obj in list)
                {
                    idlist.Add(obj.Key);
                }
            }
        }
        private string GetMessageName(string ReplyMessage)
        {
            string newReplyMsg = "";
            TextReader reader = new StringReader(ReplyMessage);

            string line;
            //寫入Tital
            while ((line = reader.ReadLine()) != null)
            {
                newReplyMsg = line;
                newReplyMsg += "\r\n";
                break;
            }

            reader.Close();

            return newReplyMsg;
        }
        public void ReplyACK(PrimaryMessageWrapper e, SecsMessage replyMsg, byte[] ack)
        {
            string str = replyMsg.ToSml();
            string newReplyMsg = "";

            TextReader reader = new StringReader(str);

            string line;

            //寫入Tital
            while ((line = reader.ReadLine()) != null)
            {
                newReplyMsg = line;
                newReplyMsg += "\r\n";
                break;
            }

            newReplyMsg += "< B[0] " + ack.ToHexString() + ">\r\n";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void CopyReceiveMsgToReply(ref SecsMessage receive, ref SecsMessage reply)
        {
            string name = reply.Name;
            reply = receive.ToSml().ToSecsMessage();
            reply.Name = name;
            reply.F = (byte)(receive.F + 1);
            reply.ReplyExpected = false;
        }
        private void ReplyMDLNAndSOFTREV(SecsMessage reply, SecsMessage replyfomat, PrimaryMessageWrapper e)
        {
            reply = replyfomat.ToSml().ToSecsMessage();
            foreach (var item in reply.SecsItem.Items)
            {
                if (item.Format == SecsFormat.List)
                {
                    RecursivelyFindMDLNAndSOFTREV(item);
                }
                else if (item.Format == SecsFormat.ASCII)
                {
                    if (item.GetString().Equals("MDLN"))
                    {
                        item.SetString(_mdln);
                    }
                    else if (item.GetString().Equals("SOFTREV"))
                    {
                        item.SetString(_softRev);
                    }
                }
            }
            e.ReplyAsync(reply);
        }
        private void RecursivelyFindMDLNAndSOFTREV(Item id)
        {
            foreach (var subItem in id.Items)
            {
                if (subItem.Format == SecsFormat.List)
                {
                    RecursivelyFindMDLNAndSOFTREV(subItem);
                }
                else if (subItem.Format == SecsFormat.ASCII)
                {
                    if (subItem.GetString().Equals("MDLN"))
                    {
                        subItem.SetString(_mdln);
                    }
                    else if (subItem.GetString().Equals("SOFTREV"))
                    {
                        subItem.SetString(_softRev);
                    }
                }
            }
        }
        private void SetItemToStringType(Item item, out string id)
        {
            id = "";
            switch (item.Format)
            {
                case SecsFormat.I1: id = item.GetValue<sbyte>().ToString(); break;
                case SecsFormat.I2: id = item.GetValue<short>().ToString(); break;
                case SecsFormat.I4: id = item.GetValue<int>().ToString(); break;
                case SecsFormat.I8: id = item.GetValue<long>().ToString(); break;
                case SecsFormat.U8: id = item.GetValue<ulong>().ToString(); break;
                case SecsFormat.U1: id = item.GetValue<byte>().ToString(); break;
                case SecsFormat.U2: id = item.GetValue<ushort>().ToString(); break;
                case SecsFormat.U4: id = item.GetValue<uint>().ToString(); break;
                case SecsFormat.ASCII: id = item.GetString(); break;
            }
        }
        private bool CheckFomart3x(Item item)
        {
            bool Ret = true;

            if (!(item.Format == SecsFormat.I8 ||   //format 30
                item.Format == SecsFormat.I1 ||     //format 31
                item.Format == SecsFormat.I2 ||     //format 32
                item.Format == SecsFormat.I4))      //format 34
            {
                Ret = false;
            }
            return Ret;
        }
        private bool CheckFomart5x(Item item)
        {
            bool Ret = true;

            if (!(item.Format == SecsFormat.U8 ||   //format 50
                item.Format == SecsFormat.U1 ||     //format 51
                item.Format == SecsFormat.U2 ||     //format 52
                item.Format == SecsFormat.U4))      //format 54
            {
                Ret = false;
            }
            return Ret;
        }
        private bool CheckFomart3x5x(Item item)
        {
            bool Ret = true;
            if (!(CheckFomart3x(item) || CheckFomart5x(item)))
                Ret = false;

            return Ret;
        }
        private bool CheckFomart3x5x20(Item item)
        {
            bool Ret = true;
            if (!(CheckFomart3x5x(item) || item.Format == SecsFormat.ASCII))
                Ret = false;

            return Ret;
        }
        private string GetDateTime()
        {
            string datetime = DateTime.Now.ToString("yyMMddhhmmss");

            _ecList.TryGetValue(ECName.GEM_TIME_FORMAT, out SanwaEC obj);

            switch (obj._defaultValue)
            {
                case 0: //12 - byte format
                    datetime = DateTime.Now.ToString("yyMMddhhmmss");
                    break;
                case 1: //16 - byte format
                    datetime = DateTime.Now.ToString("yyyyMMddhhmmsscc");
                    break;
                case 2: //Extended(max 32 byte)
                    datetime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.sTZD");
                    break;
            }

            return datetime;
        }
        public static string RecursivelySV(SanwaSV sanwaSV, string sendMessage)
        {
            switch (sanwaSV._format)
            {
                case SecsFormat.List:

                    Dictionary<string, SanwaSV> _svSubList = (Dictionary<string, SanwaSV>)sanwaSV._value;
                    sendMessage += "< L[" + _svSubList.Count.ToString() + "]\r\n";
                    foreach (var PairKey in _svSubList)
                        sendMessage = RecursivelySV(PairKey.Value, sendMessage);
                    sendMessage += ">";
                    break;
                case SecsFormat.JIS8:
                    sendMessage = sendMessage + "<J[0] " + sanwaSV._value.ToString() + ">\r\n";
                    break;
                case SecsFormat.ASCII:
                    sendMessage = sendMessage + "<A[0] " + sanwaSV._value.ToString() + ">\r\n";
                    break;

                case SecsFormat.Boolean:
                    sendMessage = sendMessage + "<Boolean[0] " + ((bool)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.Binary:
                    sendMessage += "<B[0] ";// + ((byte)Obj._value).ToString() + ">\r\n";
                    foreach (byte value in (byte[])sanwaSV._value)
                        sendMessage = sendMessage + "0x" + value.ToString("X2") + " ";

                    sendMessage += ">\r\n";
                    break;

                case SecsFormat.I1:
                    sendMessage = sendMessage + "<I1[0] " + ((sbyte)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.I2:
                    sendMessage = sendMessage + "<I2[0] " + ((short)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.I4:
                    sendMessage = sendMessage + "<I4[0] " + ((int)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.I8:
                    sendMessage = sendMessage + "<I8[0] " + ((long)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.F8:
                    sendMessage = sendMessage + "<F8[0] " + ((double)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.F4:
                    sendMessage = sendMessage + "<F4[0] " + ((float)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.U8:
                    sendMessage = sendMessage + "<U8[0] " + ((ulong)sanwaSV._value).ToString() + ">\r\n";
                    break;

                case SecsFormat.U1:
                    sendMessage = sendMessage + "<U1[0] " + ((byte)sanwaSV._value).ToString() + ">\r\n";
                    break;
                case SecsFormat.U2:
                    sendMessage = sendMessage + "<U2[0] " + ((ushort)sanwaSV._value).ToString() + ">\r\n";
                    break;
                case SecsFormat.U4:
                    sendMessage = sendMessage + "<U4[0] " + ((uint)sanwaSV._value).ToString() + ">\r\n";
                    break;

            }
            return sendMessage;
        }
    }
}

