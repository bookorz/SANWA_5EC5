using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public class SanwaEC
    {
        public string _name;
        public string _id;
        public SecsFormat _type;
        public object _minValue;
        public object _maxValue;
        public object _defaultValue;
        public object _value;
        public string _unit;
        //public string _name;
        public string _comment;
        public string _remark;
    }

    public static class ECName
    {
        /// <summary>
        /// 0:Disable, 1:Enable
        /// </summary>        
        public const string GEM_INIT_COMM_STATE = "GEM_INIT_COMM_STATE";
        /// <summary>
        /// 1:OffLine, 2:OnLine
        /// <summary>
        public const string GEM_INIT_CONTROL_STATE = "GEM_INIT_CONTROL_STATE";

        /// <summary>
        /// 0:Not Set, 1:Set  (for send S5Fx )
        /// </summary>
        public const string GEM_WBIT_S5 = "GEM_WBIT_S5";
        public const string GEM_WBIT_S6 = "GEM_WBIT_S6";
        public const string GEM_WBIT_S10 = "GEM_WBIT_S10";


        public const string GEM_ESTAB_COMM_DELAY = "GEM_ESTAB_COMM_DELAY";
        /// <summary>
        /// 1:Eqp. OFF-line , 2:Attempt On-line , 3:Host Off-line
        /// <summary>
        public const string GEM_OFF_LINE_SUBSTATE = "GEM_OFF_LINE_SUBSTATE";
        
        public const string GEM_ON_LINE_FAILED = "GEM_ON_LINE_FAILED";

        /// <summary>
        /// Host On-line 需求 4:On-line/Local, 5:ON-line/Remote
        /// <summary>
        public const string GEM_ON_LINE_SUBSTATE = "GEM_ON_LINE_SUBSTATE";

        public const string GEM_MAX_SPOOL_TRANSMIT = "GEM_MAX_SPOOL_TRANSMIT";
        public const string GEM_CONFIG_SPOOL = "GEM_CONFIG_SPOOL";
        public const string GEM_OVER_WRITE_SPOOL = "GEM_OVER_WRITE_SPOOL";

        /// <summary>
        /// 0:12-bytes, 1:16-bytes, 2:14-bytes, 3:ISO8601 format
        /// </summary>
        public const string GEM_TIME_FORMAT = "GEM_TIME_FORMAT";

        /// <summary>
        /// Data Format
        /// (1:INT_1, 2:INT_2, 3:INT_4, 4:UINT_1, 5:UINT_2, 6:UINT_4 (for send S6F11))
        /// <summary>
        public const string GEM_DATAID_FORMAT = "GEM_DATAID_FORMAT";

        /// <summary>
        /// SAMPLN  Format
        /// 1:INT_1, 2:INT_2, 3:INT_4, 4:UINT_1, 5:UINT_2, 6:UINT_4 (for send S6F1)
        /// <summary>
        public const string GEM_SAMPLN_FORMAT = "GEM_SAMPLN_FORMAT";
    }

    
}
