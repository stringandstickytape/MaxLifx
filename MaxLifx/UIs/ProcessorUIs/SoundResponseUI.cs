using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MaxLifx.ColourThemes;
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
        private Random r;

        public SoundResponseUI(SoundResponseSettings settings, List<string> labels, Random R)
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
            r = R;

            pThemes.Controls.Clear();
            var type = typeof(IColourTheme);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.ToString().EndsWith("Base"));

            int yCtr = 0;

            foreach (var t in types.OrderBy(x => x.Name)) 
            {
                var newButtonName = Regex.Replace(t.Name.Replace("ColourTheme", ""), "([a-z])([A-Z])", "$1 $2");
                var newButton = new Button {Text = newButtonName, Location = new Point(0, yCtr), Size = new Size(pThemes.Size.Width - 20,23), Tag = t};
                newButton.Click += ColourThemeClick;
                pThemes.Controls.Add(newButton);
                yCtr += newButton.Size.Height + 5;
            }

            hueSelector1.Free = _settings.Free;
            brightnessSelector1.Free = _settings.Free;
            hueSelector1.PerBulb = _settings.PerBulb;
            brightnessSelector1.PerBulb = _settings.PerBulb;
            UpdateHueSelectorHandleCount();
            UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
        }

        private void ColourThemeClick(object sender, EventArgs eventArgs)
        {
            var button = ((Button) (sender));
            var type = (Type) (button.Tag);
            var colourTheme = (IColourTheme) Activator.CreateInstance(type);

            colourTheme.SetColours(r, _settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges, _settings.Brightnesses, _settings.BrightnessRanges,
                cbPastelThemes.Checked, cbLockBrightness.Checked);

            for (int index = 0; index < _settings.Brightnesses.Count; index++)
            {
                if(_settings.Brightnesses[index] + _settings.BrightnessRanges[index] > 1)
                    _settings.BrightnessRanges[index] = 1 - _settings.Brightnesses[index];
                if (_settings.Brightnesses[index] - _settings.BrightnessRanges[index] < 0)
                    _settings.BrightnessRanges[index] = _settings.Brightnesses[index];
            }

            for (int index = 0; index < _settings.Saturations.Count; index++)
            {
                if (_settings.Saturations[index] + _settings.SaturationRanges[index] > 1)
                    _settings.SaturationRanges[index] = 1 - _settings.Saturations[index];
                if (_settings.Saturations[index] - _settings.SaturationRanges[index] < 0)
                    _settings.SaturationRanges[index] = _settings.Saturations[index];
            }

            hueSelector1.SetHuesAndSaturations(_settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges);
            brightnessSelector1.SetBrightnesses(_settings.Brightnesses, _settings.BrightnessRanges);
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
            brightnessSelector1.SetBrightnesses(_settings.Brightnesses, _settings.BrightnessRanges);
        }

        private void SetupUI()
        {
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

            //cbConfigs.Items.Clear();
            //foreach (var x in Directory.GetFiles(".", "*." + _settings.FileExtension))
            //{
            //    var fileName = x.Replace(".\\", "").Replace("." + _settings.FileExtension, "").Replace(".xml", "");
            //    cbConfigs.Items.Add(fileName);
            //}

            UpdateHueSelectorHandleCount();
            hueSelector1.SetHuesAndSaturations(_settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges);
            brightnessSelector1.SetBrightnesses(_settings.Brightnesses, _settings.BrightnessRanges);
            cbPerBulb.Checked = _settings.PerBulb;

            UpdateHueSelectorFromHuesAndSaturationsAndBrightnessSelectorFromBrightnesses();
            hueSelector1.LinkRanges = _settings.LinkRanges;
            brightnessSelector1.LinkRanges = _settings.LinkRanges;

            cbHueInvert.Checked = _settings.HueInvert;
            cbBrightnessInvert.Checked = _settings.BrightnessInvert;
            cbSaturationInvert.Checked = _settings.SaturationInvert;
            
            cbLinkRanges.Checked = _settings.LinkRanges;



            List<int> b, l;
            List<byte> lr;

            spectrumAnalyser1.GetHandles(out b,out l,out lr);

            _settings.Bins = b;
            _settings.Levels = l;
            _settings.LevelRanges = lr;

            tbOnTimes.Text = _settings.OnTimes;
            tbOffTimes.Text = _settings.OffTimes;
        }

        List<int> previouslySelectedIndices = new List<int>();
        int? lastSelectedIndex;

        private void lbLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suspendUi) return;

            var currentlySelectedIndices = lbLabels.SelectedIndices.Cast<int>().ToList();

            var diff = currentlySelectedIndices.Except(previouslySelectedIndices);

            if (Control.ModifierKeys == Keys.Shift && lastSelectedIndex.HasValue && diff.Count() == 1)
            {
                for(int i = lastSelectedIndex.Value+1; i < diff.First(); i++)
                {
                    if (!lbLabels.SelectedIndices.Contains(i))
                    {
                        var oldSuspend = _suspendUi;
                        _suspendUi = true;
                        lbLabels.SelectedIndices.Add(i);
                        _suspendUi = oldSuspend;
                    }
                }
            }

            if (diff.Count() == 1)
            {
                lastSelectedIndex = diff.First();
                System.Diagnostics.Debug.WriteLine("Index is " + lastSelectedIndex);
            }
           // else lastSelectedIndex = null;

            previouslySelectedIndices = lbLabels.SelectedIndices.Cast<int>().ToList();

            if (!_suspendUi && !cbReorder.Checked)
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
            UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
            UpdateHueSelectorFromHuesAndSaturationsAndBrightnessSelectorFromBrightnesses();
        }

        private void UpdateHueSelectorHandleCount()
        {
            //int count;
            if (_settings.PerBulb)
            {
                hueSelector1.HandleCount = _settings.SelectedLabels.Count();
                brightnessSelector1.HandleCount = _settings.SelectedLabels.Count();
            }

            var count = (_settings.PerBulb ? _settings.SelectedLabels.Count() : 1) - _settings.Bins.Count();
            if (count > 0)
                for (int i = 0; i < count; i++)
                {
                    var bin = (_settings.Bins.Count - i)*(80) + 16;
                    if (bin > 512) bin = 512;
                    _settings.Bins.Add(bin);
                    _settings.Levels.Add((110 + (int)(((float)-1 / (_settings.SelectedLabels.Count - i)) * 60)));
                    _settings.LevelRanges.Add(45);
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
                UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
            }
        }

        private void BrightnessesChanged(object sender, EventArgs eventArgs)
        {
            if (!_suspendUi)
            {
                UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
            }
        }

        private void UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector()
        {
            List<int> hues, hueRanges;
            List<double> saturations, saturationRanges;
            List<float> brightnesses, brightnessRanges;
            hueSelector1.GetHues(out hues, out hueRanges, out saturations, out saturationRanges);
            brightnessSelector1.GetBrightnesses(out brightnesses, out brightnessRanges);

            _settings.Hues = hues;
            _settings.HueRanges = hueRanges;
            _settings.Saturations = saturations;
            _settings.SaturationRanges = saturationRanges;
            _settings.Brightnesses = brightnesses;
            _settings.BrightnessRanges = brightnessRanges;

        }

        private void UpdateHueSelectorFromHuesAndSaturationsAndBrightnessSelectorFromBrightnesses()
        {
            hueSelector1.SetHuesAndSaturations(_settings.Hues, _settings.HueRanges, _settings.Saturations,
                _settings.SaturationRanges);

            brightnessSelector1.SetBrightnesses(_settings.Brightnesses, _settings.BrightnessRanges);
        }

        private void cbPerBulb_CheckedChanged(object sender, EventArgs e)
        {
            _settings.PerBulb = cbPerBulb.Checked;
            hueSelector1.PerBulb = _settings.PerBulb;
            brightnessSelector1.PerBulb = _settings.PerBulb;
            UpdateHueSelectorHandleCount();
            UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hueSelector1.ResetRanges();
            brightnessSelector1.ResetRanges();
            UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
        }

        private void cbLinkRanges_CheckedChanged(object sender, EventArgs e)
        {
            _settings.LinkRanges = cbLinkRanges.Checked;
            hueSelector1.LinkRanges = _settings.LinkRanges;
            brightnessSelector1.LinkRanges = _settings.LinkRanges;
        }

        private void cbFree_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Free = !cbFree.Checked;
            hueSelector1.Free = _settings.Free;
            brightnessSelector1.Free = _settings.Free;

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

        private void button4_Click_1(object sender, EventArgs e)
        {
            hueSelector1.ResetRanges(0,0);
            brightnessSelector1.ResetRanges(0);
            UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
        }

        private void SoundResponseUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            spectrumAnalyser1.StopCapture();
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            panel1.VerticalScroll.Enabled = true;
        }

        private void SoundResponseUI_ResizeEnd(object sender, EventArgs e)
        {
            panel1.VerticalScroll.Enabled = true;
        }

        private void btnCentreRanges_Click(object sender, EventArgs e)
        {
            brightnessSelector1.Reset();
            UpdateHuesFromHueSelectorAndBrightnessesFromBrightnessSelector();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var tuple = spectrumAnalyser1.Redistribute();
            _settings.Bins = tuple.Item1;
            _settings.Levels = tuple.Item2;
            _settings.LevelRanges = tuple.Item3;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < lbLabels.Items.Count; i++)
            {
                lbLabels.SetSelected(i, !lbLabels.GetSelected(i));
               
            }
        }

        private void bFreqResppUp_Click(object sender, EventArgs e)
        {
            var tuple = spectrumAnalyser1.ShiftUp();
            _settings.Bins = tuple.Item1;
            _settings.Levels = tuple.Item2;
            _settings.LevelRanges = tuple.Item3;
        }

        private void bFreqResppDown_Click(object sender, EventArgs e)
        {
            var tuple = spectrumAnalyser1.ShiftDown();
            _settings.Bins = tuple.Item1;
            _settings.Levels = tuple.Item2;
            _settings.LevelRanges = tuple.Item3;
        }
    }
}