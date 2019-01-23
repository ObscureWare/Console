namespace ObscureWare.AdventureGame.Interfaces
{
    using Enums;

    /// <summary>
    /// Implemented by things that player can "look at", "inspect", etc
    /// </summary>
    public interface IDescribable
    {
        ITextBlock GetDescription(DescriptionLevel level, IGameState gameState, IActor actor);

        string DescriptionCode { get; }
    }


    // Specifies details for generation of possible names in different grammatical forms
}
