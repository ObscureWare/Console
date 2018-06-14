namespace ObscureWare.Console.Root.Desktop.Schema
{
    using System;
    using System.Linq;

    using Shared;

    // Copyright (C) Microsoft.  All rights reserved.
    // Licensed under the terms described in the LICENSE file in the root of this project.
    //

    // Based on e-MIT-ed source from https://github.com/Microsoft/console/blob/master/tools/ColorTool/ColorTool/ColorScheme.cs
    // Adapted to be used and reused ObscureWare's Console library

    // TODO: looks like concurrent heuristics... Perhaps better... Maybe therefore extract heuristic as strategy plug-in?

    public class ColorScheme
    {
        public string Name { get; }

        public uint[] colorTable = null;
        public uint? foreground = null;
        public uint? background = null;

        public ColorScheme(string name)
        {
            this.Name = name;
        }

        public int CalculateIndex(uint value) =>
            this.colorTable.Select((color, idx) => Tuple.Create<uint, int>(color, idx))
                .OrderBy(Difference(value))
                .First().Item2;

        private static Func<Tuple<uint, int>, double> Difference(uint c1) =>
            // heuristic 1: nearest neighbor in RGB space
            // tup => Distance(RGB(c1), RGB(tup.Item1));
            // heuristic 2: nearest neighbor in RGB space
            // tup => Distance(HSV(c1), HSV(tup.Item1));
            // heuristic 3: weighted RGB L2 distance
            tup => WeightedRGBSimilarity(c1, tup.Item1);

        private static double WeightedRGBSimilarity(uint c1, uint c2)
        {
            var rgb1 = RGB(c1);
            var rgb2 = RGB(c2);
            var dist = rgb1.Zip(rgb2, (a, b) => Math.Pow((int)a - (int)b, 2)).ToArray();
            var rbar = (rgb1[0] + rgb1[0]) / 2.0;
            return Math.Sqrt(dist[0] * (2 + rbar / 256.0) + dist[1] * 4 + dist[2] * (2 + (255 - rbar) / 256.0));
        }

        private static double Distance(uint[] c1c, uint[] c2c)
            => Math.Sqrt(c1c.Zip(c2c, (a, b) => Math.Pow((int)a - (int)b, 2)).Sum());

        internal static uint[] RGB(uint c) => new[] { c & 0xFF, (c >> 8) & 0xFF, (c >> 16) & 0xFF };

        internal static uint[] HSV(uint c)
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

        internal void Dump(IConsole console)
        {
            void InnerDump(string str, uint c)
            {
                var rgb = RGB(c);
                var hsv = HSV(c);
                console.WriteLine($"{str} =\tRGB({rgb[0]}, {rgb[1]}, {rgb[2]}),\tHSV({hsv[0]}, {hsv[1]}, {hsv[2]})");
            }

            for (int i = 0; i < 16; ++i)
            {
                InnerDump($"Color[{i}]", this.colorTable[i]);
            }

            if (this.foreground != null)
            {
                InnerDump("FG       ", this.foreground.Value);
            }

            if (this.background != null)
            {
                InnerDump("BG       ", this.background.Value);
            }
        }
    }
}