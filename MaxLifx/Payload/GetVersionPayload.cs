using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.Payload
{
    public class GetVersionPayload : IPayload
    // getting the version info will let us know if light has zones (Beam or Z)
    {
        private byte[] _messageType = new byte[2] { 0x20, 0 };
        public byte[] MessageType { get { return _messageType; } }

        public byte[] GetPayload()
        {
            return new byte[0];
        }
    }
}
