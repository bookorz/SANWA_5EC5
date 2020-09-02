using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS2F18(PrimaryMessageWrapper e, SecsMessage replyMsg)
        {
            string datetime = GetDateTime();
            replyMsg.SecsItem = Item.A(datetime);

            e.ReplyAsync(replyMsg);
        }
    }
}
