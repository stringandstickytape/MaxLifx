using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using MaxLifx.Controllers;
using MaxLifx.Payload;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.Threads;
using MaxLifx.UIs;

namespace MaxLifx
{
    public class ScreenColourProcessor : ProcessorBase
    {
        public static Size CaptureResolution = new Size(4, 4);

        [XmlIgnore]
        public ScreenColourSettings SettingsCast => ((ScreenColourSettings) Settings);

        public override ISettings Settings { get; set; }

        public override string SettingsAsXml
        {
            get { return ((ScreenColourSettings) (Settings)).ToXmlString(); }
            set
            {
                ScreenColourSettings s;

                using (var st = new StringReader(value))
                {
                    s = (ScreenColourSettings) (new XmlSerializer(typeof (ScreenColourSettings)).Deserialize(st));
                }

                Settings = s;
            }
        }

        public void ScreenColour()
        {
        }

        public void ScreenColour(MaxLifxBulbController bulbController, Random random)
        {
            var frames = 0;
            var start = DateTime.Now;

            if (Settings == null)
            {
                Settings = new ScreenColourSettings();
            }

            foreach (var label in bulbController.Bulbs.Select(x => x.Label))
            {
                if (!SettingsCast.LabelsAndLocations.Select(x => x.Label).Contains(label))
                {
                    var l = new LabelAndLocationType();
                    l.Label = label;
                    l.ScreenLocation = ScreenLocation.All;
                    SettingsCast.LabelsAndLocations.Add(l);
                }
            }

            var payload = new SetColourPayload {Kelvin = 3500, TransitionDuration = (uint) (SettingsCast.Fade)};

            while (!TerminateThread)
            {
                using (var screenPixel = new Bitmap(CaptureResolution.Width, CaptureResolution.Height, PixelFormat.Format32bppArgb))
                {
                    using (var gdest = Graphics.FromImage(screenPixel))
                    {
                        using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
                        {
                            DoMainLoop(bulbController, ref frames, start, ref payload, screenPixel, gdest, gsrc);
                        }
                    }
                }
            }
        }

        private void DoMainLoop(MaxLifxBulbController bulbController, ref int frames, DateTime start,
            ref SetColourPayload payload, Bitmap screenPixel, Graphics gdest, Graphics gsrc)
        {
            if (ShowUI)
            {
                var t = new Thread(() =>
                {
                    var form2 = new ScreenColourUI(SettingsCast); /* (SettingsCast, bulbController.Bulbs);*/
                    form2.ShowDialog();
                });
                t.Start();
                ShowUI = false;
            }

            frames++;
            var screenColourSet = GetScreenColours(SettingsCast.TopLeft, SettingsCast.BottomRight, screenPixel, gdest, gsrc);

            if (screenColourSet != null)
            {
                foreach (var label in SettingsCast.SelectedLabels)
                {
                    var location = SettingsCast.LabelsAndLocations.FirstOrDefault(x => x.Label == label);
                    if (location == null) continue;

                    int r = 0, g = 0, b = 0, ctr = 0;

                    foreach(var pixel in location.SelectedPixels)
                    {
                        r += screenColourSet.colours[pixel].R;
                        g += screenColourSet.colours[pixel].G;
                        b += screenColourSet.colours[pixel].B;
                        ctr++;
                    }

                    var avgColour = ctr == 0 ? Color.Black : Color.FromArgb(255,r/ctr,g/ctr,b/ctr);

                    var HSV = RGBToHSV(avgColour);

                    var hue = (int) (HSV[0]);
                    var saturation = (ushort) (HSV[1]*(SettingsCast.Saturation-SettingsCast.MinSaturation) + SettingsCast.MinSaturation);
                    var brightness = (ushort) (HSV[2]*(SettingsCast.Brightness-SettingsCast.MinBrightness) + SettingsCast.MinBrightness);

                    payload = new SetColourPayload
                    {
                        Kelvin = 3500,
                        TransitionDuration = (uint) (SettingsCast.Fade),
                        Hue = hue,
                        Saturation = saturation,
                        Brightness = brightness,
                        RGBColour = avgColour
                    };
                    payload.PayloadType = bulbController.Bulbs.FirstOrDefault(x => x.Label == label).BulbType;

                    bulbController.SetColour(label, payload);
                }
            }

            Thread.Sleep(SettingsCast.Delay);
        }

