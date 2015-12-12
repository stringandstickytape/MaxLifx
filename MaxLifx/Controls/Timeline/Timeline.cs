using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Timers;
using System.Windows;
using NAudio.Wave;

namespace MaxLifx.Controls
{
    public partial class Timeline
    {
        public List<TimelineEvent> TimelineEvents = new List<TimelineEvent>();
        public Timeline()
        {
            ResizeRedraw = true;
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private readonly int samplesPerPixel = 44100/8;
        public Dictionary<string, Bitmap> WaveBitmaps = new Dictionary<string, Bitmap>();
        public Dictionary<string, float> WaveBitmapDurations = new Dictionary<string, float>();

        public float PlaybackTime
        {
            get { return _playbackTime; }
            set
            {
                _playbackTime = value; Invalidate();
            }
        }

        public void AddEvent(TimelineEvent x)
        {
            PopulateBitmapCache(x);

            TimelineEvents.Add(x);
            Invalidate();
        }

        public void DeleteEvent(TimelineEvent x)
        {
            TimelineEvents.Remove(x);
            Invalidate();
        }

        public void PopulateBitmapCache(TimelineEvent x)
        {
            if (x.Action == TimelineEventAction.PlayMp3 && !WaveBitmaps.ContainsKey(x.Parameter))
            {
                var reader = new Mp3FileReader(x.Parameter);
                var width = ((double)reader.Length / samplesPerPixel);
                var widthFloor = (int)Math.Floor(width);
                Bitmap b = new Bitmap(widthFloor, 32, PixelFormat.Format16bppRgb555);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);

                    ISampleProvider m = reader.ToSampleProvider();
                    float[] buffer = new float[reader.Length / 2];
                    m.Read(buffer, 0, buffer.Length);

                    var binCt = b.Width;
                    var bins = new float[binCt];
                    var samplesPerBin = (float)buffer.Length / binCt;

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        var bin = (int)(Math.Floor(i / (samplesPerBin + 1)));
                        if (bins[bin] < buffer[i])
                            bins[bin] = buffer[i];
                    }

                    for (int i = 0; i < binCt; i++)
                    {
                        var centre = b.Height / 2;
                        var scaledBinValue = bins[i] * centre;
                        g.DrawLine(Pens.Black, i, centre - scaledBinValue, i, centre + scaledBinValue);
                    }
                    Bitmap c = new Bitmap(b.Width, b.Height, PixelFormat.Format1bppIndexed);
                    c.Palette.Entries[0] = Color.DarkGray;
                    c = b.Clone(new Rectangle(0,0,b.Width,b.Height), PixelFormat.Format1bppIndexed);
                    
                    WaveBitmaps.Add(x.Parameter, c);
                    b.Dispose();
                    WaveBitmapDurations.Add(x.Parameter, (float)reader.Length / (reader.WaveFormat.SampleRate * (reader.WaveFormat.BitsPerSample / 8) * reader.WaveFormat.Channels));

                    reader.Close();
                    reader.Dispose();

                }

            }
            GC.Collect(2);
        }

        public long Duration = 10000;

        public Vector ViewableWindow = new Vector(0,10000);
        private float _playbackTime;
        public double ViewableWindowSize => ViewableWindow.Y - ViewableWindow.X;


    }
}