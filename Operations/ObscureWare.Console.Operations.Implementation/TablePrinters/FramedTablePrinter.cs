// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FramedTablePrinter.cs" company="Obscureware Solutions">
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
//   Defines the FramedTablePrinter class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Implementation.TablePrinters
{
    using System.Collections.Generic;

    using ObscureWare.Console.Operations.Implementation.Tables;
    using ObscureWare.Console.Operations.Interfaces.Styles;
    using ObscureWare.Console.Root.Shared;

    public class FramedTablePrinter : DataTablePrinter
    {
        private readonly TableStyle _printStyle;

        public FramedTablePrinter(IConsole console, TableStyle printStyle) : base(console, printStyle)
        {
            this._printStyle = printStyle;
        }

        protected override int ExternalFrameThickness { get; } = 2;

        protected override void RenderTable(ColumnInfo[] columns, IEnumerable<string[]> rows)
        {
            this.Console.WriteLine("Not implemented");
        }
    }
}