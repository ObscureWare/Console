namespace ObscureWare.Console.Operations.Demo
{
    using System;
    using System.Drawing;
    using System.Globalization;

    using Console.Demo.Shared;

    using Implementation.TablePrinters;
    using Implementation.Tables;

    using Interfaces.Styles;
    using Interfaces.Tables;

    using ObscureWare.Tests.Common;

    using Root.Shared;

    public class TablePrintingDemo : IDemo
    {
        /// <inheritdoc />
        public string Name { get; } = @"Tables demonstration";

        /// <inheritdoc />
        public string Author { get; } = @"Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = @"Presents several ways to display tabelaric content.";

        /// <inheritdoc />
        public bool CanRun()
        {
            return true;
        }

        /// <inheritdoc />
        public ConsoleDemoSharing ConsoleSharing { get; } = ConsoleDemoSharing.CanShare;

        /// <inheritdoc />
        public DemoSet Set { get; } = DemoSet.Operations;

        public void Run(IConsole console)
        {
            var tableFrameColor = new ConsoleFontColor(Color.Silver, Color.Black);
            var tableHeaderColor = new ConsoleFontColor(Color.White, Color.Black);
            var tableOddRowColor = new ConsoleFontColor(Color.Silver, Color.Black);
            var tableEvenRowColor = new ConsoleFontColor(Color.DimGray, Color.Black);

            TableStyle tableStyle = new TableStyle(
                tableFrameColor,
                tableHeaderColor,
                tableOddRowColor,
                tableEvenRowColor,
                @"|-||||-||-|--", // simple, ascii table
                ' ',
                TableOverflowContentBehavior.Ellipsis);

            TableStyle wrappingTableStyle = new TableStyle(
                tableFrameColor,
                tableHeaderColor,
                tableOddRowColor,
                tableEvenRowColor,
                @"|-||||-||-|--", // simple, ascii table
                ' ',
                TableOverflowContentBehavior.Wrap);

            var headers = new[] { "Row 1", "Longer row 2", "Third row" };
            var values = new[]
            {
                new[] {"1", "2", "3"},
                new[] {"10", "223423", "3"},
                new[] {"1", "2", "3"},
                new[] {"12332 ", "22332423", "3223434234"},
                new[] {"1df ds fsd fsfs fsdf s", "2234  4234 23", "3 23423423"},
            };

            var simpleTableStyleWithWrap = new SimpleTableStyle(
                tableHeaderColor,
                tableEvenRowColor,
                TableOverflowContentBehavior.Wrap)
            {
                EvenRowColor = tableOddRowColor
            };

            var simpleTableStyleWithEllipsis = new SimpleTableStyle(tableHeaderColor, tableEvenRowColor)
            {
                EvenRowColor = tableOddRowColor
            };

            // ops.WriteTabelaricData(5, 5, 50, headers, values, tableStyle);


            console.WriteLine(tableFrameColor, "Small tables");

            DataTable<string> dt = new DataTable<string>(
                new ColumnInfo("Column a", ColumnAlignment.Left),
                new ColumnInfo("Column B", ColumnAlignment.Left),
                new ColumnInfo("Column V1", ColumnAlignment.Right),
                new ColumnInfo("Column V2", ColumnAlignment.Right));

            for (int i = 0; i < 20; i++)
            {
                dt.AddRow(
                    i.ToString(),
                    new[]
                    {
                        TestTools.AlphanumericIdentifier.BuildRandomStringFrom(5, 10).Trim(),
                        TestTools.AlphaSentence.BuildRandomStringFrom(4, 15).Trim(),
                        TestTools.GetRandomFloat(10000).ToString("N2", CultureInfo.CurrentCulture),
                        TestTools.GetRandomFloat(30000).ToString("N2", CultureInfo.CurrentCulture)
                    });
            }

            SimpleTablePrinter simpleTablePrinter = new SimpleTablePrinter(console, simpleTableStyleWithEllipsis);
            SimpleTablePrinter simpleTableWithWrapping = new SimpleTablePrinter(console, simpleTableStyleWithWrap);
            FramedTablePrinter framedPrinter = new FramedTablePrinter(console, tableStyle);
            SpeflowStyleTablePrinter specflowPrinter = new SpeflowStyleTablePrinter(console, tableStyle);
            var specflowTableWithWrapping = new SpeflowStyleTablePrinter(console, wrappingTableStyle);

            simpleTablePrinter.PrintTable(dt);
            Console.WriteLine();

            simpleTableWithWrapping.PrintTable(dt);
            Console.WriteLine();

            framedPrinter.PrintTable(dt);
            Console.WriteLine();

            specflowPrinter.PrintTable(dt);
            Console.WriteLine();

            specflowTableWithWrapping.PrintTable(dt);
            Console.WriteLine();

            Console.ReadLine();

            console.WriteLine(tableFrameColor, "Positioned tables");
            Console.WriteLine();

            // TODO: PrintTableAt(dt, x, y);

            console.WaitForNextPage();

            console.WriteLine(tableFrameColor, "Large tables");
            Console.WriteLine();

            dt = new DataTable<string>(
                new ColumnInfo("Column A1", ColumnAlignment.Left),
                new ColumnInfo("Column B", ColumnAlignment.Left),
                new ColumnInfo("Column C", ColumnAlignment.Left),
                new ColumnInfo("Column V1", ColumnAlignment.Right, fixedLength: 9),
                new ColumnInfo("Column V2", ColumnAlignment.Right, fixedLength: 9),
                new ColumnInfo("Column VXX", ColumnAlignment.Right, fixedLength: 12));

            for (int i = 0; i < 20; i++)
            {
                dt.AddRow(
                    i.ToString(),
                    new[]
                    {
                        TestTools.UpperAlphanumeric.BuildRandomStringFrom(10, 15).Trim(),
                        TestTools.AlphanumericIdentifier.BuildRandomStringFrom(8, 40).Trim(),
                        TestTools.AlphaSentence.BuildRandomStringFrom(20, 50).Trim(),
                        TestTools.GetRandomFloat(10000).ToString("N2", CultureInfo.CurrentCulture),
                        TestTools.GetRandomFloat(50000).ToString("N2", CultureInfo.CurrentCulture),
                        TestTools.GetRandomFloat(3000000).ToString("N2", CultureInfo.CurrentCulture)
                    });
            }

            simpleTablePrinter.PrintTable(dt);
            Console.WriteLine();

            simpleTableWithWrapping.PrintTable(dt);
            Console.WriteLine();

            framedPrinter.PrintTable(dt);
            Console.WriteLine();

            // TODO: also wrapping framed table

            specflowPrinter.PrintTable(dt);
            Console.WriteLine();

            specflowTableWithWrapping.PrintTable(dt);
            Console.WriteLine();

            console.WriteLine(tableFrameColor, "");


            console.WaitForNextPage();
        }
    }
}