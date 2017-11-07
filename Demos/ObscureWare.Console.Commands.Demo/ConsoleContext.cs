namespace ObscureWare.Console.Commands.Demo
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;

    public class ConsoleContext : ICommandEngineContext
    {
        public bool ShallFinishInteracativeSession { get; set; }

        public string GetCurrentPrompt()
        {
            string dir = Environment.CurrentDirectory;
            if (!dir.EndsWith("\\"))
            {
                dir += "\\";
            }
            return dir;
        }
    }
}