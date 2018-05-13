/*
Copyright (c) 2017 Maicon Feldhaus

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
documentation files (the "Software"), to deal in the Software without restriction, including without
limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the
Software, and to permit persons to whom the Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
*/

using System;

public static class Ease
{
    private const double PI_M2 = Math.PI * 2.0;
    private const double PI_D2 = Math.PI / 2.0;

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double Linear(double t, double b, double c, double d)
    {
        return c * t / d + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InSine(double t, double b, double c, double d)
    {
        return -c * Math.Cos(t / d * PI_D2) + c + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutSine(double t, double b, double c, double d)
    {
        return c * Math.Sin(t / d * PI_D2) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutSine(double t, double b, double c, double d)
    {
        return -c / 2.0 * (Math.Cos(Math.PI * t / d) - 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InQuint(double t, double b, double c, double d)
    {
        return c * (t /= d) * t * t * t * t + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutQuint(double t, double b, double c, double d)
    {
        return c * ((t = t / d - 1.0) * t * t * t * t + 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutQuint(double t, double b, double c, double d)
    {
        if ((t /= d / 2.0) < 1.0)
        {
            return c / 2.0 * t * t * t * t * t + b;
        }
        return c / 2.0 * ((t -= 2.0) * t * t * t * t + 2.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InQuart(double t, double b, double c, double d)
    {
        return c * (t /= d) * t * t * t + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutQuart(double t, double b, double c, double d)
    {
        return -c * ((t = t / d - 1.0) * t * t * t - 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutQuart(double t, double b, double c, double d)
    {
        if ((t /= d / 2.0) < 1.0)
        {
            return c / 2.0 * t * t * t * t + b;
        }
        return -c / 2.0 * ((t -= 2.0) * t * t * t - 2.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InQuad(double t, double b, double c, double d)
    {
        return c * (t /= d) * t + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutQuad(double t, double b, double c, double d)
    {
        return -c * (t /= d) * (t - 2.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutQuad(double t, double b, double c, double d)
    {
        if ((t /= d / 2.0) < 1.0)
        {
            return c / 2.0 * t * t + b;
        }
        return -c / 2.0 * ((--t) * (t - 2.0) - 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InExpo(double t, double b, double c, double d)
    {
        return (t == 0) ? b : c * Math.Pow(2.0, 10.0 * (t / d - 1.0)) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutExpo(double t, double b, double c, double d)
    {
        return (t == d) ? b + c : c * (-Math.Pow(2.0, -10.0 * t / d) + 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutExpo(double t, double b, double c, double d)
    {
        if (t == 0)
        {
            return b;
        }
        if (t == d)
        {
            return b + c;
        }
        if ((t /= d / 2.0) < 1.0)
        {
            return c / 2.0 * Math.Pow(2.0, 10.0 * (t - 1.0)) + b;
        }
        return c / 2.0 * (-Math.Pow(2.0, -10.0 * --t) + 2.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <param name="a">Amplitude</param>
    /// <param name="p">Period</param>
    /// <returns></returns>
    public static double InElastic(double t, double b, double c, double d, double a = 0, double p = 0)
    {
        double s;
        if (t == 0)
        {
            return b;
        }
        if ((t /= d) == 1)
        {
            return b + c;
        }
        if (p == 0)
        {
            p = d * 0.3;
        }
        if (a == 0 || a < Math.Abs(c))
        {
            a = c;
            s = p / 4.0;
        }
        else
        {
            s = p / PI_M2 * Math.Asin(c / a);
        }
        return -(a * Math.Pow(2.0, 10.0 * (t -= 1)) * Math.Sin((t * d - s) * PI_M2 / p)) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <param name="a">Amplitude</param>
    /// <param name="p">Period</param>
    /// <returns></returns>
    public static double OutElastic(double t, double b, double c, double d, double a = 0, double p = 0)
    {
        double s;
        if (t == 0)
        {
            return b;
        }
        if ((t /= d) == 1)
        {
            return b + c;
        }
        if (p == 0)
        {
            p = d * 0.3;
        }
        if (a == 0 || a < Math.Abs(c))
        {
            a = c;
            s = p / 4.0;
        }
        else
        {
            s = p / PI_M2 * Math.Asin(c / a);
        }
        return (a * Math.Pow(2.0, -10.0 * t) * Math.Sin((t * d - s) * PI_M2 / p) + c + b);
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <param name="a">Amplitude</param>
    /// <param name="p">Period</param>
    /// <returns></returns>
    public static double InOutElastic(double t, double b, double c, double d, double a = 0, double p = 0)
    {
        double s;
        if (t == 0)
        {
            return b;
        }
        if ((t /= d / 2) == 2)
        {
            return b + c;
        }
        if (p == 0)
        {
            p = d * (0.3 * 1.5);
        }
        if (a == 0 || a < Math.Abs(c))
        {
            a = c;
            s = p / 4.0;
        }
        else
        {
            s = p / PI_M2 * Math.Asin(c / a);
        }
        if (t < 1)
        {
            return -0.5 * (a * Math.Pow(2.0, 10.0 * (t -= 1.0)) * Math.Sin((t * d - s) * PI_M2 / p)) + b;
        }
        return a * Math.Pow(2.0, -10.0 * (t -= 1.0)) * Math.Sin((t * d - s) * PI_M2 / p) * 0.5 + c + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InCirc(double t, double b, double c, double d)
    {
        return -c * (Math.Sqrt(1.0 - (t /= d) * t) - 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutCirc(double t, double b, double c, double d)
    {
        return c * Math.Sqrt(1.0 - (t = t / d - 1.0) * t) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutCirc(double t, double b, double c, double d)
    {
        if ((t /= d / 2.0) < 1.0)
        {
            return -c / 2.0 * (Math.Sqrt(1.0 - t * t) - 1.0) + b;
        }
        return c / 2.0 * (Math.Sqrt(1.0 - (t -= 2.0) * t) + 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <param name="s">Overshoot</param>
    /// <returns></returns>
    public static double InBack(double t, double b, double c, double d, double s = 1.70158)
    {
        return c * (t /= d) * t * ((s + 1.0) * t - s) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <param name="s">Overshoot</param>
    /// <returns></returns>
    public static double OutBack(double t, double b, double c, double d, double s = 1.70158)
    {
        return c * ((t = t / d - 1.0) * t * ((s + 1.0) * t + s) + 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <param name="s">Overshoot</param>
    /// <returns></returns>
    public static double InOutBack(double t, double b, double c, double d, double s = 1.70158)
    {
        if ((t /= d / 2.0) < 1.0)
        {
            return c / 2.0 * (t * t * (((s *= (1.525)) + 1.0) * t - s)) + b;
        }
        return c / 2.0 * ((t -= 2.0) * t * (((s *= (1.525)) + 1.0) * t + s) + 2.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InBounce(double t, double b, double c, double d)
    {
        return c - OutBounce(d - t, 0, c, d) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutBounce(double t, double b, double c, double d)
    {
        if ((t /= d) < (1.0 / 2.75))
        {
            return c * (7.5625 * t * t) + b;
        }
        else if (t < (2.0 / 2.75))
        {
            return c * (7.5625 * (t -= (1.5 / 2.75)) * t + 0.75) + b;
        }
        else if (t < (2.5 / 2.75))
        {
            return c * (7.5625 * (t -= (2.25 / 2.75)) * t + 0.9375) + b;
        }
        else
        {
            return c * (7.5625 * (t -= (2.625 / 2.75)) * t + 0.984375) + b;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutBounce(double t, double b, double c, double d)
    {
        if (t < d / 2.0)
        {
            return InBounce(t * 2.0, 0, c, d) * 0.5 + b;
        }
        else return OutBounce(t * 2.0 - d, 0, c, d) * 0.5 + c * 0.5 + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InCubic(double t, double b, double c, double d)
    {
        return c * (t /= d) * t * t + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double OutCubic(double t, double b, double c, double d)
    {
        return c * ((t = t / d - 1.0) * t * t + 1.0) + b;
    }

    /// <summary>
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Beginning value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns></returns>
    public static double InOutCubic(double t, double b, double c, double d)
    {
        if ((t /= d / 2.0) < 1.0)
        {
            return c / 2.0 * t * t * t + b;
        }
        return c / 2.0 * ((t -= 2.0) * t * t + 2.0) + b;
    }
}