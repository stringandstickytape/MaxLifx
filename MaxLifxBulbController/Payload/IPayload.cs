namespace MaxLifx.Payload
{
    public interface IPayload
    {
        byte[] MessageType { get; }
        byte[] GetPayload();
    }
}
