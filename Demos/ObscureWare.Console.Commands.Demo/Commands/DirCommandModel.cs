namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(DirCommand))]
    [CommandName("dir")]
    [CommandDescription(@"Lists files within current folder or repository state, depending on selected options.")]
    public class DirCommandModel : CommandModel
    {
        [CommandOptionName(@"includeFolders")]
        [Mandatory(false)]
        [CommandOptionFlag("d", "D")]
        [CommandDescription("When set, specifies whether directories shall be listed too.")]
        public bool IncludeFolders { get; set; }

        [CommandOptionName(@"mode")]
        [Mandatory(false)]
        [CommandOptionSwitch(typeof(DirectoryListMode), "m", DefaultValue = DirectoryListMode.CurrentDir)]
        [CommandDescription("Specifies which predefined directory location shall be listed.")]
        // TODO: list help for switches. Get from enumeration itself? Allow coloring syntax? Somehow...
        // TODO: more switch types?
        // TODO: runtime support switch auto-complete. Sourced through ModelBuilder & Parser
        public DirectoryListMode Mode { get; set; }

        [CommandOptionName(@"filter")]
        [Mandatory(false)]
        [CommandOptionSwitchless(0)]
        [CommandDescription("Specifies filter for enumerated files. Does not apply to folders.")]
        // TODO: runtime support for some values / unnamed values auto-completion? sourced through command itself...
        public string Filter { get; set; }

        // TODO: add sorting argument
    }
}