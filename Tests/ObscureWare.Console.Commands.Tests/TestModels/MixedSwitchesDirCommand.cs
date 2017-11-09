namespace ObscureWare.Console.Commands.Tests.TestModels
{
    using System;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModelFor(typeof(MixedSwitchesDirCommand))]
    [CommandName("dir")]
    [CommandDescription(@"Lists files within current folder or repository state, depending on selected options.")]
    public class MixedSwitchesDirCommandModel : CommandModel, IEquatable<MixedSwitchesDirCommandModel>
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
        public DirectoryListMode Mode { get; set; }

        [CommandOptionName(@"filter")]
        [Mandatory(false)]
        [CommandOptionSwitchless(0)]
        [CommandDescription("Specifies filter for enumerated files. Does not apply to folders.")]
        public string Filter { get; set; }

        public bool Equals(MixedSwitchesDirCommandModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IncludeFolders == other.IncludeFolders && Mode == other.Mode && string.Equals(Filter, other.Filter);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MixedSwitchesDirCommandModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IncludeFolders.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Mode;
                hashCode = (hashCode * 397) ^ (Filter != null ? Filter.GetHashCode() : 0);
                return hashCode;
            }
        }
    }


    [CommandModel(typeof(MixedSwitchesDirCommandModel))]
    class MixedSwitchesDirCommand : IConsoleCommand
    {
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            throw new NotImplementedException();
        }
    }

    public enum DirectoryListMode
    {
        CurrentDir,

        CurrentLocalState,

        CurrentRemoteHead
    }

}
