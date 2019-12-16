using MaxLifx.Controllers;
using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifxBulbControllerCache
{
    public class HomebrewDevicePayloadCache
    {
        public Dictionary<(Bulb, int), SetColourPayload> Payloads = new Dictionary<(Bulb, int), SetColourPayload>();

        public void Send(Dictionary<string, System.Net.Sockets.UdpClient> reusableHomebrewClientDictionary, MaxLifxBulbController bulbController)
        {
            if (Payloads.Any())
            {

                foreach (var group in Payloads.GroupBy(x => (Bulb)x.Key.Item1))
                {
                    var payloads = group.Select(x => x.Value);
                    Dictionary<int, SetColourPayload> individualPayloads = new Dictionary<int, SetColourPayload>();

                    int ctr = 0;
                    foreach (var p in group)
                    {
                        individualPayloads.Add(p.Key.Item2, p.Value);

                        if (ctr % 80 == 79 || ctr == group.Count() - 1)
                        {
                            if (!reusableHomebrewClientDictionary.ContainsKey(group.Key.IpAddress))
                            {
                                reusableHomebrewClientDictionary.Add(group.Key.IpAddress,
                                    MaxLifxBulbController.GetPersistentClient(group.Key.MacAddress, group.Key.IpAddress));
                            }

                            var payload = new SetHomebrewColourZonesPayload { IndividualPayloads = individualPayloads };

                            bulbController.SendPayloadToMacAddress(payload, group.Key.MacAddress, group.Key.IpAddress, reusableHomebrewClientDictionary[group.Key.IpAddress]);

                            individualPayloads = new Dictionary<int, SetColourPayload>();
                        }

                        ctr++;
                    }




                    // foreach(var p in group.ToDictionary(x => x.Key.Item2, x => x.Value))
                    //     MaxLifxBulbController.SendPayloadToMacAddress(p.Value, group.Key.MacAddress, group.Key.IpAddress);
                }




            }
        }
    }
}
