using MaxLifx.Util;
using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MaxLifx.Packets;
using CUE.NET;
using CUE.NET.Brushes;
using System.Drawing;
using CUE.NET.Devices.Mouse;
using AuraSDKDotNet;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Generic;

namespace MaxLifx.Controllers
{


    public class MaxLifxBulbController : IBulbController
    {
        // Network details
        UdpClient _receivingUdpClient;
        string _localIp = Utils.LocalIPAddress();
        Socket _sendingSocket;
        IPAddress _sendToAddress;
        IPEndPoint _sendingEndPoint;
        CorsairMouse Mouse;
        CorsairKeyboard Keyboard;
        Dictionary<string, CorsairLed> KeyboardLedDictionary;
        
        DateTime lastCorsairUpdate = DateTime.Now;
        DateTime lastCorsairKbdUpdate = DateTime.Now;

        AuraSDK auraSDK;
        DateTime lastAuraUpdate = DateTime.Now;

        // List of all bulbs discovered
        public List<Bulb> Bulbs { get; set; } 

        public MaxLifxBulbController()
        {
             Bulbs = new List<Bulb>();
            KeyboardLedDictionary = new Dictionary<string, CorsairLed>();
        }

        public event EventHandler ColourSet;

        public void SetColour(string label, SetColourPayload payload)
        {
            var bulb = Bulbs.Single(x => x.Label == label);
            SendPayloadToMacAddress(payload, bulb.MacAddress, bulb.IpAddress);

            ColourSet?.Invoke(new LabelAndColourPayload() { Label = label, Payload = payload }, null);
        }

        private static ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim(), _cacheMouseLock = new ReaderWriterLockSlim();

        public void SendPayloadToMacAddress(IPayload Payload, string macAddress, string ipAddress)
        {
            switch (Payload.PayloadType)
            {
                case BulbType.Lifx:
                    var targetMacAddress = Utils.StringToByteArray(macAddress + "0000");

                    //Socket sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress sendToAddress = IPAddress.Parse(ipAddress);
                    IPEndPoint sendingEndPoint = new IPEndPoint(sendToAddress, 56700);

                    byte[] sendData = Utils.StringToByteArray(PacketFactory.GetPacket(targetMacAddress, Payload));

                    var a = new UdpClient();
                    a.Connect(sendingEndPoint);
                    a.Send(sendData, sendData.Length);
                    a.Close();
                    break;
                case BulbType.CorsairMouse:
                    if (Payload is SetColourPayload)
                    {
                        var led = Mouse.Leds.FirstOrDefault(x => x.Id.ToString() == ipAddress);

                        System.Drawing.Color colour;

                        if (((SetColourPayload)Payload).RGBColour != null)
                            colour = ((SetColourPayload)Payload).RGBColour.Value;
                        else
                            colour = HsbToRgb(((SetColourPayload)Payload).Hue, ((SetColourPayload)Payload).Saturation / 65535f, ((SetColourPayload)Payload).Brightness / 65535f);

                        _cacheMouseLock.EnterReadLock();
                        try
                        {
                            led.Color = colour;
                        }
                        finally
                        {
                            _cacheMouseLock.ExitReadLock();
                        }

                        _cacheMouseLock.EnterWriteLock();
                        try
                        {
                            if ((DateTime.Now - lastCorsairUpdate).TotalMilliseconds > 20)
                            {
                                Mouse.Update();
                                lastCorsairUpdate = DateTime.Now;
                            }
                        }
                        finally
                        {
                            _cacheMouseLock.ExitWriteLock();
                        }
                    }

                    break;
                case BulbType.CorsairKeyboard:
                    if (Payload is SetColourPayload)
                    {
                        var led = KeyboardLedDictionary[ipAddress];

                        System.Drawing.Color colour;

                        if (((SetColourPayload)Payload).RGBColour != null)
                            colour = ((SetColourPayload)Payload).RGBColour.Value;
                        else
                            colour = HsbToRgb(((SetColourPayload)Payload).Hue, ((SetColourPayload)Payload).Saturation / 65535f, ((SetColourPayload)Payload).Brightness / 65535f);

                        _cacheLock.EnterReadLock();
                        try
                        {
                            led.Color = colour;
                        }
                        finally
                        {
                            _cacheLock.ExitReadLock();
                        }

                        _cacheLock.EnterWriteLock();
                        try
                        {
                            if ((DateTime.Now - lastCorsairKbdUpdate).TotalMilliseconds > 20)
                            {
                                Keyboard.Update();
                                lastCorsairKbdUpdate = DateTime.Now;
                            }
                        }
                        finally
                        {
                            _cacheLock.ExitWriteLock();
                        }
                    }

                    break;
                case BulbType.Asus:
                    if (Payload is SetColourPayload)
                    {
                        //if (!auraSDK.Motherboards.Any()) return;

                        if (disappointingAuraColourCache == null)
                        {
                            disappointingAuraColourCache = new AuraSDKDotNet.Color[auraSDK.Motherboards[0].LedCount];
                            for(var i =0; i < disappointingAuraColourCache.Length; i++)
                            {
                                disappointingAuraColourCache[i] = new AuraSDKDotNet.Color(0, 0, 0);
                            }
                        }

                        var color = (((SetColourPayload)Payload).RGBColour != null)  ? ((SetColourPayload)Payload).RGBColour : 
                            HsbToRgb(((SetColourPayload)Payload).Hue, ((SetColourPayload)Payload).Saturation / 65535f, ((SetColourPayload)Payload).Brightness / 65535f);

                        var c = new AuraSDKDotNet.Color(color.Value.R,color.Value.G,color.Value.B);
                        disappointingAuraColourCache[int.Parse(ipAddress)] = c;

                        if ((DateTime.Now - lastAuraUpdate).TotalMilliseconds > 200)
                        {
                            new Thread(() =>
                            {
                                auraSDK.Motherboards[0].SetColors(disappointingAuraColourCache);
                            }).Start();

                            //auraSDK.Motherboards[0].SetColors(disappointingAuraColourCache);
                            lastAuraUpdate = DateTime.Now;
                        }
                    }

                        break;
            }
        }

