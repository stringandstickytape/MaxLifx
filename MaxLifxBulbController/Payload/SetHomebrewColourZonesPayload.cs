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

            byte[] bytes = new byte[IndividualPayloads.Count()*10+2];
            bytes[0] = 0;
            bytes[1] = (byte)(IndividualPayloads.Count()-1);

            var ctr = 0;
            foreach (var individualPayload in IndividualPayloads)
            {
                var payload = individualPayload.Value;
                if (payload.Hue < 0)
                    payload.Hue = payload.Hue + 36000;
                payload.Hue = payload.Hue % 360;

                var _hsbkColourLE = BitConverter.GetBytes(payload.Hue * 182);
                //var _hsbkColour = new byte[2] { _hsbkColourLE[0], _hsbkColourLE[1] };

                //bytes[ctr * 10 + 0 + 2] = (byte)individualPayload.Key ;
                //bytes[ctr * 10 + 1+ 2] = (byte)individualPayload.Key ;

                bytes[ctr * 10 + 2] = _hsbkColourLE[0];
                bytes[ctr * 10 + 3] = _hsbkColourLE[1];

                var _saturation = BitConverter.GetBytes(payload.Saturation);

                bytes[ctr * 10 + 4] = _saturation[0];
                bytes[ctr * 10 + 5] = _saturation[1];

                var _brightness = BitConverter.GetBytes(payload.Brightness);

                bytes[ctr * 10 + 6] = _brightness[0];
                bytes[ctr * 10 + 7] = _brightness[1];

                var _kelvin = BitConverter.GetBytes(payload.Kelvin);

                bytes[ctr * 10 + 8] = _kelvin[0];
                bytes[ctr * 10 + 9] = _kelvin[1];

                var _transition = BitConverter.GetBytes(payload.TransitionDuration);
                //
                //// faster than array.copy...
                bytes[ctr * 10 + 10] = _transition[0];
                bytes[ctr * 10 + 11] = _transition[1];
                //bytes[ctr * 14 + 12+ 2] = _transition[2];
                //bytes[ctr * 14 + 13 + 2] = _transition[3];

                

                //byte[] start_index = new byte[1] { (byte)individualPayload.Key };
                //byte[] end_index = new byte[1] { (byte)individualPayload.Key };
                //
                //
                //
                //bytes = bytes.Concat(start_index)
                //                    .Concat(end_index)
                //                    .Concat(_hsbkColour)
                //                    .Concat(_saturation)
                //                    .Concat(_brightness)
                //                    .Concat(_kelvin)
                //                    .Concat(_transition)
                //                    .ToArray();

                ctr++;

            }
           //000070DC0000CBCCAC0D6400000000000000000099CFAC0D64000000
            return  bytes;
        }
    }
}
