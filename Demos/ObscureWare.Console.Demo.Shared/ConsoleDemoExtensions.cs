﻿namespace ObscureWare.Console.Demo.Shared
{
    using System.Drawing;

    using ObscureWare.Console.Root.Shared;

    public static class ConsoleDemoExtensions
    {
        private static readonly ConsoleFontColor DefaultStyle = new ConsoleFontColor(Color.WhiteSmoke, Color.Black);

        public static void WaitForNextPage(this IConsole console)
        {
            console.SetColors(DefaultStyle);
            console.WriteLine();
            console.WriteLine("Press ENTER to continue.");
            console.ReadLine();
        }

        public static void WaitBeforeQuit(this IConsole console)
        {
            console.SetColors(DefaultStyle);
            console.WriteLine();
            console.WriteLine("Holding console window open. Press ENTER to quit for good.");
            console.ReadLine();
        }

        public static void PrintStatus(this IConsole console, string title, object value, StatusStyles statusStyles, StatusStyle usedStyle)
        {
            console.WriteText(statusStyles.TitleStyle, title);
            console.WriteText(statusStyles.SeparatorStyle, statusStyles.SeparatorText);
            console.WriteLine(statusStyles.SelectStyle(usedStyle), value?.ToString() ?? "");
        }
    }
}
