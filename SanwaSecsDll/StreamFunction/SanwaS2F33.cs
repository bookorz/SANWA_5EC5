using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public void ReceiveS2F33(PrimaryMessageWrapper e, ref byte[] DRACK)
        {
            //Structure: L,2
            //        1. < DATAID >
            //        2.L,a # reports
            //            1.L,2 report 1
            //                1. < RPTID1 >
            //                2.L,b #VIDs this report
            //                    1. < VID1 >
            //                    .
            //                    .
            //                    b.< VIDb >
            //            a.L,2 report a
            //                1. < RPTIDa >
            //                2.L,c #VIDs this report
            //                    1. < VID1 >
            //                    .
            //                    .
            //                    c. < VIDc >
            DRACK[0] = SanwaACK.DRACK_ACK;

            Item ReportListItem = e.Message.SecsItem.Items[1];

            //不符合資料格式 start ++
            if (ReportListItem.Format != SecsFormat.List)
            {
                DRACK[0] = SanwaACK.DRACK_INVALID_FORMAT;
                return; 
            }

            for (int i = 0; i < ReportListItem.Count; i++)
            {
                Item RPTIDItem = ReportListItem.Items[i].Items[0];
                Item VIDListItem = ReportListItem.Items[i].Items[1];

                if (!CheckFomart3x5x20(RPTIDItem))
                {
                    DRACK[0] = SanwaACK.DRACK_INVALID_FORMAT;
                    return;
                }

                if (VIDListItem.Count > 0)
                {
                    SetItemToStringType(RPTIDItem, out string rptid);
                    foreach (var reportObj in _reportList)
                    {
                        SanwaRPTID sanwaRPTID = reportObj.Value;

                        //RPTID 已經被定義
                        if (sanwaRPTID._id == rptid)
                        {
                            DRACK[0] = SanwaACK.DRACK_RPTID_DEFINED;
                            return;
                        }
                    }

                    for (int j = 0; j < VIDListItem.Count; j++)
                    {
                        Item VIDItem = VIDListItem.Items[j];

                        if (!CheckFomart3x5x20(VIDItem))
                        {
                            DRACK[0] = SanwaACK.DRACK_INVALID_FORMAT;
                            return;
                        }

                        SetItemToStringType(VIDItem, out string vid);

                        _svIDList.TryGetValue(vid, out SanwaSV svObj);
                        if(svObj != null)   continue;

                        _ecIDList.TryGetValue(vid, out SanwaEC ecObj);
                        if (ecObj != null) continue;

                        _dvIDList.TryGetValue(vid, out SanwaDV dvObj);
                        if (ecObj != null) continue;

                        DRACK[0] = SanwaACK.DRACK_VID_NOT_EXIST;
                        return;

                    }
                }
            }
            //不符合資料格式 End ++
            //是否停止移除所有Host定義的PRID
            if (ReportListItem.Count == 0)
            {
                foreach(var reportObj in _reportList)
                {
                    SanwaRPTID sanwaRPTID = reportObj.Value;
                    sanwaRPTID.Clear();
                }

                foreach (var eventObj in _eventList)
                {
                    SanwaEvent eventList = eventObj.Value;
                    eventList.ClearRPTIDList();
                }

                _reportList.Clear();
            }
            //寫入_reportList中
            for (int i = 0; i < ReportListItem.Count; i++)
            {
                Item RPTIDItem = ReportListItem.Items[i].Items[0];
                Item VIDListItem = ReportListItem.Items[i].Items[1];

                SetItemToStringType(RPTIDItem, out string rptid);

                if (VIDListItem.Count != 0 )
                {
                    SanwaRPTID rptObj = new SanwaRPTID
                    {
                        _id = rptid,
                        _format = RPTIDItem.Format
                    };

                    for (int j = 0; j < VIDListItem.Count; j++)
                    {
                        Item VIDItem = VIDListItem.Items[j];
                        SetItemToStringType(VIDItem, out string vid);

                        SanwaVID vidObj = new SanwaVID();
                        vidObj._id = vid;
                        vidObj._format = VIDItem.Format;

                        rptObj.Add(vidObj);
                    }

                    _reportList.Add(rptid, rptObj);

                }
                else//VIDListItem.Count
                {
                    bool RemoveRPTID = false;
                    foreach (var reportObj in _reportList)
                    {
                        SanwaRPTID sanwaRPTID = reportObj.Value;
                        if (sanwaRPTID._id == rptid)
                        {
                            sanwaRPTID.Clear();
                            RemoveRPTID = true;
                            break;
                        }
                    }

                    if (RemoveRPTID)
                    {
                        //移除_reportList中的實體物件
                        _reportList.Remove(rptid);
                        //移除eventList中的註冊
                        foreach (var sanwaEventObj in _eventList)
                        {
                            SanwaEvent sanwaEvent = sanwaEventObj.Value;
                            if (sanwaEvent._rptidList.Count > 0)
                                sanwaEvent._rptidList.RemoveAll(id => id == rptid);
                        }
                    }
                }
            }
        }
    }
}
