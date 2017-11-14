namespace ObscureWare.Console.Root.Core
{
    using System;
    using System.Drawing;
    using System.Text;

    using Conditions;

    using ObscureWare.Console.Root.Shared;

    /// <summary>
    /// IMplemntation of IConsole for Core COnsole application
    /// </summary>
    public class CoreConsole : IConsole
    {
        private readonly CoreConsoleController _controller;

        public CoreConsole(CoreConsoleController controller)
        {
            controller.Requires(nameof(controller)).IsNotNull();

            this._controller = controller;

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
        }

        /// <inheritdoc />
        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {
            this.SetCursorPosition(x, y);
            this.SetColors(foreColor, bgColor);
            this.WriteText(text);
        }

        /// <inheritdoc />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public void WriteText(ConsoleFontColor colors, string text)
        {
            this.SetColors(colors.ForeColor, colors.BgColor);
            Console.Write(text);
        }

        /// <inheritdoc />
        public void WriteText(string text)
        {
            Console.Write(text);
        }

        /// <inheritdoc />
        public void WriteLine(ConsoleFontColor colors, string text)
        {
            this.SetColors(colors.ForeColor, colors.BgColor);
            Console.WriteLine(text);
        }

        /// <inheritdoc />
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc />
        public void SetColors(Color foreColor, Color bgColor)
        {
            Console.ForegroundColor = this._controller.CloseColorFinder.FindClosestColor(foreColor);
            Console.BackgroundColor = this._controller.CloseColorFinder.FindClosestColor(bgColor);
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
            return Console.ReadKey(intercept: true);
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
            this.SetColors(style.ForeColor, style.BgColor);
        }

        /// <inheritdoc />
        public object AtomicHandle => Console.Out; //https://github.com/dotnet/corefx/issues/2808
    }
}
