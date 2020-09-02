using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net;
//using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using System.Collections;
//using System.Messaging;
//using Secs4Net;

namespace SawanSecsDll
{

    public class SanwaSecs
    {
        public SecsGem _secsGem;
        public int T3 { get { return _secsGem.T3; } set { _secsGem.T3 = value; } }
        public int T5 { get { return _secsGem.T5; } set { _secsGem.T5 = value; } }
        public int T6 { get { return _secsGem.T6; } set { _secsGem.T6 = value; } }
        public int T7 { get { return _secsGem.T7; } set { _secsGem.T7 = value; } }
        public int T8 { get { return _secsGem.T8; } set { _secsGem.T8 = value; } }

        //提供Interface給外部紀錄Log
        public ISecsGemLogger _logger;
        
        //轉換sml檔案
        public SecsMessageList _secsMessages;

        public SanwaBaseExec _baseExec;

        public string eqpSVFileName = null;
        public string eqpEventFileName = null;
        public string eqpECFileName = null;
        public string eqpAlarmFileName = null;

        //public Dictionary<string, SanwaSV> _svList = new Dictionary<string, SanwaSV>();

        public Dictionary<string, SanwaSV> _svList = new Dictionary<string, SanwaSV>();
        public Dictionary<string, SanwaEventID> _eventIDList = new Dictionary<string, SanwaEventID>();
        public Dictionary<string, SanwaEC> _ecList = new Dictionary<string, SanwaEC>();
        public Dictionary<string, SanwaAlarm> _alarmList = new Dictionary<string, SanwaAlarm>();
        public List<string> _terminalMSGList = new List<string>();
        //
        //public BindingList<PrimaryMessageWrapper> recvBuffer = new BindingList<PrimaryMessageWrapper>();

        //連線狀態變化
        public delegate void ConnectionStateChanged();
        public event ConnectionStateChanged ConnectionStateChangedEvent;
        public void ConnectionChanged(object sender, ConnectionState e)
        {
            ConnectionStateChangedEvent?.Invoke();
        }

        public event EventHandler<PrimaryMessageWrapper> PrimaryMessageReceivedEvent;
        public void OnPrimaryMessageReceivedCallback(PrimaryMessageWrapper e)
        {
            PrimaryMessageReceivedEvent?.Invoke(this, e);
        }

        public event EventHandler<CONTROL_STATE> ChangeControlStateEvent;

        //連線是否成功
        public bool IsConnected = false;

        //TCP/IP 相關參數
        public bool IsActiveMode { get; set; }
        public string IpAddress;
        public int Port;
        public int DecoderBufferSize;

        //Device ID
        public int DeviceId;

        //連線狀態
        public ConnectionState State { get { return _secsGem.State; }}
        //S1F1
        public string strMDLN {
                get =>_baseExec?._mdln;
                set { if(_baseExec != null) _baseExec._mdln = value; } }
        public string strSOFTREV
        {
            get => _baseExec?._softRev;
            set { if (_baseExec != null) _baseExec._softRev = value; }
        }
        public CONTROL_STATE _currentState {
            get { return _baseExec._currentState; }
            set { _baseExec._currentState = value; }
        }
        public int _dataID {
            get {return _baseExec == null ? 1 : _baseExec._dataID; }
            //get { return _baseExec._dataID; }
            set { if (_baseExec != null) _baseExec._dataID = value;}
        }
        //public string _terminalText = "Terminal Text!!!";
        public SanwaSecs()
        {
            _secsGem = null;
            IsActiveMode = true;
            IpAddress = "127.0.0.1";
            Port = 5000;
            DecoderBufferSize = 65535;
            DeviceId = 0;
            _logger = null;
            _secsMessages = null;
            _baseExec = null;

            //_baseExec = new SanwaBaseExec(_secsMessages);
        }
        public SanwaSecs(bool isActive, string ip, int port, 
            int receiveBufferSize = 0x4000, ISecsGemLogger logger = null, SecsMessageList secsMessages = null)
        {
            _secsGem = null;

            IsActiveMode = isActive;
            IpAddress = ip;
            Port = port;
            DecoderBufferSize = receiveBufferSize;
            DeviceId = 0;
            _logger = logger;
            _secsMessages = secsMessages;
            _baseExec = null;

            //_baseExec = new SanwaBaseExec(_secsMessages);
        }

