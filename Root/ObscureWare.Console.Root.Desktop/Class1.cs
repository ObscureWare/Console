﻿namespace ObscureWare.Console.Root.Desktop
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml;

    // Copyright (C) Microsoft.  All rights reserved.
    // Licensed under the terms described in the LICENSE file in the root of this project.
    //

    // Based on e-MIT-ed source from https://github.com/Microsoft/console/blob/master/tools/ColorTool/ColorTool/XmlSchemeParser.cs
    // Adapted to be used and reuse ObscureWare's Console library

    public class XmlSchemeParser : ISchemeParser
    {
        // In Windows Color Table order
        private static string[] PLIST_COLOR_NAMES = {
            "Ansi 0 Color",  // DARK_BLACK
            "Ansi 4 Color",  // DARK_BLUE
            "Ansi 2 Color",  // DARK_GREEN
            "Ansi 6 Color",  // DARK_CYAN
            "Ansi 1 Color",  // DARK_RED
            "Ansi 5 Color",  // DARK_MAGENTA
            "Ansi 3 Color",  // DARK_YELLOW
            "Ansi 7 Color",  // DARK_WHITE
            "Ansi 8 Color",  // BRIGHT_BLACK
            "Ansi 12 Color", // BRIGHT_BLUE
            "Ansi 10 Color", // BRIGHT_GREEN
            "Ansi 14 Color", // BRIGHT_CYAN
            "Ansi 9 Color",  // BRIGHT_RED
            "Ansi 13 Color", // BRIGHT_MAGENTA
            "Ansi 11 Color", // BRIGHT_YELLOW
            "Ansi 15 Color" // BRIGHT_WHITE
        };

        static string FG_KEY = "Foreground Color";
        static string BG_KEY = "Background Color";
        static string RED_KEY = "Red Component";
        static string GREEN_KEY = "Green Component";
        static string BLUE_KEY = "Blue Component";

        public string Name => "iTerm Parser";

        private static bool parseRgbFromXml(XmlNode components, ref uint rgb)
        {
            int r = -1;
            int g = -1;
            int b = -1;

            foreach (XmlNode c in components.ChildNodes)
            {
                if (c.Name == "key")
                {
                    if (c.InnerText == RED_KEY)
                    {
                        r = (int)(255 * Convert.ToDouble(c.NextSibling.InnerText, CultureInfo.InvariantCulture));
                    }
                    else if (c.InnerText == GREEN_KEY)
                    {
                        g = (int)(255 * Convert.ToDouble(c.NextSibling.InnerText, CultureInfo.InvariantCulture));
                    }
                    else if (c.InnerText == BLUE_KEY)
                    {
                        b = (int)(255 * Convert.ToDouble(c.NextSibling.InnerText, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (r < 0 || g < 0 || b < 0)
            {
                // InvalidColor; // TODO: complex result? Display?
                return false;
            }

            rgb = NativeMethods.RGB(r, g, b);

            return true;
        }


        static XmlDocument loadXmlScheme(string schemeName)
        {
            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            string exeDir = System.IO.Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;
            bool found = false;
            string filename = schemeName + ".itermcolors";
            string exeSchemes = exeDir + "/schemes/";
            string cwd = "./";
            string cwdSchemes = "./schemes/";
            // Search order, for argument "name", where 'exe' is the dir of the exe.
            //  1. ./name
            //  2. ./name.itermcolors
            //  3. ./schemes/name
            //  4. ./schemes/name.itermcolors
            //  5. exe/schemes/name
            //  6. exe/schemes/name.itermcolors
            //  7. name (as an absolute path)
            string[] paths = {
                cwd + schemeName,
                cwd + filename,
                cwdSchemes + schemeName,
                cwdSchemes + filename,
                exeSchemes + schemeName,
                exeSchemes + filename,
                schemeName,
            };
            foreach (string path in paths)
            {
                try
                {
                    xmlDoc.Load(path);
                    found = true;
                    break;
                }
                catch (Exception /*e*/)
                {
                    // We can either fail to find the file,
                    //   or fail to parse the XML here.
                }
            }

            if (!found)
            {
                return null;
            }
            return xmlDoc;
        }


        public ColorScheme ParseScheme(string schemeName, bool reportErrors = true)
        {
            XmlDocument xmlDoc = loadXmlScheme(schemeName); // Create an XML document object
            if (xmlDoc == null) return null;
            XmlNode root = xmlDoc.GetElementsByTagName("dict")[0];
            XmlNodeList children = root.ChildNodes;

            uint[] colorTable = new uint[NativeMethods.COLOR_TABLE_SIZE];
            uint? fgColor = null, bgColor = null;
            int colorsFound = 0;
            bool success = false;
            foreach (var tableEntry in children.OfType<XmlNode>().Where(_ => _.Name == "key"))
            {
                uint rgb = 0;
                int index = -1;
                XmlNode components = tableEntry.NextSibling;
                success = parseRgbFromXml(components, ref rgb);
                if (!success) { break; }
                else if (tableEntry.InnerText == FG_KEY) { fgColor = rgb; }
                else if (tableEntry.InnerText == BG_KEY) { bgColor = rgb; }
                else if (-1 != (index = Array.IndexOf(PLIST_COLOR_NAMES, tableEntry.InnerText)))
                { colorTable[index] = rgb; colorsFound++; }
            }
            if (colorsFound < NativeMethods.COLOR_TABLE_SIZE)
            {
                if (reportErrors)
                {
                    // InvalidNumberOfColors 
                }
                success = false;
            }
            if (!success)
            {
                return null;
            }

            return new ColorScheme { colorTable = colorTable, foreground = fgColor, background = bgColor };
        }
    }
}
