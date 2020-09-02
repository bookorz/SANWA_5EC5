using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReceiveS5F3(PrimaryMessageWrapper e, ref byte[] ACKC5)
        {
            //  L,2
            //  1. < ALED >
            //  2. < ALID >
            //Exception: A zero-length item for ALID means all alarms.

            ACKC5[0] = SanwaACK.ACKC5_ACK;

            Item ALEDItem = e.Message.SecsItem.Items[0];
            Item ALIDItem = e.Message.SecsItem.Items[1];

            //確認格式
            if (ALEDItem.Format != SecsFormat.Binary)
            {
                ACKC5[0] = SanwaACK.ACKC5_ERROR;
                return;
            }

            //確認格式
            if (!CheckFomart3x5x(ALIDItem))
            {
                ACKC5[0] = SanwaACK.ACKC5_ERROR;
                return;
            }

            byte[] ALED = ALEDItem.GetValues<byte>();
            bool enabled = ALED[0] == SanwaACK.ALED_ENABLED ? true : false;

            if (ALIDItem.Count == 0)
            {
                foreach (var AlarmObj in _alarmList)
                {
                    SanwaAlarm obj = AlarmObj.Value;
                    obj._enabled = enabled;
                }
            }
            else
            {
                //確認是否有對應ID
                SetItemToStringType(ALIDItem, out string alarmid);
                _alarmList.TryGetValue(alarmid, out SanwaAlarm obj);

                if (obj == null)
                {
                    ACKC5[0] = SanwaACK.ACKC5_ERROR;
                    return;
                }
                else
                {
                    obj._enabled = enabled;
                }
            }

        }

    }
}
