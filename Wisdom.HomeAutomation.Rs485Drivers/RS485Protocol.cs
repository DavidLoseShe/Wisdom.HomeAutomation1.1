using System.Collections.Generic;
using Wisdom.Utils.Driver;
using Wisdom.Utils.Driver.Checker;
using Wisdom.Utils.Driver.ResponseChecker;

namespace Wisdom.HomeAutomation.Rs485Drivers
{
    internal class Rs485Protocol : ProtocolBase
    {
        public Rs485Protocol() : base(CheckerFacotry.GenFixed(8), null, CheckType.Crc16)
        {
        }


        public override IResponse CheckResponse(IRequest request, byte[] response)
        {
            var ret = new Response
            {
                Data = response
            };
            return ret;
        }

        protected override IEnumerable<byte> DoGenRequest(IRequest request)
        {
            var list = new List<byte>();
            list.AddRange(request.Address);
            list.AddRange(request.Command);
            list.AddRange(request.Data);
            list.AddRange(GenCheckBytes(list));
            return list;
        }
    }
}