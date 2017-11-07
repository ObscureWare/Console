// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameStyle.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2016 Sebastian Gruchacz
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
//   Defines the FrameStyle class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Operations.Interfaces.Styles
{
    using ObscureWare.Console.Root.Interfaces;

    /// <summary>
    /// Definition of on-screen frame
    /// </summary>
    public class FrameStyle
    {
        internal enum FramePiece : byte
        {
            TopLeft = 0,
            Top,
            TopRight,
            Left,
            Right,
            BottomLeft,
            Bottom,
            BottomRight
        }

        private readonly char[] _frameChars;

        public FrameStyle(ConsoleFontColor frameColor, ConsoleFontColor textColor, string frameChars, char backgroundFiller)
        {
            this.FrameColor = frameColor;
            this.TextColor = textColor;
            this.BackgroundFiller = backgroundFiller;
            this._frameChars = frameChars.ToCharArray();

            // TODO: Performance-wise - read all these chars only once, in the ctor. Can be refactored at any time - does not alter interface.
        }

        public ConsoleFontColor FrameColor { get; private set; }

        public ConsoleFontColor TextColor { get; private set; }

        public char BackgroundFiller { get; private set; }

        public char TopLeft => this._frameChars[(byte)FramePiece.TopLeft];

        public char Top => this._frameChars[(byte)FramePiece.Top];

        public char TopRight => this._frameChars[(byte)FramePiece.TopRight];

        public char Left => this._frameChars[(byte)FramePiece.Left];

        public char Right => this._frameChars[(byte)FramePiece.Right];

        public char BottomLeft => this._frameChars[(byte)FramePiece.BottomLeft];

        public char Bottom => this._frameChars[(byte)FramePiece.Bottom];

        public char BottomRight => this._frameChars[(byte)FramePiece.BottomRight];
    }
}