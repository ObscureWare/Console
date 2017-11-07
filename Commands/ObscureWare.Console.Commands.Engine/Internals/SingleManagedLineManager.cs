// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleManagedLineManager.cs" company="Obscureware Solutions">
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
//   Defines SingleManagedLineManager class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Linq;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Root.Interfaces;

    /// <summary>
    /// The single managed line manager.
    /// </summary>
    internal class SingleManagedLineManager : IOutputLineManager
    {
        private readonly IConsole consoleInstance;

        private readonly int lineY;

        private readonly int windowWidth;

        private int currentPosition = 0;

        private readonly string emptifier;

        public SingleManagedLineManager(IConsole consoleInstance, int lineY, int windowWidth)
        {
            if (consoleInstance == null)
            {
                throw new ArgumentNullException(nameof(consoleInstance));
            }
            if (windowWidth < 10)
            {
                throw new ArgumentException("Window is too narrow to use with Lines Manager.", nameof(windowWidth));
            }

            this.consoleInstance = consoleInstance;
            this.lineY = lineY;
            this.windowWidth = windowWidth;
            this.emptifier = new string(' ', windowWidth);
        }

        /// <summary>
        /// Gets the max length.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this.windowWidth;
            }
        }

        /// <summary>
        /// Gets the line number.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.lineY;
            }
        }

        /// <summary>
        /// The write text.
        /// </summary>
        /// <param name="style">
        /// The style.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public void WriteText(ConsoleFontColor style, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.ToCharArray().Any(ch => (int)ch < 32))
            {
                throw new ArgumentException("Messages that contain special characters are forbidden.", paramName: nameof(text));
            }

            lock (this.consoleInstance.AtomicHandle)
            {
                var backupPosition = this.consoleInstance.GetCursorPosition();

                int remainingArea = this.windowWidth - this.currentPosition;
                if (remainingArea > text.Length)
                {
                    this.consoleInstance.SetCursorPosition(this.currentPosition, this.lineY);
                    this.consoleInstance.WriteText(style, text);
                    this.currentPosition += text.Length;
                }
                else
                {
                    if (remainingArea > 0)
                    {
                        this.consoleInstance.SetCursorPosition(this.currentPosition, this.lineY);
                        this.consoleInstance.WriteText(style, text.Substring(0, remainingArea));
                        this.currentPosition += remainingArea;
                    }
                }

                this.consoleInstance.SetCursorPosition(backupPosition.X, backupPosition.Y);
            }
        }

        public void Clear()
        {
            lock (this.consoleInstance.AtomicHandle)
            {
                var backupPosition = this.consoleInstance.GetCursorPosition();

                // TODO: pass colors? As parameter?

                // now using default color...
                this.consoleInstance.SetCursorPosition(0, this.lineY);
                this.consoleInstance.WriteText(this.emptifier);
                this.consoleInstance.SetCursorPosition(0, this.lineY);
                this.currentPosition = 0;

                this.consoleInstance.SetCursorPosition(backupPosition.X, backupPosition.Y);
            }
        }
    }
}