using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<E87_HostCommand> S3F25InServiceEvent;
        public event EventHandler<E87_HostCommand> S3F25OutOfServiceEvent;
        public event EventHandler<E87_HostCommand> S3F25ReserveAtPortEvent;
        public event EventHandler<E87_HostCommand> S3F25CancelReserveAtPortEvent;
        public event EventHandler<E87_HostCommand> S3F25AutoModeEvent;
        public event EventHandler<E87_HostCommand> S3F25ManualModeEvent;

        public void ReplyS3F26(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,3   
            //    1. < PORTACTION > 
            //    2. < PTN > 
            //    3.L,m        
            //        1.L,2             
            //          1. < PARAMNAME1 > 
            //          2. < PARAMVAL1 >        
            //        .        
            //        .
            //        m.L,2             
            //          1. < PARAMNAMEm > 
            //          2. < PARAMVALm >
            Item PORTACTIONItem = e.Message.SecsItem.Items[0];
            Item PTNItem = e.Message.SecsItem.Items[1];

            switch(PORTACTIONItem.GetString().ToUpper())
            {
                case "CANCELRESERVEATPORT":
                    ReplyChangeReservePortStatus(e, receiveMsg, replyMsg, false);
                    break;

                case "RESERVEATPORT":
                    ReplyChangeReservePortStatus(e, receiveMsg, replyMsg, true);
                    break;

                case "AUTO":
                    ReplyChangeAccess(e, receiveMsg, replyMsg, true);
                    break;

                case "MANUAL":
                    ReplyChangeAccess(e, receiveMsg, replyMsg, false);
                    break;

                case "CHANGEACCESS":
                    ReplyChangeAccess(e, receiveMsg, replyMsg, true);
                    break;

                case "IN SERVICE":
                case "INSERVICE":
                    ReplyChangeServiceStatus(e, receiveMsg, replyMsg, true);
                    break;

                case "OUT OF SERVICE":
                case "OUTOFSERVICE":
                    ReplyChangeServiceStatus(e, receiveMsg, replyMsg, false);
                    break;

                case "CHANGESERVICESTATUS":
                    ReplyChangeServiceStatus(e, receiveMsg, replyMsg, true);
                    break;
            }
        }
        public void ReplyChangeServiceStatus(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool inService)
        {
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            Item PTNItem = e.Message.SecsItem.Items[1];
            Item ServiceStatusList;

            if (e.Message.SecsItem.Count == 3)  //CHANGESERVICESTATUS
            {
                ServiceStatusList = e.Message.SecsItem.Items[2];

                if(ServiceStatusList.Count > 0)
                {
                    //0 :out of service
                    //1 :In service

                    Item PARAMVALItem = ServiceStatusList.Items[1];
                    switch (PARAMVALItem.Format)
                    {
                        case SecsFormat.ASCII:
                            inService = PARAMVALItem.GetString() == "0" ? false : true;
                            break;
                        case SecsFormat.Binary:
                            byte[] paramval = PARAMVALItem.GetValues<byte>();
                            inService = paramval[0] == 0x00 ? false : true;
                            break;
                        case SecsFormat.Boolean:
                            inService = PARAMVALItem.GetValue<bool>();
                            break;
                        case SecsFormat.F4:
                            inService = PARAMVALItem.GetValue<float>() == 0.0f ? false : true;
                            break;
                        case SecsFormat.F8:
                            inService = PARAMVALItem.GetValue<double>() == 0.0d ? false : true;
                            break;
                        case SecsFormat.I1:
                            inService = PARAMVALItem.GetValue<byte>() == 0 ? false : true;
                            break;
                        case SecsFormat.I2:
                            inService = PARAMVALItem.GetValue<short>() == 0 ? false : true;
                            break;
                        case SecsFormat.I4:
                            inService = PARAMVALItem.GetValue<int>() == 0 ? false : true;
                            break;
                        case SecsFormat.I8:
                            inService = PARAMVALItem.GetValue<long>() == 0 ? false : true;
                            break;
                        case SecsFormat.U1:
                            inService = PARAMVALItem.GetValue<sbyte>() == 0 ? false : true;
                            break;
                        case SecsFormat.U2:
                            inService = PARAMVALItem.GetValue<ushort>() == 0 ? false : true;
                            break;
                        case SecsFormat.U4:
                            inService = PARAMVALItem.GetValue<uint>() == 0 ? false : true;
                            break;
                        case SecsFormat.U8:
                            inService = PARAMVALItem.GetValue<ulong>() == 0 ? false : true;
                            break;

                    }
                }

            }

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;

            newReplyMsg += "< L[2]\r\n";

            if(lpObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";

            }
            else
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";

                bool Return = true;


                if (inService)
                {
                    Return = ChangeLoadPortState(lpObj, E87_LPTS.IN_SERVICE);

                    if (Return)
                    {
                        E87_HostCommand hcObj = new E87_HostCommand
                        {
                            lpObj = lpObj,
                            Name = "INSERVICE",
                            MessageName = "S3F25"
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F25InServiceEvent?.Invoke(this, hcObj);
                        });
                    }
                }
                else
                {
                    Return = ChangeLoadPortState(lpObj, E87_LPTS.OUT_OF_SERVICE);

                    if (Return)
                    {
                        E87_HostCommand hcObj = new E87_HostCommand
                        {
                            lpObj = lpObj,
                            Name = "OUTOFSERVICE",
                            MessageName = "S3F25"
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F25OutOfServiceEvent?.Invoke(this, hcObj);
                        });
                    }
                }
                newReplyMsg += "<L[0]\r\n>\r\n";
            }


            newReplyMsg += ">\r\n";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        public void ReplyChangeAccess(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool autoMode)
        {
            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            Item PTNItem = e.Message.SecsItem.Items[1];
            Item ChangeAccessList;
            if (e.Message.SecsItem.Count == 3)  //CHANGESERVICESTATUS
            {
                ChangeAccessList = e.Message.SecsItem.Items[2];

                if(ChangeAccessList.Count > 0)
                {
                    //0 :Manual
                    //1 :Auto

                    Item PARAMVALItem = ChangeAccessList.Items[1];
                    switch (PARAMVALItem.Format)
                    {
                        case SecsFormat.ASCII:
                            autoMode = PARAMVALItem.GetString() == "0" ? false : true;
                            break;
                        case SecsFormat.Binary:
                            byte[] paramval = PARAMVALItem.GetValues<byte>();
                            autoMode = paramval[0] == 0x00 ? false : true;
                            break;
                        case SecsFormat.Boolean:
                            autoMode = PARAMVALItem.GetValue<bool>();
                            break;
                        case SecsFormat.F4:
                            autoMode = PARAMVALItem.GetValue<float>() == 0.0f ? false : true;
                            break;
                        case SecsFormat.F8:
                            autoMode = PARAMVALItem.GetValue<double>() == 0.0d ? false : true;
                            break;
                        case SecsFormat.I1:
                            autoMode = PARAMVALItem.GetValue<byte>() == 0 ? false : true;
                            break;
                        case SecsFormat.I2:
                            autoMode = PARAMVALItem.GetValue<short>() == 0 ? false : true;
                            break;
                        case SecsFormat.I4:
                            autoMode = PARAMVALItem.GetValue<int>() == 0 ? false : true;
                            break;
                        case SecsFormat.I8:
                            autoMode = PARAMVALItem.GetValue<long>() == 0 ? false : true;
                            break;
                        case SecsFormat.U1:
                            autoMode = PARAMVALItem.GetValue<sbyte>() == 0 ? false : true;
                            break;
                        case SecsFormat.U2:
                            autoMode = PARAMVALItem.GetValue<ushort>() == 0 ? false : true;
                            break;
                        case SecsFormat.U4:
                            autoMode = PARAMVALItem.GetValue<uint>() == 0 ? false : true;
                            break;
                        case SecsFormat.U8:
                            autoMode = PARAMVALItem.GetValue<ulong>() == 0 ? false : true;
                            break;
                    }
                }
            }

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;

            newReplyMsg += "< L[2]\r\n";

            if (lpObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";

            }
            else
            {
                E87RETURN_AM Return;

                if (autoMode)
                {
                    Return = ChangeAccessMode(lpObj, E87_AM.AUTO);
                }
                else
                {
                    Return = ChangeAccessMode(lpObj, E87_AM.MANUAL);
                }


                if (Return == E87RETURN_AM.SUCCESSFUL || Return == E87RETURN_AM.IN_ACCESS_MODE_ALREADY)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    if(Return == E87RETURN_AM.SUCCESSFUL)
                    {
                        if(autoMode)
                        {
                            E87_HostCommand hcObj = new E87_HostCommand
                            {
                                lpObj = lpObj,
                                Name = "AUTOMODE",
                                MessageName = "S3F25"
                            };
                            S3F25AutoModeEvent?.Invoke(this, hcObj);
                        }
                        else
                        {
                            E87_HostCommand hcObj = new E87_HostCommand
                            {
                                lpObj = lpObj,
                                Name = "MANUALMODE",
                                MessageName = "S3F25"
                            };
                            S3F25ManualModeEvent?.Invoke(this, hcObj);
                        }                        
                    }
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_OBJECT_IDENTIFIER_IN_USE.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                }
            }
            newReplyMsg += ">\r\n";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());

        }
        public void ReplyChangeReservePortStatus(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool reserveAtPort)
        {
            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            Item PTNItem = e.Message.SecsItem.Items[1];
            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;

            newReplyMsg += "< L[2]\r\n";

            if (lpObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                bool Return = true;
                if (reserveAtPort)
                {
                    Return = ChangeReserviceState(lpObj, E87_RS.RESERVED);
                }
                else
                {
                    Return = ChangeReserviceState(lpObj, E87_RS.NOT_RESERVED);
                }

                if(Return)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    if(reserveAtPort)
                    {
                        E87_HostCommand hcObj = new E87_HostCommand
                        {
                            lpObj = lpObj,
                            Name = "RESERVEATPORT",
                            MessageName = "S3F25"
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F25ReserveAtPortEvent?.Invoke(this, hcObj);
                        });
                    }
                    else
                    {
                        E87_HostCommand hcObj = new E87_HostCommand
                        {
                            lpObj = lpObj,
                            Name = "CANCELRESERVEATPORT",
                            MessageName = "S3F25"
                        };

                        ThreadPool.QueueUserWorkItem(callback =>
                        {
                            S3F25CancelReserveAtPortEvent?.Invoke(this, hcObj);
                        });
                    }
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_ALREADY_IN_USE.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
            }

            newReplyMsg += ">\r\n";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
    }
}
