using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.Payload
{
    public class GetColourZonesPayload : IPayload
    // although this is GetColourZones, we are really just calling this to get StateZone for the count of zones
    {
        private byte[] _messageType = new byte[2] { 0xF6, 0x01 };
        public byte[] MessageType {  get { return _messageType; } }
        public int Hue { get; set; }
        public byte start_index = 0;
        public byte end_index = 0;
        public byte[] GetPayload()
        {
            var _start_index = BitConverter.GetBytes(start_index);
            var _end_index = BitConverter.GetBytes(end_index);
            return new byte[0].Concat(_start_index).Concat(_end_index).ToArray();
        }
    }
}
