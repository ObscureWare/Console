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
            _innerConsole = innerConsole ?? throw new ArgumentNullException(nameof(innerConsole));
        }

        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteText(x, y, text, foreColor, bgColor);
            }
        }

        public void Clear()
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.Clear();
            }
        }

        public void WriteText(ConsoleFontColor colors, string text)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteText(colors, text);
            }
        }

        public void WriteText(string text)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteText(text);
            }
        }

        public void WriteLine(ConsoleFontColor colors, string text)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteLine(colors, text);
            }
        }

        public void WriteLine(string text)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteLine(text);
            }
        }

        public void SetColors(Color foreColor, Color bgColor)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.SetColors(foreColor, bgColor);
            }
        }

        public void SetCursorPosition(int x, int y)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.SetCursorPosition(x, y);
            }
        }

        public void WriteText(char character)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteText(character);
            }
        }

        // ReSharper disable InconsistentlySynchronizedField (reads are non-locking for obvious reasons...)

        public Point GetCursorPosition()
        {
            return _innerConsole.GetCursorPosition();
        }

        public int WindowHeight => _innerConsole.WindowHeight;

        public int WindowWidth => _innerConsole.WindowWidth;

        public ConsoleMode ConsoleMode => _innerConsole.ConsoleMode;

        public string ReadLine()
        {
            return _innerConsole.ReadLine();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return _innerConsole.ReadKey();
        }

        public object AtomicHandle => _innerConsole.AtomicHandle;

        public void HideCursor()
        {
            _innerConsole.HideCursor();
        }

        public void ShowCursor()
        {
            _innerConsole.ShowCursor();
        }

        // ReSharper restore InconsistentlySynchronizedField

        public void WriteLine()
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.WriteLine();
            }
        }

        public void SetColors(ConsoleFontColor style)
        {
            lock (_innerConsole.AtomicHandle)
            {
                _innerConsole.SetColors(style);
            }
        }

        public void RunAtomicOperations(Action action)
        {
            lock (_innerConsole.AtomicHandle)
            {
                action.Invoke();
            }
        }
    }
}
