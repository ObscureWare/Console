using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.AdventureGame.Parsing
{
    using Interfaces.CommandParsing;
    using Interfaces.Enums;

    /// <summary>
    /// This is compiled command descriptor that should speed-up process of parsing and matching entered text with commands
    /// </summary>
    public class CompiledCommandDescription : ICompiledCommand
    {
        public string[] CommandKeyWords { get; }

        public ICompiledCommandSyntax[] PossibleSyntaxes { get; }

        public InteractionType InteractionType { get; set; }

        /// <summary>
        /// Returns set of syntax-entries that match specific arguments - keyword and number, without yet analyzing their correctness or possibility
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ICompiledCommandSyntax[] Matches(params string[] args)
        {
            return this.PossibleSyntaxes.Where(s => s.Matches(args)).ToArray();
        }
    }

    public class CommandSyntax : ICompiledCommandSyntax
    {
        private readonly ICompiledCommand _parentCommand;

        public CommandSyntax(ICompiledCommand parentCommand)
        {
            this._parentCommand = parentCommand;
        }

        public ExecutionObject TryBuildExecution(IGameWorld world, params string[] args)
        {
            // this assumes that Matches() was already executed and verified and returned true... Do not waste time on second check...

            throw new NotImplementedException();
        }

        public int KeywordIndex { get; set; }

        public bool Matches(params string[] args)
        {
            if (this.KeywordIndex < 0 || args.Length < this.KeywordIndex)
            {
                return false; // sanity check
            }

            // find if keyword is fine
            var keyWord = args[this.KeywordIndex];
            if (!this._parentCommand.CommandKeyWords.Any(k => k.Equals(keyWord, StringComparison.CurrentCultureIgnoreCase))) // TODO: remember to set-up Polish culture...
            {
                return false;
            }

            // find if arguments are fine - remove white words and then verify arg-count

            // TODO:

            return true;
        }
    }
}
