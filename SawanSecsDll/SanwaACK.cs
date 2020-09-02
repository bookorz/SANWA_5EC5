using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    static class SanwaACK
    {
        //Receive/Reply by S1F15/16
        public const byte OFLACK_ACK = 0x00;

        //Receive/Reply by S1F17/18
        public const byte ONLACK_ACCEPTED = 0x00;
        public const byte ONLACK_NOT_ACCEPTED = 0x01;
        public const byte ONLACK_ALREADY_ON_LINE = 0x02;

        //Receive/Reply by S2F15/16
        public const byte EAC_ACK = 0x00;
        public const byte EAC_DENIED_EC_NOT_EXIST = 0x01;
        public const byte EAC_DENIED_BUSY = 0x02;
        public const byte EAC_DENIED_EC_OUT_RANGE = 0x03;
        public const byte EAC_DENIED_BY_SANWA_DEFINED = 0x04;

        //Receive/Reply by S2F23/24
        public const byte TIAACK_ACK = 0x00;
        public const byte TIAACK_TO_MANY_SVIDS = 0x01;
        public const byte TIAACK_NO_MORE_TRACES_ALLOWED= 0x02;
        public const byte TIAACK_INVALID_DSPER = 0x03;
        public const byte TIAACK_INVALID_SVID = 0x04;
        public const byte TIAACK_INVALID_REPGSZ = 0x05;

        //Receive/Reply by S2F31/32
        public const byte TIACK_ACK = 0x00;
        public const byte TIACK_ERROR = 0x01;

        //Receive by S5F3
        public const byte ALED_ENABLED = 0x01;
        public const byte ALED_DISABLED = 0x00;

        //Reply by S5F4
        public const byte ACKC5_ACK = 0x00;
        public const byte ACKC5_ERROR = 0x01;

        //Reply by S10F4
        public const byte ACKC10_ACK = 0x00;
        public const byte ACKC10_NOT_DISPLAYED = 0x01;
        public const byte ACKC10_TERMINAL_NOT_AVAILABLE = 0x02;
    }
}
