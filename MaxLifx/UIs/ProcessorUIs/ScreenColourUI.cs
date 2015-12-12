using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.Threads;

namespace MaxLifx.UIs
{
    public partial class ScreenColourUI : UiFormBase
    {
        private readonly ScreenColourSettings Settings;
        private Form2 f;

        public ScreenColourUI(ScreenColourSettings settings)
        {
            InitializeComponent();
            SuspendUI = true;

            Settings = settings;
            SetPositionTextBoxesFromSettings();
            SetupLabels(lbLabels, Settings.LabelsAndLocations.Select(x => x.Label).ToList(), Settings);
            cbConfigs.Items.Clear();
            foreach (var x in Directory.GetFiles(".", "*" + Settings.FileExtension))
            {
                var fileName = x.Replace(".\\", "").Replace("." + Settings.FileExtension, "").Replace(".xml", "");
                cbConfigs.Items.Add(fileName);
            }
            SuspendUI = false;
        }

        private bool SuspendUI { get; set; }

        private void button5_Click(object sender, EventArgs e)
        {
            if (f != null) return;
            f = new Form2();
            f.Opacity = .5;
            f.ResizeEnd += F_ResizeEnd;
            f.Move += F_ResizeEnd;
            f.FormClosed += F_FormClosed;
            f.Show();
        }

        private void F_FormClosed(object sender, FormClosedEventArgs e)
        {
            f.Dispose();
            f = null;
        }

        private void F_ResizeEnd(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.Left) return;

            SuspendUI = true;
            Settings.TopLeft = new Point(f.Location.X + 7, f.Location.Y);
            Settings.BottomRight = new Point(f.Location.X + f.Size.Width - 6, f.Location.Y + f.Size.Height - 6);
            SetPositionTextBoxesFromSettings();
            SuspendUI = false;

            ProcessorBase.SaveSettings(Settings, null);
        }

        private void SetPositionTextBoxesFromSettings()
        {
            tlx.Text = Settings.TopLeft.X.ToString();
            tly.Text = Settings.TopLeft.Y.ToString();
            brx.Text = Settings.BottomRight.X.ToString();
            bry.Text = Settings.BottomRight.Y.ToString();
            fade.Text = Settings.Fade.ToString();
            brightness.Text = Settings.Brightness.ToString();
            saturation.Text = Settings.Saturation.ToString();
            delay.Text = Settings.Delay.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new AssignAreaToBulbForm(Settings.LabelsAndLocations);
            f.ShowDialog();

            Settings.LabelsAndLocations = f.LabelsAndLocations;
            ProcessorBase.SaveSettings(Settings, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var t = new Thread(() =>
            {
                var s = new SaveFileDialog {DefaultExt = ".maxlifx.ScreenColourSettings.xml"};
                s.Filter = "XML files (*.maxlifx.ScreenColourSettings.xml)|*.maxlifx.ScreenColourSettings.xml";
                s.AddExtension = true;

                var result = s.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ProcessorBase.SaveSettings(Settings, s.FileName);
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void pos_TextChanged(object sender, EventArgs e)
        {
            if (SuspendUI) return;

            SuspendUI = true;

            int tlxval = 0,
                tlyval = 0,
                brxval = 0,
                bryval = 0,
                fadeval = 400,
                delayval = 50,
                brightval = 32767,
                satval = 32767;

            int.TryParse(tlx.Text, out tlxval);
            int.TryParse(tly.Text, out tlyval);
            int.TryParse(brx.Text, out brxval);
            int.TryParse(bry.Text, out bryval);
            int.TryParse(fade.Text, out fadeval);
            int.TryParse(delay.Text, out delayval);
            int.TryParse(saturation.Text, out satval);
            int.TryParse(brightness.Text, out brightval);

            Settings.TopLeft = new Point(tlxval, tlyval);
            Settings.BottomRight = new Point(brxval, bryval);
            Settings.Fade = fadeval;
            Settings.Delay = delayval;
            Settings.Saturation = satval;
            Settings.Brightness = brightval;

            SuspendUI = false;

            ProcessorBase.SaveSettings(Settings, null);
        }

        private void lbLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SuspendUI)
            {
                var selectedLabels = new List<string>();

                foreach (var q in lbLabels.SelectedItems)
                    selectedLabels.Add(q.ToString());

                Settings.SelectedLabels = selectedLabels;
            }
        }

        private void cbConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = new ScreenColourSettings();
            ProcessorBase.LoadSettings(ref s, cbConfigs.SelectedItem + "." + Settings.FileExtension);

            Settings.LabelsAndLocations = s.LabelsAndLocations;
            Settings.BottomRight = s.BottomRight;
            Settings.Brightness = s.Brightness;
            Settings.Delay = s.Delay;
            Settings.Fade = s.Fade;
            Settings.Saturation = s.Saturation;
            Settings.TopLeft = s.TopLeft;
            Settings.SelectedLabels = s.SelectedLabels;

            SuspendUI = true;
            SetupLabels(lbLabels, null, Settings);
            SetPositionTextBoxesFromSettings();
            SuspendUI = false;
        }
    }

    public class CustomPaintTrackBar : TrackBar
    {
        public CustomPaintTrackBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        public event PaintEventHandler PaintOver;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // WM_PAINT
            if (m.Msg == 0x0F)
            {
                using (var lgGraphics = Graphics.FromHwndInternal(m.HWnd))
                    OnPaintOver(new PaintEventArgs(lgGraphics, ClientRectangle));
            }
        }

        protected virtual void OnPaintOver(PaintEventArgs e)
        {
            if (PaintOver != null)
                PaintOver(this, e);

            var myPen = new Pen(Color.Aqua);

            var newSize = new Size((int) (Size.Width*.9/6), (int) (Size.Height*.15));

            var colors = new Color[7]
            {Color.Red, Color.Yellow, Color.Green, Color.Cyan, Color.Blue, Color.Magenta, Color.Red};

            Brush b;
            for (var i = 0; i < 6; i++)
            {
                var newLocation = Location + new Size((int) (Size.Width*.9/7*(i + .8)), (int) (Size.Height*.8));
                b =
                    new LinearGradientBrush(
                        Location + new Size((int) (Size.Width*.9/7*(i + .8)) - 1, (int) (Size.Height*.8)),
                        new Point((int) (newLocation.X + Size.Width*.9/6), newLocation.Y), colors[i], colors[i + 1]);
                e.Graphics.FillRectangle(b, new Rectangle(newLocation, newSize));
            }
        }
    }
}