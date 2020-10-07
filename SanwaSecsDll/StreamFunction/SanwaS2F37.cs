using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReceiveS2F37(PrimaryMessageWrapper e, ref byte[] ERACK)
        {
            //Structure: L,2
            //    1. < CEED > enable / disable
            //    2.L,n #CEIDs
            //        1. < CEID1 >
            //        .
            //        .
            //        n. < CEIDn >

            ERACK[0] = SanwaACK.ERACK_ACK;

            Item CEEDItem = e.Message.SecsItem.Items[0];
            Item EventListItem = e.Message.SecsItem.Items[1];

            //確認所有CEID是否正確
            for (int i = 0; i< EventListItem.Count; i++)
            {
                Item EventItem = EventListItem.Items[i];
                if(!CheckFomart3x5x20(EventItem))
                {
                    ERACK[0] = SanwaACK.ERACK_CEID_NOT_EXIS;
                    return;
                }

                SetItemToStringType(EventItem, out string EventID);

                _eventIDList.TryGetValue(EventID, out SanwaEvent eventObj);

                if (eventObj == null)
                {
                    ERACK[0] = SanwaACK.ERACK_CEID_NOT_EXIS;
                    return;
                }
            }

            bool enabled = CEEDItem.GetValue<bool>();
            if (EventListItem.Count == 0)   //Event 全部設定
            {
                foreach (var keyValuePair in _eventList)
                {
                    SanwaEvent eventObj = keyValuePair.Value;
                    eventObj._enabled = enabled;
                }
            }
            else
            {
                for (int i = 0; i < EventListItem.Count; i++)
                {
                    Item EventItem = EventListItem.Items[i];
                    SetItemToStringType(EventItem, out string EventID);
                    _eventIDList.TryGetValue(EventID, out SanwaEvent eventObj);
                    eventObj._enabled = enabled;
                }
            }
        }
    }
}
