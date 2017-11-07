// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertersManager.cs" company="Obscureware Solutions">
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
//   Defines ConvertersManager class that manages available data converters - used to convert string into real model values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Converters;

    using Shared;

    /// <summary>
    /// Manages available data converters - used to convert string into real model values.
    /// </summary>
    internal class ConvertersManager
    {
        private readonly Dictionary<Type, ArgumentConverter> _knownConverters = new Dictionary<Type, ArgumentConverter>();

        public ConvertersManager() // TODO: add more Assemblies to scan for custom converters
        {
            Assembly asm = this.GetType().Assembly;
            this.LoadConvertersFromAssembly(asm);
        }

        private void LoadConvertersFromAssembly(Assembly asm)
        {
            // TODO: what to do with conflicts? Now exception... Replace? Then what hierarchy... Let Attributes decide? CustomArgumentConverterAttribute? Sounds fine. Not have to scan them.

            var converterTypes = asm.GetTypes().Where(t => typeof(ArgumentConverter).IsAssignableFrom(t));
            foreach (var converterType in converterTypes)
            {
                if (converterType.IsAbstract)
                {
                    continue; // Do not need these
                }

                var att = converterType.GetCustomAttribute<ArgumentConverterTargetTypeAttribute>();
                if (att == null)
                {
                    throw new BadImplementationException($"[{converterType.FullName}] misses required {nameof(ArgumentConverterTargetTypeAttribute)}.", converterType);
                }

                var publicCtor = converterType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
                    new Type[0], null);
                if (publicCtor == null)
                {
                    throw new BadImplementationException($"[{converterType.FullName}] does not expose public, parameterless constructor.", converterType);
                }

                var converterInstance = (ArgumentConverter)Activator.CreateInstance(converterType);
                this._knownConverters.Add(att.TargetType, converterInstance);
            }
        }

        /// <summary>
        /// Tries to find Argument Converter dedicated to converting string into given type.
        /// </summary>
        /// <param name="conversionTarget"></param>
        /// <returns></returns>
        public ArgumentConverter GetConverterFor(Type conversionTarget)
        {
            ArgumentConverter converter;
            if (this._knownConverters.TryGetValue(conversionTarget, out converter))
            {
                return converter;
            }

            return null;
        }
    }
}
