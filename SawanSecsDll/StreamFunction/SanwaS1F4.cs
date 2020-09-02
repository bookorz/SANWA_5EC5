using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS1F4(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //儲存所有要顯示的EC
            List<string> sVIDList = new List<string>();

            CheckReceiveIDList(receiveMsg, ref sVIDList,_svList);

            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            newReplyMsg += "< L[" + sVIDList.Count.ToString() + "]\r\n";

            newReplyMsg = RecursivelySVList(_svList, sVIDList, newReplyMsg);

            newReplyMsg += ">";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }

        private string RecursivelySVList(Dictionary<string, SanwaSV> sVList, List<string> sVIDList, string ReplyMSG)
        {
            foreach (string iD in sVIDList)
            {
                sVList.TryGetValue(iD, out SanwaSV Obj);

                if (Obj == null)
                {
                    ReplyMSG += "<L[0]\r\n>\r\n";
                    continue;
                }

                if (Obj._value == null)
                {
                    ReplyMSG += "<L[0]\r\n>\r\n";
                    continue;
                }

                switch (Obj._format)
                {
                    case SecsFormat.List:
                        Dictionary<string, SanwaSV> _svSubList = (Dictionary<string, SanwaSV>)Obj._value;
                        List<string> SVList = new List<string>();
                        foreach (var subObj in _svSubList) SVList.Add(subObj.Key);
                        ReplyMSG = ReplyMSG + "<L [" + SVList.Count.ToString() + "]\r\n";
                        ReplyMSG = RecursivelySVList(_svSubList, SVList, ReplyMSG);
                        ReplyMSG += ">\r\n";
                        break;
                    default:
                        ReplyMSG += GetTypeStringValue(Obj._format, Obj._value);
                        break;
                }
                
            }

            return ReplyMSG;
        }
    }
}
