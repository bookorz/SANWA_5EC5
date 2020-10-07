using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReceiveS2F35(PrimaryMessageWrapper e, ref byte[] LRACK)
        {
            //Structure: L,2
            //            1. < DATAID >
            //            2.L,a # event
            //                1.L,2 #event 1
            //                    1. < CEID1>
            //                    2. L,b
            //                        1. <RPTID1>
            //                        .
            //                        .
            //                        b. <RPTIDb>

            //                a.L,2 #event a
            //                    1. < CEIDa >
            //                    2.L,c
            //                        1.<RPTID1>
            //                        .
            //                        .
            //                        c. <RPTIDc>
            LRACK[0] = SanwaACK.LRACK_ACK;

            Item EventListItem = e.Message.SecsItem.Items[1];
            //不符合資料格式 start ++
            if (EventListItem.Format != SecsFormat.List)
            {
                LRACK[0] = SanwaACK.LRACK_INVALID_FORMAT;
                return;
            }

            for (int i = 0; i < EventListItem.Count; i++)
            {
                Item CEIDItem = EventListItem.Items[i].Items[0];
                Item RPTIDListItem = EventListItem.Items[i].Items[1];

                if (!CheckFomart3x5x20(CEIDItem))
                {
                    LRACK[0] = SanwaACK.LRACK_INVALID_FORMAT;
                    return;
                }

                if (RPTIDListItem.Count > 0)
                {
                    SetItemToStringType(CEIDItem, out string ceid);

                    bool FindEventObj = false;
                    SanwaEvent eventObj = null;
                    foreach (SanwaEvent EvntObj in _eventList.Values)
                    {
                        if(EvntObj._id == ceid)
                        {
                            eventObj = EvntObj;
                            FindEventObj = true;
                            break;
                        }
                    }

                    if (!FindEventObj)
                    {
                        LRACK[0] = SanwaACK.LRACK_INVALID_FORMAT;
                        return;
                    }

                    //RPID List 清單已經滿了 
                    if (eventObj._rptidList.Count > 0)
                    {
                        LRACK[0] = SanwaACK.LRACK_CEID_DEFINED;
                        return;
                    }

                    for (int j = 0; j < RPTIDListItem.Count; j++)
                    {
                        Item RPTIDItem = RPTIDListItem.Items[j];

                        if (!CheckFomart3x5x20(RPTIDItem))
                        {
                            LRACK[0] = SanwaACK.LRACK_INVALID_FORMAT;
                            return;
                        }

                        SetItemToStringType(RPTIDItem, out string rptid);

                        _reportList.TryGetValue(rptid, out SanwaRPTID sanwaRPTID);

                        if (sanwaRPTID == null)
                        {
                            LRACK[0] = SanwaACK.LRACK_RPTID_NOT_EXIST;
                            return;
                        }
                    }
                }
            }
            //不符合資料格式 End ++

            if (EventListItem.Count == 0)
            {
                foreach (var eventObj in _eventList)
                {
                    SanwaEvent eventList = eventObj.Value;
                    eventList.ClearRPTIDList();
                }
            }

            for (int i = 0; i < EventListItem.Count; i++)
            {
                Item CEIDItem = EventListItem.Items[i].Items[0];
                Item RPTIDListItem = EventListItem.Items[i].Items[1];

                SetItemToStringType(CEIDItem, out string ceid);

                //_eventList.TryGetValue(ceid, out SanwaEvent eventObj);


                bool FindEventObj = false;
                SanwaEvent eventObj = null;
                foreach (SanwaEvent EvntObj in _eventList.Values)
                {
                    if (EvntObj._id == ceid)
                    {
                        eventObj = EvntObj;
                        FindEventObj = true;
                        break;
                    }
                }

                if(FindEventObj)
                {
                    if (RPTIDListItem.Count == 0)
                    {
                        eventObj.ClearRPTIDList();
                    }
                    else
                    {
                        for (int j = 0; j < RPTIDListItem.Count; j++)
                        {
                            Item RPTIDItem = RPTIDListItem.Items[j];
                            SetItemToStringType(RPTIDItem, out string rptid);
                            eventObj.AddRPTID(rptid);
                        }
                    }
                }

            }
        }
    }
}
