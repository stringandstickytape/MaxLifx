using System;
using System.Collections.Generic;

namespace MaxLifx.ColourThemes
{
    public class BlueAndWhite : IColourTheme
    {
        public void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges, List<float> brightnesses, List<float> brightnessRanges, bool pastel, bool lockBrightness)
        {
            for (int index = 0; index < hues.Count; index++)
            {
                        hues[index] = 240;
            }

            for (int index = 0; index < hueRanges.Count; index++)
                hueRanges[index] = 0;

            for (int index = 0; index < saturations.Count; index++)
                saturations[index] = r.Next(2) == 0 ? 0f : 1f;

            for (int index = 0; index < saturationRanges.Count; index++)
                saturationRanges[index] = 0;

            if (!lockBrightness)
                for (int index = 0; index < brightnesses.Count; index++)
                brightnesses[index] = pastel ? .75f : .5f;

            if (!lockBrightness)
                for (int index = 0; index < brightnessRanges.Count; index++)
                brightnessRanges[index] = pastel ? .25f : .5f;
        }
    }
}
