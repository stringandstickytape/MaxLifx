using System;
using System.Collections.Generic;
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

    public class SoundResponseProcessor : ProcessorBase
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
                        var bulbs = bulbController.Bulbs.Where(x => x.Zones < 2).Select(x => x.Label).ToList();

                        foreach (var bulbObj in bulbController.Bulbs.Where(x => x.Zones > 1))
                            for (var i = 0; i < bulbObj.Zones; i++)
                                bulbs.Add(bulbObj.Label + $" (Zone {(i + 1).ToString().PadLeft(3,'0')})");

                        var form = new SoundResponseUI(SettingsCast, bulbs, r);
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

                    var homebrewDevicePayloadCache = new Dictionary<(Bulb, int), SetColourPayload>();


                    foreach (var label in SettingsCast.SelectedLabels)
                    {
                        var bulbNumber = SettingsCast.PerBulb ? bulbCtr : 0;

                        // don't raise an exception if there's no input...
                        if (bulbNumber >= SettingsCast.Levels.Count) continue;

                        var bulb = bulbController.GetBulbFromLabel(label, out int zone);
                        

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

                                    float rawLevel;

                                    // don't raise an exception if there's no input...
                                    if (SettingsCast.Bins[bulbNumber] > spectrumEngine.LatestPoints.Count - 1) rawLevel = 0;
                                    else rawLevel = 1 - (spectrumEngine.LatestPoints[SettingsCast.Bins[bulbNumber]].Y / 255);

                                    float adjustedLevel;

                                    if (rawLevel < levelMin)
                                        adjustedLevel = 0;
                                    else if (rawLevel > levelMax)
                                        adjustedLevel = 1;
                                    else
                                    {
                                        adjustedLevel = (rawLevel - levelMin)/levelRange;
                                        if (adjustedLevel < 0 || adjustedLevel > 1) MessageBox.Show("D'oh!");
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


                                if (bulb.IsHomebrewDevice)
                                {
                                    homebrewDevicePayloadCache.Add((bulb, zone), _payload);
                                }
                                else
                                {
                                    bulbController.SetColour(bulb, zone, _payload, true);
                                    if (SettingsCast.Delay > 200)
                                    {
                                        bulbController.SetColour(bulb, zone, _payload, true);
                                        Thread.Sleep(1);

                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Thread.Sleep(1);
                        }




                        Thread.Sleep(1);

                        bulbCtr++;
                    }

                    if (homebrewDevicePayloadCache.Any())
                    {

                        foreach (var group in homebrewDevicePayloadCache.GroupBy(x => (Bulb)x.Key.Item1))
                        {
                            var payloads = group.Select(x => x.Value);
                            Dictionary<int, SetColourPayload> individualPayloads = new Dictionary<int, SetColourPayload>();

                            int ctr = 0;
                            foreach(var p in group)
                            {
                                individualPayloads.Add(p.Key.Item2, p.Value);

                                if (ctr % 10 == 9 || ctr == group.Count() - 1)
                                {
                                    var newPayload = new SetHomebrewColourZonesPayload()
                                    {
                                        IndividualPayloads = individualPayloads
                                    };

                                    MaxLifxBulbController.SendPayloadToMacAddress(newPayload, group.Key.MacAddress, group.Key.IpAddress);
                                    Thread.Sleep(1);

                                    individualPayloads = new Dictionary<int, SetColourPayload>();
                                }

                                ctr++;
                            }




                            // foreach(var p in group.ToDictionary(x => x.Key.Item2, x => x.Value))
                            //     MaxLifxBulbController.SendPayloadToMacAddress(p.Value, group.Key.MacAddress, group.Key.IpAddress);
                        }


                        

                    }

                }

                int diff =(int) (DateTime.Now - SettingsCast.WaveStartTime).TotalMilliseconds;
                if(diff < SettingsCast.Delay)
                    Thread.Sleep(SettingsCast.Delay-diff);
            }

            spectrumEngine.StopCapture();
        }
    }
}