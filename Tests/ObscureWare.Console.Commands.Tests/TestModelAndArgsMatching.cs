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
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Shared;

    using Shouldly;

    using Xunit;

    public class TestModelAndArgsMatching
    {
        private IConsole console = new TestConsole(); // could use mock here, but might like console output from test...

        private static ICommandParserOptions simpleSeparatedParsingOptions = new CommandParserOptions
        {
            UiCulture = CultureInfo.CreateSpecificCulture("en-US"), // "da-DK"
            FlagCharacters = new[] { @"\" },
            SwitchCharacters = new[] { @"--" },
            OptionArgumentMode = CommandOptionArgumentMode.Separated,
            OptionArgumentJoinCharacater = ':', // not used because of: CommandOptionArgumentMode.Separated
            AllowFlagsAsOneArgument = false,
            CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
            SwitchlessOptionsMode =
                SwitchlessOptionsMode.EndOnly // positional arguments can appear only at the end of commandText line
        };

        private static ICommandParserOptions simpleJoinedParsingOptions = new CommandParserOptions
        {
            UiCulture = CultureInfo.CreateSpecificCulture("en-US"), // "da-DK"
            FlagCharacters = new[] { @"\" },
            SwitchCharacters = new[] { @"--" },
            OptionArgumentMode = CommandOptionArgumentMode.Joined,
            OptionArgumentJoinCharacater = ':', // not used because of: CommandOptionArgumentMode.Separated
            AllowFlagsAsOneArgument = false,
            CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
            SwitchlessOptionsMode =
                SwitchlessOptionsMode.EndOnly // positional arguments can appear only at the end of commandText line
        };

        private static ICommandParserOptions simpleMergedParsingOptions = new CommandParserOptions
        {
            UiCulture = CultureInfo.CreateSpecificCulture("en-US"), // "da-DK"
            FlagCharacters = new[] { @"\" },
            SwitchCharacters = new[] { @"--" },
            OptionArgumentMode = CommandOptionArgumentMode.Merged,
            OptionArgumentJoinCharacater = ':', // not used because of: CommandOptionArgumentMode.Separated
            AllowFlagsAsOneArgument = false,
            CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
            SwitchlessOptionsMode =
                SwitchlessOptionsMode.EndOnly // positional arguments can appear only at the end of commandText line
        };

        private static ICommandParserOptions amibigousSeparatedParsingOptions = new CommandParserOptions
        {
            UiCulture = CultureInfo.CreateSpecificCulture("en-US"), // "da-DK"
            FlagCharacters = new[] { @"\", @"--" },
            SwitchCharacters = new[] { @"--", @"\" },
            OptionArgumentMode = CommandOptionArgumentMode.Separated,
            OptionArgumentJoinCharacater = ':', // not used because of: CommandOptionArgumentMode.Separated
            AllowFlagsAsOneArgument = false,
            CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
            SwitchlessOptionsMode =
                SwitchlessOptionsMode.EndOnly // positional arguments can appear only at the end of commandText line
        };

        public static IEnumerable<object[]> ParsingTestSets()
        {
            // 1. Multiple custom value switches
            var commandType = typeof(MultipleMandatoryCustomSwitchesFakeCmd);
            var createOrderExpectedModel = new MultipleMandatoryCustomSwitches
            {
                ChannelId = 1,
                DepartmentId = 15,
                MemberId = 20,
                SupplierId = 27,
                UserId = "h423fjf42jjfhj"
            };
            yield return new object[]
            {
                commandType,
                simpleSeparatedParsingOptions,
                "CreateOrder --sid 27 --mid 20 --did 15 --uid h423fjf42jjfhj --cid 1",
                createOrderExpectedModel
            };
            yield return new object[]
            {
                commandType,
                amibigousSeparatedParsingOptions,
                "CreateOrder --sid 27 --mid 20 \\did 15 --uid h423fjf42jjfhj \\cid 1",
                createOrderExpectedModel
            };
            yield return new object[]
            {
                commandType,
                simpleJoinedParsingOptions,
                "CreateOrder --sid:27 --mid:20 --did:15 --uid:h423fjf42jjfhj --cid:1",
                createOrderExpectedModel
            };
            yield return new object[]
            {
                commandType,
                simpleMergedParsingOptions,
                "CreateOrder --sid27 --mid20 --did15 --uidh423fjf42jjfhj --cid1",
                createOrderExpectedModel
            };

            // 2. mixed mode
            commandType = typeof(MixedSwitchesDirCommand);
            var expectedDirModel = new MixedSwitchesDirCommandModel
            {
                Filter = "*.*",
                IncludeFolders = true,
                Mode = DirectoryListMode.CurrentDir
            };
            yield return new object[]
            {
                commandType,
                simpleSeparatedParsingOptions,
                @"dir \d --m CurrentDir *.*",
                expectedDirModel
            };
            yield return new object[]
            {
                commandType,
                amibigousSeparatedParsingOptions,
                @"dir --d \m CurrentDir *.*",
                expectedDirModel
            };
            yield return new object[]
            {
                commandType,
                amibigousSeparatedParsingOptions,
                @"dir \d --m CurrentDir *.*",
                expectedDirModel
            };
            yield return new object[]
            {
                commandType,
                simpleJoinedParsingOptions,
                @"dir \d --m:CurrentDir *.*",
                expectedDirModel
            };
            yield return new object[]
            {
                commandType,
                simpleMergedParsingOptions,
                @"dir \d --mCurrentDir *.*",
                expectedDirModel
            };
        }

        [Theory]
        [MemberData(nameof(ParsingTestSets))]
        public void various_combination_of_models_shall_be_parsed_properly_no_matter_what_are_parsing_syntax_settings(Type commandType, ICommandParserOptions parserOptions, string commandText, object expectedModel)
        {
            var modelType = typeof(MultipleMandatoryCustomSwitches);

            var validatingCommandLine = CommandLineUtilities.SplitCommandLine(commandText).ToArray(); ;

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

        // TODO: test "same" commands with different settings to verify various syntaxes
    }
}
