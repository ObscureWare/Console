// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTools.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2016-2017 Sebastian Gruchacz
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
//   Defines the TestTools class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Tests.Common
{
    using System;

    using Conditions;

    public static class TestTools
    {
        private static readonly Random Rnd = new Random();

        public static float GetRandomFloat(FloatMode floatMode = FloatMode.Default)
        {
            switch (floatMode)
            {
                case FloatMode.Default:
                    return (float)Rnd.NextDouble();
                case FloatMode.PositiveReal:
                    return (float)Rnd.NextDouble() * float.MaxValue;
                case FloatMode.NegativeReal:
                    return (float)Rnd.NextDouble() * float.MinValue;
                case FloatMode.Real:
                    return (float.MaxValue / 2) - (float)Rnd.NextDouble() * float.MaxValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floatMode), floatMode, null);
            }
        }

        public static float GetRandomFloat(float multiplier, FloatMode floatMode = FloatMode.Default)
        {
            switch (floatMode)
            {
                case FloatMode.Default:
                    return (float)Rnd.NextDouble() * multiplier;
                case FloatMode.PositiveReal:
                    return (float)Rnd.NextDouble() * Math.Abs(multiplier);
                case FloatMode.NegativeReal:
                    return (float)Rnd.NextDouble() * (-1) * Math.Abs(multiplier);
                case FloatMode.Real:
                    return (Math.Abs(multiplier) / 2) - (float)Rnd.NextDouble() * Math.Abs(multiplier);
                default:
                    throw new ArgumentOutOfRangeException(nameof(floatMode), floatMode, null);
            }
        }

        /// <summary>
        /// Builds string of required length concatenating random characters from given string.
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BuildRandomStringFrom(this string sourceString, uint length)
        {
            sourceString.Requires(nameof(sourceString)).IsNotNullOrEmpty();

            char[] array = new char[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = sourceString[Rnd.Next(0, sourceString.Length)];
            }

            return new string(array);
        }

        public static string BuildRandomStringFrom(this string sourceString, int minLength, int maxLength)
        {
            sourceString.Requires(nameof(sourceString)).IsNotNullOrEmpty();
            maxLength.Requires(nameof(maxLength)).IsGreaterThan(minLength);

            int length = Rnd.Next(minLength, maxLength + 1);

            char[] array = new char[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = sourceString[Rnd.Next(0, sourceString.Length)];
            }

            return new string(array);
        }

        public static string Numeric => @"0123456789";

        public static string UpperAlpha => @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string LowerAlpha => @"abcdefghijklmnopqrstuvwxyz";

        public static string UpperAlphanumeric => UpperAlpha + Numeric;

        public static string LowerAlphanumeric => LowerAlpha + Numeric;

        public static string MixedAlphanumeric => UpperAlphanumeric + LowerAlphanumeric;

        public static string AlphanumericIdentifier => UpperAlphanumeric + LowerAlphanumeric + @"_____"; // increased probability of separation ;-)

        public static string AlphaSentence => LowerAlpha + @" .:! ,? ;"; // increased probability of space ;-)
    }
}
