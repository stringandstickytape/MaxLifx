using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.Payload
{
    public interface IPayload
    {
        byte[] MessageType { get; }
        byte[] GetPayload();
    }
}
