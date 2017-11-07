// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandManager.cs" company="Obscureware Solutions">
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
//   Defines the CommandManager internal class responsible for holding collection of known commands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ObscureWare.Console.Commands.Interfaces;


    internal class CommandManager
    {
        private readonly Dictionary<string, CommandInfo> _commands;
        private StringComparison _selectedComparison = StringComparison.InvariantCulture;

        private CommandCaseSensitivenes _commandsSensitivenes;

        /// <summary>
        /// Gets or sets whether command names are case sensitive or not.
        /// </summary>
        public CommandCaseSensitivenes CommandsSensitivenes
        {
            get
            {
                return this._commandsSensitivenes;
            }
            set
            {
                this._commandsSensitivenes = value;
                this._selectedComparison = (value == CommandCaseSensitivenes.Sensitive) ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
            }
        }

        public CommandManager(Type[] commands)
        {
            this._commands = this.InitializeCommands(commands);
        }

        /// <summary>
        /// Finds command of given name (exact or insensitive match of full name)
        /// </summary>
        /// <param name="cmdName"></param>
        /// <returns>Matching command or NULL</returns>
        public CommandInfo FindCommand(string cmdName)
        {
            return this._commands
                .SingleOrDefault(pair => pair.Key.Equals(cmdName, this._selectedComparison))
                .Value;
        }

        public IEnumerable<CommandInfo> MatchCommandsForAutoComplete(string text)
        {
            return this._commands
                .Where(pair => pair.Key.StartsWith(text, this._selectedComparison))
                .Select(pair => pair.Value);
        }

        /// <summary>
        /// Returns all commands. Sorted.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CommandInfo> GetAll()
        {
            return this._commands.Values.OrderBy(cmd => cmd.CommandModelBuilder.CommandName).ToArray();
        }

        /// <summary>
        /// Compiles and verifies commands - checks implementation correctness during syntax check
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        private Dictionary<string, CommandInfo> InitializeCommands(Type[] commands)
        {
            Dictionary<string, CommandInfo> result = new Dictionary<string, CommandInfo>();

            ConsoleCommandBuilder builder = new ConsoleCommandBuilder();
            foreach (var commandType in commands)
            {
                Tuple<CommandModelBuilder, IConsoleCommand> cmd = builder.ValidateAndBuildCommand(commandType);
                if (cmd != null)
                {
                    result.Add(cmd.Item1.CommandName, new CommandInfo(cmd.Item2, cmd.Item1));
                }
            }

            return result;
        }
    }
}