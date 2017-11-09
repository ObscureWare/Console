// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandEngine.cs" company="Obscureware Solutions">
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
//   Defines the ICommandEngine implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using ObscureWare.Console.Shared;

namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Conditions;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Engine.Styles;

    using ObscureWare.Console.Root.Interfaces;

    using ObscureWare.Console.Operations.Interfaces;


    /// <summary>
    /// The command engine internal implementation.
    /// </summary>
    internal class CommandEngine : ICommandEngine
    {
        private readonly CommandManager _commandManager;

        private readonly IConsole _console;

        private readonly CommandEngineStyles _styles;

        private readonly HelpPrinter _helpPrinter;

        private readonly ICommandParserOptions _options;

        private readonly OutputManager _outputManager;
        private readonly VirtualEntryLine _virtualLine;

        private CommandAutoCompletionProvider _autoCompletionManager;

        // NO default public constructor - by design -> use builder
        internal CommandEngine(CommandManager commandManager, ICommandParserOptions options, CommandEngineStyles styles, HelpPrinter printHelper, IConsole console, IClipboard clipboard)
        {
            commandManager.Requires(nameof(commandManager)).IsNotNull();
            options.Requires(nameof(options)).IsNotNull();
            styles.Requires(nameof(styles)).IsNotNull();
            printHelper.Requires(nameof(printHelper)).IsNotNull();
            console.Requires(nameof(console)).IsNotNull();
            clipboard.Requires(nameof(clipboard)).IsNotNull();

            this._commandManager = commandManager;
            this._autoCompletionManager = new CommandAutoCompletionProvider(this._commandManager);
            this._virtualLine = new VirtualEntryLine(console, clipboard, styles.Default);
            this._console = console;
            this._options = options;
            this._styles = styles;
            this._helpPrinter = printHelper;
            this._outputManager = new OutputManager(this._console, this._styles, this._options.UiCulture);

            // TODO: check option before displaying this message
            // this._console.WriteLine(styles.Default, "Initializing ObscureWare's command engine...");
        }

        /// <inheritdoc />
        public bool ExecuteCommand(ICommandEngineContext context, string commandLine)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(commandLine))
            {
                return true; // just ignore empty command, it's fine to do nothing ;-)
                // TODO: add option in configuration to rather display help-nagging screen instead - while this is fine for running engine, better do something on 
            }

            string[] commandLineArguments = CommandLineUtilities.SplitCommandLine(commandLine).ToArray();

            return this.ExecuteCommand(context, commandLineArguments);
        }

        public bool ExecuteCommand(ICommandEngineContext context, string[] commandLineArguments)
        {
            if (commandLineArguments == null || !commandLineArguments.Any())
            {
                // co commands - suggest assistance
                this._helpPrinter.PrintHelpOnHelp();
                return false;
            }

            // ...
            string cmdName = commandLineArguments[0];
            if (this._helpPrinter.IsGlobalHelpRequested(cmdName))
            {
                // Global, or command help?
                if (commandLineArguments.Length > 1)
                {
                    cmdName = commandLineArguments[1];
                    CommandInfo command = this._commandManager.FindCommand(cmdName);
                    if (command != null)
                    {
                        this.PrintCommandHelp(command, commandLineArguments.Skip(2)); // pass all remaining options only for detail-full syntax help (if available / implemented)
                        return true;
                    }
                    else
                    {
                        this._console.WriteLine(this._styles.Warning, $"Unknown command => \"{cmdName}\".");
                    }
                }

                this.PrintGlobalHelp(commandLineArguments.Skip(1));
                return true;
            }

            CommandInfo cmd = this._commandManager.FindCommand(cmdName);
            if (cmd == null)
            {
                this._console.WriteLine(this._styles.Warning, $"Unknown command => \"{cmdName}\".");
                this._helpPrinter.PrintHelpOnHelp();
                return false;
            }

            if (commandLineArguments.Length > 1)
            {
                // any help switch inside command line will cause displaying help instead of parsing it
                if (this._helpPrinter.IsCommandHelpRequested(commandLineArguments))
                {
                    this.PrintCommandHelp(cmd, commandLineArguments.Skip(2)); // pass all remaining options only for detail-full syntax help (if available / implemented)
                    return true;
                }
            }

            // This might crash-throw if invalid types defined. Fine.
            var model = this.BuildModelForCommand(cmd, commandLineArguments.Skip(1));

            if (model == null)
            {
                // bad syntax
                return false;
            }

            try
            {
                try
                {
                    cmd.Command.Execute(context, _outputManager, model); // skip only cmdName itself
                }
                catch (Exception ex)
                {
                    this._console.WriteLine(this._styles.Error, "An exception occurred during command execution:");
                    this._console.WriteLine(this._styles.Error, ex.ToString());

                    // TODO: log also to file?
                    return false;
                }
            }
            finally
            {
                // reset color to default after each command - best for CMD execution
                this._console.SetColors(this._styles.Default);
            }

            return true;
        }

        public void Run(ICommandEngineContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            while (!context.ShallFinishInteracativeSession)
            {
                this.DisplayPrompt(context.GetCurrentPrompt()); // TODO: perhaps multi-color prompt support?

                string cmdString = this.ReadCommand();
                if (string.IsNullOrWhiteSpace(cmdString))
                {
                    continue;
                }

                this.ExecuteCommand(context, cmdString);
            }
        }

        private string ReadCommand()
        {
            // this._console.ReadLine() // use for simple behavior
            return this._virtualLine.GetUserEntry(this._autoCompletionManager);
        }

        private void DisplayPrompt(string promptText)
        {
            this._console.WriteLine();
            this._console.WriteText(this._styles.Prompt, promptText);
            this._console.SetColors(this._styles.Default);
        }

        private void PrintGlobalHelp(IEnumerable<string> arguments)
        {
            this._helpPrinter.PrintGlobalHelp(this._commandManager.GetAll(), arguments);
        }

        private void PrintCommandHelp(CommandInfo cmd, IEnumerable<string> extraArguments)
        {
            this._helpPrinter.PrintCommandHelp(cmd.CommandModelBuilder);
        }

        private object BuildModelForCommand(CommandInfo cmdInfo, IEnumerable<string> arguments)
        {
            return cmdInfo.CommandModelBuilder.BuildModel(arguments, this._options, this._outputManager);
        }
    }
}