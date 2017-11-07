// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagPropertyParserTableStyle.cs" company="Obscureware Solutions">
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
//   Defines the TableStyle class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Interfaces.Styles
{
    using ObscureWare.Console.Root.Interfaces;

    public class TableStyle : ICoreTableStyle
    {
        internal enum TablePiece : byte
        {
            TopLeft = 0,
            Top,
            TopRight,
            Left,
            Right,
            BottomLeft,
            Bottom,
            BottomRight,
            HeaderSeparatorWithFrame,
            HeaderSeparatorWithoutFrame,
            ColumnsSeparator,
            TopConnector,
            BottomConnector
        }

        private readonly char[] _frameChars;

        public TableStyle(ConsoleFontColor frameColor, ConsoleFontColor headerColor, ConsoleFontColor rowColor, ConsoleFontColor evenRowColor,
            string frameChars, char backgroundFiller, TableOverflowContentBehavior overflowBehaviour)
        {
            this.FrameColor = frameColor;
            this.HeaderColor = headerColor;
            this.RowColor = rowColor;
            this.EvenRowColor = evenRowColor;
            this.BackgroundFiller = backgroundFiller;
            this.OverflowBehaviour = overflowBehaviour;
            this._frameChars = frameChars.ToCharArray();
        }

        public ConsoleFontColor FrameColor { get; private set; }
        public ConsoleFontColor HeaderColor { get; private set; }
        public ConsoleFontColor RowColor { get; private set; }
        public ConsoleFontColor EvenRowColor { get; private set; }

        public char BackgroundFiller { get; private set; }

        public TableOverflowContentBehavior OverflowBehaviour { get; private set; }

        public char TopLeft => this._frameChars[(byte)TablePiece.TopLeft];

        public char Top => this._frameChars[(byte)TablePiece.Top];

        public char TopRight => this._frameChars[(byte)TablePiece.TopRight];

        public char Left => this._frameChars[(byte)TablePiece.Left];

        public char Right => this._frameChars[(byte)TablePiece.Right];

        public char BottomLeft => this._frameChars[(byte)TablePiece.BottomLeft];

        public char Bottom => this._frameChars[(byte)TablePiece.Bottom];

        public char BottomRight => this._frameChars[(byte)TablePiece.BottomRight];

        public char HeaderSeparatorFrame => this._frameChars[(byte)TablePiece.HeaderSeparatorWithFrame];

        public char HeaderSeparatorCell => this._frameChars[(byte)TablePiece.HeaderSeparatorWithoutFrame];

        public char ColumnSeparator => this._frameChars[(byte)TablePiece.ColumnsSeparator];

        public char TopConnector => this._frameChars[(byte)TablePiece.TopConnector];

        public char BottomConnector => this._frameChars[(byte)TablePiece.BottomConnector];

        /// <inheritdoc />
        public bool ShowHeader { get; } = true;

        /// <inheritdoc />
        public bool AtomicPrinting { get; } = true;
    }
}