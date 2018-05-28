namespace ObscureWare.Console.Root.Shared
{
    using System;

    public interface IAtomicConsole : IConsole
    {
        void RunAtomicOperations(Action action);
    }
}