// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2015-2016 Sebastian Gruchacz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// <summary>
//   Helper extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Conditions;

namespace ObscureWare.Console.Shared
{
    public static class Extensions
    {
        /// <summary>
        /// The minimum fitting area for string splitting.
        /// </summary>
        private const uint MIN_FIT_AREA = 3;

        /// <summary>
        /// Prints <see cref="System.Drawing.Color"/> as RGB hex string.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToRgbHex(this System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        /// <summary>
        /// Info-units suffixes
        /// </summary>
        private static readonly string[] Sufixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private static readonly char[] SplitMatches = new[] { ' ', ',', '\'', '"', '.', ';', '!', '?', ':', '-', '=', '_' };

        /// <summary>
        /// Converts natural number (indexing, staring from 1) into Excel-like column numbering format (i.e. A, B, ... Z, AB, AC, ... ZY, ZZ)
        /// </summary>
        /// <param name="value">Number to be converted.</param>
        /// <returns>Excel-like column number</returns>
        /// <remarks>Taken from http://stackoverflow.com/questions/837155/fastest-function-to-generate-excel-column-letters-in-c-sharp </remarks>
        public static string ToAlphaEnum(this uint value)
        {
            value.Requires(nameof(value)).IsGreaterThan(0u);

            string columnString = string.Empty;
            decimal columnNumber = value;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }

            return columnString;
        }

        /// <summary>
        /// Converts Excel-like column numbering format into corresponding natural integer
        /// </summary>
        /// <param name="value">String to be "parsed" (converted)</param>
        /// <returns></returns>
        /// <remarks>Taken from http://stackoverflow.com/questions/837155/fastest-function-to-generate-excel-column-letters-in-c-sharp </remarks>
        public static uint FromAlphaEnum(this string value)
        {
            value.Requires(nameof(value)).IsNotNullOrEmpty();

            int retVal = 0;
            string col = value.ToUpper();
            for (int charIndex = col.Length - 1; charIndex >= 0; charIndex--)
            {
                char colPiece = col[charIndex];
                int colNum = colPiece - 64;
                retVal = retVal + (colNum * (int)Math.Pow(26, col.Length - (charIndex + 1)));
            }
            return (uint)retVal;
        }

        /// <summary>
        /// Converts number of bytes into user friendly format of KB, MB, GB etc.
        /// </summary>
        /// <param name="byteCount">Count of bytes</param>
        /// <param name="culture">Optional culture for number formatting</param>
        /// <returns></returns>
        /// <remarks>Based on http://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net </remarks>
        public static string ToFriendlyXBytesText(this long byteCount, CultureInfo culture = null)
        {
            if (byteCount == 0)
            {
                return $"{0} {Sufixes[0]}";
            }

            long bytes = Math.Abs(byteCount); // just ignore negative lengths - this is not metaphysics
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));

            if (place >= Sufixes.Length)
            {
                throw new ArgumentException(
                    $"Unexpectedly large number, objects larger than 1023{Sufixes[Sufixes.Length - 1]} are not expected to exists in the entire universe!",
                    nameof(byteCount));
            }
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return $"{(Math.Sign(byteCount) * num).ToString(culture ?? CultureInfo.InvariantCulture),1} {Sufixes[place]}";
        }

        /// <summary>
        /// Splits text into lines that fit into designated column width
        /// </summary>
        /// <param name="text">Text to split</param>
        /// <param name="columnWidth">Area width to fit the string</param>
        /// <param name="smartSplitSettings">(Not implemented yet)</param>
        /// <returns>Text splinted into matching pieces.</returns>
        /// <remarks>Based on this imperfect solution for now: http://stackoverflow.com/a/1678162
        /// This will not work properly for long "words" (and nicely for very narrow columns - do not expect miracles).
        /// This is not able to properly break the "words" in the middle to optimize space...
        /// </remarks>
        public static IEnumerable<string> SplitTextToFit(this string text, uint columnWidth, SmartSplitSettings smartSplitSettings = null)
        {
            text.Requires(nameof(text)).IsNotNull();
            columnWidth.Requires(nameof(columnWidth)).IsGreaterOrEqual(MIN_FIT_AREA, $"This is just plain nonsense - area to fit text into must be at least {MIN_FIT_AREA} characters wide.");

            if (text == string.Empty)
            {
                yield return string.Empty;
                yield break;
            }

            int offset = 0;
            while (offset < text.Length)
            {
                if (offset + (int)columnWidth < text.Length)
                {
                    // TODO: do better splitting and cleaning of white spaces / keeping punctuation....
                    int index = text.LastIndexOfAny(SplitMatches, Math.Min(text.Length, offset + (int)columnWidth));
                    if (index > 0 && index >= offset)
                    {
                        if (index < 0 || text[index] == ' ')
                        {
                            string line = text.Substring(offset, (index - offset <= 0 ? text.Length : index) - offset);
                            offset += line.Length + 1;
                            yield return line;
                        }
                        else
                        {
                            // keep punctuation character in upper line
                            int cutLen = (index - offset <= 0 ? text.Length : index) - offset + 1;
                            if (cutLen > columnWidth)
                            {
                                cutLen = (int)columnWidth;
                            }
                            string line = text.Substring(offset, cutLen);
                            offset += line.Length;
                            yield return line;
                        }
                    }
                    else
                    {
                        // split in the middle of long, unbreakable text
                        if (smartSplitSettings != null)
                        {
                            yield return SmartSplit(text, columnWidth, ref offset, smartSplitSettings);
                        }
                        else
                        {
                            yield return BruteSplit(text, columnWidth, ref offset);
                        }
                    }
                }
                else
                {
                    yield return text.Substring(offset);
                    yield break;
                }
            }
        }

        private static string SmartSplit(string text, uint columnWidth, ref int offset, SmartSplitSettings smartSplitSettings)
        {
            // TODO: smart-split, but that would require to know something about column content type (add hyphen in text, just break inside identifiers or numbers...
            // TODO: use Humanizer library perhaps?
            throw new NotImplementedException();
        }

        private static string BruteSplit(string text, uint columnWidth, ref int offset)
        {
            string line = text.Substring(offset, (int)columnWidth);
            offset += line.Length;
            return line;
        }

        /// <summary>
        /// Returns true, if this is system character (ASCII &lt; 32)
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsSystemChar(this char ch)
        {
            return (int)ch < 32;
        }
    }
}