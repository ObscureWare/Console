namespace ObscureWare.Console.Root.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    using ObscureWare.Console.Root.Shared;

    /// <summary>
    /// Super-simplified color-controlling functionality
    /// </summary>
    public class CoreConsoleController
    {
        private readonly CloseColorFinder _closeColorFinder;


        public CoreConsoleController()
        {
            this._closeColorFinder = new CloseColorFinder(this.GetCurrentColorset());
        }

        /// <summary>
        /// It's unknown how to read / set colors for Core console on UNIX / Linux, therefore just returning some (expected-to-be) defaults...
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<ConsoleColor, Color>[] GetCurrentColorset()
        {
            return new[]
            {
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Black, Color.Black),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.DarkBlue),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGreen, Color.DarkGreen),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.DarkCyan),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkRed, Color.DarkRed),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkMagenta, Color.DarkMagenta),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkYellow, Color.Gold),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Gray, Color.Gray),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGray, Color.DarkGray),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Blue, Color.Blue),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Green, Color.Green),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Cyan, Color.Cyan),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Red, Color.Red),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Magenta, Color.Magenta),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Yellow),
                new KeyValuePair<ConsoleColor, Color>(ConsoleColor.White, Color.White),
            };
        }

        public CloseColorFinder CloseColorFinder => this._closeColorFinder;
    }
}