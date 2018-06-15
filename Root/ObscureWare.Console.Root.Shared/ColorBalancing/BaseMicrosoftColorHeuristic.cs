namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    using System;
    using System.Linq;

    public abstract class BaseMicrosoftColorHeuristic : IColorHeuristic
    {
        /// <inheritdoc />
        public abstract double CalculateDistance(uint color1, uint color2);

        protected static double Distance(uint[] c1c, uint[] c2c)
            => Math.Sqrt(c1c.Zip(c2c, (a, b) => Math.Pow((int)a - (int)b, 2)).Sum());

        protected static uint[] RGB(uint c) => new[] { c & 0xFF, (c >> 8) & 0xFF, (c >> 16) & 0xFF };

        protected static uint[] HSV(uint c)
        {
            var rgb = RGB(c).Select(_ => (int)_).ToArray();
            int max = rgb.Max();
            int min = rgb.Min();

            int d = max - min;
            int h = 0;
            int s = (int)(255 * ((max == 0) ? 0 : d / (double)max));
            int v = max;

            if (d != 0)
            {
                double dh;
                if (rgb[0] == max) dh = ((rgb[1] - rgb[2]) / (double)d);
                else if (rgb[1] == max) dh = 2.0 + ((rgb[2] - rgb[0]) / (double)d);
                else /* if (rgb[2] == max) */ dh = 4.0 + ((rgb[0] - rgb[1]) / (double)d);
                dh *= 60;
                if (dh < 0) dh += 360.0;
                h = (int)(dh * 255.0 / 360.0);
            }

            return new[] { (uint)h, (uint)s, (uint)v };
        }
    }
}