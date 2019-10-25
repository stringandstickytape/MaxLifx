using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.Payload
{
    public class SetColourZonesPayload : SetColourPayload, IPayload
    {
        private byte[] _messageType = new byte[2] { 0xF5, 0x01 };
        public new byte[] MessageType { get { return _messageType; } }
        public byte[] start_index { get; set; }
        public byte[] end_index { get; set; }

        public byte[] apply { get; set; }

        public new byte[] GetPayload() { 

            // Multizone packet example
            // RECALL: uses little endian(bytes reversed)
            //
            //format & size:         Frame(8B *)           Frame Address(16B *)	       	 Protocol Header(12B *)       Payload(15B in this example)
            //                  |----------------| |---------------------------------| |------------------------| |-----------------------------------|
            //                  |                | |                                 | |                        | |                                   |
            //decimal value:    51            1                          0*      False    True      0*        0*     10     65535     3500            1
            //field:            size       source      target S/N    reserved   (res_r + ack_r + reserved)  rsvd    e_i     sat.       kel.        apply
            //                  |~~|      |~~~~~~| |~~~~~~~~~~~~~~| |~~~~~~~~~~| ||                          |~~|    ||      |~~|      |~~|           |
            //bytes:            3300 0014 01000000 d073d5XXXXXXXXXX 000000000000 02 01 0000000000000000 f501 0000 0a 0a 1c47 ffff ff7f ac0d e80300000 1
            //                       |~~|                                           || |~~~~~~~~~~~~~~| |~~|      ||    |~~|      |~~|      |~~~~~~~|
            //field:            (protocol + addressable + tagged + origin)          seq.   reserved    pkg type   s_i   hue        bri.     duration
            //decimal value:        1024*     True*        False      0*             1         0*         501      10    10        32767       1000
            //
            //Notes:
            //            - * indicates mandatory value/ size
            //            - for the fields that have plus symbol, certain bits in the byte belong to different fields; need to work in binary to get value that converts to hex


            if (Hue < 0)
                Hue = Hue + 36000;
            Hue = Hue % 360;

            // Payload

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

            var _payload = start_index.Concat(end_index)
                                .Concat(_hsbkColour)
                                .Concat(_saturation)
                                .Concat(_brightness)
                                .Concat(_kelvin)
                                .Concat(_transition)
                                .Concat(apply)
                                .ToArray();
            return _payload;
        }
    }
}
