using System;
using System.Collections.Generic;
using Wisdom.Utils.Driver;

namespace Wisdom.HomeAutomation.Sw16Drivers
{
    public class Sw16Driver : DriverBase
    {
        public Sw16Driver() : base(new Sw16Protocol())
        {
        }

        /// <summary>
        ///     打开所有按钮开关
        /// </summary>
        public void OpenAllButton()
        {
            Call(new byte[] {0x0A}, new byte[] {0x00}, new TimeSpan(7000000), true, null);
        }

        /// <summary>
        ///     关闭所有按钮开关
        /// </summary>
        public void CloseAllButton()
        {
            Call(new byte[] {0x0B}, new byte[] {0x00}, new TimeSpan(7000000), true, null);
        }

        /// <summary>
        ///     查询所有按钮开关
        ///     <para>return 按0-15顺序，1开，0关</para>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,bool> QueryAllButton()
        {
            var arrs =new Dictionary<int, bool>();
            var response = Call(new byte[] {0x1E}, new byte[] { }, new TimeSpan(7000000), true, null);
            var bytes = response.Data;
            for (var i = 0; i < 16; i++) arrs.Add(i, bytes[i + 2] == 0x01);
            return arrs;
        }

        /// <summary>
        ///     设置系统时间（未测试）
        /// </summary>
        /// <param name="time">时间，秒分时日月周年</param>
        public void ModifySystemTime(DateTime time)
        {
            byte week;
            if (time.DayOfWeek.Equals(0))
                week = 7;
            else
                week = (byte) time.DayOfWeek;
            Call(new byte[] {0x0D},
                new[]
                {
                    (byte) time.Second, (byte) time.Minute, (byte) time.Hour, (byte) time.Day, (byte) time.Month, week,
                    (byte) time.Year
                });
        }

        /// <summary>
        ///     查询系统时间
        /// </summary>
        public DateTime QuerySystemTime()
        {
            var response = Call(new byte[] {0x12}, new byte[] { }, new TimeSpan(7000000), true, null);
            var bytes = response.Data;
            var years = bytes[8] + 2000;
            var dateTime = new DateTime(years, bytes[6], bytes[5], bytes[4], bytes[3], bytes[2]);
            return dateTime;
        }

        /// <summary>
        ///     设置按钮开
        /// </summary>
        /// <param name="btnAddress">按钮地址(0-15)</param>
        public void OpenOneButton(int btnAddress)
        {
            if(btnAddress<0||btnAddress>15) throw new IndexOutOfRangeException("地址越界");
            Call(new byte[] {0x0F}, new byte[] {(byte) btnAddress, 0x01}, new TimeSpan(5000000), true, null);
        }

        /// <summary>
        ///     设置按钮关
        /// </summary>
        /// <param name="btnAddress">按钮编号（0-15）</param>
        public void CloseOneButton(int btnAddress)
        {
            if (btnAddress < 0 || btnAddress > 15) throw new IndexOutOfRangeException("地址越界");
            Call(new byte[] {0x10}, new byte[] {(byte) btnAddress, 0x02}, new TimeSpan(5000000), true, null);
        }

        /// <summary>
        ///     设置点动时间和点动标志
        /// </summary>
        /// <param name="btnAddress">按钮编号(0-15)</param>
        /// <param name="jogTime">点动时长（1-255秒）</param>
        public void InstallJogTime(int btnAddress, int jogTime)
        {
            if (btnAddress < 0 || btnAddress > 15||jogTime<1||jogTime>255) throw new IndexOutOfRangeException("地址越界或时间长度越界");
            Call(new byte[] {0x13}, new byte[] {(byte) btnAddress, (byte) jogTime, 0x01}, new TimeSpan(5000000), true,null);
        }

        /// <summary>
        ///     查询所有按钮的点动时间
        ///     <para>return: 按钮编号0-15顺序，点动时长，秒</para>
        /// </summary>
        /// <returns></returns>
        public int[] QueryAllJogTime()
        {
            var arr = new int[16];
            var response = Call(new byte[] {0x15}, new byte[] { }, new TimeSpan(5000000), true, null);
            var bytes = response.Data;
            for (var i = 0; i < 16; i++) arr[i] = bytes[i + 2];
            return arr;
        }

        /// <summary>
        ///     查询所有按钮点动还是定时模式
        ///     <para>return：0-15按顺序，1是定时，2是点动, 其他值是普通</para>
        /// </summary>
        /// <results></results>
        public int[] QueryJogOrTiming()
        {
            var arr = new int[16];
            var response = Call(new byte[] {0x1A}, new byte[] { }, new TimeSpan(5000000), true, null);
            var bytes = response.Data;
            for (var i = 0; i < 16; i++) arr[i] = bytes[i + 2];
            return arr;
        }