        private AuraSDKDotNet.Color[] disappointingAuraColourCache;

        // The following is based on https://github.com/PhilWheat/LIFX-Control
        public void DiscoverBulbs(string ip = "")
        {
            // Send discovery packet
            GetServicePayload payload = new GetServicePayload();
            byte[] sendData = Utils.StringToByteArray(PacketFactory.GetPacket(new byte[8], payload));
            if (ip != "") _localIp = ip;

            var a = new UdpClient();
            a.Connect(_sendingEndPoint);
            a.Send(sendData, sendData.Length);
            a.Close();

            //_sendingSocket.SendTo(sendData, _sendingEndPoint);

            // Listen for replies
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _receivingUdpClient = new UdpClient(56700);
            
            byte[] receivebytes;

            // Pause for a second to allow for slow bulb responses - not uncommmon :/
            Thread.Sleep(1000);
            Bulbs = new List<Bulb>();
            // Now loop through received packets
            while (_receivingUdpClient.Available > 0)
            {
                // Get the outstanding bytes
                receivebytes = _receivingUdpClient.Receive(ref remoteIpEndPoint);

                // Get the MAC address of the bulb replying
                var macAddress = Utils.ByteArrayToString(receivebytes).Substring(16, 12);
                if (macAddress != "000000000000")
                {
                    var newBulb = new Bulb() {MacAddress = macAddress, IpAddress = remoteIpEndPoint.Address.ToString()};

                    // Create a new Bulb object
                    if (Bulbs.Count(x => x.MacAddress == macAddress) == 0)
                        Bulbs.Add(newBulb);
                }
            }

            // Now, find the labels of all the bubs we detected
            GetLabelPayload labelPayload = new GetLabelPayload();
            foreach (var bulb in Bulbs)
            {
                // Send label request to a specific bulb
                sendData = Utils.StringToByteArray(PacketFactory.GetPacket(Utils.StringToByteArray(bulb.MacAddress + "0000"), labelPayload));

                a = new UdpClient();
                a.Connect(_sendingEndPoint);
                a.Send(sendData, sendData.Length);
                a.Close();

                //_sendingSocket.SendTo(sendData, _sendingEndPoint);

                Thread.Sleep(1000);

                while (_receivingUdpClient.Available > 0)
                {
                    receivebytes = _receivingUdpClient.Receive(ref remoteIpEndPoint);
                    if (receivebytes[0] == 0x44)
                    {
                        // Parse the received label and mark it against the bulb
                        var label1 = Utils.HexToAscii(Utils.ByteArrayToString(receivebytes).Substring(36 * 2));
                        bulb.Label = label1.Substring(0,label1.IndexOf('\0'));
                    }
                }
            }

            _receivingUdpClient.Close();

            if(Mouse != null)
            {
                foreach(var led in Mouse.Leds)
                {
                    Bulbs.Add(new Bulb() { Label = $"Corsair Mouse {led.Id.ToString()}", BulbType = BulbType.CorsairMouse, IpAddress = led.Id.ToString() });
                }
            }

            if (Keyboard != null)
            {
                foreach (var led in Keyboard.Leds)
                {
                    Bulbs.Add(new Bulb() { Label = $"Corsair Keyboard {led.Id.ToString()}", BulbType = BulbType.CorsairKeyboard, IpAddress = led.Id.ToString() });
                }
            }

            if (auraSDK != null && auraSDK.Motherboards.Length > 0)
            {
                for (var i = 0; i < auraSDK.Motherboards[0].LedCount; i++)
                    Bulbs.Add(new Bulb() { BulbType = BulbType.Asus, Label = $"Asus {i}", IpAddress = i.ToString()});

            }
        }

