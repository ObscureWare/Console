namespace ObscureWare.Console.Commands.Engine.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(Int64))]
    internal class Int64ArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return Int64.Parse(argumentText, culture);
        }
    }
}