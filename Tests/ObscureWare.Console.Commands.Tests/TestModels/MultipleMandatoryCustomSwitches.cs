namespace ObscureWare.Console.Commands.Tests.TestModels
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(MultipleMandatoryCustomSwitchesFakeCmd))]
    [CommandName("CreateOrder")]
    [CommandDescription(@"Creates new Order. Optionally also creates Lead.")]
    internal class MultipleMandatoryCustomSwitches : CommandModel, IEquatable<MultipleMandatoryCustomSwitches>
    {
        [CommandOptionName(@"supplierId")]
        [Mandatory(true)]
        [CommandOptionCustomValueSwitch("sid")]
        [CommandDescription("Specifies ID of the Supplier.")]
        public int SupplierId { get; set; }

        [CommandOptionName(@"memberId")]
        [Mandatory(true)]
        [CommandOptionCustomValueSwitch("mid")]
        [CommandDescription("Specifies ID of the Member.")]
        public int MemberId { get; set; }

        [CommandOptionName(@"departmentId")]
        [Mandatory(true)]
        [CommandOptionCustomValueSwitch("did")]
        [CommandDescription("Specifies ID of the User's Department.")]
        public int DepartmentId { get; set; }

        [CommandOptionName(@"userId")]
        [Mandatory(true)]
        [CommandOptionCustomValueSwitch("uid")]
        [CommandDescription("Specifies ID of the User.")]
        public string UserId { get; set; }

        [CommandOptionName(@"channelId")]
        [Mandatory(true)]
        [CommandOptionCustomValueSwitch("cid")]
        [CommandDescription("Specifies ID of the Channel used.")]
        public int ChannelId { get; set; }


        public bool Equals(MultipleMandatoryCustomSwitches other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SupplierId == other.SupplierId && MemberId == other.MemberId && DepartmentId == other.DepartmentId && string.Equals(UserId, other.UserId) && ChannelId == other.ChannelId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MultipleMandatoryCustomSwitches)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SupplierId;
                hashCode = (hashCode * 397) ^ MemberId;
                hashCode = (hashCode * 397) ^ DepartmentId;
                hashCode = (hashCode * 397) ^ (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ChannelId;
                return hashCode;
            }
        }
    }

    [CommandModel(typeof(MultipleMandatoryCustomSwitches))]
    internal class MultipleMandatoryCustomSwitchesFakeCmd : IConsoleCommand
    {
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
