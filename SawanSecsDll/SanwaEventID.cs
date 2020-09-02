using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    public class SanwaEventID
    {
        public string _nameDefine;
        public string _name;
        public string _id;
    }

    static class EventName
    {
        //EQP切換 ONLINE -> HOST OFFLINE
        public const string GEM_EQP_OFF_LINE = "GEM_EQP_OFF_LINE";

        //EQP切換 HOST OFFLINE -> ONLINE(LOCAL)
        public const string GEM_CONTROL_STATE_LOCAL = "GEM_CONTROL_STATE_LOCAL";

        //EQP切換 HOST OFFLINE -> ONLINE(REMOTE)
        public const string GEM_CONTROL_STATE_REMOTE = "GEM_CONTROL_STATE_REMOTE";

        //EC參數切換
        public const string GEM_EQ_CONST_CHANGED = "GEM_EQ_CONST_CHANGED";

    }
}
