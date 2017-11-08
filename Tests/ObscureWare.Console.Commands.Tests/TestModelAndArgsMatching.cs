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
    using ObscureWare.Console.Commands.Tests.TestModels;
    using ObscureWare.Console.Root.Framework;
    using ObscureWare.Console.Root.Interfaces;

    using Shouldly;

    using Xunit;

    public class TestModelAndArgsMatching
    {
        private IConsole console = new TestConsole();


        [Fact]
        public void mdoel_with_multiple_custom_switches_shall_match_properly()
        {
            var parsingOptions = new CommandParserOptions
            {
                UiCulture = CultureInfo.CreateSpecificCulture("en-US"), // "da-DK"
                FlagCharacters = new[] { @"\" },
                SwitchCharacters = new[] { @"--" },
                OptionArgumentMode = CommandOptionArgumentMode.Separated,
                OptionArgumentJoinCharacater = ':', // not used because of: CommandOptionArgumentMode.Separated
                AllowFlagsAsOneArgument = false,
                CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
                SwitchlessOptionsMode =
                    SwitchlessOptionsMode.EndOnly // positional arguments can appear only at the end of command line
            };

            var modelType = typeof(MultipleMandatoryCustomSwitches);
            var commandType = typeof(MultipleMandatoryCustomSwitchesFakeCmd);

            var validatingCommandLine = new string[]
                {"CreateOrder", "--sid", "27", "--mid", "20", "--did", "15", "--uid", "h423fjf42jjfhj", "--cid", "1"};

            var cmdManager = new CommandManager(new Type[] { commandType }, new DefaultCommandResolver());
            var outputManager = new OutputManager(console, CommandEngineStyles.DefaultStyles, CultureInfo.InvariantCulture);

            var cmdInfo = cmdManager.FindCommand(validatingCommandLine[0]);
            var model = this.BuildModelForCommand(cmdInfo, validatingCommandLine.Skip(1), parsingOptions, outputManager);

            model.ShouldNotBeNull();
            model.ShouldBeAssignableTo(modelType);

            MultipleMandatoryCustomSwitches m = model as MultipleMandatoryCustomSwitches;

            m.ChannelId.ShouldBe(1);
            m.DepartmentId.ShouldBe(15);
            m.MemberId.ShouldBe(20);
            m.SupplierId.ShouldBe(27);
            m.UserId.ShouldBe(@"h423fjf42jjfhj");
        }

        private object BuildModelForCommand(CommandInfo cmdInfo, IEnumerable<string> arguments, ICommandParserOptions options, ICommandOutput outputManager)
        {
            return cmdInfo.CommandModelBuilder.BuildModel(arguments, options, outputManager);
        }


    }
}
