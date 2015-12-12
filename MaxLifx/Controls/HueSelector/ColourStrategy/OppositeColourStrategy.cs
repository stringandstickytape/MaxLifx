using System.Collections.Generic;
using System.Linq;

namespace MaxLifx.Controls.ColourStrategy
{
    public class OppositeColourStrategy : IColourStrategy
    {
        public void ProcessHandles(List<HueSelectorHandle> handles, int fromHandleNumber, double previousHue,
            double previousSaturation)
        {
            var _otherHandle = handles.Single(x => x.HandleNumber != fromHandleNumber);
            var _thisHandleHue = handles.Single(x => x.HandleNumber == fromHandleNumber).Hue;
            _otherHandle.Hue = (180 + _thisHandleHue)%360;
        }
    }
}