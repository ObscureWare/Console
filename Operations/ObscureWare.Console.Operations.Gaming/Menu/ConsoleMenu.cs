namespace ObscureWare.Console.Operations.Gaming.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    using Root.Shared;

    /// <summary>
    /// Renders and serves menu content, without frame
    /// </summary>
    public class ConsoleMenu
    {
        private readonly IAtomicConsole _console;
        private readonly Rectangle _availableArea;
        private readonly ConsoleMenuItem[] _menuItems;
        private readonly MenuStyles _styling;
        private ConsoleMenuItem _selectedItem = null;
        private ConsoleMenuItem _activeItem = null;

        public ConsoleMenu(IAtomicConsole console, Rectangle availableArea, IEnumerable<ConsoleMenuItem> menuItems, MenuStyles styling)
        {
            this._console = console ?? throw new ArgumentNullException(nameof(console));
            this._availableArea = availableArea;
            this._styling = styling ?? throw new ArgumentNullException(nameof(styling));

            if (menuItems != null)
            {
                this._menuItems = menuItems.ToArray();
                this._selectedItem = this._menuItems.FirstOrDefault();
            }
            else
            {
                this._menuItems = new ConsoleMenuItem[0];
            }
        }

        public void Render()
        {
            int yindex = 0;
            foreach (var item in this._menuItems)
            {
                // TODO: use Span<char> to optimize. Hopefully System.Console will receive such overload as well
                var caption = (item.Caption.Length <= this._availableArea.Width)
                    ? item.Caption.PadRight(this._availableArea.Width, ' ') // want bg-color replacement after text as well
                    : item.Caption.Substring(0, this._availableArea.Width); // TODO: add elipsis? set style?

                this._console.RunAtomicOperations((c) =>
                {
                    c.SetCursorPosition(this._availableArea.X, this._availableArea.Y + yindex++);
                    c.WriteText(this.ChooseItemStyle(item), caption);
                });
            }
        }

        /// <summary>
        /// Activates menu.
        /// </summary>
        /// <returns>Finally activated menu item.</returns>
        public ConsoleMenuItem Focus() // TODO: async?
        {
            ConsoleKey key = ConsoleKey.Escape;
            while (key != this._styling.SelectionKey)
            {
                key = this._console.ReadKey().Key;

                // TODO: add selection moving + rendering (changed lines only) routines
            }

            this._activeItem = this._selectedItem;
            return this._activeItem;
        }

        /// <summary>
        /// Raised when user moves through menu, yet not activating items
        /// </summary>
        public event MenuItemChangedHandler ItemChanged;

        protected virtual void OnItemChanged(MenuItemChangedHandlerArgs args)
        {
            this.ItemChanged?.Invoke(this, args);
        }

        private ConsoleFontColor ChooseItemStyle(ConsoleMenuItem item)
        {
            if (item.Enabled)
            {
                if (this._selectedItem == item)
                {
                    return this._styling.SelectedItem;
                }
                else if (this._activeItem == item)
                {
                    return this._styling.ActiveItem;
                }
                else
                {
                    return this._styling.NormalItem;
                }
            }
            else
            {
                return this._styling.DisabledItem;
            }
        }
    }
}
