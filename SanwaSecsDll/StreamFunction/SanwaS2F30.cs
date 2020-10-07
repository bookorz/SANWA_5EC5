using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            foreach (var ecIDObj in eCIDList)
            {
                eCList.TryGetValue(ecIDObj.Key, out SanwaEC Obj);

                ReplyMSG +="<L[6]\r\n";

                if (Obj== null)
                {
                    ReplyMSG += GetToSMLItem(ecIDObj.Value, ecIDObj.Key);
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += "<A[0]>\r\n";
                    ReplyMSG += ">\r\n";
                    continue;
                }
                ReplyMSG += GetToSMLItem(ecIDObj.Value, Obj._id);
                ReplyMSG += Obj._name != null ? "<A[0]"+Obj._name + ">\r\n": "<A[0]>\r\n";
                ReplyMSG += GetSanwaECTypeString(Obj, Obj._minValue);
                ReplyMSG += GetSanwaECTypeString(Obj, Obj._maxValue);
                ReplyMSG += GetSanwaECTypeString(Obj, Obj._defaultValue);
                ReplyMSG += Obj._unit != null ? "<A[0]" + Obj._unit + ">\r\n" : "<A[0]>\r\n";
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
