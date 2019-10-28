using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;

namespace MaxLifx.Controls
{
    public class SpectrumAnalyserEngine
    {
        // Based on the nAudio source code

        // Other inputs are also usable. Just look through the NAudio library.
        private static readonly int FftLength = 1024; // NAudio fft wants powers of two!
        private readonly SampleAggregator _sampleAggregator = new SampleAggregator(FftLength);
        public int Bins = 512; // guess a 1024 size FFT, bins is half FFT size
        public List<Point> LatestPoints;
        public int SelectedBin = 10;
        private IWaveIn _waveIn;

        public SpectrumAnalyserEngine()
        {
            LatestPoints = new List<Point>{new Point(0, 100), new Point(1, 50)};
        }

        public void StartCapture()
        {
            _sampleAggregator.FftCalculated += FftCalculated;
            var deviceEnum = new MMDeviceEnumerator();
            var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);

            if (_waveIn == null)
            {
                _waveIn = new WasapiLoopbackCapture(); // device);
                _waveIn.DataAvailable += OnDataAvailable;
                _waveIn.RecordingStopped += OnRecordingStopped;
            }
            // Forcibly turn on the microphone (some programs (Skype) turn it off).
            device.AudioEndpointVolume.Mute = false;

            _waveIn.StartRecording();
        }

        private void FftCalculated(object sender, FftEventArgs e)
        {
            Update(e.Result);
        }

        public void Update(Complex[] fftResults)
        {
            if (fftResults.Length/2 != Bins)
            {
                Bins = fftResults.Length/2;
            }

            var points = new List<Point>();
            for (var n = 0; n < fftResults.Length/2; n ++)
            {
                points.Add(new Point(n, (int) (GetYPosLog(fftResults[n]))));
            }
            LatestPoints = points;
        }

        private double GetYPosLog(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            var intensityDb = 10*Math.Log10(Math.Sqrt(c.X*c.X + c.Y*c.Y));
            double minDb = -90;
            if (intensityDb < minDb) intensityDb = minDb;
            var percent = intensityDb/minDb;
            // we want 0dB to be at the top (i.e. yPos = 0)
            var yPos = percent*200;
            return yPos;
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show($"A problem was encountered during recording {e.Exception.Message}");
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            var buffer = e.Buffer;
            var bytesRecorded = e.BytesRecorded;
            var bufferIncrement = _waveIn.WaveFormat.BlockAlign;

            for (var index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                var sample32 = BitConverter.ToSingle(buffer, index);
                _sampleAggregator.Add(sample32);
            }
        }

        public void StopCapture()
        {
            _waveIn?.StopRecording();
        }
    }
}