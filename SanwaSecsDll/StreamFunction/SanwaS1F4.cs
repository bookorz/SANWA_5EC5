using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS1F4(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //List<string> sVIDList = new List<string>();
            Dictionary<string, SecsFormat> sVIDList = new Dictionary<string, SecsFormat>();
            CheckReceiveIDList(receiveMsg, ref sVIDList, _svIDList);

            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            newReplyMsg += "< L[" + sVIDList.Count.ToString() + "]\r\n";

            List<string> tempSVIDList = new List<string>();
            foreach(string id in sVIDList.Keys) tempSVIDList.Add(id);
            
            newReplyMsg = RecursivelySVList(_svIDList, tempSVIDList, newReplyMsg);
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
