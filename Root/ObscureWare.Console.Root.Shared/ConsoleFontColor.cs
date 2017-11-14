// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleFontColor.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2017 Sebastian Gruchacz
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
//   Defines the structure used to store color info for console operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Root.Shared
{
    using System.Drawing;

    /// <summary>
    /// Handy, immutable structure to remember settings of special-case color pairs
    /// </summary>
    public struct ConsoleFontColor
    {
        /// <summary>
        /// Initializes new instance of ConsoleFontColor tuple
        /// </summary>
        /// <param name="foreColor"></param>
        /// <param name="bgColor"></param>
        public ConsoleFontColor(Color foreColor, Color bgColor)
        {
            this.BgColor = bgColor;
            this.ForeColor = foreColor;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForeColor { get; private set; }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BgColor { get; private set; }
    }
}