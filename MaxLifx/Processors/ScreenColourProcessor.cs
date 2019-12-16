using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using MaxLifx.Controllers;
using MaxLifx.Payload;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.Threads;
using MaxLifx.UIs;
using DesktopDuplication;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using MaxLifxBulbControllerCache;
using System.Net.Sockets;

namespace MaxLifx
{
    public class ScreenColourProcessor : ProcessorBase
    {
        public Dictionary<string, List<Rectangle>> ZoneAreas = new Dictionary<string, List<Rectangle>>();

        private DesktopDuplicator _desktopDuplicator;

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

            _desktopDuplicator = new DesktopDuplicator(SettingsCast.Monitor);

            var bulbs = bulbController.Bulbs.Where(x => x.Zones < 2).Select(x => x.Label).ToList();

            foreach (var bulbObj in bulbController.Bulbs.Where(x => x.Zones > 1))
                for (var i = 0; i < bulbObj.Zones; i++)
                    bulbs.Add(bulbObj.Label + $" (Zone {(i + 1).ToString().PadLeft(3, '0')})");

            foreach (var bulbLabel in bulbs)
            {
                var bulb = bulbController.GetBulbFromLabel(bulbLabel, out int zone);

                if (!SettingsCast.BulbSettings.Select(x => x.Label).Contains(bulbLabel))
                {
                    // populating SettingsCast fields
                    var l = new BulbSetting();
                    l.Label = bulbLabel;
                    l.ScreenLocation = ScreenLocation.All;
                    l.Zones = bulb.Zones;

                    SettingsCast.BulbSettings.Add(l);
                }
            }

