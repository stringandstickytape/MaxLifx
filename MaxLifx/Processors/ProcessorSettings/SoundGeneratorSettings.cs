using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MaxLifx.SoundToken;
using MaxLifx.UIs;
using NAudio.Wave;

namespace MaxLifx.Processors.ProcessorSettings
{
    public class SoundGeneratorSettings : SettingsBase, ISettings
    {
        private string _uuid;
        public List<Sound> Sounds = new List<Sound>();
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
        public List<SoundMessage> Messages { get; private set; } = new List<SoundMessage>();

        public float Volume { get; set; }

        public Sound GetSoundFromUUID(string UUID)
        {
            return Sounds.Single(x => x.UUID == UUID);
        }
    }
}