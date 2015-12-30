using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MaxLifx.Controllers;
using MaxLifx.Controls;
using MaxLifx.Payload;
using MaxLifx.Scheduler;
using MaxLifx.Threads;
using MaxLifx.UIs;
using NAudio.Wave;
using Newtonsoft.Json;

namespace MaxLifx
{
    public partial class MainForm : Form
    {
        public readonly decimal Version = 0.2m;

        private readonly MaxLifxBulbController _bulbController = new MaxLifxBulbController();
        private readonly Random r = new Random();
        private readonly bool _suspendUi = true;
        private readonly LightControlThreadCollection _threadCollection = new LightControlThreadCollection();
        private MaxLifxSettings _settings = new MaxLifxSettings();
        private System.Timers.Timer _schedulerTimer = new System.Timers.Timer();
        private DateTime _schedulerStartTime;

        public MainForm()
        {
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
            _bulbController.SetupNetwork();

            if (File.Exists("Settings.xml"))
            {
                LoadSettings();

                PopulateBulbListbox();


                for (var i = 0; i < lbBulbs.Items.Count; i++)
                    if (_settings.SelectedLabels.Contains(lbBulbs.Items[i].ToString()))
                        lbBulbs.SelectedItems.Add(lbBulbs.Items[i]);
            }

            _suspendUi = false;
            _bulbController.ColourSet += BulbControllerOnColourSet;

        }
        delegate void BulbControllerOnColourSetDelegate(object sender, EventArgs eventArgs);

        private int thumbSize = 100;
        private void BulbControllerOnColourSet(object sender, EventArgs eventArgs)
        {
            if (InvokeRequired) // Line #1
            {
                BulbControllerOnColourSetDelegate d = BulbControllerOnColourSet;
                Invoke(d, sender, eventArgs);
                return;
            }

            LabelAndColourPayload details = ((LabelAndColourPayload) sender);

            bool pbFound = false;
            foreach (var c in panelBulbColours.Controls)
            {
                PictureBox pb = c as PictureBox;
                if (pb != null)
                {
                    if (pb.Name == "pb" + details.Label)
                    {
                        pb.BackColor = Utils.HsbToRgb(details.Payload.Hue, details.Payload.Saturation / 65535.0f,
                                details.Payload.Brightness/65535.0f);

                        pbFound = true;
                        break;
                    }
                }
            }

            if (!pbFound)
            {
                var controls = panelBulbColours.Controls;
                int nextX = 0;
                
                if(controls.Count > 0)
                    nextX = controls[controls.Count - 1].Location.X + thumbSize + 10;

                Point newLocation = new Point(nextX, 0);
                Point newLabelLocation = new Point(nextX, thumbSize + 10);

                panelBulbColours.Controls.Add(new PictureBox { Name = "pb" + details.Label, Location = newLocation, Size = new Size(thumbSize, thumbSize) });
                Button b = new Button {Name = "lbl" + details.Label, Location = newLabelLocation, Text = details.Label, Width = 100};
                b.Click += B_Click;
                panelBulbColours.Controls.Add(b);

            }
        }

        private void B_Click(object sender, EventArgs e)
        {
            var fakeBulb = new FakeBulb(_bulbController, ((Button)sender).Text) { Text = ((Button)sender).Text };
            fakeBulb.Show();
        }

