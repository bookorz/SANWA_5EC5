using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS2F18(PrimaryMessageWrapper e, SecsMessage replyMsg)
        {
            string datetime = GetDateTime();
            //_svList.TryGetValue(SVName.GEM_CLOCK, out SanwaSV svObj);
            //if(svObj != null) svObj._value = datetime;
            SetSV(SVName.GEM_CLOCK, datetime);
            replyMsg.SecsItem = Item.A(datetime);

            e.ReplyAsync(replyMsg);
        }
    }
}
