namespace ObscureWare.Console.Root.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public static class ConsoleExtensions
    {
        public static void CleanLineSync(this IAtomicConsole console, int? lineNo = null, Color? color = null) // add colors...
        {
            if (console == null) throw new ArgumentNullException(nameof(console));

            console.RunAtomicOperations((ac =>
            {
                lineNo = lineNo ?? ac.GetCursorPosition().Y;
                color = color ?? Color.Black;

                string text = new string(' ', console.WindowWidth);
                ac.SetCursorPosition(0, lineNo.Value);
                ac.WriteText(0, lineNo.Value, text, color.Value, color.Value);
            }));
        }

        public static void PrintColorfullText(this IConsole console,
            params KeyValuePair<ConsoleFontColor, string>[] texts)
        {
            foreach (var pair in texts)
            {
                console.WriteText(pair.Key, pair.Value);
            }
        }

        public static void PrintColorfullTextLine(this IConsole console,
            params KeyValuePair<ConsoleFontColor, string>[] texts)
        {
            foreach (var pair in texts)
            {
                console.WriteText(pair.Key, pair.Value);
            }

            console.WriteLine();
        }

        public static void PrintColorfullTextSync(this IAtomicConsole console,
            params KeyValuePair<ConsoleFontColor, string>[] texts)
        {

            console.RunAtomicOperations(ac =>
                {
                    foreach (var pair in texts)
                    {
                        ac.WriteText(pair.Key, pair.Value);
                    }
                }
            );
        }

        public static void PrintColorfullTextLineSync(this IAtomicConsole console,
            params KeyValuePair<ConsoleFontColor, string>[] texts)
        {
            console.RunAtomicOperations(ac =>
                {
                    foreach (var pair in texts)
                    {
                        ac.WriteText(pair.Key, pair.Value);
                    }

                    ac.WriteLine();
                }
            );
        }
    }
}
