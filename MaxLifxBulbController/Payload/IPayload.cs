using MaxLifx.Controllers;

namespace MaxLifx.Payload
{
    public interface IPayload
    {
        byte[] MessageType { get; }
        byte[] GetPayload();
        BulbType PayloadType { get; set; }
    }
}
