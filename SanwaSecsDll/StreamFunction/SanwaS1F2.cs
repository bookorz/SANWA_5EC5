using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SanwaSecsDll
{ 
    public partial class SanwaBaseExec
    {
        public bool ReplyS1F2(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {

            bool ReturnOutput = true;

            string SearchKey = "S1F2";
            _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj);
            string text = smlObj.Text;

            string line;

            string newReplyMsg = GetMessageName(replyMsg.ToSml());

            using (StringReader reader = new StringReader(text))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if(line.Contains("\'"))
                    {
                        if(line.Contains("MDLN"))
                        {
                            GetSVData(SVName.GEM_MDLN, out SanwaSV mdlnSV);
                            newReplyMsg += "<A[0]" + mdlnSV._value + ">\r\n";
                        }
                        else if(line.Contains("SOFTREV"))
                        {
                            GetSVData(SVName.GEM_SOFTREV, out SanwaSV softrevSV);
                            newReplyMsg += "<A[0]" + softrevSV._value + ">\r\n";
                        }
                        else
                        {
                            ReturnOutput = false;
                        }
                    }
                    else
                    {
                        newReplyMsg += line;
                        newReplyMsg += "\r\n";
                    }

                }
            }

            if(ReturnOutput)
                e.ReplyAsync(newReplyMsg.ToSecsMessage());

            return ReturnOutput;

            //newReplyMsg += "<L[2]\r\n";
            //GetSVData(SVName.GEM_MDLN, out SanwaSV mdlnSV);
            //GetSVData(SVName.GEM_SOFTREV, out SanwaSV softrevSV);
            //newReplyMsg += "<A[0]" + mdlnSV._value + ">\r\n";
            //newReplyMsg += "<A[0]" + softrevSV._value + ">\r\n";

            //newReplyMsg += ">\r\n";

        }
    }
}
