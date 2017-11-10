namespace ObscureWare.Console.Commands.Tests.TestModels
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModel(typeof(ParseOtherTypesCommandModel))]
    public class ParseOtherTypesCommand : IConsoleCommand
    {
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            throw new NotImplementedException();
        }
    }

    [CommandName("ParseOtherTypes")]
    [CommandDescription(@"other types parsing test model")]
    [CommandModelFor(typeof(ParseOtherTypesCommand))]
    public class ParseOtherTypesCommandModel : CommandModel, IEquatable<ParseOtherTypesCommandModel>
    {
        [CommandOptionName(@"g")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("g")]
        [CommandDescription("g")]
        public Guid G { get; set; }

        public bool Equals(ParseOtherTypesCommandModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return G.Equals(other.G);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParseOtherTypesCommandModel) obj);
        }

        public override int GetHashCode()
        {
            return G.GetHashCode();
        }
    }
}
