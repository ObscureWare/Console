namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    internal class ParsingFailure : IParsingResult
    {
        public string Message { get; }

        public ParsingFailure(string errorMessage)
        {
            this.IsFine = false;
            this.Message = errorMessage;
        }

        /// <inheritdoc />
        public bool IsFine { get; }
    }
}