        public void Initialize()
        {
            if (_baseExec == null)
            {
                _baseExec = new SanwaBaseExec(_secsMessages)
                {
                    _svList = _svList,
                    _ecList = _ecList,
                    _alarmList = _alarmList,
                    _terminalMSGList = _terminalMSGList,
                    _logger = _logger
                };

                _baseExec.ChangeControlStateEvent += ChangeControlStateEvent;
                LoadSVIDCSVFile(eqpSVFileName);
                LoadEventIDCSVFile(eqpEventFileName);
                LoadECCSVFile(eqpECFileName);
                LoadAlarmCSVFile(eqpAlarmFileName);
                _baseExec.Initialize();
            }
        }
        public void Connect()
        {
            _secsGem?.Dispose();

            _secsGem = new SecsGem(
                IsActiveMode,
                IPAddress.Parse(IpAddress),
                Port,
                DecoderBufferSize)
            { Logger = _logger, DeviceId = (ushort)DeviceId };

            _baseExec._secsGem = _secsGem;

            _secsGem.ConnectionChanged += ConnectionChanged;
            _secsGem.PrimaryMessageReceived += PrimaryMessageReceived;
            _secsGem.Start();

        }
        public void Disconnect()
        {

            _secsGem?.Dispose();
            _secsGem = null;

            IsConnected = false;
        }
        private bool CheckConnectState()
        {
            bool bRet = true;
            if (_secsGem.State != ConnectionState.Selected) bRet = false;

            

            if (!(_currentState == CONTROL_STATE.ON_LINE_LOCATE ||
                _currentState == CONTROL_STATE.ON_LINE_REMOTE))
                bRet = false;

            return bRet;
        }
        private async void PrimaryMessageReceived(object sender, PrimaryMessageWrapper e)
        {
            try
            {
                PROCESS_MSG_RESULT lResult = await _baseExec.ProcessMessage(e);

                if (lResult == PROCESS_MSG_RESULT.PROCESS_NOT_FINISH)
                {
                    OnPrimaryMessageReceivedCallback(e);
                }
                else if (lResult == PROCESS_MSG_RESULT.ALREADY_REPLIED)
                {
                    if (1== e.Message.S && 15 == e.Message.F) //Request OFF-LINE(ROFL) S,H->E,reply
                    {
                        await S6F11ChangeControlStateAsync(EventName.GEM_EQP_OFF_LINE);
                    }
                    else if(1 == e.Message.S && 17 == e.Message.F)  //Request ON-LINE(RONL) S,H->E,reply
                    {
                        if(CONTROL_STATE.ON_LINE_LOCATE == _currentState)
                            await S6F11ChangeControlStateAsync(EventName.GEM_CONTROL_STATE_LOCAL);
                        if (CONTROL_STATE.ON_LINE_REMOTE == _currentState)
                            await S6F11ChangeControlStateAsync(EventName.GEM_CONTROL_STATE_REMOTE);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Handle Primary SECS message Error", ex);
            }
        }
        public async Task ChangeToOffLineState()
        {
            if (_secsGem.State != ConnectionState.Selected)
                return;

            if (!(_currentState == CONTROL_STATE.ON_LINE_LOCATE ||
                _currentState == CONTROL_STATE.ON_LINE_REMOTE ||
                _currentState == CONTROL_STATE.HOST_OFF_LINE))
                return;
            
            if (_baseExec != null)
            {
                PROCESS_MSG_RESULT lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;     //搭配Function無實質意義
                SecsMessage s6f11format = _secsMessages[6, 11].FirstOrDefault();    //搭配Function無實質意義

                //切換狀態機
                _baseExec.ChangeControlState(CONTROL_STATE.EQUIPMENT_OFF_LINE,null, s6f11format,ref lResult);

                await S6F11ChangeControlStateAsync(EventName.GEM_EQP_OFF_LINE);

                SetEV(ECName.GEM_INIT_CONTROL_STATE, 1);
            }

        }
        public async Task RequestToOnLine()
        {
            if (_secsGem.State != ConnectionState.Selected)
                return;

            if (_currentState != CONTROL_STATE.EQUIPMENT_OFF_LINE)
                return;


            SecsMessage s1f1format = _secsMessages[1, 1].FirstOrDefault();
            PROCESS_MSG_RESULT lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;

            if (s1f1format == null)
            {
                _logger.Error("ChangeToOnLineState s1f1format error");
                return;
            }

            _baseExec.ChangeControlState(CONTROL_STATE.ATTEMPT_ON_LINE, null, s1f1format, ref lResult);

            var s1f1 = new SecsMessage(1,1, s1f1format.Name);
            var s1f2 = await _secsGem.SendAsync(s1f1);

            if (s1f2 == null)
            {
                _logger.Error("ChangeToOnLineState Timeout");
                return;
            }

            if (s1f2.F != 2)
            {
                lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;     //搭配Function無實質意義
                SecsMessage s6f11format = _secsMessages[6, 11].FirstOrDefault();    //搭配Function無實質意義

                if(s6f11format != null)
                { 
                    //切換狀態機
                    _baseExec.ChangeControlState(CONTROL_STATE.EQUIPMENT_OFF_LINE, null, s6f11format, ref lResult);

                    await S6F11ChangeControlStateAsync(EventName.GEM_EQP_OFF_LINE);
                }
                return;
            }
            else
            {
                _ecList.TryGetValue(ECName.GEM_ON_LINE_SUBSTATE, out SanwaEC sanwaEC);
                if (sanwaEC == null)
                {
                    _logger.Error("GEM_ON_LINE_SUBSTATE NO DEFINED");
                }
                else
                {
                    lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;     //搭配Function無實質意義
                    SecsMessage s6f11format = _secsMessages[6, 11].FirstOrDefault();    //搭配Function無實質意義

                    if (s6f11format != null)
                    {
                        //Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
                        if ("4" == sanwaEC._defaultValue.ToString())
                        {
                            _baseExec.ChangeControlState(CONTROL_STATE.ON_LINE_LOCATE, null, s6f11format, ref lResult);
                            await S6F11ChangeControlStateAsync(EventName.GEM_CONTROL_STATE_LOCAL);
                        }
                        else if ("5" == sanwaEC._defaultValue.ToString())
                        {
                            _baseExec.ChangeControlState(CONTROL_STATE.ON_LINE_REMOTE, null, s6f11format, ref lResult);
                            await S6F11ChangeControlStateAsync(EventName.GEM_CONTROL_STATE_REMOTE);
                        }


                        //await S6F11ChangeControlStateAsync(EventName.GEM_EQ_CONST_CHANGED);
                        SetEV(ECName.GEM_INIT_CONTROL_STATE, 2);
                    }
                }
            }
        }
        public async Task ChangeToOnLineLocateState()
        {
            if (_secsGem.State != ConnectionState.Selected)
                return;

            if (_currentState != CONTROL_STATE.ON_LINE_REMOTE)
                return;

            PROCESS_MSG_RESULT lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;     //搭配Function無實質意義
            SecsMessage s6f11format = _secsMessages[6, 11].FirstOrDefault();    //搭配Function無實質意義
            if (s6f11format != null)
            {
                _baseExec.ChangeControlState(CONTROL_STATE.ON_LINE_LOCATE, null, s6f11format, ref lResult);
                await S6F11ChangeControlStateAsync(EventName.GEM_CONTROL_STATE_LOCAL);

                //await S6F11ChangeControlStateAsync(EventName.GEM_EQ_CONST_CHANGED);
                SetEV(ECName.GEM_INIT_CONTROL_STATE, 2);
            }
            else
            {
                _logger.Error("S6F11_NO_FIND");
            }
        }
        public async Task ChangeToOnLineRemoteState()
        {
            if (_secsGem.State != ConnectionState.Selected)
                return;

            if (_currentState != CONTROL_STATE.ON_LINE_LOCATE)
                return;

            PROCESS_MSG_RESULT lResult = PROCESS_MSG_RESULT.PROCESS_FINISH;     //搭配Function無實質意義
            SecsMessage s6f11format = _secsMessages[6, 11].FirstOrDefault();    //搭配Function無實質意義
            if (s6f11format != null)
            {
                _baseExec.ChangeControlState(CONTROL_STATE.ON_LINE_REMOTE, null, s6f11format, ref lResult);
                await S6F11ChangeControlStateAsync(EventName.GEM_CONTROL_STATE_REMOTE);

                //await S6F11ChangeControlStateAsync(EventName.GEM_EQ_CONST_CHANGED);


                SetEV(ECName.GEM_INIT_CONTROL_STATE, 2);
            }
            else
            {
                _logger.Error("S6F11_NO_FIND");
            }
        }
        public async Task S1F1Async()
        {
            if (_baseExec == null) return;
            if (!CheckConnectState()) return;

            try
            {
                var s1f1 = _secsMessages[1,1].FirstOrDefault();
                var s1f2 = await _secsGem.SendAsync(s1f1);
            }
            catch (Exception ex)
            {
                _logger.Info("S1F1:" + ex.Message);
            }
        }
        public async Task S1F13Async()
        {
            if (_baseExec == null) return;
            if (!CheckConnectState()) return;

            try
            {
                SecsMessage s1f13, s1f13format;
                //根據SML的名稱(決定是Host或者是Client)
                s1f13format = IsActiveMode ? _secsMessages[1, 13, "CR"] : _secsMessages[1, 13, "CR_Host"];

                s1f13 = new SecsMessage(s1f13format.S, s1f13format.F, s1f13format.Name);
                if (IsActiveMode)
                {
                    s1f13 = s1f13format.ToSml().ToSecsMessage();

                    foreach (var id in s1f13.SecsItem.Items)
                    {
                        if (id.Format == SecsFormat.ASCII)
                        {
                            if (id.GetString().Equals("MDLN"))
                            {
                                id.SetString(strMDLN);
                            }
                            else if (id.GetString().Equals("SOFTREV"))
                            {
                                id.SetString(strSOFTREV);
                            }
                        }
                    }
                }

                SecsMessage s1f14 = await _secsGem.SendAsync(s1f13);
            }
            catch (Exception ex)
            {
                _logger.Info("S1F13:" + ex.Message);
            }
        }
        private async Task S6F11ChangeControlStateAsync(string eventName)
        {
            if (_baseExec == null) return;
            if (!CheckConnectState()) return;

            _eventIDList.TryGetValue(eventName, out SanwaEventID sanwaEvent);
            if (sanwaEvent == null) return;

            _ecList.TryGetValue(ECName.GEM_DATAID_FORMAT, out SanwaEC sanwaEC);
            if (sanwaEC == null) return;

            //Data 累加
            _dataID = _dataID + 1;


            SecsMessage s6f11format = _secsMessages[6, 11].FirstOrDefault();
            Item dataiditem = null;

            switch (sanwaEC._defaultValue.ToString())
            {
                case "1": dataiditem = Item.I1((sbyte)_dataID); break;
                case "2": dataiditem = Item.I2((short)_dataID); break;
                case "3": dataiditem = Item.I4((int)_dataID); break;
                case "4": dataiditem = Item.U1((byte)_dataID); break;
                case "5": dataiditem = Item.U2((ushort)_dataID); break;
                case "6": dataiditem = Item.U4((uint)_dataID); break;
            }

            var s6f11 = new SecsMessage(6, 11, s6f11format.Name,
                Item.L(
                    dataiditem,     //DataID
                    Item.U4(Convert.ToUInt32(sanwaEvent._id)),     //CEID
                    Item.L())
                    );

            //var s6f12 = await _secsGem.SendAsync(s6f11);
            _secsGem.SendAsync(s6f11);

        }
        public async Task S5F1SetAlarmReport(uint alarmID, bool set, int alarmMode = 0)
        {
            if (_baseExec == null)      return;
            if (!CheckConnectState())   return;

            _alarmList.TryGetValue(alarmID.ToString(), out SanwaAlarm sanwaAlarm);

            if (sanwaAlarm == null) return;
            if (sanwaAlarm._enabled == false) return;
            if (sanwaAlarm._set == set) return;
            if (alarmMode > 63) return;

            sanwaAlarm._set = set;


            //暫時不考慮其他設定型態
            Item setItem = null;
            int setvalue;
            setvalue = sanwaAlarm._set ? 0x80 : 0x00;
            setItem = sanwaAlarm._set ? Item.B((byte)(setvalue + alarmMode)) : Item.B((byte)(setvalue + alarmMode));

            Item alarmIDItem = null;
            _ecList.TryGetValue(ECName.SANWA_ALARMID_FORMAT, out SanwaEC sanwaEC);
            if (sanwaEC == null) return;

            switch (sanwaEC._defaultValue.ToString())
            {
                case "1": alarmIDItem = Item.I1(Convert.ToSByte(sanwaAlarm._id)); break;
                case "2": alarmIDItem = Item.I2(Convert.ToInt16(sanwaAlarm._id)); break;
                case "3": alarmIDItem = Item.I4(Convert.ToInt32(sanwaAlarm._id)); break;
                case "4": alarmIDItem = Item.I8(Convert.ToInt64(sanwaAlarm._id)); break;
                case "5": alarmIDItem = Item.U1(Convert.ToByte(sanwaAlarm._id)); break;
                case "6": alarmIDItem = Item.U2(Convert.ToUInt16(sanwaAlarm._id)); break;
                case "7": alarmIDItem = Item.U4(Convert.ToUInt32(sanwaAlarm._id)); break;
                case "8": alarmIDItem = Item.U8(Convert.ToUInt64(sanwaAlarm._id)); break;
            }

            SecsMessage s5f1format = _secsMessages[5, 1].FirstOrDefault();

            var s5f1 = new SecsMessage(5, 1, s5f1format.Name,
                Item.L(setItem,
                        alarmIDItem,
                        Item.A(sanwaAlarm._text)));

            //var s6f12 = await _secsGem.SendAsync(s6f11);
            _secsGem.SendAsync(s5f1);


        }
        public async Task S10F1SetTerminalMSG(string terminalText)
        {
            if (_baseExec == null) return;
            if (!CheckConnectState()) return;

            SecsMessage s10f1format = _secsMessages[10, 1].FirstOrDefault();

            var s10f1 = new SecsMessage(10, 1, s10f1format.Name,
                Item.L(Item.B(0x00),
                        Item.A(terminalText)));

            _secsGem.SendAsync(s10f1);

        }
        public async Task<SecsMessage> SetStreamFunction(SecsMessage secsMessage)
        {
            if (_secsGem.State != ConnectionState.Selected)
                return null;
            try
            {
                var reply = await _secsGem.SendAsync(secsMessage);
                return reply;
            }
            catch (Exception ex)
            {
                _logger.Error("SanwaSecs_SetStreamFunction:" + ex.Message);
                return null;
            }

        }
        private void LoadSVIDCSVFile(string filePath)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] raw = line.Split(',');

                    //0.Name
                    //1.ID
                    //2.Format
                    if (!line.Contains("//"))   //第一行不理會
                    {
                        if (_baseExec != null)
                        {
                            SanwaSV temp = new SanwaSV
                            {
                                _name = raw[0],
                                _id = raw[1],
                                _unit = raw[4],
                                _sVName = raw[6]
                            };
                            switch (raw[2].ToUpper())
                            {
                                case "LIST": temp._format = SecsFormat.List; break;
                                case "ASCII":   temp._format = SecsFormat.ASCII;    break;
                                case "JIS8":    temp._format = SecsFormat.JIS8; break;
                                case "BINARY":  temp._format = SecsFormat.Binary;   break;
                                case "Boolean": temp._format = SecsFormat.Boolean; break;
                                case "I1": temp._format = SecsFormat.I1; break;
                                case "I2": temp._format = SecsFormat.I2; break;
                                case "I4": temp._format = SecsFormat.I4; break;
                                case "I8": temp._format = SecsFormat.I8; break;
                                case "F8": temp._format = SecsFormat.F8; break;
                                case "F4": temp._format = SecsFormat.F4; break;
                                case "U8": temp._format = SecsFormat.U8; break;
                                case "U1": temp._format = SecsFormat.U1; break;
                                case "U2": temp._format = SecsFormat.U2; break;
                                case "U4": temp._format = SecsFormat.U4; break;
                            }

                            _svList.Add(temp._id, temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("SanwaSecs_LoadSVIDcsvFile:" + ex.Message);
                }
            }

            file.Close();
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
            } 
                         
            return obj;
        }
        public void GetSVData(string id, out SanwaSV SVData)
        {
            if (_baseExec != null)
            {
                _svList.TryGetValue(id, out SanwaSV obj);
                SVData = obj;
            }
            else
            {
                SVData = null;
            }
        }
        public SanwaEC SetEV(string id, object value)
        {
            GetEVData(id, out SanwaEC obj);
            if (obj != null)
            {
                switch (obj._type)
                {
                    case SecsFormat.ASCII: obj._value = value.ToString(); break;
                    case SecsFormat.Boolean: obj._value = Convert.ToBoolean(value); break;
                    case SecsFormat.Binary:
                        var enumerable = value as IEnumerable<byte>;
                        obj._value = enumerable;
                        break;
                    case SecsFormat.F4:
                        if(!(Convert.ToSingle(value) - Convert.ToSingle(obj._maxValue) > 0.0 ||
                            Convert.ToSingle(value) - Convert.ToSingle(obj._minValue) < 0.0))
                            obj._value = Convert.ToSingle(value);
                        break;
                    case SecsFormat.F8:
                        if (!(Convert.ToDouble(value) - Convert.ToDouble(obj._maxValue) > 0.0 ||
                            Convert.ToDouble(value) - Convert.ToDouble(obj._minValue) < 0.0))
                            obj._value = Convert.ToDouble(value);
                        break;
                    case SecsFormat.I1:
                        if (!(Convert.ToSByte(value) > Convert.ToSByte(obj._maxValue) ||
                            Convert.ToSByte(value) < Convert.ToSByte(obj._minValue) ))
                            obj._value = Convert.ToSByte(value);
                        break;
                    case SecsFormat.I2:
                        if (!(Convert.ToInt16(value) > Convert.ToInt16(obj._maxValue) ||
                            Convert.ToInt16(value) < Convert.ToInt16(obj._minValue)))
                            obj._value = Convert.ToInt16(value);
                        break;
                    case SecsFormat.I4:
                        if (!(Convert.ToInt32(value) > Convert.ToInt32(obj._maxValue) ||
                            Convert.ToInt32(value) < Convert.ToInt32(obj._minValue)))
                            obj._value = Convert.ToInt32(value);
                        break;
                    case SecsFormat.I8:
                        if (!(Convert.ToInt64(value) > Convert.ToInt64(obj._maxValue) ||
                            Convert.ToInt64(value) < Convert.ToInt64(obj._minValue)))
                            obj._value = Convert.ToInt64(value);
                        break;
                    case SecsFormat.JIS8: obj._value = value.ToString(); break;
                    case SecsFormat.U1:
                        if (!(Convert.ToByte(value) > Convert.ToByte(obj._maxValue) ||
                            Convert.ToByte(value) < Convert.ToByte(obj._minValue)))
                            obj._value = Convert.ToByte(value);
                        break;
                    case SecsFormat.U2:
                        if (!(Convert.ToUInt16(value) > Convert.ToUInt16(obj._maxValue) ||
                            Convert.ToUInt16(value) < Convert.ToUInt16(obj._minValue)))
                            obj._value = Convert.ToUInt16(value);
                        break;
                    case SecsFormat.U4:
                        if (!(Convert.ToUInt32(value) > Convert.ToUInt32(obj._maxValue) ||
                            Convert.ToUInt32(value) < Convert.ToUInt32(obj._minValue)))
                            obj._value = Convert.ToUInt32(value);
                        break;
                    case SecsFormat.U8:
                        if (!(Convert.ToUInt64(value) > Convert.ToUInt64(obj._maxValue) ||
                            Convert.ToUInt64(value) < Convert.ToUInt64(obj._minValue)))
                            obj._value = Convert.ToUInt64(value);
                        break;
                }

                //if(obj._value == value)
                    S6F11ChangeControlStateAsync(EventName.GEM_EQ_CONST_CHANGED);
            }

            return obj;
        }
        public void GetEVData(string id, out SanwaEC SVData)
        {
            if (_baseExec != null)
            {
                _ecList.TryGetValue(id, out SanwaEC obj);
                SVData = obj;
            }
            else
            {
                SVData = null;
            }
        }

