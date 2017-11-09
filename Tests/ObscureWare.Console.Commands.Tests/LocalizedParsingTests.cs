namespace ObscureWare.Console.Commands.Tests
{
    using Xunit;

    public class LocalizedParsingTests
    {
        [Theory]
        [InlineData("en-US", "ParseFloats -a 12.445 -b 23.454")]
        [InlineData("pl-PL", "ParseFloats -a 12,445 -b 23,454")]
        public void float_and_decimal_values_shall_be_parsed_correctly_according_to_culture(string cultureName, string cmd)
        {
            
        }


        [Theory]
        [InlineData("en-US", "ParseFloats -d 12/14/2017 -t 10:23 -dt \"12/14/2017 10:23:14\"")]
        [InlineData("pl-PL", "ParseFloats -d 14/12/2017 -t 10:23 -dt \"14/12/2017 10:23:14\"")]
        public void date_and_time_values_shall_be_parsed_correctly_according_to_culture(string cultureName, string cmd)
        {

        }

    }
}
