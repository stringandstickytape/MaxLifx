using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MaxLifx.Controls
{
    public partial class Timeline : Control
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        [Category("Layout"), Description("Specifies size of the control.")]
        public Size Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                Invalidate();
            }
        }

        private Point dividerPoint
        {
            get
            {
                return new Point(((int) (0)), (int) (DisplayRectangle.Height-0));
            }
        }

        private Rectangle MainTimelineArea {get { return new Rectangle(dividerPoint.X,0,DisplayRectangle.Width - dividerPoint.X, dividerPoint.Y );}}
        private Rectangle YAxisArea { get { return new Rectangle(dividerPoint.X, dividerPoint.Y, DisplayRectangle.Width - dividerPoint.X, DisplayRectangle.Height - dividerPoint.Y); } }

        private Rectangle XAxisArea { get { return new Rectangle(0, 0, dividerPoint.X, dividerPoint.Y); } }
        private Rectangle UnusedArea { get { return new Rectangle(0, dividerPoint.Y, dividerPoint.X, DisplayRectangle.Height - dividerPoint.Y); } }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void Initialise()
        {
            Invalidate();
        }


        protected void InvalidateEx()
        {
            if (Parent == null)
                return;
            var rc = new Rectangle(Location, Size);
            Parent.Invalidate(rc, true);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Initialise();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.Clear(BackColor);
            DrawTimeline(e);
            e.Graphics.FillRectangle(Brushes.Yellow, YAxisArea);
            e.Graphics.FillRectangle(Brushes.Green, XAxisArea);
            e.Graphics.FillRectangle(Brushes.Blue, UnusedArea);
        }

        private void DrawTimeline(PaintEventArgs e)
        {
            //if (PlaybackTime >= ViewableWindow.Y && previousPlaybackTime <= ViewableWindow.Y)
            //    MessageBox.Show("!");

            e.Graphics.FillRectangle(Brushes.White, MainTimelineArea);
            DrawWaves(e.Graphics);
            e.Graphics.DrawLine(Pens.Red, MainTimelineArea.X, MainTimelineArea.Height / 2, MainTimelineArea.Width + MainTimelineArea.X, MainTimelineArea.Height / 2);
            e.Graphics.DrawString(Math.Floor((float)(ViewableWindow.X / 1000) / 60) + ":" + (((int)(ViewableWindow.X / 1000)) % 60).ToString("00.#"), SystemFonts.MessageBoxFont, Brushes.Black, MainTimelineArea.X, MainTimelineArea.Height / 2);

            string rm = (ViewableWindow.Y/1000).ToString("0.#");
            var a = e.Graphics.MeasureString(rm, SystemFonts.MessageBoxFont);
            e.Graphics.DrawString(Math.Floor((float)(ViewableWindow.Y/1000) / 60) + ":" + (((int)(ViewableWindow.Y / 1000)) % 60).ToString("00.#"), SystemFonts.MessageBoxFont, Brushes.Black, MainTimelineArea.X + MainTimelineArea.Width - a.Width, MainTimelineArea.Height / 2);

            var ticksFloor = (int)Math.Ceiling(ViewableWindow.X/1000);
            var ticksCeiling = (int)Math.Floor(ViewableWindow.Y/1000);

            int ticksIncrement = 1;

            for (int i = ticksFloor; i <= ticksCeiling; i+= ticksIncrement)
            {
                if (
                    (ViewableWindow.Y - ViewableWindow.X < 30000) ||
                    ((ViewableWindow.Y - ViewableWindow.X >= 30000 && ViewableWindow.Y - ViewableWindow.X < 300000) && (Math.Abs(i)%10 == 0)) ||
                    ((ViewableWindow.Y - ViewableWindow.X >= 300000 && ViewableWindow.Y - ViewableWindow.X < 3000000) && (Math.Abs(i) % 100 == 0)) ||
                    ((ViewableWindow.Y - ViewableWindow.X >= 3000000 && ViewableWindow.Y - ViewableWindow.X < 30000000) && (Math.Abs(i) % 1000 == 0))
                    )
                {
                    int xPos = TimeToPixels(i);
                    e.Graphics.DrawLine(Pens.Red, xPos, ((int)(MainTimelineArea.Height * .45)), xPos,
                        (int)(MainTimelineArea.Height * .55));

                    var textString = Math.Floor((float)i/60)+":"+(i%60).ToString("00.#");

                    e.Graphics.DrawString(textString, SystemFonts.MessageBoxFont, Brushes.DarkGoldenrod, xPos,
                        MainTimelineArea.Height / 2);
                }
            }

            DrawEvents(e.Graphics);

            DrawPlaybackTime(e.Graphics);
        }

        private float previousPlaybackTime;
        private void DrawPlaybackTime(Graphics graphics)
        {
            int xPos = TimeToPixels(PlaybackTime/1000);
            graphics.DrawLine(Pens.Red, xPos, 0, xPos, MainTimelineArea.Height);
            previousPlaybackTime = PlaybackTime;
        }

        private int TimeToPixels(double i)
        {
            return (int)(MainTimelineArea.X + MainTimelineArea.Width / (ViewableWindow.Y - ViewableWindow.X) * (i * 1000 - ViewableWindow.X));
        }

        private double PixelsToTime(int i)
        {
            return ViewableWindowSize*i/MainTimelineArea.Width;
        }

        private void DrawEvents(Graphics g)
        {
            foreach (
                TimelineEvent e in TimelineEvents.Where(x => x.Time >= ViewableWindow.X && x.Time <= ViewableWindow.Y))
            {
                Rectangle rect = HandleLocation(e);

                var colour = SelectedEvents.Contains(e) ? Brushes.MediumVioletRed : Brushes.DimGray;
                g.FillRectangle(colour, rect);
                Font f = new Font("Segoe UI", 15);

                g.DrawString(e.ToString(), f, new SolidBrush(Color.DarkGoldenrod), new PointF(rect.X, 0), new StringFormat(StringFormatFlags.DirectionVertical));
            }
        }


        private void DrawWaves(Graphics g)
        {
            foreach (TimelineEvent e in TimelineEvents.Where(x => x.Action == TimelineEventAction.PlayMp3).OrderBy(x => x.Time))
            //.Where(x => x.Time >= ViewableWindow.X && x.Time <= ViewableWindow.Y))
            {
                Rectangle rect = HandleLocation(e);
                var endTime = WaveBitmapDurations[e.Parameter] * 1000 + e.Time;
                var endTimePixels = TimeToPixels(endTime / 1000) - (rect.X + rect.Width / 2);
                Console.WriteLine(endTime);
                Console.WriteLine(endTimePixels);

                var oldMode = g.InterpolationMode;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(WaveBitmaps[e.Parameter], new Rectangle(rect.X + rect.Width / 2, 0, endTimePixels, MainTimelineArea.Height));
                g.InterpolationMode = oldMode;

            }
        }
        private Rectangle HandleLocation(TimelineEvent e)
        {
            var xPos = TimeToPixels(e.Time / 1000);
            var rect = new Rectangle(xPos - 5, MainTimelineArea.Height / 2 - 20, 10, 40);
            return rect;
        }

        private Point? _dragStart;
        public List<TimelineEvent> SelectedEvents = new List<TimelineEvent>();
        private bool _mouseDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnClick(e);
            _mouseDown = true;
            bool evtWasClicked = false;
            foreach (
                TimelineEvent evt in TimelineEvents.Where(x => x.Time >= ViewableWindow.X && x.Time <= ViewableWindow.Y))
            {
                Rectangle rect = HandleLocation(evt);
                if (rect.Contains(e.Location))
                {
                    if (
                        !((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftCtrl) &
                           System.Windows.Input.KeyStates.Down) > 0))
                        SelectedEvents.Clear();

                    evtWasClicked = true;

                    if (((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftShift) & System.Windows.Input.KeyStates.Down) > 0)
                        &&
                        !((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftCtrl) & System.Windows.Input.KeyStates.Down) > 0)
                        )
                    {
                        var newEvent = new TimelineEvent(evt.Action, evt.Parameter, (long)PixelsToTime(e.X - dividerPoint.X));
                        TimelineEvents.Add(newEvent);
                        SelectedEvents.Add(newEvent);
                    }
                    else if (((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftShift) & System.Windows.Input.KeyStates.Down) > 0)
                        &&
                        ((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftCtrl) & System.Windows.Input.KeyStates.Down) > 0)
                        )
                    {
                        var newEvents = new List<TimelineEvent>();

                        foreach (var selEvt in SelectedEvents)
                            newEvents.Add(new TimelineEvent(selEvt.Action, selEvt.Parameter, selEvt.Time));


                        TimelineEvents.AddRange(newEvents);
                        SelectedEvents.Clear();
                        SelectedEvents.AddRange(newEvents);
                    }
                    else
                    {
                        if (!SelectedEvents.Contains(evt))
                            SelectedEvents.Add(evt);
                    }
                    break;
                }
            }

            if (!evtWasClicked && !((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftCtrl) &
                           System.Windows.Input.KeyStates.Down) > 0))
                SelectedEvents.Clear();

            _dragStart = e.Location;
            Invalidate();
        }

        private bool _wasDragged;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragStart != null && SelectedEvents.Count == 0 && _mouseDown)
            {
                var xDragChange = e.X - _dragStart.Value.X;
                if (xDragChange == 0) return;
                var xShift = xDragChange*(ViewableWindow.Y - ViewableWindow.X) / MainTimelineArea.Width;
                if (ViewableWindow.X - xShift < 0) xShift = ViewableWindow.X;
                ViewableWindow.X -= xShift;
                ViewableWindow.Y -= xShift;
                Invalidate();
                _dragStart = e.Location;
                _wasDragged = true;
            }
            else if (_dragStart != null && SelectedEvents != null && _mouseDown)
            {
                var newTime = PixelsToTime(e.X - dividerPoint.X) + ViewableWindow.X;
                var oldTime = PixelsToTime(_dragStart.Value.X - dividerPoint.X) + ViewableWindow.X;
                if (newTime < 0) return;
                foreach (var evt in SelectedEvents)
                {
                    evt.Time += (long) (newTime - oldTime);
                    Console.WriteLine((long)(newTime - oldTime));
                }
                _dragStart = e.Location;
                Invalidate();
                _wasDragged = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (SelectedEvents.Count == 0 && _wasDragged == false)
            {
                PlaybackTime = (float)PixelsToTime(e.X - dividerPoint.X) + (float)ViewableWindow.X;
                foreach (var evt in TimelineEvents.Where(x => x.Fired && x.Time > PlaybackTime))
                    evt.Fired = false;
            }
            _mouseDown = false;
            _wasDragged = false;
            _dragStart = null;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta == 0) return;

            if (MainTimelineArea.Contains(e.Location))
            {
                if ((e.Delta < 0 && ViewableWindowSize < 3600000) || (e.Delta > 0 && ViewableWindowSize > 10000))
                {
                    var halfWindowSize = (ViewableWindow.Y - ViewableWindow.X)/5;

                    var timelineZoomLocation = PixelsToTime(e.X - dividerPoint.X) + ViewableWindow.X;
                    var timelineZoomPixels = e.X;

                    var direction = e.Delta/-120;
                    if (direction > 0)
                    {
                        ViewableWindow.X -= halfWindowSize;
                        ViewableWindow.Y += halfWindowSize;
                    }
                    else
                    {
                        ViewableWindow.X += halfWindowSize/2;
                        ViewableWindow.Y -= halfWindowSize/2;
                    }

                    var scrollDifference = (ViewableWindow.X + ViewableWindowSize/2) - timelineZoomLocation;
                    var pixelsToScroll = timelineZoomPixels - (MainTimelineArea.X + MainTimelineArea .Width / 2);
                    var extraScrollDifference = PixelsToTime(pixelsToScroll);
                    scrollDifference += extraScrollDifference;
                    ViewableWindow.X -= scrollDifference;
                    ViewableWindow.Y -= scrollDifference;

                    if (ViewableWindow.X < 0)
                    {
                        ViewableWindow.Y -= ViewableWindow.X;
                        ViewableWindow.X = 0;
                    }
                    Invalidate();
                }
            }
        }
    }
}