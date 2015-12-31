using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.ColourThemes
{
    public class BlueAndOrangeColourTheme : IColourTheme
    {
        public void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges, List<float> brightnesses, List<float> brightnessRanges)
        {
            for (int index = 0; index < hues.Count; index++)
            {
                switch (r.Next(2))
                {
                    case 0:
                        hues[index] = r.Next(30) + 225; // blue
                        break;
                    case 1:
                        hues[index] = r.Next(30) + 10; // orange
                        break;
                }
                
            }

            for (int index = 0; index < hueRanges.Count; index++)
                hueRanges[index] = r.Next(10);

            for (int index = 0; index < saturations.Count; index++)
                saturations[index] = r.NextDouble() / 5 + .8f;

            for (int index = 0; index < saturationRanges.Count; index++)
                saturationRanges[index] = 0;

            for (int index = 0; index < brightnesses.Count; index++)
                brightnesses[index] = (float)(r.NextDouble()/2+.5f);

            for (int index = 0; index < brightnessRanges.Count; index++)
                brightnessRanges[index] = (float)(r.NextDouble() / 4);
        }
    }
}
