namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    using System;
    using System.Drawing;

    public abstract class BaseGruchenColorHeuristic : IColorHeuristic
    {

        private readonly float _colorWeightHue;

        private readonly float _colorWeightSaturation;

        private readonly float _colorWeightBrightness;

        private readonly float _colorWeightRed;

        private readonly float _colorWeightGreen;

        private readonly float _colorWeightBlue;

        private readonly float _colorProportion;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGruchenColorHeuristic"/> class.
        /// </summary>
        /// <param name="colorWeightHue">Inversed Weight of Hue influence</param>
        /// <param name="colorWeightSaturation">Weight of Saturation influence</param>
        /// <param name="colorWeightBrightness">Weight of Brightness influence</param>
        /// <param name="colorWeightRed">Inversed Weight of red color influence</param>
        /// <param name="colorWeightGreen">Inversed Weight of Green color influence</param>
        /// <param name="colorWeightBlue">Inversed Weight of Blue color influence</param>
        /// <param name="colorProportion">Global Inversed Weight of RGB colors influence</param>
        protected BaseGruchenColorHeuristic(
            float colorWeightHue,
            float colorWeightSaturation,
            float colorWeightBrightness,
            float colorWeightRed,
            float colorWeightGreen,
            float colorWeightBlue,
            float colorProportion)
        {
            this._colorWeightHue = colorWeightHue;
            this._colorWeightSaturation = colorWeightSaturation;
            this._colorWeightBrightness = colorWeightBrightness;
            this._colorWeightRed = colorWeightRed;
            this._colorWeightGreen = colorWeightGreen;
            this._colorWeightBlue = colorWeightBlue;
            this._colorProportion = colorProportion;
        }

        /// <inheritdoc />
        public double CalculateDistance(uint color1, uint color2)
        {
            var srcColor = Color.FromArgb((int)color1);
            var destColor = Color.FromArgb((int)color2);

            var sh = srcColor.GetHue();
            var ss = srcColor.GetSaturation();
            var sb = srcColor.GetBrightness();
            var dh = destColor.GetHue();
            var ds = destColor.GetSaturation();
            var db = destColor.GetBrightness();

            var sr = srcColor.R;
            var sg = srcColor.G;
            var sc = srcColor.B;
            var dr = destColor.R;
            var dg = destColor.G;
            var dc = destColor.B;

            float result = (float)Math.Sqrt(
                (Math.Abs(sh - dh) / this._colorWeightHue) +
                (Math.Abs(ss - ds) / this._colorWeightSaturation) +
                (Math.Abs(sb - db) / this._colorWeightBrightness) +
                (Math.Abs(sr - dr) / (this._colorWeightRed * this._colorProportion)) +
                (Math.Abs(sg - dg) / (this._colorWeightGreen * this._colorProportion)) +
                (Math.Abs(sc - dc) / (this._colorWeightBlue * this._colorProportion)));

            return result;
        }
    }
}