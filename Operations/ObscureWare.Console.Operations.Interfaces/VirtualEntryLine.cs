// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualEntryLine.cs" company="Obscureware Solutions">
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
//   Defines the VirtualEntryLine class that simulates command line behavior.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using Conditions;
    using ObscureWare.Console.Root.Interfaces;

    /// <summary>
    /// In order to use auto-completion I must simulate more or less all other expected behavior of console editing.
    /// </summary>
    /// <remarks>I'm gonna need this anyway for graphical console implementation in the future... So maybe better implement this correctly already...
    /// TODO: Anyway, try targeting both stateless and state-full console (most work on the Commands library though)..</remarks>
    public class VirtualEntryLine
    {
        private const int MAX_COMMAND_LENGTH = 2048; // more?! no problem, but what for?
        private readonly IConsole _console;
        private readonly ConsoleFontColor _cmdColor;
        private readonly List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;
        private bool _overWriting = false;

        public VirtualEntryLine(IConsole console, ConsoleFontColor cmdColor)
        {
            console.Requires(nameof(console)).IsNotNull();

            this._console = console;
            this._cmdColor = cmdColor;
        }

        // TODO: this method is monstrosity, but it rather not be wise to just cut it into pieces... It's slow enough...
        // TODO: make this configurable, especially behavior of UP/DOWND arrows might not be intuitive in some environments
        public string GetUserEntry(IAutoComplete acProvider)
        {
            acProvider.Requires(nameof(acProvider)).IsNotNull();

            var startPosition = this._console.GetCursorPosition();
            var consoleLineWidth = this._console.WindowWidth;
            var longestLineContentSoFar = 0;

            int currentCommandEndIndex = 0;
            char[] commandBuffer = new char[MAX_COMMAND_LENGTH];

            string[] autocompleteList = null;
            int autocompleteIndex = 0;

            while (true)
            {
                ConsoleKeyInfo key = this._console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    if (currentCommandEndIndex >= 0)
                    {
                        this._console.WriteLine();
                        string cmdContent = new string(commandBuffer, 0, currentCommandEndIndex);
                        if (!string.IsNullOrWhiteSpace(cmdContent))
                        {
                            // remember only new entries (on last position)
                            if (!this._commandHistory.Any() || !this._commandHistory.Last().Equals(cmdContent))
                            {
                                this._commandHistory.Add(cmdContent);
                                this._historyIndex = this._commandHistory.Count - 1;
                            }
                        }
                        return cmdContent;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                if (key.Key == ConsoleKey.Tab)
                {
                    // TODO: hide/ show cursor her? probably not required

                    if (autocompleteList == null)
                    {
                        autocompleteList = acProvider.MatchAutoComplete(new string(commandBuffer, 0, currentCommandEndIndex)).ToArray();
                        autocompleteIndex = -1;
                    }

                    if (autocompleteList.Any())
                    {
                        // reverse?
                        if ((key.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
                        {
                            autocompleteIndex--;
                            if (autocompleteIndex < 0)
                            {
                                autocompleteIndex = autocompleteList.Length - 1;
                            }
                        }
                        else
                        {
                            autocompleteIndex++;
                            if (autocompleteIndex >= autocompleteList.Length)
                            {
                                autocompleteIndex = 0;
                            }
                        }

                        string acText = autocompleteList[autocompleteIndex];
                        this.ApplyText(acText, this._console, commandBuffer, startPosition, ref longestLineContentSoFar, ref currentCommandEndIndex);
                    }
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (currentCommandEndIndex > 0)
                    {
                        try
                        {
                            this._console.HideCursor();

                            var currentPosition = this._console.GetCursorPosition();
                            int lineIndex = this.CalculatePositionInLine(startPosition, currentPosition, consoleLineWidth);
                            if (lineIndex > 0)
                            {
                                this.RemoveCharsAt(commandBuffer, lineIndex - 1, 1, ref currentCommandEndIndex);
                                this._console.SetCursorPosition(startPosition.X, startPosition.Y);
                                this._console.WriteText(this._cmdColor, new string(' ', currentCommandEndIndex + 1));

                                if (currentCommandEndIndex >= 0)
                                {
                                    this._console.SetCursorPosition(startPosition.X, startPosition.Y);
                                    this._console.WriteText(this._cmdColor, new string(commandBuffer, 0, currentCommandEndIndex));

                                    this._console.SetCursorPosition(currentPosition.X - 1, currentPosition.Y);
                                }
                            }
                        }
                        finally
                        {
                            this._console.ShowCursor();
                        }
                    }
                }
                else if (key.Key == ConsoleKey.Delete)
                {
                    if (currentCommandEndIndex >= 0)
                    {
                        try
                        {
                            this._console.HideCursor();

                            var currentPosition = this._console.GetCursorPosition();
                            int lineIndex = this.CalculatePositionInLine(startPosition, currentPosition, consoleLineWidth);
                            if (lineIndex >= 0 && lineIndex < currentCommandEndIndex)
                            {
                                this.RemoveCharsAt(commandBuffer, lineIndex, 1, ref currentCommandEndIndex);
                                this._console.SetCursorPosition(startPosition.X, startPosition.Y);
                                this._console.WriteText(this._cmdColor, new string(' ', currentCommandEndIndex + 1));

                                if (currentCommandEndIndex >= 0)
                                {
                                    this._console.SetCursorPosition(startPosition.X, startPosition.Y);
                                    this._console.WriteText(this._cmdColor, new string(commandBuffer, 0, currentCommandEndIndex));

                                    this._console.SetCursorPosition(currentPosition.X, currentPosition.Y);
                                }
                            }
                        }
                        finally
                        {
                            this._console.ShowCursor();
                        }
                    }
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    // TODO: multiline fix
                    var currentPosition = this._console.GetCursorPosition();
                    var deltaX = currentPosition.X - startPosition.X;
                    if (deltaX > 0)
                    {

                        this._console.SetCursorPosition(currentPosition.X - 1, startPosition.Y);
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    // TODO: multiline fix
                    var currentPosition = this._console.GetCursorPosition();
                    var deltaX = currentPosition.X - startPosition.X;
                    if (deltaX < currentCommandEndIndex)
                    {
                        this._console.SetCursorPosition(currentPosition.X + 1, startPosition.Y);
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    // TODO: multiline fix
                    // TODO:  move cursor between lines
                    // TODO: is it possible to do text selection with SHIFT?
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    // TODO: multiline fix
                    // TODO: move cursor between lines
                    // TODO: is it possible to do text selection with SHIFT?
                }
                else if (key.Key == ConsoleKey.Insert)
                {
                    if ((key.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
                    {
                        var currentPosition = this._console.GetCursorPosition();
                        int lineIndex = this.CalculatePositionInLine(startPosition, currentPosition, consoleLineWidth);
                        currentCommandEndIndex = this.PasteFromClipboard(commandBuffer, lineIndex, currentCommandEndIndex);
                    }
                    else
                    {
                        // switch insert mode
                        this._overWriting = !this._overWriting;
                    }
                }
                else if (key.Key == ConsoleKey.End)
                {
                    var endOfline = this.CalculateCursorPositionForLineIndex(startPosition, consoleLineWidth, currentCommandEndIndex);
                    this._console.SetCursorPosition(endOfline.X, endOfline.Y);
                }
                else if (key.Key == ConsoleKey.Home)
                {
                    this._console.SetCursorPosition(startPosition.X, startPosition.Y);
                }
                else if (key.Key == ConsoleKey.PageUp)
                {
                    if (this._historyIndex < this._commandHistory.Count && this._historyIndex >= 0)
                    {
                        string historyEntry = this._commandHistory[this._historyIndex--];
                        // looping?
                        //if (historyIndex < 0)
                        //{
                        //    historyIndex = this._commandHistory.Count - 1;
                        //}

                        this.ApplyText(historyEntry, this._console, commandBuffer, startPosition, ref longestLineContentSoFar, ref currentCommandEndIndex);
                    }
                }
                else if (key.Key == ConsoleKey.PageDown)
                {
                    if (this._historyIndex < this._commandHistory.Count - 1 && this._historyIndex >= -1)
                    {
                        string historyEntry = this._commandHistory[++this._historyIndex];
                        // looping?
                        //if (historyIndex >= this._commandHistory.Count)
                        //{
                        //    historyIndex = 0;
                        //}

                        this.ApplyText(historyEntry, this._console, commandBuffer, startPosition, ref longestLineContentSoFar, ref currentCommandEndIndex);
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    // do nothing
                    // TODO: or reset command-line?
                }
                else if (key.KeyChar != 0)
                {
                    var currentPosition = this._console.GetCursorPosition();
                    int lineIndex = this.CalculatePositionInLine(startPosition, currentPosition, consoleLineWidth);

                    if ((key.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control &&
                        key.Key == ConsoleKey.V)
                    {
                        currentCommandEndIndex = this.PasteFromClipboard(commandBuffer, lineIndex, currentCommandEndIndex);
                    }
                    else
                    {
                        if (currentCommandEndIndex < MAX_COMMAND_LENGTH)
                        {
                            if (lineIndex == currentCommandEndIndex)
                            {
                                // append at the end - in both INSERT modes the same
                                commandBuffer[currentCommandEndIndex] = key.KeyChar;
                                currentCommandEndIndex += 1;
                            }
                            else if (this._overWriting)
                            {
                                // overwrite in the middle
                                commandBuffer[lineIndex] = key.KeyChar;
                            }
                            else
                            {
                                // insert in the middle
                                var usedBufferLength = currentCommandEndIndex + 1;
                                this.InsertCharsAt(commandBuffer, lineIndex, new[] { key.KeyChar }, ref usedBufferLength);
                                currentCommandEndIndex++;
                            }

                            try
                            {
                                this._console.HideCursor();

                                this._console.SetCursorPosition(startPosition.X, startPosition.Y);
                                this._console.SetColors(this._cmdColor);
                                this._console.WriteText(new string(commandBuffer, 0, currentCommandEndIndex));
                                if (currentPosition.X < consoleLineWidth)
                                {
                                    this._console.SetCursorPosition(currentPosition.X + 1, currentPosition.Y);
                                }
                                else
                                {
                                    this._console.SetCursorPosition(0, currentPosition.Y + 1);
                                }
                                autocompleteList = null;
                            }
                            finally
                            {
                                this._console.ShowCursor();
                            }
                        }
                    }
                }
                else
                {
                    // delete, esc
                }
            }
        }

        private int PasteFromClipboard(char[] commandBuffer, int lineIndex, int currentCommandEndIndex)
        {
            var valueToPaste = this.GetCleanValueFromClipBoard().ToCharArray();
            if (valueToPaste.Any())
            {
                var usedBufferLength = currentCommandEndIndex + 1;
                this.InsertCharsAt(commandBuffer, lineIndex, valueToPaste, ref usedBufferLength);
                currentCommandEndIndex += valueToPaste.Length;
            }
            return currentCommandEndIndex;
        }

        private string GetCleanValueFromClipBoard()
        {
            var txt = Clipboard.GetText().ToCharArray();
            StringBuilder result = new StringBuilder(txt.Length);

            foreach (char c in txt)
            {
                if (!c.IsSystemChar())
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Calculates real position of cursor in multi-line console line
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="currentPosition"></param>
        /// <param name="consoleLineWidth"></param>
        /// <returns></returns>
        internal int CalculatePositionInLine(Point startPosition, Point currentPosition, int consoleLineWidth)
        {
            return (currentPosition.Y - startPosition.Y) * consoleLineWidth + currentPosition.X - startPosition.X;
        }

        /// <summary>
        /// Calculates where shall put cursor for specific index of multi-lined console line
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="consoleLineWidth"></param>
        /// <param name="targetIndex"></param>
        /// <returns></returns>
        internal Point CalculateCursorPositionForLineIndex(Point startPosition, int consoleLineWidth, int targetIndex)
        {
            int fullLines = ((targetIndex + startPosition.X) / consoleLineWidth);
            return new Point(
                (targetIndex - (fullLines * consoleLineWidth) + startPosition.X),
                fullLines + startPosition.Y);
        }

        private void ApplyText(string text, IConsole console, char[] commandBuffer, Point startPosition,
            ref int lineContentSoFar, ref int currentCommandEndIndex)
        {
            try
            {
                this._console.HideCursor();

                var textLength = text.Length;
                currentCommandEndIndex = textLength;
                lineContentSoFar = Math.Max(lineContentSoFar, textLength);
                text.ToCharArray().CopyTo(commandBuffer, 0);
                console.SetCursorPosition(startPosition.X, startPosition.Y);
                console.WriteText(this._cmdColor, new string(' ', lineContentSoFar));
                console.SetCursorPosition(startPosition.X, startPosition.Y);
                console.WriteText(this._cmdColor, text);
            }
            finally
            {
                this._console.ShowCursor();
            }
        }

        internal void RemoveCharsAt(char[] buffer, int from, int qty, ref int currentCommandEndIndex)
        {
            for (int i = from; i < currentCommandEndIndex - qty && i < buffer.Length && i >= 0; i++)
            {
                buffer[i] = buffer[i + qty];
            }

            currentCommandEndIndex -= qty;
        }

        internal void InsertCharsAt(char[] buffer, int from, char[] newChars, ref int bufferUsedLength)
        {
            if (bufferUsedLength + newChars.Length >= buffer.Length)
            {
                throw new ArgumentException("Buffer is not large enough to store requested sub-buffer.", nameof(buffer));
            }

            int newLen = newChars.Length;
            for (int i = bufferUsedLength - 1; i <= buffer.Length + newLen && i >= from; i--)
            {
                buffer[i + newLen] = buffer[i];
            }

            for (int i = 0; i < newLen && i < buffer.Length; i++)
            {
                buffer[from + i] = newChars[i];
            }

            bufferUsedLength += newLen;
        }
    }
}
