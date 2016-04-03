using System;
using System.Collections.Generic;

namespace MaxLifx.ColourThemes
{
    public interface IColourTheme
    {
        void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges, List<float> brightnesses, List<float> brightnessRanges, bool pastel, bool lockBrightness);
    }
}
