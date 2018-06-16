namespace ObscureWare.Console.Root.Desktop.Scheme
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    using Shared;

    // Copyright (C) Microsoft.  All rights reserved.
    // Licensed under the terms described in the LICENSE file in the root of this project.
    //

    // Based on e-MIT-ed source from https://github.com/Microsoft/console/blob/master/tools/ColorTool/ColorTool/IniSchemeParser.cs
    // Adapted to be used and reused ObscureWare's Console library

    public class IniSchemeParser : ISchemeParser
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private const string MY_EXTENSION = @".ini";

        // These are in Windows Color table order - BRG, not RGB.
        public static string[] COLOR_NAMES = {
            "DARK_BLACK",
            "DARK_BLUE",
            "DARK_GREEN",
            "DARK_CYAN",
            "DARK_RED",
            "DARK_MAGENTA",
            "DARK_YELLOW",
            "DARK_WHITE",
            "BRIGHT_BLACK",
            "BRIGHT_BLUE",
            "BRIGHT_GREEN",
            "BRIGHT_CYAN",
            "BRIGHT_RED",
            "BRIGHT_MAGENTA",
            "BRIGHT_YELLOW",
            "BRIGHT_WHITE"
        };

        public string Name => "INI File Parser";

        private static uint ParseHex(string arg)
        {
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(arg);
            return NativeMethods.RGB(col.R, col.G, col.B);
        }

        private static uint ParseRgb(string arg)
        {
            int[] components = { 0, 0, 0 };
            string[] args = arg.Split(',');
            if (args.Length != components.Length) throw new Exception("Invalid color format \"" + arg + "\"");
            if (args.Length != 3) throw new Exception("Invalid color format \"" + arg + "\"");
            for (int i = 0; i < args.Length; i++)
            {
                components[i] = Int32.Parse(args[i]);
            }

            return NativeMethods.RGB(components[0], components[1], components[2]);
        }

        private static uint ParseColor(string arg)
        {
            if (arg[0] == '#')
            {
                return ParseHex(arg.Substring(1));
            }
            else
            {
                return ParseRgb(arg);
            }
        }

        public ColorScheme ParseScheme(string filename, bool throwExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filename));
            }
            bool success = true;

            string[] tableStrings = new string[NativeMethods.COLOR_TABLE_SIZE];
            uint[] colorTable = null;

            for (int i = 0; i < NativeMethods.COLOR_TABLE_SIZE; i++)
            {
                string name = COLOR_NAMES[i];
                StringBuilder buffer = new StringBuilder(512);
                GetPrivateProfileString("table", name, null, buffer, 512, filename);

                tableStrings[i] = buffer.ToString();
                if (tableStrings[i].Length <= 0)
                {
                    success = false;
                    if (throwExceptions)
                    {
                        throw new InvalidOperationException($"Failed to parse Scheme file {filename}: {name} - {tableStrings[i]}");
                    }

                    break;
                }
            }

            if (success)
            {
                try
                {
                    colorTable = new uint[NativeMethods.COLOR_TABLE_SIZE];
                    for (int i = 0; i < NativeMethods.COLOR_TABLE_SIZE; i++)
                    {
                        colorTable[i] = ParseColor(tableStrings[i]);
                    }
                }
                catch (Exception e)
                {
                    if (throwExceptions)
                    {
                        throw new InvalidOperationException($"Failed to parse Scheme file - {filename}", e);
                    }

                    colorTable = null;
                }
            }

            if (colorTable != null)
            {
                return new ColorScheme(Path.GetFileNameWithoutExtension(filename), colorTable);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc />
        public bool CanProcess(string extension)
        {
            return MY_EXTENSION.Equals(extension, StringComparison.InvariantCultureIgnoreCase);
        }

        // <inheritdoc />
        public IEnumerable<string> GetSupportedExtensions()
        {
            yield return MY_EXTENSION;
        }
    }

}
