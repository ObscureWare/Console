namespace ObscureWare.Console.Commands.Engine.Internals.Converters
{
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(string))]
    internal class StringArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return argumentText;
        }
    }
}