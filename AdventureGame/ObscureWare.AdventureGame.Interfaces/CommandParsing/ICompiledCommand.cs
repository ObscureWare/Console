namespace ObscureWare.AdventureGame.Interfaces.CommandParsing
{
    using Enums;

    public interface ICompiledCommand
    {
        string[] CommandKeyWords { get; }

        InteractionType InteractionType { get; set; }
    }
}