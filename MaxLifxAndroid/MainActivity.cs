using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MaxLifx.Controllers;
using MaxLifx.Payload;

namespace MaxLifxAndroid
{
    [Activity(Label = "MaxLifxAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private Button button, button2;
        private SeekBar seekBar1, seekBar2;
        private TextView textView1;
        private bool bulbControllerSetUp, bulbDiscoveryDone;
        private MaxLifxBulbController bulbController = new MaxLifxBulbController();
        private int progress;
        private SetColourPayload payload;
        public List<SeekBar> seekbars = new List<SeekBar>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            button = FindViewById<Button>(Resource.Id.MyButton);
            button2 = FindViewById<Button>(Resource.Id.MyButton2);
            textView1 = FindViewById<TextView>(Resource.Id.textView1);
            seekBar1 = FindViewById<SeekBar>(Resource.Id.seekBar1);
            seekBar2 = FindViewById<SeekBar>(Resource.Id.seekBar2);
            button.Click += Button_Click;
            seekBar1.ProgressChanged += SeekBar1_ProgressChanged;
            button2.Click += Button2_Click;

            for (int i = 0; i < 16; i++)
            {
                var x = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                var sb = new SeekBar(x.Context) {Max = 100};
                x.AddView(sb);
                seekbars.Add(sb);
            }


            var prefs = Application.Context.GetSharedPreferences("MaxLifxAndroid", FileCreationMode.Private);
            var somePref = prefs.GetString("Bulbs", null);
            if (somePref != null)
            {
                var bulbsPipeSeparated = somePref.Split(new[] {"^"}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var bulbPipeSeparated in bulbsPipeSeparated)
                {
                    var tokens = bulbPipeSeparated.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
                    bulbController.Bulbs.Add(new Bulb() {Label = tokens[0], MacAddress = tokens[1]});
                }

                bulbDiscoveryDone = true;
                //Show a toast
                RunOnUiThread(() => Toast.MakeText(this, somePref, ToastLength.Long).Show());
            }

            payload = new SetColourPayload
            {
                Kelvin = 3500,
                TransitionDuration = 50,
                Hue = 0,
                Saturation = 65535,
                Brightness = 65535
            };
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => RecordAudio());
            thread.Start();
            //RecordAudio();

            
        }

        private void SeekBar1_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            //RunOnUiThread(() => Toast.MakeText(this, seekBar1.Progress, ToastLength.Long).Show());

            Thread thread = new Thread(() => SetBulbColour(seekBar1.Progress));
            thread.Start();
        }

        protected override void OnPause()
        {
            SavePreferences();

            base.OnPause();
        }

        protected override void OnDestroy()
        {
            SavePreferences();

            base.OnDestroy();
        }

        private void SavePreferences()
        {
            if (!bulbControllerSetUp) return;

            var prefs = Application.Context.GetSharedPreferences("MaxLifxAndroid", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("Bulbs", string.Join("^",bulbController.Bulbs.Select(x => x.Label+"|"+x.MacAddress)));
            prefEditor.Commit();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            button.Text = string.Format("Merely a proof of concept", count++);
            SetBulbColour(seekBar1.Progress);
        }

        private void SetBulbColour(int hue, int brightness = 0 )
        {
            try
            {
                if (!bulbControllerSetUp)
                {
                    bulbControllerSetUp = true;
                    bulbController.SetupNetwork();
                }

                if (!bulbDiscoveryDone)
                {
                    bulbController.DiscoverBulbs();
                    bulbDiscoveryDone = true;
                }

                foreach (var b in bulbController.Bulbs)
                {
                    payload.Hue = hue;
                    payload.Brightness = (ushort)brightness;
                    bulbController.SetColour(b.Label, payload);
                    bulbController.SetColour(b.Label, payload);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private SampleAggregator sA;
        void RecordAudio()
        {
            sA = new SampleAggregator(1024);
            sA.FftCalculated += SA_FftCalculated;
            byte[] audioBuffer = new byte[2048];
            var audRecorder = new AudioRecord(
              // Hardware source of recording.
              AudioSource.Mic,
              // Frequency
              44100,
              // Mono or stereo
              ChannelIn.Mono,
              // Audio encoding
              Android.Media.Encoding.Pcm16bit,
              // Length of the audio clip.
              audioBuffer.Length
            );
            var startTime = DateTime.Now;
            var endTime = startTime.AddSeconds(60);
            audRecorder.StartRecording();
            while (DateTime.Now < endTime )
                {
                    try
                    {


                        // Keep reading the buffer while there is audio input.
                        audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
                        int max = 0, currval = 0;
                        int total = 0;
                        for (int i = 0; i < audioBuffer.Length; i = i + 2)
                        {
                        
                            currval = audioBuffer[i]*256 + audioBuffer[i + 1];
                            sA.Add(currval);
                            if (max < currval)
                                max = currval;
                            if(currval > 32767)
                                total = total + currval;

                        }
                        //int level = max - 62719;
                        
                        //RunOnUiThread(() => seekBar2.Progress = (int) level);
                        //Console.WriteLine(level*23);
                        //SetBulbColour(0,level*23);

                        // Write out the audio file.
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                        break;
                    }
                }
            audRecorder.Stop();
        }

        private void SA_FftCalculated(object sender, FftEventArgs e)
        {
            var points = new List<PointF>();
            for (var index = 0; index < 16; index++)
            {
                var avg = e.Result.Skip(index*16).Take(1).Average(r => r.Y);
                var newVal = GetYPosLog(new Complex() {X = index, Y = avg});
                points.Add(new PointF(index, (float) (newVal)));
                var index1 = index;
                RunOnUiThread(() => seekbars[index1].Progress = (int)(0-newVal));
            }

            //RunOnUiThread(() => seekBar2.Progress = 0-(int)points[10].Y);
            Console.WriteLine((int)points[0].X+","+ (int)points[10].Y);
        }

        private double GetYPosLog(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            var intensityDb = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -90;
            if (intensityDb < minDB) intensityDb = minDB;
            var percent = intensityDb / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            var yPos = percent * 200;
            return yPos;
        }
    }


}

