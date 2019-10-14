using System.Collections.Generic;
using System.Drawing;

namespace MaxLifx.Processors.ProcessorSettings
{
    public class ScreenColourSettings : SettingsBase, ISettings
    {
        public Point BottomRight = new Point(1919, 1079);
        public int Brightness = 65535;
        public int Delay = 50;
        // Lighting parameters
        public int Fade = 150;
        public List<LabelAndLocationType> LabelsAndLocations = new List<LabelAndLocationType>();
        public int Saturation = 65535;
        public int MinSaturation = 0;
        public int MinBrightness = 0;
        public Point TopLeft = new Point(0, 0);
    }
}