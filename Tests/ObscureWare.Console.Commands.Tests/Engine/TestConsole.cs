namespace ObscureWare.Console.Commands.Tests.Engine
{
    using System;
    using System.Drawing;

    using ObscureWare.Console.Root.Shared;

    internal class TestConsole : IConsole
    {
        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {

        }

        public void Clear()
        {

        }

        public void WriteText(ConsoleFontColor colors, string text)
        {
            Console.Write(text);
        }

        public void WriteText(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(ConsoleFontColor colors, string text)
        {
            Console.WriteLine(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void SetCursorPosition(int x, int y)
        {

        }

        public Point GetCursorPosition()
        {
            return Point.Empty;
        }

        public void WriteText(char character)
        {
            Console.Write(character);
        }

        public int WindowHeight => Console.WindowHeight;

        public int WindowWidth => Console.WindowWidth;

        public ConsoleMode ConsoleMode => ConsoleMode.Buffered;

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public ConsoleKeyInfo ReadKey()
        {
            throw new NotImplementedException();
        }

        public void HideCursor()
        {
            throw new NotImplementedException();
        }

        public void ShowCursor()
        {
            throw new NotImplementedException();
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void SetColors(ConsoleFontColor style)
        {

        }

        public object AtomicHandle { get; }

        public void SetColors(Color foreColor, Color bgColor)
        {

        }
    }
}