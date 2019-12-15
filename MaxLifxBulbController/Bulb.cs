using System;
using System.Drawing;

namespace MaxLifx.Controllers
{
    public class Bulb
    {
        public bool IsHomebrewDevice = false;

        private string _macAddress;
        public string MacAddress { get { return _macAddress; }
            set
            {
                _macAddress = value;
                    if (!_macAddress.StartsWith("D0"))
                        IsHomebrewDevice = true;
                    else IsHomebrewDevice = false;
                }
        }
        public string IpAddress { get; set; }
        private string _label;
        // currently not using Version, but may be useful in the future to distinguish which type of light
        // in particular, gen 1 Z lights do not support the extended multizone api
        // as reference, the bulbs that support multizones are: 31 (Z), 32 (Z2), 38 (Beam)
        //public int Version { get; set; }
        public int Zones { get; set; }
        public string Label
        {
            get
            {
                if (String.IsNullOrEmpty(_label))
                    return MacAddress;
                return _label;
            }
            set { _label = value; }
        }



        // by default, the bulb is set to take average of entire screen
        public ScreenLocation Location = ScreenLocation.All;

        
    }

    public enum ScreenLocation
    {
        // None is used to allow proper multizone setup
        None,
        TopLeft,
        TopRight,
        Top,
        Left,
        Right,
        BottomLeft,
        BottomRight,
        Bottom,
        All,
        SurroundClockwise,
        SurroundCounterClockwise
    }
}
