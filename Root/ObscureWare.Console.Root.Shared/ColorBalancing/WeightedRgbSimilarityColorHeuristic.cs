namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    using System;
    using System.Linq;

    /// <summary>
    /// By MS
    /// </summary>
    public class WeightedRgbSimilarityColorHeuristic : BaseMicrosoftColorHeuristic
    {
        /// <inheritdoc />
        public override string Name => @"MS Weighted RGB Similarity";

        /// <inheritdoc />
        public override double CalculateDistance(uint color1, uint color2)
        {
            var rgb1 = RGB(color1);
            var rgb2 = RGB(color2);
            var dist = rgb1.Zip(rgb2, (a, b) => Math.Pow((int)a - (int)b, 2)).ToArray();
            var rbar = (rgb1[0] + rgb1[0]) / 2.0;

            return Math.Sqrt(dist[0] * (2 + rbar / 256.0) + dist[1] * 4 + dist[2] * (2 + (255 - rbar) / 256.0));
        }
    }
}