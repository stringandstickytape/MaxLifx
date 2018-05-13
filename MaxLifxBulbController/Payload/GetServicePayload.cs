using MaxLifx.Controllers;

namespace MaxLifx.Payload
{    
    /// <summary>
    /// Payload for a GetService (discovery) message
    /// </summary>
    public class GetServicePayload : IPayload
    {
        private byte[] _messageType = new byte[2] { 0x2, 0 };
        public byte[] MessageType { get { return _messageType; } }
        public BulbType PayloadType { get; set; }

        public byte[] GetPayload()
        {
            return new byte[0];
        }
    }
}
