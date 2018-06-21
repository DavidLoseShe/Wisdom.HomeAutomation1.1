using System.Collections.Generic;
using System.Linq;
using Wisdom.Utils.Driver;

namespace Wisdom.HomeAutomation.Sw16Drivers
{
    internal class Sw16Protocol : ProtocolBase
    {
        public override IResponse CheckResponse(IRequest request, byte[] response)
        {
            //忽略上抛的时间帧
            while (response != null && response[1] == 0x1F) response = response.Skip(20).ToArray();
            var ret = new Response
            {
                Data = response
            };
            return ret;
        }

        protected override IEnumerable<byte> DoGenRequest(IRequest request)
        {
            var list = new List<byte> {0xAA};
            list.AddRange(request.Command);
            list.AddRange(request.Data);
            while (list.Count < 18) list.Add(0x00);
            list.AddRange(GenCheckBytes(list));
            list.Add(0xBB);
            return list;
        }
    }
}