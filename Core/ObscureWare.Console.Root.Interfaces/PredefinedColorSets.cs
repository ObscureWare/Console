// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedColorSets.cs" company="Obscureware Solutions">
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
//   Defines the PredefinedColorSets class with predefined sets of colors
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Root.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public static class PredefinedColorSets
    {
        /// <summary>
        /// Returns default color-set from .Net Console prior to Windows 10 (looks better on old screens)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<ConsoleColor, Color>> DefaultDefinitions()
        {
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Black, Color.FromArgb(0, 0, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.FromArgb(0, 0, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGreen, Color.FromArgb(0, 128, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.FromArgb(0, 128, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkRed, Color.FromArgb(128, 0, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkMagenta, Color.FromArgb(128, 0, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkYellow, Color.FromArgb(128, 128, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Gray, Color.FromArgb(192, 192, 192));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGray, Color.FromArgb(128, 128, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Blue, Color.FromArgb(0, 0, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Green, Color.FromArgb(0, 255, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Cyan, Color.FromArgb(0, 255, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Red, Color.FromArgb(255, 0, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Magenta, Color.FromArgb(255, 0, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Yellow, Color.FromArgb(255, 255, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.White, Color.FromArgb(255, 255, 255));
        }

        /// <summary>
        /// Returns default Windows 10 color-set
        /// </summary>
        /// <remarks>From https://blogs.msdn.microsoft.com/commandline/2017/08/02/updating-the-windows-console-colors/</remarks>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<ConsoleColor, Color>> Windows10Definitions()
        {
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Black, Color.FromArgb(12, 12, 12));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.FromArgb(0, 55, 218));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGreen, Color.FromArgb(19, 161, 14));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.FromArgb(58, 150, 221));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkRed, Color.FromArgb(197, 15, 31));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkMagenta, Color.FromArgb(136, 23, 152));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkYellow, Color.FromArgb(193, 156, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Gray, Color.FromArgb(204, 204, 204)); // dark white
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGray, Color.FromArgb(118, 118, 118)); // bright black
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Blue, Color.FromArgb(59, 120, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Green, Color.FromArgb(22, 198, 12));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Cyan, Color.FromArgb(97, 214, 214));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Red, Color.FromArgb(231, 72, 86));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Magenta, Color.FromArgb(180, 0, 158));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Yellow, Color.FromArgb(249, 241, 165));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.White, Color.FromArgb(242, 242, 242));
        }
    }
}