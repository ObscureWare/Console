using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.AdventureGame.Commands
{
    [CommandKeywords("nalej", "nasyp", "dodaj", "nabierz")]
    [CommandWhiteKeywords("do", "z")]
    [CommandSyntax("[action:action] (number:amount) [substance:substance] (z) [container|containers:source] (do) ([number:count]) [container|containers:target] (z) (ekwipunku|wyposażenia)")]
    [CommandParametersDescriptorType(typeof(PourSubstanceIntoContainerParameterDescriptor))]
    class PourSubstanceIntoContainerCommand : ICommand<PourSubstanceIntoContainerParameterDescriptor>
    {

    }

    internal class CommandSyntaxAttribute : Attribute
    {
        public CommandSyntaxAttribute(params string[] syntaxes)
        {
            throw new NotImplementedException();
        }
    }

    internal interface ICommand<T> where T : ICommandParametersDescriptor
    {
    }

    internal class CommandParametersDescriptorTypeAttribute : Attribute
    {
        public CommandParametersDescriptorTypeAttribute(Type type)
        {
            throw new NotImplementedException();
        }
    }

    internal class CommandWhiteKeywordsAttribute : Attribute
    {
        public CommandWhiteKeywordsAttribute(params string[] keywords)
        {
            throw new NotImplementedException();
        }
    }

    internal class PourSubstanceIntoContainerParameterDescriptor : ICommandParametersDescriptor
    {
        /// <inheritdoc />
        public int ExpectedParametersCount { get; }
    }

    internal interface ICommandParametersDescriptor
    {
        int ExpectedParametersCount { get; }
    }

    internal class CommandKeywordsAttribute : Attribute
    {
        public CommandKeywordsAttribute(params string[] keywords)
        {
            throw new NotImplementedException();
        }
    }


    [CommandKeywords("podnieś", "weź")]
    [CommandSyntax("[action:action] (number:amount) [thing:thing] ((z/ze) [container])")]
    [CommandParametersDescriptorType(typeof())]
    class PickupItemCommand : ICommand<>
    {

    }
}