        /*
        /// <summary>
        /// 设置按钮定时开
        /// </summary>
        /// <param name="btnAddress">按钮编号</param>
        /// <param name="time">时间，时分周</param>
        /// <returns></returns>
        public bool InstallOpenTimeSwitch(int btnAddress, TimeSwitch time)
        {
            var response = Call(new Byte[] { 0x21 }, new Byte[] { (byte)btnAddress, (byte)time.Hour, (byte)time.Minute, (byte)time.Week }, new TimeSpan(5000000), true, null, null);
            if (response.Data[1].Equals(0x25))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        /// <summary>
        /// 设置按钮定时关
        /// </summary>
        /// <param name="btnAddress">按钮Byte地址</param>
        /// <param name="time">时间byte数组，时分周</param>
        /// <returns></returns>
        public bool InstallCloseTimeSwitch(int btnAddress, TimeSwitch time)
        {
            var response = Call(new Byte[] { 0x23 }, new Byte[] { (byte)btnAddress, (byte)time.Hour, (byte)time.Minute, (byte)time.Week }, new TimeSpan(5000000), true, null, null);
            if (response.Data[1].Equals(0x25))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 删除按钮定时开
        /// </summary>
        /// <param name="btnAddress">按钮编号</param>
        /// <param name="time">时间，时分周</param>
        /// <param name="saveAddress">存储地址（0-4）</param>
        public bool DeleteOpenTimeSwitch(int btnAddress, TimeSwitch time, int saveAddress)
        {
            var response = Call(new Byte[] { 0x28 }, new Byte[] { (byte)btnAddress, (byte)time.Hour, (byte)time.Minute, (byte)time.Week, (byte)saveAddress }, new TimeSpan(5000000), true, null, null);
            if (response.Data[1].Equals(0x2C))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 删除按钮定时关
        /// </summary>
        /// <param name="btnAddress">按钮编号</param>
        /// <param name="time">时间byte数组，时分周</param>
        ///  <param name="saveAddress">存储地址（0-4）</param>
        public bool DeleteCloseTimeSwitch(int btnAddress, TimeSwitch time, int saveAddress)
        {
            var response = Call(new Byte[] { 0x2A }, new Byte[] { (byte)btnAddress, (byte)time.Hour, (byte)time.Minute, (byte)time.Week, (byte)saveAddress }, new TimeSpan(5000000), true, null, null);
            if (response.Data[1].Equals(0x2C))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        */
        /// <summary>
        ///     设置按钮模式（点动，定时，普通）
        /// </summary>
        /// <param name="btnAddress">按钮编号</param>
        /// <param name="type">模式(1点动，2定时，3不设置)</param>
        public void InstallButtonType(int btnAddress, int type)
        {
            if (btnAddress < 0 || btnAddress > 15||type<0||type>3) throw new IndexOutOfRangeException("地址越界或模式类别越界");
            Call(new byte[] {0x26}, new[] {(byte) btnAddress, (byte) type}, new TimeSpan(5000000), true, null);
        }

        /*
        /// <summary>
        /// 查询按钮的定时时间  
        /// <para> (点动标志和模式协议没有写)</para>
        /// </summary>
        /// <param name="btnAddress">按钮编号</param>
        public MyButton QueryTimeSwitch(int btnAddress)
        {
            var myButtons = new MyButton();
            myButtons.OpenSwitchTime = new List<TimeSwitch>();
            myButtons.CloseSwitchTime = new List<TimeSwitch>();
            var response = Call(new Byte[] { 0x2D }, new Byte[] { (byte)btnAddress }, new TimeSpan(5000000), true, null, null);
            var bytes = response.Data;
            for (int i = 0; i < 5; i++)
            {
                var dateOpen = new TimeSwitch();//定时开启时间
                dateOpen.Week = bytes[5 + i * 3];
                dateOpen.Hour = bytes[3 + i * 3];
                dateOpen.Minute = bytes[4 + i * 3];
                if (dateOpen.Week.Equals(0))
                {
                    myButtons.OpenSwitchTime.Add(null);
                }
                else
                {
                    myButtons.OpenSwitchTime.Add(dateOpen);
                }
                var dateClose = new TimeSwitch();//定时关闭时间
                dateClose.Week = bytes[25 + i * 3];
                dateClose.Hour = bytes[23 + i * 3];
                dateClose.Minute = bytes[24 + i * 3];
                if (dateClose.Week.Equals(0))
                {
                    myButtons.CloseSwitchTime.Add(null);
                }
                else
                {
                    myButtons.CloseSwitchTime.Add(dateClose);
                }
            }
            return myButtons;
        }
        */
    }
}