        public static double[] RGBToHSV(Color rgb)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            v = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (rgb.R == v)
                    h = (rgb.G - rgb.B) / delta;
                else if (rgb.G == v)
                    h = 2 + (rgb.B - rgb.R) / delta;
                else if (rgb.B == v)
                    h = 4 + (rgb.R - rgb.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return new double[] { h, s, (v / 255) };

        }

        #region

        //[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        //private static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc,
        //    int ySrc, int dwRop);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int StretchBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int nWidthSrc, int nHeightSrc, int dwRop);

        /// <summary>
        /// SetStretchBltMode
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool SetStretchBltMode(IntPtr hObject, int nStretchMode);
        private class ScreenColorSet
        {
            public Color[] colours;
        }


        private unsafe ScreenColorSet GetScreenColours(Point tl, Point br, Bitmap screenPixel, Graphics gdest, Graphics gsrc)
        {
            IntPtr hSrcDC;
            IntPtr hDC;

            var width = br.X - tl.X;
            if (width < 0)
                width = 0 - width;

            var height = br.Y - tl.Y;
            if (height < 0)
                height = 0 - height;

            if (height == 0 || width == 0) return null;

            //var realtlx = 50;//tl.X < br.X ? tl.X : br.X;
            //var realtly = 50;//tl.Y < br.Y ? tl.Y : br.Y;
            ScreenColorSet returnValue;
            var thumbSize = CaptureResolution;

            hSrcDC = gsrc.GetHdc();
            hDC = gdest.GetHdc();
            //var retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0,
            //    (int) CopyPixelOperation.SourceCopy);
            SetStretchBltMode(hDC, 0x4);
            StretchBlt(hDC, 0, 0, thumbSize.Width, thumbSize.Height, hSrcDC, tl.X, tl.Y, width, height, 
                (int)CopyPixelOperation.SourceCopy);
                        
            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();
                //
            var srcData = screenPixel.LockBits(
                new Rectangle(0, 0, screenPixel.Width, screenPixel.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            //screenPixel.Save("c:\\temp\\test.bmp");

            var stride = srcData.Stride;

            var scan0 = srcData.Scan0;

            var p = (byte*) (void*) scan0;

            var returnColors = new Color[CaptureResolution.Width * CaptureResolution.Height];

            for (var x = 0; x < CaptureResolution.Width; x++)
                for (var y = 0; y < CaptureResolution.Height; y++)
                {
                    returnColors[x + y * CaptureResolution.Height] = PixelToColour(x, y, p, stride);
                }

            returnValue = new ScreenColorSet
            {
                colours = returnColors
            };

            return returnValue;
        }

        private unsafe Color PixelToColour(int x, int y, byte* p, int stride)
        {
            var bIndex = x * 4 + y * stride;
            return Color.FromArgb(255, p[bIndex + 2], p[bIndex + 1], p[bIndex]);
        }

        private unsafe Color AreaToColour(Point topLeft, Point bottomRight, byte* p, int stride)
        {
            int ctr = 0;
            int r = 0, g = 0, b = 0;

            for(var x = topLeft.X; x <= bottomRight.X; x++)
                for (var y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    var bIndex = x * 4 + y * stride;
                    ctr++;
                    r += p[bIndex + 2];
                    g += p[bIndex + 1];
                    b += p[bIndex];
                }

            return Color.FromArgb(255, r/ctr, g/ctr, b/ctr);
        }
        #endregion
    }


    public class LabelAndLocationType
    {
        public string Label { get; set; }
        public ScreenLocation ScreenLocation { get; set; }
        public List<int> SelectedPixels { get; set; }

        public LabelAndLocationType()
        {
            SelectedPixels = new List<int>();
        }
    }
}