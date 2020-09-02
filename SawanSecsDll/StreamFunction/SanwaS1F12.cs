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
        private string ParseSVNRRToSML(List<string> svIDList, string ReceiveMSG)
        {
            string ReplyMSG = GetMessageName(ReceiveMSG);

            ReplyMSG += "< L[" + svIDList.Count.ToString() + "]\r\n";

            foreach (string svid in svIDList)
            {
                ReplyMSG += "<L [3]\r\n";
                _svList.TryGetValue(svid, out SanwaSV obj);

                ReplyMSG = ReplyMSG + "<U4[0]" + svid + ">\r\n";
                if (obj == null)
                {
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                }
                else
                {
                    ReplyMSG = ReplyMSG + "<A[0]" + obj._name + ">\r\n";

                    if (obj._unit == null)
                        ReplyMSG += "<A[0]>\r\n";
                    else
                        ReplyMSG = ReplyMSG + "<A[0]" + obj._unit + ">\r\n";
                }

                ReplyMSG += ">\r\n";
            }

            ReplyMSG += ">\r\n";

            return ReplyMSG;
        }
    }
}
