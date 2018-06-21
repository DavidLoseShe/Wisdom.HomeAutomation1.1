using System;
using System.Collections.Generic;
using Wisdom.Utils.Driver;
using Wisdom.Utils.Driver.Arg;

namespace Wisdom.HomeAutomation.Rs485Drivers
{
    public class Rs485Driver : DriverBase
    {
        public Rs485Driver() : base(new Rs485Protocol(),new Rs485ProtocolRead())
        {

           
        }

        /// <summary>
        ///     设置32路继电器输出状态（字写）
        /// </summary>
        /// <param name="arrAddress">key对应继电器地址，value值true输出，false不输出</param>
        public void Install32RelayOutputState(Dictionary<int, bool> arrAddress)
        {
            var arr = new int[32];
            foreach (var item in arrAddress)
            {
                var adr = item.Key;
                if (adr < 0 || adr > 32) throw new IndexOutOfRangeException("按钮地址越界");
                arr[adr] = item.Value ? 1 : 0;
            }

            var by1 = 0;
            var by2 = 0;
            var by3 = 0;
            var by4 = 0;
            for (var i = 0; i < 7; i++)
            {
                by1 = arr[i] * (int) Math.Pow(2, i) + by1;
                by2 = arr[i + 8] * (int) Math.Pow(2, i) + by2;
                by3 = arr[i + 16] * (int) Math.Pow(2, i) + by3;
                by4 = arr[i + 24] * (int) Math.Pow(2, i) + by4;
            }

            Call(new byte[] {0x01}, new byte[] {0x10},
                new byte[] {0x80, 0x50, 0x00, 0x02, 0x04, (byte) by2, (byte) by1, (byte) by4, (byte) by3});
        }

        /// <summary>
        ///     设置指定继电器输出状态（位写）
        /// </summary>
        /// <param name="relayAddress">范围(0-31)</param>
        /// <param name="state">true对应开，false对应关闭</param>
        public void InstallRelayOutputState(int relayAddress, bool state)
        {
            if (relayAddress < 0 || relayAddress > 32) throw new IndexOutOfRangeException("按钮地址越界");
            var states = state ? (byte) 0xFF : (byte) 0x00;
            Call(new byte[] {0x01}, new byte[] {0x05}, new byte[] {0x00, (byte) relayAddress, states, 0x00});
        }

        /// <summary>
        ///     读多路继电器输出状态
        ///     <para>return:key对应地址，true输出，false不输出</para>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, bool> Read32RelayOutputState()
        {
            var arr = new Dictionary<int, bool>();
            var response = Call(new byte[] {0x01}, new byte[] {0x01}, new byte[] {0x00, 0x00, 0x00, 0x20},null,null,null,"Read");
            var bytes = response.Data;
            int a1 = bytes[3];
            int a2 = bytes[4];
            int a3 = bytes[5];
            int a4 = bytes[6];
            for (var i = 0; i < 8; i++)
            {
                arr.Add(i, a1 % 2 == 1);
                arr.Add(i + 8, a2 % 2 == 1);
                arr.Add(i + 16, a3 % 2 == 1);
                arr.Add(i + 24, a4 % 2 == 1);
                a1 = a1 / 2;
                a2 = a2 / 2;
                a3 = a3 / 2;
                a4 = a4 / 2;
            }

            return arr;
        }

        /// <summary>
        ///     设置波特率
        /// </summary>
        /// <param name="bps">范围（0-2）对应波特率19200,9600,115200</param>
        public void InstallBps(int bps)
        {
            if (bps < 0 || bps > 2) throw new IndexOutOfRangeException("波特率设置方案范围超限");

            Call(new byte[] {0x00}, new byte[] {0x06}, new byte[] {0x00, 0x00, (byte) bps, 0x01});
        }
    }

}