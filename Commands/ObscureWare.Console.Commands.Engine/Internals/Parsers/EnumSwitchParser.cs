// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumSwitchParser.cs" company="Obscureware Solutions">
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
//   Defines the EnumSwitchParser class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    internal class EnumSwitchParser : BaseSwitchPropertyParser
    {
        private readonly Type _enumType;

        private readonly string[] _validValues;
        private object _defaultValue;

        public EnumSwitchParser(PropertyInfo propertyInfo, CommandOptionSwitchAttribute optionSwitchAttribute) : base(propertyInfo, optionSwitchAttribute.CommandLiterals)
        {
            if (optionSwitchAttribute == null)
            {
                throw new ArgumentNullException(nameof(optionSwitchAttribute));
            }

            this._enumType = optionSwitchAttribute.SwitchBaseType;
            this._validValues = Enum.GetNames(this._enumType);
            this._defaultValue = optionSwitchAttribute.DefaultValue;
        }

        /// <inheritdoc />
        protected override IParsingResult DoApplySwitch(CommandModel model, string[] switchArguments, IValueParsingOptions pOptions)
        {
            if (pOptions == null)
            {
                throw new ArgumentNullException(nameof(pOptions));
            }
            if (switchArguments.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(switchArguments));
            }

            try
            {
                string enumText = switchArguments[0];
                if (string.IsNullOrWhiteSpace(enumText))
                {
                    return new ParsingFailure("Switch value cannot be empty.");
                }

                object enumValue;
                try
                {
                    enumValue = Enum.Parse(this._enumType, enumText, true); // TODO: is there a better way to parse generic enum?
                }
                catch (ArgumentException)
                {
                    return new ParsingFailure($"'{enumText}' is not proper expected value from [{this._enumType.Name}] enumeration.");
                }

                this.TargetProperty.SetValue(model, enumValue);

                return ParsingSuccess.Instance;
            }
            catch (Exception e)
            {
                return new ParsingFailure(e.Message);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<string> GetValidValues()
        {
            return this._validValues;
        }

        public override IParsingResult ApplyDefault(CommandModel model)
        {
            try
            {
                this.TargetProperty.SetValue(model, this._defaultValue);

                return ParsingSuccess.Instance;
            }
            catch (Exception e)
            {
                return new ParsingFailure(e.Message);
            }
        }
    }
}