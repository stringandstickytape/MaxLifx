using System;
using System.Linq;

namespace MaxLifx.Payload
{
    /// <summary>
    /// Payload for a SetColour message
    /// </summary>
    public class SetMaxLifxColourPayload : SetColourPayload, IPayload
    {
        private byte[] _messageType = new byte[2] { 0xF5, 1 };
    }
}
