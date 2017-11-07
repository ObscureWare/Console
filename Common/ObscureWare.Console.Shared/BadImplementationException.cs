// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BadImplementationException.cs" company="Obscureware Solutions">
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
//   Defines BadImplementationException class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Shared
{
    using System;

    /// <summary>
    /// Used to signal to the developer that his implementation of interfaces or inherited types is invalid - and cannot be limited to InvalidOperation or Argument exceptions.
    /// It might signal that maybe some relation (mostly conflict) between several types / values would cause nondeterministic or invalid behavior.
    /// Such errors should be detected during testing or debug sessions though - of course unless depend on user accessible configuration or extensions...
    /// </summary>
    [Serializable]
    public class BadImplementationException : Exception
    {
        /// <summary>
        /// The type that has been improperly implemented or configured.
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="BadImplementationException"/> class.
        /// </summary>
        /// <param name="message">Error message describing conflict</param>
        /// <param name="targetType">Type that is causing problems</param>
        public BadImplementationException(string message, Type targetType) : base(message)
        {
            this.TargetType = targetType;
        }

        // OTHER CONSTRUCTORS NOT ADDED BY DESIGN - this exception is not really structured.
    }
}