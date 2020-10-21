using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public event EventHandler<SecsMessageCommand> S10F3TerminalMessageEvent;


        public void ReceiveS10F3(PrimaryMessageWrapper e, ref byte[] ACKC10)
        {
            //ReceiveS10F3
            //L,2
            //    1.< TID >
            //    2.< TEXT >

            int TIDIndex = -1;
            int TEXTIndex = -1;

            string SearchKey = "S10F3";
            _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj);
            string text = smlObj.Text;

            List<string> requestList = new List<string>();
            using (StringReader reader = new StringReader(smlObj.Text))
            {
                string line;
                int rowIndex = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("TID"))
                    {
                        TIDIndex = rowIndex;
                    }
                    else if (line.Contains("TEXT"))
                    {
                        TEXTIndex = rowIndex;                       
                    }

                    if(line.Contains("<L"))
                    {
                        rowIndex++;
                    }
                    else
                    {
                        rowIndex = 0;
                    }
                }
            }

            ACKC10[0] = SanwaACK.ACKC10_ACK;

            if (TIDIndex != -1)
            {
                Item TIDItem = e.Message.SecsItem.Items[TIDIndex];

                if (TIDItem.Format != SecsFormat.Binary)
                {
                    ACKC10[0] = SanwaACK.ACKC10_NOT_DISPLAYED;
                    return;
                }
            }

            if(TEXTIndex != -1)
            {
                Item TEXTItem = e.Message.SecsItem.Items[TEXTIndex];

                if (TEXTItem.Format != SecsFormat.ASCII)
                {
                    ACKC10[0] = SanwaACK.ACKC10_NOT_DISPLAYED;
                    return;
                }

                _terminalMSGList.Clear();

                _terminalMSGList.Add(TEXTItem.GetString());
            }

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
    }
}
