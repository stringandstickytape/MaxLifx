using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MaxLifx.Controls.ColourStrategy;
using Size = System.Drawing.Size;

namespace MaxLifx.Controls
{
    public class HueSelector : Control
    {
        private bool _free;
        private int _handleCount;

        private bool _invert;
        private bool _linkRanges;
        private bool _perBulb;
        private int _currentHandle = -1;
        private int _currentRangeHandle = -1;

        public HueSelector()
        {
            ResizeRedraw = true;
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Handles = new List<HueSelectorHandle>();
            HandleCount = 1;

            SelectedOneColourStrategy = new AnalogousColourStrategy();
            SelectedTwoColourStrategy = new AnalogousColourStrategy();
            SelectedThreeColourStrategy = new AnalogousColourStrategy();
            GenericColourStrategy = new AnalogousColourStrategy();
        }

        public int HandleCount
        {
            get { return _handleCount; }
            set
            {
                _handleCount = value;
                Initialise();
            }
        }

        [Category("Behavior"), Description("Determines whether changing one range changes all ranges.")]
        public bool LinkRanges
        {
            get { return _linkRanges; }
            set
            {
                if (_linkRanges != value)
                {
                    _linkRanges = value;
                }
            }
        }

        public bool Free
        {
            get { return _free; }
            set
            {
                if (_free != value)
                {
                    _free = value;

                    if (!Free)
                    {
                        SelectedOneColourStrategy = new AnalogousColourStrategy();
                        SelectedTwoColourStrategy = new AnalogousColourStrategy();
                        SelectedThreeColourStrategy = new AnalogousColourStrategy();
                        GenericColourStrategy = new AnalogousColourStrategy();
                    }
                    else
                    {
                        SelectedOneColourStrategy = new FreeColourStrategy();
                        SelectedTwoColourStrategy = new FreeColourStrategy();
                        SelectedThreeColourStrategy = new FreeColourStrategy();
                        GenericColourStrategy = new FreeColourStrategy();
                    }
                }
            }
        }

        [Category("Behavior"), Description("Determines whether there is one handle per bulb, or two handles in total")]
        public bool PerBulb
        {
            get { return _perBulb; }
            set
            {
                if (_perBulb != value)
                {
                    _perBulb = value;

                    if (_perBulb == false)
                        HandleCount = 1;

                    Initialise();
                }
            }
        }

        public bool Invert
        {
            get { return _invert; }
            set
            {
                _invert = value;
                Invalidate();
            }
        }

        private List<HueSelectorHandle> Handles { get; }
        public IColourStrategy SelectedOneColourStrategy { get; set; }
        public IColourStrategy SelectedTwoColourStrategy { get; set; }
        public IColourStrategy SelectedThreeColourStrategy { get; set; }
        public IColourStrategy GenericColourStrategy { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        private int HalfHandleSizeX => Size.Width/30;

        private int HalfHandleSizeY => Size.Height/30;

        [Category("Action"), Description("Fires when hues change.")]
        public event EventHandler HuesChanged;

        public void GetHues(out List<int> hues, out List<int> hueRanges, out List<double> saturations,
            out List<double> saturationRanges)
        {
            hues = Handles.Select(x => (int) (x.Hue)).ToList();
            hueRanges = Handles.Select(x => (int) (x.HueRange)).ToList();
            saturations = Handles.Select(x => x.Saturation).ToList();
            saturationRanges = Handles.Select(x => x.SaturationRange).ToList();
        }

        public void SetHuesAndSaturations(List<int> hues, List<int> hueRanges, List<double> saturations,
            List<double> saturationRanges)
        {
            var ctr = 0;
            foreach (var h in Handles.OrderBy(x => x.HandleNumber))
            {
                if (ctr > hues.Count - 1)
                    break;
                h.Hue = hues[ctr];
                h.HueRange = hueRanges[ctr];
                h.Saturation = saturations[ctr];
                h.SaturationRange = saturationRanges[ctr];
                ctr++;
            }
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
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

        private void Initialise()
        {
            if(Handles.Count < HandleCount)
                for (var i = Handles.Count; i < HandleCount; i++)
                    Handles.Add(new HueSelectorHandle(i));
            else if (HandleCount < Handles.Count)
                for (var i = HandleCount; i < Handles.Count; i++)
                    Handles.RemoveAt(i);

            Invalidate();
        }

        private void UpdateHueAndSaturationFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs) (e);
            var v = new Vector((mouseEv.X - Size.Width/2)*Size.Height, (mouseEv.Y - Size.Height/2)*Size.Width);

            var up = new Vector(0, -1);

            var handleToChange = Handles.Single(x => x.HandleNumber == _currentHandle);
            var previousHue = handleToChange.Hue;
            handleToChange.Hue = (Vector.AngleBetween(up, v) + 360)%360;

            var halfWidth = (ClientRectangle.Width)/2;
            var halfHeight = (ClientRectangle.Height)/2;
            var dist = Math.Sqrt(Math.Pow(halfWidth - mouseEv.X, 2) + Math.Pow(halfHeight - mouseEv.Y, 2))/
                       (halfWidth - 10);
            if (dist > 1) dist = 1;

            var previousSaturation = handleToChange.Saturation;
            handleToChange.Saturation = dist;

            SetDependentHues(_currentHandle, previousHue, previousSaturation);
            Invalidate();
            HuesChanged(null, null);
        }

