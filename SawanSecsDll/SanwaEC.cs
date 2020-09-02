using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    public class SanwaEC
    {
        public string _nameDefine;
        public string _id;
        public SecsFormat _type;
        public object _minValue;
        public object _maxValue;
        public object _defaultValue;
        public object _value;
        public string _unit;
        public string _name;
        public string _remark;
    }

    static class ECName
    {
        /// <summary>
        /// 1:OffLine, 2:OnLine
        /// <summary>
        public const string GEM_INIT_CONTROL_STATE = "8";
        /// <summary>
        /// 1:Eqp. OFF-line , 2:Attempt On-line , 3:Host Off-line
        /// <summary>
        public const string GEM_OFF_LINE_SUBSTATE = "49";
        /// <summary>
        /// Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
        /// <summary>
        public const string GEM_ON_LINE_SUBSTATE = "51";

        /// <summary>
        /// 0:12-bytes, 1:16-bytes, 2:14-bytes, 3:ISO8601 format
        /// </summary>
        public const string GEM_TIME_FORMAT = "68";

        /// <summary>
        /// Data Format
        /// (1:INT_1, 2:INT_2, 3:INT_4, 4:UINT_1, 5:UINT_2, 6:UINT_4 (for send S6F11))
        /// <summary>
        public const string GEM_DATAID_FORMAT = "71";

        /// <summary>
        /// Data Format
        /// (1:INT_1, 2:INT_2, 3:INT_4, 4:UINT_1, 5:UINT_2, 6:UINT_4 (for send S6F11))
        /// <summary>
        public const string SANWA_ALARMID_FORMAT = "77";


    }

    
}
