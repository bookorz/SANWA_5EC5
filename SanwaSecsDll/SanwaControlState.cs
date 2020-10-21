using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<CONTROL_STATE> ChangeControlStateEvent;

        /// <summary>
        /// 在OFFLine的請況下收到Event
        /// </summary>
        public event EventHandler<PrimaryMessageWrapper> ReceiveInOffLineEvent;

        private void ReplyInOffLineState(PrimaryMessageWrapper e)
        {
            string SearchKey = "S" + e.Message.S.ToString() + "F0";
            _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj);

            if(smlObj != null)
            {
                SecsMessage replySecsmsg = new SecsMessage(e.Message.S, (byte)0, smlObj.MessageName);
                replySecsmsg.ReplyExpected = false;

                string newReplyMsg = GetMessageName(replySecsmsg.ToSml());

                string text = smlObj.Text;
                string line;
                using (StringReader reader = new StringReader(text))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        newReplyMsg += line;
                        newReplyMsg += "\r\n";
                    }
                }

                e.ReplyAsync(newReplyMsg.ToSecsMessage());
            }
            else
            {
                ThreadPool.QueueUserWorkItem(callback =>
                {
                    ReceiveInOffLineEvent?.Invoke(this, e);
                });
            }
        }
        private void ReplyOnLineState(PrimaryMessageWrapper e, SecsMessage secsMessage, byte[] ONLACK)
        {
            SecsMessage relymsg = new SecsMessage(secsMessage.S, secsMessage.F,
                                    secsMessage.Name,
                                    Item.B(ONLACK),
                                    false);

            e.ReplyAsync(relymsg);
        }
        private void ReplyOffLineState(PrimaryMessageWrapper e, SecsMessage secsMessage, byte[] OFLACK)
        {
            SecsMessage relymsg = new SecsMessage(secsMessage.S, secsMessage.F,
                                    secsMessage.Name,
                                    Item.B(OFLACK),
                                    false);

            e.ReplyAsync(relymsg);
        }
        public void ChangeControlState(CONTROL_STATE newState, PrimaryMessageWrapper e, SecsMessage secsmsg, ref PROCESS_MSG_RESULT lResult)
        {
            SanwaSV svObj;
            if (_currentState == CONTROL_STATE.EQUIPMENT_OFF_LINE ||
                _currentState == CONTROL_STATE.ON_LINE_LOCAL ||
                _currentState == CONTROL_STATE.ON_LINE_REMOTE)
            {
                //1:Eqp OffLine, 2:OnLine Local, 3:OnLine Remote
                _svList.TryGetValue(SVName.GEM_PREVIOUS_CONTROL_STATE, out svObj);

                if(svObj!= null)
                {
                    if (_currentState == CONTROL_STATE.EQUIPMENT_OFF_LINE) SetSV(SVName.GEM_PREVIOUS_CONTROL_STATE, 1);
                    if (_currentState == CONTROL_STATE.ON_LINE_LOCAL) SetSV(SVName.GEM_PREVIOUS_CONTROL_STATE, 2);
                    if (_currentState == CONTROL_STATE.ON_LINE_REMOTE) SetSV(SVName.GEM_PREVIOUS_CONTROL_STATE, 3);
                }
            }

            switch (_currentState)
            {
                case CONTROL_STATE.EQUIPMENT_OFF_LINE:
                    
                    if (e != null)  //是否由Host收到訊息
                    {
                        if (newState == CONTROL_STATE.HOST_OFF_LINE)
                        {
                            //已經是HOST_OFF_LINE
                            ReplyInOffLineState(e);
                        }
                        else if(newState == CONTROL_STATE.ON_LINE_LOCAL ||
                                newState == CONTROL_STATE.ON_LINE_REMOTE)
                        {
                            //拒絕連線
                            byte[] replybyte = { SanwaACK.ONLACK_NOT_ACCEPTED };
                            ReplyOnLineState(e, secsmsg, replybyte);
                        }

                        return;
                    }

                    //僅能接收嘗試連線(CONTROL_STATE.ATTEMPT_ON_LINE)
                    if (CONTROL_STATE.ATTEMPT_ON_LINE != newState) return;

                    _currentState = newState;
                    break;

                case CONTROL_STATE.ATTEMPT_ON_LINE:

                    if (e != null)
                    {
                        //已經是HOST_OFF_LINE，拒絕Host任何指令
                        ReplyInOffLineState(e);
                        return;
                    }

                    _currentState = newState;
                    break;

                case CONTROL_STATE.HOST_OFF_LINE:

                    //1.由EQP進入 => e == null
                    //2.由HOST進入=> e != null
                    if (e != null)
                    {
                        if (CONTROL_STATE.HOST_OFF_LINE == newState)
                        {
                            ReplyInOffLineState(e);
                            return;
                        }

                        lResult = PROCESS_MSG_RESULT.ALREADY_REPLIED;                           //需通知AP層
                        _currentState = newState;

                        //由外部參數決定EC為CONTROL_STATE.ON_LINE_LOCATE 或 CONTROL_STATE.ON_LINE_REMOTE
                        //接收連線要求
                        byte[] replybyte = { SanwaACK.ONLACK_ACCEPTED };
                        ReplyOnLineState(e, secsmsg, replybyte);
                    }
                    else
                    {
                        //僅能接收嘗試連線(CONTROL_STATE.EQUIPMENT_OFF_LINE)
                        if (CONTROL_STATE.EQUIPMENT_OFF_LINE != newState)    return;

                        _currentState = newState;
                    }
                    
                    break;

                case CONTROL_STATE.ON_LINE_LOCAL:

                    //1.由EQP進入 => e == null
                    //2.由HOST進入=> e != null
                    if (e != null)
                    {
                        _currentState = newState;

                        if (newState == CONTROL_STATE.HOST_OFF_LINE)
                        {
                            lResult = PROCESS_MSG_RESULT.ALREADY_REPLIED;
                            byte[] replybyte = { SanwaACK.OFLACK_ACK };
                            ReplyOnLineState(e, secsmsg, replybyte);
                        }
                        else if (newState == CONTROL_STATE.ON_LINE_LOCAL ||
                                newState == CONTROL_STATE.ON_LINE_REMOTE)
                        {

                            byte[] replybyte = { SanwaACK.ONLACK_ALREADY_ON_LINE };
                            ReplyOnLineState(e, secsmsg, replybyte);
                        }
                    }
                    else
                    {
                        if (!(CONTROL_STATE.EQUIPMENT_OFF_LINE == newState ||
                            CONTROL_STATE.ON_LINE_REMOTE == newState))
                        {
                            return;
                        }

                        _currentState = newState;
                    }


                    break;

                case CONTROL_STATE.ON_LINE_REMOTE:

                    if (e != null)
                    {
                        _currentState = newState;

                        if (newState == CONTROL_STATE.HOST_OFF_LINE)
                        {
                            lResult = PROCESS_MSG_RESULT.ALREADY_REPLIED;

                            byte[] replybyte = { SanwaACK.OFLACK_ACK };
                            ReplyOnLineState(e, secsmsg, replybyte);
                        }
                        else if (newState == CONTROL_STATE.ON_LINE_LOCAL ||
                                newState == CONTROL_STATE.ON_LINE_REMOTE)
                        {

                            byte[] replybyte = { SanwaACK.ONLACK_ALREADY_ON_LINE };
                            ReplyOnLineState(e, secsmsg, replybyte);
                        }
                    }
                    else
                    {
                        if (!(CONTROL_STATE.EQUIPMENT_OFF_LINE == newState ||
                            CONTROL_STATE.ON_LINE_LOCAL == newState))
                        {
                            return;
                        }

                        _currentState = newState;
                    }
                    break;
            }


            //_svList.TryGetValue(SVName.GEM_CONTROL_STATE, out svObj);
            //if (svObj != null)
            //    svObj._value = GetCurrentStateForSV();
            SetSV(SVName.GEM_CONTROL_STATE, GetCurrentStateForSV());


            //傳給使用端
            ThreadPool.QueueUserWorkItem(callback =>
            {
                ChangeControlStateEvent?.Invoke(this, _currentState);
            });
            
            
           
        }
    }
}
