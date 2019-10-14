using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using NAudio.Wave;

namespace MaxLifx
{
    public static class Utils
    {
        public static string ToXmlString<T>(this T input)
        {
            using (var writer = new StringWriter())
            {
                input.ToXml(writer);
                return writer.ToString();
            }
        }

        public static void ToXml<T>(this T objectToSerialize, Stream stream)
        {
            new XmlSerializer(typeof (T)).Serialize(stream, objectToSerialize);
        }

        public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
        {
            new XmlSerializer(typeof (T)).Serialize(writer, objectToSerialize);
        }

        public static void FromXml<T>(this T objectToDeserialize, Stream stream)
        {
            objectToDeserialize = (T) (new XmlSerializer(typeof (T)).Deserialize(stream));
        }

        public static void FromXml<T>(this T objectToDeserialize, StringReader reader)
        {
            objectToDeserialize = (T) (new XmlSerializer(typeof (T)).Deserialize(reader));
        }

        public static void FromXmlString<T>(this string input, ref T output)
        {
            using (var reader = new StringReader(input))
            {
                output.FromXml(reader);
            }
        }

        // Color.GetSaturation() and Color.GetBrightness() return HSL values, not HSV
        // so need this to convert
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color HsvToRgb(double h, double S, double V)
        {
            int r, g, b;
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        // from http://stackoverflow.com/questions/2900837/does-the-net-framework-3-5-have-an-hsbtorgb-converter-or-do-i-need-to-roll-my-o
        public static Color HsbToRgb(double h, double s, double b)
        {
            if (s == 0)
                return RawRgbToRgb(b, b, b);
            else
            {
                var sector = h / 60;
                var sectorNumber = (int)Math.Truncate(sector);
                var sectorFraction = sector - sectorNumber;
                var b1 = b * (1 - s);
                var b2 = b * (1 - s * sectorFraction);
                var b3 = b * (1 - s * (1 - sectorFraction));
                switch (sectorNumber)
                {
                    case 0:
                        return RawRgbToRgb(b, b3, b1);
                    case 1:
                        return RawRgbToRgb(b2, b, b1);
                    case 2:
                        return RawRgbToRgb(b1, b, b3);
                    case 3:
                        return RawRgbToRgb(b1, b2, b);
                    case 4:
                        return RawRgbToRgb(b3, b1, b);
                    case 5:
                        return RawRgbToRgb(b, b1, b2);
                    default:
                        throw new ArgumentException("Brightness must be between 0 and 360");
                }
            }
        }

        private static Color RawRgbToRgb(double rawR, double rawG, double rawB)
        {
            return Color.FromArgb(
                (int)Math.Round(rawR * 255),
                (int)Math.Round(rawG * 255),
                (int)Math.Round(rawB * 255));
        }
    }

    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        /// <summary>
        ///     Creates a new Loop stream
        /// </summary>
        /// <param name="sourceStream">
        ///     The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
        ///     or else we will not loop to the start again.
        /// </param>
        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            EnableLooping = true;
        }

        /// <summary>
        ///     Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping { get; set; }

        /// <summary>
        ///     Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        /// <summary>
        ///     LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get { return sourceStream.Length; }
        }

        /// <summary>
        ///     LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                var bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }

 
    }
}