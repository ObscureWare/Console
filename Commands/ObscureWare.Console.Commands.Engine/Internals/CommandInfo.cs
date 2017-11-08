// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandInfo.cs" company="Obscureware Solutions">
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
//   Defines the CommandInfo record.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Engine.Internals
{
    using ObscureWare.Console.Commands.Interfaces;


    /// <summary>
    /// Precompiled command information record
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        /// Gets instance of the command
        /// </summary>
        public IConsoleCommand Command { get; private set; }

        /// <summary>
        /// Gets command model's builder instance
        /// </summary>
        public CommandModelBuilder CommandModelBuilder { get; private set; }

        /// <summary>
        /// Initializes new instance of <see cref="CommandInfo"/>
        /// </summary>
        /// <param name="commandInstance">Instance of the command itself</param>
        /// <param name="commandModelBuilder">Instance of model builder</param>
        public CommandInfo(IConsoleCommand commandInstance, CommandModelBuilder commandModelBuilder)
        {
            this.Command = commandInstance;
            this.CommandModelBuilder = commandModelBuilder;
        }
    }
}