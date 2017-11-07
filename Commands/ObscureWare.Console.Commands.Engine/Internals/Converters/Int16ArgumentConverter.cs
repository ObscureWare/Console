namespace ObscureWare.Console.Commands.Engine.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(Int16))]
    internal class Int16ArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return Int16.Parse(argumentText, culture);
        }
    }
}