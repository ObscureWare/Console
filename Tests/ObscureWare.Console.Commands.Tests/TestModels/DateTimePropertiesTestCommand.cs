namespace ObscureWare.Console.Commands.Tests.TestModels
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;


    [CommandModel(typeof(DateTimePropertiesTestCommandModel))]
    public class DateTimePropertiesTestCommand : IConsoleCommand
    {
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            throw new NotImplementedException();
        }
    }

    [CommandName("ParseDateTime")]
    [CommandDescription(@"float parsing test model")]
    [CommandModelFor(typeof(DateTimePropertiesTestCommand))]
    public class DateTimePropertiesTestCommandModel : CommandModel, IEquatable<DateTimePropertiesTestCommandModel>
    {
        [CommandOptionName(@"d")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("d")]
        [CommandDescription("d")]
        public DateTime D { get; set; }

        [CommandOptionName(@"dt")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("dt")]
        [CommandDescription("dt")]
        public DateTime DT { get; set; }

        [CommandOptionName(@"t")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("t")]
        [CommandDescription("t")]
        public TimeSpan T { get; set; }

        public bool Equals(DateTimePropertiesTestCommandModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return D.Equals(other.D) && DT.Equals(other.DT) && T.Equals(other.T);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DateTimePropertiesTestCommandModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = D.GetHashCode();
                hashCode = (hashCode * 397) ^ DT.GetHashCode();
                hashCode = (hashCode * 397) ^ T.GetHashCode();
                return hashCode;
            }
        }
    }
}
