namespace ObscureWare.Console.Commands.Demo.Commands
{
    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    /// <summary>
    /// The change dir command model.
    /// </summary>
    [CommandModelFor(typeof(ChangeDirCommand))]
    [CommandName("cd")]
    [CommandDescription(@"Moves Current Directory specific way.")]
    public class ChangeDirCommandModel : CommandModel
    {
        [CommandOptionName(@"target")]
        [Mandatory(false)]
        [CommandOptionSwitchless(0)]
        [AutoCompletable(@"Returns sub-directories of current directory.")]
        [CommandDescription("Specifies how directory shall be changed. Anything except below special values means subdirectory or exact location - if has rooted format...")]
        [SpecialValueDescription("nothing or '.'", "Current folder will not change")]
        [SpecialValueDescription("'..'", "Current folder will go one level up")]
        [SpecialValueDescription("'\\'", "Current folder will immediately jump to the root")]
        public string Target { get; set; }
    }
}