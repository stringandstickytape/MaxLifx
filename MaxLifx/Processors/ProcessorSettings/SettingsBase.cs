using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace MaxLifx.Processors.ProcessorSettings
{
    public class SettingsBase
    {
        private string _offTimes;
        private string _onTimes;
        public List<string> SelectedLabels { get; set; } = new List<string>();

        public string FileExtension => (GetType() + ".xml").Replace("MaxLifx.Processors.ProcessorSettings.","");

        public string OnTimes
        {
            get { return _onTimes; }
            set
            {
                _onTimes = value;
                OnTimesList = GetTimeStringsFromColonSeparatedList(value).OrderBy(x => x).ToList();
            }
        }

        public string OffTimes
        {
            get { return _offTimes; }
            set
            {
                _offTimes = value;
                OffTimesList = GetTimeStringsFromColonSeparatedList(value).OrderBy(x => x).ToList();
            }
        }

        [XmlIgnore]
        public List<int> OnTimesList { get; set; }

        [XmlIgnore]
        public List<int> OffTimesList { get; set; }

        public static List<int> GetTimeStringsFromColonSeparatedList(string timesString)
        {
            if (timesString == null)
                return new List<int>();

            var times = timesString.Replace(" ", "").Replace(":", "").Split(';');

            var validTimeStrings = new List<int>();

            foreach (var time in times)
            {
                DateTime dateTime;

                if (DateTime.TryParseExact(time, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    int i;
                    int.TryParse(time, out i);
                    validTimeStrings.Add(i);
                }
            }

            return validTimeStrings;
        }

        public bool OffOrOn()
        {
            try
            {
                if (OnTimesList == null || OffTimesList == null)
                    return true;

                int currentTime;
                int.TryParse(DateTime.Now.ToString("HHmm"), out currentTime);

                var ons = OnTimesList.Where(x => x <= currentTime).ToList();
                var offs = OffTimesList.Where(x => x <= currentTime).ToList();

                if (ons.Count == 0)
                    if (offs.Count == 0)
                        return true;
                    else return false;

                var mostRecentOn = ons.Max();

                if (offs.Count == 0)
                    return true;
                var mostRecentOff = offs.Max();

                if (mostRecentOff > mostRecentOn)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}