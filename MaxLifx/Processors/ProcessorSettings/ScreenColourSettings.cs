using System.Collections.Generic;
using System.Drawing;
namespace MaxLifx.Processors.ProcessorSettings
{
    public class ScreenColourSettings : SettingsBase, ISettings
    {
        public int Brightness = 65535;
        public int Kelvin = 3500;
        public int Delay = 50;
        public int Fade = 150;
        public List<BulbSetting> BulbSettings = new List<BulbSetting>();
        public int Saturation = 65535;
        public int MinSaturation = 0;
        public int MinBrightness = 0;
        public Point CentrePoint = new Point(0, 0);
        // keep track of the different number of multizone lights
        public HashSet<int> MultiColourZones = new HashSet<int>();

        public int Monitor = 0;
    }
}