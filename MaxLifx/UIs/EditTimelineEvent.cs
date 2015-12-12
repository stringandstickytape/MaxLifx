using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MaxLifx.Controls;

namespace MaxLifx.UIs
{
    public partial class EditTimelineEvent : Form
    {
        public TimelineEvent EditEvent;

        public EditTimelineEvent(TimelineEvent eventToEdit, bool editMultiple)
        {
            InitializeComponent();

            if (editMultiple) tbTime.Visible = false;

            EditEvent = eventToEdit;
            tbParameter.Text = eventToEdit.Parameter;
            tbTime.Text = (eventToEdit.Time/1000).ToString();

            int ctr = 0;
            int selectItem = -1;
            foreach (var v in Enum.GetNames(typeof (TimelineEventAction)).OrderBy(x => x))
            {
                cbEventType.Items.Add(v);
                if (v == eventToEdit.Action.ToString())
                    selectItem = ctr;

                ctr++;
            }

            if (selectItem > -1)
                cbEventType.SelectedIndex = selectItem;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbTime_TextChanged(object sender, EventArgs e)
        {
            float ms;
            if (float.TryParse(tbTime.Text, out ms))
                EditEvent.Time = (long)(ms*1000);
        }

        private void cbEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimelineEventAction s;
            if (Enum.TryParse(cbEventType.SelectedItem.ToString(), out s))
            {
                EditEvent.Action = s;
            }
        }

        private void tbParameter_TextChanged(object sender, EventArgs e)
        {
            EditEvent.Parameter = tbParameter.Text;
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            if (EditEvent.Action == TimelineEventAction.Unspecified)
            {
                MessageBox.Show("Choose an action first.");
                return;
            }

            var s = new OpenFileDialog();
            switch (EditEvent.Action)
            {
                case TimelineEventAction.PlayMp3:
                    s.DefaultExt = ".mp3";
                    s.Filter = "MP3 files (*.mp3)|*.mp3";
                    break;
                case TimelineEventAction.StartThreadSet:
                    s.DefaultExt = ".MaxLifx.Threadset.xml";
                    s.Filter = "XML files (*.MaxLifx.Threadset.xml)|*.MaxLifx.Threadset.xml";
                    break;
            }

            s.InitialDirectory = Directory.GetCurrentDirectory();
            s.AddExtension = true;

            if (s.ShowDialog() == DialogResult.OK)
            {
                tbParameter.Text = s.FileName;
            }
        }
    }
}
