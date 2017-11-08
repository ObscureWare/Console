// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyntaxInfo.cs" company="Obscureware Solutions">
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
//   Defines SyntaxInfo class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Linq;
    using System.Reflection;

    using ObscureWare.Console.Commands.Interfaces;

    public class SyntaxInfo
    {
        private readonly PropertyInfo _propertyInfo;

        public SyntaxInfo(PropertyInfo propertyInfo, string optionName)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            if (string.IsNullOrWhiteSpace(optionName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(optionName));

            this.OptionName = optionName;
            this._propertyInfo = propertyInfo;
        }

        public bool IsMandatory { get; internal set; }

        public string[] Literals { get; internal set; }

        public string OptionName { get; private set; }

        public SyntaxOptionType OptionType { get; internal set; }

        public string[] SwitchValues { get; set; }

        public string Description { get; set; }

        public string TargetPropertyName => this._propertyInfo.Name;

        public object DefaultValue { get; set; }

        public string GetSyntaxString(ICommandParserOptions options)
        {
            // TODO: take into consideration this: https://technet.microsoft.com/en-us/library/cc771080(v=ws.11).aspx (and perhaps Linux-equivalent...)
            // TODO: maybe separate syntax printing from syntax info record?

            var wrapper = (this.IsMandatory) ? "<{0}{1}>" : "[{0}{1}]";
            return string.Format(wrapper, this.GetInnerSyntaxSelector(options), this.GetInnerSyntaxString(options));
        }

        private object GetInnerSyntaxSelector(ICommandParserOptions options)
        {
            switch (this.OptionType)
            {
                case SyntaxOptionType.Flag:
                    return options.FlagCharacters.First();
                case SyntaxOptionType.Switch:
                    return options.SwitchCharacters.First();
                case SyntaxOptionType.CustomValueSwitch:
                    return options.SwitchCharacters.First();
                case SyntaxOptionType.Switchless:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(SyntaxOptionType));
            }
        }

        private string GetInnerSyntaxString(ICommandParserOptions options)
        {
            switch (this.OptionType)
            {
                case SyntaxOptionType.Flag:
                    return this.Literals.First();
                case SyntaxOptionType.Switch:
                    return this.GetSwitchSyntax(options);
                case SyntaxOptionType.CustomValueSwitch:
                    return this.GetCustomSwitchSyntax(options);
                case SyntaxOptionType.Switchless:
                    return $"\"{this.OptionName}\"";
                default:
                    throw new ArgumentOutOfRangeException(nameof(SyntaxOptionType));
            }
        }

        private string GetSwitchSyntax(ICommandParserOptions options)
        {
            string literal = this.Literals.First();
            string value = string.Join("|", this.SwitchValues);

            switch (options.OptionArgumentMode)
            {
                case CommandOptionArgumentMode.Separated:
                    return $"{literal} {value}";
                case CommandOptionArgumentMode.Merged:
                    return $"{literal}{value}";
                case CommandOptionArgumentMode.Joined:
                    return $"{literal}{options.OptionArgumentJoinCharacater}{value}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(CommandOptionArgumentMode));
            }
        }

        private string GetCustomSwitchSyntax(ICommandParserOptions options)
        {
            string literal = this.Literals.First();
            string value = this.OptionName;

            switch (options.OptionArgumentMode)
            {
                case CommandOptionArgumentMode.Separated:
                    return $"{literal} {value}";
                case CommandOptionArgumentMode.Merged:
                    return $"{literal}{value}";
                case CommandOptionArgumentMode.Joined:
                    return $"{literal}{options.OptionArgumentJoinCharacater}{value}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(CommandOptionArgumentMode));
            }
        }
    }

    public enum SyntaxOptionType
    {
        Flag,

        Switch,

        CustomValueSwitch,

        Switchless
    }
}