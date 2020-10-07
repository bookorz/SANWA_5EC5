using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public class SanwaAlarm
    {
        public string _name;
        public string _id;
        public string _cd;
        public bool _enabled = true;
        public string _text;

        public bool _set = false;
    }

    public static class AlarmID
    {
        public const uint ALARM_PUMP_PRESS = 9001;
        public const uint ALARM_EM = 9002;
        public const uint ALARM_HEATER_FAIL = 9003;
        public const uint ALARM_HEATER_FAIL2 = 1000001;
    }


}
