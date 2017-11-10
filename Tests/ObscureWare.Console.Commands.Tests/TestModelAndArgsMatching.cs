namespace ObscureWare.Console.Commands.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;
    using ObscureWare.Console.Commands.Tests.TestModels;

    using Xunit;

    public class TestModelAndArgsMatching : ParsingTestsBase
    {
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

        // ReSharper disable once MemberCanBePrivate.Global (Not really, must be public, so it can be discovered by MemberDataAttribute below.)
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
            this.ValidateCommandLine(commandText, commandType, parserOptions, expectedModel as CommandModel);
        }

        // TODO: test _MORE_ "same" commands with different settings to verify various syntaxes
    }
}
