namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModel(typeof(ClsCommandModel))]
    public class ClsCommand : IConsoleCommand
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            output.Clear();
        }
    }
}