// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemConsole.cs" company="Obscureware Solutions">
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
//   Defines the core SystemConsole wrapper on SystemConsole that implements IConsole interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Root.Desktop
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using Conditions;

    using ObscureWare.Console.Root.Shared;

    /// <summary>
    /// Wraps System.Console with IConsole interface methods
    /// </summary>
    public class SystemConsole : IConsole
    {
        private readonly ConsoleController _controller;

        /// <summary>
        /// In characters...
        /// </summary>
        public Point WindowSize { get; }

        /// <summary>
        /// The most safe constructor - uses default window and buffer sizes
        /// </summary>
        /// <param name="controller"></param>
        public SystemConsole(ConsoleController controller)
        {
            controller.Requires(nameof(controller)).IsNotNull();

            this._controller = controller;

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            this.WindowWidth = Console.WindowWidth;
            this.WindowHeight = Console.WindowHeight;

            this.TryTurningVirtualConsoleMode();
        }

        /// <summary>
        /// Full-screen constructor, when flag value is TRUE. If no - it's just constructs 120x40 window with 120x500 buffer (larger than default).
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="isFullScreen"></param>
        public SystemConsole(ConsoleController controller, bool isFullScreen) : this(controller)
        {
            if (isFullScreen)
            {
                this.SetConsoleWindowToFullScreen();

                // now can calculate how large could be full-screen buffer

                // SG: (-2) On Win8 the only working way to keep borders on the screen :(
                // (-1) required on Win10 though :(
                this.WindowSize = new Point(Console.LargestWindowWidth - 2, Console.LargestWindowHeight - 1);

                // setting full-screen
                Console.BufferWidth = this.WindowSize.X;
                Console.WindowWidth = this.WindowSize.X;
                Console.BufferHeight = this.WindowSize.Y;
                Console.WindowHeight = this.WindowSize.Y;
                Console.SetWindowPosition(0, 0);
            }
            else
            {
                if (!this.IsExcutedAsChild)
                {
                    // set console (buffer) little bigger by default (not when child process of cmd...)
                    // TODO: improve this code in case of already configured larger window...
                    Console.BufferWidth = 120;
                    Console.BufferHeight = 500;
                    Console.WindowWidth = 120;
                    Console.WindowHeight = 40;
                }
            }

            this.WindowWidth = Console.WindowWidth;
            this.WindowHeight = Console.WindowHeight;

            this.TryTurningVirtualConsoleMode();
        }

        public bool IsExcutedAsChild
        {
            get
            {
                try
                {
                    var parentprocess = ParentProcessUtilities.GetParentProcess();
                    return parentprocess?.MainWindowTitle.Contains("cmd.exe") ?? false;
                }
                catch
                {
                    return true; // safe assumption in case of failure ( API changed) - do not resize.
                }
            }
        }

        /// <summary>
        /// Custom sized console
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="bufferSize">Width and height of the buffer. Must be >= window size. Will be resized automatically if not fits...</param>
        /// <param name="windowSize">Width and height of the window (In columns and rows, depends of font and screen resolution!). Cannot exceed screen size. Will be automatically trimmed.</param>
        public SystemConsole(ConsoleController controller, Point bufferSize, Point windowSize) : this(controller)
        {
            windowSize.X = Math.Min(windowSize.X, Console.LargestWindowWidth - 2);
            windowSize.Y = Math.Min(windowSize.Y, Console.LargestWindowHeight - 1);
            bufferSize.X = Math.Max(bufferSize.X, windowSize.X);
            bufferSize.Y = Math.Max(bufferSize.Y, windowSize.Y);

            Console.BufferWidth = bufferSize.X;
            Console.WindowWidth = windowSize.X;
            Console.BufferHeight = bufferSize.Y;
            Console.WindowHeight = windowSize.Y;

            this.WindowWidth = Console.WindowWidth;
            this.WindowHeight = Console.WindowHeight;

            this.TryTurningVirtualConsoleMode();
        }

        private void SetConsoleWindowToFullScreen()
        {
            // http://www.codeproject.com/Articles/4426/Console-Enhancements
            this.SetWindowPosition(
                0,
                0,
                Screen.PrimaryScreen.WorkingArea.Width - (2 * 16),
                Screen.PrimaryScreen.WorkingArea.Height - (2 * 16) - SystemInformation.CaptionHeight);
        }

        /// <inheritdoc />
        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {
            this.SetCursorPosition(x, y);
            if (this.VirtualConsoleEnabled)
            {
                Console.Write($"\x1b[38;2;{foreColor.R};{foreColor.G};{foreColor.B};48;2;{bgColor.R};{bgColor.G};{bgColor.B}m{text}");
            }
            else
            {
                this.SetColors(foreColor, bgColor);
                this.WriteText(text);
            }
        }

        /// <inheritdoc />
        private void WriteText(string text)
        {
            Console.Write(text);
        }

        /// <inheritdoc />
        void IConsole.WriteText(string text)
        {
            this.WriteText(text);
        }

        /// <inheritdoc />
        public void WriteLine(ConsoleFontColor colors, string text) // TODO: add flag to keep BG color to next line
        {
            this.SetColors(colors.ForeColor, colors.BgColor);
            Console.Write(text);
            Console.ResetColor();
            // this.SetColors(colors.ForeColor, Color.Black); // try resetting BG color...
            Console.WriteLine();
        }

        /// <inheritdoc />
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc />
        public void SetColors(Color foreColor, Color bgColor)
        {
            if (this.VirtualConsoleEnabled)
            {
                Console.Write($"\x1b[38;2;{foreColor.R};{foreColor.G};{foreColor.B};48;2;{bgColor.R};{bgColor.G};{bgColor.B}m");
            }
            else
            {
                Console.ForegroundColor = this._controller.CloseColorFinder.FindClosestColor(foreColor);
                Console.BackgroundColor = this._controller.CloseColorFinder.FindClosestColor(bgColor);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public void WriteText(ConsoleFontColor colors, string text)
        {
            if (this.VirtualConsoleEnabled)
            {
                Console.Write($"\x1b[38;2;{colors.ForeColor.R};{colors.ForeColor.G};{colors.ForeColor.B};48;2;{colors.BgColor.R};{colors.BgColor.G};{colors.BgColor.B}m{text}");
            }
            else
            {
                this.SetColors(colors.ForeColor, colors.BgColor);
                Console.Write(text);
            }
        }

        /// <inheritdoc />
        public void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        /// <inheritdoc />
        public Point GetCursorPosition()
        {
            return new Point(Console.CursorLeft, Console.CursorTop);
        }

        /// <inheritdoc />
        public void WriteText(char character)
        {
            Console.Write(character);
        }

        /// <inheritdoc />
        public int WindowHeight { get; }

        /// <inheritdoc />
        public int WindowWidth { get; }

        /// <inheritdoc />
        public ConsoleMode ConsoleMode => ConsoleMode.Buffered;

        /// <inheritdoc />
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(intercept: true);
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <inheritdoc />
        public void SetColors(ConsoleFontColor style)
        {
            this.SetColors(style.ForeColor, style.BgColor);
        }

        /// <inheritdoc />
        public object AtomicHandle => Console.Out; //https://github.com/dotnet/corefx/issues/2808

        /// <inheritdoc />
        public void ReplaceConsoleColor(ConsoleColor color, Color rgbColor)
        {
            this._controller.ReplaceConsoleColor(color, rgbColor);
        }

        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public void SetWindowPosition(int x, int y, int width, int height)
        {
            IntPtr hwnd = NativeMethods.GetConsoleWindow();
            NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, x, y, width, height, NativeMethods.SWP_NOZORDER | NativeMethods.SWP_NOACTIVATE);
            // no release handle?
        }

        /// <inheritdoc />
        public void HideCursor()
        {
            System.Console.CursorVisible = false;
        }

        /// <inheritdoc />
        public void ShowCursor()
        {
            System.Console.CursorVisible = true;
        }

        public void TryTurningVirtualConsoleMode()
        {
            this.VirtualConsoleEnabled = false;

            var hConsoleOutput = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE); // 7
            if (hConsoleOutput == NativeMethods.INVALID_HANDLE)
            {
                throw new SystemException("GetStdHandle->WinError: #" + Marshal.GetLastWin32Error());
            }

            uint dwMode;
            if (!NativeMethods.GetConsoleMode(hConsoleOutput, out dwMode))
            {
                throw new SystemException("GetStdHandle->WinError: #" + Marshal.GetLastWin32Error());
            }

            if ((dwMode & (uint)ConsoleOutputModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING) == (uint)ConsoleOutputModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING)
            {
                this.VirtualConsoleEnabled = true;
            }
            dwMode |= (uint)ConsoleOutputModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            if (!NativeMethods.SetConsoleMode(hConsoleOutput, dwMode))
            {
                // TODO: warn NativeMethods.GetLastError();
                return;
            }

            this.VirtualConsoleEnabled = true;
        }

        public bool VirtualConsoleEnabled { get; private set; }
    }
}