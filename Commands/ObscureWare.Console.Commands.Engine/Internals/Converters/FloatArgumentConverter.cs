namespace ObscureWare.Console.Commands.Engine.Internals.Converters
{
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(float))]
    internal class FloatArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return float.Parse(argumentText, culture);
        }
    }
}