using System;
using System.Windows.Forms;
using MaxLifx.Controllers;

namespace MaxLifx.UIs
{
    public partial class FakeBulb : Form
    {
        public string Label;

        public FakeBulb(MaxLifxBulbController bulbController, string label)
        {
            InitializeComponent();
            Label = label;
            bulbController.ColourSet += SetColour;
        }

        public void SetColour(object sender, EventArgs e)
        {
            LabelAndColourPayload details = ((LabelAndColourPayload) sender);

            if(details.Label == Label)
                BackColor = Utils.HsbToRgb(details.Payload.Hue, details.Payload.Saturation/65535.0f,
                    details.Payload.Brightness/65535.0f);
        }

        private void FakeBulb_DoubleClick(object sender, EventArgs e)
        {
            if(FormBorderStyle == FormBorderStyle.Sizable)
                FormBorderStyle = FormBorderStyle.None;
            else FormBorderStyle = FormBorderStyle.Sizable;
        }
    }
}
