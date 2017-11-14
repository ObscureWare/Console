// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAutoCompleter.cs" company="Obscureware Solutions">
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
//   Defines the TestAutoCompleter class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.TestShared
{
    using System.Collections.Generic;
    using System.Linq;

    using ObscureWare.Console.Operations.Interfaces;

    /// <summary>
    /// This auto-completer provides some auto-completion from predefined, hard-coded list
    /// </summary>
    public class TestAutoCompleter : IAutoComplete
    {
        private readonly string[] _availableTexts;

        public TestAutoCompleter(params string[] availableTexts)
        {
            this._availableTexts = availableTexts;
        }

        /// <inheritdoc />
        public IEnumerable<string> MatchAutoComplete(string text)
        {
            return this._availableTexts.Where(t => t.Contains(text));
        }
    }
}