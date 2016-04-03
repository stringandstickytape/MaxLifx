using System;
using System.Xml.Serialization;

namespace MaxLifx.Controls
{
    public class TimelineEvent
    {
        public string Uuid;
        public float Time { get; set; }
        [XmlIgnore]
        public bool Fired { get; set; }

        private TimelineEventAction _action;
        public TimelineEventAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public string Parameter { get; set; }

        public TimelineEvent()
        {
        }

        public TimelineEvent(TimelineEventAction action, string parameter, float time)
        {
            Action = action;
            Parameter = parameter;
            Time = time;
            Uuid = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Action == TimelineEventAction.Unspecified ? "Unspecified" : Parameter.Substring(Parameter.LastIndexOf("\\") + 1).Replace(".mp3", "").Replace(".MaxLifx.Threadset.xml", "");
        }
    }

    public enum TimelineEventAction
    {
        Unspecified,
        StartThreadSet,
        PlayMp3
    }
}
