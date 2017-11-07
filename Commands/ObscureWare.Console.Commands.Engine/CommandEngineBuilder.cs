using ObscureWare.Console.Operations.Interfaces;

namespace ObscureWare.Console.Commands.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ObscureWare.Console.Commands.Engine.Internals;
    using ObscureWare.Console.Commands.Engine.Styles;
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Shared;

    /// <summary>
    /// Fluent-syntax based CommandEngine builder
    /// </summary>
    public class CommandEngineBuilder
    {
        private CommandParserOptions _options;

        private CommandEngineStyles _styles;

        private readonly List<Type> _commands;

        private IClipboard _clipboard;

        private IDependencyResolver _resolver;

        private CommandEngineBuilder()
        {
            this._commands = new List<Type>();
            this._styles = CommandEngineStyles.DefaultStyles;
            // TODO: this._options = CommandParserOptions.Default;

            this.AddStandardCommands();
        }

        private void AddStandardCommands()
        {
            // ...
        }

        public CommandEngineBuilder UsingStyles(CommandEngineStyles styles)
        {
            if (styles == null)
            {
                throw new ArgumentNullException(nameof(styles));
            }

            this._styles = styles;

            return this;
        }

        public CommandEngineBuilder UsingOptions(CommandParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this._options = options;

            return this;
        }

        public CommandEngineBuilder WithCommandsFromAssembly(Assembly asm)
        {
            if (asm == null)
            {
                throw new ArgumentNullException(nameof(asm));
            }

            var commandTypes = asm.GetTypes().Where(t => typeof(IConsoleCommand).IsAssignableFrom(t));
            this._commands.AddRange(commandTypes);

            return this;
        }

        public CommandEngineBuilder WithCommands(params Type[] commandTypes)
        {
            if (commandTypes == null)
            {
                throw new ArgumentNullException(nameof(commandTypes));
            }

            this._commands.AddRange(commandTypes);


            return this;
        }

        public CommandEngineBuilder UsingClipboardProvider(IClipboard clipboard)
        {
            if (clipboard == null)
            {
                throw new ArgumentNullException(nameof(clipboard));
            }

            this._clipboard = clipboard;

            return this;
        }

        /// <summary>
        /// This dependency resolver will be used to construct command objects
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public CommandEngineBuilder UsingDependencyResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            this._resolver = resolver;

            return this;
        }

        /// <summary>
        /// Finally construct Engine instance when all items are ready
        /// </summary>
        /// <param name="console"></param>
        /// <returns></returns>
        public ICommandEngine ConstructForConsole(IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (this._options == null)
            {
                throw new InvalidOperationException("Could not construct engine without providing Options object.");
            }

            if (this._styles == null)
            {
                throw new InvalidOperationException("Could not construct engine without providing Styles object.");
            }

            if (this._clipboard == null)
            {
                this._clipboard = new DoNothingClipBoard();
            }

            if (this._resolver == null)
            {
                this._resolver = new DefaultCommandResolver();
            }

            var printHelper = new HelpPrinter(this._options, this._styles.HelpStyles, console);
            var commandManager = new CommandManager(this._commands.Distinct().ToArray(), this._resolver)
            {
                CommandsSensitivenes = this._options.CommandsSensitivenes
            };

            var keywords = printHelper.GetCommandKeyWords();
            // TODO: here eventually construct other subsystems with keywords, and then merge them. Also validate different keywords from different subsystems...

            ValidateKeywords(keywords, commandManager);
            // TODO: switches too?

            return new CommandEngine(commandManager, this._options, this._styles, printHelper, console, this._clipboard);
        }

        /// <summary>
        /// Verifies if any command name is in conflict with built-in commands (mostly help-related)
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="commandManager"></param>
        private static void ValidateKeywords(IEnumerable<string> keywords, CommandManager commandManager)
        {
            var conflictKeywords = keywords.Select(commandManager.FindCommand).Where(cmd => cmd != null).ToArray();
            if (conflictKeywords.Any())
            {
                throw new BadImplementationException($"Following commands are in conflict with keywords: {string.Join(", ", conflictKeywords.GetType().Name)}", typeof(CommandManager));
            }
        }

        /// <summary>
        /// Start fluent syntax
        /// </summary>
        /// <returns></returns>
        public static CommandEngineBuilder Build()
        {
            return new CommandEngineBuilder();
        }
    }
}
