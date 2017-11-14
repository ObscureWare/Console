// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseColorFinder.cs" company="Obscureware Solutions">
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
//   Defines the CloseColorFinder class responsible for color matching routines.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Root.Shared
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Conditions;

    /// <summary>
    /// Class responsible for finding closest color index from given console colors array.
    /// </summary>
    public class CloseColorFinder : IDisposable
    {
        private readonly ConcurrentDictionary<Color, ConsoleColor> _knownMappings = new ConcurrentDictionary<Color, ConsoleColor>();

        private readonly KeyValuePair<ConsoleColor, Color>[] _colorBuffer;

        private readonly ColorBalancer _colorBalancer;

        private bool _disposed = false;

        private readonly IEnumerable<KeyValuePair<ConsoleColor, Color>> _defaultSet;

        public CloseColorFinder(KeyValuePair<ConsoleColor, Color>[] colorBuffer, ColorBalancer colorBalancer = null)
        {
            _defaultSet = colorBuffer;

            int expLength = Enum.GetNames(typeof(ConsoleColor)).Length;

            colorBuffer.Requires(nameof(colorBuffer))
                .IsNotNull()
                .IsNotEmpty()
                .HasLength(expLength);

            if (colorBuffer.Select(pair => pair.Key).Distinct().Count() != expLength) // TODO: optionally add extension to Conditions...
            {
                throw new ArgumentException($"Must contain definition for each {nameof(ConsoleColor)} value.", nameof(colorBuffer));
            };

            this._colorBuffer = colorBuffer;
            this._colorBalancer = colorBalancer ?? ColorBalancer.Default;
        }

        /// <summary>
        /// Tries to find the closest match for given RGB color among current set of colors used by System.Console
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns> 
        public ConsoleColor FindClosestColor(Color color)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(CloseColorFinder));
            }

            ConsoleColor cc;
            if (this._knownMappings.TryGetValue(color, out cc))
            {
                return cc;
            }

            cc = this._colorBuffer.OrderBy(kp => this._colorBalancer.ColorMatching(color, kp.Value)).First().Key;
            this._knownMappings.TryAdd(color, cc);
            return cc;
        }

        /// <summary>
        /// Returns actual ARGB color stored at console enumerated colors.
        /// </summary>
        /// <param name="cc">Enumeration-index in console colors</param>
        /// <returns>ARGB color.</returns>
        public Color GetCurrentConsoleColor(ConsoleColor cc)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(CloseColorFinder));
            }

            return this._colorBuffer.Single(pair => pair.Key == cc).Value;
        }

        public CloseColorFinder GetDefault()
        {
            return new CloseColorFinder(GetDefaultDefinitions().ToArray());
        }

        private IEnumerable<KeyValuePair<ConsoleColor, Color>> GetDefaultDefinitions()
        {
            // TODO: Provide switch to turn old colors set (now being alternate...)
            return this._defaultSet;
        }

        public CloseColorFinder CustomizedDefault(params Tuple<ConsoleColor, Color>[] overwrites)
        {
            Condition.Requires(overwrites, nameof(overwrites))
                .IsNotNull()
                .IsNotEmpty();

            var dict = GetDefaultDefinitions().ToDictionary(p => p.Key, p => p.Value);
            foreach (var overwrite in overwrites)
            {
                dict[overwrite.Item1] = overwrite.Item2;
            }

            return new CloseColorFinder(dict.ToArray());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._disposed = true;
        }
    }
}