using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<CONTROL_STATE> ChangeControlStateEvent;
        private void ReplyInOffLineState(PrimaryMessageWrapper e)
        {
            e.ReplyAsync(_secsMessages[e.Message.S, 0].FirstOrDefault());
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
                        else if(newState == CONTROL_STATE.ON_LINE_LOCATE ||
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

                case CONTROL_STATE.ON_LINE_LOCATE:

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
                        else if (newState == CONTROL_STATE.ON_LINE_LOCATE ||
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
                        else if (newState == CONTROL_STATE.ON_LINE_LOCATE ||
                                newState == CONTROL_STATE.ON_LINE_REMOTE)
                        {

                            byte[] replybyte = { SanwaACK.ONLACK_ALREADY_ON_LINE };
                            ReplyOnLineState(e, secsmsg, replybyte);
                        }
                    }
                    else
                    {
                        if (!(CONTROL_STATE.EQUIPMENT_OFF_LINE == newState ||
                            CONTROL_STATE.ON_LINE_LOCATE == newState))
                        {
                            return;
                        }

                        _currentState = newState;
                    }
                    break;
            }

            //傳給使用端
            ChangeControlStateEvent?.Invoke(this, _currentState);
        }
    }
}
