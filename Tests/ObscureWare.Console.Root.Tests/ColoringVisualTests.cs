namespace ObscureWare.Console.Root.Tests
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.IO;
    using System.Reflection;

    using ObscureWare.Console.Root.Desktop;

    using Xunit;
    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Shared;

    public class ColoringVisualTests
    {
        private const string TEST_ROOT = @"C:\\TestResults\\ConsoleColors\\";

        // This is obviously not possible to programmatically verify how "close" were colors matched - it's all manual testing required...
        // Therefore - help pages will be rendered for default and some customized results

        [Fact]
        public void PrintDefaultColorsTest()
        {
            string fName = "default_setup.html";
            using (var colorHelper = new CloseColorFinder(PredefinedColorSets.Windows10Definitions().ToArray()).GetDefault())
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrintCustomizedColorsBySeba()
        {
            string fName = "custom_setup_seba.html";
            using (var colorHelper = new CloseColorFinder(PredefinedColorSets.Windows10Definitions().ToArray()).CustomizedDefault(
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue)))
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrintCustomizedColorsByDnv()
        {
            var converter = new ColorConverter();

            string fName = "custom_setup_dnv.html";
            using (
                var colorHelper = new CloseColorFinder(PredefinedColorSets.Windows10Definitions().ToArray()).CustomizedDefault(
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Cyan, (Color)converter.ConvertFromString("#99d9f0")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan,
                        (Color)converter.ConvertFromString("#e98300")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, (Color)converter.ConvertFromString("#009fda")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, (Color)converter.ConvertFromString("#fecb00")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkGreen,
                        (Color)converter.ConvertFromString("#36842d")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, (Color)converter.ConvertFromString("#003591")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Magenta,
                        (Color)converter.ConvertFromString("#635091")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkRed,
                        (Color)converter.ConvertFromString("#c4262e")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue,
                        (Color)converter.ConvertFromString("#0f204b")),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkGray,
                        (Color)converter.ConvertFromString("#988f86"))))
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        private static void PrintAllNamedColorsToHtml(CloseColorFinder helper, string fName)
        {
            var props = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Color));

            var colorsVersionAtt = typeof(CloseColorFinder).Assembly.GetCustomAttributes()
                .FirstOrDefault(att => att is AssemblyFileVersionAttribute) as AssemblyFileVersionAttribute;
            string colorsVersion = colorsVersionAtt?.Version ?? "unknown";

            var dir = $"{TEST_ROOT}{colorsVersion}\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var path = Path.Combine(dir, fName);

            using (var tw = new StreamWriter(path))
            {
                tw.WriteLine("<html><body><table>");
                tw.WriteLine("<tr><th>ColorName</th><th>Sys Color</th><th>Console Color</th></tr>");

                foreach (var propertyInfo in props)
                {
                    tw.WriteLine("<tr>");

                    Color c = (Color)propertyInfo.GetValue(null);
                    ConsoleColor cc = helper.FindClosestColor(c);

                    Color cCol = helper.GetCurrentConsoleColor(cc);
                    var ccName = Enum.GetName(typeof(ConsoleColor), cc);

                    tw.WriteLine(
                        $"<td>{propertyInfo.Name}</td><td bgcolor=\"{c.ToRgbHex()}\">{c.Name}</td><td bgcolor=\"{cCol.ToRgbHex()}\">{ccName}</td>");
                    tw.WriteLine("</tr>");
                }

                tw.WriteLine("</table></body></html>");
                tw.Close();
            }
        }
    }
}
