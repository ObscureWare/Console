namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ConsoleTests;
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModel(typeof(ExitCommandModel))]
    public class ExitCommand : IConsoleCommand
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            output.PrintWarning("Terminating application...");
            (contextObject as ConsoleContext).ShallFinishInteracativeSession = true; // let it throw on null
        }
    }
}