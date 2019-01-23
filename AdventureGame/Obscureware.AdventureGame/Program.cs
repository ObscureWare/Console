using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.AdventureGame
{
    using System;
    using System.Drawing;

    using Console.Demo.Shared;
    using Console.Operations.Implementation;
    using Console.Root.Desktop;
    using Console.Root.Shared;

    class Program
    {
        static void Main(string[] args)
        {
            ConsoleController controller = new ConsoleController();
            //helper.ReplaceConsoleColor(ConsoleColor.DarkCyan, Color.Salmon);
            controller.ReplaceConsoleColors(
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue));

            var consoleConfiguration = new ConsoleStartConfiguration(ConsoleStartConfiguration.GamingFullScreen)
            { 
                RunFullScreen = false,
                DesiredRowWidth = 120,
                DesiredRowCount = 80,
                FontSize = 28
            };

            IConsole console = new SystemConsole(controller, consoleConfiguration);
            ConsoleOperations ops = new ConsoleOperations(console);
 

            ops.WriteTextBox(5, 5, 30, 5, "Game DEMO", new ConsoleFontColor(Color.Aquamarine, Color.Black));

            console.SetCursorPosition(0, 10);

            console.WaitBeforeQuit();

        }
    }

    /// <summary>
    /// This class displays and controls main in-game terminal behavior
    /// </summary>
    public class Terminal
    {
        private List<string> _buffer = new List<string>();



    }
}
