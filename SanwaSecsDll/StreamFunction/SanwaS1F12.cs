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
            //解碼 SVID、SVNAME、UNITS
            _smlManager._messageList.TryGetValue("S1F12", out SanwaSML smlObj);

            List<string> requestList = new List<string>();
            using (StringReader reader = new StringReader(smlObj.Text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(line.Contains("SVID"))
                    {
                        requestList.Add("SVID");
                    }
                    else if(line.Contains("SVNAME"))
                    {
                        requestList.Add("SVNAME");
                    }
                    else if(line.Contains("UNITS"))
                    {
                        requestList.Add("UNITS");
                    }
                }
            }

            string ReplyMSG = GetMessageName(ReceiveMSG);

            ReplyMSG += "< L[" + svIDList.Count.ToString() + "]\r\n";

            foreach (var svIDObj in svIDList)
            {
               
                ReplyMSG += "<L["+ requestList.Count + "]\r\n";
                _svIDList.TryGetValue(svIDObj.Key, out SanwaSV obj);

                if (obj == null)
                {
                    foreach(string str in requestList)
                    {
                        if (str.Contains("SVID"))
                        {
                            ReplyMSG += GetToSMLItem(svIDObj.Value, svIDObj.Key);
                        }
                        else if(str.Contains("SVNAME"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }
                        else if(str.Contains("UNITS"))
                        {
                            ReplyMSG += "<A[0]>\r\n";
                        }

                    }
                }
                else
                {
                    foreach (string str in requestList)
                    {
                        if (str.Contains("SVID"))
                        {
                            //SVID Format 由Host端決定 
                            ReplyMSG += GetToSMLItem(svIDObj.Value, obj._id);
                        }
                        else if (str.Contains("SVNAME"))
                        {
                            ReplyMSG = ReplyMSG + "<A[0]" + obj._name + ">\r\n";
                        }
                        else if (str.Contains("UNITS"))
                        {
                            if (obj._unit == null)
                                ReplyMSG += "<A[0]>\r\n";
                            else
                                ReplyMSG = ReplyMSG + "<A[0]" + obj._unit + ">\r\n";
                        }
                    }
                }

                ReplyMSG += ">\r\n";
            }

            ReplyMSG += ">\r\n";

            return ReplyMSG;
        }
        public void ReplyS1F12(PrimaryMessageWrapper e, SecsMessage receiveSecsmsg, SecsMessage tempReplySecsmsg)
        {
            //1.複製"收到訊息格式"給"回覆訊息的格式"
            //2.根據訊息長度決定根據SVID回覆，還是全部回覆
            //2.a.將個別SVID存入List裡面
            //2.b.將所有SVID存入List裡面   
            //3.根據SVID開始寫檔

            //1.複製"收到訊息格式"給"回覆訊息的格式"
            CopyReceiveMsgToReply(ref receiveSecsmsg, ref tempReplySecsmsg);

            Dictionary<string, SecsFormat> svIDList = new Dictionary<string, SecsFormat>();
            if (tempReplySecsmsg.SecsItem.Count > 0)//2.根據訊息長度決定根據SVID回覆，還是全部回覆
            {
                //a.將個別SVID存入List裡面
                foreach (Item items in receiveSecsmsg.SecsItem.Items)
                {
                    SetItemToStringType(items, out string id);
                    //SanwaSV svObj = _svList.FirstOrDefault(x => x.Value._id == id).Value;
                    _svIDList.TryGetValue(id, out SanwaSV svObj);
                    if (svObj != null)
                    {
                        svIDList.Add(svObj._id, items.Format);
                    }
                    else
                    {
                        svIDList.Add(id, items.Format);
                    }

                }
            }
            else
            {
                //2.b.將所有SVID存入List裡面 
                foreach (KeyValuePair<string, SanwaSV> item in _svIDList)
                {
                    //暫時將輸出SVID的Format定義為U4
                    svIDList.Add(item.Key, SecsFormat.U4);
                }
            }

            //3.根據SVID開始寫檔
            string smlFormat = tempReplySecsmsg.ToSml();

            //4.編輯SML檔
            string NewsmlFormat = ParseSVNRRToSML(svIDList, smlFormat);

            //5.轉換為SECSMessage
            tempReplySecsmsg = NewsmlFormat.ToSecsMessage();

            e.ReplyAsync(tempReplySecsmsg);
        }



    }
}
