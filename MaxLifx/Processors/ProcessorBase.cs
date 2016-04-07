using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MaxLifx.Processors.ProcessorSettings;

namespace MaxLifx.Threads
{
    public class ProcessorBase : IProcessor
    {
        internal bool _showUI;
        internal object Locker = new object();
        public bool TerminateThread { get; set; }
        public virtual ISettings Settings { get; set; }
        public virtual string SettingsAsXml { get; set; }

        public bool ShowUI
        {
            get
            {
                lock (Locker)
                {
                    return _showUI;
                }
            }
            set
            {
                lock (Locker)
                {
                    _showUI = value;
                }
            }
        }

        public static void SaveSettings<T>(T settings, string filename)
        {
            //if (!SuspendUI)
            //

            if (string.IsNullOrEmpty(filename))
                filename = typeof (T).Name + "Default.maxlifx.xml";

            var xml = new XmlSerializer(typeof (T));

            using (var stream = new MemoryStream())
            {
                xml.Serialize(stream, settings);
                stream.Position = 0;
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);
                xmlDocument.Save(filename);
                stream.Close();
            }
            //}
        }

        public static void LoadSettings<T>(ref T settings, string filename)
        {
            if (filename == "")
                filename = typeof (T).Name + "Default.maxlifx.xml";


            if (!File.Exists(filename))
                return;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            var xmlString = xmlDocument.OuterXml;

            using (var read = new StringReader(xmlString))
            {
                var outType = typeof (T);

                var serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    settings = (T) serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }
        }
    }
}