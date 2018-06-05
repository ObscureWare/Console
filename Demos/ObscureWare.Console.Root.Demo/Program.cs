namespace ObscureWare.Console.Root.Demo
{
    using System;
    using System.Drawing;
    using System.Linq;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Root.Desktop;
    using ObscureWare.Console.Root.Shared;

    using OsInfo;

    using Console = System.Console;

    public static class Program
    {
        private const byte ESC = 0x1B;

        private static void Main(string[] args)
        {
            var controller = new ConsoleController();
            var console = new SystemConsole(controller, new ConsoleStartConfiguration(ConsoleStartConfiguration.Colorfull)
            {
                DesiredRowWidth = 128 // for bars
            });

            console.PrintStatus("Virtual Console enabled", console.VirtualConsoleEnabled, StatusStyles.Default, StatusStyles.Default.SelectFlagStyle(console.VirtualConsoleEnabled));
            console.WriteLine();

            PrintOsVersionDemo(console);
            RainbowColors(console);


            // color-full tests


            Console.WriteLine("\x1b[31;32;33;34;35;36;101;102;103;104;105;106;107mThis text attempts to apply many colors in the same command. Note the colors are applied from left to right so only the right-most option of foreground cyan (SGR.36) and background bright white (SGR.107) is effective.\r\n");




            console.WaitBeforeQuit();
        }

        #region 24bit colors

        // based on https://github.com/bitcrazed/24bit-color/blob/master/24-bit-color.sh
        private static void RainbowColors(IConsole console)
        {
            Color foreColor = Color.White;
            Color bgColor = Color.Black;

            console.WriteLine(@"Based on: https://github.com/bitcrazed/24bit-color/blob/master/24-bit-color.sh");
            NextLine(console);

            foreach (int r in Enumerable.Range(0, 127))
            {
                console.SetColors(foreColor, Color.FromArgb(r, 0, 0));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int r in Enumerable.Range(128, 127).Reverse())
            {
                console.SetColors(foreColor, Color.FromArgb(r, 0, 0));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int g in Enumerable.Range(0, 127))
            {
                console.SetColors(foreColor, Color.FromArgb(0, g, 0));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int g in Enumerable.Range(128, 127).Reverse())
            {
                console.SetColors(foreColor, Color.FromArgb(0, g, 0));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int b in Enumerable.Range(0, 127))
            {
                console.SetColors(foreColor, Color.FromArgb(0, 0, b));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int b in Enumerable.Range(128, 127).Reverse())
            {
                console.SetColors(foreColor, Color.FromArgb(0, 0, b));
                console.WriteText('_');
            }

            NextLine(console);
            NextLine(console);

            foreach (int i in Enumerable.Range(0, 127))
            {
                console.SetColors(foreColor, BuildRainbowColor(i));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int i in Enumerable.Range(128, 127).Reverse())
            {
                console.SetColors(foreColor, BuildRainbowColor(i));
                console.WriteText('_');
            }

            NextLine(console);


            console.WaitForNextPage();
        }

        private static void NextLine(IConsole console)
        {
            Color foreColor = Color.White;
            Color bgColor = Color.Black;

            console.SetColors(foreColor, bgColor);
            console.WriteLine();
        }

        /*
        # Gives a color $1/255 % along HSV
        # Who knows what happens when $1 is outside 0-255
        # Echoes "$red $green $blue" where
        # $red $green and $blue are integers
        # ranging between 0 and 255 inclusive
        */
        private static Color BuildRainbowColor(int param1)
        {
            int h = param1 / 43;
            int f = param1 - 43 * h;
            int t = f * 255 / 43;
            int q = 255 - t;

            switch (h)
            {
                case 0: return Color.FromArgb(255, t, 0);
                case 1: return Color.FromArgb(q, 255, 0);
                case 2: return Color.FromArgb(0, 255, t);
                case 3: return Color.FromArgb(0, q, 255);
                case 4: return Color.FromArgb(t, 0, 255);
                case 5: return Color.FromArgb(255, 0, q);

                default: throw new InvalidOperationException(@"should never reach here");
            }
        }

        #endregion

        #region Sys Info

        private static void PrintOsVersionDemo(IConsole console)
        {
            StatusStyles statusStyles = StatusStyles.Default;
            console.PrintStatus("Windows version", OsVersion.Info.VersionString, statusStyles, StatusStyle.Info);
            console.PrintStatus("App bitness", OsVersion.Info.ApplicationBitness, statusStyles, StatusStyle.Info);
            console.PrintStatus("OS bitness", OsVersion.Info.SystemBitness, statusStyles, StatusStyle.Info);
            console.PrintStatus("CPU bitness", OsVersion.Info.ProcessorBitness, statusStyles, StatusStyle.Info);

            console.WriteLine();

            console.PrintStatus("IsWindows10", OsVersion.Win10SystemInfo.IsWindows10, statusStyles, StatusStyle.Info);
            console.PrintStatus("IsMobile", OsVersion.Win10SystemInfo.IsMobile, statusStyles, StatusStyle.Info);
            console.PrintStatus("HasAnniversaryUpdate", OsVersion.Win10SystemInfo.HasAnniversaryUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasAnniversaryUpdate));
            console.PrintStatus("HasApril2018Update", OsVersion.Win10SystemInfo.HasApril2018Update, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasApril2018Update));
            console.PrintStatus("HasCreatorsUpdate", OsVersion.Win10SystemInfo.HasCreatorsUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasCreatorsUpdate));
            console.PrintStatus("HasFallCreatorsUpdate", OsVersion.Win10SystemInfo.HasFallCreatorsUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasFallCreatorsUpdate));
            console.PrintStatus("HasNovemberUpdate", OsVersion.Win10SystemInfo.HasNovemberUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasNovemberUpdate));
            console.PrintStatus("HasRedstone5Update", OsVersion.Win10SystemInfo.HasRedstone5Update, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasRedstone5Update));
            console.PrintStatus("IsThreshold1Version", OsVersion.Win10SystemInfo.IsThreshold1Version, statusStyles,
                statusStyles.SelectFlagStyle(!OsVersion.Win10SystemInfo.IsThreshold1Version));

            console.WaitForNextPage();
        }

        #endregion Sys Info

        #region Animations


        #endregion Animations
    }


}
