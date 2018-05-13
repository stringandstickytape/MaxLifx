using MaxLifx.Payload;
using System;
using System.Collections.Generic;

namespace MaxLifx.Controllers
{
    public interface IBulbController
    {

        List<Bulb> Bulbs { get; set; }
        event EventHandler ColourSet;
        void SetColour(string label, SetColourPayload payload);
        void SendPayloadToMacAddress(IPayload Payload, string macAddress, string ipAddress);
        void DiscoverBulbs(string ip = "");
        void SetupNetwork();
    }
}