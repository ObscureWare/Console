namespace ObscureWare.Console.Commands.Interfaces.Styles
{
    using ObscureWare.Console.Root.Shared;

    public interface ICommonStyles
    {
        ConsoleFontColor Warning { get; set; }

        ConsoleFontColor Error { get; set; }

        ConsoleFontColor Default { get; set; }

        ConsoleFontColor OddRowColor { get; }

        ConsoleFontColor EvenRowColor { get; }
    }
}