namespace ObscureWare.Console.Demo.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;

    using Demos.Interfaces;

    using Operations.Implementation;
    using Root.Shared;
    using Console.Shared;

    using OsInfo;

    public class DemoRunner
    {
        private const string NUMBER_SEPARATOR = @". ";

        private readonly LabelStyle _styleActiveTitle = new LabelStyle(Color.Azure, Color.Black, TextAlign.Center);

        private readonly LabelStyle _styleInactiveTitle = new LabelStyle(Color.Gray, Color.Black, TextAlign.Center);

        private readonly LabelStyle _styleAuthor  = new LabelStyle(Color.Aqua, Color.Black, TextAlign.Center);

        private readonly LabelStyle _styleDescription  = new LabelStyle(Color.BlueViolet, Color.Black, TextAlign.Center);

        private readonly ConsoleFontColor _promptStyle = new ConsoleFontColor(Color.Yellow, Color.Black);

        private readonly IDemo[] _demos;

        private readonly OsVersion _osInfo;

        private int _selectionRow;
        

        public DemoRunner(IEnumerable<IDemo> demos, OsVersion osInfo)
        {
            this._osInfo = osInfo;
            this._demos = demos.ToArray();
        }

        public void RunDemos(IConsole console)
        {
            console.Clear();
            var items = this.PrintDemosList(console).ToArray();
            IDemo demo;
            while ((demo = this.SelectDemo(console, items)) != null)
            {
                console.Clear();

                demo.Run(demo.ConsoleSharing == ConsoleDemoSharing.CanShare ? console : null);

                console.Clear();
                items = this.PrintDemosList(console).ToArray(); // in case of resizing during demo layout could be updated...
            }
        }

        private void ResetDemoConsole(IConsole console)
        {
            // console.ResetSettings(new ConsoleStartConfiguration)...
        }

        private IEnumerable<DemoItem> PrintDemosList(IConsole console)
        {
            int maxNumberLength = (int)Math.Floor(Math.Log10((double) this._demos.Length)) + 1;

            int longestDemoName = this._demos.Max(d => d.Name.Length);
            int longestAuthorname = this._demos.Max(d => d.Author.Length);

            int availableWidth = console.WindowWidth;
            int demoHeaderBiggestWidth = Math.Max(longestDemoName + maxNumberLength + NUMBER_SEPARATOR.Length, longestAuthorname);
            int headerSpaceWithFrames = demoHeaderBiggestWidth + 2; // +2 for frames, more for margins?
            int possibleColumnCount = (int)Math.Floor((decimal)availableWidth / headerSpaceWithFrames);

            int realColumnWidth = (int)Math.Floor((decimal)availableWidth / possibleColumnCount);

            var demoItems = this.BuildDemoItems(realColumnWidth).ToArray();
            var descriptionMaxRows = demoItems.Max(i => i.DescriptionRows.Length);

            // print frame
            // TODO:
            // print items

            // this is row at which demo selection prompt will be displayed;
            this._selectionRow = (descriptionMaxRows + 2 + 1) * this._demos.Length / possibleColumnCount + 1;

            return demoItems;
        }

        private IEnumerable<DemoItem> BuildDemoItems(int realColumnWidth)
        {
            int index = 1;
            foreach (var demo in this._demos)
            {
                yield return new DemoItem
                {
                    Demo = demo,
                    Number = index,
                    DisplayTitle = $"{index}{NUMBER_SEPARATOR}{demo.Name}",
                    DescriptionRows = demo.Description?.SplitTextToFit((uint)realColumnWidth).ToArray(),
                    Enabled = demo.CanRun(this._osInfo)
                };

                index++;
            }
        }

        private void PrintSingleDemoItem(IConsole console, DemoItem item, Point start, int maxColumnWidth)
        {
            var op = new ConsoleOperations(console);

            console.PrintLabel(start.X, start.Y, maxColumnWidth, item.DisplayTitle,
                (item.Enabled) ? this._styleActiveTitle : this._styleInactiveTitle);

            console.PrintLabel(start.X, start.Y + 1, maxColumnWidth, item.Demo.Author, this._styleAuthor);

            int index = start.Y + 2;
            foreach (var row in item.DescriptionRows)
            {
                console.PrintLabel(start.X, index++, maxColumnWidth, row, this._styleDescription);
            }
        }

        private IDemo SelectDemo(IConsole console, DemoItem[] items)
        {
            IDemo demo = null;

            while (demo == null)
            {
                console.CleanLine(this._selectionRow);
                console.WriteText(0, this._selectionRow, @"Select demo number (type 'exit' to quit): ",
                    this._promptStyle.ForeColor, this._promptStyle.BgColor);

                string txt = console.ReadLine(); // TODO: virtual, length-limited prompt when done
                if ("exit".Equals(txt, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                int.TryParse(txt, NumberStyles.Integer, CultureInfo.InvariantCulture, out int selectedIndex);
                if (selectedIndex <= 0 && selectedIndex >= this._demos.Length)
                {
                    continue;
                }

                var demoItem = items[selectedIndex - 1];
                if (demoItem.Enabled)
                {
                    demo = demoItem.Demo;
                }
            }

            return demo;
        }
    }
}
