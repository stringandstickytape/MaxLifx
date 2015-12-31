using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using MaxLifx.Controllers;
using MaxLifx.Controls;
using MaxLifx.Payload;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.Threads;
using MaxLifx.UIs;
using NAudio.CoreAudioApi;

namespace MaxLifx
{
    public enum WaveTypes
    {
        Sine,
        Square,
        Sawtooth,
        Audio,
        Noise
    }

    public class SoundResponseProcessor : ProcessorBase, IProcessor
    {
        private Random r;

        [XmlIgnore]
        public SoundResponseSettings SettingsCast
        {
            get { return ((SoundResponseSettings) Settings); }
        }

        public override ISettings Settings { get; set; }

        public override string SettingsAsXml
        {
            get { return ((SoundResponseSettings) (Settings)).ToXmlString(); }
            set
            {
                var s = new SoundResponseSettings();

                using (var st = new StringReader(value))
                {
                    s = (SoundResponseSettings) (new XmlSerializer(typeof (SoundResponseSettings)).Deserialize(st));
                }

                Settings = s;
            }
        }

        public void SoundResponse(MaxLifxBulbController bulbController, Random random)
        {
            r = random;

            if (Settings == null)
            {
                Settings = new SoundResponseSettings();
                SettingsCast.Hues.Add(0);
                SettingsCast.HueRanges.Add(0);
                SettingsCast.Saturations.Add(0);
                SettingsCast.SaturationRanges.Add(0);
                SettingsCast.Brightnesses.Add(.9f);
                SettingsCast.BrightnessRanges.Add(.1f);
                SettingsCast.Levels.Add(50);
                SettingsCast.LevelRanges.Add(25);
                SettingsCast.Bins.Add(10);
            }

            var de = new MMDeviceEnumerator();
            var device = de.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            DoMainLoop(bulbController, device);
        }

