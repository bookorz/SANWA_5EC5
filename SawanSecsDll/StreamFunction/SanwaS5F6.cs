using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS5F6(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //儲存所有要顯示的EC
            List<string> alarmidlist = new List<string>();

            CheckReceiveIDList(receiveMsg, ref alarmidlist, _alarmList);

            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            newReplyMsg += "< L[" + alarmidlist.Count.ToString() + "]\r\n";

            newReplyMsg = GetAlarmDetailItem(_alarmList, alarmidlist, newReplyMsg);

            newReplyMsg += ">";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }

        private string GetAlarmDetailItem(Dictionary<string, SanwaAlarm> alarmList, List<string> alarmIDList, string ReplyMSG)
        {
            foreach (string iD in alarmIDList)
            {
                alarmList.TryGetValue(iD, out SanwaAlarm Obj);

                ReplyMSG += "<L[3]\r\n";
                //ReplyMSG += "<A[0]" + iD + ">\r\n";

                if (Obj == null)
                {
                    ReplyMSG += "<B[0]>\r\n";
                    ReplyMSG += "<U4[0]"+ Obj._id + ">\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                    continue;
                }

                if (Obj._set)
                {
                    ReplyMSG += "<B[0]0x80>\r\n";
                }
                else
                {
                    ReplyMSG += "<B[0]0x00>\r\n";
                }

                ReplyMSG += "<U4[0]" + Obj._id + ">\r\n";
                ReplyMSG += "<A[0]"+ Obj._text + ">\r\n";
                ReplyMSG += ">\r\n";
            }

            return ReplyMSG;
        }
    }
}
