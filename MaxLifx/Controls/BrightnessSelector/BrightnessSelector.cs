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
    public class BrightnessSelector : Control
    {
        private double _brightness;
        private bool _free;
        private int _handleCount;

        private bool _invert;
        private bool _linkRanges;
        private bool _perBulb;
        private int _currentHandle = -1;
        private int _currentRangeHandle = -1;

        public BrightnessSelector()
        {
            ResizeRedraw = true;
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Handles = new List<BrightnessSelectorHandle>();
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

        public double Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = value;
                Invalidate();
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

        private List<BrightnessSelectorHandle> Handles { get; }
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

        [Category("Action"), Description("Fires when brightnesses change.")]
        public event EventHandler BrightnessesChanged;

        public void GetBrightnesses(out List<float> brightnesses, out List<float> brightnessRanges)
        {
            brightnesses = Handles.Select(x => x.Brightness).ToList();
            brightnessRanges = Handles.Select(x => x.BrightnessRange).ToList();
        }

        public void SetBrightnesses(List<float> brightnesses, List<float> brightnessRanges)
        {
            var ctr = 0;
            foreach (var h in Handles.OrderBy(x => x.HandleNumber))
            {
                if (ctr > brightnesses.Count - 1)
                    break;
                h.Brightness = brightnesses[ctr];
                h.BrightnessRange = brightnessRanges[ctr];
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
                    Handles.Add(new BrightnessSelectorHandle(i));
            else if (HandleCount < Handles.Count)
                for (var i = HandleCount; i < Handles.Count; i++)
                    Handles.RemoveAt(i);

            Invalidate();
        }

        private void UpdateBrightnessFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs) (e);

            var handleToChange = Handles.Single(x => x.HandleNumber == _currentHandle);

            handleToChange.Brightness = 1-(float)mouseEv.Y / ClientRectangle.Height;

            if (handleToChange.Brightness + handleToChange.BrightnessRange > 1) handleToChange.Brightness = 1 - handleToChange.BrightnessRange;
            if (handleToChange.Brightness - handleToChange.BrightnessRange < 0) handleToChange.Brightness = handleToChange.BrightnessRange;

            Invalidate();
            BrightnessesChanged?.Invoke(null, null);
        }

        private void UpdateRangeFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs) (e);

            var handleToChange = Handles.Single(x => x.HandleNumber == _currentRangeHandle);

            var brightnessRangeRaw = (1 - (float) mouseEv.Y/ClientRectangle.Height);
            handleToChange.BrightnessRange = brightnessRangeRaw > handleToChange.Brightness ? brightnessRangeRaw - handleToChange.Brightness : handleToChange.Brightness - brightnessRangeRaw;

            if (handleToChange.Brightness + handleToChange.BrightnessRange > 1) handleToChange.BrightnessRange = 1 - handleToChange.Brightness;
            if (handleToChange.Brightness - handleToChange.BrightnessRange < 0) handleToChange.BrightnessRange = handleToChange.Brightness;

            Invalidate();
            BrightnessesChanged?.Invoke(null, null);
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

            DrawGreyscale(e);

            DrawHandles(e);
        }

        private void DrawGreyscale(PaintEventArgs e)
        {
            LinearGradientBrush linGrBrush = new LinearGradientBrush(
               new System.Drawing.Point(0, Size.Height),
               new System.Drawing.Point(0, -1),
               Color.FromArgb(255, 0, 0, 0),
               Color.FromArgb(255, 255, 255, 255));

            Pen pen = new Pen(linGrBrush);

            e.Graphics.FillRectangle(linGrBrush, new Rectangle(System.Drawing.Point.Empty, Size));
        }

        private void DrawHandles(PaintEventArgs e)
        {
            var handleClientRectangle = new Rectangle(ClientRectangle.X + 10, ClientRectangle.Y + 10,
                ClientRectangle.Width - 20, ClientRectangle.Height - 20);
            foreach (var handle in Handles.OrderByDescending(x => x.HandleNumber))
                DrawHandles(e, handle);
        }

        private void DrawHandles(PaintEventArgs e, BrightnessSelectorHandle handle)
        {
            var range1Rect = handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, true,
                PerBulb ? handle.HandleNumber : 1, HandleCount, handle.HandleNumber);

            var r1Centre = new System.Drawing.Point(range1Rect.X + range1Rect.Width/2, range1Rect.Y );

            var range2Rect = handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, false,
                PerBulb ? handle.HandleNumber : 1, HandleCount, handle.HandleNumber);

            var r2Centre = new System.Drawing.Point(range2Rect.X + range2Rect.Width / 2, range2Rect.Y + range1Rect.Height);

            e.Graphics.DrawLine(Pens.Black, r1Centre, r2Centre);

            var handleBrush = new SolidBrush(Color.FromArgb(224, 128, 128, 255));

            DrawHandle(e, handle, false, handleBrush, range1Rect);
            DrawHandle(e, handle, false, handleBrush, range2Rect);

            handleBrush = new SolidBrush(Color.FromArgb(224, 128, 255, 128));

            DrawHandle(e, handle, true, handleBrush,
                handle.GetHandleRectangle(ClientRectangle, HalfHandleSizeX, HalfHandleSizeY,
                    PerBulb ? handle.HandleNumber : 1, HandleCount, handle.HandleNumber));
        }

        private void DrawHandle(PaintEventArgs e, BrightnessSelectorHandle handle, bool label, SolidBrush handleBrush,
            Rectangle handleRect)
        {
            e.Graphics.FillEllipse(handleBrush, handleRect);
            e.Graphics.DrawEllipse(Pens.Black, handleRect);

            if (label)
                using (var font1 = new Font("Segoe UI", ClientRectangle.Width/12, System.Drawing.FontStyle.Bold, GraphicsUnit.Point))
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
                    PerBulb ? handle.HandleNumber : 1, HandleCount, handle.HandleNumber);

                if (handleRect.Contains(e.Location))
                {
                    _currentHandle = handle.HandleNumber;
                    UpdateBrightnessFromMouse(e);
                    break;
                }

                handleRect = handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, true,
                    PerBulb ? handle.HandleNumber : 1, HandleCount, handle.HandleNumber);

                if (handleRect.Contains(e.Location))
                {
                    _currentRangeHandle = handle.HandleNumber;
                    UpdateRangeFromMouse(e);
                    break;
                }

                handleRect = handle.GetHandleRangeRectangle(ClientRectangle, HalfHandleSizeX/2, HalfHandleSizeY/2, false,
                    PerBulb ? handle.HandleNumber : 1, HandleCount, handle.HandleNumber);

                if (handleRect.Contains(e.Location))
                {
                    _currentRangeHandle = handle.HandleNumber;
                    UpdateRangeFromMouse(e);
                    break;
                }
            }
        }

        public void ResetRanges(float dflt = .1f)
        {
            foreach (var handle in Handles)
            {
                handle.BrightnessRange = dflt;
                if (handle.Brightness + handle.BrightnessRange > 1) handle.BrightnessRange = 1 - handle.Brightness;
                else if (handle.Brightness - handle.BrightnessRange < 0) handle.BrightnessRange = handle.Brightness;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_currentHandle != -1)
                UpdateBrightnessFromMouse(e);

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