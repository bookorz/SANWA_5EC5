using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS16F20(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            newReplyMsg += "<L["+ _pJList.Count + "]\r\n";
            foreach (var pjObj in _pJList)
            {
                newReplyMsg += "<L[2]\r\n";
                newReplyMsg += "<A[0]"+ pjObj.ObjID + ">\r\n";
                newReplyMsg += "<U1[0]" + ((byte)pjObj.PRJobState).ToString() + ">\r\n";
                newReplyMsg += ">\r\n";
            }
            newReplyMsg += ">\r\n";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
    }
}
