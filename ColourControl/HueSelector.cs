using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;


namespace ColourControl
{
    public class HueSelector : Control
    {
        [Category("Action"), Description("Fires when hues change.")]
        public event EventHandler HuesChanged;

        private int _handleCount = 0;
        [Category("Behavior"), Description("Determines how many handles there are")]
        public int HandleCount
        {
            get { return _handleCount; }
            set
            {
                _handleCount = value;
                Initialise();
            }
        }


        private bool _rangeRequired;
        [Category("Behavior"), Description("Determines whether a range is required")]
        public bool RangeRequired
        {
            get { return _rangeRequired; }
            set
            {
                _rangeRequired = value;
                Invalidate();
            }
        }

        private bool _invert;
        public bool Invert
        {
            get { return _invert; }
            set
            {
                _invert = value;
                Invalidate();
            }
        }

        public List<int> GetHues()
        {
            return Handles.Select(x => (int)(x.Hue)).ToList();
        }

        public void SetHues(List<int> hues)
        {
            int ctr = 0;
            foreach(var hue in hues)
            {
                Handles[ctr].Hue = hue;
                ctr++;
            }
            Invalidate();
            return;
        }

        private List<HueSelectorHandle> Handles { get; set; }
        private int currentHandle = -1;

        public HueSelector() 
        { 
            ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint |ControlStyles.AllPaintingInWmPaint, true);
            Handles = new List<HueSelectorHandle>();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            Initialise();
        }

        private void Initialise()
        {
            Handles.Clear();

            for (int i = 0; i < HandleCount; i++)
                Handles.Add(new HueSelectorHandle(i));

            SetDependentHues(0);

            Invalidate();
        }

        #region Overrides

        [Category("Layout"), Description("Specifies size of the control.")]
        public System.Drawing.Size Size
        {
        
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                Invalidate(); // invoke OnPaint
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.Clear(SystemColors.Control);

            var innerClientRect = new Rectangle(ClientRectangle.X + 4, ClientRectangle.Y + 4, ClientRectangle.Width - 8, ClientRectangle.Height - 8);

            var outerClientRect = new Rectangle(ClientRectangle.X + (int)(ClientRectangle.Width * 0.01),
                                                ClientRectangle.Y + (int)(ClientRectangle.Height * 0.01),
                                                (int)(ClientRectangle.Width) - (int)(ClientRectangle.Width * 0.01),
                                                (int)(ClientRectangle.Height) - (int)(ClientRectangle.Height * 0.01));

            var anotherInnerRect = new Rectangle(ClientRectangle.X + (int)(ClientRectangle.Width * 0.01),
                                                ClientRectangle.Y + (int)(ClientRectangle.Height * 0.01),
                                                (int)(ClientRectangle.Width) - (int)(ClientRectangle.Width * 0.02),
                                                (int)(ClientRectangle.Height) - (int)(ClientRectangle.Height * 0.02));

            e.Graphics.DrawImage(Properties.Resources.ColourWheel, outerClientRect);

            Pen p = new Pen(BackColor, (innerClientRect.Width + innerClientRect.Height) / 60);
            p.Width = p.Width < 2 ? 2 : p.Width;
            var ellipseShrink = (innerClientRect.Width + innerClientRect.Height) / 600;

            e.Graphics.DrawEllipse(p, anotherInnerRect);

            var innerCircleRect = new Rectangle((int)(ClientRectangle.X + ClientRectangle.Width * .24),
                                                (int)(ClientRectangle.Y + ClientRectangle.Height * .24),
                                                (int)(ClientRectangle.Width * .52),
                                                (int)(ClientRectangle.Height * .52));

            Pen innerCirclePen = new Pen(BackColor, (innerClientRect.Width + innerClientRect.Height) / 40);
            e.Graphics.DrawEllipse(innerCirclePen, innerCircleRect);

            DrawHandles(e);
        }

