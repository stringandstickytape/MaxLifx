using MaxLifx.Util;
using MaxLifx.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MaxLifx.Packets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxLifx.Controllers
{
    public class LabelAndColourPayload
    {
        public string Label;
        public SetColourPayload Payload;
    }
    public class MaxLifxBulbController
    {
        // Network details
        UdpClient _receivingUdpClient;
        string _localIp = Utils.LocalIPAddress();
        Socket _sendingSocket;
        IPAddress _sendToAddress;
        IPEndPoint _sendingEndPoint;

        // List of all bulbs discovered
        public List<Bulb> Bulbs = new List<Bulb>();

        public event EventHandler ColourSet;

        public void SetColour(Bulb bulb,  int zone, SetColourPayload payload, bool updateBox)
        {
            
            if (bulb.Zones > 1)
            {
                var newPayload = new SetColourZonesPayload()
                {
                    Brightness = payload.Brightness,
                    Hue = payload.Hue,
                    Kelvin = payload.Kelvin,
                    Saturation = payload.Saturation,
                    TransitionDuration = payload.TransitionDuration,
                    start_index = new byte[] { (byte)zone },
                    end_index = new byte[] { (byte)zone },
                    apply = new byte[] { 0 }
                };

                SendPayloadToMacAddress(newPayload, bulb.MacAddress, bulb.IpAddress);
            }
            else
                SendPayloadToMacAddress(payload, bulb.MacAddress, bulb.IpAddress);

            // this updates the bulb monitor, skip for multizone lights
            if (updateBox)
            {
                ColourSet?.Invoke(new LabelAndColourPayload() { Label = bulb.Label, Payload = payload }, null);
            }
        }

        private Dictionary<string, (Bulb, int)> labelToBulbZoneCache = new Dictionary<string, (Bulb, int)>();

        public Bulb GetBulbFromLabel(string label, out int zone)
        {
            if (!labelToBulbZoneCache.ContainsKey(label))
            {
                var bulb = Bulbs.SingleOrDefault(x => x.Label == label);
                zone = 0;
                if (bulb == null)
                {
                    bulb = Bulbs.Single(x => x.Label == label.Substring(0, label.LastIndexOf(" (Zone ")));

                    var zonex = label.Substring(label.LastIndexOf(" (Zone ") + 7);
                    zonex = zonex.Substring(0, zonex.Length - 1);
                    zone = int.Parse(zonex) - 1;
                }

                labelToBulbZoneCache.Add(label, (bulb,zone));
            }
            zone = labelToBulbZoneCache[label].Item2;
            return labelToBulbZoneCache[label].Item1;
        }

        Dictionary<string, byte[]> macAddressCache = new Dictionary<string, byte[]>();
        public void SendPayloadToMacAddress(IPayload Payload, string macAddress, string ipAddress, UdpClient persistentClient = null)
        {
            var fullMac = $"{macAddress}0000";
            if (!macAddressCache.ContainsKey(fullMac))
                macAddressCache.Add(fullMac, Utils.StringToByteArray(fullMac));
            
            SendPayloadToMacAddress(Payload, macAddressCache[fullMac], ipAddress, persistentClient);
        }

        public void SendPayloadToMacAddress(IPayload Payload, byte[] targetMacAddress, string ipAddress, UdpClient persistentClient = null)
        {
            IPAddress sendToAddress = IPAddress.Parse(ipAddress);
            IPEndPoint sendingEndPoint = new IPEndPoint(sendToAddress, 56700);

            byte[] sendData = PacketFactory.GetPacket(targetMacAddress, Payload);

            if (persistentClient == null)
            {
                var a = new UdpClient();
                a.Connect(sendingEndPoint);
                a.Send(sendData, sendData.Length);
                a.Close();
            }
            else persistentClient.Send(sendData, sendData.Length);
        }

        public static UdpClient GetPersistentClient(string macAddress, string ipAddress)
        {
            var targetMacAddress = Utils.StringToByteArray(macAddress + "0000");
            IPAddress sendToAddress = IPAddress.Parse(ipAddress);
            IPEndPoint sendingEndPoint = new IPEndPoint(sendToAddress, 56700);
            var a = new UdpClient();
            a.Connect(sendingEndPoint);
            return a;
        }

        private void recv(IAsyncResult res)
        {
            UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).ut;
            IPEndPoint e = (IPEndPoint)((UdpState)(res.AsyncState)).e;

            byte[] received = u.EndReceive(res, ref e);

            if(Bulbs == null) Bulbs = new List<Bulb>();

                // Get the MAC address of the bulb replying
                var macAddress = Utils.ByteArrayToString(received).Substring(16, 12);
                if (macAddress != "000000000000")
                {
                    var newBulb = new Bulb() { MacAddress = macAddress, IpAddress = e.Address.ToString() };

                    // Create a new Bulb object
                    if (Bulbs.Count(x => x.MacAddress == macAddress) == 0)
                        Bulbs.Add(newBulb);
                }

            //
            ////Process codes
            //
            //MessageBox.Show(Encoding.UTF8.GetString(received));
            //Client.BeginReceive(new AsyncCallback(recv), null);
            
           //u.BeginReceive(new AsyncCallback(recv), ((UdpState)(res.AsyncState)));
        }

        UdpClient udpClient;

        // The following is based on https://github.com/PhilWheat/LIFX-Control
        public void DiscoverBulbs(Form f, string ip = "")
        {
            //// Send discovery packet
            //GetServicePayload payload = new GetServicePayload();
            //byte[] sendData = PacketFactory.GetPacket(new byte[8], payload);
            //if (ip != "") _localIp = ip;
            //
            //udpClient = new UdpClient();
            //udpClient.Connect(_sendingEndPoint);
            //udpClient.Send(sendData, sendData.Length);
            //udpClient.Close();

            //_sendingSocket.SendTo(sendData, _sendingEndPoint);

            // Listen for replies



            /*Bulbs = new List<Bulb>();
            // Now loop through received packets
            while (_receivingUdpClient.Available > 0)
            {
                // Get the outstanding bytes
                receivebytes = _receivingUdpClient.Receive(ref remoteIpEndPoint);

                // Get the MAC address of the bulb replying
                var macAddress = Utils.ByteArrayToString(receivebytes).Substring(16, 12);
                if (macAddress != "000000000000")
                {
                    var newBulb = new Bulb() { MacAddress = macAddress, IpAddress = remoteIpEndPoint.Address.ToString() };

                    // Create a new Bulb object
                    if (Bulbs.Count(x => x.MacAddress == macAddress) == 0)
                        Bulbs.Add(newBulb);
                }
            }
            
            // Now, find the labels of all the bubs we detected
            GetLabelPayload labelPayload = new GetLabelPayload();
            // and also the version of each bulb
            GetVersionPayload versionPayload = new GetVersionPayload();
            // and zones if any
            GetColourZonesPayload ColourZonesPayload = new GetColourZonesPayload();
            foreach (var bulb in Bulbs)
            {
                a = new UdpClient();
                a.Connect(_sendingEndPoint);
                // Send label request to a specific bulb
                sendData = PacketFactory.GetPacket(Utils.StringToByteArray(bulb.MacAddress + "0000"), labelPayload);
                a.Send(sendData, sendData.Length);
                // Send version request to a specific bulb
                //sendData = Utils.StringToByteArray(PacketFactory.GetPacket(Utils.StringToByteArray(bulb.MacAddress + "0000"), versionPayload));
                //a.Send(sendData, sendData.Length);
                a.Close();

                //_sendingSocket.SendTo(sendData, _sendingEndPoint);

                Thread.Sleep(1000);

                while (_receivingUdpClient.Available > 0)
                {
                    receivebytes = _receivingUdpClient.Receive(ref remoteIpEndPoint);
                    if (receivebytes[0] == 0x44 && remoteIpEndPoint.Address.ToString() == bulb.IpAddress)
                    {
                        // Parse the received label and mark it against the bulb
                        var label1 = Utils.HexToAscii(Utils.ByteArrayToString(receivebytes).Substring(36 * 2));
                        bulb.Label = label1.Substring(0, label1.IndexOf('\0'));
                    }
                    // if (receivebytes[0] == 48)
                    //{
                        // set the proper version of bulb
                       // bulb.Version = receivebytes[40];
                   // } 
                }
            }
            // seperating the 2 seems more reliable
            foreach (var bulb in Bulbs)
            {
                a = new UdpClient();
                a.Connect(new IPEndPoint(IPAddress.Parse(bulb.IpAddress), 56700));
                // Send zone request
                sendData = PacketFactory.GetPacket(Utils.StringToByteArray(bulb.MacAddress + "0000"), ColourZonesPayload);
                a.Send(sendData, sendData.Length);
                a.Close();

                //_sendingSocket.SendTo(sendData, _sendingEndPoint);

                Thread.Sleep(1000);

                while (_receivingUdpClient.Available > 0)
                {
                    receivebytes = _receivingUdpClient.Receive(ref remoteIpEndPoint);
                    if (receivebytes[32] == 250 && receivebytes[33] == 1 && remoteIpEndPoint.Address.ToString() == bulb.IpAddress)
                    {
                        // set the zones count of bulb
                        bulb.Zones = receivebytes[36];
                    }
                }
            }

            foreach (var bulb in Bulbs)
            {
                a = new UdpClient();
                a.Connect(_sendingEndPoint);
                // Send zone request

                
                sendData = PacketFactory.GetPacket(Utils.StringToByteArray(bulb.MacAddress + "0000"), versionPayload);
                a.Send(sendData, sendData.Length);
                a.Close();

                //_sendingSocket.SendTo(sendData, _sendingEndPoint);

                Thread.Sleep(1000);

                while (_receivingUdpClient.Available > 0)
                {
                    receivebytes = _receivingUdpClient.Receive(ref remoteIpEndPoint);
                    if (receivebytes[32] == 0x13)
                    {

                        System.Diagnostics.Debug.WriteLine(Utils.ByteArrayToString(receivebytes));
                    
                  //      if (bulb.MacAddress.StartsWith("01"))
                    {
                        System.Diagnostics.Debug.WriteLine("01 ^^^^");
                    }

                    // set the proper version of bulb
                    // bulb.Version = receivebytes[40];
                     } 
                }
            }
            */
            
            //_receivingUdpClient.Close();
        }

        public void UdpDiscoveryListen(Func<int> callback) {
            //_receivingUdpClient = new UdpClient(56700);

            Bulbs.Clear();
            byte[] sendData;
            Bulb bulb;

            GetServicePayload payload = new GetServicePayload();
            sendData = PacketFactory.GetPacket(new byte[8], payload);
            var udpClient2 = new UdpClient();
            udpClient2.Connect(_sendingEndPoint);
            udpClient2.Send(sendData, sendData.Length);
            udpClient2.Close();

            using (var udpClient = new UdpClient())
            {
                udpClient.ExclusiveAddressUse = false;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 56700));


                while (true)
                {
                    //IPEndPoint object will allow us to read datagrams sent from any source.
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var receivebytes = udpClient.Receive(ref remoteEndPoint);

                    // Get the MAC address of the bulb replying
                    var macAddress = Utils.ByteArrayToString(receivebytes).Substring(16, 12);
                    if (macAddress != "000000000000")
                    {
                        System.Diagnostics.Debug.WriteLine($"{macAddress} : {receivebytes[32] + ((receivebytes[33] * 256))}");
                        switch (receivebytes[32])
                        {
                            case 3:
                                var newBulb = new Bulb() { MacAddress = macAddress, IpAddress = remoteEndPoint.Address.ToString() };
                            
                                // Create a new Bulb object
                                if (Bulbs.Count(x => x.MacAddress == macAddress) == 0)
                                {
                                    Bulbs.Add(newBulb);

                                    // Send a GetLabel
                                    GetLabelPayload labelPayload = new GetLabelPayload();
                                    sendData = PacketFactory.GetPacket(Utils.StringToByteArray(newBulb.MacAddress + "0000"), labelPayload);
                                    SendDataPacket(sendData, remoteEndPoint);

                                }


                                break;
                            case 25: // State Label
                                bulb = Bulbs.FirstOrDefault(x => x.MacAddress == macAddress);
                            
                                if (bulb != null)
                                {
                                    // Parse the received label and mark it against the bulb
                                    var label1 = Utils.HexToAscii(Utils.ByteArrayToString(receivebytes).Substring(36 * 2));
                                    bulb.Label = label1.Substring(0, label1.IndexOf('\0'));

                                    callback.Invoke();
                                    //f.PopulateBulbListbox();
                                    //f._suspendUi = false;
                                    //f.SaveSettings();
                                    //f._suspendUi = true;
                                    
                                    GetColourZonesPayload ColourZonesPayload = new GetColourZonesPayload();
                            
                                    sendData = PacketFactory.GetPacket(Utils.StringToByteArray(bulb.MacAddress + "0000"), ColourZonesPayload);
                                    SendDataPacket(sendData, remoteEndPoint);
                                }
                                break;
                            case 247:
                            case 250:
                                if (receivebytes[33] == 1) // 503 Zones
                                {
                                    bulb = Bulbs.FirstOrDefault(x => x.MacAddress == macAddress);

                                    if (bulb != null)
                                    {
                                        bulb.Zones = receivebytes[36];

                                        callback.Invoke();
                                    }
                                }
                                break;
                            default:

                                break;
                        }

                    }

                    //loggingEvent += Encoding.ASCII.GetString(receivedResults);
                }

            }

