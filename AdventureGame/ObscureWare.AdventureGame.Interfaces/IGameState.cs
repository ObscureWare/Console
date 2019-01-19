namespace ObscureWare.AdventureGame.Interfaces
{
    public interface IGameState
    {
        /// <summary>
        /// Global, shared messages to be reused by other components. 
        /// </summary>
        IGlobalMessages GlobalMessages { get; }

        /// <summary>
        /// Game player reference
        /// </summary>
        IPlayer Player { get; }
    }
}