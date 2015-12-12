using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaxLifx.Processors.ProcessorSettings
{
    public class SoundResponseSettings : SettingsBase, ISettings
    {
        public bool BrightnessInvert = false;
        public int Delay = 200;
        public bool Free = true;
        public bool HueInvert = false;
        public ushort Kelvin = 3500;

        public bool LinkRanges = true;
        public ushort MaxBrightness = 65535;
        public ushort MinBrightness = 32767;
        public bool PerBulb = false;
        public bool SaturationInvert = false;
        //public long TransitionDuration = 200;
        public long TransitionDuration = 300;
        public int WaveDuration = 5000;
        public WaveTypes WaveType = WaveTypes.Audio;

        [XmlIgnore]
        public DateTime WaveStartTime { get; set; }

        public List<int> HueRanges { get; set; } = new List<int>();
        public List<int> Hues { get; set; } = new List<int>();
        public List<double> SaturationRanges { get; set; } = new List<double>();
        public List<double> Saturations { get; set; } = new List<double>();

        public List<int> LevelRanges { get; set; } = new List<int>();
        public List<int> Levels { get; set; } = new List<int>();
        public List<int> Bins { get; set; } = new List<int>();

        [XmlIgnore]
        public int BrightnessRange => MaxBrightness - MinBrightness;
    }
}