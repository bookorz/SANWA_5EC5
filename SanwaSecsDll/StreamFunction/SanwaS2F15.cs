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
        public void ReceiveS2F15(PrimaryMessageWrapper e, ref byte [] ECA)
        {
            //L,n   
            //    1.L,2        
            //        1. < ECID1 > 
            //        2. < ECV1 > 
            //    2.L,2.
            //        1. < ECID2 > 
            //        2. < ECV2 > 
            //    .n.L,2        
            //        1. < ECIDn >
            //        2. < ECVn >

            ECA[0] = SanwaACK.EAC_ACK;
            //先比對所有訊息
            //比對異常就不能修改內容
            for(int i = 0; i< e.Message.SecsItem.Count; i++)
            {
                //確認格式是否正確
                if (e.Message.SecsItem.Items[i].Format != SecsFormat.List)
                {
                    ECA[0] = SanwaACK.EAC_DENIED_BY_SANWA_DEFINED;
                    return;
                }

                Item ECIDItem = e.Message.SecsItem.Items[i].Items[0];
                Item ECVItem = e.Message.SecsItem.Items[i].Items[1];

                if (ECIDItem == null ||
                    ECVItem == null)
                {
                    ECA[0] = SanwaACK.EAC_DENIED_BY_SANWA_DEFINED;
                    return;
                }

                if (!CheckFomart3x5x(ECIDItem))
                {
                    ECA[0] = SanwaACK.EAC_DENIED_BY_SANWA_DEFINED;
                    return;
                }

                SanwaEC Obj = FindECObjInECList(ECIDItem);

                if (Obj == null)
                {
                    ECA[0] = SanwaACK.EAC_DENIED_EC_NOT_EXIST;
                    return;
                }

                if (Obj._type != ECVItem.Format)
                {
                    ECA[0] = SanwaACK.EAC_DENIED_BY_SANWA_DEFINED;
                    return;
                }

                bool ecInRange = true;

                if (SecsFormat.I1 == ECVItem.Format)
                {
                    sbyte ec = FindECLimitValue(ECVItem ,out sbyte ecMax, out sbyte ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.I2 == ECVItem.Format)
                {
                    short ec = FindECLimitValue(ECVItem, out short ecMax, out short ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.I4 == ECVItem.Format)
                {
                    int ec = FindECLimitValue(ECVItem, out int ecMax, out int ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.I8 == ECVItem.Format)
                {
                    long ec = FindECLimitValue(ECVItem, out long ecMax, out long ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.U8 == ECVItem.Format)
                {
                    ulong ec = FindECLimitValue(ECVItem, out ulong ecMax, out ulong ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.U1 == ECVItem.Format)
                {
                    byte ec = FindECLimitValue(ECVItem, out byte ecMax, out byte ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.U2 == ECVItem.Format)
                {
                    ushort ec = FindECLimitValue(ECVItem, out ushort ecMax, out ushort ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.U4 == ECVItem.Format)
                {
                    uint ec = FindECLimitValue(ECVItem, out uint ecMax, out uint ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.F4 == ECVItem.Format)
                {
                    float ec = FindECLimitValue(ECVItem, out float ecMax, out float ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }
                else if (SecsFormat.F8 == ECVItem.Format)
                {
                    double ec = FindECLimitValue(ECVItem, out double ecMax, out double ecMin, Obj);
                    if (ec < ecMin || ec > ecMax) ecInRange = false;
                }

                if (!ecInRange)
                {
                    ECA[0] = SanwaACK.EAC_DENIED_EC_OUT_RANGE;
                    return;
                }
            }

            for (int i = 0; i < e.Message.SecsItem.Count; i++)
            {
                Item ECIDItem = e.Message.SecsItem.Items[i].Items[0];
                Item ECVItem = e.Message.SecsItem.Items[i].Items[1];

                //
                SanwaEC Obj = FindECObjInECList(ECIDItem);

                switch (ECVItem.Format)
                {
                    case SecsFormat.I1:
                        SetECByID(Obj._id, ECVItem.GetValue<sbyte>());
                        break;

                    case SecsFormat.I2:
                        SetECByID(Obj._id, ECVItem.GetValue<short>());
                        break;

                    case SecsFormat.I4:
                        SetECByID(Obj._id, ECVItem.GetValue<int>());
                        break;

                    case SecsFormat.I8:
                        SetECByID(Obj._id, ECVItem.GetValue<long>());
                        break;

                    case SecsFormat.U8:
                        SetECByID(Obj._id, ECVItem.GetValue<ulong>());
                        break;

                    case SecsFormat.U1:
                        SetECByID(Obj._id, ECVItem.GetValue<byte>());
                        break;

                    case SecsFormat.U2:
                        SetECByID(Obj._id, ECVItem.GetValue<ushort>());
                        break;

                    case SecsFormat.U4:
                        SetECByID(Obj._id, ECVItem.GetValue<uint>());
                        break;

                    case SecsFormat.F4:
                        SetECByID(Obj._id, ECVItem.GetValue<float>());
                        break;

                    case SecsFormat.F8:
                        SetECByID(Obj._id, ECVItem.GetValue<double>());
                        break;

                    case SecsFormat.ASCII:
                        SetECByID(Obj._id, ECVItem.GetString());
                        break;

                    case SecsFormat.JIS8:
                        SetECByID(Obj._id, ECVItem.GetString());
                        break;

                    case SecsFormat.Boolean:
                        SetECByID(Obj._id, ECVItem.GetValue<bool>());
                        break;

                    case SecsFormat.Binary:
                        SetECByID(Obj._id, ECVItem.GetValues<byte>());
                        break;
                }
            }
           
        }
        private T FindECLimitValue<T>(Item item, out T ecMax, out T ecMin, SanwaEC Obj)
        {
            ecMax = (T)Obj._maxValue;
            ecMin = (T)Obj._minValue;

            return item.GetValue<T>();
        }
        private SanwaEC FindECObjInECList(Item item)
        {
            string ecid = "";
            switch (item.Format)
            {
                case SecsFormat.I1: ecid = item.GetValue<sbyte>().ToString(); break;
                case SecsFormat.I2: ecid = item.GetValue<short>().ToString(); break;
                case SecsFormat.I4: ecid = item.GetValue<int>().ToString(); break;
                case SecsFormat.I8: ecid = item.GetValue<long>().ToString(); break;
                case SecsFormat.U8: ecid = item.GetValue<ulong>().ToString(); break;
                case SecsFormat.U1: ecid = item.GetValue<byte>().ToString(); break;
                case SecsFormat.U2: ecid = item.GetValue<ushort>().ToString(); break;
                case SecsFormat.U4: ecid = item.GetValue<uint>().ToString(); break;
            }
            _ecIDList.TryGetValue(ecid, out SanwaEC Obj);
            return Obj;
        }
       
    }
}
