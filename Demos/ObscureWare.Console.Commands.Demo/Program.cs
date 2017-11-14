namespace ObscureWare.Console.Commands.Demo
{
    using System;
    using System.Drawing;

    using ObscureWare.Console.Operations.Implementation;
    using ObscureWare.Console.Operations.Interfaces;
    using ObscureWare.Console.Root.Desktop;
    using ObscureWare.Console.Root.Shared;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            ConsoleController controller = new ConsoleController();
            //helper.ReplaceConsoleColor(ConsoleColor.DarkCyan, Color.Salmon);
            controller.ReplaceConsoleColors(
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue));

            IConsole console = new SystemConsole(controller, isFullScreen: false);
            ConsoleOperations ops = new ConsoleOperations(console);

            TestCommands test = new TestCommands(console);

            console.ReadLine();
        }
    }
}