using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS2F30(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //儲存所有要顯示的EC
            //List<string> ecidlist = new List<string>();
            Dictionary<string,SecsFormat> ecidlist = new Dictionary<string,SecsFormat>();

            CheckReceiveIDList(receiveMsg, ref ecidlist, _ecIDList);

            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            newReplyMsg += "< L[" + ecidlist.Count.ToString() + "]\r\n";

            newReplyMsg = GetECDetailItem(_ecIDList, ecidlist, newReplyMsg);

            newReplyMsg += ">";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        //private string GetECDetailItem(Dictionary<string, SanwaEC> eCList, List<string> eCIDList, string ReplyMSG)
        private string GetECDetailItem(Dictionary<string, SanwaEC> eCList, Dictionary<string, SecsFormat> eCIDList, string ReplyMSG)
        {
            //解碼 SVID、SVNAME、UNITS
            _smlManager._messageList.TryGetValue("S2F30", out SanwaSML smlObj);

            List<string> requestList = new List<string>();
            using (StringReader reader = new StringReader(smlObj.Text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("ECID"))
                    {
                        requestList.Add("ECID");
                    }
                    else if (line.Contains("ECNAM"))
                    {
                        requestList.Add("ECNAM");
                    }
                    else if (line.Contains("ECMIN"))
                    {
                        requestList.Add("ECMIN");
                    }
                    else if (line.Contains("ECMAX"))
                    {
                        requestList.Add("ECMAX");
                    }
                    else if (line.Contains("ECDEF"))
                    {
                        requestList.Add("ECDEF");
                    }
                    else if(line.Contains("UNIT"))
                    {
                        requestList.Add("UNIT");
                    }
                }
            }

            foreach (var ecIDObj in eCIDList)
            {
                eCList.TryGetValue(ecIDObj.Key, out SanwaEC Obj);

                ReplyMSG +="<L["+ requestList.Count+ "]\r\n";

                if (Obj== null)
                {
                    foreach(string str in requestList)
                    {
                        if(str.Equals("ECID"))
                        {
                            ReplyMSG += GetToSMLItem(ecIDObj.Value, ecIDObj.Key);
                        }
                        else if(str.Equals("ECNAM"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }
                        else if(str.Equals("ECMIN"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }
                        else if(str.Equals("ECMAX"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }
                        else if(str.Equals("ECDEF"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }
                        else if(str.Equals("UNIT"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }
                    }

                    ReplyMSG += ">\r\n";
                    continue;
                }

                foreach (string str in requestList)
                {
                    if (str.Equals("ECID"))
                    {
                        ReplyMSG += GetToSMLItem(ecIDObj.Value, Obj._id);
                    }
                    else if (str.Equals("ECNAM"))
                    {
                        ReplyMSG += Obj._name != null ? "<A[0]" + Obj._name + ">\r\n" : "<A[0]>\r\n";
                    }
                    else if (str.Equals("ECMIN"))
                    {
                        ReplyMSG += GetSanwaECTypeString(Obj, Obj._minValue);
                    }
                    else if (str.Equals("ECMAX"))
                    {
                        ReplyMSG += GetSanwaECTypeString(Obj, Obj._maxValue);
                    }
                    else if (str.Equals("ECDEF"))
                    {
                        ReplyMSG += GetSanwaECTypeString(Obj, Obj._defaultValue);
                    }
                    else if (str.Equals("UNIT"))
                    {
                        ReplyMSG += Obj._unit != null ? "<A[0]" + Obj._unit + ">\r\n" : "<A[0]>\r\n";
                    }
                }
                ReplyMSG += ">\r\n";
            }

            return ReplyMSG;
        }
        private string GetSanwaECTypeString(SanwaEC sanwaEC, object Value)
        {
            string strRet = "<A[0]>\r\n";

            if(Value!= null)
            { 
                switch (sanwaEC._type)
                {
                    case SecsFormat.List:
                        break;
                    default:
                        strRet = GetTypeStringValue(sanwaEC._type, Value);
                        break;
                }
            }

            return strRet;
        }
    }
}
