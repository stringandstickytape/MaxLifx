using System.Collections.Generic;
using MaxLifx.Controllers;

namespace MaxLifx
{
    public class MaxLifxSettings
    {
        // Bulbs (for load and save purposes, these are passed to MaxLifxBulbController when we load to avoid having to discover every time)
        public List<Bulb> Bulbs = new List<Bulb>();
        // Selections
        public List<string> SelectedLabels = new List<string>();
    }
}