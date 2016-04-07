using System;
using System.Collections.Generic;

namespace MaxLifx.ColourThemes
{
    public class RainbowV2ColourTheme : IColourTheme
    {
        public void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges, List<float> brightnesses, List<float> brightnessRanges, bool pastel, bool lockBrightness)
        {
            for (int index = 0; index < hues.Count; index++)
                hues[index] = (360/hues.Count)*index;

            for (int index = 0; index < hueRanges.Count; index++)
                hueRanges[index] = 180;

            for (int index = 0; index < saturations.Count; index++)
                saturations[index] = 1f;

            for (int index = 0; index < saturationRanges.Count; index++)
                saturationRanges[index] = 0;

            if (!lockBrightness)
                for (int index = 0; index < brightnesses.Count; index++)
                brightnesses[index] = 1f;

            if (!lockBrightness)
                for (int index = 0; index < brightnessRanges.Count; index++)
                brightnessRanges[index] = 0;

            if (pastel)
            {
                for (int index = 0; index < saturations.Count; index++)
                    saturations[index] = saturations[index] / 2;

                for (int index = 0; index < brightnesses.Count; index++)
                    brightnesses[index] = (brightnesses[index] * 2 < 1f ? brightnesses[index] * 2 : 1f);
            }
        }
    }
}
