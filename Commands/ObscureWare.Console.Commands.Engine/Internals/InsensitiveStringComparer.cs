﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsensitiveStringComparer.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2016 Sebastian Gruchacz
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
//   Defines InsensitiveStringComparer class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;

    /// <summary>
    /// Case-insensitive, invariant comparer
    /// </summary>
    internal class InsensitiveStringComparer : StringComparer
    {
        /// <inheritdoc />
        public override int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public override int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }
}