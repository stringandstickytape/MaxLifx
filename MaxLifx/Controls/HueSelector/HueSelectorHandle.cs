using System;
using System.Drawing;

namespace MaxLifx.Controls.HueSelector
{
    public class HueSelectorHandle
    {
        private int _handleNumber;

        public HueSelectorHandle(int handleNumber)
        {
            HandleNumber = handleNumber;
            Hue = handleNumber*45;
            HueRange = 50;
        }

        public int HandleNumber
        {
            get { return _handleNumber; }
            set { _handleNumber = value; }
        }

        public double Hue { get; set; }
        public double HueRange { get; set; }
        public double Saturation { get; set; } = 0;
        public double SaturationRange { get; set; } = 0;

        public Rectangle GetHandleRectangle(Rectangle clientRectangle, int halfHandleSizeX, int halfHandleSizeY,
            int ring, bool shadow)
        {
            var controlCentre = GetControlCentre(clientRectangle);
            var handleRect = new Rectangle(controlCentre.X - halfHandleSizeX,
                controlCentre.Y - halfHandleSizeY,
                halfHandleSizeX*2,
                halfHandleSizeY*2);

            handleRect.X = (int) (handleRect.X + Math.Sin(Hue*Math.PI/180)*(clientRectangle.Width - 20)/2*Saturation);
            handleRect.Y = (int) (handleRect.Y - Math.Cos(Hue*Math.PI/180)*(clientRectangle.Height - 20)/2*Saturation);

            return handleRect;
        }

        public Rectangle GetHandleRangeRectangle(Rectangle clientRectangle, int halfHandleSizeX, int halfHandleSizeY,
            bool positive, int ring, bool shadow)
        {
            var controlCentre = GetControlCentre(clientRectangle);
            var handleRect = new Rectangle(controlCentre.X - halfHandleSizeX,
                controlCentre.Y - halfHandleSizeY,
                halfHandleSizeX*2,
                halfHandleSizeY*2);

            var angle = positive ? Hue + HueRange : Hue - HueRange;

            var satLevel = positive ? Saturation + SaturationRange : Saturation - SaturationRange;

            handleRect.X = (int) (handleRect.X + Math.Sin(angle*Math.PI/180)*(clientRectangle.Width - 20)/2*satLevel);
            handleRect.Y = (int) (handleRect.Y - Math.Cos(angle*Math.PI/180)*(clientRectangle.Height - 20)/2* satLevel);

            return handleRect;
        }

        public Point GetControlCentre(Rectangle clientRectangle)
        {
            return new Point(clientRectangle.Width/2, clientRectangle.Height/2);
        }
    }
}