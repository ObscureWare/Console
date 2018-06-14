namespace ObscureWare.Console.Demo.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Shared;

    public class DemoRunner
    {
        private const string NUMBER_SEPARATOR = @". ";

        private readonly LabelStyle _styleActiveTitle = new LabelStyle(Color.Azure, Color.FromArgb(10, 10, 10), TextAlign.Center);

        private readonly LabelStyle _styleInactiveTitle = new LabelStyle(Color.Gray, Color.FromArgb(10, 10, 10), TextAlign.Center);

        private readonly LabelStyle _styleAuthor = new LabelStyle(Color.CadetBlue, Color.FromArgb(15, 15, 15), TextAlign.Center);

        private readonly LabelStyle _styleDescription = new LabelStyle(Color.CornflowerBlue, Color.FromArgb(25, 25, 25), TextAlign.Center);

        private readonly LabelStyle _styleHeader = new LabelStyle(Color.Gold, Color.DimGray, TextAlign.Center);
        private readonly LabelStyle _styleHeaderBorder = new LabelStyle(Color.DimGray, Color.FromArgb(30, 30, 30), TextAlign.Center);

        private readonly ConsoleFontColor _promptStyle = new ConsoleFontColor(Color.Gold, Color.Black);

        private readonly ConsoleFontColor _promptLabelStyle = new ConsoleFontColor(Color.WhiteSmoke, Color.DimGray);

        private readonly IDemo[] _demos;

        private int _selectionRow;


        public DemoRunner(IEnumerable<IDemo> demos)
        {
            this._demos = demos.ToArray();
        }

        public void RunDemos(IConsole console)
        {
            console.Clear();
            var items = this.PrintDemosList(console).ToArray();
            IDemo demo;
            while ((demo = this.SelectDemo(console, items)) != null)
            {
                console.SetColors(Color.WhiteSmoke, Color.Black); // TODO: default reset?
                console.Clear();

                demo.Run(demo.ConsoleSharing == ConsoleDemoSharing.CanShare ? console : null);

                console.SetColors(Color.WhiteSmoke, Color.Black); // TODO: default reset?
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
            int availableWidth = console.WindowWidth;

            int maxNumberLength = (int)Math.Floor(Math.Log10((double)this._demos.Length)) + 1;

            int longestDemoName = this._demos.Max(d => d.Name.Length);
            int longestAuthorname = this._demos.Max(d => d.Author.Length);

            int demoHeaderBiggestWidth = Math.Max(longestDemoName + maxNumberLength + NUMBER_SEPARATOR.Length, longestAuthorname);
            int headerSpaceWithFrames = demoHeaderBiggestWidth + 2; // +2 for frames, more for margins? or just leave margins?
            int possibleColumnCount = (int)Math.Floor((decimal)availableWidth / headerSpaceWithFrames);

            int realColumnWidth = (int)Math.Floor((decimal)availableWidth / possibleColumnCount) - 2;

            var demoItems = this.BuildDemoItems(realColumnWidth).ToArray();
            var descriptionMaxRows = demoItems.Max(i => i.DescriptionRows.Length);
            var maxItemHeight = descriptionMaxRows + 3; // desc + header + author + separator

            console.PrintLabel(0, 0, availableWidth, "", _styleHeaderBorder);
            console.PrintLabel(0, 1, availableWidth, "Available Demos", _styleHeader);
            console.PrintLabel(0, 2, availableWidth, "", _styleHeaderBorder);

            // print frame
            // TODO:
            // print items
            var itemIndex = 1;
            int menuStartIndex = 3;
            foreach (var item in demoItems)
            {
                var culumnNumber = itemIndex % possibleColumnCount; // 1-based
                var posX = 1 + (1 + realColumnWidth) * (culumnNumber - 1);
                var rowNumber = (int)Math.Floor((decimal)itemIndex / possibleColumnCount); // 0-based
                var posY = (maxItemHeight + 1) * rowNumber + menuStartIndex; // +1 for frame/margin between menu rows

                this.PrintSingleDemoItem(console, item, new Point(posX, posY), realColumnWidth, descriptionMaxRows);

                itemIndex++;
            }

            // this is row at which demo selection prompt will be displayed;
            this._selectionRow = (descriptionMaxRows + 2 + 1) * this._demos.Length / possibleColumnCount + menuStartIndex + 3; // some extra spacing below last menu row

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
                    Enabled = demo.CanRun()
                };

                index++;
            }
        }

        private void PrintSingleDemoItem(IConsole console, DemoItem item, Point start, int maxColumnWidth, int descriptionMaxRows)
        {
            console.PrintLabel(start.X, start.Y, maxColumnWidth, item.DisplayTitle,
                (item.Enabled) ? this._styleActiveTitle : this._styleInactiveTitle);

            console.PrintLabel(start.X, start.Y + 1, maxColumnWidth, item.Demo.Author, this._styleAuthor);

            // separator
            console.PrintLabel(start.X, start.Y + 2, maxColumnWidth, "", this._styleDescription);

            // description
            int index = 0;
            foreach (var row in item.DescriptionRows)
            {
                console.PrintLabel(start.X, start.Y + 3 + index++, maxColumnWidth, row, this._styleDescription);
            }

            //trailing empty rows
            for (int i = index; i < descriptionMaxRows; i++)
            {
                console.PrintLabel(start.X, start.Y + 3 + i, maxColumnWidth, "", this._styleDescription);
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

                console.SetColors(this._promptLabelStyle);
                string txt = console.ReadLine(); // TODO: virtual, length-limited prompt when done

                if ("exit".Equals(txt, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                int.TryParse(txt, NumberStyles.Integer, CultureInfo.InvariantCulture, out int selectedIndex);
                if (selectedIndex <= 0 || selectedIndex > this._demos.Length)
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
