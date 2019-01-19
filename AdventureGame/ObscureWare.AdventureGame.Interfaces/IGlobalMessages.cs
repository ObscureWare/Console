namespace ObscureWare.AdventureGame.Interfaces
{
    using Enums;

    /// <summary>
    /// Provides global, shared messages to be reused by other components. Can also return one, random version of particular message. Can also use game state for templates.
    /// </summary>
    public interface IGlobalMessages
    {
        /// <summary>
        /// Returns build-in standard messages
        /// </summary>
        /// <param name="messageCode"></param>
        /// <returns></returns>
        ITextBlock GetStandardMessage(StandardMessages messageCode);

        /// <summary>
        /// Returns standard messages installed by mods, extensions or chapters
        /// </summary>
        /// <param name="extendedMessageCode"></param>
        /// <returns></returns>
        ITextBlock GetStandardMessage(string extendedMessageCode);
    }
}