using System;
using System.Drawing;

namespace SleepLogger
{
    // Interpolate between 2 colors in C#
    // Taken from answer by user Jason
    // http://stackoverflow.com/questions/1236683/color-interpolation-between-3-colors-in-net
    class ColorInterpolator
    {
        delegate byte ComponentSelector(Color color);
        static ComponentSelector _redSelector = color => color.R;
        static ComponentSelector _greenSelector = color => color.G;
        static ComponentSelector _blueSelector = color => color.B;

        public static Color InterpolateBetween(
            Color endPoint1,
            Color endPoint2,
            double lambda)
        {
            if (lambda < 0 || lambda > 1)
            {
                throw new ArgumentOutOfRangeException("lambda");
            }
            Color color = Color.FromArgb(
                InterpolateComponent(endPoint1, endPoint2, lambda, _redSelector),
                InterpolateComponent(endPoint1, endPoint2, lambda, _greenSelector),
                InterpolateComponent(endPoint1, endPoint2, lambda, _blueSelector)
            );

            return color;
        }

        static byte InterpolateComponent(
            Color endPoint1,
            Color endPoint2,
            double lambda,
            ComponentSelector selector)
        {
            return (byte)(selector(endPoint1)
                + (selector(endPoint2) - selector(endPoint1)) * lambda);
        }
    }
}
