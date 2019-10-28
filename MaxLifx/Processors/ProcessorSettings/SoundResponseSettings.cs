using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaxLifx.Processors.ProcessorSettings
{
    public class SoundResponseSettings : SettingsBase, ISettings
    {
        public bool BrightnessInvert = false;
        public int Delay = 90;
        public bool Free = true;
        public bool HueInvert = false;
        public ushort Kelvin = 3500;

        public bool LinkRanges = false;
        public bool PerBulb = true;
        public bool SaturationInvert = false;
        public long TransitionDuration = 100;
        public int WaveDuration = 5000;
        public WaveTypes WaveType = WaveTypes.Audio;

        [XmlIgnore]
        public DateTime WaveStartTime { get; set; }

        public List<float> BrightnessRanges { get; set; } = new List<float>();
        public List<float> Brightnesses { get; set; } = new List<float>();
        public List<int> HueRanges { get; set; } = new List<int>();
        public List<int> Hues { get; set; } = new List<int>();
        public List<double> SaturationRanges { get; set; } = new List<double>();
        public List<double> Saturations { get; set; } = new List<double>();

        public List<byte> LevelRanges { get; set; } = new List<byte>();
        public List<int> Levels { get; set; } = new List<int>();
        public List<int> Bins { get; set; } = new List<int>();

    }
}