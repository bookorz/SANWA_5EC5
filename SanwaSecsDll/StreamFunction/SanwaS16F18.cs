using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS16F18(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,m[m = # jobs to remove]   
            //    1. <PRJOBID1>   
            //    .   
            //    m. <PRJOBIDm> 

            //L,2   
            //    1.L,m[m = # jobs removed]        
            //        1. <PRJOBID1>        
            //        .        
            //        m. <PRJOBIDm>   
            //    2. L,2        
            //        1. <ACKA>        
            //        2. L,n             
            //            1. L,2                  
            //                1. <ERRCODE1>                  
            //                2. <ERRTEXT1>             
            //            .             
            //            n. L,2                  
            //                1. <ERRCODEn>                  
            //                2. <ERRTEXTn>

            List<string> prJobID = new List<string>();

            Item PRJOBIDItemList = e.Message.SecsItem;
            if(PRJOBIDItemList.Count.Equals(0))
            {
                foreach(SanwaPJ sanwaPJ in _pJQueue)
                {
                    prJobID.Add(sanwaPJ.ObjID);
                }
            }
            else
            {
                for(int m = 0; m< PRJOBIDItemList.Count; m++)
                {
                    Item PRJOBIDItem = PRJOBIDItemList.Items[m];
                    
                    SetItemToStringType(PRJOBIDItem, out string jobID);

                    var sanwaPJObjQ = _pJQueue
                                    .Where (x=> x.ObjID.Equals(jobID))
                                    .Select (x => x);

                    foreach(var sanwaPJ in sanwaPJObjQ)
                    {
                        SanwaPJ PJ = (SanwaPJ)sanwaPJ;
                        prJobID.Add(PJ.ObjID);
                    }

                }
            }

            bool acka = SanwaACK.ACKA_SUCCESSFUL;

            if(acka == SanwaACK.ACKA_SUCCESSFUL)
            {
                foreach(var id in prJobID)
                {
                    int count;
                    count = _pJQueue.RemoveAll(x => x.ObjID == id);

                    if(!count.Equals(0))
                    {
                        _pJList.RemoveAll(x => x.ObjID == id);
                    }
                }
            }

            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            newReplyMsg += "<L[2]\r\n";
            newReplyMsg += "<L[" + prJobID.Count + "]\r\n";
            foreach(string ObjID in prJobID)
            {
                newReplyMsg += "<A[1]" + ObjID + ">\r\n";
            }
            newReplyMsg += ">\r\n";
            newReplyMsg += "<L[2]\r\n";
            newReplyMsg += "<Boolean[1]" + acka.ToString() + ">\r\n";
            newReplyMsg += "<L[0]\r\n";
            newReplyMsg += ">\r\n";
            newReplyMsg += ">\r\n";
            newReplyMsg += ">\r\n";

            e.ReplyAsync(newReplyMsg.ToSecsMessage());

        }
    }
}
