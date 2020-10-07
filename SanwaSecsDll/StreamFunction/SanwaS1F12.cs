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
        private string ParseSVNRRToSML(Dictionary<string, SecsFormat> svIDList, string ReceiveMSG)
        {
            string ReplyMSG = GetMessageName(ReceiveMSG);

            ReplyMSG += "< L[" + svIDList.Count.ToString() + "]\r\n";

            foreach (var svIDObj in svIDList)
            {
                ReplyMSG += "<L [3]\r\n";
                _svIDList.TryGetValue(svIDObj.Key, out SanwaSV obj);

                if (obj == null)
                {
                   // ReplyMSG +="<U4[0]" + svid + ">\r\n";
                    ReplyMSG += GetToSMLItem(svIDObj.Value, svIDObj.Key);
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                }
                else
                {
                    //SVID Format 由Host端決定 
                    ReplyMSG += GetToSMLItem(svIDObj.Value, obj._id);
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
