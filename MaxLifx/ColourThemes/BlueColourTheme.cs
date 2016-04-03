using System;
using System.Collections.Generic;

namespace MaxLifx.ColourThemes
{
    public class BlueColourTheme : IColourTheme
    {
        public new void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges, List<float> brightnesses, List<float> brightnessRanges, bool pastel, bool lockBrightness)
        {
            for (int index = 0; index < hues.Count; index++)
                hues[index] = r.Next(30) + 225;

            for (int index = 0; index < hueRanges.Count; index++)
                hueRanges[index] = r.Next(15);

            for (int index = 0; index < saturations.Count; index++)
                saturations[index] = r.NextDouble() / 4 + .75f;

            for (int index = 0; index < saturationRanges.Count; index++)
                saturationRanges[index] = r.NextDouble() / 4;

            if (!lockBrightness)
                for (int index = 0; index < brightnesses.Count; index++)
                brightnesses[index] = (float)(r.NextDouble());

            if (!lockBrightness)
                for (int index = 0; index < brightnessRanges.Count; index++)
                brightnessRanges[index] = (float)(r.NextDouble() / 4);

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
