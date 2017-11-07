// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandParserOptions.cs" company="Obscureware Solutions">
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
//   Defines the core CommandParserOptions implementation of ICommandParserOptions interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Interfaces
{
    using System.Globalization;
    using System.Linq;
    using ObscureWare.Console.Shared;

    public class CommandParserOptions : ICommandParserOptions
    {
        public CommandParserOptions()
        {
            this.UiCulture = CultureInfo.CurrentUICulture; // default, can override
        }

        public string[] FlagCharacters { get; set; } = new[] { @"\" };

        public string[] SwitchCharacters { get; set; } = new[] { @"-" };

        public CommandOptionArgumentMode OptionArgumentMode { get; set; } = CommandOptionArgumentMode.Separated;

        public char OptionArgumentJoinCharacater { get; set; } = ':';

        public bool AllowFlagsAsOneArgument { get; set; } = false;

        public SwitchlessOptionsMode SwitchlessOptionsMode { get; set; } = SwitchlessOptionsMode.EndOnly;

        public CommandCaseSensitivenes CommandsSensitivenes { get; set; } = CommandCaseSensitivenes.Insensitive;

        public CultureInfo UiCulture { get; set; }

        public void ValidateParserOptions()
        {
            if (this.FlagCharacters == null || this.FlagCharacters.Length < 1)
            {
                throw new BadImplementationException($"{nameof(this.FlagCharacters)} cannot be null or empty.", typeof(CommandParserOptions));
            }

            if (this.SwitchCharacters == null || this.SwitchCharacters.Length < 1)
            {
                throw new BadImplementationException($"{nameof(this.SwitchCharacters)} cannot be null or empty.", typeof(CommandParserOptions));
            }

            if (this.FlagCharacters.Any(string.IsNullOrWhiteSpace))
            {
                throw new BadImplementationException($"{nameof(this.FlagCharacters)} cannot contain null or empty elements.", typeof(CommandParserOptions));
            }

            if (this.SwitchCharacters.Any(string.IsNullOrWhiteSpace))
            {
                throw new BadImplementationException($"{nameof(this.SwitchCharacters)} cannot contain null or empty elements.", typeof(CommandParserOptions));
            }

            if ((this.OptionArgumentMode == CommandOptionArgumentMode.Joined) && char.IsWhiteSpace(this.OptionArgumentJoinCharacater))
            {
                throw new BadImplementationException($"In {nameof(CommandOptionArgumentMode.Joined)} mode {nameof(this.OptionArgumentJoinCharacater)} cannot be white character.", typeof(CommandParserOptions));
            }

            if (this.AllowFlagsAsOneArgument && this.OptionArgumentMode == CommandOptionArgumentMode.Merged)
            {
                throw new BadImplementationException($"When {nameof(this.OptionArgumentMode)} is set to \"{nameof(CommandOptionArgumentMode.Merged)}\", {nameof(this.AllowFlagsAsOneArgument)} cannot be set to TRUE because this could led to ambiguous syntax.", typeof(CommandParserOptions));
            }

            if (this.SwitchlessOptionsMode == SwitchlessOptionsMode.Mixed && this.OptionArgumentMode == CommandOptionArgumentMode.Separated)
            {
                throw new BadImplementationException($"Options {nameof(this.OptionArgumentMode)}=\"{nameof(CommandOptionArgumentMode.Separated)}\" and {nameof(this.SwitchlessOptionsMode)}=\"{nameof(SwitchlessOptionsMode.Mixed)}\" should not be selected at the same time because this could led to ambiguous syntax.", typeof(CommandParserOptions));
            }
        }

        // TODO: default static constructors for DOS-style, Unix-style etc...
    }
}