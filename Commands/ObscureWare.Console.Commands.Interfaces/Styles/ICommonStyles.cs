namespace ObscureWare.Console.Commands.Interfaces.Styles
{
    using ObscureWare.Console.Root.Interfaces;

    public interface ICommonStyles
    {
        ConsoleFontColor Warning { get; set; }

        ConsoleFontColor Error { get; set; }

        ConsoleFontColor Default { get; set; }

        ConsoleFontColor OddRowColor { get; }

        ConsoleFontColor EvenRowColor { get; }
    }
}