using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SanwaSecsDll
{
    public partial class SanwaBaseExec
    {
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("kernel32.dll")]
        private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("Kernel32.dll")]
        //public static extern bool SetLocalTime(ref SYSTEMTIME sysTime);
        public extern static bool SetLocalTime(ref SYSTEMTIME sysTime);

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        public extern static void Win32GetSystemTime(ref SYSTEMTIME sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static bool Win32SetSystemTime(ref SYSTEMTIME sysTime);

        public void ReceiveS2F31(PrimaryMessageWrapper e, ref byte[] TIACK)
        {
            //確認格式是否正確
            if (e.Message.SecsItem.Format != SecsFormat.ASCII)
            {
                TIACK[0] = SanwaACK.TIACK_ERROR;
                return;
            }

            string DataTime = e.Message.SecsItem.GetString();

            if (!(DataTime.Length == 12 || DataTime.Length == 14 ||
                DataTime.Length == 16 || DataTime.Length == 19))
            {
                TIACK[0] = SanwaACK.TIACK_ERROR;
                return;
            }

            SYSTEMTIME systime = new SYSTEMTIME();
            SYSTEMTIME hosttime = new SYSTEMTIME();
            Win32GetSystemTime(ref systime);//不先取得時間就修改不了

            switch (DataTime.Length)
            {
                case 12:
                    systime.wYear = Convert.ToUInt16("20" + e.Message.SecsItem.GetString().Substring(0, 2));
                    systime.wMonth = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(2, 2));
                    systime.wDay = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(4, 2));
                    systime.wHour = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(6, 2));
                    systime.wMinute = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(8, 2));
                    systime.wSecond = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(10, 2));
                    break;

                case 14:
                    systime.wYear = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(0, 4));
                    systime.wMonth = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(4, 2));
                    systime.wDay = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(6, 2));
                    systime.wHour = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(8, 2));
                    systime.wMinute = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(10, 2));
                    systime.wSecond = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(12, 2));
                    break;

                case 16:
                    systime.wYear = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(0, 4));
                    systime.wMonth = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(4, 2));
                    systime.wDay = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(6, 2));
                    systime.wHour = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(8, 2));
                    systime.wMinute = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(10, 2));
                    systime.wSecond = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(12, 2));
                    systime.wMilliseconds = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(14, 2));
                    break;
                case 19:
                    systime.wYear = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(0, 4));
                    systime.wMonth = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(5, 2));
                    systime.wDay = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(8, 2));
                    systime.wHour = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(11, 2));
                    systime.wMinute = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(14, 2));
                    systime.wSecond = Convert.ToUInt16(e.Message.SecsItem.GetString().Substring(17, 2));
                    break;
            }

            try
            {
                hosttime = systime;

                Win32SetSystemTime(ref systime);
                Win32GetSystemTime(ref systime);

                string datetime = GetDateTime();
                _svList.TryGetValue(SVName.GEM_CLOCK, out SanwaSV svObj);

                if (svObj != null) svObj._value = datetime;

                //避免跨小時
                if (systime.wHour == hosttime.wHour)
                { 
                    if (Math.Abs(systime.wMinute - hosttime.wMinute) > 1)
                    {
                        TIACK[0] = SanwaACK.TIACK_ERROR;
                        return;
                    }
                }

                //避免跨天
                if(systime.wDay == hosttime.wDay)
                { 
                    if (Math.Abs(systime.wHour - hosttime.wHour) > 0)
                    {
                        TIACK[0] = SanwaACK.TIACK_ERROR;
                        return;
                    }
                }
                
                //避免跨月
                if(systime.wMonth == hosttime.wMonth)
                { 
                    if (Math.Abs(systime.wDay - hosttime.wDay) > 0)
                    {
                        TIACK[0] = SanwaACK.TIACK_ERROR;
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}
