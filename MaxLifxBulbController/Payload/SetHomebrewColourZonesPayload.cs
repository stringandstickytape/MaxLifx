using MaxLifx.Controllers;
using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.Payload
{
    public class SetHomebrewColourZonesPayload : SetColourPayload, IPayload
    {
        private byte[] _messageType = new byte[2] { 0xF5, 0x01 };
        public new byte[] MessageType { get { return _messageType; } }

        public List<GetColourZonesPayload> Zones { get; set; }
        public Dictionary<int, SetColourPayload> IndividualPayloads { get; set; }

        public new byte[] GetPayload() {

            byte[] bytes = new byte[0] { };

            foreach (var individualPayload in IndividualPayloads)
            {
                var payload = individualPayload.Value;
                if (payload.Hue < 0)
                    payload.Hue = payload.Hue + 36000;
                payload.Hue = payload.Hue % 360;

                var _hsbkColourLE = BitConverter.GetBytes((payload.Hue * 65535) / 360);
                var _hsbkColour = new byte[2] { _hsbkColourLE[0], _hsbkColourLE[1] };

                var _saturation = BitConverter.GetBytes(payload.Saturation);
                var _brightness = BitConverter.GetBytes(payload.Brightness);

                var _kelvin = BitConverter.GetBytes(payload.Kelvin);

                var _transition = BitConverter.GetBytes(payload.TransitionDuration);
                byte[] start_index = new byte[1] { (byte)individualPayload.Key };
                byte[] end_index = new byte[1] { (byte)individualPayload.Key };

                

                var _payload = start_index.Concat(end_index)
                                    .Concat(_hsbkColour)
                                    .Concat(_saturation)
                                    .Concat(_brightness)
                                    .Concat(_kelvin)
                                    .Concat(_transition)
                                    .ToArray();

                bytes = bytes.Concat(_payload).ToArray();
            }
           //000070DC0000CBCCAC0D6400000000000000000099CFAC0D64000000
            return  bytes;
        }
    }
}
