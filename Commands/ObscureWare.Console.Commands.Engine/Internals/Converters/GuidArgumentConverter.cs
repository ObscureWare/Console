namespace ObscureWare.Console.Commands.Engine.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(Guid))]
    internal class GuidArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return Guid.Parse(argumentText);
        }
    }
}