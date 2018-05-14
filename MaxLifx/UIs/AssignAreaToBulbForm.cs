using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MaxLifx.Controllers;

namespace MaxLifx
{
    public partial class AssignAreaToBulbForm : Form
    {
        public List<LabelAndLocationType> LabelsAndLocations;
        public LabelAndLocationType SelectedLabelAndLocation;
        private bool _suspendUi;

        public AssignAreaToBulbForm(List<LabelAndLocationType> labelsAndLocations)
        {
            LabelsAndLocations = labelsAndLocations;
            InitializeComponent();
            foreach (var l in LabelsAndLocations.OrderBy(x => x.Label))
                lbBulbs.Items.Add(l.Label);

            foreach (var v in Enum.GetNames(typeof (ScreenLocation)).OrderBy(x => x))
                cbArea.Items.Add(v);

            var panel = panelScreenLocations;

            var pbHt = panel.Height / (float)ScreenColourProcessor.CaptureResolution.Height;
            var pbWd = panel.Width / (float)ScreenColourProcessor.CaptureResolution.Width;

            for (var x = 0; x < ScreenColourProcessor.CaptureResolution.Width; x++)
                for (var y = 0; y < ScreenColourProcessor.CaptureResolution.Height; y++)
                {
                    var pb = new PictureBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White, Tag = $"{x+y * ScreenColourProcessor.CaptureResolution.Width}", Location = new Point((int)(x * pbWd), (int)(y * pbHt)), Size = new Size((int)pbWd, (int)pbHt) };
                    pb.Click += Pb_Click;
                    panel.Controls.Add(pb);
                }

        }

        private void Pb_Click(object sender, EventArgs e)
        {
            if (lbBulbs.SelectedItem == null) return;

            SelectedLabelAndLocation = LabelsAndLocations.SingleOrDefault(x => x.Label == (lbBulbs.SelectedItem.ToString()));
            if (SelectedLabelAndLocation == null) return;


            var pb = ((PictureBox)sender);
            if (SelectedLabelAndLocation.SelectedPixels.Contains(int.Parse((string)pb.Tag)))
            {
                SelectedLabelAndLocation.SelectedPixels.Remove(int.Parse((string)pb.Tag));
                pb.BackColor = Color.White;
            }
            else
            {
                SelectedLabelAndLocation.SelectedPixels.Add(int.Parse((string)pb.Tag));
                pb.BackColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbBulbs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suspendUi) return;
            _suspendUi = true;
            if (lbBulbs.SelectedItem == null)
            {
                cbArea.Enabled = false;
                return;
            }

            SelectedLabelAndLocation =
                LabelsAndLocations.Single(x => x.Label == (((ListBox) sender).SelectedItem.ToString()));

            foreach (var v in cbArea.Items)
                if (v.ToString() == Enum.GetName(typeof (ScreenLocation), SelectedLabelAndLocation.ScreenLocation))
                    cbArea.SelectedItem = v;

            cbArea.Enabled = true;

            RefreshGrid();

            _suspendUi = false;
        }

        private void RefreshGrid()
        {
            if (lbBulbs.SelectedItem == null) return;

            SelectedLabelAndLocation = LabelsAndLocations.SingleOrDefault(x => x.Label == (lbBulbs.SelectedItem.ToString()));
            if (SelectedLabelAndLocation == null) return;

            var panel = panelScreenLocations;
            foreach(var pb in panel.Controls.OfType<PictureBox>())
            {
                if (SelectedLabelAndLocation.SelectedPixels.Contains(int.Parse((string)pb.Tag)))
                    pb.BackColor = Color.Red;
                else pb.BackColor = Color.White;
            }
        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suspendUi) return;
            _suspendUi = true;

            LabelsAndLocations.Remove(LabelsAndLocations.Single(x => x.Label == SelectedLabelAndLocation.Label));
            var l = new LabelAndLocationType();
            l.Label = SelectedLabelAndLocation.Label;
            l.ScreenLocation =
                (ScreenLocation) (Enum.Parse(typeof (ScreenLocation), ((ComboBox) sender).SelectedItem.ToString()));
            LabelsAndLocations.Add(l);
            _suspendUi = false;
        }
    }
}