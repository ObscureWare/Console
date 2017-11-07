// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandOutput.cs" company="Obscureware Solutions">
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
//   Defines the ICommandOutput interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ObscureWare.Console.Operations.Interfaces.Tables;
using ObscureWare.Console.Root.Interfaces;

namespace ObscureWare.Console.Commands.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;

    public interface ICommandOutput
    {
        /// <summary>
        /// Prints to the output given lines using Result Styling
        /// </summary>
        /// <param name="results"></param>
        void PrintResultLines(IEnumerable<string> results);

        void Clear();

        void PrintSimpleTable<T>(DataTable<T> filesTable);

        void PrintWarning(string message);

        void PrintError(string message);

        CultureInfo UiCulture { get; }

        void WriteText(ConsoleFontColor color, string message);


        void WriteLine(ConsoleFontColor color, string message);

        /// <summary>
        /// Obtains first unused line area and returns management object.
        /// </summary>
        /// <returns></returns>
        IOutputLineManager ReserveNewLine();
    }
}