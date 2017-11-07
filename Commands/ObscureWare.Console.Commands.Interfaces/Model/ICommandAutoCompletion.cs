// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoCompletableAttribute.cs" company="Obscureware Solutions">
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
//   Defines the AutoCompletableAttribute command model type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ObscureWare.Console.Commands.Interfaces.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for commands that support auto-completion.
    /// </summary>
    public interface ICommandAutoCompletion
    {
        /// <summary>
        /// Shall return list of auto-completion values for current context, already filled model, targeted auto-completable property and starting text
        /// </summary>
        /// <param name="contextObject">Current state context.</param>
        /// <param name="runtimeModel">Model object built so far.</param>
        /// <param name="targetPropertyName">Property name of model switch being under edition</param>
        /// <param name="matchText">Already entered text for auto-completion of property.</param>
        /// <returns></returns>
        IEnumerable<string> AutoCompleteCommand(object contextObject, CommandModel runtimeModel, string targetPropertyName, string matchText);
    }
}
