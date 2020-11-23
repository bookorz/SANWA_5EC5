using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReplyS16F12(PrimaryMessageWrapper e, SecsMessage receiveMsg, SecsMessage replyMsg)
        {
            //L,7   
            //    1.  < DATAID > 
            //    2.  < PRJOBID > 
            //    3.  < MF > 
            //    4a.L,n[MF = carrier, n = # of carriers]         
            //        1. L,2              
            //            1. <CARRIERID1>              
            //            2. L,j   [j = # of slots, may be implemented as an array]                   
            //                1. <SLOTID1>                                                     
            //                .                   
            //                j. <SLOTIDj>         
            //        .         
            //        n. L,2              
            //            1. <CARRIERIDn>              
            //            2. L,j   [j = # of slots, may be implemented as an array]                   
            //                1. <SLOTID1>                   
            //                .                  
            //                j. <SLOTIDj>   
            //    4b. L,n    [MF = substrate]         
            //        1. <MID1>         
            //        .         
            //        n. <MIDn>   
            //    5.  L,3         
            //        1. <PRRECIPEMETHOD>         
            //        2. <RCPSPEC> or <PPID>         
            //        3. L,m   [m = # recipe parameters]              
            //            1. L,2                   
            //                1. <RCPPARNM1>                   
            //                2. <RCPPARVAL1>              
            //            .              
            //            m. L,2                   
            //                1. <RCPPARNMm>                   
            //                2. <RCPPARVALm>   
            //    6. <PRPROCESSSTART>   
            //    7. <PRPAUSEEVENT>

            // The list for specifying material (item 4a and 4b) is empty (L, 0 instead of L, n), 
            // when no material is specified for the Process Job.
            // The form of data item 4(a or b) depends on the value in MF.
            // If an implementer used PPID for this message, 
            // the format of PPID shall be ASCII because RecID is defined as the text in SEMI E40.

            //L,2    
            //    1. < PRJOBID > 
            //    2.L,2         
            //        1. < ACKA > 
            //        2.L,n(n = { 0,n})              
            //            1.L,2                   
            //                1. < ERRCODE1 > 
            //                2. < ERRTEXT1 >              
            //            .              
            //            n.L,2                   
            //                1. < ERRCODEn > 
            //                2. < ERRTEXTn >

            //檢查相關格式 Start
            bool acka = SanwaACK.ACKA_SUCCESSFUL;
            ErrorStatus errorStatus;
            List<ErrorStatus> errorList = new List<ErrorStatus>();

            Item PRJOBIDItem = null;
            Item MFItem = null;
            Item MFItemList = null;
            Item PPIDItemList = null;
                Item PRRECIPEMETHODItem = null;
                Item PPIDItem = null;
                Item RCPPARItemList = null;
            Item PRPROCESSSTARTItem = null;
            Item PRPAUSEEVENTItem = null;

            if (e.Message.SecsItem.Count != 7)
            {
                acka = SanwaACK.ACKA_UNSUCCESSFUL;
                errorStatus = new ErrorStatus
                {
                    Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                    Errortext = "Parameters improperly specified"
                };
                errorList.Add(errorStatus);

                _logger.Warn("ReplyS6F12_e.Message.SecsItem.Count != 7");
            }
            else
            {
                PRJOBIDItem = e.Message.SecsItem.Items[1];
                MFItem = e.Message.SecsItem.Items[2];
                MFItemList = e.Message.SecsItem.Items[3];
                PPIDItemList = e.Message.SecsItem.Items[4];
                PRPROCESSSTARTItem = e.Message.SecsItem.Items[5];
                PRPAUSEEVENTItem = e.Message.SecsItem.Items[6];

                if (!CheckFomart20(PRJOBIDItem))
                {
                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                    errorStatus = new ErrorStatus
                    {
                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                        Errortext = "Parameters improperly specified"
                    };
                    errorList.Add(errorStatus);

                    _logger.Warn("ReplyS6F12_!CheckFomart20(PRJOBIDItem)");
                }
                else
                {
                    if(PRJOBIDItem.GetString().Length.Equals(0))
                    {
                        acka = SanwaACK.ACKA_UNSUCCESSFUL;
                        errorStatus = new ErrorStatus
                        {
                            Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                            Errortext = "PRJobID is empty"
                        };
                        errorList.Add(errorStatus);

                        _logger.Warn("ReplyS6F12_PRJOBIDItem.GetString().Length.Equals(0)");
                    }
                    else
                    {
                        bool IsMemberExists = _pJList.Exists(x => x.ObjID.Equals(PRJOBIDItem.GetString()));

                        if(IsMemberExists)
                        {
                            acka = SanwaACK.ACKA_UNSUCCESSFUL;
                            errorStatus = new ErrorStatus
                            {
                                Errorcode = SanwaACK.ERRCODE_OBJECT_IDENTIFIER_IN_USE,
                                Errortext = "PRJobID identifier in use"
                            };
                            errorList.Add(errorStatus);

                            _logger.Warn("ReplyS6F12_Object identifier in use");
                        }
                    }
                }

                if (!CheckFomart1020(MFItem))
                {
                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                    errorStatus = new ErrorStatus
                    {
                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                        Errortext = "Parameters improperly specified"
                    };
                    errorList.Add(errorStatus);

                    _logger.Warn("ReplyS6F12_!CheckFomart1020(MFItem)");
                }
                else
                {
                    if (MFItem.Format == SecsFormat.Binary)
                    {
                        if(MFItem.Count != 0)
                        {
                            byte[] mf = MFItem.GetValues<byte>();

                            if (mf[0] != (byte)E30_MF.IN_CARRIERS &&
                                mf[0] != (byte)E30_MF.IN_SUBSTRATES)
                            {
                                acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                errorStatus = new ErrorStatus
                                {
                                    Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                    Errortext = "Parameters improperly specified"
                                };
                                errorList.Add(errorStatus);

                                _logger.Warn("ReplyS6F12_(mf[0] != (byte)E30_MF.IN_CARRIERS && mf[0] != (byte)E30_MF.IN_SUBSTRATES)");
                            }
                            else
                            {
                                if (MFItemList.Format != SecsFormat.List)
                                {
                                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                    errorStatus = new ErrorStatus
                                    {
                                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                        Errortext = "Parameters improperly specified"
                                    };
                                    errorList.Add(errorStatus);


                                    _logger.Warn("ReplyS6F12_MFItemList.Format != SecsFormat.List");
                                }
                                else
                                {
                                    if (mf[0] == E30_MF.IN_CARRIERS)
                                    {
                                        for (int i = 0; i < MFItemList.Count; i++)
                                        {
                                            Item item = MFItemList.Items[i];

                                            if (item.Format != SecsFormat.List)
                                            {
                                                acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                                errorStatus = new ErrorStatus
                                                {
                                                    Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                                    Errortext = "Parameters improperly specified"
                                                };
                                                errorList.Add(errorStatus);

                                                _logger.Warn("ReplyS6F12_item.Format != SecsFormat.List");
                                            }
                                            else
                                            {
                                                Item CarrierItem = item.Items[0];
                                                Item SlotIDItemList = item.Items[1];

                                                if (!CheckFomart20(CarrierItem) || SlotIDItemList.Format != SecsFormat.List)
                                                {
                                                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                                    errorStatus = new ErrorStatus
                                                    {
                                                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                                        Errortext = "Parameters improperly specified"
                                                    };
                                                    errorList.Add(errorStatus);

                                                    _logger.Warn("ReplyS6F12_!CheckFomart20(CarrierItem) || SlotIDItemList.Format != SecsFormat.List");
                                                }
                                                else if(CarrierItem.GetString().Equals(0))
                                                {
                                                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                                    errorStatus = new ErrorStatus
                                                    {
                                                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                                        Errortext = "Carrier is empty"
                                                    };
                                                    errorList.Add(errorStatus);

                                                    _logger.Warn("ReplyS6F12_CarrierItem.GetString().Equals(0)");

                                                }
                                                else
                                                {
                                                    for (int j = 0; j < SlotIDItemList.Count; j++)
                                                    {
                                                        Item slotItem = SlotIDItemList.Items[j];

                                                        if (slotItem.Format != SecsFormat.U1)
                                                        {
                                                            acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                                            errorStatus = new ErrorStatus
                                                            {
                                                                Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                                                Errortext = "Parameters improperly specified"
                                                            };
                                                            errorList.Add(errorStatus);

                                                            _logger.Warn("ReplyS6F12_slotItem.Format != SecsFormat.U1");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < MFItemList.Count; i++)
                                        {
                                            Item item = MFItemList.Items[i];

                                            if (item.Format != SecsFormat.ASCII)
                                            {
                                                acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                                errorStatus = new ErrorStatus
                                                {
                                                    Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                                    Errortext = "Parameters improperly specified"
                                                };
                                                errorList.Add(errorStatus);
                                                _logger.Warn("ReplyS6F12_item.Format != SecsFormat.ASCII");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Items with format 20 will be a unit identifier for one of the special SECS generic units, 
                        //as specified in § 12
                    }
                }

                if (PPIDItemList.Format != SecsFormat.List)
                {
                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                    errorStatus = new ErrorStatus
                    {
                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                        Errortext = "Parameters improperly specified"
                    };
                    errorList.Add(errorStatus);
                    _logger.Warn("ReplyS6F12_PPIDItemList.Format != SecsFormat.List");
                }
                else
                {
                    PRRECIPEMETHODItem = PPIDItemList.Items[0];
                    PPIDItem = PPIDItemList.Items[1];
                    RCPPARItemList = PPIDItemList.Items[2];

                    //bool mFFormatErrror = false;
                    if (PRRECIPEMETHODItem.Format != SecsFormat.U1)
                    {
                        acka = SanwaACK.ACKA_UNSUCCESSFUL;
                        errorStatus = new ErrorStatus
                        {
                            Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                            Errortext = "Parameters improperly specified"
                        };
                        errorList.Add(errorStatus);

                        _logger.Warn("ReplyS6F12_PRRECIPEMETHODItem.Format != SecsFormat.U1");
                    }

                    if (PPIDItem.Format != SecsFormat.ASCII)
                    {
                        acka = SanwaACK.ACKA_UNSUCCESSFUL;
                        errorStatus = new ErrorStatus
                        {
                            Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                            Errortext = "Parameters improperly specified"
                        };
                        errorList.Add(errorStatus);

                        _logger.Warn("ReplyS6F12_PPIDItem.Format != SecsFormat.ASCII");
                    }

                    if (RCPPARItemList.Format != SecsFormat.List)
                    {
                        acka = SanwaACK.ACKA_UNSUCCESSFUL;
                        errorStatus = new ErrorStatus
                        {
                            Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                            Errortext = "Parameters improperly specified"
                        };
                        errorList.Add(errorStatus);

                        _logger.Warn("ReplyS6F12_RCPPARItemList.Format != SecsFormat.List");
                    }
                    else
                    {
                        for (int i = 0; i < RCPPARItemList.Count; i++)
                        {
                            Item RCPPARNMItem = RCPPARItemList.Items[0];
                            Item RCPPARVALItem = RCPPARItemList.Items[1];

                            if (RCPPARNMItem.Format != SecsFormat.ASCII)
                            {
                                acka = SanwaACK.ACKA_UNSUCCESSFUL;
                                errorStatus = new ErrorStatus
                                {
                                    Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                                    Errortext = "Parameters improperly specified"
                                };
                                errorList.Add(errorStatus);

                                _logger.Warn("ReplyS6F12_RCPPARNMItem.Format != SecsFormat.ASCII");
                            }
                        }
                    }
                }

                if(PRPROCESSSTARTItem.Format != SecsFormat.Boolean)
                {
                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                    errorStatus = new ErrorStatus
                    {
                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                        Errortext = "Parameters improperly specified"
                    };
                    errorList.Add(errorStatus);

                    _logger.Warn("ReplyS6F12_PRPROCESSSTARTItem.Format != SecsFormat.Boolean");
                }

                if(PRPAUSEEVENTItem.Format != SecsFormat.List)
                {
                    acka = SanwaACK.ACKA_UNSUCCESSFUL;
                    errorStatus = new ErrorStatus
                    {
                        Errorcode = SanwaACK.ERRCODE_PARAMETERS_IMPROPERLY_SPECIFIED,
                        Errortext = "Parameters improperly specified"
                    };
                    errorList.Add(errorStatus);

                    _logger.Warn("ReplyS6F12_PRPAUSEEVENTItem.Format != SecsFormat.List");
                }
            }
            //檢查相關格式 End

            string prJobID = "";

            if(PRJOBIDItem != null)
            {
                if(PRJOBIDItem.Format == SecsFormat.ASCII)
                {
                    prJobID = PRJOBIDItem.GetString();
                }
            }

            string newReplyMsg = GetMessageName(replyMsg.ToSml());
            newReplyMsg += "<L[2]\r\n";
            newReplyMsg += "<Boolean[1]" + acka.ToString() + ">\r\n";
            newReplyMsg += "<L[" + errorList.Count + "]\r\n";
            for(int i = 0; i< errorList.Count; i++)
            {
                errorStatus = errorList[i];
                newReplyMsg += GetToSMLItem(SecsFormat.U1, errorStatus.Errorcode.ToString());
                newReplyMsg += "< A[0]" + errorStatus.Errortext + ">\r\n";
            }
            newReplyMsg += ">\r\n";
            newReplyMsg += ">\r\n";

            if(acka == SanwaACK.ACKA_SUCCESSFUL)
            {
                SanwaPJ sanwaPJ = new SanwaPJ
                {
                    ObjID = prJobID
                };

                for (int i = 0; i< PRPAUSEEVENTItem.Count; i++)
                {
                    Item EventItem = PRPAUSEEVENTItem.Items[i];

                    SetItemToStringType(EventItem, out string eventId);

                    if(eventId != "")
                        sanwaPJ.PauseEvent.Add(eventId);
                }

                if (MFItem.Count != 0)
                {
                    byte[] mf = MFItem.GetValues<byte>();
                    sanwaPJ.PRMtlType = mf[0];

                    if (sanwaPJ.PRMtlType == E30_MF.IN_CARRIERS)
                    {
                        //object carrierList;
                        Dictionary<string, List<string>> carrierIDList = new Dictionary<string, List<string>>();
                        for (int n = 0; n < MFItemList.Count; n++)
                        {
                            Item CarrierIDList = MFItemList.Items[n];

                            Item CarrierIDItem = CarrierIDList.Items[0];
                            Item SlotIDList = CarrierIDList.Items[1];

                            SetItemToStringType(CarrierIDItem, out string carrierID);

                            List<string> slotList = new List<string>();
                            AddToPRMtlNameList(SlotIDList, slotList);

                            carrierIDList.Add(carrierID, slotList);
                        }
                        sanwaPJ.PRMtlNameList = carrierIDList;

                    }
                    else if (sanwaPJ.PRMtlType == E30_MF.IN_SUBSTRATES)
                    {
                        //object Substrates;
                        List<string> mIDList = new List<string>();
                        for (int n = 0; n < MFItemList.Count; n++)
                        {
                            Item MIDItem = MFItemList.Items[n];

                            SetItemToStringType(MIDItem, out string materialID);

                            mIDList.Add(materialID);
                        }
                        sanwaPJ.PRMtlNameList = mIDList;
                    }
                }

                if(PRRECIPEMETHODItem != null)
                {
                    sanwaPJ.PRRecipeMethod = (E40_PR_RECIPE_METHOD)PRRECIPEMETHODItem.GetValue<byte>();
                }

                if(PPIDItem != null)
                {
                    sanwaPJ.RecID = PPIDItem.GetString();
                }

                if(RCPPARItemList != null)
                {
                    for(int m = 0; m< RCPPARItemList.Count; m++)
                    {
                        Item RCPPARItem = RCPPARItemList.Items[m];
                        Item RCPPARNMItem = null;
                        Item RCPPARVALItem = null;
                        if (RCPPARItem.Count.Equals(2))
                        {
                            RCPPARNMItem = RCPPARItem.Items[0];
                            RCPPARVALItem = RCPPARItem.Items[1];

                            RecipeVariable recipeVariable = new RecipeVariable
                            {
                                RecipeVarName = RCPPARNMItem.GetString()
                            };
                            switch (RCPPARVALItem.Format)
                            {
                                case SecsFormat.ASCII:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetString();
                                    break;
                                case SecsFormat.Binary:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValues<byte>();
                                    break;
                                case SecsFormat.Boolean:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<bool>();
                                    break;
                                case SecsFormat.F4:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<float>();
                                    break;
                                case SecsFormat.F8:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<double>();
                                    break;
                                case SecsFormat.I1:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<sbyte>();
                                    break;
                                case SecsFormat.I2:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<short>();
                                    break;
                                case SecsFormat.I4:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<int>();
                                    break;
                                case SecsFormat.I8:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<long>();
                                    break;
                                case SecsFormat.U1:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<byte>();
                                    break;
                                case SecsFormat.U2:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<ushort>();
                                    break;
                                case SecsFormat.U4:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<uint>();
                                    break;
                                case SecsFormat.U8:
                                    recipeVariable.RecipeVarValue = RCPPARVALItem.GetValue<ulong>();
                                    break;
                                default:
                                    ///case SecsFormat.List:
                                    break;
                            }

                            sanwaPJ.RecVariableList.Add(recipeVariable.RecipeVarName, recipeVariable);
                        }
                    }
                }

                if(PRPROCESSSTARTItem != null)
                {
                    sanwaPJ.PRProcessStart = PRPROCESSSTARTItem.GetValue<bool>();
                }

                _pJList.Add(sanwaPJ);
                _pJQueue.Add(sanwaPJ);
            }

            e.ReplyAsync(newReplyMsg.ToSecsMessage());
        }
        public void AddToPRMtlNameList(Item itemlist, List<string> nameList)
        {
            for (int m = 0; m < itemlist.Count; m++)
            {
                Item IDItem = itemlist.Items[m];
                switch (IDItem.Format)
                {
                    case SecsFormat.ASCII:
                        nameList.Add(IDItem.GetString());
                        break;

                    case SecsFormat.U1:
                        nameList.Add(IDItem.GetValue<byte>().ToString());
                        break;

                    case SecsFormat.U2:
                        nameList.Add(IDItem.GetValue<ushort>().ToString());
                        break;

                    case SecsFormat.U4:
                        nameList.Add(IDItem.GetValue<uint>().ToString());
                        break;

                    case SecsFormat.U8:
                        nameList.Add(IDItem.GetValue<ulong>().ToString());
                        break;
                }
            }
        }
        
    }


}
