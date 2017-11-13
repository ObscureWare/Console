namespace ObscureWare.Console.Root.Core
{
    using System;
    using System.Drawing;

    using ObscureWare.Console.Root.Interfaces;

    /// <summary>
    /// IMplemntation of IConsole for Core COnsole application
    /// </summary>
    public class CoreConsole : IConsole
    {
        private readonly object _instanceHandle = new object();

        /// <inheritdoc />
        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public void WriteText(ConsoleFontColor colors, string text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteText(string text)
        {
            Console.Write(text);
        }

        /// <inheritdoc />
        public void WriteLine(ConsoleFontColor colors, string text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc />
        public void SetColors(Color foreColor, Color bgColor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        /// <inheritdoc />
        public Point GetCursorPosition()
        {
            return new Point(Console.CursorLeft, Console.CursorTop);
        }

        /// <inheritdoc />
        public void WriteText(char character)
        {
            Console.Write(character);
        }

        /// <inheritdoc />
        public int WindowHeight => Console.WindowHeight;

        /// <inheritdoc />
        public int WindowWidth => Console.WindowWidth;

        /// <inheritdoc />
        public ConsoleMode ConsoleMode => ConsoleMode.Buffered;

        /// <inheritdoc />
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        /// <inheritdoc />
        public void HideCursor()
        {
            Console.CursorVisible = false;
        }

        /// <inheritdoc />
        public void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <inheritdoc />
        public void SetColors(ConsoleFontColor style)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object AtomicHandle => this._instanceHandle;
    }
}
