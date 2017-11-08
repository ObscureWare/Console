namespace ObscureWare.Console.Commands.Tests
{
    using System.Linq;

    using ObscureWare.Console.Shared;

    using Shouldly;

    using Xunit;

    public class TestCommandLineSplitting
    {

        [Theory]
        [InlineData(@"", new string[] { })]
        [InlineData(@"a", new string[] { "a" })]
        [InlineData(@"'a'", new string[] { "'a'" })]
        [InlineData("\"a\"", new string[] { "a" })]
        public void single_command_shall_be_parsed_properly_in_different_scenarios(string cmd, string[] expected)
        {
            var results = CommandLineUtilities.SplitCommandLine(cmd).ToArray();

            results.ShouldBe(expected);
        }

        [Theory]
        [InlineData(@"--a a", new string[] { "--a", "a" })]
        [InlineData(@"--a 20", new string[] { "--a", "20" })]
        [InlineData(@"-abc 'a'", new string[] { "-abc", "'a'" })]
        [InlineData("--bcd \"a\"", new string[] { "--bcd", "a" })]
        public void tuple_command_shall_be_parsed_properly_in_different_scenarios(string cmd, string[] expected)
        {
            var results = CommandLineUtilities.SplitCommandLine(cmd).ToArray();

            results.ShouldBe(expected);
        }
    }
}
