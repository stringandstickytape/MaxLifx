using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxLifx.Controls
{
    public class TimelineEvent
    {
        public string UUID;
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
            UUID = Guid.NewGuid().ToString();
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
