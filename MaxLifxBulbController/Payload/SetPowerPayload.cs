using System;
using System.Linq;

namespace MaxLifx.Payload
{
    /// <summary>
    /// Payload for a GetLabel message
    /// </summary>
    public class SetPowerPayload : IPayload
    {
        private byte[] _messageType = new byte[2] { 21, 0 };
        public byte[] MessageType { get { return _messageType; } }

        public bool PowerState;

        public SetPowerPayload(bool powerState)
        {
            PowerState = powerState;
        }

        public byte[] GetPayload()
        {
            ushort x = (ushort)(PowerState ? 65535 : 0);

            return new byte[0].Concat(BitConverter.GetBytes(x)).ToArray();
        }
    }
}
