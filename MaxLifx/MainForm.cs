using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MaxLifx.Controllers;
using MaxLifx.Controls;
using MaxLifx.Packets;
using MaxLifx.Payload;
using MaxLifx.Threads;
using MaxLifx.UIs;
using NAudio.Wave;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace MaxLifx
{
    public partial class MainForm : Form
    {
        private readonly MaxLifxBulbController _bulbController = new MaxLifxBulbController();
        private readonly List<IBulbController> controllers = new List<IBulbController>();

        private bool _suspendUi = true;
        private readonly LightControlThreadCollection _threadCollection = new LightControlThreadCollection();
        private readonly Random _r = new Random();
        public readonly decimal Version = 0.5m;
        private Mp3FileReader _schedulerReader;
        private DateTime _schedulerStartTime;
        private Timer _schedulerTimer = new Timer();
        private WaveOut _schedulerWaveOut;
        private MaxLifxSettings _settings = new MaxLifxSettings();

        public MainForm()
        {
            controllers.Add(_bulbController);
            
            // this.Icon = new System.Drawing.Icon("Resources\\Image1.ico");
            Bitmap bmp = MaxLifx.Properties.Resources.m__1_;
            this.Icon = Icon.FromHandle(bmp.GetHicon());

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MaxLifx"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MaxLifx");

            if (
                !Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MaxLifx\\Sounds"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "\\MaxLifx\\Sounds");

            if (
                !Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                  "\\MaxLifx\\Sounds\\Loops"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "\\MaxLifx\\Sounds\\Loops");

            if (
                !Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                  "\\MaxLifx\\Sounds\\Random"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "\\MaxLifx\\Sounds\\Random");

            InitializeComponent();
            _suspendUi = true;

            foreach (var controller in controllers) controller.SetupNetwork();
            
            if (File.Exists("Settings.xml"))
            {
                LoadSettings();

                PopulateBulbListbox();


                for (var i = 0; i < lbBulbs.Items.Count; i++)
                    if (_settings.SelectedLabels.Contains(lbBulbs.Items[i].ToString()))
                        lbBulbs.SelectedItems.Add(lbBulbs.Items[i]);
            }

            foreach (var controller in controllers)
            {
                if (controller.Bulbs.Count == 0)
                {
                    var dialogResult =
                        MessageBox.Show(
                            "No bulbs discovered.  Run bulb discovery now?  The app willl hang for about ten seconds."
                            , "Discover bulbs?",
                            MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {

                        controller.DiscoverBulbs();

                        if (controller.Bulbs.Count == 0)
                        {
                            MessageBox.Show("No bulbs found. If you have just received a Windows Firewall popup, try Bulbs -> Discover Bulbs now.");
                        }

                        PopulateBulbListbox();
                        _suspendUi = false;
                        SaveSettings();
                        _suspendUi = true;

                    }
                }

                _suspendUi = false;
                controller.ColourSet += BulbControllerOnColourSet;
            }
        }

        private void BulbControllerOnColourSet(object sender, EventArgs eventArgs)
        {
           /* if (InvokeRequired) // Line #1
            {
                BulbControllerOnColourSetDelegate d = BulbControllerOnColourSet;
                Invoke(d, sender, eventArgs);
                return;
            }

            var details = ((LabelAndColourPayload) sender);

            var pbFound = false;
            foreach (var c in panelBulbColours.Controls)
            {
                var pb = c as PictureBox;
                if (pb != null)
                {
                    if (pb.Name == "pb" + details.Label)
                    {
                        var col = Utils.HsvToRgb(details.Payload.Hue, details.Payload.Saturation/65535.0f,
                            details.Payload.Brightness/65535.0f);

                        pb.BackColor = col;

                        pbFound = true;
                        break;
                    }
                }
            }

            if (!pbFound)
            {
                var controls = panelBulbColours.Controls;
                var nextX = 0;

                if (controls.Count > 0)
                    nextX = controls[controls.Count - 1].Location.X + _thumbSize + 10;

                var newLocation = new Point(nextX, 0);
                var newLabelLocation = new Point(nextX, _thumbSize);

                var pb = new PictureBox
                {
                    Name = "pb" + details.Label,
                    Location = newLocation,
                    Size = new Size(_thumbSize, _thumbSize),
                    Text = details.Label,
                };
                pb.Click += B_Click2;

                panelBulbColours.Controls.Add(pb);
                var b = new Button
                {
                    Name = "lbl" + details.Label,
                    Location = newLabelLocation,
                    Text = details.Label,
                    Width = 100
                };
                b.Click += B_Click;
                panelBulbColours.Controls.Add(b);
            }*/
        }

        private void B_Click(object sender, EventArgs e)
        {
            var fakeBulb = new FakeBulb(_bulbController, ((Button) sender).Text) {Text = ((Button) sender).Text};
            fakeBulb.Show();
        }

        private void B_Click2(object sender, EventArgs e)
        {
            var fakeBulb = new FakeBulb(_bulbController, ((PictureBox)sender).Text) { Text = ((PictureBox)sender).Text };
            fakeBulb.Show();
        }

        private void StartNewThread(Thread thread, string threadName, IProcessor processor)
        {
            //thread.SetApartmentState(ApartmentState.STA);

            var newThread = thread;
            var newLightThread = _threadCollection.AddThread(newThread, threadName, processor);

            var lvi = new ListViewItem(newLightThread.Name);
            lvi.SubItems.Add(newLightThread.Uuid);
            lvThreads.Items.Add(lvi);
            newLightThread.Start();
        }

        private void LoadSettings(string filename = "Settings.xml")
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            var xmlString = xmlDocument.OuterXml;

            using (var read = new StringReader(xmlString))
            {
                var outType = typeof (MaxLifxSettings);

                var serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    _settings = (MaxLifxSettings) serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }

            _bulbController.Bulbs = _settings.Bulbs;
        }

        private void PopulateBulbListbox()
        {
            lbBulbs.Items.Clear();
            Text = $"MaxLifx : {_bulbController.Bulbs.Count} bulbs (";
            int ctr = 0;
            foreach (var b in _bulbController.Bulbs.OrderBy(x => x.Label))
            {
                Text += b.Label;
                lbBulbs.Items.Add(b.Label);
                ctr++;
                if (_bulbController.Bulbs.Count != ctr) Text += ", ";
            }
            Text += ")";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var processor = new ScreenColourProcessor();
            processor.ShowUI = true;
            var thread = new Thread(() => processor.ScreenColour(_bulbController, new Random(_r.Next())));
            StartNewThread(thread, "Screen Colour Thread", processor);
            
        }

        private void lbBulbs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_suspendUi)
            {
                _settings.SelectedLabels.Clear();

                var macAddress = lbBulbs.SelectedItems;

                foreach (var x in macAddress)
                    _settings.SelectedLabels.Add(x.ToString());

                SaveSettings();
            }
        }


        private void SaveThreads(string filename = "Threads.xml")
        {
            if (!_suspendUi)
            {
                var xml = new XmlSerializer(typeof (LightControlThreadCollection));

                using (var stream = new MemoryStream())
                {
                    xml.Serialize(stream, _threadCollection);
                    stream.Position = 0;
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(stream);
                    xmlDocument.Save(filename);
                    stream.Close();
                }
            }
        }

        private void LoadThreads(string filename = "Threads.xml")
        {
            if (InvokeRequired) // Line #1
            {
                LoadThreadsDelegate d = LoadThreads;
                Invoke(d, filename);
                return;
            }

            if (!_suspendUi)
            {
                var xmlDocument = new XmlDocument();
                if (File.Exists(filename))
                {
                    xmlDocument.Load(filename);
                    var xmlString = xmlDocument.OuterXml;

                    using (var read = new StringReader(xmlString))
                    {
                        var outType = typeof (LightControlThreadCollection);

                        LightControlThreadCollection loadedThreadCollection;

                        var serializer = new XmlSerializer(outType);
                        using (XmlReader reader = new XmlTextReader(read))
                        {
                            loadedThreadCollection = (LightControlThreadCollection) serializer.Deserialize(reader);
                            reader.Close();
                        }

                        foreach (var lightThread in loadedThreadCollection.LightControlThreads)
                        {
                            Thread t = null;

                            switch (lightThread.Processor.GetType().ToString())
                            {
                                case "MaxLifx.SoundResponseProcessor":
                                    t =
                                        new Thread(
                                            () =>
                                                ((SoundResponseProcessor) (lightThread.Processor)).SoundResponse(
                                                    _bulbController, new Random(_r.Next())));
                                    break;
                                case "MaxLifx.ScreenColourProcessor":
                                    t =
                                        new Thread(
                                            () =>
                                                ((ScreenColourProcessor) (lightThread.Processor)).ScreenColour(
                                                    _bulbController, new Random(_r.Next())));
                                    break;
                                case "MaxLifx.SoundGeneratorProcessor":
                                    t =
                                        new Thread(
                                            () =>
                                                ((SoundGeneratorProcessor) (lightThread.Processor)).SoundGenerator(
                                                    _bulbController, new Random(_r.Next())));
                                    break;
                            }

                            var threadName = lightThread.Name;
                            StartNewThread(t, threadName, lightThread.Processor);
                        }

                        read.Close();
                    }
                }
            }
        }

        private void SaveSettings(string filename = "Settings.xml")
        {
            if (!_suspendUi)
            {
                var xml = new XmlSerializer(typeof (MaxLifx.MaxLifxSettings));
                _settings.Bulbs = _bulbController.Bulbs;

                using (var stream = new MemoryStream())
                {
                    xml.Serialize(stream, _settings);
                    stream.Position = 0;
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(stream);
                    xmlDocument.Save(filename);
                    stream.Close();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem x in lvThreads.Items)
                _threadCollection.GetThread(x.SubItems[1].Text).Abort();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var s = new SoundResponseProcessor();
            var thread = new Thread(() => s.SoundResponse(_bulbController, new Random(_r.Next())));
            StartNewThread(thread, "Sound Response Thread", s);
            s.ShowUI = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (lvThreads.SelectedItems.Count == 0) return;
            var selectedThreadUuid = lvThreads.SelectedItems[0].SubItems[1].Text;
            if (lvThreads == null) return;
            var thread = _threadCollection.GetThread(selectedThreadUuid);
            thread.Abort();
            _threadCollection.RemoveThread(selectedThreadUuid);
            lvThreads.Items.Remove(lvThreads.SelectedItems[0]);
        }

        private void lvThreads_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string selectedThreadUuid;
            if (lvThreads.SelectedItems.Count == 0)
            {
                if (lvThreads.Items.Count == 1)
                {
                    selectedThreadUuid = lvThreads.Items[0].SubItems[1].Text;
                }
                else return;
            }
            else selectedThreadUuid = lvThreads.SelectedItems[0].SubItems[1].Text;
            if (lvThreads == null) return;
            var thread = _threadCollection.GetThread(selectedThreadUuid);
            thread.Processor.ShowUI = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = new SaveFileDialog {DefaultExt = ".MaxLifx.Threadset.xml"};
            s.Filter = "XML files (*.MaxLifx.Threadset.xml)|*.MaxLifx.Threadset.xml";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.AddExtension = true;

            if (s.ShowDialog() == DialogResult.OK)
                SaveThreads(s.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopAllThreads();

            var s = new OpenFileDialog {DefaultExt = ".MaxLifx.Threadset.xml"};
            s.Filter = "XML files (*.MaxLifx.Threadset.xml)|*.MaxLifx.Threadset.xml";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.AddExtension = true;

            if (s.ShowDialog() == DialogResult.OK)
                LoadThreads(s.FileName);
        }

        private void bStopAll_Click(object sender, EventArgs e)
        {
            StopAllThreads();
        }

        private void StopAllThreads()
        {
            if (InvokeRequired) // Line #1
            {
                StopAllThreadsDelegate d = StopAllThreads;
                Invoke(d);
            }

            for (var i = 0; i < lvThreads.Items.Count; i++)
            {
                var selectedThreadUuid = lvThreads.Items[i].SubItems[1].Text;
                var thread = _threadCollection.GetThread(selectedThreadUuid);
                thread.Abort();

                _threadCollection.RemoveThread(selectedThreadUuid);
            }
            lvThreads.Items.Clear();
        }


        private void TurnAllBulbsOn()
        {
            var p = new SetPowerPayload(true);

            foreach (var b in _bulbController.Bulbs)
            {
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress, b.IpAddress);
                Thread.Sleep(1);
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress, b.IpAddress);
            }
        }


        private void TurnAllBulbsOff()
        {
            var p = new SetPowerPayload(false);

            foreach (var b in _bulbController.Bulbs)
            {
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress, b.IpAddress);
                Thread.Sleep(1);
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress, b.IpAddress);
            }
        }


        private void Panic()
        {
            StopAllThreads();
            TurnAllBulbsOn();

            var c = new SetColourPayload
            {
                Hue = 0,
                Saturation = 0,
                Brightness = 65535,
                Kelvin = 3000,
                TransitionDuration = 1
            };

            foreach (var b in _bulbController.Bulbs)
            {
                _bulbController.SendPayloadToMacAddress(c, b.MacAddress, b.IpAddress);
                Thread.Sleep(1);
                _bulbController.SendPayloadToMacAddress(c, b.MacAddress, b.IpAddress);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var s = new SoundGeneratorProcessor();
            var thread = new Thread(() => s.SoundGenerator(_bulbController, new Random(_r.Next())));
            StartNewThread(thread, "Sound Generator Thread", s);
            s.ShowUI = true;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                string sURL;
                sURL = @"https://api.github.com/repos/stringandstickytape/MaxLifx/releases";
                string response;

                var webRequest = WebRequest.Create(sURL) as HttpWebRequest;
                webRequest.Method = "GET";
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.KeepAlive = false;
                webRequest.UserAgent = "YourAppName";

                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;

                decimal maxVersion = .7M;

                using (var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    response = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

                var ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                dynamic releaseDetails = null;

                foreach (var x in data)
                {
                    string release = x.tag_name;

                    var thisVersion = decimal.Parse(release, NumberStyles.Any, ci);
                    if (maxVersion < thisVersion)
                    {
                        maxVersion = thisVersion;
                        releaseDetails = x;
                    }
                }

                if (Version < maxVersion)
                {
                    var dialogResult =
                        MessageBox.Show(
                            "Newer version found! Quit MaxLifx and browse to GitHub to download it?\r\n\r\nv" +
                            maxVersion +
                            ":\r\n" + releaseDetails.body, "Update?",
                            MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        Process.Start(@"https://github.com/stringandstickytape/MaxLifx/releases");
                        Application.Exit();
                    }
                }
            }
            catch
            {
            }
        }

        private void turnOnAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnAllBulbsOn();
        }

        private void turnOffAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnAllBulbsOff();
        }

        private void panicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panic();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _bulbController.DiscoverBulbs();

            if (_bulbController.Bulbs.Count == 0)
            {
                MessageBox.Show("No bulbs found. If you have just received a Windows Firewall popup, try Bulbs -> Discover Bulbs now.");
            }
            PopulateBulbListbox();

            _suspendUi = false;
            SaveSettings();
            _suspendUi = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        private delegate void BulbControllerOnColourSetDelegate(object sender, EventArgs eventArgs);

        private delegate void LoadThreadsDelegate(string filename);

        private delegate void StopAllThreadsDelegate();

        private delegate void SetSchedulerTimeTextBoxDelegate(TimeSpan elapsedTime);

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                ShowInTaskbar = false;
                if (notifyIcon1.Icon == null)
                {
                    Bitmap bmp = MaxLifx.Properties.Resources.m__1_;
                    notifyIcon1.Icon = Icon.FromHandle(bmp.GetHicon());
                }
                notifyIcon1.Visible = true;
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseClick(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, e);
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, e);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tbManualBulbMac.Text.Length != 6 || !System.Text.RegularExpressions.Regex.IsMatch(tbManualBulbMac.Text, @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                MessageBox.Show(
                    "Enter the last six characters of the bulb's MAC address.  This can be found in the \"Edit Light\" dialog of the Android app, amongst other places.  For a virtual bulb, just enter 000000.");
                return;
            }
            var n =
                _bulbController.Bulbs.Where(x => x.Label.Length > 12 && x.Label.Substring(0, 12) == "Manual Bulb ")
                    .Select(x => int.Parse(x.Label.Substring(12)));

            int n2 = 1;

            if (n.Count() > 0)
                n2 = n.Max() + 1;

            Bulb b = new Bulb() { Label = "Manual Bulb "+n2, Location = ScreenLocation.All, MacAddress = "D073D5" + tbManualBulbMac.Text.Length};
            _bulbController.Bulbs.Add(b);
            PopulateBulbListbox();
            _suspendUi = false;
            SaveSettings();
            _suspendUi = true;
        }

        private void advancedDiscoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _bulbController.DiscoverBulbs(ConfigurationManager.AppSettings["subnet"]);

            if (_bulbController.Bulbs.Count == 0)
            {
                MessageBox.Show("No bulbs found. If you have just received a Windows Firewall popup, try Bulbs -> Discover Bulbs now.");
            }
            PopulateBulbListbox();

            _suspendUi = false;
            SaveSettings();
            _suspendUi = true;
            //Form2 testDialog = new Form2();
            //
            //// Show testDialog as a modal dialog and determine if DialogResult = OK.
            //if (testDialog.ShowDialog(this) == DialogResult.OK)
            //{
            //    // Read the contents of testDialog's TextBox.
            //    this.txtResult.Text = testDialog.TextBox1.Text;
            //}
            //else
            //{
            //    this.txtResult.Text = "Cancelled";
            //}
            //testDialog.Dispose();
        }
    }
}