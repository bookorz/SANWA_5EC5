using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public class SanwaSV
    {
        public string _name;
        public string _id;
        public SecsFormat _format;
        public string _unit;
        public Object _value;
        public string _sVName;
    }

    static class SVName
    {
        public const string LICENSE_CODE_SVID = "LICENSE_CODE_SVID";
        public const string LICENSE_STATUS_SVID = "LICENSE_STATUS_SVID";
        //時間
        public const string GEM_CLOCK = "GEM_CLOCK";
        /// <summary>
        /// EQP連線狀態
        /// 1:Eqp OffLine, 2:Attemp - OnLine, 3:Host Off-Line, 4:OnLine Local, 5:OnLine Remote
        /// </summary>
        public const string GEM_CONTROL_STATE = "GEM_CONTROL_STATE";
        //與TCPIP連線狀態
        public const string GEM_LINK_STATE = "GEM_LINK_STATE";
        //目前強制為1
        public const string GEM_COMM_MODE = "GEM_COMM_MODE";
        //前一筆CEID訊息
        public const string GEM_PREVIOUS_CEID = "GEM_PREVIOUS_CEID";
        /// <summary>
        /// 1:Eqp OffLine, 2:Attempt OnLine, 3:HostOffLiine, 0:unknown
        /// </summary>
        public const string GEM_OFF_LINE_SUB_STATE_SV = "GEM_OFF_LINE_SUB_STATE_SV";
        /// <summary>
        /// 1:Eqp OffLine, 2:OnLine Local, 3:OnLine Remote
        /// </summary>
        public const string GEM_PREVIOUS_CONTROL_STATE = "GEM_PREVIOUS_CONTROL_STATE";
        public const string GEM_PREVIOUS_PROCESS_STATE = "GEM_PREVIOUS_PROCESS_STATE";
        public const string GEM_PROCESS_STATE = "GEM_PROCESS_STATE";

        //mdln
        public const string GEM_MDLN = "GEM_MDLN";
        //softrev
        public const string GEM_SOFTREV = "GEM_SOFTREV";

        public const string GEM_ALARM_ENABLED = "GEM_ALARM_ENABLED";
        public const string GEM_ALARM_SET = "GEM_ALARM_SET";
        public const string GEM_EVENT_ENABLED = "GEM_EVENT_ENABLED";
        public const string GEM_PP_EXEC_NAME = "GEM_PP_EXEC_NAME";
        public const string PP_FORMAT = "PP_FORMAT";
        public const string GEM_SPOOL_COUNT_ACTUAL = "GEM_SPOOL_COUNT_ACTUAL";
        public const string GEM_SPOOL_COUNT_TOTAL = "GEM_SPOOL_COUNT_TOTAL";
        public const string GEM_SPOOL_FULL_TIME = "GEM_SPOOL_FULL_TIME";
        public const string GEM_SPOOL_START_TIME = "GEM_SPOOL_START_TIME";
        public const string GEM_SPOOL_STATE = "GEM_SPOOL_STATE";
        public const string GEM_SOFTWARE_REVISION = "GEM_SOFTWARE_REVISION";
    }
}
