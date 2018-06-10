// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorBalancer.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2015-2017 Sebastian Gruchacz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// <summary>
//   Implementation of ColorBalancer class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Root.Shared
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Defines 
    /// </summary>
    /// <remarks>Influenced by http://stackoverflow.com/questions/1720528/what-is-the-best-algorithm-for-finding-the-closest-color-in-an-array-to-another </remarks>
    public class ColorBalancer
    {
        private readonly float _colorWeightHue;

        private readonly float _colorWeightSaturation;

        private readonly float _colorWeightBrightness;

        private readonly float _colorWeightRed;

        private readonly float _colorWeightGreen;

        private readonly float _colorWeightBlue;

        private readonly float _colorProportion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBalancer"/> class.
        /// </summary>
        /// <param name="colorWeightHue">Inversed Weight of Hue influence</param>
        /// <param name="colorWeightSaturation">Weight of Saturation influence</param>
        /// <param name="colorWeightBrightness">Weight of Brightness influence</param>
        /// <param name="colorWeightRed">Inversed Weight of red color influence</param>
        /// <param name="colorWeightGreen">Inversed Weight of Green color influence</param>
        /// <param name="colorWeightBlue">Inversed Weight of Blue color influence</param>
        /// <param name="colorProportion">Global Inversed Weight of RGB colors influence</param>
        private ColorBalancer(
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

        /// <summary>
        /// Compares how much two colors are similar.
        /// </summary>
        /// <param name="srcColor">The source color. </param>
        /// <param name="destColor">The destination color.</param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public float ColorMatching(Color srcColor, Color destColor)
        {
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

        #region Default settings

        // weights are reversed

        private const float COLOR_WEIGHT_HUE = 16.3f;

        private const float COLOR_WEIGHT_SATURATION = 25.3f;

        private const float COLOR_WEIGHT_BRIGHTNESS = 37.3f;

        private const float COLOR_WEIGHT_RED = 28.5f;

        private const float COLOR_WEIGHT_GREEN = 18.5f;

        private const float COLOR_WEIGHT_BLUE = 28.75f;

        private const float COLOR_PROPORTION = 0.2f; // 100f / 255f;

        /// <summary>
        /// Gets the default color balancer rules
        /// </summary>
        public static ColorBalancer Default
        {
            get
            {
                return new ColorBalancer(
                    COLOR_WEIGHT_HUE,
                    COLOR_WEIGHT_SATURATION,
                    COLOR_WEIGHT_BRIGHTNESS,
                    COLOR_WEIGHT_RED,
                    COLOR_WEIGHT_GREEN,
                    COLOR_WEIGHT_BLUE,
                    COLOR_PROPORTION);
            }
        }

        #endregion

        /// <summary>
        /// Gets the default color balancer rules but without RGB part influence.
        /// </summary>
        public static ColorBalancer DefaultWithoutRgbInfluence
        {
            get
            {
                return new ColorBalancer(
                    COLOR_WEIGHT_HUE,
                    COLOR_WEIGHT_SATURATION,
                    COLOR_WEIGHT_BRIGHTNESS,
                    COLOR_WEIGHT_RED,
                    COLOR_WEIGHT_GREEN,
                    COLOR_WEIGHT_BLUE,
                    0.0f);
            }
        }
    }
}