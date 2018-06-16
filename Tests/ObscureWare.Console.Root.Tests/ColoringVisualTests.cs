namespace ObscureWare.Console.Root.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.IO;
    using System.Reflection;

    using Desktop.Scheme;

    using Xunit;
    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Shared;

    using Shared.ColorBalancing;

    public class ColoringVisualTests
    {
        private const string TEST_ROOT = @"C:\\TestResults\\ConsoleColors\\";

        private readonly ColorScheme _gruchaScheme;
        private readonly ColorScheme _dnvScheme;
        private readonly IEnumerable<PropertyInfo> _props;
        readonly AssemblyFileVersionAttribute _colorsVersionAtt;

        public ColoringVisualTests()
        {
            var converter = new ColorConverter();

            this._gruchaScheme = BuildInColorShemes.Windows10Default.CustomizeScheme(
                @"Grucha Scheme",
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue));

            this._dnvScheme = BuildInColorShemes.Windows10Default.CustomizeScheme(
                @"DNV scheme",
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
                    (Color)converter.ConvertFromString("#988f86")));

            this._props = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Color));

            this._colorsVersionAtt = typeof(ColorBalancer).Assembly.GetCustomAttributes()
                .FirstOrDefault(att => att is AssemblyFileVersionAttribute) as AssemblyFileVersionAttribute;
        }

        private readonly IColorHeuristic[] _heuristics = new IColorHeuristic[]
        {
            new GruchenDefaultColorHeuristic(),
            new GruchenNoRgbColorHeuristic(),
            new NearestNeighborHsvColorHeuristic(),
            new NearestNeighborRgbColorHeuristic(),
            new WeightedRgbSimilarityColorHeuristic(),
        }; // TODO: MEF?

        // This is obviously not possible to programmatically verify how "close" were colors matched - it's all manual testing required...
        // Therefore - help pages will be rendered for default and some customized results

        [Fact]
        public void PrintDefaultColorsTest()
        {
            string fName = "default_setup.html";
            using (var colorHelper = new ColorBalancer(BuildInColorShemes.WindowsDefault, new GruchenDefaultColorHeuristic()))
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrintWin10DefaultColorsTest()
        {
            string fName = "default_win10_setup.html";
            using (var colorHelper = new ColorBalancer(BuildInColorShemes.Windows10Default, new GruchenDefaultColorHeuristic()))
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrintCustomizedColorsBySeba()
        {
            string fName = "custom_setup_seba.html";
            using (var colorHelper = new ColorBalancer(this._gruchaScheme, new GruchenDefaultColorHeuristic()))
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrintCustomizedColorsByDnv()
        {
            string fName = "custom_setup_dnv.html";
            using (var colorHelper = new ColorBalancer(this._dnvScheme, new GruchenDefaultColorHeuristic()))
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrinAllschemesWithAllHeauristicsTest()
        {
            var fileSchemes = SchemeLoader.LoadAllFromFolder(@"..\..\..\..\Demos\colorschemes");
            var schemes = (new[] { BuildInColorShemes.WindowsDefault, BuildInColorShemes.Windows10Default })
                .Concat(fileSchemes)
                .Concat(new [] {this._gruchaScheme, this._dnvScheme })
                .ToArray();

            foreach (var colorScheme in schemes)
            {
                this.PrintHeuriticComparisonsColorsToHtml(colorScheme);
            }
        }

        private void PrintHeuriticComparisonsColorsToHtml(ColorScheme colorScheme)
        {
            string path = this.GetOutputFolder("AllSchemes", colorScheme.Name + ".html");

            using (var tw = new StreamWriter(path))
            {
                tw.WriteLine("<html><body>");

                // print header
                int index = 0;
                tw.WriteLine($"<html><body><h3>Scheme:&nbsp;{colorScheme.Name}</h3><table><tr>");
                foreach (uint ci in colorScheme.GetAll())
                {
                    Color c = Color.FromArgb((int)ci);
                    tw.Write($"<td bgcolor=\"{c.ToRgbHex()}\">{((ConsoleColor)index++)}</td>");
                }
                tw.WriteLine("</tr><tr>");
                foreach (uint ci in colorScheme.GetAll())
                {
                    Color c = Color.FromArgb((int)ci);
                    tw.Write($"<td bgcolor=\"{c.ToRgbHex()}\">{c.Name}</td>");
                }
                tw.WriteLine("</tr></table></br>");

                var balancers = this._heuristics.Select(h => new ColorBalancer(colorScheme, h)).ToArray();

                // print colors
                tw.WriteLine("<table><tr><th>ColorName</th><th>Sys Color</th>");
                foreach (var h in _heuristics)
                {
                    tw.Write($"<th>{h.Name}</th>");
                }
                tw.WriteLine("</tr>");

                foreach (var propertyInfo in this._props)
                {
                    tw.WriteLine("<tr>");

                    Color c = (Color)propertyInfo.GetValue(null);
                    

                    tw.Write($"<td>{propertyInfo.Name}</td><td bgcolor=\"{c.ToRgbHex()}\">{c.Name}</td>");

                    foreach (var balancer in balancers)
                    {
                        ConsoleColor cc = balancer.FindClosestColor(c);

                        Color cCol = balancer.GetCurrentConsoleColor(cc);
                        var ccName = Enum.GetName(typeof(ConsoleColor), cc);

                        tw.Write($"<td bgcolor=\"{cCol.ToRgbHex()}\">{ccName}</td>");
                    }

                    tw.WriteLine("</tr>");
                }


                tw.WriteLine("</table>");


                tw.WriteLine("</body></html>");
                tw.Close();
            }
        }

        private void PrintAllNamedColorsToHtml(ColorBalancer helper, string fName)
        {
            string path = this.GetOutputFolder(fName);

            using (var tw = new StreamWriter(path))
            {
                tw.WriteLine("<html><body><table>");
                tw.WriteLine("<tr><th>ColorName</th><th>Sys Color</th><th>Console Color</th></tr>");

                foreach (var propertyInfo in this._props)
                {
                    tw.WriteLine("<tr>");

                    Color c = (Color)propertyInfo.GetValue(null);
                    ConsoleColor cc = helper.FindClosestColor(c);

                    Color cCol = helper.GetCurrentConsoleColor(cc);
                    var ccName = Enum.GetName(typeof(ConsoleColor), cc);

                    tw.WriteLine($"<td>{propertyInfo.Name}</td><td bgcolor=\"{c.ToRgbHex()}\">{c.Name}</td><td bgcolor=\"{cCol.ToRgbHex()}\">{ccName}</td>");
                    tw.WriteLine("</tr>");
                }

                tw.WriteLine("</table></body></html>");
                tw.Close();
            }
        }

        private string GetOutputFolder(params string[] fileParts)
        {
            string colorsVersion = this._colorsVersionAtt?.Version ?? "unknown";
            var dir = $"{TEST_ROOT}{colorsVersion}\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!fileParts.Any())
            {
                return dir;
            }

            var subFolders = fileParts.Take(fileParts.Length - 1).ToArray();
            if (subFolders.Any())
            {
                dir = Path.Combine((new[] {dir}).Concat(subFolders).ToArray());
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = Path.Combine(dir, fileParts.Last());

            return path;
        }
    }
}
