using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<SecsMessageCommand> S10F3TerminalMessageEvent;
        public event EventHandler<SecsMessageCommand> S10F5TerminalMessageEvent;

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

            if (e.Message.F == 3)
            {
                SecsMessageCommand msgObj = new SecsMessageCommand
                {
                    Name = "TerminalDisplay(S)",
                    MessageName = "S10F3"
                };

                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S10F3TerminalMessageEvent?.Invoke(this, msgObj);
                });
            }
            else
            {
                SecsMessageCommand msgObj = new SecsMessageCommand
                {
                    Name = "TerminalDisplay(M)",
                    MessageName = "S10F5"
                };

                ThreadPool.QueueUserWorkItem(callback =>
                {
                    S10F5TerminalMessageEvent?.Invoke(this, msgObj);
                });
            }

        }
    }
}
