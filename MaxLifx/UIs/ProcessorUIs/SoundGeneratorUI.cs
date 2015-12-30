using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.SoundToken;
using MaxLifx.Threads;

namespace MaxLifx.UIs
{
    public partial class SoundGeneratorUI : UiFormBase
    {
        private readonly int _controlSpacing = 5;
        private readonly SoundGeneratorSettings _settings;
        private int _currentRowHeight;
        private Point _currentSoundControl;

        public SoundGeneratorUI(SoundGeneratorSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            SetupUI();
        }

        private void SetupUI()
        {
            cbConfigs.Items.Clear();
            foreach (var x in Directory.GetFiles(".", "*." + _settings.FileExtension))
            {
                var fileName = x.Replace(".\\", "").Replace("." + _settings.FileExtension, "").Replace(".xml", "");
                cbConfigs.Items.Add(fileName);
            }

            ClearPanel(pLoops);
            ClearPanel(pRandoms);

            _currentSoundControl = new Point(_controlSpacing, _controlSpacing);
            _currentRowHeight = 0;

            foreach (var sound in _settings.Sounds.Where(x => x.SoundType == Sound.SoundTypes.Looping))
            {
                AddSoundControlsToPanel(sound, pLoops);
            }

            _currentSoundControl = new Point(_controlSpacing, _controlSpacing);
            _currentRowHeight = 0;

            foreach (var sound in _settings.Sounds.Where(x => x.SoundType == Sound.SoundTypes.Random))
            {
                AddSoundControlsToPanel(sound, pRandoms);
            }

            tbOnTimes.Text = _settings.OnTimes;
            tbOffTimes.Text = _settings.OffTimes;
        }

        private void AddSoundControlsToPanel(Sound sound, Panel panel)
        {
            //AddSoundControl(new Label() { Width = 60, Text = sound.SoundType.ToString() }, panel);

            var soundButton = new Button
            {
                Width = 50,
                Text = "Start/Stop",
                Tag = sound.UUID,
                BackColor =
                    (sound.Started || sound.StartStopRequest == StartStop.Start)
                        ? Color.PaleVioletRed
                        : SystemColors.Control
            };
            soundButton.Click += StopStart;
            AddSoundControl(soundButton, panel);

            AddSoundControl(new TextBox {Width = 200, Text = sound.Name}, panel);

            AddSoundControl(new Label {Width = 10, Text = "Vol"}, panel);

            var tbVolume = new TrackBar
            {
                Width = 100,
                Height = 10,
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 10,
                Tag = sound.UUID,
                Value = (int) (sound.Volume*100)
            };
            tbVolume.ValueChanged += VolumeChanged;
            AddSoundControl(tbVolume, panel);

            AddSoundControl(new Label {Width = 10, Text = "Pan"}, panel);

            var tbPan = new TrackBar
            {
                Width = 100,
                Height = 10,
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 10,
                Tag = sound.UUID,
                Value = (int) (sound.Volume*100)
            };
            tbPan.ValueChanged += PanChanged;
            ;
            AddSoundControl(tbPan, panel);

            if (sound.SoundType == Sound.SoundTypes.Random)
            {
                AddSoundControl(new Label {Width = 80, Text = "Average every"}, panel);

                var tbFrequency = new TextBox {Width = 50, Text = sound.Frequency.ToString(), Tag = sound.UUID};
                tbFrequency.TextChanged += FrequencyChanged;
                AddSoundControl(tbFrequency, panel);

                AddSoundControl(new Label {Width = 50, Text = "seconds"}, panel);
            }

            StartNewSoundControlRow();
        }

        private void PanChanged(object sender, EventArgs e)
        {
            var uuid = ((TrackBar) sender).Tag.ToString();
            var sound = _settings.GetSoundFromUUID(uuid);
            lock (_settings.Messages)
            {
                _settings.Messages.Add(new SoundMessage
                {
                    SoundMessageType = SoundMessageTypes.SetPan,
                    ParameterType = typeof (int),
                    Parameter = ((TrackBar) sender).Value,
                    SoundUUID = uuid
                });
            }
        }

