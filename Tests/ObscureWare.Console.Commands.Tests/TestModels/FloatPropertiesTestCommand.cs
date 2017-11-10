namespace ObscureWare.Console.Commands.Tests.TestModels
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModel(typeof(FloatPropertiesTestCommandModel))]
    public class FloatPropertiesTestCommand : IConsoleCommand
    {
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            throw new NotImplementedException();
        }
    }

    [CommandName("ParseFloats")]
    [CommandDescription(@"float parsing test model")]
    [CommandModelFor(typeof(FloatPropertiesTestCommand))]
    public class FloatPropertiesTestCommandModel : CommandModel, IEquatable<FloatPropertiesTestCommandModel>
    {
        [CommandOptionName(@"a")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("a")]
        [CommandDescription("a")]
        public double A { get; set; }

        [CommandOptionName(@"b")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("b")]
        [CommandDescription("a")]
        public float B { get; set; }

        public bool Equals(FloatPropertiesTestCommandModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return A.Equals(other.A) && B.Equals(other.B);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FloatPropertiesTestCommandModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (A.GetHashCode() * 397) ^ B.GetHashCode();
            }
        }
    }
}