//            var udpState = new UdpState { e = remoteIpEndPoint, ut = _receivingUdpClient };
            /*             UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).ut;
            IPEndPoint e = (IPEndPoint)((UdpState)(res.AsyncState)).e;

            byte[] received = u.EndReceive(res, ref e);

            if(Bulbs == null) Bulbs = new List<Bulb>();

                // Get the MAC address of the bulb replying
                var macAddress = Utils.ByteArrayToString(received).Substring(16, 12);
                if (macAddress != "000000000000")
                {
                    var newBulb = new Bulb() { MacAddress = macAddress, IpAddress = e.Address.ToString() };

                    // Create a new Bulb object
                    if (Bulbs.Count(x => x.MacAddress == macAddress) == 0)
                        Bulbs.Add(newBulb);
                } */
        }

        private static void SendDataPacket(byte[] sendData, IPEndPoint remoteEndPoint)
        {
            using (var u2 = new UdpClient())
            {
                u2.ExclusiveAddressUse = false;
                u2.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                u2.Client.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 56700));
                u2.Connect(remoteEndPoint.Address, 56700);
                u2.Send(sendData, sendData.Length);
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
        }
    }

    public class UdpState
    {
        public UdpClient ut;
        public IPEndPoint e;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public int counter = 0;
    }
}
