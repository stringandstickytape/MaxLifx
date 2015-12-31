using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.ColourThemes
{
    public class RainbowColourTheme : IColourTheme
    {
        public void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges, List<float> brightnesses, List<float> brightnessRanges)
        {
            for (int index = 0; index < hues.Count; index++)
                hues[index] = (360/hues.Count)*index;

            for (int index = 0; index < hueRanges.Count; index++)
                hueRanges[index] = 360/hueRanges.Count;

            for (int index = 0; index < saturations.Count; index++)
                saturations[index] = 1f;

            for (int index = 0; index < saturationRanges.Count; index++)
                saturationRanges[index] = 0;

            for (int index = 0; index < brightnesses.Count; index++)
                brightnesses[index] = 1f;

            for (int index = 0; index < brightnessRanges.Count; index++)
                brightnessRanges[index] = 0;
        }
    }
}
