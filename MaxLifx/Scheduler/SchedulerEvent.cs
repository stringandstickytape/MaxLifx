using System;
using System.Xml.Serialization;

namespace MaxLifx.Scheduler
{
    public class SchedulerEvent
    {
        public string UUID;
        public long Milliseconds { get; set; }
        [XmlIgnore]
        public bool Fired { get; set; }

        private SchedulerEventAction _action;
        public SchedulerEventAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public string Parameter { get; set; }

        public SchedulerEvent()
        {
        }

        public SchedulerEvent(SchedulerEventAction action, string parameter, long milliseconds)
        {
            Action = action;
            Parameter = parameter;
            Milliseconds = milliseconds;
            UUID = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return ((float) Milliseconds/1000) + "s: " + Action + " - " + Parameter;
        }
    }

    public enum SchedulerEventAction
    {
        Unspecified,
        StartThreadSet,
        PlayMp3
    }
}
