using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public bool ReceiveS5F3(PrimaryMessageWrapper e, ref byte[] ACKC5)
        {
            //  L,2
            //  1. < ALED >
            //  2. < ALID >
            //Exception: A zero-length item for ALID means all alarms.

            bool ReturnOutput = true;

            string SearchKey = "S5F3";
            _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj);
            string text = smlObj.Text;

            ACKC5[0] = SanwaACK.ACKC5_ACK;

            int ALEDIndex = -1;
            int ALIDIndex = -1;

            string line;
            using (StringReader reader = new StringReader(text))
            {
                int rowIndex = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("ALED"))
                    {
                        ALEDIndex = rowIndex - 1;
                    }
                    else if (line.Contains("ALID"))
                    {
                        ALIDIndex = rowIndex - 1;
                    }
                    rowIndex++;
                }
            }

            if(ALEDIndex < 0 || ALIDIndex < 0)
            {
                return false;
            }

            Item ALEDItem = e.Message.SecsItem.Items[ALEDIndex];
            Item ALIDItem = e.Message.SecsItem.Items[ALIDIndex];

            //確認格式
            if (ALEDItem.Format != SecsFormat.Binary)
            {
                ACKC5[0] = SanwaACK.ACKC5_ERROR;
                return ReturnOutput;
            }

            //確認格式
            if (!CheckFomart3x5x(ALIDItem))
            {
                ACKC5[0] = SanwaACK.ACKC5_ERROR;
                return ReturnOutput;
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
                _alarmIDList.TryGetValue(alarmid, out SanwaAlarm obj);

                if (obj == null)
                {
                    ACKC5[0] = SanwaACK.ACKC5_ERROR;
                    return ReturnOutput;
                }
                else
                {
                    obj._enabled = enabled;
                }
            }

            return ReturnOutput;

        }

    }
}