        private void StartNewThread(Thread thread, string threadName, IProcessor processor)
        {
            //thread.SetApartmentState(ApartmentState.STA);

            var newThread = thread;
            var _newLightThread = _threadCollection.AddThread(newThread, threadName, processor);

            var lvi = new ListViewItem(_newLightThread.m_Name);
            lvi.SubItems.Add(_newLightThread.UUID);
            lvThreads.Items.Add(lvi);
            _newLightThread.Start();
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
            foreach (var b in _bulbController.Bulbs.OrderBy(x => x.Label))
                lbBulbs.Items.Add(b.Label);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var processor = new ScreenColourProcessor();
            var thread = new Thread(() => processor.ScreenColour(_bulbController, new Random(r.Next())));
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

        private void bRediscover_Click(object sender, EventArgs e)
        {
            _bulbController.DiscoverBulbs();

            SaveSettings();

            PopulateBulbListbox();
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

        delegate void LoadThreadsDelegate(string filename);

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
                            var threadName = "";

                            switch (lightThread.m_Processor.GetType().ToString())
                            {
                                case "MaxLifx.SoundResponseProcessor":
                                    t =
                                        new Thread(
                                            () =>
                                                ((SoundResponseProcessor) (lightThread.m_Processor)).SoundResponse(
                                                    _bulbController, new Random(r.Next())));
                                    break;
                                case "MaxLifx.ScreenColourProcessor":
                                    t =
                                        new Thread(
                                            () =>
                                                ((ScreenColourProcessor) (lightThread.m_Processor)).ScreenColour(
                                                    _bulbController, new Random(r.Next())));
                                    break;
                                case "MaxLifx.SoundGeneratorProcessor":
                                    t =
                                        new Thread(
                                            () =>
                                                ((SoundGeneratorProcessor) (lightThread.m_Processor)).SoundGenerator(
                                                    _bulbController, new Random(r.Next())));
                                    break;
                            }

                            threadName = lightThread.m_Name;
                            StartNewThread(t, threadName, lightThread.m_Processor);
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
                var xml = new XmlSerializer(typeof (MaxLifxSettings));
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
            var thread = new Thread(() => s.SoundResponse(_bulbController, new Random(r.Next())));
            StartNewThread(thread, "Sound Response Thread", s);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (lvThreads.SelectedItems.Count == 0) return;
            var selectedThreadUUID = lvThreads.SelectedItems[0].SubItems[1].Text;
            if (lvThreads == null) return;
            var thread = _threadCollection.GetThread(selectedThreadUUID);
            thread.Abort();
            _threadCollection.RemoveThread(selectedThreadUUID);
            lvThreads.Items.Remove(lvThreads.SelectedItems[0]);
        }

        private void lvThreads_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string selectedThreadUUID;
            if (lvThreads.SelectedItems.Count == 0)
            {
                if (lvThreads.Items.Count == 1)
                {
                    selectedThreadUUID = lvThreads.Items[0].SubItems[1].Text;
                }
                else return;
            }
            else selectedThreadUUID = lvThreads.SelectedItems[0].SubItems[1].Text;
            if (lvThreads == null) return;
            var thread = _threadCollection.GetThread(selectedThreadUUID);
            thread.m_Processor.ShowUI = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = new SaveFileDialog {DefaultExt = ".MaxLifx.Threadset.xml" };
            s.Filter = "XML files (*.MaxLifx.Threadset.xml)|*.MaxLifx.Threadset.xml";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.AddExtension = true;

            if (s.ShowDialog() == DialogResult.OK)
                SaveThreads(s.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopAllThreads();

            var s = new OpenFileDialog {DefaultExt = ".MaxLifx.Threadset.xml" };
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

        delegate void StopAllThreadsDelegate();

        private void StopAllThreads()
        {
            if (InvokeRequired) // Line #1
            {
                StopAllThreadsDelegate d = StopAllThreads;
                Invoke(d);
            }

            for (var i = 0; i < lvThreads.Items.Count; i++)
            {
                var selectedThreadUUID = lvThreads.Items[i].SubItems[1].Text;
                var thread = _threadCollection.GetThread(selectedThreadUUID);
                thread.Abort();

                _threadCollection.RemoveThread(selectedThreadUUID);
            }
            lvThreads.Items.Clear();
        }

        private void bTurnOn_Click(object sender, EventArgs e)
        {
            TurnAllBulbsOn();
        }

        private void TurnAllBulbsOn()
        {
            var p = new SetPowerPayload(true);

            foreach (var b in _bulbController.Bulbs)
            {
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress);
                Thread.Sleep(1);
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress);
            }
        }

        private void bTurnOff_Click(object sender, EventArgs e)
        {
            TurnAllBulbsOff();
        }

        private void TurnAllBulbsOff()
        {
            var p = new SetPowerPayload(false);

            foreach (var b in _bulbController.Bulbs)
            {
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress);
                Thread.Sleep(1);
                _bulbController.SendPayloadToMacAddress(p, b.MacAddress);
            }
        }

        private void bPanic_Click(object sender, EventArgs e)
        {
            Panic();
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
                _bulbController.SendPayloadToMacAddress(c, b.MacAddress);
                Thread.Sleep(1);
                _bulbController.SendPayloadToMacAddress(c, b.MacAddress);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var s = new SoundGeneratorProcessor();
            var thread = new Thread(() => s.SoundGenerator(_bulbController, new Random(r.Next())));
            StartNewThread(thread, "Sound Generator Thread", s);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            foreach (var evt in timeline1.TimelineEvents)
                evt.Fired = false;

            _schedulerTimer.Elapsed += OnSchedulerElapsed;
            _schedulerTimer.Interval = 100;
            _schedulerTimer.Enabled = true;
            _schedulerStartTime = DateTime.Now;
        }

