// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleController.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2015-2016 Sebastian Gruchacz
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
//   Provides routines used to manipulate console colors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Root.Desktop
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using Conditions;

    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Root.Shared.ColorBalancing;

    /// <summary>
    /// Class used to control basic system's console behavior
    /// </summary>
    public class ConsoleController : IDisposable
    {
        private static readonly IntPtr HConsoleOutput;

        static ConsoleController()
        {
            if (HConsoleOutput == IntPtr.Zero)
            {
                HConsoleOutput = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE); // 7
                if (HConsoleOutput == NativeMethods.INVALID_HANDLE)
                {
                    throw new SystemException("GetStdHandle->WinError: #" + Marshal.GetLastWin32Error());
                }
            }

            AppDomain.CurrentDomain.ProcessExit += StaticClass_Dtor;
        }

        private ColorBalancer _closeColorFinder;

        // https://blogs.msdn.microsoft.com/commandline/2016/09/22/24-bit-color-in-the-windows-console/
        // https://github.com/bitcrazed/24bit-color
        // TODO: use VT Sequences?

        /// <summary>
        /// Initializes new instance of ConsoleColorsHelper class
        /// </summary>
        public ConsoleController()
        {
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

            // TODO: second instance created is crashing. Find out why and how to fix it / prevent. In the worst case - hidden control instance singleton
            // Not very important, can wait


            this._closeColorFinder = ColorBalancer.Default;
        }

        /// <summary>
        /// Exposes instance of inner <seealso cref="CloseColorFinder"/>
        /// </summary>
        public ColorBalancer CloseColorFinder => this._closeColorFinder;

        /// <summary>
        /// Replaces default (or previous...) values of console colors with new RGB values.
        /// </summary>
        /// <param name="mappings"></param>
        public void ReplaceConsoleColors(params Tuple<ConsoleColor, Color>[] mappings)
        {
            Condition.Requires(mappings, nameof(mappings))
                .IsNotNull()
                .IsNotEmpty();

            var csbe = this.GetConsoleScreenBufferInfoEx();

            foreach (var mapping in mappings)
            {
                SetNewColorDefinition(ref csbe, mapping.Item1, mapping.Item2);
            }

            this.ApplyColorChanges(csbe);
        }

        /// <summary>
        /// Replaces default (or previous...) single value of console color with new RGB value.
        /// </summary>
        /// <param name="color">Console named color</param>
        /// <param name="rgbColor">New RGB value to be used under this color name</param>
        /// <remarks>For multiple colors change rather use <see cref="ReplaceConsoleColors"/> instead!</remarks>
        public void ReplaceConsoleColor(ConsoleColor color, Color rgbColor)
        {
            var csbe = this.GetConsoleScreenBufferInfoEx();

            SetNewColorDefinition(ref csbe, color, rgbColor);

            this.ApplyColorChanges(csbe);
        }

        /// <summary>
        /// Returns current set of colors - what exact RGB is actually used for each Console Color
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<ConsoleColor, Color>[] GetCurrentColorset()
        {
            var csbe = this.GetConsoleScreenBufferInfoEx();

            return new[]
            {
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Black, csbe.black.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkBlue, csbe.darkBlue.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGreen, csbe.darkGreen.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkCyan, csbe.darkCyan.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkRed, csbe.darkRed.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkMagenta, csbe.darkMagenta.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkYellow, csbe.darkYellow.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Gray, csbe.gray.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGray, csbe.darkGray.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Blue, csbe.blue.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Green, csbe.green.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Cyan, csbe.cyan.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Red, csbe.red.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Magenta, csbe.magenta.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Yellow, csbe.yellow.GetColor()),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.White, csbe.white.GetColor()),
            };
        }

        private NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX GetConsoleScreenBufferInfoEx()
        {
            NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = Marshal.SizeOf(csbe); // 96 = 0x60

            bool brc = NativeMethods.GetConsoleScreenBufferInfoEx(HConsoleOutput, ref csbe);
            if (!brc)
            {
                throw new SystemException("GetConsoleScreenBufferInfoEx->WinError: #" + Marshal.GetLastWin32Error());
            }
            return csbe;
        }

        private void ApplyColorChanges(NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX csbe)
        {
            // strange, needs to be done because window is shrunken somehow
            ++csbe.srWindow.Bottom;
            ++csbe.srWindow.Right;

            bool brc = NativeMethods.SetConsoleScreenBufferInfoEx(HConsoleOutput, ref csbe);
            if (!brc)
            {
                throw new SystemException("SetConsoleScreenBufferInfoEx->WinError: #" + Marshal.GetLastWin32Error());
            }

            // TODO: build scheme

            this._closeColorFinder = new ColorBalancer(ColorScheme.FromDefinition("from system", this.GetCurrentColorset()), new GruchenDefaultColorHeuristic());
        }

        private static void SetNewColorDefinition(ref NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX csbe, ConsoleColor color, Color rgbColor)
        {
            // Eh... Ugly code here...

            var r = rgbColor.R;
            var g = rgbColor.G;
            var b = rgbColor.B;

            switch (color)
            {
                case ConsoleColor.Black:
                    csbe.black = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkBlue:
                    csbe.darkBlue = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGreen:
                    csbe.darkGreen = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkCyan:
                    csbe.darkCyan = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkRed:
                    csbe.darkRed = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkMagenta:
                    csbe.darkMagenta = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkYellow:
                    csbe.darkYellow = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Gray:
                    csbe.gray = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGray:
                    csbe.darkGray = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Blue:
                    csbe.blue = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Green:
                    csbe.green = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Cyan:
                    csbe.cyan = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Red:
                    csbe.red = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Magenta:
                    csbe.magenta = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.Yellow:
                    csbe.yellow = new NativeMethods.COLORREF(r, g, b);
                    break;
                case ConsoleColor.White:
                    csbe.white = new NativeMethods.COLORREF(r, g, b);
                    break;
            }
        }

        #region IDsiposable implementation

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ConsoleController"/> class. 
        /// </summary>
        ~ConsoleController()
        {
            // NOTE: Leave out the finalizer altogether if this class doesn't 
            // own unmanaged resources itself, but leave the other methods
            // exactly as they are. 
            this.Dispose(false);
        }

        /// <summary>
        /// Actual disposing method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
        }

        static void StaticClass_Dtor(object sender, EventArgs e)
        {
            // free native resources
            if (HConsoleOutput != NativeMethods.INVALID_HANDLE)
            {
                NativeMethods.CloseHandle(HConsoleOutput);
            }
        }

        #endregion IDsiposable implementation
    }
}