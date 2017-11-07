namespace ObscureWare.Console.Commands.Engine.Internals.Parsers
{
    internal interface IParsingResult
    {
        bool IsFine { get; }

        string Message { get; }
    }
}