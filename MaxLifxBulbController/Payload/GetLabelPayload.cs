
namespace MaxLifx.Payload
{
    /// <summary>
    /// Payload for a GetLabel message
    /// </summary>
    public class GetLabelPayload : IPayload
    {
        private byte[] _messageType = new byte[2] { 23, 0 };
        public byte[] MessageType { get { return _messageType; } }

        public byte[] GetPayload()
        {
            return new byte[0];
        }
    }
}
