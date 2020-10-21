using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SanwaSecsDll
{
    /// <summary>
    /// Trace Initialize Send Obj
    /// </summary>
    public class SanwaTIS
    {
        public string _trid;
        public int _dsper;    //Data sample period
        public int _totsmp;   //Total samples to be made
        public int _repgsz;  //the reporting group size and corresponds to the number of time sample
        public List<string> _svIDList = new List<string>();
    }
    public class SanwaTISThread
    {
        public Thread _thread;
        public SanwaTIS _sanwaTIS;
        public bool isStopThread = false;
    }

    public partial class SanwaBaseExec
    {

        public bool ReceiveS2F23(PrimaryMessageWrapper e, ref byte[] TIAACK, ref SanwaTIS obj)
        {
            //L,5
            //    1. < TRID >
            //    2. < DSPER >
            //    3. < TOTSMP >
            //    4. < REPGSZ >
            //    5.L,n
            //        1. < SVID1 >
            //        2. < SVID2 >
            //        n. < SVIDn >

            //L,5
            //    1. < TRID >
            //    2. < DSPER >
            //    3. < TOTSMP >
            //    4. < REPGSZ >
            //    5. < SVID1 SVID2 SVID3 SVIDn>

            int TRIDIndex = -1;
            int DSPERIndex = -1;
            int TOTSMPIndex = -1;
            int REPGSZIndex = -1;
            int SVIDListIndex = 4;

            string SearchKey = "S2F23";
            _smlManager._messageList.TryGetValue(SearchKey, out SanwaSML smlObj);
            if(smlObj == null)  return false;

            string text = smlObj.Text;
            string line;
            using (StringReader reader = new StringReader(text))
            {
                int rowIndex = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("TRID"))
                    {
                        TRIDIndex = rowIndex;
                    }
                    else if(line.Contains("DSPER"))
                    {
                        DSPERIndex = rowIndex;
                    }
                    else if(line.Contains("TOTSMP"))
                    {
                        TOTSMPIndex = rowIndex;
                    }
                    else if(line.Contains("REPGSZ"))
                    {
                        REPGSZIndex = rowIndex;
                    }

                    if(line.Contains("L"))
                    {
                        rowIndex = 0;
                    }
                    else
                    {
                        rowIndex++;
                    }

                }
            }


             TIAACK[0] = SanwaACK.TIAACK_ACK;

            if (e.Message.SecsItem.Count != 5)
            {
                TIAACK[0] = SanwaACK.TIAACK_NO_MORE_TRACES_ALLOWED;
                return true;
            }

            if (TRIDIndex >= 0)
            {
                Item TRIDItem = e.Message.SecsItem.Items[TRIDIndex];

                if (!CheckFomart3x5x20(TRIDItem))
                {
                    TIAACK[0] = SanwaACK.TIAACK_INVALID_SVID;
                    return true;
                }

                SetItemToStringType(TRIDItem, out obj._trid);
            }

            if(DSPERIndex >= 0)
            {
                Item DSPERItem = e.Message.SecsItem.Items[1];
                if (DSPERItem.Format != SecsFormat.ASCII)
                {
                    TIAACK[0] = SanwaACK.TIAACK_INVALID_DSPER;
                    return true;
                }

                string _dsper = DSPERItem.GetString();

                if (!(_dsper.Length == 6 || _dsper.Length == 8))
                {
                    TIAACK[0] = SanwaACK.TIAACK_INVALID_DSPER;
                    return true;
                }

                string hh = _dsper.Substring(0, 2);
                string mm = _dsper.Substring(2, 2);
                string ss = _dsper.Substring(4, 2);
                string cc = _dsper.Length == 6 ? "0" : _dsper.Substring(6, 2);

                obj._dsper = Convert.ToInt32(hh) * 60 * 60 * 1000 +
                                Convert.ToInt32(mm) * 60 * 1000 +
                                Convert.ToInt32(ss) * 1000 +
                                Convert.ToInt32(cc);
            }

            if(TOTSMPIndex >= 0)
            {
                Item TOTSMPItem = e.Message.SecsItem.Items[2];

                if (!CheckFomart3x5x20(TOTSMPItem))
                {
                    TIAACK[0] = SanwaACK.TIAACK_NO_MORE_TRACES_ALLOWED;
                    return true;
                }

                SetItemToStringType(TOTSMPItem, out string totsmp);

                obj._totsmp = Convert.ToInt32(totsmp);
            }

            if(REPGSZIndex >=0 )
            {
                Item REPGSZItem = e.Message.SecsItem.Items[3];

                if (!CheckFomart3x5x20(REPGSZItem))
                {
                    TIAACK[0] = SanwaACK.TIAACK_INVALID_REPGSZ;
                    return true;
                }

                SetItemToStringType(REPGSZItem, out string repgsz);

                if (repgsz == "0")
                {
                    TIAACK[0] = SanwaACK.TIAACK_INVALID_REPGSZ;
                    return true;
                }

                obj._repgsz = Convert.ToInt32(repgsz);
            }

            if(SVIDListIndex >= 0)
            {
                //型式1與型式二
                Item SVIDListItem = e.Message.SecsItem.Items[SVIDListIndex];
                if (SVIDListItem.Format == SecsFormat.List)
                {
                    for (int i = 0; i < SVIDListItem.Count; i++)
                    {
                        Item SVItem = SVIDListItem.Items[i];

                        if (!CheckFomart3x5x20(SVItem))
                        {
                            TIAACK[0] = SanwaACK.TIAACK_INVALID_SVID;
                            return true;
                        }

                        SetItemToStringType(SVItem, out string svid);

                        _svIDList.TryGetValue(svid, out SanwaSV sanwaSV);

                        if (sanwaSV == null)
                        {
                            TIAACK[0] = SanwaACK.TIAACK_INVALID_SVID;
                            return true;
                        }
                        else
                        {
                            obj._svIDList.Add(svid);
                        }
                    }
                }
                else
                {
                    if (SVIDListItem.Format == SecsFormat.I1)
                    {
                        sbyte[] array = new sbyte[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<sbyte>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.I2)
                    {
                        short[] array = new short[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<short>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.I4)
                    {
                        int[] array = new int[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<int>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.I8)
                    {
                        long[] array = new long[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<long>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.U1)
                    {
                        byte[] array = new byte[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<byte>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.U2)
                    {
                        ushort[] array = new ushort[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<ushort>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.U4)
                    {
                        uint[] array = new uint[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<uint>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                    else if (SVIDListItem.Format == SecsFormat.U8)
                    {
                        ulong[] array = new ulong[SVIDListItem.Count];
                        array = SVIDListItem.GetValues<ulong>();
                        for (int i = 0; i < SVIDListItem.Count; ++i)
                        {
                            obj._svIDList.Add(array[i].ToString());
                        }
                    }
                }
            }
            return true;
        }
    }
}
