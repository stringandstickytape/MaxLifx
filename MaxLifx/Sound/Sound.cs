using System;
using System.Xml.Serialization;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MaxLifx.SoundToken
{
    [Serializable]
    public class Sound
    {
        public enum SoundTypes
        {
            Looping,
            Random
        }

        private int _frequency = 30;
        private float _pan = .5f;
        private string _uuid;
        private float _volume = .5f;

        public Sound(string name, string filename, SoundTypes soundType)
        {
            Name = name;
            Filename = filename;
            SoundType = soundType;
        }

        public Sound()
        {
        }

        public string Name { get; set; }
        public string Filename { get; set; }

        public string UUID
        {
            get
            {
                if (_uuid == null)
                    _uuid = Guid.NewGuid().ToString();
                return _uuid;
            }
            set { _uuid = value; }
        }

        [XmlIgnore]
        public WaveOut WaveOut { get; set; }

        [XmlIgnore]
        public VolumeSampleProvider VolumeProvider { get; set; }

        [XmlIgnore]
        public PanningSampleProvider PanProvider { get; set; }

        public SoundTypes SoundType { get; set; }
        public bool Started { get; set; }
        public StartStop StartStopRequest { get; set; }

        public float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public float Pan
        {
            get { return _pan; }
            set { _pan = value; }
        }

        public int Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }
    }
}