using MaxLifx.Controllers;
using MaxLifx.Payload;
using System;

namespace MaxLifxCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            

            if (args.Length > 0 && args[0].Contains("list"))
            {
                MaxLifxBulbController bulbController = new MaxLifxBulbController();
                bulbController.SetupNetwork();
                bulbController.DiscoverBulbs(null);

                foreach (Bulb b in bulbController.Bulbs)
                    Console.WriteLine(b.Label + " - " + b.MacAddress);
            }
            else if (args.Length > 0 && args[0].Contains("setcolour"))
            {
                if (args.Length != 7)
                {
                    B0Rk("Wrong number of arguments for setcolour.");
                }

                string macAddress = args[1].ToUpper();

                int hue;
                int.TryParse(args[2], out hue);
                ushort sat;
                ushort.TryParse(args[3], out sat);
                ushort bri;
                ushort.TryParse(args[4], out bri);
                ushort kel;
                ushort.TryParse(args[5], out kel);
                UInt32 tra;
                UInt32.TryParse(args[6], out tra);

                if (args[1].Length != 12) B0Rk("Mac address must be 12 characters.");

                MaxLifxBulbController bulbController = new MaxLifxBulbController();
                bulbController.SetupNetwork();
                var payload = new SetColourPayload() { Hue = hue, Brightness = bri, Kelvin = kel, Saturation = sat, TransitionDuration = tra };
                //bulbController.SendPayloadToMacAddress(payload, macAddress, ip);
            }
            else
            {
                Console.WriteLine("\r\nMaxLifxCmd - a C# LAN protocol Lifx bulb controller\r\n"+
                                  "===================================================\r\n\r\n"+
                                  "Usage:\r\n\r\n"+
                                  "Search for bulbs and list their labels and MAC addresses:\r\n\r\n" +
                                  "maxlifxcmd list\r\n\r\n" +
                                  "Set one or all bulb colours:\r\n\r\n" +
                                  "maxlifxcmd setcolour <macaddress> <hue> <saturation> <brightness>\r\n" +
                                  "           <kelvin> <transition>\r\n\r\n"+
                                  "mac address: ABCDEF012345 or 000000000000 for all bulbs\r\n"+
                                  "hue : between 0 and 360\r\n"+
                                  "saturation, brightness, kelvin: between 0 and 65535\r\n"+
                                  "transition duration: between 0 and "+UInt32.MaxValue+" (ms)");
                                
            }


        }

        private static void B0Rk(string s)
        {
            Console.Write(s);
            Environment.Exit(-1);
        }
    }
}
