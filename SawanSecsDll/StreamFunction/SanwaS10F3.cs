using System;
using System.Collections.Generic;
using System.Text;

namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReceiveS10F3(PrimaryMessageWrapper e, ref byte[] ACKC10)
        {
            //ReceiveS10F3
            //L,2
            //    1. < TID >
            //    2.< TEXT >

            //ReceiveS10F5
            //L,2
            //    1. < TID >
            //    L,n.
            //      1.< TEXT1 >
            //      2.< TEXT2 >
            //      3.< TEXT3 >
            //      4.< TEXTn >

            Item TIDItem = e.Message.SecsItem.Items[0];
            Item TEXTItem = e.Message.SecsItem.Items[1];

            ACKC10[0] = SanwaACK.ACKC10_ACK;

            if (TIDItem.Format != SecsFormat.Binary)
            {
                ACKC10[0] = SanwaACK.ACKC10_NOT_DISPLAYED;
                return;
            }

            if (TEXTItem.Format != SecsFormat.List)
            {
                if (TEXTItem.Format != SecsFormat.ASCII)
                {
                    ACKC10[0] = SanwaACK.ACKC10_NOT_DISPLAYED;
                    return;
                }

                _terminalMSGList.Clear();

                _terminalMSGList.Add(TEXTItem.GetString());
            }
            else
            {
                _terminalMSGList.Clear();

                for (int i = 0; i< TEXTItem.Count; i++)
                    _terminalMSGList.Add(TEXTItem.Items[i].GetString());
            }



        }
    }
}