        private void DoMainLoop(MaxLifxBulbController bulbController, MMDevice device)
        {
            SettingsCast.WaveStartTime = DateTime.Now;

            var persistentFloatH = (float) r.NextDouble();
            var persistentFloatS = (float) r.NextDouble();
            var persistentFloatB = (float) r.NextDouble();
            var persistedSince = DateTime.Now;

            var spectrumEngine = new SpectrumAnalyserEngine();
            spectrumEngine.StartCapture();

            while (!TerminateThread)
            {
                if (ShowUI)
                {
                    var t = new Thread(() =>
                    {
                        var form = new SoundResponseUI(SettingsCast, bulbController.Bulbs.Select(x => x.Label).ToList(), r);
                            /* (SettingsCast, bulbController.Bulbs);*/
                        form.ShowDialog();
                    });
                    t.Start();
                    ShowUI = false;
                }

                var _offOrOn = Settings.OffOrOn();
                if (_offOrOn)
                {
                    ushort brightness = 0;
                    ushort saturation = 0;
                    var _hue = 0;
                    var timeRunning = DateTime.Now - SettingsCast.WaveStartTime;

                    var floatValueH = 0f;
                    var floatValueS = 0f;
                    var floatValueB = 0f;

                    var bulbCtr = 0;
                    foreach (var label in SettingsCast.SelectedLabels)
                    {
                        var bulbNumber = SettingsCast.PerBulb ? bulbCtr : 0;

                        try
                        {

                            switch (SettingsCast.WaveType)
                            {
                                case WaveTypes.Audio:
                                    var levelMin = 1 -
                                                   (((float) (SettingsCast.Levels[bulbNumber] + SettingsCast.LevelRanges[bulbNumber]/ 2 > 255 ? 255 : SettingsCast.Levels[bulbNumber] + SettingsCast.LevelRanges[bulbNumber] / 2) /
                                                     255));
                                    var levelMax = 1 -
                                                   (((float) (SettingsCast.Levels[bulbNumber] - SettingsCast.LevelRanges[bulbNumber] / 2 < 0 ? 0 : SettingsCast.Levels[bulbNumber] - SettingsCast.LevelRanges[bulbNumber] / 2) /255));
                                    var levelRange = levelMax - levelMin;

                                    var rawLevel = 1 - (spectrumEngine.LatestPoints[SettingsCast.Bins[bulbNumber]].Y/255);

                                    float adjustedLevel;

                                    if (rawLevel < levelMin)
                                        adjustedLevel = 0;
                                    else if (rawLevel > levelMax)
                                        adjustedLevel = 1;
                                    else
                                    {
                                        adjustedLevel = (rawLevel - levelMin)/levelRange;
                                        if (adjustedLevel < 0 || adjustedLevel > 1) MessageBox.Show("Doh!");
                                    }

                                    floatValueH = floatValueS = floatValueB = adjustedLevel;
                                        // device.AudioMeterInformation.MasterPeakValue;
                                    break;
                                case WaveTypes.Sine:
                                    floatValueH =
                                        floatValueS =
                                            floatValueB =
                                                (float)
                                                    (Math.Sin(timeRunning.TotalSeconds*6.283*500/SettingsCast.WaveDuration) +
                                                     1)/2;
                                    break;
                                case WaveTypes.Square:
                                    floatValueH =
                                        floatValueS =
                                            floatValueB =
                                                ((int) (timeRunning.TotalMilliseconds/SettingsCast.WaveDuration))%2;
                                    break;
                                case WaveTypes.Sawtooth:
                                    floatValueH =
                                        floatValueS =
                                            floatValueB =
                                                ((float) timeRunning.TotalMilliseconds -
                                                 (((int) timeRunning.TotalMilliseconds/SettingsCast.WaveDuration)*
                                                  SettingsCast.WaveDuration))/SettingsCast.WaveDuration;
                                    break;
                                case WaveTypes.Noise:
                                    var span = DateTime.Now - persistedSince;
                                    if (span.TotalMilliseconds > SettingsCast.WaveDuration)
                                    {
                                        floatValueH = (float) r.NextDouble();
                                        floatValueS = (float) r.NextDouble();
                                        floatValueB = (float) r.NextDouble();
                                        persistentFloatH = floatValueH;
                                        persistentFloatS = floatValueS;
                                        persistentFloatB = floatValueB;
                                        persistedSince = DateTime.Now;
                                    }
                                    else
                                    {
                                        floatValueH = persistentFloatH;
                                        floatValueS = persistentFloatS;
                                        floatValueB = persistentFloatB;
                                    }
                                    break;
                            }

                            if (SettingsCast.Hues.Count > bulbNumber)
                            {
                                brightness =
                                    (ushort)
                                        (((SettingsCast.BrightnessInvert ? 1 - floatValueB : floatValueB) *
                                          SettingsCast.BrightnessRanges[bulbNumber] * 2 +
                                          (SettingsCast.Brightnesses[bulbNumber] -
                                           SettingsCast.BrightnessRanges[bulbNumber])) *
                                         65535);
                                saturation =
                                    (ushort)
                                        (((SettingsCast.SaturationInvert ? 1 - floatValueS : floatValueS)*
                                          SettingsCast.SaturationRanges[bulbNumber]*2 +
                                          (SettingsCast.Saturations[bulbNumber] -
                                           SettingsCast.SaturationRanges[bulbNumber]))*
                                         65535);
                                _hue =
                                    ((int)
                                        ((SettingsCast.HueInvert ? 1 - floatValueH : floatValueH)*
                                         SettingsCast.HueRanges[bulbNumber]*2 +
                                         (SettingsCast.Hues[bulbNumber] - SettingsCast.HueRanges[0])) + 720)%360;
                                var _payload = new SetColourPayload
                                {
                                    Hue = _hue,
                                    Saturation = saturation,
                                    Brightness = brightness,
                                    Kelvin = SettingsCast.Kelvin,
                                    TransitionDuration = (uint) SettingsCast.TransitionDuration
                                };

                                bulbController.SetColour(label, _payload);
                               if (SettingsCast.Delay > 200)
                               {
                                   bulbController.SetColour(label, _payload);
                                   Thread.Sleep(1);
                               
                               }
                            }
                            else
                            {
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            Thread.Sleep(1);
                        }
                        Thread.Sleep(1);

                        bulbCtr++;
                    }
                }
                Thread.Sleep(SettingsCast.Delay);
            }
        }
    }
}