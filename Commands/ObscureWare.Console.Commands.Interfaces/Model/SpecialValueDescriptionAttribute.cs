// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialValueDescriptionAttribute.cs" company="Obscureware Solutions">
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
//   Defines the SpecialValueDescriptionAttribute command model type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Interfaces.Model
{
    using System;

    /// <summary>
    /// Decorate command model's property with this attribute to provide special values descriptions in help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SpecialValueDescriptionAttribute : Attribute
    {
        public string Value { get; }
        public string Description { get; }

        public SpecialValueDescriptionAttribute(string specialValue, string description)
        {
            if (string.IsNullOrWhiteSpace(specialValue))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(specialValue));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(description));
            }

            this.Value = specialValue;
            this.Description = description;
        }
    }
}