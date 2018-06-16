namespace ObscureWare.Console.Demo.Shared
{
    using System.Collections.Generic;
    using System.Drawing;

    using Root.Shared;

    public static class ConsoleDemoExtensions
    {
        public const char ELLIPSIS_CHARACTER = '…';

        private static readonly ConsoleFontColor DefaultStyle = new ConsoleFontColor(Color.WhiteSmoke, Color.Black);
        private static readonly ConsoleFontColor HighlightStyle = new ConsoleFontColor(Color.DarkOrange, Color.Black);

        public static void WaitForNextPage(this IConsole console)
        {
            console.SetColors(DefaultStyle);
            console.WriteLine();
            console.PrintColorfullTextLine(
                new KeyValuePair<ConsoleFontColor, string>(DefaultStyle, "Press "),
                new KeyValuePair<ConsoleFontColor, string>(HighlightStyle, "ENTER"),
                new KeyValuePair<ConsoleFontColor, string>(DefaultStyle, " to continue."));
            console.ReadLine();
        }

        public static void WaitBeforeQuit(this IConsole console)
        {
            console.SetColors(DefaultStyle);
            console.WriteLine();
            console.PrintColorfullTextLine(
                new KeyValuePair<ConsoleFontColor, string>(DefaultStyle, "Holding console window open. Press "),
                new KeyValuePair<ConsoleFontColor, string>(HighlightStyle, "ENTER"),
                new KeyValuePair<ConsoleFontColor, string>(DefaultStyle, " to quit for good."));
            console.ReadLine();
        }

        public static void PrintStatus(this IConsole console, string title, object value, StatusStyles statusStyles, StatusStyle usedStyle)
        {
            console.WriteText(statusStyles.TitleStyle, title);
            console.WriteText(statusStyles.SeparatorStyle, statusStyles.SeparatorText);
            console.WriteLine(statusStyles.SelectStyle(usedStyle), value?.ToString() ?? "");
        }

        public static void PrintLabel(this IConsole console, int posX, int posY, int labelWidth, string text, LabelStyle style)
        {
            string caption = "###";
            var textLength = text.Length;

            if (textLength <= labelWidth)
            {
                switch (style.Alignment)
                {
                    case TextAlign.Left:
                        {
                            caption = text.PadRight(labelWidth);
                            break;
                        }
                    case TextAlign.Right:
                        {
                            caption = text.PadLeft(labelWidth);
                            break;
                        }
                    case TextAlign.Center:
                        {
                            int padLeft = (labelWidth - textLength) / 2;
                            caption = text.PadLeft(padLeft + textLength).PadRight(labelWidth);
                            break;
                        }
                }
            }
            else
            {
                caption = (style.AddEllipsisOnOverflow)
                    ? text.Substring(0, labelWidth - 1) + ELLIPSIS_CHARACTER
                    : text.Substring(0, labelWidth);
            }

            console.WriteText(posX, posY, caption, style.Colors.ForeColor, style.Colors.BgColor);
        }
    }
}
