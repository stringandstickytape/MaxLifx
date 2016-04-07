using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using MaxLifx.Controllers;
using MaxLifx.Processors.ProcessorSettings;
using MaxLifx.SoundToken;
using MaxLifx.Threads;
using MaxLifx.UIs;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MaxLifx
{
    public class SoundGeneratorProcessor : ProcessorBase
    {
        private Random _r;

        [XmlIgnore]
        public SoundGeneratorSettings SettingsCast
        {
            get { return ((SoundGeneratorSettings) Settings); }
        }

        public override ISettings Settings { get; set; }

        public override string SettingsAsXml
        {
            get { return ((SoundGeneratorSettings) (Settings)).ToXmlString(); }
            set
            {
                var s = new SoundGeneratorSettings();

                using (var st = new StringReader(value))
                {
                    s = (SoundGeneratorSettings) (new XmlSerializer(typeof (SoundGeneratorSettings)).Deserialize(st));
                }

                Settings = s;
            }
        }

        public void SoundGenerator(MaxLifxBulbController bulbController, Random random)
        {
            _r = random;

            if (Settings == null)
            {
                Settings = new SoundGeneratorSettings();
            }

            var f =
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MaxLifx\\Sounds\\Loops",
                    "*.wav");
            var enu = f.GetEnumerator();

            while (enu.MoveNext())
            {
                SettingsCast.Sounds.Add(
                    new Sound(
                        enu.Current.ToString()
                            .Replace(
                                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                "\\MaxLifx\\Sounds\\Loops\\", "")
                            .Replace(".wav", ""), enu.Current.ToString(), Sound.SoundTypes.Looping));
            }

            f =
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MaxLifx\\Sounds\\Random",
                    "*.wav");
            enu = f.GetEnumerator();

            while (enu.MoveNext())
            {
                SettingsCast.Sounds.Add(
                    new Sound(
                        enu.Current.ToString()
                            .Replace(
                                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                "\\MaxLifx\\Sounds\\Random\\", "")
                            .Replace(".wav", ""), enu.Current.ToString(), Sound.SoundTypes.Random));
            }

            var de = new MMDeviceEnumerator();
            de.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            DoMainLoop();
        }

        private void DoMainLoop()
        {
            VolumeSampleProvider v = null;
            bool previousOffOrOn, offOrOn = false;

            while (!TerminateThread)
            {
                if (ShowUI)
                {
                    var t = new Thread(() =>
                    {
                        var form = new SoundGeneratorUI(SettingsCast); /* (SettingsCast, bulbController.Bulbs);*/
                        form.ShowDialog();
                    });
                    t.Start();

                    ShowUI = false;
                }

                previousOffOrOn = offOrOn;
                offOrOn = Settings.OffOrOn();

                // Process any messages from the UI
                lock (SettingsCast.Messages)
                {
                    if (SettingsCast.Messages.Count > 0)
                    {
                        Sound sound;

                        foreach (var message in SettingsCast.Messages)
                        {
                            switch (message.SoundMessageType)
                            {
                                case SoundMessageTypes.SetVolume:
                                    var newVolume = (((float) ((int) (message.Parameter))))/100;
                                    sound = SettingsCast.GetSoundFromUUID(message.SoundUUID);
                                    sound.Volume = newVolume;

                                    if (sound.WaveOut != null && sound.VolumeProvider != null)
                                        sound.VolumeProvider.Volume = newVolume;

                                    break;
                                case SoundMessageTypes.SetPan:
                                    var newPan = (((float) ((int) (message.Parameter))))/50 - 1;
                                    sound = SettingsCast.GetSoundFromUUID(message.SoundUUID);
                                    sound.Pan = newPan;
                                    if (sound.WaveOut != null && sound.PanProvider != null)
                                    {
                                        sound.PanProvider.Pan = newPan;
                                    }

                                    break;
                            }
                        }
                        SettingsCast.Messages.Clear();
                    }
                }


                // Start looping sounds
                if (offOrOn)
                    foreach (
                        var sound in
                            SettingsCast.Sounds.Where(
                                x => x.SoundType == Sound.SoundTypes.Looping && x.StartStopRequest == StartStop.Start))
                        StartLoopingSound(sound);

                // Stop looping sounds
                foreach (
                    var sound in
                        SettingsCast.Sounds.Where(
                            x => x.SoundType == Sound.SoundTypes.Looping && x.StartStopRequest == StartStop.Stop))
                {
                    sound.Started = false;
                    sound.StartStopRequest = StartStop.Neither;

                    if (sound.WaveOut == null)
                        continue;

                    TerminateSound(sound);
                }

                // "Start" random sounds
                if (offOrOn)
                    foreach (
                        var sound in
                            SettingsCast.Sounds.Where(
                                x => x.SoundType == Sound.SoundTypes.Random && x.StartStopRequest == StartStop.Start))
                    {
                        sound.Started = true;
                        sound.StartStopRequest = StartStop.Neither;
                    }

                // Stop random sounds
                foreach (
                    var sound in
                        SettingsCast.Sounds.Where(
                            x => x.SoundType == Sound.SoundTypes.Random && x.StartStopRequest == StartStop.Stop))
                {
                    sound.Started = false;
                    sound.StartStopRequest = StartStop.Neither;

                    if (sound.WaveOut == null)
                        continue;

                    TerminateSound(sound);
                }

                // Play random Sounds
                if (offOrOn)
                    foreach (
                        var sound in SettingsCast.Sounds.Where(x => x.SoundType == Sound.SoundTypes.Random && x.Started)
                        )
                    {
                        if (_r.Next(sound.Frequency*10) == 0 && sound.WaveOut == null) // every 30 seconds
                        {
                            var reader = new WaveFileReader(sound.Filename);
                            sound.WaveOut = new WaveOut();
                            sound.VolumeProvider = new VolumeSampleProvider(reader.ToSampleProvider());
                            sound.VolumeProvider.Volume = sound.Volume;
                            sound.PanProvider = new PanningSampleProvider(sound.VolumeProvider);
                            sound.PanProvider.Pan = sound.Pan;
                            sound.WaveOut.Init(sound.PanProvider);
                            sound.WaveOut.Play();
                        }
                    }

                // Tidy up stopped randoms
                foreach (
                    var sound in
                        SettingsCast.Sounds.Where(
                            x =>
                                x.SoundType == Sound.SoundTypes.Random && x.WaveOut != null &&
                                x.WaveOut.PlaybackState == PlaybackState.Stopped))
                {
                    TerminateSound(sound);
                }

                // if we've just turned off, kill all playback
                if (!offOrOn && previousOffOrOn)
                {
                    foreach (var sound in SettingsCast.Sounds.Where(x => x.WaveOut != null))
                    {
                        TerminateSound(sound);
                    }
                }

                // if we've just turned on, kill all playback
                if (offOrOn && !previousOffOrOn)
                {
                    foreach (var sound in SettingsCast.Sounds.Where(x => x.WaveOut == null && x.Started))
                    {
                        StartLoopingSound(sound);
                    }
                }

                Thread.Sleep(100);
            }

            foreach (var sound in SettingsCast.Sounds.Where(x => x.WaveOut != null))
                TerminateSound(sound);
        }

        private static void StartLoopingSound(Sound sound)
        {
            sound.Started = true;
            sound.StartStopRequest = StartStop.Neither;

            if (sound.WaveOut != null)
                TerminateSound(sound);

            var reader = new AudioFileReader(sound.Filename);
            var loop = new LoopStream(reader);
            sound.WaveOut = new WaveOut();

            sound.VolumeProvider = new VolumeSampleProvider(loop.ToSampleProvider());
            sound.VolumeProvider.Volume = sound.Volume;
            sound.PanProvider = new PanningSampleProvider(sound.VolumeProvider);
            sound.PanProvider.Pan = sound.Pan;

            sound.WaveOut.Init(sound.PanProvider);
            sound.WaveOut.Play();
        }

        private static void TerminateSound(Sound sound)
        {
            sound.WaveOut.Stop();
            sound.WaveOut.Dispose();
            sound.WaveOut = null;
            sound.VolumeProvider = null;
            sound.PanProvider = null;
        }
    }


    public enum StartStop
    {
        Neither,
        Start,
        Stop
    }
}