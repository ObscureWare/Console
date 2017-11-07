namespace ObscureWare.Console.Operations.Interfaces.Styles
{
    /// <summary>
    /// Specifies how large content of table rows will be treated
    /// </summary>
    public enum TableOverflowContentBehavior
    {
        /// <summary>
        /// Cut to fit with ellipsis
        /// </summary>
        Ellipsis,

        /// <summary>
        /// Multi-lined
        /// </summary>
        Wrap
    }
}