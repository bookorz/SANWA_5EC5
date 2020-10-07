using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<E87_HostCommand> S3F21AutoModeEvent;
        public event EventHandler<E87_HostCommand> S3F21ManualModeEvent;
        public void ReplyS3F22(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,3   
            //    1. < PORTGRPNAME > 
            //    2. < ACCESSMODE > 
            //    3.L,n        
            //        1. < PTN1 >        
            //        .        
            //        .
            //        n. < PTNn >
                    
            ReplyAccessMode(e, receiveMsg, replyMsg, true);

        }
    }
}
