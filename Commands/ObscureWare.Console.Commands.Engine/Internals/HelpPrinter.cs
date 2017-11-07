// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpPrinter.cs" company="Obscureware Solutions">
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
//   Defines the HelpPrinter type, used to display help messages - lists of commands and syntaxes...
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ObscureWare.Console.Shared;
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Styles;
    using ObscureWare.Console.Operations.Interfaces.Styles;
    using ObscureWare.Console.Operations.Interfaces.Tables;
    using ObscureWare.Console.Operations.Interfaces.TablePrinters;

    using Parsers;

    internal class HelpPrinter : IKeyWordProvider
    {
        private static readonly string[] HelpCommands = new[] { "?", "help", "h" };
        private static readonly IEqualityComparer<string> SensitiveComparer = new SensitiveStringComparer();
        private static readonly IEqualityComparer<string> InsensitiveComparer = new InsensitiveStringComparer();

        private readonly ICommandParserOptions _options;
        private readonly IHelpStyles _helpStyles;

        private readonly IConsole _console;
        private readonly string[] _allInlineHelpOptions;
        private readonly IEqualityComparer<string> _commandNameComparer;

        public HelpPrinter(ICommandParserOptions options, IHelpStyles helpStyles, IConsole console)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (helpStyles == null)
            {
                throw new ArgumentNullException(nameof(helpStyles));
            }
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            this._options = options;
            this._helpStyles = helpStyles;
            this._console = console;
            this._allInlineHelpOptions = CommandsSyntaxHelpers.Combine(this._options.FlagCharacters, HelpCommands, ((s, s1) => s + s1)).ToArray();
            this._commandNameComparer = (options.CommandsSensitivenes == CommandCaseSensitivenes.Sensitive)
                ? SensitiveComparer
                : InsensitiveComparer;
        }

        /// <summary>
        /// Returns true, if given command is Global-Help command
        /// </summary>
        /// <param name="cmdName"></param>
        /// <returns></returns>
        public bool IsGlobalHelpRequested(string cmdName)
        {
            return this._allInlineHelpOptions.Contains(cmdName, this._commandNameComparer);
        }

        /// <summary>
        /// Returns true, if user requested help about command details.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public bool IsCommandHelpRequested(IEnumerable<string> arguments)
        {
            return arguments.Any(arg => this._allInlineHelpOptions.Contains(arg, this._commandNameComparer));
        }

        /// <summary>
        /// Prints full (?) help about particular command.
        /// </summary>
        /// <param name="cmdModelBuilder"></param>
        public void PrintCommandHelp(CommandModelBuilder cmdModelBuilder)
        {
            // TODO: implement table that allows separate column coloring
            // TODO: or per-cell coloring
            // this would allow for better display.

            this._console.WriteLine(this._helpStyles.CommonStyles.Warning, $"Function '{nameof(this.PrintCommandHelp)}' is not yet fully implemented.");
            this._console.WriteLine(this._helpStyles.CommonStyles.Default, string.Empty);

            this._console.WriteLine(this._helpStyles.HelpDescription, cmdModelBuilder.CommandDescription);
            this._console.WriteLine(this._helpStyles.HelpBody, "Syntax:");
            this._console.WriteText(this._helpStyles.CommonStyles.Default, "  ");

            // TODO: move switch-less to the end and sort!

            var syntax = cmdModelBuilder.GetSyntax().ToArray();
            var options = string.Join(" ", syntax.OrderBy(s => s.IsMandatory).ThenBy(s => s.OptionType).Select(s => s.GetSyntaxString(this._options)));

            // TODO: use same syntax order like in GetSyntax()

            this._console.WriteLine(this._helpStyles.HelpSyntax, $"{cmdModelBuilder.CommandName} {options}");

            if (syntax.Any())
            {
                this._console.WriteLine(this._helpStyles.CommonStyles.Default, string.Empty);
                this._console.WriteLine(this._helpStyles.HelpBody, "Where:");
                foreach (var syntaxInfo in syntax)
                {
                    var mandatoryIndicator = syntaxInfo.IsMandatory ? "*" : string.Empty;

                    if (syntaxInfo.OptionType != SyntaxOptionType.Switchless)
                    {
                        var literals = string.Join(" ", syntaxInfo.Literals);

                        this._console.WriteText(this._helpStyles.HelpDefinition, $"  {literals}\t{syntaxInfo.OptionName}{mandatoryIndicator}\t");
                        string desc = (syntaxInfo.DefaultValue == null)
                            ? syntaxInfo.Description
                            : $"{syntaxInfo.Description} Defaults to '{syntaxInfo.DefaultValue.ToString()}'";
                        this._console.WriteLine(this._helpStyles.HelpDescription, desc);

                        // TODO: more description for values of switches / values etc or custom description lines - for each property or available value
                    }
                    else
                    {
                        this._console.WriteText(this._helpStyles.HelpDefinition, $"  \"{syntaxInfo.OptionName}\"{mandatoryIndicator}\t");
                        string desc = (syntaxInfo.DefaultValue == null)
                            ? syntaxInfo.Description
                            : $"{syntaxInfo.Description} Defaults to '{syntaxInfo.DefaultValue.ToString()}'";
                        this._console.WriteLine(this._helpStyles.HelpDescription, desc);
                    }
                }

                this._console.WriteLine(this._helpStyles.CommonStyles.Default, string.Empty);
                if (syntax.Any(s => s.IsMandatory))
                {
                    this._console.WriteLine(this._helpStyles.CommonStyles.Default, "Options denoted with \"*\" character are mandatory. In the syntax they are in pointy brackets.");
                }
                this._console.WriteLine(this._helpStyles.CommonStyles.Default, "All option switches are case sensitive until option explicitly states case alternatives.");

                if (this._options.AllowFlagsAsOneArgument && syntax.Count(s => s.OptionType == SyntaxOptionType.Flag) > 1)
                {
                    this._console.WriteText(this._helpStyles.CommonStyles.Default, "All flag-options being turned on can be also specified altogether, i. e. ");
                    string allFlagsText = string.Concat(syntax.Where(s => s.OptionType == SyntaxOptionType.Flag).Select(s => s.Literals.First()));
                    this._console.WriteText(this._helpStyles.HelpDefinition, $"{this._options.FlagCharacters.SelectRandom()}{allFlagsText}");
                    this._console.WriteText(this._helpStyles.CommonStyles.Default, string.Empty);
                }

                if (this._options.SwitchlessOptionsMode == SwitchlessOptionsMode.Mixed && syntax.Count(s => s.OptionType == SyntaxOptionType.Switchless) > 0)
                {
                    this._console.WriteLine(this._helpStyles.CommonStyles.Default, "All options that do not have option selector can be placed anywhere between other options (and their values). Only order is important.");
                    this._console.WriteLine(this._helpStyles.CommonStyles.Default, "Actual order of all OTHER option switches is unimportant.");
                }
                else
                {
                    this._console.WriteLine(this._helpStyles.CommonStyles.Default, "Actual order of all option switches is unimportant.");
                }
            }

            // TODO: larger command description itself - if available

            this._console.WriteLine(this._helpStyles.CommonStyles.Default, string.Empty);
        }

        /// <summary>
        /// Prints generic help information.
        /// </summary>
        public void PrintHelpOnHelp()
        {
            this._console.WriteText(this._helpStyles.CommonStyles.Default, "To get list of available commands type ");
            for (int i = 0; i < this._allInlineHelpOptions.Length; i++)
            {
                if (i > 0)
                {
                    this._console.WriteText(this._helpStyles.CommonStyles.Default, i == this._allInlineHelpOptions.Length - 1 ? " or " : ", ");
                }

                this._console.WriteText(this._helpStyles.HelpDefinition, this._allInlineHelpOptions[i]);
            }

            this._console.WriteLine(this._helpStyles.CommonStyles.Default, ".");
        }

        /// <summary>
        /// Prints global help and list of available commands.
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="arguments">Extra help arguments. Not used at the moment.</param>
        public void PrintGlobalHelp(IEnumerable<CommandInfo> commands, IEnumerable<string> arguments)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            this._console.WriteLine(this._helpStyles.HelpHeader, "Available commands:");

            // TODO: make this printer and style read-only in constructor - no changes. if style changes, instance changes...

            var verySimpleStyling = new SimpleTableStyle(
                this._helpStyles.HelpHeader,
                this._helpStyles.CommonStyles.OddRowColor)
            {
                AtomicPrinting = true,
                EvenRowColor = this._helpStyles.CommonStyles.EvenRowColor,
                OverflowBehaviour = TableOverflowContentBehavior.Wrap,
                ShowHeader = false
            };

            var commandInfos = commands as CommandInfo[] ?? commands.ToArray();

            var commandListTable = new DataTable<CommandInfo>(
                new ColumnInfo("Command Name", ColumnAlignment.Left, commandInfos.Max(cmd => cmd.CommandModelBuilder.CommandName.Length) + 2),
                new ColumnInfo("Description", ColumnAlignment.Left));

            foreach (var cmdInfo in commandInfos)
            {
                commandListTable.AddRow(cmdInfo, new[]{
                    "  " + cmdInfo.CommandModelBuilder.CommandName,
                    cmdInfo.CommandModelBuilder.CommandDescription});
            }

            var printer = new SimpleTablePrinter(this._console, verySimpleStyling);
            printer.PrintTable(commandListTable); // TODO: printAt + 2 columns of margin, now simulated by +2 spaces to first column
            // printAt() or PrintIntended(table, indentation)

            this._console.WriteLine();
            this._console.WriteLine(this._helpStyles.CommonStyles.Default, $"All command names are case {this._options.CommandsSensitivenes.ToString().ToLower()}.");
            this._console.WriteText(this._helpStyles.CommonStyles.Default, "To receive syntax help about particular command use \"");
            this._console.WriteText(this._helpStyles.HelpDefinition, $"<commandName> {this._options.FlagCharacters.SelectRandom()}{HelpCommands.SelectRandom()}");
            this._console.WriteText(this._helpStyles.CommonStyles.Default, "\" or \"");
            this._console.WriteText(this._helpStyles.HelpDefinition, $"{this._options.FlagCharacters.SelectRandom()}{HelpCommands.SelectRandom()} <commandName>");
            this._console.WriteLine(this._helpStyles.CommonStyles.Default, "\" syntax.");
            this._console.WriteLine();
        }

        public IEnumerable<string> GetCommandKeyWords()
        {
            return HelpCommands;
        }

        public IEnumerable<string> GetOptionKeyWords()
        {
            return HelpCommands;
        }
    }
}