using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<SecsMessageCommand> S10F5TerminalMessageEvent;

        public void ReceiveS10F5(PrimaryMessageWrapper e, ref byte[] ACKC10)
        {
            //ReceiveS10F5
            //L,2
            //    1. < TID >
            //    L,n.
            //      1.< TEXT>
            Item TIDItem = e.Message.SecsItem.Items[0];
            Item TEXTItemList = e.Message.SecsItem.Items[1];

            ACKC10[0] = SanwaACK.ACKC10_ACK;

            if (TIDItem.Format != SecsFormat.Binary)
            {
                ACKC10[0] = SanwaACK.ACKC10_NOT_DISPLAYED;
                return;
            }

            if (TEXTItemList.Format != SecsFormat.ASCII)
            {
                ACKC10[0] = SanwaACK.ACKC10_NOT_DISPLAYED;
                return;
            }

            _terminalMSGList.Clear();

            for (int i = 0; i < TEXTItemList.Count; i++)
                _terminalMSGList.Add(TEXTItemList.Items[i].GetString());



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
