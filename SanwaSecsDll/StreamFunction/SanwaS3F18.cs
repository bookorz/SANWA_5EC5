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
        public event EventHandler<E87_HostCommand> S3F17BindEvent;
        public event EventHandler<E87_HostCommand> S3F17CancelBindEvent;
        public event EventHandler<E87_HostCommand> S3F17CarrierNotificationEvent;
        public event EventHandler<E87_HostCommand> S3F17CancelCarrierNotificationEvent;
        public event EventHandler<E87_HostCommand> S3F17ProceedWithCarrierEvent;
        public event EventHandler<E87_HostCommand> S3F17CancelCarrierEvent;
        public event EventHandler<E87_HostCommand> S3F17CancelCarrierAtPortEvent;
        public event EventHandler<E87_HostCommand> S3F17CarrierReCreateEvent;
        public event EventHandler<E87_HostCommand> S3F17CarrierReleaseEvent;

        /// <summary>
        ///  Internal Buffer Production Equipment Only
        /// </summary>
        public event EventHandler<E87_HostCommand> S3F17CarrierOutEvent;
        /// <summary>
        ///  Internal Buffer Production Equipment Only
        /// </summary>
        public event EventHandler<E87_HostCommand> S3F17CancelCarrierOutEvent;
        /// <summary>
        ///  Internal Buffer Production Equipment Only
        /// </summary>
        public event EventHandler<E87_HostCommand> S3F17CarrierInEvent;


        public void ReplyS3F18(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,5   
            //    1. < DATAID > 
            //    2. < CARRIERACTION > 
            //    3. < CARRIERID > 
            //    4. < PTN > 
            //    5.L,n n = number of carrier attributes        
            //        1.L,2             
            //            1. < CATTRID1 > 
            //            2. < CATTRDATA1 >        
            //            .        
            //            .
            //        n.L,2             
            //            1. < CATTRIDn > 
            //            2. < CATTRDATAn >
            Item CARRIERACTIONItem = e.Message.SecsItem.Items[1];

            //CARRIERACTION
            //Bind
            //CancelBind
            //CancelCarrier
            //CancelCarrierAtPort
            //CancelCarrierNotification
            //CarrierNotification
            //CarrierReCreate
            //ProceedWithCarrier
            //CarrierRelease

            switch(CARRIERACTIONItem.GetString().ToUpper())
            {
                case "BIND":
                    ReplyBindService(e, receiveMsg, replyMsg);
                    break;
                case "CANCELBIND":
                    ReplyCancelBindService(e, receiveMsg, replyMsg);
                    break;
                case "CANCELCARRIER":
                    ReplyCancelCarrierService(e, receiveMsg, replyMsg, false);
                    break;
                case "CANCELCARRIERATPORT":
                    ReplyCancelCarrierService(e, receiveMsg, replyMsg, true);
                    break;
                case "CANCELCARRIERNOTIFICATION":
                    ReplyCancelCarrierNotificationService(e, receiveMsg, replyMsg);
                    break;
                case "CARRIERNOTIFICATION":
                    ReplyCarrierNotificationService(e, receiveMsg, replyMsg);
                    break;
                case "CARRIERRECREATE":
                    ReplyCarrierReCreate(e, receiveMsg, replyMsg);
                    break;
                case "PROCEEDWITHCARRIER":
                    ReplyProceedWithCarrierService(e, receiveMsg, replyMsg);
                    break;
                case "CARRIERRELEASE":
                    ReplyCarrierRelease(e, receiveMsg, replyMsg);
                    break;
                case "CARRIEROUT":
                    ReplyCarrierOut(e, receiveMsg, replyMsg);
                    break;
                case "CANCELCARRIEROUT":
                    ReplyCancelCarrierOut(e, receiveMsg, replyMsg);
                    break;
                case "CARRIERIN":
                    ReplyCarrierIn(e, receiveMsg, replyMsg);
                    break;
            }
        }
        private async void ReplyBindService(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if(lpObj.IsReserved == E87_RS.RESERVED)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_ALREADY_IN_USE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if(carrierObj != null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_OBJECT_IDENTIFIER_IN_USE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                Item CattrList = e.Message.SecsItem.Items[4];
                ulong CheckCattrReturn = CheckCattrList(CattrList);

                if(CheckCattrReturn != SanwaACK.ERRCODE_NO_ERROR)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, CheckCattrReturn.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    SanwaCarrier newCarrierObj = new SanwaCarrier
                    {
                        ObjID = carrierID
                    };

                    SetCarrierAttribute(CattrList, ref newCarrierObj);

                    _carrierList.Add(newCarrierObj.ObjID, newCarrierObj);

                    //ChangeReserviceState(lpObj, E87_RS.RESERVED);

                    //LoadPortCarrierAssociated(lpObj, newCarrierObj, E87_ASSOCIATED.ASSOCIATION);

                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "BIND",
                        MessageName = "S3F17",
                        lpObj = lpObj,
                        carrierObj = newCarrierObj
                    };

                    ThreadPool.QueueUserWorkItem(callback =>
                    {
                        S3F17BindEvent?.Invoke(this, Obj);
                    });
                }
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCancelBindService(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                _carrierList.Remove(carrierObj.ObjID);

                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";

                E87_HostCommand Obj = new E87_HostCommand
                {
                    Name = "CancelBind",
                    MessageName = "S3F17",
                    lpObj = lpObj,
                    carrierObj = carrierObj
                };

                //S3F17CancelBindEvent?.Invoke(this, Obj);
                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S3F17CancelBindEvent?.Invoke(this, Obj);
                });

            } 

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCarrierNotificationService(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            string carrierID = e.Message.SecsItem.Items[2].GetString();
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (carrierObj != null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_OBJECT_IDENTIFIER_IN_USE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                Item CattrList = e.Message.SecsItem.Items[4];
                ulong CheckCattrReturn = CheckCattrList(CattrList);

                if (CheckCattrReturn != SanwaACK.ERRCODE_NO_ERROR)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, CheckCattrReturn.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    SanwaCarrier newCarrierObj = new SanwaCarrier
                    {
                        ObjID = carrierID
                    };

                    SetCarrierAttribute(CattrList, ref newCarrierObj);

                    _carrierList.Add(newCarrierObj.ObjID, newCarrierObj);

                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "CarrierNotification",
                        MessageName = "S3F17",
                        lpObj = null,
                        carrierObj = newCarrierObj
                    };

                    //S3F17CarrierNotificationEvent?.Invoke(this, Obj);
                    ThreadPool.QueueUserWorkItem(callback =>
                    {
                        S3F17CarrierNotificationEvent?.Invoke(this, Obj);
                    });
                }
            }
            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCancelCarrierNotificationService(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            string carrierID = e.Message.SecsItem.Items[2].GetString();

            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                _carrierList.Remove(carrierObj.ObjID);

                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";

                E87_HostCommand Obj = new E87_HostCommand
                {
                    Name = "CancelCarrierNotification",
                    MessageName = "S3F17",
                    lpObj = null,
                    carrierObj = carrierObj
                };

                //S3F17CancelCarrierNotificationEvent?.Invoke(this, Obj);
                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S3F17CancelCarrierNotificationEvent?.Invoke(this, Obj);
                });
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyProceedWithCarrierService(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                Item CattrList = e.Message.SecsItem.Items[4];
                ulong CheckCattrReturn = CheckCattrList(CattrList);

                if (CheckCattrReturn != SanwaACK.ERRCODE_NO_ERROR)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, CheckCattrReturn.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    SetCarrierAttribute(CattrList, ref carrierObj);

                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "ProceedWithCarrier",
                        MessageName = "S3F17",
                        lpObj = lpObj,
                        carrierObj = carrierObj
                    };

                    //S3F17ProceedWithCarrierEvent?.Invoke(this, Obj);
                    ThreadPool.QueueUserWorkItem(callback =>
                    {
                        S3F17ProceedWithCarrierEvent?.Invoke(this, Obj);
                    });
                }
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCancelCarrierService(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg, bool atPort)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            SanwaCarrier carrierObj = null;
            if (!atPort)
            {
                _carrierList.TryGetValue(carrierID, out carrierObj);
            }
            else
            {
                carrierObj = lpObj.Carrier;
            }

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if (carrierObj == null)
            {
                if (!atPort)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_MISSING_CARRIER.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
            }
            else if(lpObj.Carrier.ObjID != carrierID && !atPort)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_MISSING_CARRIER.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";

                if(!atPort)
                {
                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "CancelCarrier",
                        MessageName = "S3F17",
                        lpObj = lpObj,
                        carrierObj = carrierObj
                    };

                    S3F17CancelCarrierEvent.Invoke(this, Obj);
                }
                else
                {
                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "CancelCarrierAtPort",
                        MessageName = "S3F17",
                        lpObj = lpObj,
                        carrierObj = carrierObj
                    };

                    //S3F17CancelCarrierAtPortEvent?.Invoke(this, Obj);
                    ThreadPool.QueueUserWorkItem(callback =>
                    {
                        S3F17CancelCarrierAtPortEvent?.Invoke(this, Obj);
                    });
                }

            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCarrierReCreate(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            //else if (lpObj.IsReserved == E87_RS.RESERVED)
            //{
            //    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
            //    newReplyMsg += "<L[2]\r\n";
            //    newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_ALREADY_IN_USE.ToString());
            //    newReplyMsg += "<A[0]>\r\n";
            //    newReplyMsg += ">\r\n";
            //}
            else if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if (lpObj.CurrentState != E87_LPTS.READY_TO_UNLOAD)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_COMMAND_NOT_VALID_FOR_CURRENT_STATE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                Item CattrList = e.Message.SecsItem.Items[4];
                ulong CheckCattrReturn = CheckCattrList(CattrList);

                if (CheckCattrReturn != SanwaACK.ERRCODE_NO_ERROR)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, CheckCattrReturn.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    _carrierList.Remove(carrierID);

                    SanwaCarrier newCarrierObj = new SanwaCarrier
                    {
                        ObjID = carrierID
                    };

                    SetCarrierAttribute(CattrList, ref newCarrierObj);

                    _carrierList.Add(newCarrierObj.ObjID, newCarrierObj);

                    //ChangeReserviceState(lpObj, E87_RS.NOT_RESERVED);

                    //LoadPortCarrierAssociated(lpObj, newCarrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);

                    ////等效於Bind
                    //if (CattrList.Count > 0)
                    //{
                    //    ChangeReserviceState(lpObj, E87_RS.RESERVED);

                    //    LoadPortCarrierAssociated(lpObj, newCarrierObj, E87_ASSOCIATED.ASSOCIATION);
                    //}


                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "CarrierReCreate",
                        MessageName = "S3F17",
                        lpObj = lpObj,
                        carrierObj = newCarrierObj
                    };

                    //S3F17CarrierReCreateEvent?.Invoke(this, Obj);
                    ThreadPool.QueueUserWorkItem(callback =>
                    {
                        S3F17CarrierReCreateEvent?.Invoke(this, Obj);
                    });
                }
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCarrierRelease(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                Item CattrList = e.Message.SecsItem.Items[4];
                ulong CheckCattrReturn = CheckCattrList(CattrList);

                if (CheckCattrReturn != SanwaACK.ERRCODE_NO_ERROR)
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                    newReplyMsg += "<L[2]\r\n";
                    newReplyMsg += GetToSMLItem(SecsFormat.U1, CheckCattrReturn.ToString());
                    newReplyMsg += "<A[0]>\r\n";
                    newReplyMsg += ">\r\n";
                }
                else
                {
                    newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                    newReplyMsg += "<L[0]\r\n>\r\n";

                    SetCarrierAttribute(CattrList, ref carrierObj);

                    E87_HostCommand Obj = new E87_HostCommand
                    {
                        Name = "CarrierRelease",
                        MessageName = "S3F17",
                        lpObj = lpObj,
                        carrierObj = carrierObj
                    };

                    //S3F17CarrierReleaseEvent?.Invoke(this, Obj);
                    ThreadPool.QueueUserWorkItem(callback =>
                    {
                        S3F17CarrierReleaseEvent?.Invoke(this, Obj);
                    });
                }
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCarrierOut(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (lpObj == null)
            {
                //無對應Load Port
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_LOAD_PORT_DOES_NOT_EXIST.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";

                E87_HostCommand Obj = new E87_HostCommand
                {
                    Name = "CarrierOut",
                    MessageName = "S3F17",
                    lpObj = lpObj,
                    carrierObj = carrierObj
                };

                //S3F17CarrierOutEvent?.Invoke(this, Obj);
                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S3F17CarrierOutEvent?.Invoke(this, Obj);
                });
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCancelCarrierOut(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            //LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";

                E87_HostCommand Obj = new E87_HostCommand
                {
                    Name = "CancelCarrierOut",
                    MessageName = "S3F17",
                    lpObj = null,
                    carrierObj = carrierObj
                };

                //S3F17CancelCarrierOutEvent?.Invoke(this, Obj);
                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S3F17CancelCarrierOutEvent?.Invoke(this, Obj);
                });
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private void ReplyCarrierIn(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //    0. < DATAID > 
            //    1. < CARRIERACTION > 
            //    2. < CARRIERID > 
            //    3. < PTN > 
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            string carrierID = e.Message.SecsItem.Items[2].GetString();
            Item PTNItem = e.Message.SecsItem.Items[3];

            //LoadPort lpObj = _loadPortList.FirstOrDefault(x => x.Value.Number == PTNItem.GetValue<byte>()).Value;
            newReplyMsg += "< L[2]\r\n";

            _carrierList.TryGetValue(carrierID, out SanwaCarrier carrierObj);

            if (carrierObj == null)
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_CMD_PERFOEM_WITH_ERR.ToString() + ">\r\n";
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += GetToSMLItem(SecsFormat.U1, SanwaACK.ERRCODE_UNKNOWN_OBJECT_INSTANCE.ToString());
                newReplyMsg += "<A[0]>\r\n";
                newReplyMsg += ">\r\n";
            }
            else
            {
                newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
                newReplyMsg += "<L[0]\r\n>\r\n";

                E87_HostCommand Obj = new E87_HostCommand
                {
                    Name = "CarrierIn",
                    MessageName = "S3F17",
                    lpObj = null,
                    carrierObj = carrierObj
                };

                //S3F17CarrierInEvent?.Invoke(this, Obj);
                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S3F17CarrierInEvent?.Invoke(this, Obj);
                });
            }

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        private ulong CheckCattrList(Item CattrList)
        {
            ulong Return = SanwaACK.ERRCODE_NO_ERROR;
            //Capacity
            //ContentMap
            //SlotMap
            //SubstateCount
            //Usage

            for (int i = 0; i < CattrList.Count; i++)
            {
                Item Cattr = CattrList.Items[i];
                Item CATTRIDItem = Cattr.Items[0];
                Item CATTRDATAItem = Cattr.Items[1];

                switch (CATTRIDItem.GetString().ToUpper())
                {
                    case "CAPACITY":
                        if(CATTRDATAItem.Format != SecsFormat.U1)
                            Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                        break;

                    case "CONTENTMAP":
                        if(CATTRDATAItem.Format != SecsFormat.List)
                        {
                            Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                        }
                        else
                        {
                            for (int j = 0; j< CATTRDATAItem.Count; j++)
                            {
                                Item ContentMapList = CATTRDATAItem.Items[j];

                                Item LotID = ContentMapList.Items[0];
                                Item SubstLotID = ContentMapList.Items[1];

                                if(LotID.Format != SecsFormat.ASCII ||
                                    SubstLotID.Format != SecsFormat.ASCII)
                                {
                                    Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                                    break;
                                }
                            }                            
                        }

                        break;

                    case "SLOTMAP":
                        if (CATTRDATAItem.Format != SecsFormat.List)
                        {
                            Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                        }
                        else
                        {
                            for (int j = 0; j < CATTRDATAItem.Count; j++)
                            {
                                Item SlotList = CATTRDATAItem.Items[j];

                                if (SlotList.Format != SecsFormat.U1)
                                {
                                    Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                                    break;
                                }
                            }
                        }
                        break;

                    case "SUBSTATECOUNT":
                        if (CATTRDATAItem.Format != SecsFormat.U1)
                            Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                        break;

                    case "USAGE":
                        if (CATTRDATAItem.Format != SecsFormat.ASCII)
                            Return = SanwaACK.ERRCODE_INVALID_ATTRIBUTE_VALUE;
                        break;

                    default:

                        Return = SanwaACK.ERRCODE_UNKNOWN_ATTRIBUTE_NAME;
                        break;
                }

                if (Return != SanwaACK.ERRCODE_NO_ERROR) break;
            }
            return Return;
        }
        private void SetCarrierAttribute(Item CattrList, ref SanwaCarrier carrierObj)
        {
            for (int i = 0; i < CattrList.Count; i++)
            {
                Item Cattr = CattrList.Items[i];
                Item CATTRIDItem = Cattr.Items[0];
                Item CATTRDATAItem = Cattr.Items[1];

                switch (CATTRIDItem.GetString().ToUpper())
                {
                    case "CAPACITY":
                        carrierObj.Capacity = CATTRDATAItem.GetValue<byte>();
                        break;

                    case "CONTENTMAP":

                        if(carrierObj.ContentMap.Count > 0)
                            carrierObj.ContentMap.Clear();

                        for (int j = 0; j < CATTRDATAItem.Count; j++)
                        {
                            Item ContentMapList = CATTRDATAItem.Items[j];

                            Item LotID = ContentMapList.Items[0];
                            Item SubstLotID = ContentMapList.Items[1];

                            sContentMap cmObj = new sContentMap
                            {
                                LotID = LotID.GetString(),
                                SubstrateID = SubstLotID.GetString()
                            };

                            carrierObj.ContentMap.Add(cmObj.SubstrateID, cmObj);
                        }

                        break;

                    case "SLOTMAP":
                        if (carrierObj.SlotMap.Count > 0)
                            carrierObj.SlotMap.Clear();

                        for (int j = 0; j < CATTRDATAItem.Count; j++)
                        {
                            Item ContentMapList = CATTRDATAItem.Items[j];
                            carrierObj.SlotMap.Add((eSlotMap)ContentMapList.GetValue<byte>());
                        }

                        break;

                    case "SUBSTATECOUNT":
                        carrierObj.SubstrateCount = CATTRDATAItem.GetValue<byte>();
                        break;

                    case "USAGE":
                        carrierObj.Usage = CATTRDATAItem.GetString();
                        break;
                }
            }
        }
    }
}
