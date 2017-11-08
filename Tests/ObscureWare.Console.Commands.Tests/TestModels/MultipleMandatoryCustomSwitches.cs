namespace ObscureWare.Console.Commands.Tests.TestModels
{
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(MultipleMandatoryCustomSwitchesFakeCmd))]
    [CommandName("CreateOrder")]
    [CommandDescription(@"Creates new Order. Optionally also creates Lead.")]
    internal class MultipleMandatoryCustomSwitches : CommandModel
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
