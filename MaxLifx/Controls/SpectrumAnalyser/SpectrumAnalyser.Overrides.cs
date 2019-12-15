using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MaxLifx.Controls
{
    public partial class SpectrumAnalyser
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

        public bool ShowUpdates = true;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.Clear(BackColor);

            DrawGraph(e.Graphics);

            DrawHandles(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnClick(e);

            foreach (var handle in _handles)
            {
                var handleRects = GetHandleRects(handle, 30);
            
                 if (handleRects[1].Contains(e.Location) || handleRects[2].Contains(e.Location))
                {
                    _currentHandle = handle;
                    _currentHandleIsRange = true;
                    UpdateLevelRangeFromMouse(e);
                }
                else if (handleRects[0].Contains(e.Location))
                {
                    _currentHandle = handle;
                    _currentHandleIsRange = false;
                    UpdateBinAndLevelFromMouse(e);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_currentHandleIsRange &&_currentHandle != null)
                UpdateBinAndLevelFromMouse(e);
            else if (_currentHandleIsRange && _currentHandle != null)
                UpdateLevelRangeFromMouse(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _currentHandle = null;
        }


    }
}