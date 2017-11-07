// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandDescriptionAttribute.cs" company="Obscureware Solutions">
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
//   Defines the CommandDescriptionAttribute command model type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Interfaces.Model
{
    using System;

    /// <summary>Specifies a description for a command model or any of fragment of that model.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class CommandDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDescriptionAttribute"/> class with a description.
        /// </summary>
        /// <param name="description">The description text.</param>
        public CommandDescriptionAttribute(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(description));
            }

            this.Description = description;
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <returns>The description stored in this attribute.</returns>
        public string Description { get; }

        /// <summary>
        /// Returns whether the value of the given object is equal to the current <see cref="CommandDescriptionAttribute"/>
        /// </summary>
        /// <param name="obj">he object to test the value equality of.</param>
        /// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (Object.Equals(this, obj))
            {
                return true;
            }

            CommandDescriptionAttribute cast = obj as CommandDescriptionAttribute;
            if (cast == null || !cast.Description.Equals(this.Description))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Description?.GetHashCode() ?? 0;
        }
    }
}