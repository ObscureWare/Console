namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(ChangeDirUpCommand))]
    [CommandName("cd..")]
    [CommandDescription(@"Moves Current Directory one level up.")]
    public class ChangeDirUpCommandModel : CommandModel
    {

    }
}