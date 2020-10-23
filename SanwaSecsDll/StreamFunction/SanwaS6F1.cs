using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SanwaSecsDll
{
    public class SanwaTDS
    {
        public string _trid;
        public int _smpln;    //the sample number of the last sample in this message 3(), 5() 
        public string _stime;   //20
        public Dictionary<string, SanwaSV> _svList = new Dictionary<string, SanwaSV>();
    }
    public partial class SanwaBaseExec
    {
        public async Task SendS6F1Async(SanwaTDS sanwaTDS)
        {
            //_logger.Info(sanwaTDS._smpln.ToString());
            //SecsMessage secsMessage = _secsMessages[6, 1].FirstOrDefault();
            //if (secsMessage != null)
            //_strFunList.TryGetValue("S6F1", out SanwaStrFunSetting strfunObj);
            _smlManager._messageList.TryGetValue("S6F1", out SanwaSML sanwaSML);

            if (sanwaSML != null)
            {
                SecsMessage secsMessage = new SecsMessage(6, 1, sanwaSML.MessageName);
                string newSendMsg = GetMessageName(secsMessage.ToSml());

                newSendMsg += "< L[4]\r\n";
                newSendMsg += "<U4[0]" + sanwaTDS._trid + ">\r\n";
                newSendMsg += "<U4[0]" + sanwaTDS._smpln.ToString() + ">\r\n";
                newSendMsg += "<A[0]" + sanwaTDS._stime + ">\r\n";
                newSendMsg += "< L[" + sanwaTDS._svList.Count.ToString() + "]\r\n";
                foreach (var PairKey in sanwaTDS._svList)
                {
                    newSendMsg = RecursivelySV(PairKey.Value, newSendMsg);
                }
                newSendMsg += ">\r\n";
                newSendMsg += ">";

                _secsGem.SendAsync(newSendMsg.ToSecsMessage());
            }
        }
    }
}