        // The following is taken verbatim from https://github.com/PhilWheat/LIFX-Control
        public void SetupNetwork()
        {
            var pos = _localIp.LastIndexOf('.');
            if (pos >= 0)
                _localIp = _localIp.Substring(0, pos);
            _localIp = _localIp + ".255";
            // Set up UDP connection
            //_sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            SetupNetwork(_localIp);
        }

        private void SetupNetwork(string ip)
        {
            _localIp = ip;
            _sendToAddress = IPAddress.Parse(ip);
            _sendingEndPoint = new IPEndPoint(_sendToAddress, 56700);

            try
            {
                CueSDK.Initialize();
                if(CueSDK.KeyboardSDK != null) CueSDK.KeyboardSDK.Brush = (SolidColorBrush)System.Drawing.Color.Transparent;
                if (CueSDK.MouseSDK != null) CueSDK.MouseSDK.Brush = (SolidColorBrush)System.Drawing.Color.Transparent;
                Mouse = CueSDK.MouseSDK;
                Keyboard = CueSDK.KeyboardSDK;
                if (Keyboard != null) KeyboardLedDictionary = Keyboard.Leds.ToDictionary(x => x.Id.ToString(), x => x);
            }
            catch (Exception e) { }

            auraSDK = new AuraSDK();
        }

        public static System.Drawing.Color HsbToRgb(double h, double s, double b)
        {
            if (s == 0)
                return RawRgbToRgb(b, b, b);
            else
            {
                var sector = h / 60;
                var sectorNumber = (int)Math.Truncate(sector);
                var sectorFraction = sector - sectorNumber;
                var b1 = b * (1 - s);
                var b2 = b * (1 - s * sectorFraction);
                var b3 = b * (1 - s * (1 - sectorFraction));
                switch (sectorNumber)
                {
                    case 0:
                        return RawRgbToRgb(b, b3, b1);
                    case 1:
                        return RawRgbToRgb(b2, b, b1);
                    case 2:
                        return RawRgbToRgb(b1, b, b3);
                    case 3:
                        return RawRgbToRgb(b1, b2, b);
                    case 4:
                        return RawRgbToRgb(b3, b1, b);
                    case 5:
                        return RawRgbToRgb(b, b1, b2);
                    default:
                        return RawRgbToRgb(0, 0, 0);
                }
            }
        }

        private static System.Drawing.Color RawRgbToRgb(double rawR, double rawG, double rawB)
        {
            if (rawR < 0 || rawG < 0 || rawB < 0) return System.Drawing.Color.Black;
            return System.Drawing.Color.FromArgb(
                (int)Math.Round(rawR * 255),
                (int)Math.Round(rawG * 255),
                (int)Math.Round(rawB * 255));
        }
    }
}
