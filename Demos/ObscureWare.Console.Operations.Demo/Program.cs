// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Obscureware Solutions">
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
//   Just some DEMO stuff. Used for visual testing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Demo
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Console.Demo.Components;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Operations.Implementation;
    using ObscureWare.Console.Operations.Implementation.TablePrinters;
    using ObscureWare.Console.Operations.Implementation.Tables;
    using ObscureWare.Console.Operations.Interfaces.Styles;
    using ObscureWare.Console.Operations.Interfaces.Tables;
    using ObscureWare.Console.Root.Desktop;
    using ObscureWare.Tests.Common;
    using ObscureWare.Console.Shared;
    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.TestShared;

    using FrameStyle = Interfaces.Styles.FrameStyle;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            ConsoleController controller = new ConsoleController();
            //helper.ReplaceConsoleColor(ConsoleColor.DarkCyan, Color.Salmon);

            controller.ReplaceConsoleColors(
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue));

            IConsole console = new SystemConsole(controller, ConsoleStartConfiguration.Colorfull);
            
            DemoRunner runner = new DemoRunner(new IDemo[]
            {
                new MenuDemo(), 
                new CommandLineDemo(),
                new TextSplittingDemo(),
                new FramesDemo(),
                new TablePrintingDemo()

            }); // TODO: MEF

            runner.RunDemos(console);
        }

        // TODO: write simulation console to automatically test complex printing functions results (content only, no color abstracting)
    }
}