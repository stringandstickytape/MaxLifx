using System.Drawing;

namespace MaxLifx.Controls.BrightnessSelector
{
    public class BrightnessSelectorHandle
    {
        private int _handleNumber;

        public BrightnessSelectorHandle(int handleNumber)
        {
            HandleNumber = handleNumber;
            Brightness = .9f;
            BrightnessRange = .1f;
        }

        public int HandleNumber
        {
            get { return _handleNumber; }
            set { _handleNumber = value; }
        }

        public float Brightness { get; set; }
        public float BrightnessRange { get; set; }

        public Rectangle GetHandleRectangle(Rectangle clientRectangle, int halfHandleSizeX, int halfHandleSizeY,
            int ring, int handleCount, int handleNumber)
        {
            var hPos = (int)(clientRectangle.Width / (handleCount) * (handleNumber + .5));

            var handleRect = new Rectangle(hPos - 15,
                (int)(clientRectangle.Height * (1-Brightness)) - 15,
                30,
                30);

            return handleRect;
        }

        public Rectangle GetHandleRangeRectangle(Rectangle clientRectangle, int halfHandleSizeX, int halfHandleSizeY,
            bool positive, int ring, int handleCount, int handleNumber)
        {
            var hPos = (int)(clientRectangle.Width/(handleCount) * (handleNumber + .5));

            var handleRect = new Rectangle(hPos - 10,
                (int)(clientRectangle.Height * (1 - ( (positive ? Brightness - BrightnessRange : Brightness + BrightnessRange)))) - 10,
                20,
                20);

            return handleRect;
        }

        public Point GetControlCentre(Rectangle clientRectangle)
        {
            return new Point(clientRectangle.Width/2, clientRectangle.Height/2);
        }
    }
}