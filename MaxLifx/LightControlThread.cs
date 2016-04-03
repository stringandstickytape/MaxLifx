using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using MaxLifx.Threads;

namespace MaxLifx
{
    public class LightControlThread
    {
        public LightControlThread()
        {
        }

        public LightControlThread(Thread thread, string name, IProcessor processor)
        {
            Thread = thread;
            Uuid = Guid.NewGuid().ToString();
            Name = name;
            Processor = processor;
        }

        private Thread Thread { get; }
        public string Uuid { get; set; }
        public string Name { get; set; }

        [XmlIgnore]
        public IProcessor Processor { get; private set; }

        [XmlElement("Processor")]
        public string ProcessorSerialized
        {
            get { return Processor.SettingsAsXml; }
            set
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(value);

                var typeName = xmlDocument.DocumentElement.Name;

                // Create processor
                var processorName = "MaxLifx." + typeName.Replace("Settings", "Processor");
                var processorType = Type.GetType(processorName);
                var processorConstructor = processorType.GetConstructor(Type.EmptyTypes);
                Processor = (IProcessor) (processorConstructor.Invoke(new object[] {}));

                Processor.SettingsAsXml = value;
            }
        }

        public void Abort()
        {
            Processor.TerminateThread = true;
        }

        public void Start()
        {
            Thread.Start();
            //Thread.Sleep(10);
            //Processor.ShowUI = true;
        }
    }

    public class LightControlThreadCollection
    {
        public LightControlThreadCollection()
        {
            LightControlThreads = new List<LightControlThread>();
        }

        public List<LightControlThread> LightControlThreads { get; set; }

        public LightControlThread AddThread(Thread thread, string name, IProcessor processor)
        {
            var lightControlThread = new LightControlThread(thread, name, processor);
            LightControlThreads.Add(lightControlThread);
            return (lightControlThread);
        }

        public LightControlThread GetThread(string threadUuid)
        {
            return (LightControlThreads.SingleOrDefault(x => x.Uuid == threadUuid));
        }

        public void RemoveThread(string threadUuid)
        {
            LightControlThreads.Remove(LightControlThreads.Single(x => x.Uuid == threadUuid));
        }
    }
}