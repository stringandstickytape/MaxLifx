using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MaxLifx.Controls;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.SoundToken;
using MaxLifx.Threads;
using NAudio.Wave;

namespace MaxLifx.UIs
{
    public partial class SoundResponseUI : UiFormBase
    {
        private readonly SoundResponseSettings _settings;
        private bool _suspendUi;

        public SoundResponseUI(SoundResponseSettings settings, List<string> labels)
        {
            InitializeComponent();
            _settings = settings;
            _suspendUi = true;
            SetupLabels(lbLabels, labels, _settings);
            spectrumAnalyser1.AddHandle();
            SetupUI();
            _suspendUi = false;
            Load += SoundResponseUI_Load;
            spectrumAnalyser1.SelectionChanged += SpectrumAnalyser1_SelectionChanged;
            
        }

        private void SpectrumAnalyser1_SelectionChanged(object sender, EventArgs e)
        {
            var handle = ((SpectrumAnalyserHandle) sender);
            _settings.Bins[handle.Number] = handle.Bin;
            _settings.Levels[handle.Number] = handle.Level;
            _settings.LevelRanges[handle.Number] = handle.LevelRange;
        }

        private void SoundResponseUI_Load(object sender, EventArgs e)
        {
            spectrumAnalyser1.StartCapture();
            hueSelector1.SetHuesAndSaturations(_settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges);
        }

        private void SetupUI()
        {
            
            tbMinBrightness.Value = _settings.MinBrightness;
            tbMaxBrightness.Value = _settings.MaxBrightness;

            cbWaveType.Items.Clear();
            foreach (var s in Enum.GetNames(typeof (WaveTypes)))
            {
                cbWaveType.Items.Add(s);
            }

            foreach (var item in cbWaveType.Items)
            {
                if (item.ToString() == _settings.WaveType.ToString())
                {
                    cbWaveType.SelectedItem = item;
                    break;
                }
            }

            nDelay.Value = _settings.Delay;
            nTransition.Value = _settings.TransitionDuration;
            nWaveDuration.Value = _settings.WaveDuration;

            cbConfigs.Items.Clear();
            foreach (var x in Directory.GetFiles(".", "*." + _settings.FileExtension))
            {
                var fileName = x.Replace(".\\", "").Replace("." + _settings.FileExtension, "").Replace(".xml", "");
                cbConfigs.Items.Add(fileName);
            }

            UpdateHueSelectorHandleCount();
            hueSelector1.SetHuesAndSaturations(_settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges);
            cbPerBulb.Checked = _settings.PerBulb;

            UpdateHueSelectorFromHuesAndSaturations();
            hueSelector1.LinkRanges = _settings.LinkRanges;

            cbHueInvert.Checked = _settings.HueInvert;
            cbBrightnessInvert.Checked = _settings.BrightnessInvert;
            cbSaturationInvert.Checked = _settings.SaturationInvert;
            
            cbLinkRanges.Checked = _settings.LinkRanges;



            List<int> b, l, lr;

            spectrumAnalyser1.GetHandles(out b,out l,out lr);

            _settings.Bins = b;
            _settings.Levels = l;
            _settings.LevelRanges = lr;

            tbOnTimes.Text = _settings.OnTimes;
            tbOffTimes.Text = _settings.OffTimes;
        }

        private void tbMinBrightness_ValueChanged(object sender, EventArgs e)
        {
            _settings.MinBrightness = (ushort) (tbMinBrightness.Value);
            if (_settings.MaxBrightness < _settings.MinBrightness)
            {
                tbMaxBrightness.Value = tbMinBrightness.Value;
                _settings.MaxBrightness = _settings.MinBrightness;
            }

            hueSelector1.Brightness = (double) (_settings.MinBrightness + _settings.MaxBrightness)/(65536*2);
        }

        private void tbMaxBrightness_ValueChanged(object sender, EventArgs e)
        {
            _settings.MaxBrightness = (ushort) (tbMaxBrightness.Value);
            if (_settings.MaxBrightness < _settings.MinBrightness)
            {
                tbMinBrightness.Value = tbMaxBrightness.Value;
                _settings.MinBrightness = _settings.MaxBrightness;
            }
        }

