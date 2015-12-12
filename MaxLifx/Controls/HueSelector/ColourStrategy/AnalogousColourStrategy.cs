using System.Collections.Generic;
using System.Linq;

namespace MaxLifx.Controls.ColourStrategy
{
    public class AnalogousColourStrategy : IColourStrategy
    {
        public void ProcessHandles(List<HueSelectorHandle> handles, int fromHandleNumber, double previousHue,
            double previousSaturation)
        {
            if (handles.Count == 0) return;

            var selectedHandle = handles.Single(x => x.HandleNumber == fromHandleNumber);

            var difference = selectedHandle.Hue - previousHue;
            var satDifference = selectedHandle.Saturation - previousSaturation;
            if (fromHandleNumber == 0)
            {
                foreach (var handle in handles.Where(x => x.HandleNumber > fromHandleNumber).OrderBy(x => x.HandleNumber))
                {
                    handle.Hue += difference;
                    handle.Saturation += satDifference;
                    if (handle.Saturation < 0) handle.Saturation = 0;
                    if (handle.Saturation > 1) handle.Saturation = 1;
                }
            }
            else
            {
                var hueStep = (handles[fromHandleNumber].Hue - handles[0].Hue)/fromHandleNumber;
                var satStep = (handles[fromHandleNumber].Saturation - handles[0].Saturation) / fromHandleNumber;
                foreach (var handle in handles.Where(x => x.HandleNumber != 0 && x.HandleNumber != fromHandleNumber).OrderBy(x => x.HandleNumber))
                {
                    handle.Hue = handles[0].Hue + hueStep * handle.HandleNumber;
                    handle.Saturation = handles[0].Saturation + satStep * handle.HandleNumber;
                    if (handle.Saturation < 0) handle.Saturation = 0;
                    if (handle.Saturation > 1) handle.Saturation = 1;
                }
            }
        }
    }
}