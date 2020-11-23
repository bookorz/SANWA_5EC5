using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS16F6(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,4
            //    1. <DATAID>   
            //    2. <PRJOBID>   
            //    3. <PRCMDNAME>   
            //    4. L,n        
            //        1. L,2             
            //            1. <CPNAME1>             
            //            2. <CPVAL1>        
            //        .        
            //        .        
            //        n.L,2             
            //            1. <CPNAMEn>             
            //            2. <CPVALn>

            bool acka = SanwaACK.ACKA_SUCCESSFUL;
            ErrorStatus errorStatus;
            List<ErrorStatus> errorList = new List<ErrorStatus>();

            Item PRJOBIDItem = null;
            Item PRCMDNAMEItem = null;

            if (e.Message.SecsItem.Count != 4)
            {
                acka = SanwaACK.ACKA_UNSUCCESSFUL;
                errorStatus = new ErrorStatus
                {
                    Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                    Errortext = "Parameters improperly specified"
                };
                errorList.Add(errorStatus);

                _logger.Warn("ReplyS16F6_e.Message.SecsItem.Count != 4");
            }
            else
            {
                PRJOBIDItem = e.Message.SecsItem.Items[1];
                PRCMDNAMEItem = e.Message.SecsItem.Items[2];

                if (!CheckFomart20(PRJOBIDItem))
                {
                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                    errorStatus = new ErrorStatus
                    {
                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                        Errortext = "Parameters improperly specified"
                    };
                    errorList.Add(errorStatus);

                    _logger.Warn("ReplyS16F6_!CheckFomart20(PRJOBIDItem)");
                }
                else
                {
                    if (PRJOBIDItem.GetString().Length.Equals(0))
                    {
                        acka = SanwaACK.ACKA_UNSUCCESSFUL;
                        errorStatus = new ErrorStatus
                        {
                            Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                            Errortext = "PRJobID is empty"
                        };
                        errorList.Add(errorStatus);

                        _logger.Warn("ReplyS16F6_PRJOBIDItem.GetString().Length.Equals(0)");
                    }
                    else
                    {
                        bool IsMemberExists = _pJList.Exists(x => x.ObjID.Equals(PRJOBIDItem.GetString()));

                        if (!IsMemberExists)
                        {
                            acka = SanwaACK.ACKA_UNSUCCESSFUL;
                            errorStatus = new ErrorStatus
                            {
                                Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                Errortext = "PRJobID is empty"
                            };
                            errorList.Add(errorStatus);

                            _logger.Warn("ReplyS16F6_PRJOBIDItem.GetString().Length.Equals(0)");
                        }
                    }
                }
            }
        }
    }
}
