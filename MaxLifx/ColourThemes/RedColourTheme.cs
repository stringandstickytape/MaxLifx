using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.ColourThemes
{
    public class RedColourTheme : IColourTheme
    {
        public new void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges)
        {
            for (int index = 0; index < hues.Count; index++)
                hues[index] = r.Next(30) - 15;

            for (int index = 0; index < hueRanges.Count; index++)
                hueRanges[index] = r.Next(15);

            for (int index = 0; index < saturations.Count; index++)
                saturations[index] = r.NextDouble() / 4 + .75f;

            for (int index = 0; index < saturationRanges.Count; index++)
                saturationRanges[index] = 0;
        }
    }
}
