using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public class SanwaEvent
    {
        public string _name;
        public string _comment;
        public string _id;
        public bool _enabled = true;

        public List<string> _rptidList = new List<string>();

        public void AddRPTID(string rptid)
        {
            _rptidList.Add(rptid);
        }

        public void ClearRPTIDList()
        {
            _rptidList.Clear();
        }
    }

    public class SanwaVID
    {
        public string _id;
        public SecsFormat _format = SecsFormat.U4;
    }

    public class SanwaRPTID
    {
        public string _id;
        public string _eventid = null;
        public SecsFormat _format = SecsFormat.U4;

        //public List<string> _vidList = new List<string>();
        public Dictionary<string, SanwaVID> _vidList = new Dictionary<string, SanwaVID>();
        //public void Add(string vid)
        //{
        //    _vidList.Add(vid);
        //}
        public void Add(SanwaVID vid)
        {
            _vidList.Add(vid._id, vid);
        }

        public void Clear()
        {
            _vidList.Clear();
        }
    }

    static public class EventName
    {
        public const string GEM_PP_CHANGE = "GEM_PP_CHANGE";

        //EQP切換 HOST OFFLINE -> ONLINE(LOCAL)
        public const string GEM_CONTROL_STATE_LOCAL = "GEM_CONTROL_STATE_LOCAL";

        //EQP切換 HOST OFFLINE -> ONLINE(REMOTE)
        public const string GEM_CONTROL_STATE_REMOTE = "GEM_CONTROL_STATE_REMOTE";

        //EC 變更
        public const string GEM_EQ_CONST_CHANGED = "GEM_EQ_CONST_CHANGED";

        public const string GEM_MESSAGE_RECOGNITION = "GEM_MESSAGE_RECOGNITION";
        
        //EQP切換 ONLINE -> HOST OFFLINE
        public const string GEM_EQP_OFF_LINE = "GEM_EQP_OFF_LINE";

        public const string GEM_SPOOLING_ACTIVED = "GEM_SPOOLING_ACTIVED";

        public const string GEM_SPOOLING_DEACTIVED = "GEM_SPOOLING_DEACTIVED";

        public const string GEM_SPOOL_TRANSMIT_FAILURE = "GEM_SPOOL_TRANSMIT_FAILURE";




    }
}
