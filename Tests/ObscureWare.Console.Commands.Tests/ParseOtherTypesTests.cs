namespace ObscureWare.Console.Commands.Tests
{
    using System;
    using System.Globalization;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Tests.Engine;
    using ObscureWare.Console.Commands.Tests.TestModels;

    using Xunit;

    public class ParseOtherTypesTests : ParsingTestsBase
    {
        [Theory]
        [InlineData("ParseOtherTypes -g C48037DE-E6C8-4D2F-9740-FCF21450AA86")]     // guid-raw
        [InlineData("ParseOtherTypes -g \"C48037DE-E6C8-4D2F-9740-FCF21450AA86\"")] // guid-quoted
        public void other_data_types_shall_be_also_parsed_properly(string cmd)
        {
            var parserOptions = BuildParserOptions();
            var commandType = typeof(ParseOtherTypesCommand);
            var expectedModel = new ParseOtherTypesCommandModel
            {
                G = new Guid("C48037DE-E6C8-4D2F-9740-FCF21450AA86")
            };

            this.ValidateCommandLine(cmd, commandType, parserOptions, expectedModel);
        }

        private static ICommandParserOptions BuildParserOptions()
        {
            return new CommandParserOptions
            {
                UiCulture = CultureInfo.CreateSpecificCulture("en-EN"),
                FlagCharacters = new[] { @"\" },
                SwitchCharacters = new[] { @"-" },
                OptionArgumentMode = CommandOptionArgumentMode.Separated,
                OptionArgumentJoinCharacater = ':',
                AllowFlagsAsOneArgument = false,
                CommandsSensitivenes = CommandCaseSensitivenes.Insensitive,
                SwitchlessOptionsMode =
                    SwitchlessOptionsMode.EndOnly
            };
        }
    }
}