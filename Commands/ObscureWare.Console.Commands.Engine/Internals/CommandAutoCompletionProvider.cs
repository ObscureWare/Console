// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandAutoCompletionProvider.cs" company="Obscureware Solutions">
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
//   Defines the CommandAutoCompletionProvider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System.Collections.Generic;
    using System.Linq;

    using ObscureWare.Console.Operations.Interfaces;

    /// <summary>
    /// This class provides two-step auto-completion information for command engine
    /// </summary>
    internal class CommandAutoCompletionProvider : IAutoComplete
    {
        private readonly CommandManager _manager;

        public CommandAutoCompletionProvider(CommandManager manager)
        {
            this._manager = manager;
        }

        /// <inheritdoc cref="IAutoComplete"/>
        public IEnumerable<string> MatchAutoComplete(string text)
        {
            // now it only suggests commands, later, when found only one command and having something more 
            // (at least one space after command) it shall try to provide auto-completion from that command, using perhaps model?

            var matchingCommands = this._manager.MatchCommandsForAutoComplete(text);

            // return commands
            return matchingCommands.Select(cmdInfo => cmdInfo.CommandModelBuilder.CommandName);

            // TODO: return command + command part... very low priority
        }
    }
}
