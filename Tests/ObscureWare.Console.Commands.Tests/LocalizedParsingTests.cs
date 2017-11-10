namespace ObscureWare.Console.Commands.Tests
{
    using System;
    using System.Globalization;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Tests.TestModels;

    using Xunit;

    public class LocalizedParsingTests : ParsingTestsBase
    {
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

            this.ValidateCommandLine(cmd, commandType, parserOptions, expectedModel);
        }

        [Theory]
        [InlineData("en-US", "ParseDateTime -d 12/14/2017 -t 10:23 -dt \"12/14/2017 10:23:14\"")]
        [InlineData("pl-PL", "ParseDateTime -d 14/12/2017 -t 10:23 -dt \"14/12/2017 10:23:14\"")]
        public void date_and_time_values_shall_be_parsed_correctly_according_to_culture(string cultureName, string cmd)
        {
            var parserOptions = BuildParserOptions(cultureName);
            var commandType = typeof(DateTimePropertiesTestCommand);

            var expectedModel = new DateTimePropertiesTestCommandModel()
            {
                D = new DateTime(2017, 12, 14),
                DT = new DateTime(2017, 12, 14, 10, 23, 14),
                T = new TimeSpan(10, 23, 0)
            };

            this.ValidateCommandLine(cmd, commandType, parserOptions, expectedModel);
        }

        private static ICommandParserOptions BuildParserOptions(string cultureName)
        {
            return new CommandParserOptions
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
        }
    }
}
