using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SanwaSecsDll
{

    public partial class SanwaBaseExec
    {
        public event EventHandler<E87_HostCommand> S3F27AutoModeEvent;
        public event EventHandler<E87_HostCommand> S3F27ManualModeEvent;

        public void ReplyS3F28(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,2   1. < ACCESSMODE > 
            //    2.L,n        
            //        1. < PTN1 >
            //        .        
            //        .
            //        n. < PTNn >
            ReplyAccessMode(e, receiveMsg, replyMsg, false);

        }

    }
}
