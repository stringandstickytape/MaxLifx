using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

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

        public void SetupHandles(List<int> bins, List<int> levels, List<byte> levelRanges)
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

        public void GetHandles(out List<int> bins, out List<int> levels, out List<byte> levelRanges)
        {
            bins = _handles.Select(x => x.Bin).ToList();
            levels = _handles.Select(x => (int)x.Level).ToList();
            levelRanges = _handles.Select(x => (byte)x.LevelRange).ToList();
        }

        public List<byte> ResetRanges()
        {
            var retVal = new List<byte>();

            foreach (var handle in _handles)
            {
                handle.LevelRange = 25;
                if (handle.Level + ((int)handle.LevelRange)/2 > 255) handle.LevelRange = (byte)(255 - handle.Level);
                if (handle.Level - ((int)handle.LevelRange)/2 <0) handle.LevelRange = (handle.Level);

                retVal.Add(handle.LevelRange);
            }
            return retVal;
        }


        public (List<int>,List<int>, List<byte>) Redistribute()
        {
            (List<int>, List<int>, List<byte>) retVal;
            retVal.Item1 = new List<int>();
            retVal.Item2 = new List<int>();
            retVal.Item3 = new List<byte>();

            var ctr = 0;

            foreach (var handle in _handles)
            {
                handle.Bin = 512 * ctr / _handles.Count();

                handle.Level = GetDefaultLevelForBin(handle.Bin);
                handle.LevelRange = 70;

                retVal.Item1.Add(handle.Bin);
                retVal.Item2.Add(handle.Level);
                retVal.Item3.Add(handle.LevelRange);

                ctr++;
            }
            return retVal;
        }

        public (List<int>, List<int>, List<byte>) ShiftUp()
        {
            (List<int>, List<int>, List<byte>) retVal;
            retVal.Item1 = new List<int>();
            retVal.Item2 = new List<int>();
            retVal.Item3 = new List<byte>();

            var ctr = 0;

            foreach (var handle in _handles)
            {
                handle.Level = (byte)(handle.Level - 10);

                retVal.Item1.Add(handle.Bin);
                retVal.Item2.Add(handle.Level);
                retVal.Item3.Add(handle.LevelRange);

                ctr++;
            }
            return retVal;
        }

        public (List<int>, List<int>, List<byte>) ShiftDown()
        {
            (List<int>, List<int>, List<byte>) retVal;
            retVal.Item1 = new List<int>();
            retVal.Item2 = new List<int>();
            retVal.Item3 = new List<byte>();

            var ctr = 0;

            foreach (var handle in _handles)
            {
                handle.Level = (byte)(handle.Level + 10);

                retVal.Item1.Add(handle.Bin);
                retVal.Item2.Add(handle.Level);
                retVal.Item3.Add(handle.LevelRange);

                ctr++;
            }
            return retVal;
        }

        private byte GetDefaultLevelForBin(int bin)
        {
            if (bin == 0) return 65;
            return (byte)(Math.Log(bin/20f+.8f)*13+65);
        }
    }


}