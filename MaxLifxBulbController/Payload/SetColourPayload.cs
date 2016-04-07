using System;
using System.Linq;

namespace MaxLifx.Payload
{
    /// <summary>
    /// Payload for a SetColour message
    /// </summary>
    public class SetColourPayload : IPayload
    {
        private byte[] _messageType = new byte[2] { 0x66, 0 };
        public byte[] MessageType {  get { return _messageType; } }
        public int Hue { get; set; }
        public UInt16 Saturation { get; set; }
        public UInt16 Brightness { get; set; }
        public UInt16 Kelvin { get; set; }
        public UInt32 TransitionDuration { get; set; }

        // The following is interpreted from https://community.lifx.com/t/building-a-lifx-packet/59
        public byte[] GetPayload()
        {
            if (Hue < 0)
                Hue = Hue + 36000;
            Hue = Hue % 360;

            // Payload
            // The payload starts with a reserved field of 8 bits (1 bytes).
            var _payloadReserved = new byte[1];

            // HSBK colour: Next up is the color described in HSBK. The HSBK format is described at the top of the light messages7 page. 
            // It starts with a 16 bit (2 byte) integer representing the Hue. The hue of green is 120 degrees. Our scale however 
            // goes from 1-65535 instead of the traditional 1-360 hue scale. To represent this we use a simple formula to find the 
            // hue in our range. 120 / 360 * 65535 which yields a result of 21845. This is 0x5555 in hex. In our case it isn't important, 
            // but remember to represent this number in little endian.

            // Watch out, BitConverter.GetBytes returns a little endian answer so we needn't reverse the result
            //var hue = r.Next(360);
            var _hsbkColourLE = BitConverter.GetBytes((Hue * 65535) / 360);
            var _hsbkColour = new byte[2] { _hsbkColourLE[0], _hsbkColourLE[1] };

            // We want maximum saturation which in a 16bit (2 byte) value is represented as 0xFFFF.
            var _saturation = BitConverter.GetBytes(Saturation);
            var _brightness = BitConverter.GetBytes(Brightness);

            // Finally we set the Kelvin to mid-range which is 3500 in decimal or 0x0DAC.
            var _kelvin = BitConverter.GetBytes(Kelvin);

            // The final part of our payload is the number of milliseconds over which to perform the transition. Lets set it to 
            // 1024ms because its an easy number and this is getting complicated. 1024 is 0x00000400 in hex.
            // var _transitionLE = BitConverter.GetBytes(1024);
            // is 4 bytes
            var _transition = BitConverter.GetBytes(TransitionDuration);

            var _payload = _payloadReserved.Concat(_hsbkColour)
                                .Concat(_saturation)
                                .Concat(_brightness)
                                .Concat(_kelvin)
                                .Concat(_transition)
                                .ToArray();
            return _payload;
        }
    }
}
