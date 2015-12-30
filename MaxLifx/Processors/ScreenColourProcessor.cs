using System;
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
    public class ScreenColourProcessor : ProcessorBase, IProcessor
    {
        [XmlIgnore]
        public ScreenColourSettings SettingsCast => ((ScreenColourSettings) Settings);

        public override ISettings Settings { get; set; }

        public override string SettingsAsXml
        {
            get { return ((ScreenColourSettings) (Settings)).ToXmlString(); }
            set
            {
                var s = new ScreenColourSettings();

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
                using (var screenPixel = new Bitmap(2, 2, PixelFormat.Format32bppArgb))
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
                    var location = SettingsCast.LabelsAndLocations.Single(x => x.Label == label).ScreenLocation;

                    var avgColour = screenColourSet.all;

                    switch (location)
                    {
                        case ScreenLocation.Top:
                            avgColour = screenColourSet.top;
                            break;
                        case ScreenLocation.Bottom:
                            avgColour = screenColourSet.bottom;
                            break;
                        case ScreenLocation.Left:
                            avgColour = screenColourSet.left;
                            break;
                        case ScreenLocation.Right:
                            avgColour = screenColourSet.right;
                            break;
                        case ScreenLocation.TopLeft:
                            avgColour = screenColourSet.topleft;
                            break;
                        case ScreenLocation.TopRight:
                            avgColour = screenColourSet.topright;
                            break;
                        case ScreenLocation.BottomLeft:
                            avgColour = screenColourSet.bottomleft;
                            break;
                        case ScreenLocation.BottomRight:
                            avgColour = screenColourSet.bottomright;
                            break;
                    }

                    var hue = (int) (avgColour.GetHue());
                    var saturation = (ushort) (avgColour.GetSaturation()*(SettingsCast.Saturation-SettingsCast.MinSaturation) + SettingsCast.MinSaturation);
                    var brightness = (ushort) (avgColour.GetBrightness()*(SettingsCast.Brightness-SettingsCast.MinBrightness) + SettingsCast.MinBrightness);

                    payload = new SetColourPayload
                    {
                        Kelvin = 3500,
                        TransitionDuration = (uint) (SettingsCast.Fade),
                        Hue = hue,
                        Saturation = saturation,
                        Brightness = brightness
                    };

                    bulbController.SetColour(label, payload);
                    bulbController.SetColour(label, payload);
                }
            }

            var interval = DateTime.Now - start;

            Thread.Sleep(SettingsCast.Delay);
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
            public Color topleft, topright, bottomleft, bottomright, left, right, top, bottom, all;
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
            Color topleft, bottomleft, topright, bottomright, top, bottom, left, right, all;
            ScreenColorSet returnValue;
            var thumbSize = new Size(2,2);

                        hSrcDC = gsrc.GetHdc();
                        hDC = gdest.GetHdc();
                        //var retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0,
                        //    (int) CopyPixelOperation.SourceCopy);
                        SetStretchBltMode(hDC, 0x04);
                        var retval = StretchBlt(hDC, 0, 0, thumbSize.Width, thumbSize.Height, hSrcDC, tl.X, tl.Y, width, height, 
                            (int)CopyPixelOperation.SourceCopy);
                        
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                //screenPixel.Save("Pics\\" + DateTime.Now.ToString("hhmmss").Replace("/", "").Replace(":", "") + ".bmp");
                var srcData = screenPixel.LockBits(
                    new Rectangle(0, 0, screenPixel.Width, screenPixel.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                var stride = srcData.Stride;

                var scan0 = srcData.Scan0;

                var p = (byte*) (void*) scan0;

                topleft = Color.FromArgb(255,     p[2], p[1], p[0]);
                bottomleft = Color.FromArgb(255,  p[stride + 2], p[stride + 1], p[stride+0]);
                topright = Color.FromArgb(255,    p[4 + 2], p[4 + 1], p[4 + 0]);
                bottomright = Color.FromArgb(255, p[stride+4+2], p[stride + 4 + 1], p[stride + 4 + 0]);

                top = Color.FromArgb(255, (topleft.R + topright.R)/2, (topleft.G + topright.G)/2,
                    (topleft.B + topright.B)/2);
                bottom = Color.FromArgb(255, (bottomleft.R + bottomright.R)/2, (bottomleft.G + bottomright.G)/2,
                    (bottomleft.B + bottomright.B)/2);
                left = Color.FromArgb(255, (topleft.R + bottomleft.R)/2, (topleft.G + bottomleft.G)/2,
                    (topleft.B + bottomleft.B)/2);
                right = Color.FromArgb(255, (bottomright.R + topright.R)/2, (bottomright.G + topright.G)/2,
                    (bottomright.B + topright.B)/2);

                all = Color.FromArgb(255, (top.R + bottom.R)/2, (top.G + bottom.G)/2, (top.B + bottom.B)/2);

                returnValue = new ScreenColorSet
                {
                    topleft = topleft,
                    topright = topright,
                    top = top,
                    bottomleft = bottomleft,
                    bottomright = bottomright,
                    bottom = bottom,
                    left = left,
                    right = right,
                    all = all
                };

            return returnValue;
        }


        #endregion
    }


    public class LabelAndLocationType
    {
        public string Label { get; set; }
        public ScreenLocation ScreenLocation { get; set; }
    }
}