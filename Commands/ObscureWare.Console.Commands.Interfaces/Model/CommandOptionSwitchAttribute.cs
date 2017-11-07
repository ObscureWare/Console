// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandOptionSwitchAttribute.cs" company="Obscureware Solutions">
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
//   Defines the CommandOptionSwitchAttribute command model type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Interfaces.Model
{
    using System;

    public class CommandOptionSwitchAttribute : Attribute
    {
        public Type SwitchBaseType { get; private set; }

        public string[] CommandLiterals { get; private set; }

        public object DefaultValue { get; set; }

        public CommandOptionSwitchAttribute(Type switchBaseType, params string[] commandLiterals)
        {
            if (switchBaseType == null) throw new ArgumentNullException(nameof(switchBaseType));
            if (commandLiterals == null) throw new ArgumentNullException(nameof(commandLiterals));
            if (!switchBaseType.IsEnum) throw new ArgumentException("Value must be an Enumeration type.", nameof(switchBaseType));
            if (commandLiterals.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(commandLiterals));

            this.SwitchBaseType = switchBaseType;
            this.CommandLiterals = commandLiterals;
        }
    }
}