namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(ExitCommand))]
    [CommandName("exit")]
    [CommandDescription(@"Immediately terminates the application.")]
    public class ExitCommandModel : CommandModel
    {
    }
}