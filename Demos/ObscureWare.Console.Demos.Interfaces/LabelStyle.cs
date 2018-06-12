namespace ObscureWare.Console.Demo.Shared
{
    using System.Drawing;

    using Root.Shared;

    public struct LabelStyle
    {
        public LabelStyle(Color fore, Color bg, TextAlign alignment)
        {
            this.Colors = new ConsoleFontColor(fore, bg);
            this.Alignment = alignment;
        }

        public ConsoleFontColor Colors { get; }

        public TextAlign Alignment { get; }
    }
}