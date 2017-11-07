// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandEngine.cs" company="Obscureware Solutions">
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
//   Defines the ICommandEngine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Interfaces
{
    /// <summary>
    /// The CommandEngine interface.
    /// </summary>
    public interface ICommandEngine
    {
        /// <summary>Executes single command line - already parsed into pieces.</summary>
        /// <param name="context">Shared context object, that will be passed to the commands</param>
        /// <param name="commandLine">Command line to be executed. Please, without executable name</param>
        /// <returns>TRUE if command has been successfully executed.</returns>
        bool ExecuteCommand(ICommandEngineContext context, string commandLine);

        /// <summary>
        /// Executes single command line - already parsed into pieces. Just pass "args" argument passed to the Main() function
        /// </summary>
        /// <param name="context">Shared context object, that will be passed to the commands</param>
        /// <param name="commandLineArguments">Arguments passed from the command line</param>
        /// <returns></returns>
        bool ExecuteCommand(ICommandEngineContext context, string[] commandLineArguments);

        /// <summary>
        /// Starts user-interactive session
        /// </summary>
        /// <param name="context">Shared context object, that will be passed to the commands</param>
        void Run(ICommandEngineContext context);
    }
}