        private void UpdateRangeFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs) (e);
            var v = new Vector((mouseEv.X - Size.Width/2)*Size.Height, (mouseEv.Y - Size.Height/2)*Size.Width);

            var up = new Vector(0, -1);
            var handleInQuestion = Handles.Single(x => x.HandleNumber == _currentRangeHandle);
            handleInQuestion.HueRange = handleInQuestion.Hue - (Vector.AngleBetween(up, v) + 360)%360;

            SetDependentRanges(_currentRangeHandle);
            Invalidate();
            HuesChanged(null, null);
        }

        private void SetDependentRanges(int fromHandleNumber)
        {
            if (LinkRanges)
            {
                var thisHandle = Handles.Single(x => x.HandleNumber == fromHandleNumber);
                foreach (var handle in Handles.Where(x => x.HandleNumber != fromHandleNumber))
                    handle.HueRange = thisHandle.HueRange;
            }
        }

        private void SetDependentHues(int fromHandleNumber, double previousHue, double previousSaturation)
        {
            if (PerBulb)
                switch (HandleCount)
                {
                    case 1:
                        SelectedOneColourStrategy.ProcessHandles(Handles, fromHandleNumber, previousHue,
                            previousSaturation);
                        break;
                    case 2:
                        SelectedTwoColourStrategy.ProcessHandles(Handles, fromHandleNumber, previousHue,
                            previousSaturation);
                        break;
                    case 3:
                        SelectedThreeColourStrategy.ProcessHandles(Handles, fromHandleNumber, previousHue,
                            previousSaturation);
                        break;
                    default:
                        GenericColourStrategy.ProcessHandles(Handles, fromHandleNumber, previousHue,
                            previousSaturation);
                        break;
                }
        }

        #region Overrides

        [Category("Layout"), Description("Specifies size of the control.")]
        public Size Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                Invalidate(); // invoke OnPaint
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.Clear(BackColor);

            DrawColorWheel(e);

            DrawHandles(e);
        }

        private void DrawColorWheel(PaintEventArgs e)
        {
            var ptsTotal = 12;
            // Make the points for a hexagon.
            var pts = new PointF[ptsTotal];
            var cx = ClientRectangle.Width/2;
            var cy = ClientRectangle.Height/2;
            var theta = 0 - Math.PI/2;
            var dtheta = 2*Math.PI/ptsTotal;
            for (var i = 0; i < pts.Length; i++)
            {
                pts[i].X = (int) (cx + cx*Math.Cos(theta));
                pts[i].Y = (int) (cy + cy*Math.Sin(theta));
                theta += dtheta;
            }

            var h = 255;
            var m = 127;

            // Make a path gradient brush.
            using (var path_brush =
                new PathGradientBrush(pts))
            {
                // Define the center and surround colors.
                path_brush.CenterColor = Color.FromArgb(255, h, h, h);
                path_brush.SurroundColors = new[]
                {
                    Color.FromArgb(255, h, 0, 0), Color.FromArgb(255, h, m, 0), Color.FromArgb(255, h, h, 0),
                    Color.FromArgb(255, m, h, 0),
                    Color.FromArgb(255, 0, h, 0), Color.FromArgb(255, 0, h, m), Color.FromArgb(255, 0, h, h),
                    Color.FromArgb(255, 0, m, h),
                    Color.FromArgb(255, 0, 0, h), Color.FromArgb(255, m, 0, h), Color.FromArgb(255, h, 0, h),
                    Color.FromArgb(255, h, 0, m)
                };

                // Fill the hexagon.
                e.Graphics.FillEllipse(path_brush, ClientRectangle);
            }

            var rect = new Rectangle(ClientRectangle.Location.X + 5, ClientRectangle.Location.Y + 5,
                ClientRectangle.Size.Width - 10, ClientRectangle.Size.Height - 10);
            // Outline the hexagon.
            var p = new Pen(BackColor, 10);
            e.Graphics.DrawEllipse(p, rect);
            p = new Pen(Color.Black, 2);
            rect = new Rectangle(ClientRectangle.Location.X + 9, ClientRectangle.Location.Y + 9,
                ClientRectangle.Size.Width - 18, ClientRectangle.Size.Height - 18);
            e.Graphics.DrawEllipse(p, rect);
        }

        private void DrawHandles(PaintEventArgs e)
        {
            var handleClientRectangle = new Rectangle(ClientRectangle.X + 10, ClientRectangle.Y + 10,
                ClientRectangle.Width - 20, ClientRectangle.Height - 20);
            foreach (var handle in Handles.OrderByDescending(x => x.HandleNumber))
            {
                //var rectSize = .615 + (PerBulb ? handle.HandleNumber : 1) * .150;
                var rectSize = handle.Saturation;
                var rectMargin = (1 - rectSize)/2;

                var arcRect = new Rectangle((int) (handleClientRectangle.X + (handleClientRectangle.Width*rectMargin)),
                    (int) (handleClientRectangle.Y + (handleClientRectangle.Height*rectMargin)),
                    (int) ((handleClientRectangle.Width)*rectSize),
                    (int) ((handleClientRectangle.Height)*rectSize));

                var c = Color.FromArgb(128, 255, 255, 255);
                if (handle.Saturation != 0 && arcRect.Width > 0 && arcRect.Height > 0)
                    DrawArc(e, handle, arcRect, c, 6);

                if (handle.Saturation != 0 && arcRect.Width > 0 && arcRect.Height > 0)
                    DrawArc(e, handle, arcRect, Color.Black, 2);

                DrawHandles(e, handle, true);
                DrawHandles(e, handle, false);
            }
        }

        private void DrawArc(PaintEventArgs e, HueSelectorHandle handle, Rectangle arcRect, Color penColor, int penWidth)
        {
            var p = new Pen(penColor, penWidth);

            try
            {
                e.Graphics.DrawArc(p, arcRect, (float) (handle.Hue) - 90, (float) (handle.HueRange));
                e.Graphics.DrawArc(p, arcRect, (float) (handle.Hue) - 90, 0 - (float) (handle.HueRange));
            }
            catch (OutOfMemoryException ex)
            { 
                // ..?
            }
        }

        private void DrawHandles(PaintEventArgs e, HueSelectorHandle handle, bool shadow)
        {
            var handleBrush = new SolidBrush(Color.FromArgb(128, 32, 32, 32));
            DrawHandle(e, handle, false, handleBrush,
                handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, true,
                    PerBulb ? handle.HandleNumber : 1, shadow));
            DrawHandle(e, handle, false, handleBrush,
                handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, false,
                    PerBulb ? handle.HandleNumber : 1, shadow));
            handleBrush = new SolidBrush(Color.FromArgb(128, 32, 32, 32));
            DrawHandle(e, handle, true, handleBrush,
                handle.GetHandleRectangle(ClientRectangle, HalfHandleSizeX, HalfHandleSizeY,
                    PerBulb ? handle.HandleNumber : 1, shadow));
        }

        private void DrawHandle(PaintEventArgs e, HueSelectorHandle handle, bool label, SolidBrush handleBrush,
            Rectangle handleRect)
        {
            e.Graphics.FillEllipse(handleBrush, handleRect);

            if (label)
                using (var font1 = new Font("Segoe UI", ClientRectangle.Width/30, System.Drawing.FontStyle.Bold, GraphicsUnit.Point))
                {
                    var stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(handle.HandleNumber.ToString(), font1, Brushes.White, handleRect, stringFormat);
                }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnClick(e);

            foreach (var handle in Handles)
            {
                var handleRect = handle.GetHandleRectangle(ClientRectangle, HalfHandleSizeX, HalfHandleSizeY,
                    PerBulb ? handle.HandleNumber : 1, false);

                if (handleRect.Contains(e.Location))
                {
                    _currentHandle = handle.HandleNumber;
                    UpdateHueAndSaturationFromMouse(e);
                    break;
                }

                handleRect = handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, true,
                    PerBulb ? handle.HandleNumber : 1, false);

                if (handleRect.Contains(e.Location))
                {
                    _currentRangeHandle = handle.HandleNumber;
                    UpdateRangeFromMouse(e);
                    break;
                }

                handleRect = handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, false,
                    PerBulb ? handle.HandleNumber : 1, false);

                if (handleRect.Contains(e.Location))
                {
                    _currentRangeHandle = handle.HandleNumber;
                    UpdateRangeFromMouse(e);
                    break;
                }
            }
        }

        public void ResetRanges(int dflt = 20)
        {
            foreach (var handle in Handles)
            {
                handle.HueRange = dflt;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_currentHandle != -1)
                UpdateHueAndSaturationFromMouse(e);

            if (_currentRangeHandle != -1)
                UpdateRangeFromMouse(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _currentHandle = -1;
            _currentRangeHandle = -1;
        }

        #endregion
    }
}