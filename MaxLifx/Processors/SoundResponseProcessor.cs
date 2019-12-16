using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using MaxLifx.Controllers;
using MaxLifx.Controls;
using MaxLifx.Payload;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.Threads;
using MaxLifx.UIs;
using MaxLifxBulbControllerCache;
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
            get { return Settings as SoundResponseSettings; }
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
            var reusableHomebrewClientDictionary = new Dictionary<string,UdpClient>();

            SettingsCast.WaveStartTime = DateTime.Now;

            var persistentFloatH = (float) r.NextDouble();
            var persistentFloatS = (float) r.NextDouble();
            var persistentFloatB = (float) r.NextDouble();
            var persistedSince = DateTime.Now;

            var spectrumEngine = new SpectrumAnalyserEngine();
            spectrumEngine.StartCapture();
            var sc = SettingsCast;
            while (!TerminateThread)
            {
                var passStart = DateTime.Now;
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
                    TimeSpan timeRunning = DateTime.Now - sc.WaveStartTime;

                    var floatValueH = 0f;
                    var floatValueS = 0f;
                    var floatValueB = 0f;

                    var bulbCtr = 0;

                    var homebrewDevicePayloadCache = new HomebrewDevicePayloadCache();


                    foreach (var label in sc.SelectedLabels)
                    {
                        var bulbNumber = sc.PerBulb ? bulbCtr : 0;

                        // don't raise an exception if there's no input...
                        if (bulbNumber >= sc.Levels.Count) 
                            continue;

                        var bulb = bulbController.GetBulbFromLabel(label, out int zone);
                        

                        try
                        {
                            

                            switch (sc.WaveType)
                            {
                                case WaveTypes.Audio:
                                    var halfRange = sc.LevelRanges[bulbNumber] >> 1;
                                    var centre = sc.Levels[bulbNumber] + halfRange;

                                    var levelMin = centre > 255 ? 255 : 255 - centre;

                                    var levelMax =  centre < 0 ? 0 : 255 - sc.Levels[bulbNumber] + halfRange;

                                    var levelRange = levelMax - levelMin;

                                    int rawLevel;

                                    // don't raise an exception if there's no input...
                                    if (sc.Bins[bulbNumber] > spectrumEngine.LatestPoints.Count - 1) 
                                        rawLevel = 0;

                                    else rawLevel = 255 - (spectrumEngine.LatestPoints[sc.Bins[bulbNumber]].Y);

                                    float adjustedLevel;

                                    if (rawLevel < levelMin)
                                        adjustedLevel = 0;
                                    else if (rawLevel > levelMax)
                                        adjustedLevel = 255;
                                    else
                                    {
                                        adjustedLevel = (rawLevel - levelMin)/(float)levelRange;
                                    }

                                    floatValueH = floatValueS = floatValueB = adjustedLevel;
                                    break;
                                case WaveTypes.Sine:
                                    floatValueH =
                                        floatValueS =
                                            floatValueB =
                                                (float)
                                                    (Math.Sin(timeRunning.TotalSeconds*6.283*500/sc.WaveDuration) +
                                                     1)/2;
                                    break;
                                case WaveTypes.Square:
                                    floatValueH =
                                        floatValueS =
                                            floatValueB =
                                                ((int) (timeRunning.TotalMilliseconds/sc.WaveDuration))%2;
                                    break;
                                case WaveTypes.Sawtooth:
                                    floatValueH =
                                        floatValueS =
                                            floatValueB =
                                                ((float) timeRunning.TotalMilliseconds -
                                                 (((int) timeRunning.TotalMilliseconds/sc.WaveDuration)*
                                                  sc.WaveDuration))/sc.WaveDuration;
                                    break;
                                case WaveTypes.Noise:
                                    var span = DateTime.Now - persistedSince;
                                    //if (span.TotalMilliseconds > sc.WaveDuration)
                                    //{
                                        floatValueH = (float) r.NextDouble();
                                        floatValueS = (float) r.NextDouble();
                                        floatValueB = (float) r.NextDouble();
                                        //persistentFloatH = floatValueH;
                                        //persistentFloatS = floatValueS;
                                        //persistentFloatB = floatValueB;
                                        //persistedSince = DateTime.Now;
                                    //}
                                    //else
                                    //{
                                    //    floatValueH = persistentFloatH;
                                   //    floatValueS = persistentFloatS;
                                    //    floatValueB = persistentFloatB;
                                    //}
                                    break;
                            }

                            if (sc.Hues.Count > bulbNumber)
                            {
                                brightness =
                                    (ushort)
                                        (((sc.BrightnessInvert ? 1 - floatValueB : floatValueB) *
                                          sc.BrightnessRanges[bulbNumber] * 2 +
                                          (sc.Brightnesses[bulbNumber] -
                                           sc.BrightnessRanges[bulbNumber])) *
                                         65535);
                                saturation =
                                    (ushort)
                                        (((sc.SaturationInvert ? 1 - floatValueS : floatValueS)*
                                          sc.SaturationRanges[bulbNumber]*2 +
                                          (sc.Saturations[bulbNumber] -
                                           sc.SaturationRanges[bulbNumber]))*
                                         65535);
                                _hue =
                                    ((int)
                                        ((sc.HueInvert ? 1 - floatValueH : floatValueH)*
                                         sc.HueRanges[bulbNumber]*2 +
                                         (sc.Hues[bulbNumber] - sc.HueRanges[0])) + 720)%360;

                                var _payload = new SetColourPayload
                                {
                                    Hue = _hue,
                                    Saturation = saturation,
                                    Brightness = brightness,
                                    Kelvin = sc.Kelvin,
                                    TransitionDuration = (uint)sc.TransitionDuration
                                };

                                if (bulb.IsHomebrewDevice)
                                {
                                    homebrewDevicePayloadCache.Payloads.Add((bulb, zone), _payload);
                                }
                                else
                                {
                                    bulbController.SetColour(bulb, zone, _payload, true);
                                    if (sc.Delay > 200)
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





                        bulbCtr++;
                    }

                    homebrewDevicePayloadCache.Send(reusableHomebrewClientDictionary, bulbController);



                }
                
                int diff =(int) (DateTime.Now - passStart).TotalMilliseconds;
                if(diff < sc.Delay)
                    Thread.Sleep(sc.Delay-diff);
            }

            foreach(var i in reusableHomebrewClientDictionary)
            {
                i.Value.Close();
            }

            spectrumEngine.StopCapture();
        }
    }
}