using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.ColourThemes
{
    public interface IColourTheme
    {
        void SetColours(Random r, List<int> hues, List<int> hueRanges, List<double> saturations, List<double> saturationRanges);
    }
}
