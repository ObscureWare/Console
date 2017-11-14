// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTablePrinter.cs" company="Obscureware Solutions">
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
//   Defines the DataTablePrinter class.
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

    /// <summary>
    /// Base class for printing content of the <see cref="DataTable{T}"/>. Contains base routines for measurement.
    /// </summary>
    public abstract class DataTablePrinter
    {
        private const int MIN_SPACE_PER_COLUMN = 3; // TODO: improve code to reduce limit to 1-2

        private readonly IConsole _console;
        private readonly ICoreTableStyle _coreStyle;

        private readonly object _atom;

        protected DataTablePrinter(IConsole console, ICoreTableStyle coreStyle)
        {
            this._console = console;
            this._coreStyle = coreStyle;
            this._atom = coreStyle.AtomicPrinting ? console.AtomicHandle : new object();
        }

        /// <summary>
        /// Target, rendering console
        /// </summary>
        protected IConsole Console
        {
            get
            {
                return this._console;
            }
        }

        /// <summary>
        /// Prints data as simple, frame-less table
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        public void PrintTable(ColumnInfo[] columns, string[][] rows)
        {
            // TODO: replace altering  original columns with producing RuntimeColumnInfo with more fields
            this.CalculateRequiredRowSizes(columns, rows);

            int spacingWidth = columns.Length - 1; // This assumes 1-character wide spacing. If in need of using internal margins will have to expose related property from inheriting class
            int totalAvailableWidth = this.Console.WindowWidth - 1 - this.ExternalFrameThickness; // -1 for ENDL - need to not overflow, to avoid empty lines
            int maxRequiredWidth = columns.Select(col => col.CurrentLength).Sum() + spacingWidth;

            int totalFixedWidth = columns.Where(col => col.HasFixedLength).Sum(col => col.FixedLength);
            if (totalFixedWidth > totalAvailableWidth)
            {
                throw new ArgumentException("Total length of fixed-length columns exceed total available area.", nameof(columns));
            }

            // TODO: more complex calculation required - need real "MinLength" columns not only "FixedLength" ones

            int fixedColumnsCount = columns.Count(col => col.HasFixedLength);
            if ((totalAvailableWidth - totalFixedWidth) / (columns.Length - fixedColumnsCount) < MIN_SPACE_PER_COLUMN)
            {
                throw new ArgumentException($"Fixed-length columns leave not enough space for remaining columns. It shall be no less than {MIN_SPACE_PER_COLUMN} characters per non-fixed-length column. Or you have just declared too many columns for current console resolution.", nameof(columns));
            }

            // check if table fits to the screen width
            if (maxRequiredWidth > totalAvailableWidth)
            {
                int availableWidth = totalAvailableWidth - spacingWidth - totalFixedWidth;
                float scale = (float)this.Console.WindowWidth / (float)maxRequiredWidth;

                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i].HasFixedLength)
                    {
                        continue;
                    }

                    // TODO: probably round would be as good... gonna check
                    int newLength = (int)Math.Floor(columns[i].CurrentLength * scale);
                    if (newLength < columns[i].FixedLength)
                    {
                        newLength = Math.Min(availableWidth, columns[i].FixedLength); // if all columns have minWidth and overflow the screen - some will not be displayed at all...
                        columns[i].CurrentLength = newLength;
                    }

                    availableWidth -= newLength;
                    if (availableWidth < 0)
                    {
                        newLength = newLength - Math.Abs(availableWidth);
                        availableWidth = 0;
                    }

                    columns[i].CurrentLength = Math.Max(0, newLength);
                }
            }

            // Now can render table
            lock (this._atom)
            {
                this.RenderTable(columns, rows);
            }
        }

        private void CalculateRequiredRowSizes(ColumnInfo[] columns, string[][] rows)
        {
            // headers room
            for (int i = 0; i < columns.Length; ++i)
            {
                columns[i].CurrentLength = Math.Max(columns[i].CurrentLength, columns[i].FixedLength);

                foreach (string[] row in rows)
                {
                    if (i < row.Length) // some data might be missing, headers no
                    {
                        columns[i].CurrentLength = Math.Max(columns[i].CurrentLength, (row[i]?.Length) ?? 1);
                    }
                }
            }
        }

        private int[] CalculateRequiredRowSizes(string[] header, string[][] rows)
        {
            int[] result = new int[header.Length];

            // headers room
            for (int i = 0; i < header.Length; ++i)
            {
                result[i] = Math.Max(result[i], header[i].Length);

                foreach (string[] row in rows)
                {
                    if (i < row.Length) // some data might be missing, headers no
                    {
                        result[i] = Math.Max(result[i], (row[i]?.Length) ?? 1);
                    }
                }
            }

            return result;
        }

        public void PrintTable<T>(DataTable<T> table)
        {
            this.PrintTable(table.Columns.Values.Cast<ColumnInfo>().ToArray(), table.GetRows().ToArray());
        }

        /// <summary>
        /// Specifies extra external frames total thickness
        /// </summary>
        protected abstract int ExternalFrameThickness { get; }

        /// <summary>
        /// Actual rendering function
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        protected abstract void RenderTable(ColumnInfo[] columns, IEnumerable<string[]> rows);
    }
}
