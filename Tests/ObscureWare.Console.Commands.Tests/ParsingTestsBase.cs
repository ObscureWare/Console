namespace ObscureWare.Console.Commands.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using ObscureWare.Console.Commands.Engine;
    using ObscureWare.Console.Commands.Engine.Internals;
    using ObscureWare.Console.Commands.Engine.Styles;
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Shared;

    using Shouldly;

    public class ParsingTestsBase
    {
        protected static IConsole Console = new TestConsole();

        protected object BuildModelForCommand(CommandInfo cmdInfo, IEnumerable<string> arguments, ICommandParserOptions options, ICommandOutput outputManager)
        {
            return cmdInfo.CommandModelBuilder.BuildModel(arguments, options, outputManager);
        }

        protected void ValidateCommandLine<TModel>(string cmd, Type commandType, ICommandParserOptions parserOptions, TModel expectedModel) where TModel : CommandModel
        {
            var validatingCommandLine = CommandLineUtilities.SplitCommandLine(cmd).ToArray(); ;

            var cmdManager = new CommandManager(new Type[] { commandType }, new DefaultCommandResolver());
            var outputManager = new OutputManager(Console, CommandEngineStyles.DefaultStyles, CultureInfo.InvariantCulture);

            var cmdInfo = cmdManager.FindCommand(validatingCommandLine[0]);
            var model = this.BuildModelForCommand(cmdInfo, validatingCommandLine.Skip(1), parserOptions, outputManager);

            model.ShouldNotBeNull();
            model.ShouldBeOfType(expectedModel.GetType());
            model.ShouldBe(expectedModel);
        }
    }
}