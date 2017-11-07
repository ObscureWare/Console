// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleOperations.cs" company="Obscureware Solutions">
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
//   Defines the ConsoleOperations class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Interfaces
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Shared;
    using ObscureWare.Console.Operations.Interfaces.Styles;

    /// <summary>
    /// TODO: totally refactor this into pieces...
    /// </summary>
    public class ConsoleOperations
    {
        private readonly IConsole _console;

        public ConsoleOperations(IConsole console)
        {
            this._console = console;
        }

        public bool WriteTextBox(Rectangle textArea, string text, FrameStyle frameDef)
        {
            if (textArea.Width <= 0 || textArea.Height <= 0)
            {
                throw new ArgumentException("Rectangle must have reasonable ( >0 ) dimensions.", nameof(textArea));
            }

            uint boxWidth = (uint)textArea.Width;
            uint boxHeight = (uint)textArea.Height;
            this.LimitBoxDimensions(textArea.X, textArea.Y, ref boxWidth, ref boxHeight);
            Debug.Assert(boxWidth >= 3, "boxWidth >= 3");
            Debug.Assert(boxHeight >= 3, "boxHeight >= 3");
            this.WriteTextBoxFrame(textArea.X, textArea.Y, (int)boxWidth, (int)boxHeight, frameDef);
            return this.WriteTextBox(textArea.X + 1, textArea.Y + 1, boxWidth - 2, boxHeight - 2, text, frameDef.TextColor);
        }

        private void WriteTextBoxFrame(int boxX, int boxY, int boxWidth, int boxHeight, FrameStyle frameDef)
        {
            this._console.SetColors(frameDef.FrameColor.ForeColor, frameDef.FrameColor.BgColor);
            this._console.SetCursorPosition(boxX, boxY);
            this._console.WriteText(frameDef.TopLeft);
            for (int i = 1; i < boxWidth - 1; i++)
            {
                this._console.WriteText(frameDef.Top);
            }
            this._console.WriteText(frameDef.TopRight);
            string body = frameDef.Left + new string(frameDef.BackgroundFiller, boxWidth - 2) + frameDef.Right;
            for (int j = 1; j < boxHeight - 1; j++)
            {
                this._console.SetCursorPosition(boxX, boxY + j);
                this._console.WriteText(body);
            }
            this._console.SetCursorPosition(boxX, boxY + boxHeight - 1);
            this._console.WriteText(frameDef.BottomLeft);
            for (int i = 1; i < boxWidth - 1; i++)
            {
                this._console.WriteText(frameDef.Bottom);
            }
            this._console.WriteText(frameDef.BottomRight);
        }

        public bool WriteTextBox(Rectangle textArea, string text, ConsoleFontColor colorDef)
        {
            return this.WriteTextBox(textArea.X, textArea.Y, (uint)textArea.Width, (uint)textArea.Height, text, colorDef);
        }

        public bool WriteTextBox(int x, int y, uint boxWidth, uint boxHeight, string text, ConsoleFontColor colorDef)
        {
            this.LimitBoxDimensions(x, y, ref boxWidth, ref boxHeight); // so do not have to check for this every line is drawn...
            this._console.SetCursorPosition(x, y);
            this._console.SetColors(colorDef.ForeColor, colorDef.BgColor);

            string[] lines = text.SplitTextToFit(boxWidth).ToArray();
            int i;
            for (i = 0; i < lines.Length && i < boxHeight; ++i)
            {
                this._console.SetCursorPosition(x, y + i);
                this.WriteTextJustified(lines[i], boxWidth);
            }

            return i == lines.Length;
        }

        /// <summary>
        /// Write text justified.
        /// </summary>
        /// <param name="text">The to be written.</param>
        /// <param name="boxWidth">Available area.</param>
        private void WriteTextJustified(string text, uint boxWidth)
        {
            if (text.Length == boxWidth)
            {
                System.Console.Write(text); // text that already spans whole box does not need justification
            }
            else
            {
                string[] parts = text.Split(new[] { @" ", @"\t" }, StringSplitOptions.RemoveEmptyEntries); // both split and clean
                if (parts.Length == 1)
                {
                    System.Console.Write(text); // we cannot do anything about one long word...
                }
                else
                {
                    uint cleanedLength = (uint)(parts.Select(s => s.Length).Sum() + parts.Length - 1);
                    uint remainingBlanks = boxWidth - cleanedLength;
                    if (remainingBlanks > cleanedLength / 2)
                    {
                        System.Console.Write(text); // text is way too short to expand it, keep it to the left
                    }
                    else
                    {
                        int longerSpacesCount = (int)Math.Floor((decimal)remainingBlanks / (parts.Length - 1));
                        if (longerSpacesCount > 1)
                        {
                            decimal remainingLowerSpacesJoins = remainingBlanks - (longerSpacesCount * (parts.Length - 1));
                            if (remainingLowerSpacesJoins > 0)
                            {
                                int longerQty = parts.Length - longerSpacesCount;
                                System.Console.Write(
                                    string.Join(new string(' ', longerSpacesCount), parts.Take(longerQty + 1)) +
                                    string.Join(new string(' ', longerSpacesCount - 1), parts.Skip(longerQty + 1)));
                            }
                            else
                            {
                                // all gaps equal
                                System.Console.Write(string.Join(new string(' ', longerSpacesCount), parts));
                            }
                        }
                        else
                        {
                            System.Console.Write(
                                string.Join(new string(' ', 2), parts.Take((int)(remainingBlanks + 1))) +
                                string.Join(new string(' ', 1), parts.Skip((int)(remainingBlanks + 1))));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Limits box dimensions to actual window sizes (avoid overlapping AND exceptions...)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void LimitBoxDimensions(int x, int y, ref uint width, ref uint height)
        {
            if (x + width > this._console.WindowWidth)
            {
                width = (uint)Math.Max(0, this._console.WindowWidth - x);
            }

            if (y + height > this._console.WindowHeight)
            {
                height = (uint)Math.Max(0, this._console.WindowHeight - y);
            }
        }
    }
}
