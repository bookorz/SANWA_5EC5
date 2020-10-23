using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows;



namespace SanwaSecsDll
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
        ON_LINE_LOCAL = 3,
        ON_LINE_REMOTE = 4
    }
    public class SecsMessageCommand
    {
        public string Name = null;
        public string MessageName = null;
    }

    public class E87_HostCommand
    {
        public string Name = null;
        public string MessageName = null;
        public LoadPort lpObj = null;
        public SanwaCarrier carrierObj = null;
    }

    public partial class SanwaBaseExec
    {
        public string _mdln;
        public string _softRev;
        public int _dataID;        //sent report

        public SecsGem _secsGem;

        public SanwaSMLManager _smlManager;

        //提供Interface給外部紀錄Log
        public ISecsGemLogger _logger;

        public CONTROL_STATE _currentState { get; set; }

        //public SecsMessageList _secsMessages;
        public Dictionary<string, SanwaSV> _svList;     //search by Name
        public Dictionary<string, SanwaSV> _svIDList;   //search by ID       
        public Dictionary<string, SanwaEC> _ecList;     //search by Name     
        public Dictionary<string, SanwaEC> _ecIDList;   //search by ID
        public Dictionary<string, SanwaDV> _dvList;
        public Dictionary<string, SanwaDV> _dvIDList;
        public Dictionary<string, SanwaAlarm> _alarmList;   //search by Name
        public Dictionary<string, SanwaAlarm> _alarmIDList; //search by ID
        public Dictionary<string, SanwaEvent> _eventList;   //search by Name
        public Dictionary<string, SanwaEvent> _eventIDList; //search by ID
        public Dictionary<string, SanwaRPTID> _reportList;
        public Dictionary<string, SanwaCarrier> _carrierList;

        public List<string> _terminalMSGList;

        public Dictionary<string, LoadPort> _loadPortList;
        public Dictionary<string, LoadPortGroup> _loadPortGroupList;

        //public Dictionary<string, SanwaStrFunSetting> _strFunList;


        private string _svFolderName;
        private string _ecFolderName;
        private string _dvFolderName;

        public SanwaBaseExec()
        {
            //_secsMessages = secsMessages;

            _mdln = "TestMDLN";
            _softRev = "Test_SOFTREV";

            _currentState = CONTROL_STATE.ON_LINE_LOCAL;

            _dataID = 0;

            _secsGem = null;

            string EnvironmentDirectory = Environment.CurrentDirectory;

            _svFolderName = EnvironmentDirectory + "\\Data_SV\\";
            _ecFolderName = EnvironmentDirectory + "\\Data_EC\\";
            _dvFolderName = EnvironmentDirectory + "\\Data_DV\\";
        }
        public void Initialize()
        {
            SanwaEC ecObj;
            _ecList.TryGetValue(ECName.GEM_INIT_CONTROL_STATE, out ecObj);
            if (ecObj != null)
            {
                if ("1" == ecObj._value.ToString())
                {
                    _ecList.TryGetValue(ECName.GEM_OFF_LINE_SUBSTATE, out ecObj);
                    if (ecObj != null)
                    {
                        /// 1:Eqp. OFF-line , 2:Attempt On-line , 3:Host Off-line
                        if ("1" == ecObj._value.ToString())
                        {
                            _currentState = CONTROL_STATE.EQUIPMENT_OFF_LINE;
                        }
                        else if ("2" == ecObj._value.ToString())
                        {
                            _currentState = CONTROL_STATE.ATTEMPT_ON_LINE;
                        }
                        else if ("3" == ecObj._value.ToString())
                        {
                            _currentState = CONTROL_STATE.HOST_OFF_LINE;
                        }
                    }
                }
                else
                {
                    _ecList.TryGetValue(ECName.GEM_ON_LINE_SUBSTATE, out ecObj);
                    if (ecObj != null)
                    {
                        //Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
                        if ("4" == ecObj._value.ToString())
                        {
                            _currentState = CONTROL_STATE.ON_LINE_LOCAL;
                        }
                        else if ("5" == ecObj._value.ToString())
                        {
                            _currentState = CONTROL_STATE.ON_LINE_REMOTE;
                        }
                    }
                }
            }

            //_currentLPState = E87_LPTS.IN_SERVICE;
            //_previousLPState = E87_LPTS.IN_SERVICE;
        }
        public SanwaSV SetSV(string id, object value)
        {
            GetSVData(id, out SanwaSV obj);

            if (obj != null)
            {
                switch (obj._format)
                {
                    case SecsFormat.ASCII: obj._value = value.ToString(); break;
                    case SecsFormat.Binary:
                        var enumerable = value as IEnumerable<byte>;
                        obj._value = enumerable;
                        break;
                    case SecsFormat.Boolean: obj._value = Convert.ToBoolean(value); break;
                    case SecsFormat.F4: obj._value = Convert.ToSingle(value); break;
                    case SecsFormat.F8: obj._value = Convert.ToDouble(value); break;
                    case SecsFormat.I1: obj._value = Convert.ToSByte(value); break;
                    case SecsFormat.I2: obj._value = Convert.ToInt16(value); break;
                    case SecsFormat.I4: obj._value = Convert.ToInt32(value); break;
                    case SecsFormat.I8: obj._value = Convert.ToInt64(value); break;
                    case SecsFormat.JIS8: obj._value = value.ToString(); break;
                    case SecsFormat.List: obj._value = (Dictionary<string, SanwaSV>)value; break;
                    case SecsFormat.U1: obj._value = Convert.ToByte(value); break;
                    case SecsFormat.U2: obj._value = Convert.ToUInt16(value); break;
                    case SecsFormat.U4: obj._value = Convert.ToUInt32(value); break;
                    case SecsFormat.U8: obj._value = Convert.ToUInt64(value); break;
                }

                if(!Directory.Exists(_svFolderName))
                {
                    Directory.CreateDirectory(_svFolderName);
                }

                if (obj._format != SecsFormat.List)
                {
                    StreamWriter sw = new StreamWriter(_svFolderName + obj._id.ToString()+ ".txt");

                    if (obj._format == SecsFormat.ASCII || obj._format == SecsFormat.JIS8)
                    {
                        sw.WriteLine(obj._value);
                    }
                    else if (obj._format == SecsFormat.Binary)
                    {
                        string str = "";
                        foreach (byte temp in (byte[])obj._value)
                            str = str + "0x" + temp.ToString("X2") + " ";

                        sw.WriteLine(str);
                    }
                    else
                    {
                        sw.WriteLine(obj._value.ToString());
                    }

                    //Close the file
                    sw.Close();
                }
                else
                {
                    string subFolder = obj._id.ToString();
                    subFolder = _svFolderName + subFolder + "\\";

                    RecursionWriteSVListToFile(subFolder, (Dictionary<string, SanwaSV>)obj._value);
                }

            }

            return obj;
        }
        private void RecursionWriteSVListToFile(string foldername, Dictionary<string, SanwaSV> SVList)
        {
            if (Directory.Exists(foldername))
            {
                DelectDir(foldername);
                //Directory.(foldername);
            }

            Directory.CreateDirectory(foldername);

            foreach (object SVobject in SVList.Values)
            {
                SanwaSV sanwaSV = (SanwaSV)SVobject;
                if (sanwaSV._format != SecsFormat.List)
                {
                    StreamWriter sw = new StreamWriter(foldername + sanwaSV._id.ToString()+ ".txt");

                    if (sanwaSV._format == SecsFormat.ASCII || sanwaSV._format == SecsFormat.JIS8)
                    {
                        sw.WriteLine(sanwaSV._value);
                    }
                    else if (sanwaSV._format == SecsFormat.Binary)
                    {
                        string str = "";
                        foreach (byte temp in (byte[])sanwaSV._value)
                            str = str + "0x" + temp.ToString("X2") + " ";

                        sw.WriteLine(str);
                    }
                    else
                    {
                        sw.WriteLine(sanwaSV._value.ToString());
                    }

                    //Close the file
                    sw.Close();
                }
                else
                {
                    string subFolder = sanwaSV._id.ToString();
                    subFolder = foldername + subFolder + "\\";

                    RecursionWriteSVListToFile(subFolder, (Dictionary<string, SanwaSV>)SVobject);
                }
            }
        }
        private void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public void GetSVData(string name, out SanwaSV SVData)
        {
            _svList.TryGetValue(name, out SanwaSV obj);
            SVData = obj;
        }
        public SanwaEC SetECByID(string id, object value)
        {
            _ecIDList.TryGetValue(id,out SanwaEC sanwaEC);

            SetEC(sanwaEC._name, value);

            return sanwaEC;

        }
        public SanwaEC SetEC(string name, object value)
        {
            GetECData(name, out SanwaEC obj);
            bool ecChanged = false;
            if (obj != null && value != null)
            {
                if(obj._value != null)
                    SetDV(DVName.GEM_PREVIOUS_EC_VALUE, obj._value);

                switch (obj._type)
                {
                    case SecsFormat.ASCII:
                        obj._value = value.ToString();
                        ecChanged = true;
                        break;
                    case SecsFormat.Boolean:
                        obj._value = Convert.ToBoolean(value);
                        ecChanged = true;
                        break;
                    case SecsFormat.Binary:
                        var enumerable = value as IEnumerable<byte>;
                        obj._value = enumerable;
                        ecChanged = true;
                        break;
                    case SecsFormat.F4:
                        if (!(Convert.ToSingle(value) - Convert.ToSingle(obj._maxValue) > 0.0 ||
                            Convert.ToSingle(value) - Convert.ToSingle(obj._minValue) < 0.0))
                        {
                            obj._value = Convert.ToSingle(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.F8:
                        if (!(Convert.ToDouble(value) - Convert.ToDouble(obj._maxValue) > 0.0 ||
                            Convert.ToDouble(value) - Convert.ToDouble(obj._minValue) < 0.0))
                        {
                            obj._value = Convert.ToDouble(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.I1:
                        if (!(Convert.ToSByte(value) > Convert.ToSByte(obj._maxValue) ||
                            Convert.ToSByte(value) < Convert.ToSByte(obj._minValue)))
                        {
                            obj._value = Convert.ToSByte(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.I2:
                        if (!(Convert.ToInt16(value) > Convert.ToInt16(obj._maxValue) ||
                            Convert.ToInt16(value) < Convert.ToInt16(obj._minValue)))
                        {
                            obj._value = Convert.ToInt16(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.I4:
                        if (!(Convert.ToInt32(value) > Convert.ToInt32(obj._maxValue) ||
                            Convert.ToInt32(value) < Convert.ToInt32(obj._minValue)))
                        {
                            obj._value = Convert.ToInt32(value);

                        }

                        break;
                    case SecsFormat.I8:
                        if (!(Convert.ToInt64(value) > Convert.ToInt64(obj._maxValue) ||
                            Convert.ToInt64(value) < Convert.ToInt64(obj._minValue)))
                        {
                            obj._value = Convert.ToInt64(value);
                            ecChanged = true;
                        }
                        break;
                    case SecsFormat.JIS8: obj._value = value.ToString(); break;
                    case SecsFormat.U1:
                        if (!(Convert.ToByte(value) > Convert.ToByte(obj._maxValue) ||
                            Convert.ToByte(value) < Convert.ToByte(obj._minValue)))
                        {
                            obj._value = Convert.ToByte(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.U2:
                        if (!(Convert.ToUInt16(value) > Convert.ToUInt16(obj._maxValue) ||
                            Convert.ToUInt16(value) < Convert.ToUInt16(obj._minValue)))
                        {
                            obj._value = Convert.ToUInt16(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.U4:
                        if (!(Convert.ToUInt32(value) > Convert.ToUInt32(obj._maxValue) ||
                            Convert.ToUInt32(value) < Convert.ToUInt32(obj._minValue)))
                        {
                            obj._value = Convert.ToUInt32(value);
                            ecChanged = true;
                        }

                        break;
                    case SecsFormat.U8:
                        if (!(Convert.ToUInt64(value) > Convert.ToUInt64(obj._maxValue) ||
                            Convert.ToUInt64(value) < Convert.ToUInt64(obj._minValue)))
                        {
                            obj._value = Convert.ToUInt64(value);
                            ecChanged = true;
                        }
                        break;
                }

                //指定值與設定值相同代表切換成功
                if (ecChanged)
                {  
                    if (!Directory.Exists(_ecFolderName))
                    {
                        Directory.CreateDirectory(_ecFolderName);
                    }

                    if (obj._type != SecsFormat.List)
                    {
                        StreamWriter sw = new StreamWriter(_ecFolderName + obj._id.ToString() + ".txt");

                        if (obj._type == SecsFormat.ASCII || obj._type == SecsFormat.JIS8)
                        {
                            sw.WriteLine(obj._value);
                        }
                        else if (obj._type == SecsFormat.Binary)
                        {
                            string str = "";
                            foreach (byte temp in (byte[])obj._value)
                                str = str + "0x" + temp.ToString("X2") + " ";

                            sw.WriteLine(str);
                        }
                        else
                        {
                            sw.WriteLine(obj._value.ToString());
                        }

                        //Close the file
                        sw.Close();
                    }
                    else
                    {
                        string subFolder = obj._id.ToString();
                        subFolder = _ecFolderName + subFolder + "\\";

                        RecursionWriteECListToFile(subFolder, (Dictionary<string, SanwaEC>)obj._value);
                    }

                    SendEventReportAsync(EventName.GEM_EQ_CONST_CHANGED, false);
                }
                    

                SetDV(DVName.GEM_DV_ECID_CHANGED, obj._id);
                SetDV(DVName.GEM_DV_EC_VALUE_CHANGED, obj._value);
            }

            return obj;
        }
        public void GetECData(string name, out SanwaEC ECData)
        {
            _ecList.TryGetValue(name, out ECData);
        }
        private void RecursionWriteECListToFile(string foldername, Dictionary<string, SanwaEC> ECList)
        {
            if (Directory.Exists(foldername))
            {
                DelectDir(foldername);
                //Directory.(foldername);
            }

            Directory.CreateDirectory(foldername);

            foreach (object ECobject in ECList.Values)
            {
                SanwaEC sanwaEC = (SanwaEC)ECobject;
                if (sanwaEC._type != SecsFormat.List)
                {
                    StreamWriter sw = new StreamWriter(foldername + sanwaEC._id.ToString() + ".txt");

                    if (sanwaEC._type == SecsFormat.ASCII || sanwaEC._type == SecsFormat.JIS8)
                    {
                        sw.WriteLine(sanwaEC._value);
                    }
                    else if (sanwaEC._type == SecsFormat.Binary)
                    {
                        string str = "";
                        foreach (byte temp in (byte[])sanwaEC._value)
                            str = str + "0x" + temp.ToString("X2") + " ";

                        sw.WriteLine(str);
                    }
                    else
                    {
                        sw.WriteLine(sanwaEC._value.ToString());
                    }

                    //Close the file
                    sw.Close();
                }
                else
                {
                    string subFolder = sanwaEC._id.ToString();
                    subFolder = foldername + subFolder + "\\";

                    RecursionWriteECListToFile(subFolder, (Dictionary<string, SanwaEC>)ECobject);
                }
            }
        }
        public SanwaDV SetDV(string name, object value)
        {
            GetDVData(name, out SanwaDV obj);

            if (obj != null && value != null)
            {
                switch (obj._format)
                {
                    case SecsFormat.ASCII: obj._value = value.ToString(); break;
                    case SecsFormat.Binary:
                        var enumerable = value as IEnumerable<byte>;
                        obj._value = enumerable;
                        break;
                    case SecsFormat.Boolean: obj._value = Convert.ToBoolean(value); break;
                    case SecsFormat.F4: obj._value = Convert.ToSingle(value); break;
                    case SecsFormat.F8: obj._value = Convert.ToDouble(value); break;
                    case SecsFormat.I1: obj._value = Convert.ToSByte(value); break;
                    case SecsFormat.I2: obj._value = Convert.ToInt16(value); break;
                    case SecsFormat.I4: obj._value = Convert.ToInt32(value); break;
                    case SecsFormat.I8: obj._value = Convert.ToInt64(value); break;
                    case SecsFormat.JIS8: obj._value = value.ToString(); break;
                    case SecsFormat.U1: obj._value = Convert.ToByte(value); break;
                    case SecsFormat.U2: obj._value = Convert.ToUInt16(value); break;
                    case SecsFormat.U4: obj._value = Convert.ToUInt32(value); break;
                    case SecsFormat.U8: obj._value = Convert.ToUInt64(value); break;
                    case SecsFormat.List: obj._value = (Dictionary<string, SanwaDV>)value; break;

                }

                if (!Directory.Exists(_dvFolderName))
                {
                    Directory.CreateDirectory(_dvFolderName);
                }

                StreamWriter sw = new StreamWriter(_dvFolderName + obj._id.ToString() + ".txt");

                if (obj._format == SecsFormat.ASCII || obj._format == SecsFormat.JIS8)
                {
                    sw.WriteLine(obj._value);
                }
                else if (obj._format == SecsFormat.Binary)
                {
                    string str = "";
                    foreach (byte temp in (byte[])obj._value)
                        str = str + "0x" + temp.ToString("X2") + " ";

                    sw.WriteLine(str);
                }
                else
                {
                    sw.WriteLine(obj._value.ToString());
                }

                //Close the file
                sw.Close();
            }
            else
            {
                string subFolder = obj._id.ToString();
                subFolder = _dvFolderName + subFolder + "\\";

                RecursionWriteDVListToFile(subFolder, (Dictionary<string, SanwaDV>)obj._value);
            }
            return obj;
        }
        private void RecursionWriteDVListToFile(string foldername, Dictionary<string, SanwaDV> DVList)
        {
            if (Directory.Exists(foldername))
            {
                DelectDir(foldername);
            }

            Directory.CreateDirectory(foldername);

            foreach (object DVobject in DVList.Values)
            {
                SanwaDV sanwaDV = (SanwaDV)DVobject;
                if (sanwaDV._format != SecsFormat.List)
                {
                    StreamWriter sw = new StreamWriter(foldername + sanwaDV._id.ToString() + ".txt");

                    if (sanwaDV._format == SecsFormat.ASCII || sanwaDV._format == SecsFormat.JIS8)
                    {
                        sw.WriteLine(sanwaDV._value);
                    }
                    else if (sanwaDV._format == SecsFormat.Binary)
                    {
                        string str = "";
                        foreach (byte temp in (byte[])sanwaDV._value)
                            str = str + "0x" + temp.ToString("X2") + " ";

                        sw.WriteLine(str);
                    }
                    else
                    {
                        sw.WriteLine(sanwaDV._value.ToString());
                    }

                    //Close the file
                    sw.Close();
                }
                else
                {
                    string subFolder = sanwaDV._id.ToString();
                    subFolder = foldername + subFolder + "\\";

                    RecursionWriteDVListToFile(subFolder, (Dictionary<string, SanwaDV>)DVobject);
                }
            }
        }
        public void GetDVData(string name, out SanwaDV DVData)
        {
            _dvList.TryGetValue(name, out DVData);
        }
        public async Task SendEventReportAsync(string eventName, bool annotated)
        {
            if (_secsGem.State != ConnectionState.Selected) return;

            if (!(_currentState == CONTROL_STATE.ON_LINE_LOCAL ||
                _currentState == CONTROL_STATE.ON_LINE_REMOTE)) return;

            _eventList.TryGetValue(eventName, out SanwaEvent sanwaEvent);
            if (sanwaEvent == null) return;
            if (!sanwaEvent._enabled) return;

            _ecList.TryGetValue(ECName.GEM_DATAID_FORMAT, out SanwaEC sanwaEC);
            if (sanwaEC == null) return;;

            SanwaSML sanwaSML;
            if (!annotated)
            {
                _smlManager._messageList.TryGetValue("S6F11", out sanwaSML);
            }
            else
            {
                _smlManager._messageList.TryGetValue("S6F13", out sanwaSML);
            }


            _svList.TryGetValue(SVName.GEM_PREVIOUS_CEID, out SanwaSV sanwaSV);
            if (sanwaSV != null)
                SetSV(SVName.GEM_PREVIOUS_CEID, sanwaSV._id);

            //Data 累加
            _dataID = _dataID + 1;

            SanwaEC ecObj;

            SecsMessage secsMessage;

            if (!annotated)
            {
                secsMessage = new SecsMessage(6, 11, sanwaSML.MessageName);
            }
            else
            {
                secsMessage = new SecsMessage(6, 13, sanwaSML.MessageName);
            }

            string ReplyMSG = GetMessageName(secsMessage.ToSml());

            ReplyMSG += "< L[3]\r\n";
            switch (sanwaEC._value.ToString())
            {
                case "1":
                    ReplyMSG += GetTypeStringValue(SecsFormat.I1, (sbyte)_dataID);
                    break;
                case "2":
                    ReplyMSG += GetTypeStringValue(SecsFormat.I2, (short)_dataID);
                    break;
                case "3":
                    ReplyMSG += GetTypeStringValue(SecsFormat.I4, (int)_dataID);
                    break;
                case "4":
                    ReplyMSG += GetTypeStringValue(SecsFormat.U1, (byte)_dataID);
                    break;
                case "5":
                    ReplyMSG += GetTypeStringValue(SecsFormat.U2, (ushort)_dataID);
                    break;
                case "6":
                    ReplyMSG += GetTypeStringValue(SecsFormat.U4, (uint)_dataID);
                    break;
            }

            //目前暫定所有的CEID為"U4"
            ReplyMSG += "<U4[0] " + sanwaEvent._id.ToString() + ">\r\n";
            ReplyMSG += GetEventReportSML(sanwaEvent, annotated);
            //RPTList end
            ReplyMSG += ">\r\n";


            SecsMessage replyMSG = ReplyMSG.ToSecsMessage();
            //GEM_WBIT_S6
            _ecList.TryGetValue(ECName.GEM_WBIT_S6, out ecObj);
            replyMSG.ReplyExpected = "1" == ecObj._value.ToString() ? true : false;

            _secsGem.SendAsync(replyMSG);
        }
        public void ReplyUnrecognizedStreamType(PrimaryMessageWrapper e)
        {
            //e.ReplyAsync(_secsMessages[9, 3].FirstOrDefault());
            SecsMessage replySecsmsg = new SecsMessage(e.Message.S, (byte)3, "Unrecognized Stream Type(USN)")
            {
                ReplyExpected = false
            };

            string newReplyMsg = GetMessageName(replySecsmsg.ToSml());

            e.ReplyAsync(newReplyMsg.ToSecsMessage());

        }
        public void ReplyUnrecognizedFunctionType(PrimaryMessageWrapper e)
        {
            SecsMessage replySecsmsg = new SecsMessage(e.Message.S, (byte)5, "Unrecognized Function Type(UFN)")
            {
                ReplyExpected = false
            };

            string newReplyMsg = GetMessageName(replySecsmsg.ToSml());

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        public void ReplyIllegalData(PrimaryMessageWrapper e)
        {
            SecsMessage replySecsmsg = new SecsMessage(e.Message.S, (byte)7, "Illegal Data (IDN)")
            {
                ReplyExpected = false
            };

            string newReplyMsg = GetMessageName(replySecsmsg.ToSml());

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
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
                        //replySecsmsg = _secsMessages[e.Message.S, (byte)(e.Message.F + 1)].FirstOrDefault();

                        if (receiveSecsmsg == null)
                        {
                            //無相關訊息
                            ReplyIllegalData(e);
                            lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;
                            return lResult;
                        }

                        string SearchKey = "S" + receiveSecsmsg.S.ToString() + "F" + ((int)receiveSecsmsg.F).ToString();
                        _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj);

                        if (smlObj == null)
                        {
                            lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                            return lResult;
                        }

                        SearchKey = "S" + receiveSecsmsg.S.ToString() + "F" + ((int)receiveSecsmsg.F + 1).ToString();
                        _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj2);
                        if (smlObj2 == null)
                        {
                            lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                            return lResult;
                        }

                        lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;

                        replySecsmsg = new SecsMessage(receiveSecsmsg.S, (byte)(receiveSecsmsg.F + 1), smlObj.MessageName);

                        if (replySecsmsg.F % 2 == 0)
                            replySecsmsg.ReplyExpected = false;

                        if (1 == replySecsmsg.S)
                        {
                            if (2 == replySecsmsg.F)
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                if(!ReplyS1F2(e, receiveSecsmsg, replySecsmsg))
                                {
                                    lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                                    return lResult;
                                }
                            }
                            else if (4 == replySecsmsg.F)//Selected Equipment Status Data (SSD) M,H<-E
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                ReplyS1F4(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if (12 == replySecsmsg.F)// Status Variable Namelist Reply(SVNRR) M,H<-E
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                ReplyS1F12(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if (14 == replySecsmsg.F)//Establish Communications Request Acknowledge (CRA)
                            {
                                if (IsOfflineState())
                                {
                                    ReplyInOffLineState(e);
                                    return lResult;
                                }

                                if (!ReplyS1F14(e, receiveSecsmsg, replySecsmsg))
                                {
                                    lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                                    return lResult;
                                }

                            }
                            else if (16 == replySecsmsg.F)  //OFF-LINE ACKNOWLEDGE(OFLA)
                            {
                                ChangeControlState(CONTROL_STATE.HOST_OFF_LINE, e, replySecsmsg, ref lResult);
                            }
                            else if (18 == replySecsmsg.F)  //ON-LINE ACKNOWLEDGE(OFLA)
                            {
                                //先取出收到On command的預設值
                                _ecList.TryGetValue(ECName.GEM_ON_LINE_SUBSTATE, out SanwaEC sanwaEC);
                                if (sanwaEC == null)
                                {
                                    ChangeControlState(CONTROL_STATE.ON_LINE_LOCAL, e, replySecsmsg, ref lResult);
                                }
                                else
                                {
                                    //Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
                                    if ("4" == sanwaEC._value.ToString())
                                    {
                                        ChangeControlState(CONTROL_STATE.ON_LINE_LOCAL, e, replySecsmsg, ref lResult);
                                    }
                                    else if ("5" == sanwaEC._value.ToString())
                                    {
                                        ChangeControlState(CONTROL_STATE.ON_LINE_REMOTE, e, replySecsmsg, ref lResult);
                                    }
                                }
                            }
                            else
                            {
                                //Replay S9F5
                                //基本上流程不會到這邊,
                                //如果Config檔沒有定義,
                                //就前面流程將通知AP端回覆
                                ReplyUnrecognizedFunctionType(e);
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
                                ReplyS2F14(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if (16 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.EAC_ACK };
                                ReceiveS2F15(e, ref ack);

                                ReplyACK(e, replySecsmsg, ack);
                            }
                            else if (18 == replySecsmsg.F)
                            {
                                ReplyS2F18(e, replySecsmsg);
                            }
                            else if (24 == replySecsmsg.F)  //Trace Initialize Acknowledge (TIA) 
                            {
                                byte[] ack = { SanwaACK.TIAACK_ACK };

                                SanwaTIS sanwaTIS = new SanwaTIS();

                                if(ReceiveS2F23(e, ref ack, ref sanwaTIS))
                                {
                                    ReplyACK(e, replySecsmsg, ack);

                                    SanwaTISThread sanwaTISThread = new SanwaTISThread
                                    {
                                        _sanwaTIS = sanwaTIS
                                    };

                                    ExecuteTraceData(sanwaTISThread);
                                }
                                else
                                {
                                    lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                                    return lResult;
                                }


                            }
                            else if (30 == replySecsmsg.F)  //Equipment Constant Namelist (ECN)
                            {
                                ReplyS2F30(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if (32 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.TIACK_ACK };
                                ReceiveS2F31(e, ref ack);
                                ReplyACK(e, replySecsmsg, ack);
                            }
                            else if (34 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.DRACK_ACK };
                                ReceiveS2F33(e, ref ack);
                                ReplyACK(e, replySecsmsg, ack);
                            }
                            else if (36 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.DRACK_ACK };
                                ReceiveS2F35(e, ref ack);
                                ReplyACK(e, replySecsmsg, ack);
                            }
                            else if (38 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.ERACK_ACK };
                                ReceiveS2F37(e, ref ack);
                                ReplyACK(e, replySecsmsg, ack);

                                SetEventEnabledForSV();
                            }
                            else
                            {
                                //Replay S9F5
                                //基本上流程不會到這邊,
                                //如果Config檔沒有定義,
                                //就前面流程將通知AP端回覆
                                ReplyUnrecognizedFunctionType(e);
                            }
                        }
                        else if (3 == replySecsmsg.S)
                        {
                            if (IsOfflineState())
                            {
                                ReplyInOffLineState(e);
                                return lResult;
                            }

                            if (18 == replySecsmsg.F)
                            {
                                ReplyS3F18(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if(20 == replySecsmsg.F)       // Cancel All Carrier Out Request /Cancel All Carrier Out Acknowledge
                            {
                                lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                                return lResult;
                            }
                            else if (22 == replySecsmsg.F)
                            {
                                ReplyS3F22(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if( 24 == replySecsmsg.F)      //S3,F23  Port Group Action Request /S3F24 Port Group Action Acknowledge
                            {
                                lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                                return lResult;
                            }
                            else if (26 == replySecsmsg.F)
                            {
                                ReplyS3F26(e, receiveSecsmsg, replySecsmsg);
                            }
                            else if(28 == replySecsmsg.F)
                            {
                                ReplyS3F28(e, receiveSecsmsg, replySecsmsg);
                            }
                            else
                            {
                                //Replay S9F5
                                //基本上流程不會到這邊,
                                //如果Config檔沒有定義,
                                //就前面流程將通知AP端回覆
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
                                if(ReceiveS5F3(e, ref ack))
                                {
                                    ReplyACK(e, replySecsmsg, ack);
                                    SetAlarmEnabledForSV();
                                }
                                else
                                {
                                    lResult = PROCESS_MSG_RESULT.PROCESS_NOT_FINISH;
                                    return lResult;
                                }
                            }
                            else if (6 == replySecsmsg.F)   //List Alarms Request (LAR)
                            {
                                ReplyS5F6(e, receiveSecsmsg, replySecsmsg);
                            }
                            else
                            {
                                //Replay S9F5
                                //基本上流程不會到這邊,
                                //如果Config檔沒有定義,
                                //就前面流程將通知AP端回覆
                                ReplyUnrecognizedFunctionType(e);
                            }
                        }
                        else if (6 == replySecsmsg.S)
                        {
                            if (IsOfflineState())
                            {
                                ReplyInOffLineState(e);
                                return lResult;
                            }

                            if (16 == replySecsmsg.F) //Enable/Disable Alarm Acknowledge (EAA)
                            {
                                ReplyEventReport(e, receiveSecsmsg, replySecsmsg, false);
                            }
                            else if (18 == replySecsmsg.F) //Annotated Event Report Data(AERD)
                            {
                                ReplyEventReport(e, receiveSecsmsg, replySecsmsg, true);
                            }
                            else if (20 == replySecsmsg.F) //Individual Report Data(IRD)
                            {
                                ReplyIndividualReport(e, receiveSecsmsg, replySecsmsg, false);
                            }
                            else if (22 == replySecsmsg.F) //Annotated Individual Report Data(IARD)
                            {
                                ReplyIndividualReport(e, receiveSecsmsg, replySecsmsg, true);
                            }
                            else
                            {
                                //Replay S9F5
                                //基本上流程不會到這邊,
                                //如果Config檔沒有定義,
                                //就前面流程將通知AP端回覆
                                ReplyUnrecognizedFunctionType(e);
                            }

                        }
                        else if (10 == replySecsmsg.S)
                        {
                            if (IsOfflineState())
                            {
                                ReplyInOffLineState(e);
                                return lResult;
                            }

                            if (4 == replySecsmsg.F)    // Terminal Display, Single (VTN)
                            {
                                byte[] ack = { SanwaACK.ACKC10_ACK };
                                ReceiveS10F3(e, ref ack);
                                ReplyACK(e, replySecsmsg, ack);
                            }
                            else if(6 == replySecsmsg.F)
                            {
                                byte[] ack = { SanwaACK.ACKC10_ACK };
                                ReceiveS10F5(e, ref ack);
                                ReplyACK(e, replySecsmsg, ack);
                            }
                            else
                            {
                                //Replay S9F5
                                //基本上流程不會到這邊,
                                //如果Config檔沒有定義,
                                //就前面流程將通知AP端回覆
                                ReplyUnrecognizedFunctionType(e);
                            }

                        }
                        else
                        {
                            //Replay S9F3
                            //基本上流程不會到這邊,
                            //如果Config檔沒有定義,
                            //就前面流程將通知AP端回覆
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
        public string GetToSMLItem(SecsFormat ecsFormat, string value)
        {
            string strRet = "";

            switch (ecsFormat)
            {
                case SecsFormat.JIS8:
                    strRet += "<J[0] " + value + ">\r\n";
                    break;
                case SecsFormat.ASCII:
                    strRet += "<A[0] " + value + ">\r\n";
                    break;

                case SecsFormat.Boolean:
                    strRet += "<Boolean[0] " + value + ">\r\n";
                    break;

                case SecsFormat.Binary:
                    strRet += "<B[0] " + value + ">\r\n";
                    break;

                case SecsFormat.I1:
                    strRet += "<I1[0] " + value + ">\r\n";
                    break;

                case SecsFormat.I2:
                    strRet += "<I2[0] " + value + ">\r\n";
                    break;

                case SecsFormat.I4:
                    strRet += "<I4[0] " + value + ">\r\n";
                    break;

                case SecsFormat.I8:
                    strRet += "<I8[0] " + value + ">\r\n";
                    break;

                case SecsFormat.F8:
                    strRet += "<F8[0] " + value + ">\r\n";
                    break;

                case SecsFormat.F4:
                    strRet += "<F4[0] " + value + ">\r\n";
                    break;

                case SecsFormat.U8:
                    strRet += "<U8[0] " + value + ">\r\n";
                    break;

                case SecsFormat.U1:
                    strRet += "<U1[0] " + value + ">\r\n";
                    break;
                case SecsFormat.U2:
                    strRet += "<U2[0] " + value + ">\r\n";
                    break;
                case SecsFormat.U4:
                    strRet += "<U4[0] " + value + ">\r\n";
                    break;
                default:
                    break;
            }

            return strRet;
        }
        public string GetTypeStringValue(SecsFormat ecsFormat, object value)
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
                    strRet += "<Boolean[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.Binary:
                    strRet += "<B[0] ";// + ((byte)Obj._value).ToString() + ">\r\n";
                    foreach (byte bValue in (byte[])value)
                        strRet += "0x" + bValue.ToString("X2") + " ";

                    strRet += ">\r\n";
                    break;

                case SecsFormat.I1:
                    strRet += "<I1[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.I2:
                    strRet += "<I2[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.I4:
                    strRet += "<I4[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.I8:
                    strRet += "<I8[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.F8:
                    strRet += "<F8[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.F4:
                    strRet += "<F4[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.U8:
                    strRet += "<U8[0] " + value.ToString() + ">\r\n";
                    break;

                case SecsFormat.U1:
                    strRet += "<U1[0] " + value.ToString() + ">\r\n";
                    break;
                case SecsFormat.U2:
                    strRet += "<U2[0] " + value.ToString() + ">\r\n";
                    break;
                case SecsFormat.U4:
                    strRet += "<U4[0] " + value.ToString() + ">\r\n";
                    break;
                default:
                    break;
            }

            return strRet;
        }
        //private void CheckReceiveIDList<T>(SecsMessage receiveMsg, ref List<string> idlist, Dictionary<string, T> list)
        private void CheckReceiveIDList<T>(SecsMessage receiveMsg, ref Dictionary<string, SecsFormat> idlist, Dictionary<string, T> list)
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

                            object tempList = list;

                            if ("SanwaSecsDll.SanwaSV"== typeof(T).FullName.ToString() ||
                                "SanwaSV" == typeof(T).Name.ToString())
                            {
                                Dictionary < string, SanwaSV>  tempIDList = (Dictionary<string, SanwaSV>)tempList;
                                tempIDList.TryGetValue(id, out SanwaSV svObj);

                                if (svObj != null)
                                {
                                    idlist.Add(svObj._id, iItems.Format);
                                }
                                else
                                {
                                    idlist.Add(id, iItems.Format);
                                }
                            }
                            else if("SanwaSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                                "SanwaEC" == typeof(T).Name.ToString())
                            {
                                Dictionary<string, SanwaEC> tempIDList = (Dictionary<string, SanwaEC>)tempList;

                                tempIDList.TryGetValue(id, out SanwaEC ecObj);

                                if (ecObj != null)
                                {
                                    idlist.Add(ecObj._id, iItems.Format);
                                }
                                else
                                {
                                    idlist.Add(id, iItems.Format);
                                }
                            }
                            else if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                                "SanwaAlarm" == typeof(T).Name.ToString())
                            {
                                Dictionary<string, SanwaAlarm> tempIDList = (Dictionary<string, SanwaAlarm>)tempList;

                                tempIDList.TryGetValue(id, out SanwaAlarm alarmObj);
                                if (alarmObj != null)
                                {
                                    idlist.Add(alarmObj._id, iItems.Format);
                                }
                                else
                                {
                                    idlist.Add(id, iItems.Format);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (receiveMsg.SecsItem.Format == SecsFormat.I1)
                    {
                        sbyte[] array = new sbyte[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<sbyte>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.I1);

                        if ("SanwaSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                             "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.I1);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.I1);

                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.I2)
                    {
                        short[] array = new short[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<short>();

                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.I2);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.I2);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.I2);

                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.I4)
                    {
                        int[] array = new int[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<int>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.I4);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.I4);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.I4);
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.I8)
                    {
                        long[] array = new long[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<long>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.I8);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.I8);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.I8);
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U1)
                    {
                        byte[] array = new byte[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<byte>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.U1);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.U1);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.U1);
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U2)
                    {
                        ushort[] array = new ushort[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<ushort>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.U2);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.U2);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.U2);
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U4)
                    {
                        uint[] array = new uint[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<uint>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.U4);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.U4);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.U4);
                    }
                    else if (receiveMsg.SecsItem.Format == SecsFormat.U8)
                    {
                        ulong[] array = new ulong[receiveMsg.SecsItem.Count];
                        array = receiveMsg.SecsItem.GetValues<ulong>();
                        if ("SanwaSecsDll.SanwaSV" == typeof(T).FullName.ToString() ||
                            "SanwaSV" == typeof(T).Name.ToString())
                            ParseSVIDInVector(ref idlist, array, SecsFormat.U8);

                        if ("SawanSecsDll.SanwaEC" == typeof(T).FullName.ToString() ||
                            "SanwaEC" == typeof(T).Name.ToString())
                            ParseECIDInVector(ref idlist, array, SecsFormat.U8);

                        if ("SanwaSecsDll.SanwaAlarm" == typeof(T).FullName.ToString() ||
                            "SanwaAlarm" == typeof(T).Name.ToString())
                            ParseAlarmIDInVector(ref idlist, array, SecsFormat.U8);
                    }
                }
            }
            else
            {
                //全部回覆
                foreach (var obj in list)
                {
                    idlist.Add(obj.Key, SecsFormat.U4);
                }
            }
        }
        private void ParseSVIDInVector<T>(ref Dictionary<string, SecsFormat> idlist, T[] array, SecsFormat secsFormat)
        {
            for (int i = 0; i < array.Length; i++)
            {
                _svIDList.TryGetValue(array[i].ToString(), out SanwaSV svObj);
                if (svObj != null)
                {
                    idlist.Add(svObj._id, secsFormat);
                }
                else
                {
                    idlist.Add(array[i].ToString(), secsFormat);
                }
            }
        }
        private void ParseECIDInVector<T>(ref Dictionary<string, SecsFormat> idlist, T[] array, SecsFormat secsFormat)
        {
            for (int i = 0; i < array.Length; i++)
            {
                _ecIDList.TryGetValue(array[i].ToString(), out SanwaEC ecObj);
                if (ecObj != null)
                {
                    idlist.Add(ecObj._id, secsFormat);
                }
                else
                {
                    idlist.Add(array[i].ToString(), secsFormat);
                }
            }
        }
        private void ParseAlarmIDInVector<T>(ref Dictionary<string, SecsFormat> idlist, T[] array, SecsFormat secsFormat)
        {
            for (int i = 0; i < array.Length; i++)
            {
                _alarmIDList.TryGetValue(array[i].ToString(), out SanwaAlarm alarmObj);
                if (alarmObj != null)
                {
                    idlist.Add(alarmObj._id, secsFormat);
                }
                else
                {
                    idlist.Add(array[i].ToString(), secsFormat);
                }
            }
        }
        public string GetMessageName(string Message)
        {
            string newReplyMsg = "";
            TextReader reader = new StringReader(Message);

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
        //private void RecursivelyFindMDLNAndSOFTREV(Item id)
        //{
        //    foreach (var subItem in id.Items)
        //    {
        //        if (subItem.Format == SecsFormat.List)
        //        {
        //            RecursivelyFindMDLNAndSOFTREV(subItem);
        //        }
        //        else if (subItem.Format == SecsFormat.ASCII)
        //        {
        //            if (subItem.GetString().Equals("MDLN"))
        //            {
        //                subItem.SetString(_mdln);
        //            }
        //            else if (subItem.GetString().Equals("SOFTREV"))
        //            {
        //                subItem.SetString(_softRev);
        //            }
        //        }
        //    }
        //}
        private void SetItemToStringType(Item item, out string id)
        {
            SetItemToStringType(item, out id, out SecsFormat secsFormat);
        }
        private void SetItemToStringType(Item item, out string id, out SecsFormat secsFormat)
        {
            id = "";
            secsFormat = item.Format;
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
        public bool CheckFomart20(Item item)
        {
            bool Ret = true;

            if (item.Format != SecsFormat.ASCII)      //format ASCII
            {
                Ret = false;
            }
            return Ret;
        }
        public bool CheckFomart3x(Item item)
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
        public bool CheckFomart5x(Item item)
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
        public bool CheckFomart3x5x(Item item)
        {
            bool Ret = true;
            if (!(CheckFomart3x(item) || CheckFomart5x(item)))
                Ret = false;

            return Ret;
        }
        public bool CheckFomart3x5x20(Item item)
        {
            bool Ret = true;
            if (!(CheckFomart3x5x(item) || item.Format == SecsFormat.ASCII))
                Ret = false;

            return Ret;
        }
        public object GetCurrentStateForSV()
        {
            object iRet = 1;
            //1:Eqp OffLine, 2:Attemp - OnLine, 3:Host Off-Line, 4:OnLine Local, 5:OnLine Remote
            switch (_currentState)
            {
                case CONTROL_STATE.EQUIPMENT_OFF_LINE:
                    iRet = 1;   break;
                case CONTROL_STATE.ATTEMPT_ON_LINE:
                    iRet = 2; break;
                case CONTROL_STATE.HOST_OFF_LINE:
                    iRet = 3; break;
                case CONTROL_STATE.ON_LINE_LOCAL:
                    iRet = 4; break;
                case CONTROL_STATE.ON_LINE_REMOTE:
                    iRet = 5; break;
            }
            return iRet;
        }
        public string GetDateTime()
        {
            string datetime = DateTime.Now.ToString("yyMMddhhmmss");

            _ecList.TryGetValue(ECName.GEM_TIME_FORMAT, out SanwaEC obj);

            switch (obj._value.ToString())
            {
                case "0": //12 - byte format
                    datetime = DateTime.Now.ToString("yyMMddhhmmss");
                    break;
                case "1": //16 - byte format
                    datetime = DateTime.Now.ToString("yyyyMMddhhmmssff");
                    break;
                case "2": //Extended(max 32 byte)
                    datetime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.sTZD");
                    break;
            }

            return datetime;
        }
        /// <summary>
        /// mode = 0 Check Enabled List
        /// mode = 1 Check set List
        /// </summary>
        /// <param name="mode"></param>
        private void SetListForSV(int mode)
        {
            SanwaSV svObj = null;
            if (mode == 0)  {_svList.TryGetValue(SVName.GEM_ALARM_ENABLED, out svObj);}
            else if(mode == 1) {_svList.TryGetValue(SVName.GEM_ALARM_SET, out svObj);}
            else if (mode == 2) {_svList.TryGetValue(SVName.GEM_EVENT_ENABLED, out svObj);}
                

            if (svObj != null)
            {
                if (svObj._format == SecsFormat.List)
                {
                    Dictionary<string, SanwaSV> svList = (Dictionary<string, SanwaSV>)svObj._value;

                    svList.Clear();

                    if (mode == 0 || mode == 1)
                    {
                        foreach (var alarmObj in _alarmList.Values)
                        {
                            if (alarmObj._enabled && mode == 0)
                            {
                                svObj = new SanwaSV
                                {
                                    _id = alarmObj._id,
                                    _format = SecsFormat.U4,  //暫時預設
                                    _value = Convert.ToUInt32(alarmObj._id)
                                };
                                svList.Add(svObj._id, svObj);
                            }
                            else if (alarmObj._set && mode == 1)
                            {
                                svObj = new SanwaSV
                                {
                                    _id = alarmObj._id,
                                    _format = SecsFormat.U4,  //暫時預設
                                    _value = Convert.ToUInt32(alarmObj._id)
                                };
                                svList.Add(svObj._id, svObj);
                            }
                        }
                    }
                    else if (mode == 2)
                    {
                        foreach (var eventObj in _eventList.Values)
                        {
                            if(eventObj._enabled && mode == 2)
                            {
                                svObj = new SanwaSV
                                {
                                    _id = eventObj._id,
                                    _format = SecsFormat.U4,  //暫時預設
                                    _value = Convert.ToUInt32(eventObj._id)
                                };
                                svList.Add(svObj._id, svObj);
                            }
                        }
                    }

                    if (mode == 0) { SetSV(SVName.GEM_ALARM_ENABLED, svList); }
                    else if (mode == 1) { SetSV(SVName.GEM_ALARM_SET, svList); }
                    else if (mode == 2) { SetSV(SVName.GEM_EVENT_ENABLED, svList); }

                }
            }
        }
        public void SetAlarmEnabledForSV()
        {
            SetListForSV(0);
        }
        public void SetAlarmSetForSV()
        {
            SetListForSV(1);
        }
        public void SetEventEnabledForSV()
        {
            SetListForSV(2);
        }
        public string RecursivelySV(SanwaSV sanwaSV, string sendMessage)
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
        public string GetEventReportSML(SanwaEvent sanwaEvent, bool annotated)
        {
            string ReplyMSG = "";
            ReplyMSG += "<L[" + sanwaEvent._rptidList.Count.ToString() + "]\r\n";
            foreach (string rptid in sanwaEvent._rptidList)
            {
                _reportList.TryGetValue(rptid, out SanwaRPTID sanwaRPTID);
                if (sanwaRPTID != null)
                {
                    ReplyMSG += GetToSMLItem(sanwaRPTID._format, sanwaRPTID._id);
                    ReplyMSG += GetReportVIDSML(sanwaRPTID, annotated);
                }
            }
            ReplyMSG += ">\r\n";

            return ReplyMSG;
        }
        public void ReplyEventReport(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool annotated)
        {
            Item CEIDItem = e.Message.SecsItem;
            SetItemToStringType(CEIDItem, out string ceid);

            //data id format
            _ecList.TryGetValue(ECName.GEM_DATAID_FORMAT, out SanwaEC sanwaEC);
            _dataID = _dataID + 1;

            _eventIDList.TryGetValue(ceid, out SanwaEvent sanwaEvent);

            string ReplyMSG = GetMessageName(replyMsg.ToSml());
            ReplyMSG += "<L[3]\r\n";

            switch (sanwaEC._value.ToString())
            {
                case "1":
                    ReplyMSG += GetTypeStringValue(SecsFormat.I1, (sbyte)_dataID);
                    break;
                case "2":
                    ReplyMSG += GetTypeStringValue(SecsFormat.I2, (short)_dataID);
                    break;
                case "3":
                    ReplyMSG += GetTypeStringValue(SecsFormat.I4, (int)_dataID);
                    break;
                case "4":
                    ReplyMSG += GetTypeStringValue(SecsFormat.U1, (byte)_dataID);
                    break;
                case "5":
                    ReplyMSG += GetTypeStringValue(SecsFormat.U2, (ushort)_dataID);
                    break;
                case "6":
                    ReplyMSG += GetTypeStringValue(SecsFormat.U4, (uint)_dataID);
                    break;
            }

            ReplyMSG += GetToSMLItem(CEIDItem.Format, ceid);

            if (sanwaEvent == null)
            {
                ReplyMSG += "<L[0]\r\n>\r\n";
            }
            else
            {
                ReplyMSG += GetEventReportSML(sanwaEvent, annotated);
            }

            ReplyMSG += ">\r\n";

            e.ReplyAsync(ReplyMSG.ToSecsMessage());
        }
        public void ReplyIndividualReport(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool annotated)
        {
            Item PRTIDItem = e.Message.SecsItem;
            SetItemToStringType(PRTIDItem, out string prtid);
            string ReplyMSG = GetMessageName(replyMsg.ToSml());
            _reportList.TryGetValue(prtid, out SanwaRPTID sanwaRPTID);

            if (sanwaRPTID == null)
            {
                ReplyMSG += "<L[0]\r\n>\r\n";
            }
            else
            {
                ReplyMSG += GetReportVIDSML(sanwaRPTID, annotated);
            }

            e.ReplyAsync(ReplyMSG.ToSecsMessage());
        }
        public string GetReportVIDSML(SanwaRPTID sanwaRPTID, bool annotated)
        {
            string ReplyMSG = "";
            ReplyMSG += "<L[" + sanwaRPTID._vidList.Count.ToString() + "]\r\n";
            foreach (var keyValuePair in sanwaRPTID._vidList)
            {
                SanwaVID sanwaVID = keyValuePair.Value;
                _svIDList.TryGetValue(sanwaVID._id, out SanwaSV svObj);
                if (svObj == null)
                {
                    _ecIDList.TryGetValue(sanwaVID._id, out SanwaEC ecObj);
                    if (ecObj == null)
                    {
                        _dvIDList.TryGetValue(sanwaVID._id, out SanwaDV dvObj);

                        if (dvObj != null)
                        {
                            if (annotated)
                            {
                                ReplyMSG += "<L[2]]\r\n";
                                ReplyMSG += GetToSMLItem(SecsFormat.U4, dvObj._id);     
                                ReplyMSG += GetTypeStringValue(dvObj._format, dvObj._value);
                                ReplyMSG += ">\r\n";
                            }
                            else
                            {
                                ReplyMSG += GetTypeStringValue(dvObj._format, dvObj._value);
                            }
                        }

                    }
                    else
                    {
                        if (annotated)
                        {
                            ReplyMSG += "<L[2]]\r\n";
                            ReplyMSG += GetToSMLItem(SecsFormat.U4, ecObj._id);
                            ReplyMSG += GetTypeStringValue(ecObj._type, ecObj._value);
                            ReplyMSG += ">\r\n";
                        }
                        else
                        {
                            ReplyMSG += GetTypeStringValue(ecObj._type, ecObj._value);
                        }
                    }
                }
                else
                {
                    if (annotated)
                    {
                        ReplyMSG += "<L[2]]\r\n";
                        ReplyMSG += GetToSMLItem(SecsFormat.U4 ,svObj._id);
                        ReplyMSG += GetTypeStringValue(svObj._format, svObj._value);
                        ReplyMSG += ">\r\n";
                    }
                    else
                    {
                        ReplyMSG += GetTypeStringValue(svObj._format, svObj._value);
                    }
                }
            }
            ReplyMSG += ">\r\n";

            return ReplyMSG;
        }
    }
}

