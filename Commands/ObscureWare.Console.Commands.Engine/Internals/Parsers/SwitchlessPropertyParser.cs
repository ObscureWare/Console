// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchlessPropertyParser.cs" company="Obscureware Solutions">
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
//   Defines the SwitchlessPropertyParser class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    using System;
    using System.Reflection;
    using Converters;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    /// <summary>
    /// Command model's property parser for "free-roaming" options, that do not have dedicated switches. 
    /// </summary>
    internal class SwitchlessPropertyParser : BasePropertyParser
    {
        private readonly ArgumentConverter _converter;

        public SwitchlessPropertyParser(int argumentIndex, PropertyInfo propertyInfo, ArgumentConverter converter) : base(propertyInfo)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            this.ArgumentIndex = argumentIndex;
            this._converter = converter;
        }

        public int ArgumentIndex { get; }

        /// <inheritdoc />
        protected override IParsingResult DoApply(ICommandParserOptions options, CommandModel model, string[] args, ref int argIndex)
        {
            try
            {
                this.TargetProperty.SetValue(model, this._converter.TryConvert(args[argIndex], options.UiCulture));

                return ParsingSuccess.Instance;
            }
            catch (Exception e)
            {
                return new ParsingFailure(e.Message);
            }
        }
    }
}