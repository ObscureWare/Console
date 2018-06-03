namespace ObscureWare.Console.Root.Shared
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Wrapper on IConsole for scenarios where some asynchronous operations are done to console.
    /// It's responsible for separating possible conflicts - especially regarding color-change
    /// </summary>
    public class AtomicConsole : IAtomicConsole
    {
        private readonly IConsole _innerConsole;

        public AtomicConsole(IConsole innerConsole)
        {
            this._innerConsole = innerConsole ?? throw new ArgumentNullException(nameof(innerConsole));
        }

        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteText(x, y, text, foreColor, bgColor);
            }
        }

        public void Clear()
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.Clear();
            }
        }

        public void WriteText(ConsoleFontColor colors, string text)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteText(colors, text);
            }
        }

        public void WriteText(string text)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteText(text);
            }
        }

        public void WriteLine(ConsoleFontColor colors, string text)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteLine(colors, text);
            }
        }

        public void WriteLine(string text)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteLine(text);
            }
        }

        public void SetColors(Color foreColor, Color bgColor)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.SetColors(foreColor, bgColor);
            }
        }

        public void SetCursorPosition(int x, int y)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.SetCursorPosition(x, y);
            }
        }

        public void WriteText(char character)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteText(character);
            }
        }

        // ReSharper disable InconsistentlySynchronizedField (reads are non-locking for obvious reasons...)

        public Point GetCursorPosition()
        {
            return this._innerConsole.GetCursorPosition();
        }

        public int WindowHeight => this._innerConsole.WindowHeight;

        public int WindowWidth => this._innerConsole.WindowWidth;

        public ConsoleMode ConsoleMode => this._innerConsole.ConsoleMode;

        public string ReadLine()
        {
            return this._innerConsole.ReadLine();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return this._innerConsole.ReadKey();
        }

        public object AtomicHandle => this._innerConsole.AtomicHandle;

        public void HideCursor()
        {
            this._innerConsole.HideCursor();
        }

        public void ShowCursor()
        {
            this._innerConsole.ShowCursor();
        }

        // ReSharper restore InconsistentlySynchronizedField

        public void WriteLine()
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.WriteLine();
            }
        }

        public void SetColors(ConsoleFontColor style)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                this._innerConsole.SetColors(style);
            }
        }

        public void RunAtomicOperations(Action<IConsole> action)
        {
            lock (this._innerConsole.AtomicHandle)
            {
                action.Invoke(this);
            }
        }
    }
}
