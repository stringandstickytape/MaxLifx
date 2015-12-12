using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColourControl
{
    public partial class ColourControl: UserControl
    {
        public ColourControl()
        {
            InitializeComponent();
            hueSelector1.HuesChanged += HueSelector1_HuesChanged;
        }

        private void HueSelector1_HuesChanged(object sender, EventArgs e)
        {
            HuesChanged(null, null);
        }

        [Category("Action"), Description("Fires when hues change.")]
        public event EventHandler HuesChanged;

        public List<int> GetHues() { return hueSelector1.GetHues(); }
        public void SetHues(List<int> hues) { hueSelector1.SetHues(hues); Invalidate(); }

        public bool Invert { get { return hueSelector1.Invert; } set { hueSelector1.Invert = value; } }
        [Category("Behavior"), Description("Determines how many handles there are")]
        public int HandleCount { get { return hueSelector1.HandleCount; } set { hueSelector1.HandleCount = value; } }
        [Category("Behavior"), Description("Determines whether a range is required")]
        public bool RangeRequired { get { return hueSelector1.RangeRequired; } set { hueSelector1.RangeRequired = value; } }
    }
}