        private WaveOut _schedulerWaveOut;
        private Mp3FileReader _schedulerReader;
        private void OnSchedulerElapsed(object sender, ElapsedEventArgs e)
        {
            var elapsedTime = DateTime.Now - _schedulerStartTime;
            SetSchedulerTimeTextBox(elapsedTime);

            timeline1.PlaybackTime = (float)(elapsedTime.TotalMilliseconds);

            if ((timeline1.PlaybackTime > (timeline1.ViewableWindowSize / 2 + timeline1.ViewableWindow.X))
                ||
                (timeline1.PlaybackTime < ( timeline1.ViewableWindow.X - timeline1.ViewableWindowSize / 2)))
            {
                var windowSize = timeline1.ViewableWindowSize;
                
                timeline1.ViewableWindow.X = timeline1.PlaybackTime - windowSize / 2;
                timeline1.ViewableWindow.Y = timeline1.PlaybackTime + windowSize / 2;
            }

            foreach (var evt in timeline1.TimelineEvents.Where(x => x.Fired == false && x.Time < elapsedTime.TotalMilliseconds))
            {
                evt.Fired = true;

                switch (evt.Action)
                {
                    case TimelineEventAction.StartThreadSet:
                        StopAllThreads();
                        LoadThreads(evt.Parameter);
                        break;
                    case TimelineEventAction.PlayMp3:
                        PlayMp3(evt, TimeSpan.Zero);
                        break;
                    case TimelineEventAction.Unspecified:
                        break;

                }

            }
        }

        private void PlayMp3(TimelineEvent evt, TimeSpan startTime)
        {
            if (_schedulerWaveOut != null)
            {
                _schedulerWaveOut.Stop();

                if (_schedulerReader != null)
                {
                    _schedulerReader.Close();
                    _schedulerReader.Dispose();
                }

                _schedulerWaveOut.Dispose();
                _schedulerWaveOut = null;
            }

            _schedulerReader = new Mp3FileReader(evt.Parameter);
            _schedulerWaveOut = new WaveOut(); // or WaveOutEvent()

            _schedulerReader.CurrentTime = startTime;
            
            _schedulerWaveOut.Init(_schedulerReader);
            _schedulerWaveOut.Play();
        }

        delegate void SetSchedulerTimeTextBoxDelegate(TimeSpan elapsedTime);


        private void SetSchedulerTimeTextBox(TimeSpan elapsedTime)
        {
            if (InvokeRequired) // Line #1
            {
                SetSchedulerTimeTextBoxDelegate d = SetSchedulerTimeTextBox;
                Invoke(d, elapsedTime);
                return;
            }
            tbSchedTime.Text = ((((int)elapsedTime.TotalMilliseconds)/100)/10f).ToString();
        }

        private TimeSpan schedulerTimeElapsed;
        private void bStopSched_Click(object sender, EventArgs e)
        {



        }

