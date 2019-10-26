using MaxLifx.Payload;
using MaxLifx.Util;
using System;
using System.Linq;

namespace MaxLifx.Packets
{
    class PacketFactory
    {
        public static string GetPacket(byte[] targetMacAddress, IPayload Payload)
        {
            // Mac address must be left-aligned and padded with zeroes to a length of 8...
            if (targetMacAddress.Length != 8) throw new NotImplementedException();

            // http://lan.developer.lifx.com/v2.0/docs/header-description#frame
            // Frame is 16+2+1+1+12+32=64 bits = 8 bytes

            byte[] sizelessHeader = GetSizelessHeader(targetMacAddress, Payload);

            //System.Diagnostics.Debug.WriteLine("Sizeless header is: " + BitConverter.ToString(sizelessHeader).Replace("-", ""));

            byte[] payloadBytes = Payload.GetPayload();

            var frameSizeInt = Convert.ToInt16(2 + sizelessHeader.Length + payloadBytes.Length);
            var frameSize = BitConverter.GetBytes(frameSizeInt);

            System.Diagnostics.Debug.WriteLine("payloadBytes is: " + BitConverter.ToString(payloadBytes).Replace("-", ""));

            var packet = frameSize.Concat(sizelessHeader)
                                .Concat(payloadBytes);

            var packetAsString = BitConverter.ToString(packet.ToArray()).Replace("-", "");

            return packetAsString;// ;
        }

        // The following is interpreted from https://community.lifx.com/t/building-a-lifx-packet/59
        private static byte[] GetSizelessHeader(byte[] targetMacAddress, IPayload payload)
        {
            // Sizeless Header
            var originAddressTaggedProtocol = new byte[2];

            // [Next], The documentation says that the first two bits here are the message origin indicator and must be zero. (they already are)
            // [so bits 7 and 6 are unset]

            // The next bit represents the tagged field.We want this packet to be processed by all devices that receive it so we need to set it to one (1).
            if ((targetMacAddress.Sum(x => x) == 0) || payload.GetType() == typeof(GetServicePayload))
                Utils.SetBit(ref originAddressTaggedProtocol, 5);

            // The next bit represents the addressable field. This indicates that the next header will be a frame address header. Since all of our frames require this it will always be set to 1.
            Utils.SetBit(ref originAddressTaggedProtocol, 4);

            // The final 12 bits are the protocol field. This must be set to 1024 which is 0100 0000 0000 in binary. Now our two bytes are complete.
            Utils.SetBit(ref originAddressTaggedProtocol, 2);

            // we need to switch the two bytes around before we add them to the frame
            Array.Reverse(originAddressTaggedProtocol, 0, originAddressTaggedProtocol.Length);

            // The next 32 bit(4 bytes) are the source, which are unique to the client and used to identify broadcasts that it cares about.Since 
            // we are a dumb client and don't care about the response, lets set this all to zero (0). If you are writing a client program you'll 
            // want to use a unique value here.
            byte[] source = new byte[4];
            // we want the response of the following payloads:
            if((targetMacAddress.Sum(x => x) != 0) && payload.GetType() != typeof(GetServicePayload) && payload.GetType() != typeof(GetLabelPayload) && payload.GetType() != typeof(GetColourZonesPayload) && payload.GetType() != typeof(GetVersionPayload))
                source = new byte[]  { 1,2,3,4 };

            // The frame address starts with 64 bits (8 bytes) of the target field. Since we want this message to be processed by all device we will set it to zero.
            var frameAddress = targetMacAddress;// tagged = 0 and  { 0xD0, 0x73, 0xD5, 0x02, 0xA7, 0x72,0,0};
            //Array.Reverse(_frameAddress, 0, _frameAddress.Length);

            // This is followed by a reserved section of 48 bits(6 bytes) that must be all zeros.
            var reserved = new byte[6];

            // The next byte is another field so follow the steps above to build the binary then hex representations. In this example we will be setting the 
            // ack_required and res_required fields to zero (0) because our bash script wont be listening for a response. This leads to a byte of zero being added.
            var ackResRequired = new byte[1];

            // Since we aren't processing or creating responses the sequence number is irrelevant so lets also set it to zero (0)
            var sequence = new byte[1];

            // Next we include the protocol header. which begins with 64 reserved bits (8 bytes). Set these all to zero.
            var prReserved = new byte[8];

            // We are changing the color of our lights, so lets use SetColor9, which is message type 0x66 (102 in decimal). Remember to represent this in little endian.
            var messageType = payload.MessageType;
            //_messageType[0] = 23;

            // Finally another reserved field of 16 bits (2 bytes).
            var prReserved2 = new byte[2];

            var sizelessHeader = originAddressTaggedProtocol
                                .Concat(source)
                                .Concat(frameAddress)
                                .Concat(reserved)
                                .Concat(ackResRequired)
                                .Concat(sequence)
                                .Concat(prReserved)
                                .Concat(messageType)
                                .Concat(prReserved2)
                                .ToArray();
            return sizelessHeader;
        }
    }
}
