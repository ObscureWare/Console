namespace ObscureWare.Console.Demo.Shared
{
    using System;
    using System.Drawing;

    using Root.Shared;

    public class StatusStyles
    {
        public ConsoleFontColor TitleStyle { get; set; } = new ConsoleFontColor(Color.WhiteSmoke, Color.Black);

        public ConsoleFontColor SeparatorStyle { get; set; } = new ConsoleFontColor(Color.Yellow, Color.Black);

        public ConsoleFontColor ErrorStyle { get; set; } = new ConsoleFontColor(Color.DarkRed, Color.Black);

        public ConsoleFontColor FineStyle { get; set; } = new ConsoleFontColor(Color.Green, Color.Black);

        public ConsoleFontColor InfoStyle { get; set; } = new ConsoleFontColor(Color.DodgerBlue, Color.Black);

        public string SeparatorText { get; set; } = " => ";

        public StatusStyle SelectFlagStyle(bool flag)
        {
            return (flag) ? StatusStyle.Ok : StatusStyle.Bad;
        }

        public ConsoleFontColor SelectStyle(StatusStyle usedStyle)
        {
            switch (usedStyle)
            {
                case StatusStyle.Info: return this.InfoStyle;
                case StatusStyle.Ok: return this.FineStyle;
                case StatusStyle.Bad: return this.ErrorStyle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(usedStyle));
            }
        }

        public static StatusStyles Default { get; } = new StatusStyles();
    }
}