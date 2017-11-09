// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagPropertyParser.cs" company="Obscureware Solutions">
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
//   Defines the FlagPropertyParser class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    using System;
    using System.Reflection;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    /// <summary>
    /// Property parser for flags (BOOL)
    /// </summary>
    internal class FlagPropertyParser : BasePropertyParser
    {
        public FlagPropertyParser(PropertyInfo flagProperty) : base(flagProperty)
        {

        }

        /// <inheritdoc />
        protected override IParsingResult DoApply(ICommandParserOptions options, CommandModel model, string[] args, ref int argIndex)
        {
            try
            {
                this.TargetProperty.SetValue(model, true);

                argIndex++;

                return ParsingSuccess.Instance;
            }
            catch (Exception e)
            {
                return new ParsingFailure(e.Message);
            }
        }
    }
}