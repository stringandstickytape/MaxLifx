using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxLifx.Waveforms
{
    public class AdditionalWaveform
    {
        public int Duration { get; set; }
        public WaveTypes WaveType { get; set; }
        public bool InverseWaveform { get; set; }
    }
}
