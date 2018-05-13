using System;

namespace MaxLifx.Controllers
{
    public class Bulb
    {
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        private string _label;
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

        public ScreenLocation Location = ScreenLocation.All;
        public BulbType BulbType = BulbType.Lifx;
    }

    public enum ScreenLocation
    {
        TopLeft,
        TopRight,
        Top,
        Left,
        Right,
        BottomLeft,
        BottomRight,
        Bottom,
        All
    }

    public enum BulbType
    {
        Lifx,
        CorsairMouse,
        CorsairKeyboard,
        Asus
    }
}
