namespace ObscureWare.AdventureGame.Interfaces.Enums
{
    /// <summary>
    /// Specifies possible levels of describing a thing
    /// </summary>
    public enum DescriptionLevel
    {
        /// <summary>
        /// Default description, only mentioning the thing
        /// </summary>
        Default,

        /// <summary>
        /// Description providing some details about thing, if there any.
        /// </summary>
        Brief,

        /// <summary>
        /// Reveals any interesting details or information about thing if there any and Actor is capable of seeing them.
        /// </summary>
        Inspection
    }
}