// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IColumnInfo.cs" company="Obscureware Solutions">
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
//   Defines the IDataTable<T> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Operations.Interfaces.Tables
{
    using System.Collections.Generic;

    /// <summary>
    /// Core functionality for a DataTable collection.
    /// </summary>
    /// <typeparam name="T">Stored objects type</typeparam>
    public interface IDataTable<T>
    {
        /// <summary>
        /// Gets columns collection
        /// </summary>
        Dictionary<string, IColumnInfo> Columns { get; }

        IEnumerable<string[]> GetRows();

        /// <summary>
        /// Finds first value that is identified by value stored in the first column or NULL
        /// </summary>
        /// <param name="aIdentifier">Artificial Idx identifier to be used for searching in the first column</param>
        /// <returns></returns>
        T GetUnderlyingValue(string aIdentifier);

        /// <summary>
        /// Gets number of rows
        /// </summary>
        int RowCount { get; }
    }
}