        private void FrequencyChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(((TextBox) sender).Text, out i))
            {
                var uuid = ((TextBox) sender).Tag.ToString();
                var sound = _settings.GetSoundFromUUID(uuid);
                sound.Frequency = i;
            }
        }

        private void VolumeChanged(object sender, EventArgs e)
        {
            var uuid = ((TrackBar) sender).Tag.ToString();
            var sound = _settings.GetSoundFromUUID(uuid);
            lock (_settings.Messages)
            {
                _settings.Messages.Add(new SoundMessage
                {
                    SoundMessageType = SoundMessageTypes.SetVolume,
                    ParameterType = typeof (int),
                    Parameter = ((TrackBar) sender).Value,
                    SoundUUID = uuid
                });
            }
        }

        private void ClearPanel(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c.GetType() == typeof (Button)) ((Button) c).Click -= StopStart;
                if (c.GetType() == typeof (TrackBar)) ((TrackBar) c).ValueChanged -= VolumeChanged;
                c.Dispose();
            }

            control.Controls.Clear();
        }

        private void AddSoundControl(Control control, Control panelToAddTo)
        {
            control.Location = _currentSoundControl;

            switch (control.GetType().ToString())
            {
                case "System.Windows.Forms.Label":
                    control.Location = new Point(control.Location.X, control.Location.Y + 2);
                    break;
                case "System.Windows.Forms.Button":
                    control.Location = new Point(control.Location.X, control.Location.Y - 2);
                    break;
                default:
                    break;
            }

            panelToAddTo.Controls.Add(control);
            _currentSoundControl.X += control.Width + _controlSpacing;
            _currentRowHeight = _currentRowHeight < control.Height ? control.Height : _currentRowHeight;
        }

        private void StartNewSoundControlRow()
        {
            _currentSoundControl.X = _controlSpacing;
            _currentSoundControl.Y = _currentSoundControl.Y + _currentRowHeight; // + _controlSpacing;
            _currentRowHeight = 0;
        }

        private void StopStart(object sender, EventArgs e)
        {
            var sound = _settings.GetSoundFromUUID(((Button) sender).Tag.ToString());

            if (sound.Started)
            {
                sound.StartStopRequest = StartStop.Stop;
                ((Button) sender).BackColor = SystemColors.Control;
            }
            else
            {
                sound.StartStopRequest = StartStop.Start;
                ((Button) sender).BackColor = Color.PaleVioletRed;
            }
        }

        //private void ToggleLoopPlayback(SoundToken.Sound Sound)
        //{

        //}

        private void bSave_Click(object sender, EventArgs e)
        {
            var t = new Thread(() =>
            {
                var s = new SaveFileDialog {DefaultExt = _settings.FileExtension};
                s.Filter = "XML files (*." + _settings.FileExtension + ")|*." + _settings.FileExtension;
                s.InitialDirectory = Directory.GetCurrentDirectory();
                s.AddExtension = true;

                var result = s.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var filename = s.FileName;
                    if (string.IsNullOrEmpty(filename))
                        filename = _settings.GetType().Name + "Default.maxlifx.xml";

                    var xml = new XmlSerializer(typeof (SoundGeneratorSettings));

                    using (var stream = new MemoryStream())
                    {
                        xml.Serialize(stream, _settings);
                        stream.Position = 0;
                        var xmlDocument = new XmlDocument();
                        xmlDocument.Load(stream);
                        xmlDocument.Save(filename);
                    }
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void cbConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (_settings.Sounds.Any(x => x.WaveOut != null))
            {
                foreach (var sound in _settings.Sounds.Where(x => x.WaveOut != null))
                {
                    sound.StartStopRequest = StartStop.Stop;
                }
                Thread.Sleep(1);
            }
            var s = new SoundGeneratorSettings();
            ProcessorBase.LoadSettings(ref s, cbConfigs.SelectedItem + "." + _settings.FileExtension);

            _settings.SelectedLabels = s.SelectedLabels;
            _settings.OnTimes = s.OnTimes;
            _settings.OffTimes = s.OffTimes;
            _settings.Volume = s.Volume;
            _settings.Sounds = s.Sounds;

            // start any sounds as necessary
            foreach (var sound in _settings.Sounds.Where(x => x.Started && x.WaveOut == null))
            {
                sound.StartStopRequest = StartStop.Start;
            }

            SetupUI();
        }

        private void tbOffTimes_TextChanged(object sender, EventArgs e)
        {
            _settings.OffTimes = tbOffTimes.Text;
        }

        private void tbOnTimes_TextChanged(object sender, EventArgs e)
        {
            _settings.OnTimes = tbOnTimes.Text;
        }
    }
}