        private void LoadEventIDCSVFile(string filePath)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] raw = line.Split(',');

                    //[0] _nameDefine;
                    //[1] _id;
                    //[2] _name;
                    if (!line.Contains("//"))   
                    {
                        if (_baseExec != null)
                        {
                            SanwaEventID temp = new SanwaEventID
                            {
                                _nameDefine = raw[0],
                                _id = raw[1],
                                _name = raw[2]
                            };
                            _eventIDList.Add(temp._nameDefine, temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("SanwaSecs_LoadEventIDCSVFile:" + ex.Message);
                }
            }

            file.Close();
        }
        private void LoadECCSVFile(string filePath)
        {
            string line;
            bool readfilerror = false;
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] raw = line.Split(',');


                    //[0]_nameDefine;
                    //[1]_id;
                    //[2]_type;
                    //[3]_minValue;
                    //[4]_maxValue;
                    //[5]_defaultValue;
                    //[6]_unit;
                    //[7]_name;
                    //[8]_remark;
                    if (!line.Contains("//"))
                    {
                        if (_baseExec != null)
                        {
                            SanwaEC temp = new SanwaEC
                            {
                                _nameDefine = raw[0],
                                _id = raw[1],
                                _unit = raw[6],
                                _name = raw[7],

                                _minValue = null,
                                _maxValue = null,
                                _defaultValue = null
                                 
                            };

                            switch (raw[2].ToUpper())
                            {
                                case "ASCII":
                                    temp._type = SecsFormat.ASCII;
                                    temp._minValue = raw[3];
                                    temp._maxValue = raw[4];
                                    temp._value = temp._defaultValue = raw[5];
                                    break;
                                case "B":
                                    temp._type = SecsFormat.Binary;

                                    string[] vector3 = raw[3].Split(' ');
                                    string[] vector4 = raw[4].Split(' ');
                                    string[] vector5 = raw[5].Split(' ');

                                    if (vector3.Length == vector4.Length &&
                                        vector3.Length == vector5.Length)
                                    {
                                        if (raw[3] != "")
                                        {
                                            byte[] minvalueVector = new byte[vector3.Length];
                                            for(int i = 0; i< minvalueVector.Length; i++)
                                            {
                                                minvalueVector[i] = Convert.ToByte(vector3[i]);
                                            }
                                            temp._value = temp._minValue = minvalueVector;

                                        }

                                        if(raw[4] != "")
                                        {
                                            byte[] maxvalueVector = new byte[vector4.Length];
                                            for (int i = 0; i < maxvalueVector.Length; i++)
                                            {
                                                maxvalueVector[i] = Convert.ToByte(vector4[i]);
                                            }
                                            temp._value = temp._maxValue = maxvalueVector;

                                        }

                                        if (raw[5] != "")
                                        {
                                            byte[] defaultvalueVector = new byte[vector5.Length];
                                            for (int i = 0; i < defaultvalueVector.Length; i++)
                                            {
                                                defaultvalueVector[i] = Convert.ToByte(vector5[i]);
                                            }
                                            temp._value = temp._defaultValue = defaultvalueVector;
                                        }
                                    }                                    
                                    break;

                                case "BOOLEAN": temp._type = SecsFormat.Boolean;
                                    temp._minValue = raw[3] == "0" ? false : true;
                                    temp._maxValue = raw[4] == "0" ? false : true;
                                    temp._value = temp._defaultValue = raw[5] == "0" ? false : true;
                                    break;

                                case "I1": temp._type = SecsFormat.I1;
                                    if(raw[3] != "")
                                        temp._minValue = Convert.ToSByte(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToSByte(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToSByte(raw[5]);
                                    break;

                                case "I2": temp._type = SecsFormat.I2;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToInt16(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToInt16(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToInt16(raw[5]);
                                    break;

                                case "I4": temp._type = SecsFormat.I4;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToInt32(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToInt32(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToInt32(raw[5]);

                                    break;

                                case "I8": temp._type = SecsFormat.I8;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToInt64(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToInt64(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToInt64(raw[5]);
                                    break;

                                case "F8": temp._type = SecsFormat.F8;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToDouble(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToDouble(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToDouble(raw[5]);                                        
                                    break;

                                case "F4": temp._type = SecsFormat.F4;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToSingle(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToSingle(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToSingle(raw[5]);
                                    break;

                                case "U8": temp._type = SecsFormat.U8;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToUInt64(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToUInt64(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToUInt64(raw[5]);
                                    break;

                                case "U1": temp._type = SecsFormat.U1;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToByte(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToByte(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToByte(raw[5]);
                                    break;

                                case "U2": temp._type = SecsFormat.U2;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToUInt16(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToUInt16(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToUInt16(raw[5]);
                                    break;

                                case "U4": temp._type = SecsFormat.U4;
                                    if (raw[3] != "")
                                        temp._minValue = Convert.ToUInt32(raw[3]);
                                    if (raw[4] != "")
                                        temp._maxValue = Convert.ToUInt32(raw[4]);
                                    if (raw[5] != "")
                                        temp._value = temp._defaultValue = Convert.ToUInt32(raw[5]);
                                    break;

                                default:
                                    readfilerror = true;
                                    break;
                            }



                            _ecList.Add(temp._id, temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("SanwaSecs_LoadECCSVFile:" + ex.Message);
                }

                if (readfilerror)
                {
                    _logger.Error("SanwaSecs_LoadECCSVFile:Format Error");
                }
            }

            file.Close();
        }
        private void LoadAlarmCSVFile(string filePath)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    string[] raw = line.Split(',');
                    //[0]_name;
                    //[1]_id;
                    //[2]_cd;
                    //[3]_enabled;
                    //[4]_text;
                    if (!line.Contains("//"))
                    {
                        if (_baseExec != null)
                        {
                            SanwaAlarm temp = new SanwaAlarm()
                            {
                                _name = raw[0],
                                _id = raw[1],
                                _cd = raw[2],
                                _text = raw[4],

                                _enabled = raw[3] == "1" ? true : false
                            };

                            _alarmList.Add(temp._id, temp);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("LoadAlarmCSVFile:" + ex.Message);
                }
            }

            file.Close();
        }
    }
}
