// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnInfo.cs" company="Obscureware Solutions">
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
//   Defines the ColumnInfo class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Implementation.Tables
{
    using System;
    using System.Linq;

    using ObscureWare.Console.Operations.Interfaces.Tables;
    using ObscureWare.Console.Shared;

    /// <summary>
    /// Defines table-column
    /// </summary>
    public class ColumnInfo : IColumnInfo
    {
        public ColumnInfo(string header, ColumnAlignment alignment = ColumnAlignment.Left, int fixedLength = 0)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));
            }
            if (header.ToCharArray().Any(ch => ch.IsSystemChar()))
            {
                throw new ArgumentException("Header name cannot contain special characters.", nameof(header));
            }

            this.Header = header;
            this.Alignment = alignment;
            this.CurrentLength = header.Length;
            this.FixedLength = fixedLength;
        }

        public string Header { get; private set; }

        public int FixedLength { get; set; }

        internal int CurrentLength { get; set; }

        public ColumnAlignment Alignment { get; set; }

        public bool HasFixedLength
        {
            get { return this.FixedLength > 0; }
        }
    }
}