            while (!TerminateThread)
            {
               DoMainLoop(bulbController, ref frames, start);
            }
        }
        /*private void zoneCalculation()
        {
            // getting a rectangle based on the Settings area
            var size = new Size(SettingsCast.BottomRight.X - SettingsCast.TopLeft.X, SettingsCast.BottomRight.Y - SettingsCast.TopLeft.Y);
            var rect = new Rectangle(SettingsCast.TopLeft, size);

            // can also just get screen from point, but should be less ambiguity with a rectangle
            var bounds = Screen.FromRectangle(rect).Bounds;

            // the optimal measurement for the starting zone would be the start of the strip to the 0.3cm past the last led
            // since there is about 1.3cm space from the start of the strip to first led
            // and about 1.6cm space between last led in zone and first led in new zone (of same strip)
            // the distance between last led in strip and first led in next strip is about 1.9cm
            // but YMMV

            // want the original edge to determine when to swap width and height
            int originalEdge = closestEdge(ref rect, bounds);

            // split up the monitor into zones; repeat for each multizone light that has different number of zones
            foreach (int zoneNumber in SettingsCast.MultiColourZones.ToList())
            {
                // TODO:
                // condense and simplify the steps below to avoid repeating all this duplicate stuff for each edge and direction
                // but today is not the day for me to figure that out; I'm just happy to get this working for now

                
                var rectList = new List<Rectangle>();
                var tempRect = rect;
                int currentEdge = originalEdge;
                // add starting zone
                rectList.Add(rect);
                // for clockwise
                for (int i = 1; i < zoneNumber; i++)
                {
                    int height = rect.Height;
                    int width = rect.Width;
                    // if started out on adjacent edge, swap width and height
                    if (originalEdge % 2 != currentEdge % 2)
                    {
                        width = rect.Height;
                        height = rect.Width;
                    }
                    switch (currentEdge)
                    {
                        case 0:
                            // check if enough space to fit the full area
                            if (tempRect.Top - height > bounds.Top)
                            {
                                tempRect = new Rectangle(bounds.X, tempRect.Top - height, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                // need to split zone with the next edge
                                int currentBound = tempRect.Top;
                                int nextBound = height - currentBound;
                                // make sure to use proper area for next calculation, but don't add to list
                                tempRect = new Rectangle(bounds.X, bounds.Y, nextBound, currentBound);
                                Rectangle tempRect2 = tempRect;
                                // next edge
                                currentEdge = (currentEdge + 1) % 4;
                                // add a "full size" area, depending on which edge has more of the zones left
                                if (currentBound > nextBound && nextBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.X, bounds.Y, width, currentBound);
                                }
                                else if (nextBound >= currentBound && currentBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.X, bounds.Y, nextBound, width);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                        case 1:
                            if (tempRect.Right + width < bounds.Right)
                            {
                                tempRect = new Rectangle(tempRect.Right, bounds.Y, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                int currentBound = bounds.Right - tempRect.Right;
                                int nextBound = width - currentBound;
                                tempRect = new Rectangle(tempRect.Right, tempRect.Top, currentBound, nextBound);
                                Rectangle tempRect2 = tempRect;
                                currentEdge = (currentEdge + 1) % 4;
                                if (currentBound > nextBound && nextBound < height)
                                {
                                    tempRect2 = new Rectangle(tempRect.Right, tempRect.Top, currentBound, height);
                                }
                                else if (nextBound >= currentBound && currentBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - height, tempRect.Top, height, nextBound);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                        case 2:
                            if (tempRect.Bottom + height < bounds.Bottom)
                            {
                                tempRect = new Rectangle(bounds.Right - width, tempRect.Bottom, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                int currentBound = bounds.Bottom - tempRect.Bottom;
                                int nextBound = height - currentBound;
                                tempRect = new Rectangle(bounds.Right - nextBound, tempRect.Bottom, nextBound, currentBound);
                                Rectangle tempRect2 = tempRect;
                                currentEdge = (currentEdge + 1) % 4;
                                if (currentBound > nextBound && nextBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - width, tempRect.Bottom, width, currentBound);
                                }
                                else if (nextBound >= currentBound && currentBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - nextBound, bounds.Bottom - width, nextBound, width);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                        case 3:
                            if (tempRect.Left - width > bounds.Left)
                            {
                                tempRect = new Rectangle(tempRect.Left - width, bounds.Bottom - height, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                int currentBound = tempRect.Left;
                                int nextBound = width - currentBound;
                                tempRect = new Rectangle(bounds.X, bounds.Bottom - nextBound, currentBound, nextBound);
                                Rectangle tempRect2 = tempRect;
                                currentEdge = (currentEdge + 1) % 4;
                                if (currentBound > nextBound && nextBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.X, bounds.Bottom - height, currentBound, height);
                                }
                                else if (nextBound >= currentBound && currentBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.X, bounds.Bottom - nextBound, height, nextBound);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                    }

                }
                // clockwise dictionary
                ZoneAreas[zoneNumber.ToString() + "cw"] = rectList;

                // one more time for counterclockwise
                rectList = new List<Rectangle>();
                tempRect = rect;
                currentEdge = originalEdge;
                rectList.Add(rect);
                for (int i = 1; i < zoneNumber; i++)
                {
                    int height = rect.Height;
                    int width = rect.Width;
                    // if started out on adjacent edge, swap width and height
                    if (originalEdge % 2 != currentEdge % 2)
                    {
                        width = rect.Height;
                        height = rect.Width;
                    }
                    switch (currentEdge)
                    {
                        case 0:
                            // check if enough space to fit the full area
                            if (tempRect.Bottom + height < bounds.Bottom)
                            {
                                tempRect = new Rectangle(bounds.X, tempRect.Bottom, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                // need to split zone with the next edge
                                int currentBound = bounds.Bottom - tempRect.Bottom;
                                int nextBound = height - currentBound;
                                // make sure to use proper area for next calculation, but don't add to list
                                tempRect = new Rectangle(bounds.X, bounds.Bottom - currentBound, nextBound, currentBound);
                                Rectangle tempRect2 = tempRect;
                                // next edge
                                currentEdge = 3;
                                if (currentBound > nextBound && nextBound < width)
                                {
                                    // add a "full size" area, dependin on which edge has more of the zones left
                                    tempRect2 = new Rectangle(bounds.X, bounds.Bottom - currentBound, width, currentBound);
                                }
                                else if (nextBound >= currentBound && currentBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.X, bounds.Bottom - width, nextBound, width);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                        case 1:
                            if (tempRect.Left - width > bounds.Left)
                            {
                                tempRect = new Rectangle(tempRect.Left - width, bounds.Y, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                int currentBound = tempRect.Left;
                                int nextBound = width - currentBound;
                                tempRect = new Rectangle(bounds.Left, bounds.Top, currentBound, nextBound);
                                Rectangle tempRect2 = tempRect;
                                currentEdge = 0;
                                if (currentBound > nextBound && nextBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.Left, bounds.Top, currentBound, height);
                                }
                                else if (nextBound >= currentBound && currentBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.Left, bounds.Top, height, nextBound);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                        case 2:
                            if (tempRect.Top - height > bounds.Top)
                            {
                                tempRect = new Rectangle(bounds.Right - width, tempRect.Top - height, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                int currentBound = tempRect.Top;
                                int nextBound = height - currentBound;
                                tempRect = new Rectangle(bounds.Right - nextBound, tempRect.Top, nextBound, currentBound);
                                Rectangle tempRect2 = tempRect;
                                currentEdge = 1;
                                if (currentBound > nextBound && nextBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - width, bounds.Top, width, currentBound);
                                }
                                else if (nextBound >= currentBound && currentBound < width)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - nextBound, bounds.Top, nextBound, width);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                        case 3:
                            if (tempRect.Right + width < bounds.Right)
                            {
                                tempRect = new Rectangle(tempRect.Right, bounds.Bottom - height, width, height);
                                rectList.Add(tempRect);
                            }
                            else
                            {
                                int currentBound = bounds.Right - tempRect.Right;
                                int nextBound = width - currentBound;
                                tempRect = new Rectangle(bounds.Right - currentBound, bounds.Bottom - nextBound, currentBound, nextBound);
                                Rectangle tempRect2 = tempRect;
                                currentEdge = 2;
                                if (currentBound > nextBound && nextBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - currentBound, bounds.Bottom - height, currentBound, height);
                                }
                                else if (nextBound >= currentBound && currentBound < height)
                                {
                                    tempRect2 = new Rectangle(bounds.Right - height, bounds.Bottom - nextBound, height, nextBound);
                                }
                                rectList.Add(tempRect2);
                            }
                            break;
                    }
                }
                // counterclockwise dictionary
                ZoneAreas[zoneNumber.ToString() + "ccw"] = rectList;
            }
        }*/
        private int closestEdge(ref Rectangle rect, Rectangle bounds)
        {
            int[] boundsDistance = new int[4];
            boundsDistance[0] = Math.Abs(rect.Left - bounds.Left);
            boundsDistance[1] = Math.Abs(rect.Top - bounds.Top);
            boundsDistance[2] = Math.Abs(rect.Right - bounds.Right);
            boundsDistance[3] = Math.Abs(rect.Bottom - bounds.Bottom);

            for (int i = 0; i < boundsDistance.Length; i++)
            {
                // allow some error when selecting the area; it's hard to get it exact
                if (boundsDistance[i] <= 20)
                {
                    boundsDistance[i] = 0;
                }
            }
            // see if there are multiple closest edges
            int[] minDistance = boundsDistance.Select((v, j) => new {
                value = v,
                index = j
            }).Where(pair => pair.value == boundsDistance.Min())
              .Select(pair => pair.index)
              .ToArray();
            int edge = minDistance.Max();
            // if the box is in a corner then need to decide which is starting edge
            // best guess is that the longer side will be the starting edge
            // if width is longer, then we're starting either top or bottom
            if (rect.Width >= rect.Height)
            {
                if (minDistance.Contains(3))
                {
                    edge = 3;
                    // make sure starts at border
                    rect.Y = bounds.Bottom - rect.Height;
                }
                else if (minDistance.Contains(1))
                {
                    edge = 1;
                    rect.Y = bounds.Top;
                }
            }
            else
            {
                // if height is longer, than we're starting either left or right
                if (minDistance.Contains(0))
                {
                    edge = 0;
                    // make sure starts at border
                    rect.X = bounds.Left;
                }
                else if (minDistance.Contains(2))
                {
                    edge = 2;
                    rect.X = bounds.Right - rect.Height;
                }
            }
            return edge;
        }


