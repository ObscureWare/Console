// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualEntryLineTests.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2017 Sebastian Gruchacz
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
//   Defines tests for VirtualEntryLine class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ObscureWare.Console.Operations.Interfaces;
using ObscureWare.Console.Root.Interfaces;

namespace ObscureWare.Console.Operations.Tests
{
    using System.Drawing;

    using Moq;

    using Shouldly;

    using Xunit;

    public class VirtualEntryLineTests
    {
        [Theory]
        [InlineData(5, 5, 5, 5, 10, 0)]
        [InlineData(5, 5, 6, 5, 10, 1)]
        [InlineData(5, 5, 5, 6, 10, 10)]
        [InlineData(5, 5, 3, 6, 10, 8)]
        [InlineData(5, 5, 8, 6, 10, 13)]
        public void CalculatePositionInLineShouldCalculateCorrectPositions(int xStart, int yStart, int xCurrent, int yCurrent, int lineLength, int expected)
        {
            var testedObj = new VirtualEntryLine(new Mock<IConsole>().Object, new FakeClipBoard(), new ConsoleFontColor(Color.AliceBlue, Color.AntiqueWhite));

            testedObj.CalculatePositionInLine(new Point(xStart, yStart), new Point(xCurrent, yCurrent), lineLength).ShouldBe(expected);
        }

        [Theory]
        [InlineData(5, 5, 10, 0, 5, 5)]
        [InlineData(5, 5, 10, 1, 6, 5)]
        [InlineData(5, 5, 10, 10, 5, 6)]
        [InlineData(5, 5, 10, 8, 3, 6)]
        [InlineData(5, 5, 10, 13, 8, 6)]
        public void CalculateCursorPositionForLineIndexShouldCalculateCorrectPositions(int xStart, int yStart, int lineLength, int targetIndex, int xExpected, int yExpected)
        {
            var testedObj = new VirtualEntryLine(new Mock<IConsole>().Object, new FakeClipBoard(), new ConsoleFontColor(Color.AliceBlue, Color.AntiqueWhite));

            testedObj.CalculateCursorPositionForLineIndex(new Point(xStart, yStart), lineLength, targetIndex).ShouldBe(new Point(xExpected, yExpected));
        }

        [Theory]
        [InlineData("", 0, 0, "")]
        [InlineData("1", 0, 0, "1")]
        [InlineData("1", 0, 1, "")]
        [InlineData("abc", 0, 1, "bc")]
        [InlineData("abc", 1, 1, "ac")]
        [InlineData("abc", 2, 1, "ab")]
        public void RemoveCharsAtShallProduceCorrectStrings(string entry, int index, int removeQty, string expectedResult)
        {
            var testedObj = new VirtualEntryLine(new Mock<IConsole>().Object, new FakeClipBoard(), new ConsoleFontColor(Color.AliceBlue, Color.AntiqueWhite));

            char[] buffer = entry.ToCharArray();
            int len = entry.Length;

            testedObj.RemoveCharsAt(buffer, index, removeQty, ref len);

            len.ShouldBe(expectedResult.Length);

            string result = new string(buffer, 0, len);

            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("", 0, "", "")]
        [InlineData("1", 0, "1", "11")]
        [InlineData("1", 0, "12", "121")]
        [InlineData("bc", 0, "a", "abc")]
        [InlineData("bc", 1, "a", "bac")]
        [InlineData("bc", 2, "a", "bca")]
        public void InsertCharsAtShallProduceCorrectStrings(string entry, int index, string textToInsert, string expectedResult)
        {
            var testedObj = new VirtualEntryLine(new Mock<IConsole>().Object, new FakeClipBoard(), new ConsoleFontColor(Color.AliceBlue, Color.AntiqueWhite));

            char[] buffer = new char[entry.Length + textToInsert.Length];
            entry.ToCharArray().CopyTo(buffer, 0);
            char[] insertBuffer = textToInsert.ToCharArray();
            int len = entry.Length;

            testedObj.InsertCharsAt(buffer, index, insertBuffer, ref len);

            string result = new string(buffer, 0, len);

            result.ShouldBe(expectedResult);
        }
    }
}
