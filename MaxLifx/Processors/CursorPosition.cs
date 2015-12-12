using MaxLifx.Controllers;
using MaxLifx.Payload;
using MaxLifx.Threads;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxLifx
{
    public class CursorPositionProcessor : IProcessor
    {
        private object locker = new object();
        private bool _showUI;
        public bool ShowUI
        {
            get
            {
                lock (locker)
                {
                    return _showUI;
                }
            }
            set
            {
                lock (locker)
                {
                    _showUI = value;
                }
            }
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        public void CursorPosition()
        {
            MMDeviceEnumerator de = new MMDeviceEnumerator();
            MMDevice device = de.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            Point cursor = new Point();

            while (1 == 1)
            {
                GetCursorPos(ref cursor);

                try
                {
                    //d.Invoke("X: " + cursor.X + "  Y: " + cursor.Y, (int)(device.AudioMeterInformation.MasterPeakValue * 100) );
                }
                catch
                { }

                Thread.Sleep(500);
            }
        }
    }
}