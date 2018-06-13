namespace ObscureWare.Console.Demo.Shared
{
    using System.Drawing;

    using Root.Shared;

    public struct LabelStyle
    {
        public LabelStyle(Color fore, Color bg, TextAlign alignment = TextAlign.Left, bool useEllipsis = true)
        {
            this.Colors = new ConsoleFontColor(fore, bg);
            this.Alignment = alignment;
            this.AddEllipsisOnOverflow = useEllipsis;
        }

        public ConsoleFontColor Colors { get; }

        public TextAlign Alignment { get; }

        public bool AddEllipsisOnOverflow { get; }
    }
}