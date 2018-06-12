namespace ObscureWare.Console.Demo.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using ObscureWare.Console.Operations.Implementation;
    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Shared;

    using OsInfo;

    public class DemoRunner
    {
        private const string NUMBER_SEPARATOR = @". ";

        private ConsoleFontColor StyleActiveTitle { get; } = new ConsoleFontColor(Color.Azure, Color.Black);

        private ConsoleFontColor StyleInactiveTitle { get; } = new ConsoleFontColor(Color.Gray, Color.Black);

        private ConsoleFontColor StyleAuthor { get; } = new ConsoleFontColor(Color.Aqua, Color.Black);

        private ConsoleFontColor StyleDescription { get; } = new ConsoleFontColor(Color.BlueViolet, Color.Black);

        private readonly IDemo[] _demos;

        private readonly OsVersion _osInfo;

        public DemoRunner(IEnumerable<IDemo> demos, OsVersion osInfo)
        {
            _osInfo = osInfo;
            this._demos = demos.ToArray();
        }

        private void ResetDemoConsole(IConsole console)
        {
            // console.ResetSettings(new ConsoleStartConfiguration)...
        }

        private void PrintDemosList(IConsole console)
        {
            int maxNumberLength = (int)Math.Floor(Math.Log10((double)_demos.Length)) + 1;

            int longestDemoName = _demos.Max(d => d.Name.Length);
            int longestAuthorname = _demos.Max(d => d.Author.Length);

            int availableWidth = console.WindowWidth;
            int demoHeaderBiggestWidth = Math.Max(longestDemoName + maxNumberLength + NUMBER_SEPARATOR.Length, longestAuthorname);
            int headerSpaceWithFrames = demoHeaderBiggestWidth + 2; // +2 for frames, more for margins?
            int possibleColumnCount = (int)Math.Floor((decimal)availableWidth / headerSpaceWithFrames);

            int realColumnWidth = (int)Math.Floor((decimal)availableWidth / possibleColumnCount);

            var demoItems = this.BuildDemoItems(realColumnWidth).ToArray();
            var descriptionMaxColumns = demoItems.Max(i => i.DescriptionRows.Length);

            // print frame

            // print items
        }

        private IEnumerable<DemoItem> BuildDemoItems(int realColumnWidth)
        {
            int index = 1;
            foreach (var demo in _demos)
            {
                yield return new DemoItem
                {
                    Demo = demo,
                    Number = index,
                    DisplayTitle = $"{index}{NUMBER_SEPARATOR}{demo.Name}",
                    DescriptionRows = demo.Description?.SplitTextToFit((uint)realColumnWidth).ToArray()
                };

                index++;
            }
        }

        private void PrintSingleDemoItem(IConsole console, DemoItem item, Point start, int maxColumnWidth)
        {
            var op = new ConsoleOperations(console);

            if (item.Demo.CanRun(_osInfo))
            {
                console.PrintLabel(start.X, start.Y, maxColumnWidth, item.DisplayTitle, TextAlign.Center, this.StyleActiveTitle);
            }
            else
            {
                console.PrintLabel(start.X, start.Y, maxColumnWidth, item.DisplayTitle, TextAlign.Center, this.StyleInactiveTitle);
            }

            console.PrintLabel(start.X, start.Y + 1, maxColumnWidth, item.Demo.Author, TextAlign.Center, this.StyleAuthor);

            int index = start.Y + 2;
            foreach (var row in item.DescriptionRows)
            {
                console.PrintLabel(start.X, index++, maxColumnWidth, row, TextAlign.Center, this.StyleDescription);
            }
        }

        private IDemo SelectDemo(IConsole console, IEnumerable<DemoItem> items)
        {

        }
    }

    internal class DemoItem
    {
        public IDemo Demo { get; set; }
        public int Number { get; set; }
        public string DisplayTitle { get; set; }
        public string[] DescriptionRows { get; set; }
    }
}
