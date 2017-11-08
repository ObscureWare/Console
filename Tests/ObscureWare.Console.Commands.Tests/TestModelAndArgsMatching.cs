namespace ObscureWare.Console.Commands.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Conditions;

    using ObscureWare.Console.Commands.Engine;
    using ObscureWare.Console.Commands.Engine.Internals;
    using ObscureWare.Console.Commands.Engine.Styles;
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Tests.TestModels;
    using ObscureWare.Console.Root.Framework;
    using ObscureWare.Console.Root.Interfaces;
    using ObscureWare.Console.Shared;

    using Shouldly;

    using Xunit;
    using Xunit.Sdk;

    public class TestModelAndArgsMatching
    {
        private IConsole console = new TestConsole(); // could use mock here, but migth like console output from test...

        private ICommandParserOptions simpleSeparatedParsingOptions = new CommandParserOptions
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

        private ICommandParserOptions simpleJoinedParsingOptions = new CommandParserOptions
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

        private MultipleMandatoryCustomSwitches createOrderExpectedModel = new MultipleMandatoryCustomSwitches
        {
            ChannelId = 1,
            DepartmentId = 15,
            MemberId = 20,
            SupplierId = 27,
            UserId = "h423fjf42jjfhj"
        };

        [Theory]
        [MultipleSourceTestData(
            new TypeSource(typeof(MultipleMandatoryCustomSwitchesFakeCmd)),
            new MemberSource(typeof(TestModelAndArgsMatching), nameof(simpleSeparatedParsingOptions)),
            new ValueSource("CreateOrder --sid 27 --mid 20 --did 15 --uid h423fjf42jjfhj --cid 1"),
            new MemberSource(typeof(TestModelAndArgsMatching), nameof(createOrderExpectedModel))
            )]
        [MultipleSourceTestData(
            new TypeSource(typeof(MultipleMandatoryCustomSwitchesFakeCmd)),
            new MemberSource(typeof(TestModelAndArgsMatching), nameof(simpleJoinedParsingOptions)),
            new ValueSource("CreateOrder --sid:27 --mid:20 --did:15 --uid:h423fjf42jjfhj --cid:1"),
            new MemberSource(typeof(TestModelAndArgsMatching), nameof(createOrderExpectedModel))
        )]
        public void various_combination_of_models_shall_be_parsed_properly_no_matter_what_are_parsing_syntax_settings(Type commandType, ICommandParserOptions parserOptions, string commandText, object expectedModel)
        {
            var modelType = typeof(MultipleMandatoryCustomSwitches);

            var validatingCommandLine = CommandLineUtilities.SplitCommandLine(commandText).ToArray(); ;

            var cmdManager = new CommandManager(new Type[] { commandType }, new DefaultCommandResolver());
            var outputManager = new OutputManager(console, CommandEngineStyles.DefaultStyles, CultureInfo.InvariantCulture);

            var cmdInfo = cmdManager.FindCommand(validatingCommandLine[0]);
            var model = this.BuildModelForCommand(cmdInfo, validatingCommandLine.Skip(1), parserOptions, outputManager);

            model.ShouldNotBeNull();
            model.ShouldBeAssignableTo(expectedModel.GetType());
            model.ShouldBe(expectedModel);
        }

        private object BuildModelForCommand(CommandInfo cmdInfo, IEnumerable<string> arguments, ICommandParserOptions options, ICommandOutput outputManager)
        {
            return cmdInfo.CommandModelBuilder.BuildModel(arguments, options, outputManager);
        }

        // TODO: test "same" commands with different settings to verify various syntaxes
    }

    public struct ValueSource : IDataSource
    {
        private readonly object _value;

        public ValueSource(object value)
        {
            this._value = value;
        }

        /// <inheritdoc />
        public object GetData()
        {
            return this._value;
        }
    }

    public struct TypeSource : IDataSource
    {
        private readonly Type _type;

        public TypeSource(Type type)
        {
            type.Requires(nameof(type)).IsNotNull();

            this._type = type;
        }

        /// <inheritdoc />
        public object GetData()
        {
            return this._type;
        }
    }

    public interface IDataSource
    {
        object GetData();
    }

    public struct MemberSource : IDataSource
    {
        public MemberSource(Type memberType, string memberName)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc />
        public object GetData()
        {
            throw new NotImplementedException();
        }
    }

    public class MultipleSourceTestDataAttribute : DataAttribute
    {
        private readonly IDataSource[] _sources;

        public MultipleSourceTestDataAttribute(params IDataSource[] sources)
        {
            sources.Requires(nameof(sources)).IsNotNull();

            this._sources = sources;
        }

        /// <inheritdoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return new [] { this._sources.Select(source => source.GetData()).ToArray() };
        }
    }
}
