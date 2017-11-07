// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseSwitchPropertyParser.cs" company="Obscureware Solutions">
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
//   Defines the BaseSwitchPropertyParser base class for all parsers using value-switch syntax.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;


    using Shared;

    internal abstract class BaseSwitchPropertyParser : BasePropertyParser
    {
        private readonly string[] _switchLetters;

        private readonly uint _expectedArguments;

        /// <inheritdoc />
        protected BaseSwitchPropertyParser(PropertyInfo propertyInfo, string[] switchLetters, uint expectedArguments = 1)
            : base(propertyInfo)
        {
            if (switchLetters == null) throw new ArgumentNullException(nameof(switchLetters));
            if (switchLetters.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(switchLetters));

            this._switchLetters = switchLetters;
            this._expectedArguments = expectedArguments;
        }

        protected override IParsingResult DoApply(ICommandParserOptions options, CommandModel model, string[] args, ref int argIndex)
        {
            // read more arguments if necessary
            if (options.OptionArgumentMode == CommandOptionArgumentMode.Separated)
            {
                string[] optionArguments = new string[this._expectedArguments];
                for (int i = 0; i < optionArguments.Length; ++i)
                {
                    argIndex++; // skip option arg itself
                    optionArguments[i] = this.SafelyGetNextArg(args, ref argIndex);
                }

                return this.DoApplySwitch(model, optionArguments, options);
            }
            else if (options.OptionArgumentMode == CommandOptionArgumentMode.Joined)
            {
                string firstArg = this.SafelyGetNextArg(args, ref argIndex);
                string[] parts = firstArg.Split(options.OptionArgumentJoinCharacater);
                if (parts.Length < 2)
                {
                    return new ParsingFailure($"Expected argument syntax is '{parts[0]}{options.OptionArgumentJoinCharacater}<some_value>'.");
                }

                return this.DoApplySwitch(model, parts.Skip(1).ToArray(), options); // will support multi-values as well...
            }
            else if (options.OptionArgumentMode == CommandOptionArgumentMode.Merged)
            {
                if (this._expectedArguments != 1)
                {
                    throw new BadImplementationException(
                        "Options in Merged mode can only have one value. This mode could not support more.",
                        this.GetType());
                }

                string firstArg = this.SafelyGetNextArg(args, ref argIndex);
                string[] combinations = CommandsSyntaxHelpers.Combine(options.SwitchCharacters, this._switchLetters, (s, s1) => s + s1).ToArray();
                string remainder = firstArg.CutLeftFirst(combinations);

                return this.DoApplySwitch(model, new []{ remainder }, options);
            }

            throw new ArgumentOutOfRangeException(nameof(options), nameof(options.OptionArgumentMode));
        }

        private string SafelyGetNextArg(string[] args, ref int argIndex)
        {
            if (argIndex >= args.Length)
            {
                throw new InvalidOperationException("Command line provided less arguments that Option expected.");
            }

            return args[argIndex++];
        }

        protected abstract IParsingResult DoApplySwitch(CommandModel model, string[] switchArguments, IValueParsingOptions pOptions);

        public abstract IEnumerable<string> GetValidValues();

        public abstract IParsingResult ApplyDefault(CommandModel model);
    }
}