        // this is where the actual colours are determined and set for the bulbs
        private void DoMainLoop(MaxLifxBulbController bulbController, ref int frames, DateTime start)
        {
            var reusableHomebrewClientDictionary = new Dictionary<string, UdpClient>();

            if (_desktopDuplicator.mWhichOutputDevice != SettingsCast.Monitor)
                _desktopDuplicator = new DesktopDuplicator(SettingsCast.Monitor);

            if (ShowUI)
            {
                var t = new Thread(() =>
                {
                    var form2 = new ScreenColourUI(SettingsCast, bulbController); /* (SettingsCast, bulbController.Bulbs);*/
                    form2.ShowDialog();
                });
                t.Start();
                ShowUI = false;
            }

            frames++;
            // determine colours on screen
            // var screenColourSet = GetScreenColours(SettingsCast.TopLeft, SettingsCast.BottomRight, screenPixel, gdest, gsrc);
            Color? avgColour = null;

            bool needNewFrame = true;

            var homebrewDevicePayloadCache = new HomebrewDevicePayloadCache();

            foreach (var bulbSetting in SettingsCast.BulbSettings)
            {
                if (!bulbSetting.Enabled) continue;
                if (bulbSetting.TopLeft.X == bulbSetting.BottomRight.X || bulbSetting.TopLeft.Y == bulbSetting.BottomRight.Y) continue;

                var label = bulbSetting.Label;
                var location = bulbSetting.ScreenLocation;
                var zones = bulbSetting.Zones;

                // set once
                var bulb = bulbController.GetBulbFromLabel(label, out int zone);

                //screenColourSet = GetScreenColours(bulbSetting.TopLeft, bulbSetting.BottomRight, screenPixel, gdest, gsrc);

                avgColour = TestDD(bulbSetting, needNewFrame);
                needNewFrame = false;

                if (avgColour == null) continue;

                // Color isn't HSV, so need to convert
                double hue = 0;
                double saturation = 0;
                double brightness = 0;
                Utils.ColorToHSV((Color)avgColour, out hue, out saturation, out brightness);
                brightness = (brightness * (SettingsCast.Brightness - SettingsCast.MinBrightness) + SettingsCast.MinBrightness);
                saturation = (saturation * (SettingsCast.Saturation - SettingsCast.MinSaturation) + SettingsCast.MinSaturation);
                var payload = new SetColourPayload
                {
                    Kelvin = (ushort)SettingsCast.Kelvin,
                    TransitionDuration = (uint)(SettingsCast.Fade),
                    Hue = (int)hue,
                    Saturation = (ushort)saturation,
                    Brightness = (ushort)brightness
                };

                if (bulb.IsHomebrewDevice)
                {
                    homebrewDevicePayloadCache.Payloads.Add((bulb, zone), payload);
                }
                else
                {
                    bulbController.SetColour(bulb, zone, payload, true);
                }
                
            }

            homebrewDevicePayloadCache.Send(reusableHomebrewClientDictionary, bulbController);

            Thread.Sleep(SettingsCast.Delay);
        }

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        DesktopFrame frame;
        private unsafe Color? TestDD(BulbSetting bulbSetting, bool getNewFrame = false)
        {
            if(getNewFrame || frame == null)
                frame = _desktopDuplicator.GetLatestFrame();

            Color? all = null;
            Bitmap b = new Bitmap(1, 1);
            if (frame != null && frame.DesktopImage != null)
            {
                int PixelSize = 4;

                var tlx = bulbSetting.TopLeft.X;
                var tly = bulbSetting.TopLeft.Y;
                var brx = bulbSetting.BottomRight.X;
                var bry = bulbSetting.BottomRight.Y;
                var hei = bry - tly;
                var wid = brx - tlx;
                BitmapData bmd = frame.DesktopImage.LockBits(new Rectangle(tlx, tly, wid, hei),
                                  System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                  frame.DesktopImage.PixelFormat);

                int rTot = 0, bTot = 0, gTot = 0, ctr = 0;

                unsafe
                {
                    for (int y = 0; y < hei; y++)
                    {
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);

                        for (int x = 0; x < wid; x++)
                        {
                            bTot = bTot + row[x * PixelSize];
                            gTot = gTot + row[x * PixelSize + 1];
                            rTot = rTot + row[x * PixelSize + 2];
                            ctr++;
                        }
                    }
                }

                all = Color.FromArgb(rTot/ctr, gTot/ctr, bTot/ctr);

                frame.DesktopImage.UnlockBits(bmd);
            }