        private void lbLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!_suspendUi && !cbReorder.Checked)
                UpdateSelectedLabels();

        }

        private void UpdateSelectedLabels()
        {
            if (!_suspendUi)
            {
                var selectedLabels = new List<string>();

                foreach (var q in lbLabels.SelectedItems)
                    selectedLabels.Add(q.ToString());

                _settings.SelectedLabels = selectedLabels;
            }

            UpdateHueSelectorHandleCount();
            UpdateHueSelectorFromHuesAndSaturations();
        }

        private void UpdateHueSelectorHandleCount()
        {
            //int count;
            if (_settings.PerBulb)
                hueSelector1.HandleCount = _settings.SelectedLabels.Count();

            var count = (_settings.PerBulb ? _settings.SelectedLabels.Count() : 1) - _settings.Bins.Count();
            if (count > 0)
                for (int i = 0; i < count; i++)
                {
                    _settings.Bins.Add(50);
                    _settings.Levels.Add(100);
                    _settings.LevelRanges.Add(25);
                }
            else
            if (count < 0)
                for (int i = count; i < 0; i++)
                {
                    _settings.Bins.RemoveAt(_settings.Bins.Count()-1);
                    _settings.Levels.RemoveAt(_settings.Levels.Count() - 1);
                    _settings.LevelRanges.RemoveAt(_settings.LevelRanges.Count() - 1);
                }

            spectrumAnalyser1.SetupHandles(_settings.Bins, _settings.Levels, _settings.LevelRanges);
        }

        private void cbWaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.WaveType = (WaveTypes) Enum.Parse(typeof (WaveTypes), cbWaveType.SelectedItem.ToString());
        }

        private void nDelay_ValueChanged(object sender, EventArgs e)
        {
            _settings.Delay = (int) (nDelay.Value);
        }

        private void nTransition_ValueChanged(object sender, EventArgs e)
        {
            _settings.TransitionDuration = (int) (nTransition.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _settings.WaveStartTime = DateTime.Now;
        }

        private void nWaveDuration_Changed(object sender, EventArgs e)
        {
            _settings.WaveDuration = (int) nWaveDuration.Value;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            var t = new Thread(() =>
            {
                var s = new SaveFileDialog {DefaultExt = _settings.FileExtension};
                s.Filter = "XML files (*." + _settings.FileExtension + ")|*." + _settings.FileExtension;
                s.InitialDirectory = Directory.GetCurrentDirectory();
                s.AddExtension = true;

                var result = s.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ProcessorBase.SaveSettings(_settings, s.FileName);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void cbConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = new SoundResponseSettings();
            ProcessorBase.LoadSettings(ref s, cbConfigs.SelectedItem + "." + _settings.FileExtension);

            _suspendUi = true;
            _settings.WaveType = s.WaveType;
            _settings.WaveDuration = s.WaveDuration;
            _settings.SelectedLabels = s.SelectedLabels;
            _settings.Delay = s.Delay;
            _settings.Kelvin = s.Kelvin;
            _settings.MaxBrightness = s.MaxBrightness;
            _settings.MinBrightness = s.MinBrightness;
            _settings.Hues = s.Hues;
            _settings.HueRanges = s.HueRanges;
            _settings.Saturations = s.Saturations;
            _settings.SaturationRanges = s.SaturationRanges;
            _settings.TransitionDuration = s.TransitionDuration;
            _settings.BrightnessInvert = s.BrightnessInvert;
            _settings.SaturationInvert = s.SaturationInvert;
            _settings.HueInvert = s.HueInvert;
            _settings.LinkRanges = s.LinkRanges;
            _settings.OnTimes = s.OnTimes;
            _settings.OffTimes = s.OffTimes;
            _settings.PerBulb = s.PerBulb;
            _settings.Bins = s.Bins;
            _settings.Levels = s.Levels;
            _settings.LevelRanges = s.LevelRanges;

            SetupLabels(lbLabels, null, _settings);
            SetupUI();
            _suspendUi = false;
        }

        private void cbBrightnessInvert_CheckedChanged(object sender, EventArgs e)
        {
            _settings.BrightnessInvert = cbBrightnessInvert.Checked;
        }

        private void cbSaturationInvert_CheckedChanged(object sender, EventArgs e)
        {
            _settings.SaturationInvert = cbSaturationInvert.Checked;
        }

        private void cbHueInvert_CheckedChanged(object sender, EventArgs e)
        {
            _settings.HueInvert = cbHueInvert.Checked;
        }

        private void tbOffTimes_TextChanged(object sender, EventArgs e)
        {
            _settings.OffTimes = tbOffTimes.Text;
        }

        private void tbOnTimes_TextChanged(object sender, EventArgs e)
        {
            _settings.OnTimes = tbOnTimes.Text;
        }

        private void colourControl1_HuesChanged(object sender, EventArgs e)
        {
            if (!_suspendUi)
            {
                UpdateHuesFromHueSelector();
            }
        }

        private void UpdateHuesFromHueSelector()
        {
            List<int> hues, hueRanges;
            List<double> saturations, saturationRanges;
            hueSelector1.GetHues(out hues, out hueRanges, out saturations, out saturationRanges);

            _settings.Hues = hues;
            _settings.HueRanges = hueRanges;
            _settings.Saturations = saturations;
            _settings.SaturationRanges = saturationRanges;
        }

        private void UpdateHueSelectorFromHuesAndSaturations()
        {
            hueSelector1.SetHuesAndSaturations(_settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges);
        }

        private void cbPerBulb_CheckedChanged(object sender, EventArgs e)
        {
            _settings.PerBulb = cbPerBulb.Checked;
            hueSelector1.PerBulb = _settings.PerBulb;
            UpdateHueSelectorHandleCount();
            UpdateHuesFromHueSelector();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hueSelector1.ResetRanges();
        }

        private void cbLinkRanges_CheckedChanged(object sender, EventArgs e)
        {
            _settings.LinkRanges = cbLinkRanges.Checked;
            hueSelector1.LinkRanges = _settings.LinkRanges;
        }

        private void cbFree_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Free = cbFree.Checked;
            hueSelector1.Free = _settings.Free;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _settings.LevelRanges = spectrumAnalyser1.ResetRanges();
        }

        private void bUp_Click(object sender, EventArgs e)
        {
            MoveItem(true);
        }

        private void MoveItem(bool Up)
        {
            foreach (var index in lbLabels.SelectedIndices)
            {
                if (Up && (int)index == 0) continue;
                if (!Up && (int)index == lbLabels.Items.Count) continue;

                int newIndex = (int)index - (Up ? 1 : -1);

                if (newIndex < 0 || newIndex >= lbLabels.Items.Count)
                    return; 

                object selected = lbLabels.Items[(int)index];

                _suspendUi = true;
                lbLabels.Items.Remove(selected);
                lbLabels.Items.Insert(newIndex, selected);
                lbLabels.SetSelected(newIndex, true);
                _suspendUi = false;
            }
        }

        private void bDown_Click(object sender, EventArgs e)
        {
            MoveItem(false);
        }

        private List<object> _oldSelectedLabels = new List<object>();
        private void cbReorder_CheckedChanged(object sender, EventArgs e)
        {
            if (cbReorder.Checked)
            {
                bUp.Enabled = true;
                bDown.Enabled = true;
                _oldSelectedLabels = new List<object>();
                foreach (var y in lbLabels.SelectedItems)
                {
                    _oldSelectedLabels.Add(y);
                }
                lbLabels.ClearSelected();
                lbLabels.SelectionMode = SelectionMode.One;
            }
            else
            {
                bUp.Enabled = false;
                bDown.Enabled = false;
                List<object> newSelectedLabels = new List<object>();
                foreach (var y in _oldSelectedLabels)
                {
                    newSelectedLabels.Add(y);
                }
                _suspendUi = true;
                lbLabels.ClearSelected();
                lbLabels.SelectionMode = SelectionMode.MultiSimple;
                foreach (var z in newSelectedLabels)
                    lbLabels.SelectedItems.Add(z);
                _suspendUi = false;
                UpdateSelectedLabels();

            }
        }

        private void cbUpdateAudioResponse_CheckedChanged(object sender, EventArgs e)
        {
            spectrumAnalyser1.ShowUpdates = cbUpdateAudioResponse.Checked;
        }
    }
}