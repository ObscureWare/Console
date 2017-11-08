namespace ObscureWare.Console.Commands.Engine
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;

    public class DefaultCommandResolver : IDependencyResolver
    {
        public IConsoleCommand BuildCommand(Type commandType)
        {
            IConsoleCommand instance;
            try
            {
                instance = (IConsoleCommand)Activator.CreateInstance(commandType);
            }
            catch (Exception iex)
            {
                throw new InvalidOperationException($"DefaultCommandResolver was not able to create instance of the command {commandType.FullName} using default activator. Make sure it exposes public, parameterless constructor or provide your custom resolver.", iex);
            }

            return instance;
        }
    }
}