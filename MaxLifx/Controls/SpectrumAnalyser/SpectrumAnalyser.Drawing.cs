using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace MaxLifx.Controls
{
    public partial class SpectrumAnalyser
    {
        private const int BytesPerPixel = 4;
        private SpectrumAnalyserHandle _currentHandle;
        private bool _currentHandleIsRange;
        private Bitmap _previousBitmap;

        public void DrawGraph(Graphics graphics2)
        {
            var b = new Bitmap(Size.Height, Size.Width);
            var graphics = Graphics.FromImage(b);

            if (_previousBitmap != null)
            {
                using (var semiOpaque = (Bitmap)ChangeImageOpacity(_previousBitmap, 0.95))
                {
                    graphics.DrawImage(semiOpaque, new Point(0, 0));
                }
                _previousBitmap.Dispose();
            }
            else graphics.Clear(Color.LightCyan);

            if (ShowUpdates)
            {
                Point? lastPoint = null;
                var pen = new Pen(Color.FromArgb(255, 255, 1, 1));
                var latestPoints = _spectrumEngine.LatestPoints;

                for (var ctr = 0; ctr < latestPoints.Count; ctr ++)
                {
                    var a = graphics.VisibleClipBounds;
                    var p = latestPoints[ctr];

                    var newP = new Point((int) (a.Width*p.X/_spectrumEngine.Bins), ((int) (p.Y*a.Height / 256)));

                    if (lastPoint != null)
                        graphics.DrawLine(pen, lastPoint.Value, newP);

                    lastPoint = newP;
                }
            }
            graphics2.DrawImage(b, new Point(0, 0));

            _previousBitmap = b;
        }

        private int handleSize = 30;
        private void DrawHandles(Graphics graphics)
        {
            var handleColour = Color.FromArgb(128, 128, 255, 32);
            var handleHighlightPen = new Pen(Color.Black, 1);
            var handleHighlightPen2 = new Pen(Color.White, 2);
            var handleBrush = new SolidBrush(handleColour);

            foreach (var handle in _handles)
            {
                var handleRects = GetHandleRects(handle, handleSize);

                graphics.FillEllipse(handleBrush, handleRects[0]);

                DrawHandleText(graphics, handleSize / 2, handle, Brushes.White, handleRects[0]);
                DrawHandleText(graphics, handleSize / 2 - 7, handle, Brushes.White, new Rectangle(handleRects[0].X, handleRects[0].Y + 3, handleRects[0].Width, handleRects[0].Height));
                DrawHandleText(graphics, handleSize / 2 -2, handle, Brushes.Black, new Rectangle(handleRects[0].X,handleRects[0].Y + 1,handleRects[0].Width,handleRects[0].Height));

                graphics.DrawEllipse(handleHighlightPen2, handleRects[0]);
                graphics.DrawEllipse(handleHighlightPen, handleRects[0]);
                graphics.FillEllipse(handleBrush, handleRects[1]);
                graphics.DrawEllipse(handleHighlightPen2, handleRects[1]);
                graphics.DrawEllipse(handleHighlightPen, handleRects[1]);
                graphics.FillEllipse(handleBrush, handleRects[2]);
                graphics.DrawEllipse(handleHighlightPen2, handleRects[2]);
                graphics.DrawEllipse(handleHighlightPen, handleRects[2]);

                var handlePoints = GetHandlePoints(handle);
                Pen p = new Pen(handleColour);

                var p1 = new Point(handlePoints[0].X, handlePoints[0].Y - 15);
                var p2 = new Point(handlePoints[0].X, handlePoints[0].Y + 15);
                graphics.DrawLine(handleHighlightPen2, handlePoints[1], p1);
                graphics.DrawLine(handleHighlightPen2, handlePoints[2], p2);
                graphics.DrawLine(p, handlePoints[1],p1);
                graphics.DrawLine(p, p2, handlePoints[2]);
            }
        }

        private static void DrawHandleText(Graphics graphics, int fontSize, SpectrumAnalyserHandle handle, Brush fontBrush,
            Rectangle handleRect)
        {
            using (var font1 = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Point))
            {
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                graphics.DrawString(handle.Number.ToString(), font1, fontBrush, handleRect, stringFormat);
            }
        }

        private List<Rectangle> GetHandleRects(SpectrumAnalyserHandle handle, int handleRadius)
        {
            List<Rectangle> returnValue = new List<Rectangle>();
            var xPos = handle.Bin*Size.Width/_spectrumEngine.Bins;
            var yPos = handle.Level*Size.Height/256;
            returnValue.Add(new Rectangle(xPos - handleRadius/2, yPos - handleRadius/2, handleRadius, handleRadius));
            handleRadius = handleRadius/2;
            returnValue.Add(new Rectangle(xPos - handleRadius / 2, (yPos - handle.LevelRange) - handleRadius / 2, handleRadius, handleRadius));
            returnValue.Add(new Rectangle(xPos - handleRadius / 2, (yPos + handle.LevelRange) - handleRadius / 2, handleRadius, handleRadius));
            return returnValue;
        }

        private List<Point> GetHandlePoints(SpectrumAnalyserHandle handle)
        {
            List<Point> returnValue = new List<Point>();
            var xPos = handle.Bin * Size.Width / _spectrumEngine.Bins;
            var yPos = handle.Level * Size.Height / 256;
            returnValue.Add(new Point(xPos, yPos));
            returnValue.Add(new Point(xPos, yPos - handle.LevelRange));
            returnValue.Add(new Point(xPos, yPos + handle.LevelRange));
            return returnValue;
        }

        private void UpdateBinAndLevelFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs) (e);
            var bin = mouseEv.X*_spectrumEngine.Bins/Size.Width;
            if (bin < 0) bin = 0;
            if (bin > _spectrumEngine.Bins) bin = _spectrumEngine.Bins;
            _currentHandle.Bin = bin;
            var level = mouseEv.Y*256/Size.Height;
            if (level < 0) level = 0;
            if (level > 255) level = 255;
            _currentHandle.Level = (byte) level;
            if (level + _currentHandle.LevelRange/2 > 255) _currentHandle.LevelRange = (byte)((255 - level)*2);
            if (level - _currentHandle.LevelRange/2 < 0) _currentHandle.LevelRange = (byte)((level)*2);

            SelectionChanged(_currentHandle, null);
        }

        private void UpdateLevelRangeFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs)(e);
            var level = mouseEv.Y * 256 / Size.Height;
            Console.WriteLine(level);
            int newRange;
            if(level > _currentHandle.Level)
                newRange = (level - _currentHandle.Level) * Size.Height / 256;
            else
                newRange = (_currentHandle.Level - level) * Size.Height / 256;

            if (_currentHandle.Level + newRange / 2 > 255) newRange = (byte)((255 - _currentHandle.Level) * 2);
            if (_currentHandle.Level - newRange / 2 < 0) newRange = (byte)((_currentHandle.Level) * 2);

            if (newRange > 255) newRange = 255;
            _currentHandle.LevelRange = (byte) newRange;

            SelectionChanged(_currentHandle, null);
        }

        // http://stackoverflow.com/questions/4779027/changing-the-opacity-of-a-bitmap-image
        public static Image ChangeImageOpacity(Image originalImage, double opacity)
        {
            if ((originalImage.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
            {
                // Cannot modify an image with indexed colors
                return originalImage;
            }

            var bmp = (Bitmap) originalImage.Clone();

            // Specify a pixel format.
            var pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            var ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 32 bits per pixels 
            // (32 bits = 4 bytes, 3 for RGB and 1 byte for alpha).
            var numBytes = bmp.Width*bmp.Height*BytesPerPixel;
            var argbValues = new byte[numBytes];

            // Copy the ARGB values into the array.
            Marshal.Copy(ptr, argbValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the
            // RGB values for all pixels in the the bitmap.
            for (var counter = 0; counter < argbValues.Length; counter += BytesPerPixel)
            {
                // argbValues is in format BGRA (Blue, Green, Red, Alpha)

                // If 100% transparent, skip pixel
                if (argbValues[counter + BytesPerPixel - 1] == 0)
                    continue;

                var pos = 0;

                if (argbValues[counter + pos] == 1)
                    argbValues[counter + pos] = 128;
                else if (argbValues[counter + pos] > 0 && argbValues[counter + pos] < 255)
                {
                    argbValues[counter + pos]++;
                }

                pos++; // B value

                if (argbValues[counter + pos] == 1)
                    argbValues[counter + pos] = 128;
                else if (argbValues[counter + pos] > 0 && argbValues[counter + pos] < 255)
                {
                    argbValues[counter + pos] ++;
                }

                pos++; // G value

                pos++; // R value

                argbValues[counter + pos] = (byte) (argbValues[counter + pos]*1); //opacity
            }

            // Copy the ARGB values back to the bitmap
            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}