            return all;
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
            public Color all;
        }

        private unsafe ScreenColorSet GetScreenColours(Point tl, Point br, Bitmap screenPixel, Graphics gdest, Graphics gsrc)
        {
            IntPtr hSrcDC;
            IntPtr hDC;

            Color all;
            ScreenColorSet returnValue;
            var thumbSize = new Size(1, 1);

            hSrcDC = gsrc.GetHdc();
            hDC = gdest.GetHdc();

            SetStretchBltMode(hDC, 0x04);
            StretchBlt(hDC, 0, 0, 1, 1, hSrcDC, tl.X, tl.Y, 10, 10, (int)CopyPixelOperation.SourceCopy);

            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();
            
            var srcData = screenPixel.LockBits(new Rectangle(0, 0, 1, 1), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            var stride = srcData.Stride;

            var scan0 = srcData.Scan0;

            var p = (byte*)(void*)scan0;

            all = Color.FromArgb(255, p[2], p[1], p[0]);

            screenPixel.UnlockBits(srcData);
            returnValue = new ScreenColorSet
            {
                all = all
            };

            return returnValue;
        }

        /*

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

            Color topleft, bottomleft, topright, bottomright, top, bottom, left, right, all;
            ScreenColorSet returnValue;
            var thumbSize = new Size(2,2);

            hSrcDC = gsrc.GetHdc();
            hDC = gdest.GetHdc();
            //var retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0,
            //    (int) CopyPixelOperation.SourceCopy);
            SetStretchBltMode(hDC, 0x04);
            StretchBlt(hDC, 0, 0, thumbSize.Width, thumbSize.Height, hSrcDC, tl.X, tl.Y, width, height, 
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
            screenPixel.UnlockBits(srcData);
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
        }*/

        private unsafe Color? GetScreenColourZones(Rectangle area, BitmapData srcData, Graphics gdest, Graphics gsrc)
        {
            IntPtr hSrcDC;
            IntPtr hDC;

            var width = area.Width;

            var height = area.Height;
            
            if (height == 0 || width == 0) return null;

            var thumbSize = new Size(1, 1);

            hSrcDC = gsrc.GetHdc();
            hDC = gdest.GetHdc();

            // we are scaling the original image down to a pixel, i.e. average colour
            SetStretchBltMode(hDC, 0x04);
            StretchBlt(hDC, 0, 0, thumbSize.Width, thumbSize.Height, hSrcDC, area.X, area.Y, width, height,
                (int)CopyPixelOperation.SourceCopy);

            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();

            // get the first (and only) pixel
            var scan0 = srcData.Scan0;

            var p = (byte*)(void*)scan0;
            // grab the colours from the pixel
            Color all = Color.FromArgb(255, p[2], p[1], p[0]);
            return all;
        }

        // this is used for dominant colour; it works but it doesn't look very good
        /*public static Color dominantColour(Bitmap bmp)
        {

            Color MostUsedColor;
            //int MostUsedColorIncidence;

        int pixelColor;

        Dictionary<int, int> dctColorIncidence;

        MostUsedColor = Color.Empty;
            //MostUsedColorIncidence = 0;

            dctColorIncidence = new Dictionary<int, int>();

            // this is what you want to speed up with unmanaged code
            for (int row = 0; row < bmp.Size.Width; row++)
            {
                for (int col = 0; col < bmp.Size.Height; col++)
                {
                    pixelColor = bmp.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }

            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            MostUsedColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
            // MostUsedColorIncidence = dctSortedByValueHighToLow.First().Value;
            return MostUsedColor;
        }*/
        #endregion
    }


    public class BulbSetting
    {
        public string Label { get; set; }
        public int Zones { get; set; }
        public ScreenLocation ScreenLocation { get; set; }

        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public bool Enabled { get; set; }
    }

    public class GdiInterop
    {
        /// <summary>
        /// Enumeration for the raster operations used in BitBlt.
        /// In C++ these are actually #define. But to use these
        /// constants with C#, a new enumeration _type is defined.
        /// </summary>
        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020, // dest = source
            SRCPAINT = 0x00EE0086, // dest = source OR dest
            SRCAND = 0x008800C6, // dest = source AND dest
            SRCINVERT = 0x00660046, // dest = source XOR dest
            SRCERASE = 0x00440328, // dest = source AND (NOT dest)
            NOTSRCCOPY = 0x00330008, // dest = (NOT source)
            NOTSRCERASE = 0x001100A6, // dest = (NOT src) AND (NOT dest)
            MERGECOPY = 0x00C000CA, // dest = (source AND pattern)
            MERGEPAINT = 0x00BB0226, // dest = (NOT source) OR dest
            PATCOPY = 0x00F00021, // dest = pattern
            PATPAINT = 0x00FB0A09, // dest = DPSnoo
            PATINVERT = 0x005A0049, // dest = pattern XOR dest
            DSTINVERT = 0x00550009, // dest = (NOT dest)
            BLACKNESS = 0x00000042, // dest = BLACK
            WHITENESS = 0x00FF0062, // dest = WHITE
        };

        /// <summary>
        /// Enumeration to be used for those Win32 function 
        /// that return BOOL
        /// </summary>
        public enum Bool
        {
            False = 0,
            True
        };

        /// <summary>
        /// Sets the background color.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="crColor">Color of the cr.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr hdc, int crColor);

        /// <summary>
        /// CreateCompatibleDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// DeleteDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// SelectObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <summary>
        /// DeleteObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// CreateCompatibleBitmap
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject, int width, int height);

        /// <summary>
        /// BitBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// StretchBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool StretchBlt(IntPtr hObject, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hObjSource, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// SetStretchBltMode
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool SetStretchBltMode(IntPtr hObject, int nStretchMode);
    }
}