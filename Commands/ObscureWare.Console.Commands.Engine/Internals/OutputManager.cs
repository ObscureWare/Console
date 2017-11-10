// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputManager.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2016-2017 Sebastian Gruchacz
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
//   Defines OutputManager class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System.Collections.Generic;
    using System.Globalization;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Commands.Engine.Styles;
    using ObscureWare.Console.Operations.Interfaces;
    using ObscureWare.Console.Operations.Interfaces.Styles;
    using ObscureWare.Console.Operations.Interfaces.Tables;
    using ObscureWare.Console.Operations.Interfaces.TablePrinters;

    public class OutputManager : ICommandOutput
    {
        // TODO: add everywhere console / buffer boundaries checking
        // TODO: monitor console/buffer resizing  / scrolling - might be very tricky!
        // TODO: need for  progress / status / spinner display - have to control how these objects move through living buffer and to stop  putting content when already outside boundaries...

        private readonly IConsole _consoleInstance;
        private readonly CommandEngineStyles _engineStyles;

        private readonly CultureInfo _uiCulture;

        /// <summary>
        /// Default table printer for all commands
        /// </summary>
        private readonly DataTablePrinter _tablePrinter;

        public OutputManager(IConsole consoleInstance, CommandEngineStyles engineStyles, CultureInfo uiCulture)
        {
            this._consoleInstance = consoleInstance;
            this._engineStyles = engineStyles;
            this._uiCulture = uiCulture;

            this._tablePrinter = new SimpleTablePrinter(
                consoleInstance,
                new SimpleTableStyle(engineStyles.HelpStyles.HelpHeader, engineStyles.OddRowColor)
                {
                    EvenRowColor = engineStyles.EvenRowColor,
                    AtomicPrinting = true,
                    ShowHeader = true,
                    OverflowBehaviour = TableOverflowContentBehavior.Wrap
                });
        }

        public void PrintResultLines(IEnumerable<string> results)
        {
            // TODO: improve!

            foreach (var result in results)
            {
                this._consoleInstance.WriteLine(result);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            this._consoleInstance.Clear();
        }

        /// <inheritdoc />
        public void PrintSimpleTable<T>(DataTable<T> filesTable)
        {
            this._tablePrinter.PrintTable(filesTable);
        }

        public void PrintWarning(string message)
        {
            this._consoleInstance.WriteLine(this._engineStyles.Error, message);
        }

        public void PrintError(string message)
        {
            this._consoleInstance.WriteLine(this._engineStyles.Warning, message);
        }

        public CultureInfo UiCulture
        {
            get
            {
                return this._uiCulture;
            }
        }

        public void WriteText(ConsoleFontColor color, string message)
        {
            this._consoleInstance.WriteText(color, message);
        }

        public void WriteLine(ConsoleFontColor color, string message)
        {
            this._consoleInstance.WriteLine(color, message);
        }

        /// <summary>
        /// Reserved whole line (if cursor is at beginning - current, if not - next) and puts cursor on the beginning of line below
        /// </summary>
        /// <returns></returns>
        public IOutputLineManager ReserveNewLine()
        {
            lock (this._consoleInstance.AtomicHandle)
            {
                var currentPosition = this._consoleInstance.GetCursorPosition();
                if (currentPosition.X == 0)
                {
                    this._consoleInstance.SetCursorPosition(0, currentPosition.Y + 1);
                    return new SingleManagedLineManager(this._consoleInstance, currentPosition.Y, this._consoleInstance.WindowWidth); // line is empty, reserve it
                }
                else
                {
                    // automatically moves cursor below reserved line
                    this._consoleInstance.SetCursorPosition(0, currentPosition.Y + 2);
                    return new SingleManagedLineManager(this._consoleInstance, currentPosition.Y + 1, this._consoleInstance.WindowWidth); // ignore X, just reserve whole next line 
                }
            }
        }
    }
}