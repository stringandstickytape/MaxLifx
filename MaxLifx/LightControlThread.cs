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

        public LightControlThread(Thread Thread, string Name, IProcessor Processor)
        {
            m_Thread = Thread;
            UUID = Guid.NewGuid().ToString();
            m_Name = Name;
            m_Processor = Processor;
        }

        private Thread m_Thread { get; }
        public string UUID { get; set; }
        public string m_Name { get; set; }

        [XmlIgnore]
        public IProcessor m_Processor { get; private set; }

        [XmlElement("m_Processor")]
        public string ProcessorSerialized
        {
            get { return m_Processor.SettingsAsXml; }
            set
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(value);

                var typeName = xmlDocument.DocumentElement.Name;

                // Create processor
                var processorName = "MaxLifx." + typeName.Replace("Settings", "Processor");
                var processorType = Type.GetType(processorName);
                var processorConstructor = processorType.GetConstructor(Type.EmptyTypes);
                m_Processor = (IProcessor) (processorConstructor.Invoke(new object[] {}));

                m_Processor.SettingsAsXml = value;
            }
        }

        public void Abort()
        {
            m_Processor.TerminateThread = true;
        }

        public void Start()
        {
            m_Thread.Start();
            //Thread.Sleep(10);
            //m_Processor.ShowUI = true;
        }
    }

    public class LightControlThreadCollection
    {
        public LightControlThreadCollection()
        {
            LightControlThreads = new List<LightControlThread>();
        }

        public List<LightControlThread> LightControlThreads { get; set; }

        public LightControlThread AddThread(Thread Thread, string Name, IProcessor Processor)
        {
            var _lightControlThread = new LightControlThread(Thread, Name, Processor);
            LightControlThreads.Add(_lightControlThread);
            return (_lightControlThread);
        }

        public LightControlThread GetThread(string ThreadUUID)
        {
            return (LightControlThreads.SingleOrDefault(x => x.UUID == ThreadUUID));
        }

        public void RemoveThread(string ThreadUUID)
        {
            LightControlThreads.Remove(LightControlThreads.Single(x => x.UUID == ThreadUUID));
        }
    }
}