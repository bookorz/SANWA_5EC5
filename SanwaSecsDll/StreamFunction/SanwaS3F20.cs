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
        public event EventHandler<E87_HostCommand> S3F19CancelAllCarrierEvent;
        public void ReplyS3F20(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            newReplyMsg += "< L[2]\r\n";

            newReplyMsg += "<U1[0] " + SanwaACK.CAACK_ACK.ToString() + ">\r\n";
            newReplyMsg += "<L[0]\r\n>\r\n";

            newReplyMsg += ">\r\n";
            e.ReplyAsync(newReplyMsg.ToSecsMessage());

            E87_HostCommand hcObj = new E87_HostCommand
            {
                Name = "CancelAllCarrier",
                MessageName = "S3F19",
                lpObj = null,
                carrierObj = null
            };

            ThreadPool.QueueUserWorkItem(callback =>
            {
                S3F19CancelAllCarrierEvent?.Invoke(this, hcObj);
            });            
        }
    }
}
