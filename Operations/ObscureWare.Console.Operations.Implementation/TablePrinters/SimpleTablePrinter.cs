// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTablePrinter.cs" company="Obscureware Solutions">
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
//   Defines the SimpleTablePrinter class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Implementation.TablePrinters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ObscureWare.Console.Operations.Implementation.Tables;
    using ObscureWare.Console.Operations.Interfaces.Styles;
    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Shared;

    public class SimpleTablePrinter : DataTablePrinter
    {
        private readonly ICoreTableStyle _style;

        public SimpleTablePrinter(IConsole console, ICoreTableStyle style): base(console, style)
        {
            if (style == null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            this._style = style;
        }

        protected override int ExternalFrameThickness { get; } = 0; // no external frame

        protected override void RenderTable(ColumnInfo[] columns, IEnumerable<string[]> rows)
        {
            // yeah, some duplicated code here. Leave these optimized paths or make code more beautiful... decisions, decisions...
            // TODO: refactoring, simplification, 

            int index = 0;
            string formatter = string.Join(" ", columns.Select(col => $"{{{index++},{col.CurrentLength * (int)col.Alignment}}}"));

            if (this._style.ShowHeader)
            {
                this.Console.WriteLine(this._style.HeaderColor,
                    string.Format(formatter,
                        columns.Select(col => col.Header.Substring(0, Math.Min(col.Header.Length, col.CurrentLength)))
                            .ToArray()));
            }

            index = 0;
            foreach (string[] row in rows)
            {
                switch (this._style.OverflowBehaviour)
                {
                    case TableOverflowContentBehavior.Ellipsis:
                    {
                        string[] result = new string[columns.Length];
                        for (int i = 0; i < columns.Length; i++)
                        {
                            // taking care for asymmetric array, btw
                            if (row.Length > i)
                            {
                                if ((row[i]?? "").Length <= columns[i].CurrentLength)
                                {
                                    result[i] = row[i];
                                }
                                else
                                {
                                    result[i] = row[i].Substring(0, columns[i].CurrentLength);
                                }
                            }
                        }

                        this.Console.WriteLine(
                            (index % 2 == 0) ? this._style.EvenRowColor : this._style.RowColor,
                            string.Format(formatter, result));
                        break;
                    }
                    case TableOverflowContentBehavior.Wrap:
                    {
                        bool allCellsFit = true;
                        for (int r = 0; r < row.Length && r < columns.Length; r++)
                        {
                            if ((row[r] ?? "").Length > columns[r].CurrentLength)
                            {
                                allCellsFit = false;
                                break;
                            }
                        }

                        if (allCellsFit)
                        {
                            this.Console.WriteLine(
                                (index % 2 == 0) ? this._style.EvenRowColor : this._style.RowColor,
                                string.Format(formatter, row));
                        }
                        else
                        {
                            List<string[]> stacks = new List<string[]>(columns.Length);
                            for (int i = 0; i < columns.Length && i < row.Length; i++)
                            {
                                stacks.Add((row[i] ?? "").SplitTextToFit((uint) columns[i].CurrentLength).ToArray());
                            }

                            var tallestStack = stacks.Max(st => st.Length);
                            for (int stIndex = 0; stIndex < tallestStack; stIndex++)
                            {
                                string[] content = stacks.Select(st => (st.Length > stIndex) ? st[stIndex] : "").ToArray();
                                this.Console.WriteLine(
                                    (index % 2 == 0) ? this._style.EvenRowColor : this._style.RowColor,
                                    string.Format(formatter, content));
                            }
                        }

                        // write extra empty line to separate rows - simple table does not have any coloring for odd / even rows
                        // looks bad, do not - coloring rows instead...
                        //this.Console.WriteLine(this._style.RowColor, string.Format(formatter, new string[columns.Length]));
                        break;
                    }
                    default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(TableOverflowContentBehavior));
                    }
                }

                index++;

            }
        }
    }
}