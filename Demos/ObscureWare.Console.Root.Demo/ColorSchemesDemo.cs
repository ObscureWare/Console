namespace ObscureWare.Console.Root.Demo
{
    using System;
    using System.Drawing;
    using System.Linq;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Demos.Interfaces;
    using ObscureWare.Console.Root.Desktop;
    using ObscureWare.Console.Root.Shared;

    using OsInfo;

    public class ColorSchemesDemo : IDemo
    {
        /// <inheritdoc />
        public string Name { get; } = "Color schemes";

        /// <inheritdoc />
        public string Author { get; } = "Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = "Demonstrates schemes differences using 24-bit color capabilities.";

        /// <inheritdoc />
        public bool CanRun()
        {
            return OsVersion.Win10SystemInfo.IsWindows10;
        }

        /// <inheritdoc />
        public ConsoleDemoSharing ConsoleSharing { get; } = ConsoleDemoSharing.SelfCreate;

        /// <inheritdoc />
        public DemoSet Set { get; } = DemoSet.Root;

        /// <inheritdoc />
        public void Run(IConsole console)
        {
            var controller = new ConsoleController();
            console = new SystemConsole(controller, new ConsoleStartConfiguration(ConsoleStartConfiguration.Colorfull)
            {
                DesiredRowWidth = 128 // for bars
            });

            //RainbowColors(console);

            console.WriteLine(@"TODO");

            console.WaitForNextPage();
        }

        // based on https://github.com/bitcrazed/24bit-color/blob/master/24-bit-color.sh

        private static void RainbowColors(IConsole console)
        {
            Color foreColor = Color.White;
            Color bgColor = Color.Black;

            console.WriteLine(@"TODO");

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
    }
}