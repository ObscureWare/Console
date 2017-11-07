namespace ObscureWare.Console.Commands.Engine.Internals.Converters
{
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(byte))]
    internal class ByteArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return byte.Parse(argumentText, culture);
        }
    }
}