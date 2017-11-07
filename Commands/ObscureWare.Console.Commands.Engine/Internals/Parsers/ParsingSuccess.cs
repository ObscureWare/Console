namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    internal class ParsingSuccess : IParsingResult
    {
        private ParsingSuccess()
        {
            
        }

        /// <inheritdoc />
        public bool IsFine { get; } = true;

        /// <inheritdoc />
        public string Message { get; } = @"OK";


        public static IParsingResult Instance { get; } = new ParsingSuccess();
    }
}