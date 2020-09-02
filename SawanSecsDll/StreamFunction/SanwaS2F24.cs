using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace SawanSecsDll
{
    public partial class SanwaBaseExec
    {
        public Dictionary<string, SanwaTISThread> _tisThreadList = new Dictionary<string, SanwaTISThread>();

        public void ExecuteTraceData(SanwaTISThread sanwaTISThread)
        {
            string trid = sanwaTISThread._sanwaTIS._trid;

            _tisThreadList.TryGetValue(trid, out SanwaTISThread oldTISThreadObj);

            if (oldTISThreadObj != null)
            {
                //先停止並移除List
                oldTISThreadObj.isStopThread = true;
                _tisThreadList.Remove(trid);
            }

            if (!CheckTISData(sanwaTISThread._sanwaTIS)) return;

            //生成新的Thread
            Thread ExecuteS6F1Thread = new Thread(ExecuteTDSThread);
            sanwaTISThread._thread = ExecuteS6F1Thread;

            //加入List裡面
            _tisThreadList.Add(trid, sanwaTISThread);

            //啟用
            ExecuteS6F1Thread.Start(sanwaTISThread);

        }

        public async void ExecuteTDSThread(Object Obj)
        {
            SanwaTISThread ThreadObj = (SanwaTISThread)Obj;
            SanwaTIS sanwaTIS = ThreadObj._sanwaTIS;
            SanwaTDS sanwaTDS = new SanwaTDS
            {
                _trid = sanwaTIS._trid,
                _smpln = 0
            };

            while (!ThreadObj.isStopThread && sanwaTDS._smpln < sanwaTIS._totsmp)
            {
                //Data sample period
                Thread.Sleep(sanwaTIS._dsper);
                sanwaTDS._smpln++;  ////運算次數累加

                foreach (string svid in sanwaTIS._svIDList)
                {
                    _svList.TryGetValue(svid, out SanwaSV sanwaSV);

                    if (sanwaSV != null && sanwaSV._value != null)
                    {
                        SanwaSV svObj = new SanwaSV
                        {
                            _id = sanwaSV._id,
                            _value = sanwaSV._value,
                            _format = sanwaSV._format,
                            _name = sanwaSV._name,
                            _sVName = sanwaSV._sVName,
                            _unit = sanwaSV._unit,
                        };
                        sanwaTDS._svList.Add(svObj._id + "_"+ sanwaTDS._smpln.ToString(), svObj);
                    }
                }

                if (sanwaTDS._smpln % sanwaTIS._repgsz == 0) //不送出S6F1   
                {
                    //時間
                    sanwaTDS._stime = GetDateTime();

                    //送出S6F1
                    if(!IsOfflineState())
                    {
                        await SendS6F1Async(sanwaTDS);
                    }
                    //送出S6F1
                    sanwaTDS._svList.Clear();
                }
            }

            sanwaTDS._svList.Clear();

            //將從Thread List移除
            if (!ThreadObj.isStopThread)
                _tisThreadList.Remove(sanwaTIS._trid);
        }

        public bool CheckTISData(SanwaTIS sanwaTIS)
        {
            bool bRet = true;
            
            //取樣數目為零
            if (sanwaTIS._totsmp == 0) bRet = false;
            
            //SVID長度
            if (sanwaTIS._svIDList.Count == 0) bRet = false;

            return bRet;
        }
    }
}
