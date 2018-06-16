namespace ObscureWare.Console.Root.Demo
{
    using System;
    using System.Drawing;
    using System.Linq;

    using Console.Demo.Shared;

    using Desktop;
    using Desktop.Scheme;

    using OsInfo;

    using Shared;
    using Shared.ColorBalancing;

    public class ColorHeuriticsDemo : IDemo
    {
        private readonly ColorScheme _scheme = BuildInColorShemes.Windows10Default;

        private readonly IColorHeuristic[] _heuristics = new IColorHeuristic[]
        {
            new GruchenDefaultColorHeuristic(),
            new GruchenNoRgbColorHeuristic(),
            new NearestNeighborHsvColorHeuristic(),
            new NearestNeighborRgbColorHeuristic(),
            new WeightedRgbSimilarityColorHeuristic(),
        }; // TODO: MEF?

        /// <inheritdoc />
        public string Name { get; } = "Color distance heuristics";

        /// <inheritdoc />
        public string Author { get; } = @"Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = "Demonstrates how different heuristics work on same scheme. Using rainbow from Rainbow demo.";

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
                DesiredRowWidth = 128, // for bars
                DesiredRowCount = (uint)(8 + 5 * this._heuristics.Length)   // many samples... 
            });

            console.WriteLine($"Using '{this._scheme.Name}' scheme");
            console.WriteLine();

            this.PrintBaseRainbowColors(console);

            foreach (var heuristic in _heuristics)
            {
                this.PrintSchemeRainbowColors(console, heuristic);
            }

            console.WaitForNextPage();
        }

        private void PrintSchemeRainbowColors(IConsole console, IColorHeuristic heuristics)
        {
            Color foreColor = Color.DarkGray;

            console.WriteLine(@" Heuristics: " + heuristics.Name);
            var balancer = new ColorBalancer(this._scheme, heuristics);

            foreach (int i in Enumerable.Range(0, 127))
            {
                console.SetColors(foreColor, this.LimitScheme(BuildRainbowColor(i), balancer));
                console.WriteText('_');
            }

            NextLine(console);

            foreach (int i in Enumerable.Range(128, 127).Reverse())
            {
                console.SetColors(foreColor, this.LimitScheme(BuildRainbowColor(i), balancer));
                console.WriteText('_');
            }

            NextLine(console);
            NextLine(console);
        }

        private Color LimitScheme(Color color, ColorBalancer balancer)
        {
            var colorIndex = balancer.FindClosestColor(color);
            return Color.FromArgb((int)balancer.Scheme[colorIndex]);
        }

        private void PrintBaseRainbowColors(IConsole console)
        {
            Color foreColor = Color.DarkGray;

            console.WriteLine(@" Referential colors:");

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
            NextLine(console);
        }

        // based on https://github.com/bitcrazed/24bit-color/blob/master/24-bit-color.sh

        private static void NextLine(IConsole console)
        {
            Color foreColor = Color.White;
            Color bgColor = Color.Black;

            console.SetColors(foreColor, bgColor);
            console.WriteLine();
        }

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