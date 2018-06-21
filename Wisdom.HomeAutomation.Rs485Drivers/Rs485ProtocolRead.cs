using System.Collections.Generic;
using Wisdom.Utils.Driver;
using Wisdom.Utils.Driver.Checker;
using Wisdom.Utils.Driver.ResponseChecker;

namespace Wisdom.HomeAutomation.Rs485Drivers
{
    class Rs485ProtocolRead : ProtocolBase
    { 
        public Rs485ProtocolRead() : base(CheckerFacotry.GenFixed(9), null, CheckType.Crc16,null,"Read")
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
