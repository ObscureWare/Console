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
    using ObscureWare.Console.Commands.Tests.TestModels;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Shared;

    using Shouldly;

    using Xunit;

    public class LocalizedParsingTests
    {
        private static IConsole console = new TestConsole();

        [Theory]
        [InlineData("en-US", "ParseFloats -a 12.445 -b 23.454 -d 67.33")]
        [InlineData("pl-PL", "ParseFloats -a 12,445 -b 23,454 -d 67,33")]
        public void float_and_decimal_values_shall_be_parsed_correctly_according_to_culture(string cultureName, string cmd)
        {
            var parserOptions = BuildParserOptions(cultureName);
            var commandType = typeof(FloatPropertiesTestCommand);

            var expectedModel = new FloatPropertiesTestCommandModel
            {
                A = 12.445,
                B = 23.454f,
                D = 67.33m
            };

            var validatingCommandLine = CommandLineUtilities.SplitCommandLine(cmd).ToArray(); ;

            var cmdManager = new CommandManager(new Type[] { commandType }, new DefaultCommandResolver());
            var outputManager = new OutputManager(console, CommandEngineStyles.DefaultStyles, CultureInfo.InvariantCulture);

            var cmdInfo = cmdManager.FindCommand(validatingCommandLine[0]);
            var model = this.BuildModelForCommand(cmdInfo, validatingCommandLine.Skip(1), parserOptions, outputManager);

            model.ShouldNotBeNull();
            model.ShouldBeOfType(expectedModel.GetType());
            model.ShouldBe(expectedModel);
        }

        [Theory]
        [InlineData("en-US", "ParseDateTime -d 12/14/2017 -t 10:23 -dt \"12/14/2017 10:23:14\"")]
        [InlineData("pl-PL", "ParseDateTime -d 14/12/2017 -t 10:23 -dt \"14/12/2017 10:23:14\"")]
        public void date_and_time_values_shall_be_parsed_correctly_according_to_culture(string cultureName, string cmd)
        {
            var simpleSeparatedParsingOptions = BuildParserOptions(cultureName);

            var parserOptions = BuildParserOptions(cultureName);
            var commandType = typeof(DateTimePropertiesTestCommand);

            var expectedModel = new DateTimePropertiesTestCommandModel()
            {
                D = new DateTime(2017, 12, 14),
                DT = new DateTime(2017, 12, 14, 10, 23, 14),
                T = new TimeSpan(10, 23, 0)
            };

            var validatingCommandLine = CommandLineUtilities.SplitCommandLine(cmd).ToArray(); ;

            var cmdManager = new CommandManager(new Type[] { commandType }, new DefaultCommandResolver());
            var outputManager = new OutputManager(console, CommandEngineStyles.DefaultStyles, CultureInfo.InvariantCulture);

            var cmdInfo = cmdManager.FindCommand(validatingCommandLine[0]);
            var model = this.BuildModelForCommand(cmdInfo, validatingCommandLine.Skip(1), parserOptions, outputManager);

            model.ShouldNotBeNull();
            model.ShouldBeOfType(expectedModel.GetType());
            model.ShouldBe(expectedModel);
        }

        private object BuildModelForCommand(CommandInfo cmdInfo, IEnumerable<string> arguments, ICommandParserOptions options, ICommandOutput outputManager)
        {
            return cmdInfo.CommandModelBuilder.BuildModel(arguments, options, outputManager);
        }

        private static ICommandParserOptions BuildParserOptions(string cultureName)
        {
            ICommandParserOptions simpleSeparatedParsingOptions = new CommandParserOptions
            {
                UiCulture = CultureInfo.CreateSpecificCulture(cultureName),
                FlagCharacters = new[] { @"\" },
                SwitchCharacters = new[] { @"-" },
                OptionArgumentMode = CommandOptionArgumentMode.Separated,
                OptionArgumentJoinCharacater = ':',
                AllowFlagsAsOneArgument = false,
                CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
                SwitchlessOptionsMode =
                    SwitchlessOptionsMode.EndOnly
            };
            return simpleSeparatedParsingOptions;
        }
    }
}
