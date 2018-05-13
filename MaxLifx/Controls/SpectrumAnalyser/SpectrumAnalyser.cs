using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using MaxLifx.Processors.ProcessorSettings;

namespace MaxLifx.Controls
{
    public partial class SpectrumAnalyser : Control
    {
        private readonly SpectrumAnalyserEngine _spectrumEngine = new SpectrumAnalyserEngine();
        private readonly System.Timers.Timer _updateSpectrumTimer = new System.Timers.Timer(50) { AutoReset = true };
        private readonly List<SpectrumAnalyserHandle> _handles = new List<SpectrumAnalyserHandle>();

        public event EventHandler SelectionChanged;
        
        public SpectrumAnalyser()
        {
            ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        public void StartCapture()
        {
            _spectrumEngine.StartCapture();
            _updateSpectrumTimer.Elapsed += UpdateSpectrumTimer_Elapsed;
            _updateSpectrumTimer.Enabled = true;
        }

        public void StopCapture()
        {
            _spectrumEngine.StopCapture();
            _updateSpectrumTimer.Elapsed += UpdateSpectrumTimer_Elapsed;
            _updateSpectrumTimer.Enabled = false;
        }

        private void UpdateSpectrumTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Invalidate();
        }

        public void AddHandle()
        {
            _handles.Add(new SpectrumAnalyserHandle() { Number = _handles.Count() });
        }

        public void RemoveHandle()
        {
            _handles.RemoveAt(_handles.Count()-1);
        }

        public int SetHandleCount(int handles)
        {
            var startCount = _handles.Count();

            if(handles > startCount)
                for (int i = startCount; i < handles; i++)
                    AddHandle();
            else
            if (handles < startCount)
                for (int i = handles; i < startCount; i++)
                    RemoveHandle();

            return handles - startCount;
        }

        public void SetupHandles(List<int> bins, List<int> levels, List<int> levelRanges)
        {
            for (int i = 0; i < bins.Count(); i++)
            {
                if (i < _handles.Count)
                {
                    _handles[i].Bin = bins[i];
                    _handles[i].Level = (byte)levels[i];
                    _handles[i].LevelRange = (byte)levelRanges[i];
                }
                else
                {
                    _handles.Add(new SpectrumAnalyserHandle() { Bin = bins[i], Level = (byte)levels[i], LevelRange = (byte)levelRanges[i], Number = i});
                }
            }

            if(bins.Count < _handles.Count)
                for (int i = _handles.Count; i > bins.Count; i--)
                    _handles.RemoveAt(i-1);
        }

        public void GetHandles(out List<int> bins, out List<int> levels, out List<int> levelRanges)
        {
            bins = _handles.Select(x => x.Bin).ToList();
            levels = _handles.Select(x => (int)x.Level).ToList();
            levelRanges = _handles.Select(x => (int)x.LevelRange).ToList();
        }

        public List<int> IncrementLevels()
        {
            var retVal = new List<int>();

            foreach (var handle in _handles)
            {
                handle.Level += 5;
                if (handle.Level > 255) handle.Level = 255;
                if (handle.Level + handle.LevelRange > 255) handle.Level = (byte)(255 - handle.Level);

                retVal.Add(handle.Level);
            }
            return retVal;
        }


        public List<int> DecrementLevels()
        {
            var retVal = new List<int>();

            foreach (var handle in _handles)
            {
                handle.Level -= 5;
                if (handle.Level < 0) handle.Level = 0;
                if (handle.Level + handle.LevelRange < 0) handle.Level = (byte)(handle.Level);

                retVal.Add(handle.Level);
            }
            return retVal;
        }


        public List<int> ResetRanges()
        {
            var retVal = new List<int>();

            foreach (var handle in _handles)
            {
                handle.LevelRange = 25;
                if (handle.Level + handle.LevelRange/2 > 255) handle.LevelRange = (byte)(255 - handle.Level);
                if (handle.Level - handle.LevelRange/2 <0) handle.LevelRange = (handle.Level);

                retVal.Add(handle.LevelRange);
            }
            return retVal;
        }

        public void RedistributeBins(SoundResponseSettings settings)
        {
            if (_handles.Count == 0) return;

            double spread = 400 / _handles.Count;

            settings.Bins = new List<int>();
            settings.Levels = new List<int>();
            settings.LevelRanges = new List<int>();

            var ctr = 0;
            foreach (var handle in _handles)
            {
                handle.Bin = 10 + (int)(ctr * spread);
                handle.Level = (byte)(50 - 50 * Math.Pow(1.008, 512 - handle.Bin) / Math.Pow(1.008, 512) + 50);
                handle.LevelRange = 55;
                settings.Bins.Add(handle.Bin);
                settings.Levels.Add(handle.Level);
                settings.LevelRanges.Add(handle.LevelRange);
                ctr++;
            }
            return;
        }

    }
}