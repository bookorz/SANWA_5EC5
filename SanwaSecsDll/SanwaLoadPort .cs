using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SanwaSecsDll
{
    /// <summary>
    /// E87 Load Port Transfer State
    /// </summary>
    public enum E87_LPTS
    {
        NO_STATE = 0,
        OUT_OF_SERVICE = 1,
        IN_SERVICE = 2,
        TRANSFER_BLOCKED = 3,
        TRANSFER_READY = 4,
        READY_TO_LOAD = 5,
        READY_TO_UNLOAD = 6
    }
    /// <summary>
    /// Access Mode Transfer State
    /// </summary>
    public enum E87_AM
    {
        NO_STATE = 0,
        MANUAL = 1,
        AUTO = 2,
    }

    /// <summary>
    /// Reservation State Model
    /// </summary>
    public enum E87_RS
    {
        NO_STATE = 0,
        NOT_RESERVED = 1,
        RESERVED = 2,
    }
    public enum E87RETURN_AM
    {
        SUCCESSFUL = 0x00,
        IN_TRANSFER = 0x01,
        RESERVED = 0x02,
        IN_ACCESS_MODE_ALREADY = 0x03
    }

    public enum E87_ASSOCIATED
    {
        NO_STATE = 0,
        NOT_ASSOCIATION = 1,
        ASSOCIATION = 2,
    }

    public class LoadPort
    {
        public string Name { get; set; }
        public int Index;
        public byte Number;
        public E87_LPTS CurrentState = E87_LPTS.NO_STATE;
        public E87_LPTS PreviousState = E87_LPTS.NO_STATE;

        public SanwaCarrier Carrier;
        public E87_ASSOCIATED Associated = E87_ASSOCIATED.NOT_ASSOCIATION;
        

        public E87_AM AccessMode = E87_AM.NO_STATE;

        internal bool InTransfer = false;
        internal E87_RS Reserved = E87_RS.NOT_RESERVED;

        public bool IsInTransfer { get {  return InTransfer;  }}
        public E87_RS IsReserved { get { return Reserved; } }
    }

    public class LoadPortGroup
    {
        public string Name;
        public Dictionary<string, LoadPort> _loadPortList = new Dictionary<string, LoadPort>();
    }

    public class SanwaLoadPortACK
    {
        public byte PortNumber;
        public ulong ErrCode;
        public string ErrText = "";
    }

    public partial class SanwaBaseExec
    {
        //public event EventHandler<LoadPort> AccessModeFinishEvent;
        //public event EventHandler<LoadPort> ReservedFinishEvent;
        //public event EventHandler<LoadPort> AssociatedFinishEvent;

        private bool CheckLoadPortACK(byte accessmode, LoadPort loadObj, ref SanwaLoadPortACK lpACKObj)
        {
            E87RETURN_AM Return = E87RETURN_AM.SUCCESSFUL;
            bool allPTNSuccessful = true;
            if (accessmode == 0)    //Manual
            {
                Return = ChangeAccessMode(loadObj, E87_AM.MANUAL);
            }
            else                    //Auto
            {
                Return = ChangeAccessMode(loadObj, E87_AM.AUTO);
            }

            if (Return == E87RETURN_AM.SUCCESSFUL)
            {
                lpACKObj.PortNumber = loadObj.Number;
                lpACKObj.ErrCode = SanwaACK.ERRCODE_NO_ERROR;
            }
            else if (Return == E87RETURN_AM.IN_ACCESS_MODE_ALREADY)
            {
                lpACKObj.PortNumber = loadObj.Number;
                lpACKObj.ErrCode = SanwaACK.ERRCODE_NO_ERROR;
            }
            else
            {
                allPTNSuccessful = false;

                lpACKObj.PortNumber = loadObj.Number;
                lpACKObj.ErrCode = SanwaACK.ERRCODE_LOAD_PORT_ALREADY_IN_USE;
            }

            return allPTNSuccessful;
        }
        private void CheckLoadPortACK(bool GroupDefine, byte accessmode, LoadPort loadObj, 
                                    ref SanwaLoadPortACK lpACKObj,ref bool allPTNSuccessful)
        {
            if (!CheckLoadPortACK(accessmode, loadObj, ref lpACKObj))
            {
                allPTNSuccessful = false;
            }
            else
            {
                if (!GroupDefine)
                {
                    if(accessmode == 0x01)
                    {
                        E87_HostCommand Obj = new E87_HostCommand
                        {
                            Name = "AUTOMODE",
                            MessageName = "S3F27",
                            lpObj = loadObj
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F27AutoModeEvent?.Invoke(this, Obj);
                        });
                    }
                    else
                    {
                        E87_HostCommand Obj = new E87_HostCommand
                        {
                            Name = "MANUALMODE",
                            MessageName = "S3F27",
                            lpObj = loadObj
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F27ManualModeEvent?.Invoke(this, Obj);
                        });
                    }
                }
                else
                {
                    if (accessmode == 0x01)
                    {
                        E87_HostCommand Obj = new E87_HostCommand
                        {
                            Name = "AUTOMODE",
                            MessageName = "S3F21",
                            lpObj = loadObj
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F21AutoModeEvent?.Invoke(this, Obj);
                        });
                    }
                    else
                    {
                        E87_HostCommand Obj = new E87_HostCommand
                        {
                            Name = "MANUALMODE",
                            MessageName = "S3F21",
                            lpObj = loadObj
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F21ManualModeEvent?.Invoke(this, Obj);
                        });
                    }
                }
            }
        }
        private void ReplyAccessMode(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool GroupDefine)
        {
            Item PORTGRPNAMEItem = null;
            Item ACCESSMODEItem = null;
            Item PTNList = null;

            if (GroupDefine)
            {
                PORTGRPNAMEItem = e.Message.SecsItem.Items[0];
                ACCESSMODEItem = e.Message.SecsItem.Items[1];
                PTNList = e.Message.SecsItem.Items[2];
            }
            else
            {
                ACCESSMODEItem = e.Message.SecsItem.Items[0];
                PTNList = e.Message.SecsItem.Items[1];
            }

            if (GroupDefine)
            {
                string LPGroupName = PORTGRPNAMEItem.GetString();
                _loadPortGroupList.TryGetValue(LPGroupName, out LoadPortGroup LPGObj);

                if (LPGObj != null)
                    _loadPortGroupList.Remove(LPGroupName);

                if (PTNList.Count > 0)
                {
                    LoadPortGroup lpgObj = new LoadPortGroup
                    {
                        Name = LPGroupName
                    };

                    for (int i = 0; i < PTNList.Count; i++)
                    {
                        Item PTNItem = PTNList.Items[i];
                        byte ptn = PTNItem.GetValue<byte>();
                        LoadPort loadObj = _loadPortList.FirstOrDefault(x => x.Value.Number == ptn).Value;
                        if (loadObj != null)
                            lpgObj._loadPortList.Add(loadObj.Name, loadObj);
                    }

                    _loadPortGroupList.Add(LPGroupName, lpgObj);

                }
            }

            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            newReplyMsg += "< L[2]\r\n";

            Dictionary<string, SanwaLoadPortACK> LPACKList = new Dictionary<string, SanwaLoadPortACK>();
            if (ACCESSMODEItem.Format != SecsFormat.U1 || PTNList.Format != SecsFormat.List)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_INVALID_DATA.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";
            }
            else
            {
                byte accessmode = ACCESSMODEItem.GetValue<byte>();

                bool allPTNSuccessful = true;

                if (PTNList.Count > 0)
                {
                    for (int i = 0; i < PTNList.Count; i++)
                    {
                        Item PTNItem = PTNList.Items[i];
                        SanwaLoadPortACK lpACKObj = new SanwaLoadPortACK();
                        byte ptn = PTNItem.GetValue<byte>();
                        LoadPort loadObj = _loadPortList.FirstOrDefault(x => x.Value.Number == ptn).Value;

                        if (loadObj == null)
                        {
                            allPTNSuccessful = false;

                            lpACKObj.PortNumber = ptn;
                            lpACKObj.ErrCode = SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST;
                        }
                        else
                        {
                            CheckLoadPortACK(GroupDefine, accessmode, loadObj, ref lpACKObj, ref allPTNSuccessful);
                        }

                        LPACKList.Add(lpACKObj.PortNumber.ToString(), lpACKObj);
                    }
                }
                else
                {
                    foreach (var loadObj in _loadPortList.Values)
                    {
                        SanwaLoadPortACK lpACKObj = new SanwaLoadPortACK();

                        CheckLoadPortACK(GroupDefine, accessmode, loadObj, ref lpACKObj, ref allPTNSuccessful);

                        LPACKList.Add(lpACKObj.PortNumber.ToString(), lpACKObj);

                    }
                }

                if (allPTNSuccessful)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    foreach (var lpACKObj in LPACKList.Values)
                    {
                        newReplyMsg += GetToSMLItem(SecsFormat.U1, lpACKObj.ErrCode.ToString());
                        newReplyMsg += GetToSMLItem(SecsFormat.ASCII, lpACKObj.ErrText);
                    }
                }

                newReplyMsg += ">\r\n";
                e.ReplyAsync(newReplyMsg.ToSecsMessage());
            }

        }
        public bool ChangeLoadPortState(LoadPort lpObj, E87_LPTS newState)
        {
            bool Return = true;
            lpObj.InTransfer = true;

            if (lpObj.CurrentState != E87_LPTS.NO_STATE)
                if (newState == lpObj.CurrentState) Return = false;

            switch (lpObj.CurrentState)
            {
                case E87_LPTS.NO_STATE:
                    if (!(newState == E87_LPTS.IN_SERVICE ||
                        newState == E87_LPTS.OUT_OF_SERVICE ||
                        newState == E87_LPTS.TRANSFER_BLOCKED ||
                        newState == E87_LPTS.TRANSFER_READY))
                        Return = false;

                    break;

                case E87_LPTS.OUT_OF_SERVICE:
                    //任何狀態下都可以切換 OUT_OF_SERVICE

                    break;

                case E87_LPTS.IN_SERVICE:
                    if(!(newState == E87_LPTS.OUT_OF_SERVICE ||
                        newState == E87_LPTS.TRANSFER_BLOCKED ||
                        newState == E87_LPTS.TRANSFER_READY))
                        Return = false;

                    break;

                case E87_LPTS.TRANSFER_BLOCKED:
                    if (!(newState == E87_LPTS.TRANSFER_READY ||
                        newState == E87_LPTS.READY_TO_LOAD ||
                        newState == E87_LPTS.READY_TO_UNLOAD ||
                        newState == E87_LPTS.OUT_OF_SERVICE))

                        Return = false;

                    break;
                case E87_LPTS.TRANSFER_READY:
                    if (!(newState == E87_LPTS.READY_TO_LOAD ||
                        newState == E87_LPTS.READY_TO_UNLOAD ||
                        newState == E87_LPTS.OUT_OF_SERVICE))

                        Return = false;

                    break;

                case E87_LPTS.READY_TO_LOAD:
                    if (!(newState == E87_LPTS.TRANSFER_BLOCKED ||
                        newState == E87_LPTS.OUT_OF_SERVICE))
                        Return = false; 
                    break;

                case E87_LPTS.READY_TO_UNLOAD:
                    if (!(newState == E87_LPTS.TRANSFER_BLOCKED ||
                        newState == E87_LPTS.OUT_OF_SERVICE))
                        Return = false;
                    break;
            }

            if(Return)
            {
                lpObj.PreviousState = lpObj.CurrentState;
                lpObj.CurrentState = newState;
            }

            lpObj.InTransfer = false;
            return true;
        }
        public E87RETURN_AM ChangeAccessMode(LoadPort lpObj, E87_AM newAccessMode)
        {
            E87RETURN_AM Return = E87RETURN_AM.SUCCESSFUL;

            if (lpObj.IsInTransfer) Return = E87RETURN_AM.IN_TRANSFER;
            if (lpObj.IsReserved == E87_RS.RESERVED)    Return = E87RETURN_AM.RESERVED;
            if (newAccessMode == lpObj.AccessMode)      Return = E87RETURN_AM.IN_ACCESS_MODE_ALREADY;

            if (Return == E87RETURN_AM.SUCCESSFUL || Return == E87RETURN_AM.IN_ACCESS_MODE_ALREADY)
            {
                lpObj.AccessMode = newAccessMode;
                //AccessModeFinishEvent?.Invoke(this, lpObj);
            }
          
            return Return;
        }
        public bool ChangeReserviceState(LoadPort lpObj, E87_RS ReserviceState)
        {
            bool Return = true;

            if(ReserviceState == E87_RS.RESERVED && ReserviceState == lpObj.Reserved)
                Return = false;

            if (Return)
            {
                lpObj.Reserved = ReserviceState;
                //ReservedFinishEvent?.Invoke(this, lpObj);
            }

            return Return;
        }

        public void LoadPortCarrierAssociated(LoadPort lpObj, SanwaCarrier carrierObj, E87_ASSOCIATED associated)
        {
            if(associated == E87_ASSOCIATED.ASSOCIATION)
            {
                lpObj.Carrier = carrierObj;
                carrierObj.LoadPortObj = lpObj;
            }
            else
            {
                lpObj.Carrier = null;
                carrierObj.LoadPortObj = null;
            }


            lpObj.Associated = associated;
            carrierObj.Associated = associated;

            //AssociatedFinishEvent?.Invoke(this, lpObj);
        }
    }
}
