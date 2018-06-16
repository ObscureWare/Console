namespace ObscureWare.Console.Root.Shared
{
    /// <summary>
    /// Specifies which screen will console window venture in Full_Screen mode. Only.
    /// </summary>
    public enum ScreenSelector
    {
        /// <summary>
        /// Default. Also used in non-full-screen mode.
        /// </summary>
        DoNotTouch = 0,

        Primary = 1,
        Secondary = 2,
        Tertiary = 3,

        // Need more? seriously?
    }
}
