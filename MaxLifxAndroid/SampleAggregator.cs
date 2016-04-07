using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MaxLifxAndroid
{
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
            _m = (int)Math.Log(fftLength, 2.0);
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
            _fftBuffer[_fftPos] = new Complex()
            {
                X = (float) (value*FastFourierTransform.HammingWindow(_fftPos, _fftLength)),
                Y = 0
            };

            _fftPos++;
            if (_fftPos >= _fftLength)
            {
                _fftPos = 0;
                FastFourierTransform.FFT(true, _m, _fftBuffer);
                if (FftCalculated != null) FftCalculated(this, _fftArgs);
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