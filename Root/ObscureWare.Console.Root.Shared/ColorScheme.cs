namespace ObscureWare.Console.Root.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    // Copyright (C) Microsoft.  All rights reserved.
    // Licensed under the terms described in the LICENSE file in the root of this project.
    //

    // Based on e-MIT-ed source from https://github.com/Microsoft/console/blob/master/tools/ColorTool/ColorTool/ColorScheme.cs
    // Adapted to be used and reused ObscureWare's Console library

        // Stripped many code, changed interface, added some encapsulation. Removed all calculations into heuristics

    /// <summary>The idea is that schema is immutable. COlor balancer is caching heuristic results for future usages, therefore it must not change.</summary>  
    public class ColorScheme
    {
        private readonly uint[] _colorTable = null;
        private uint? _foreground = null;
        private uint? _background = null;


        public string Name { get; }

        public uint? DefaultForeground
        {
            get => this._foreground;
            set => this._foreground = value;
        }

        public uint? DefaultBacground
        {
            get => this._background;
            set => this._background = value;
        }

        public uint this[int colorIndex] => this._colorTable[colorIndex];

        public uint this[ConsoleColor colorIndex] => this._colorTable[(int)colorIndex];

        public ColorScheme(string name, IEnumerable<uint> colors)
        {
            this.Name = name;
            this._colorTable = colors.ToArray();

            if (this._colorTable.Length != 16)
            {
                throw new ArgumentOutOfRangeException(nameof(colors), @"Must provide all 16 console colors.");
            }
        }
        
        /// <summary>
        /// Build Color scheme from older definition
        /// </summary>
        /// <param name="schemeName"></param>
        /// <param name="keyValuePair"></param>
        /// <returns></returns>
        public static ColorScheme FromDefinition(string schemeName, KeyValuePair<ConsoleColor, Color>[] keyValuePair)
        {
            var scheme = new ColorScheme(schemeName, keyValuePair.OrderBy(p => (int)p.Key).Select(p => (uint)p.Value.ToArgb()));
            return scheme;
        }

        /// <summary>
        /// Read only copy of color table
        /// </summary>
        /// <returns></returns>
        public uint[] GetAll()
        {
            return (uint[])this._colorTable.Clone();
        }
    }
}