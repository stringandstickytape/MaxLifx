using System;

namespace MaxLifx.UIs
{
    public class SoundMessage
    {
        public SoundMessageTypes SoundMessageType { get; set; }
        public object Parameter { get; set; }
        public Type ParameterType { get; set; }
        public string SoundUUID { get; set; }
    }

    public enum SoundMessageTypes
    {
        SetVolume,
        SetPan
    }
}