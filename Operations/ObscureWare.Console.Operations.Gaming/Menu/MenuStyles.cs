namespace ObscureWare.Console.Operations.Gaming.Menu
{
    using System;

    using Root.Shared;

    public class MenuStyles
    {
        public ConsoleFontColor NormalItem { get; set; }

        public ConsoleFontColor DisabledItem { get; set; }

        public ConsoleFontColor SelectedItem { get; set; }

        public ConsoleFontColor ActiveItem { get; set; }

        public ConsoleKey SelectionKey { get; set; } = ConsoleKey.Enter;

        public ConsoleKey MoveUpKey { get; set; } = ConsoleKey.UpArrow;

        public ConsoleKey MoveDownKey { get; set; } = ConsoleKey.DownArrow;

        public ConsoleKey MoveTopKey { get; set; } = ConsoleKey.Home;

        public ConsoleKey MoveBottomKey { get; set; } = ConsoleKey.End;
    }
}