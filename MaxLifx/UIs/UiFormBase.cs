using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MaxLifx.Processors.ProcessorSettings;

namespace MaxLifx.UIs
{
    public class UiFormBase : Form
    {
        public List<string> SelectedLabels { get; set; } = new List<string>();

        public void SetupLabels(ListBox lbLabels, List<string> labels, ISettings settings)
        {
            if (labels != null)
            {
                lbLabels.Items.Clear();

                foreach (var label in labels.OrderBy(x => x))
                    lbLabels.Items.Add(label);
            }
            else lbLabels.SelectedItems.Clear();

            for (var i = 0; i < lbLabels.Items.Count; i++)
            {
                if (settings.SelectedLabels.Contains(lbLabels.Items[i].ToString()))
                {
                    lbLabels.SelectedItems.Add(lbLabels.Items[i]);
                }
            }
        }
    }
}