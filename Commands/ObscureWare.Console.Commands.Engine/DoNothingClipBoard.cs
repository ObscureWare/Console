namespace ObscureWare.Console.Commands.Engine
{
    using ObscureWare.Console.Operations.Interfaces;

    /// <summary>
    /// Default Clipboard provider. Used if no custom one is provider and in scenarios without clipboard possibility (like single command execution only)
    /// </summary>
    internal class DoNothingClipBoard : IClipboard
    {
        public string GetText()
        {
            return null;
        }
    }
}