        private void bSaveSched_Click(object sender, EventArgs e)
        {
            var s = new SaveFileDialog {DefaultExt = ".MaxLifx.Sequence.xml"};
            s.Filter = "XML files (*.MaxLifx.Sequence.xml)|*.MaxLifx.Sequence.xml";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.AddExtension = true;

            if (s.ShowDialog() == DialogResult.OK)
            {
                var xml = new XmlSerializer(typeof (List<TimelineEvent>));
                using (var stream = new MemoryStream())
                {
                    xml.Serialize(stream, timeline1.TimelineEvents);
                    stream.Position = 0;
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(stream);
                    xmlDocument.Save(s.FileName);
                    stream.Close();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var s = new OpenFileDialog { DefaultExt = ".MaxLifx.Sequence.xml" };
            s.Filter = "XML files (*.MaxLifx.Sequence.xml)|*.MaxLifx.Sequence.xml";
            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.AddExtension = true;

            if (s.ShowDialog() == DialogResult.OK)
            {
                var serializer = new XmlSerializer(typeof (List<TimelineEvent>));
                using (XmlReader reader = new XmlTextReader(s.FileName))
                {
                    timeline1.TimelineEvents = (List<TimelineEvent>) serializer.Deserialize(reader);
                    timeline1.Invalidate();
                    reader.Close();
                }
                foreach (var evt in timeline1.TimelineEvents)
                    timeline1.PopulateBitmapCache(evt);
            }
        }


        private void bTimelineAdd_Click(object sender, EventArgs e)
        {
            var evt = new TimelineEvent
            {
                Time = (long)timeline1.PlaybackTime,
                Parameter = @"",
                Action = TimelineEventAction.Unspecified
            };
            timeline1.AddEvent(evt);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            TimelineEvent eventToEdit;
            bool editMultiple = false;

            if (timeline1.SelectedEvents.Count != 1)
            {
                eventToEdit = new TimelineEvent();
                editMultiple = true;
            }
            else eventToEdit = timeline1.SelectedEvents.Single();

            var f = new EditTimelineEvent(eventToEdit, editMultiple);
            f.ShowDialog();

            if (editMultiple)
            {
                foreach (var evt in timeline1.SelectedEvents)
                {
                    evt.Action = f.EditEvent.Action;
                    evt.Parameter = f.EditEvent.Parameter;
                    timeline1.PopulateBitmapCache(evt);
                }
            }
            else timeline1.PopulateBitmapCache(f.EditEvent);
           
            timeline1.Invalidate();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            foreach (var evt in timeline1.SelectedEvents)
                timeline1.DeleteEvent(evt);

            timeline1.SelectedEvents.Clear();
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            if (!_schedulerTimer.Enabled)
            {
                _schedulerTimer.Elapsed += OnSchedulerElapsed;
                _schedulerTimer.Interval = 100;
                _schedulerTimer.Enabled = true;

                _schedulerStartTime = DateTime.Now - new TimeSpan(0, 0, 0, 0, (int) timeline1.PlaybackTime);

                foreach (
                    var evt in
                        timeline1.TimelineEvents.Where(
                            x =>
                                x.Action == TimelineEventAction.PlayMp3 && x.Time < timeline1.PlaybackTime &&
                                (x.Time + timeline1.WaveBitmapDurations[x.Parameter]*1000 > timeline1.PlaybackTime)))
                {
                    //var start
                    var startTime = timeline1.PlaybackTime - evt.Time;
                    var startTimeSpan = new TimeSpan(0, 0, 0, 0, (int) startTime);
                    PlayMp3(evt, startTimeSpan);
                }
            }
            else
            {
                schedulerTimeElapsed = DateTime.Now - _schedulerStartTime;
                _schedulerTimer.Enabled = false;
                _schedulerTimer.Dispose();
                _schedulerTimer = new System.Timers.Timer();

                if (_schedulerWaveOut != null)
                {
                    _schedulerWaveOut.Stop();

                    if (_schedulerReader != null)
                    {
                        _schedulerReader.Close();
                        _schedulerReader.Dispose();
                    }

                    _schedulerWaveOut.Dispose();
                    _schedulerWaveOut = null;
                }
            }
        }

        private bool collapseToggle = false;
        private void bCollapseMonitors_Click(object sender, EventArgs e)
        {
            if (collapseToggle)
            {
                gbMonitors.Size = new Size(753, 166);
                gbSequencer.Location = new Point(gbSequencer.Location.X, gbSequencer.Location.Y + 146);
                MaximumSize = new Size(Size.Width, Size.Height + 146);
                MinimumSize = new Size(Size.Width, Size.Height + 146);
                Size = new Size(Size.Width, Size.Height + 146);
                bCollapseSequencer.Location = new Point(bCollapseSequencer.Location.X, bCollapseSequencer.Location.Y + 146);
            }
            else
            {
                gbMonitors.Size = new Size(753, 18);
                gbSequencer.Location = new Point(gbSequencer.Location.X, gbSequencer.Location.Y - 146);
                MinimumSize = new Size(Size.Width, Size.Height - 146);
                MaximumSize = new Size(Size.Width, Size.Height - 146);
                Size = new Size(Size.Width, Size.Height- 146);
                bCollapseSequencer.Location = new Point(bCollapseSequencer.Location.X, bCollapseSequencer.Location.Y - 146);
            }

            collapseToggle = !collapseToggle;
        }

        private bool collapseSequencerToggle = true;
        private void bCollapseSequencer_Click(object sender, EventArgs e)
        {
            if (collapseSequencerToggle)
            {
                gbSequencer.Size = new Size(753, 309);
                MaximumSize = new Size(Size.Width, Size.Height + 291);
                MinimumSize = new Size(Size.Width, Size.Height + 291);
                Size = new Size(Size.Width, Size.Height + 291);
            }
            else
            {
                gbSequencer.Size = new Size(753, 18);
                MinimumSize = new Size(Size.Width, Size.Height - 291);
                MaximumSize = new Size(Size.Width, Size.Height - 291);
                Size = new Size(Size.Width, Size.Height - 291);
            }

            collapseSequencerToggle = !collapseSequencerToggle;
        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string sURL;
            sURL = @"https://api.github.com/repos/stringandstickytape/MaxLifx/releases";
            string response;

            HttpWebRequest webRequest = System.Net.WebRequest.Create(sURL) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "YourAppName";

            decimal maxVersion = -1;

            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                response = responseReader.ReadToEnd();

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
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
                var dialogResult = MessageBox.Show("Newer version found! Quit MaxLifx and browse to GitHub to download it?\r\n\r\nv" + maxVersion + ":\r\n" + releaseDetails.body, "Update?",
                    MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start(@"https://github.com/stringandstickytape/MaxLifx/releases");
                    Application.Exit();
                }
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

            SaveSettings();

            PopulateBulbListbox();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }
    }
}