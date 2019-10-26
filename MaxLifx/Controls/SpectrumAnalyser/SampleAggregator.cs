using System;
using NAudio.Dsp;

namespace MaxLifx.Controls
{
    // Based on the nAudio source code
    public class SampleAggregator
    {
        private readonly FftEventArgs _fftArgs;
        private readonly Complex[] _fftBuffer;
        private readonly int _fftLength;
        private readonly int _m;
        private int _fftPos;

        public SampleAggregator(int fftLength)
        {
            if (!IsPowerOfTwo(fftLength))
            {
                throw new ArgumentException("FFT Length must be a power of two");
            }
            _m = (int) Math.Log(fftLength, 2.0);
            _fftLength = fftLength;
            _fftBuffer = new Complex[fftLength];
            _fftArgs = new FftEventArgs(_fftBuffer);
        }

        public bool PerformFFT { get; set; }
        // FFT
        public event EventHandler<FftEventArgs> FftCalculated;

        private bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public void Add(float value)
        {
            if (FftCalculated != null)
            {
                // Remember the window function! There are many others as well.
                _fftBuffer[_fftPos].X = (float)(value * FastFourierTransform.HammingWindow(_fftPos, _fftLength));
                _fftBuffer[_fftPos].Y = 0; // This is always zero with audio.
                _fftPos++;
                if (_fftPos >= _fftLength)
                {
                    _fftPos = 0;
                    FastFourierTransform.FFT(true, _m, _fftBuffer);
                    FftCalculated(this, _fftArgs);
                }
            }
        }
    }

    public class FftEventArgs : EventArgs
    {
        public FftEventArgs(Complex[] result)
        {
            Result = result;
        }

        public Complex[] Result { get; private set; }
    }
}