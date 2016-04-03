using System.Collections.Generic;
using System.Linq;
using MaxLifx.Controls.HueSelector;

namespace MaxLifx.Controls.ColourStrategy
{
    public class OppositeColourStrategy : IColourStrategy
    {
        public void ProcessHandles(List<HueSelectorHandle> handles, int fromHandleNumber, double previousHue,
            double previousSaturation)
        {
            var otherHandle = handles.Single(x => x.HandleNumber != fromHandleNumber);
            var thisHandleHue = handles.Single(x => x.HandleNumber == fromHandleNumber).Hue;
            otherHandle.Hue = (180 + thisHandleHue)%360;
        }
    }
}