        private void DrawHandles(PaintEventArgs e)
        {
            if (HandleCount == 2 && RangeRequired)
            {
                var arcRect = new Rectangle((int)(ClientRectangle.X + ClientRectangle.Width * .125),
                    (int)(ClientRectangle.Y + ClientRectangle.Height * .125),
                    (int)(ClientRectangle.Width * .75),
                    (int)(ClientRectangle.Height * .75));

                var handle0centre = Handles[0].GetHandleRectangle(ClientRectangle, 0, 0);
                var handle1centre = Handles[1].GetHandleRectangle(ClientRectangle, 0, 0);
                handle0centre.X -= Size.Width / 2;
                handle1centre.X -= Size.Width / 2;
                handle0centre.Y -= Size.Height / 2;
                handle1centre.Y -= Size.Height / 2;

                System.Windows.Vector v0 = new System.Windows.Vector(handle0centre.X, handle0centre.Y);
                System.Windows.Vector v1 = new System.Windows.Vector(handle1centre.X, handle1centre.Y);
                System.Windows.Vector up = new System.Windows.Vector(0, -1);

                var angle0 = (System.Windows.Vector.AngleBetween(up, v0) + 360) % 360;
                var distance = (System.Windows.Vector.AngleBetween(up, v1) + 360) % 360 - angle0;

                if (distance < 0) distance = distance + 360;

                if (Invert) distance -= 360;

                Pen p = new Pen(Color.Black, 5);

                e.Graphics.DrawArc(p, arcRect, (float)angle0 - 90, (float)(distance));
            }


            foreach (var handle in Handles.OrderByDescending(x => x.HandleNumber))
            {
                SolidBrush handleBrush = new SolidBrush(Color.Black);
                Rectangle handleRect = handle.GetHandleRectangle(ClientRectangle, halfHandleSizeX, halfHandleSizeY);
                e.Graphics.FillEllipse(handleBrush, handleRect);

                using (Font font1 = new Font("Segoe UI", ClientRectangle.Width / 15, FontStyle.Bold, GraphicsUnit.Point))
                {
                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    // Draw the text and the surrounding rectangle.
                    e.Graphics.DrawString(handle.HandleNumber.ToString(), font1, Brushes.White, handleRect, stringFormat);
                }
            }

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnClick(e);

            foreach (var handle in Handles)
            {
                Rectangle handleRect = handle.GetHandleRectangle(ClientRectangle, halfHandleSizeX, halfHandleSizeY);

                if (handleRect.Contains(e.Location))
                {
                    currentHandle = handle.HandleNumber;
                    UpdateHueFromMouse(e);
                    break;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (currentHandle != -1)
            {
                UpdateHueFromMouse(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            currentHandle = -1;
        }
        #endregion

        private void UpdateHueFromMouse(EventArgs e)
        {
            var mouseEv = (MouseEventArgs)(e);
            System.Windows.Vector v = new System.Windows.Vector((mouseEv.X - Size.Width / 2) * Size.Height, (mouseEv.Y - Size.Height / 2) * Size.Width);

            System.Windows.Vector up = new System.Windows.Vector(0, -1);
            Handles.Single(x => x.HandleNumber == currentHandle).Hue = (System.Windows.Vector.AngleBetween(up, v) + 360) % 360;

            SetDependentHues(currentHandle);
            Invalidate();
            HuesChanged(null, null);
        }

        private void SetDependentHues(int fromHandleNumber)
        {
            switch(HandleCount)
            {
                case 1:
                    break;
                case 2:
                    if (!RangeRequired)
                    {
                        var _otherHandle = Handles.Single(x => x.HandleNumber != fromHandleNumber);
                        var _thisHandleHue = Handles.Single(x => x.HandleNumber == fromHandleNumber).Hue;
                        _otherHandle.Hue = (180 + _thisHandleHue) % 360;
                    }
                    break;
                case 3:
                    var _thisHandleHue2 = Handles.Single(x => x.HandleNumber == fromHandleNumber).Hue;
                    int ctr = 1;
                    foreach (var _otherHandle2 in Handles.Where(x => x.HandleNumber != fromHandleNumber)) 
                    {
                        _otherHandle2.Hue = (120 * ctr + _thisHandleHue2) % 360;
                        ctr++;
                    }
                    break;
                default:
                    break;
            }
        }

        private int halfHandleSizeX { get { return this.Size.Width / 24; } }
        private int halfHandleSizeY { get { return this.Size.Height / 24; } }
    }

    public class HueSelectorHandle
    {
        public int HandleNumber { get; set; }

        private double _hue = 0;
        public double Hue { get { return _hue; } set { _hue = value; } }

        public Rectangle GetHandleRectangle(Rectangle ClientRectangle, int halfHandleSizeX, int halfHandleSizeY)
        {
            Point controlCentre = GetControlCentre(ClientRectangle);
            Rectangle handleRect = new Rectangle(controlCentre.X - halfHandleSizeX,
                controlCentre.Y - halfHandleSizeY,
                halfHandleSizeX * 2,
                halfHandleSizeY * 2);

            handleRect.X = (int)(handleRect.X + Math.Sin(Hue * Math.PI / 180) * ClientRectangle.Width * .375);
            handleRect.Y = (int)(handleRect.Y - Math.Cos(Hue * Math.PI / 180) * ClientRectangle.Height * .375);

            return handleRect;
        }

        public HueSelectorHandle(int handleNumber)
        {
            HandleNumber = handleNumber;
            Hue = handleNumber * 45;
        }
        public Point GetControlCentre(Rectangle ClientRectangle)
        {
            return new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
        }
    }
}