namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(ClsCommand))]
    [CommandName("cls")]
    [CommandDescription(@"Clears / Resets screen of the console.")]
    public class ClsCommandModel : CommandModel
    {
    }
}