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
        public const char ELLIPSIS_CHARACTER = '…';

        private readonly IAtomicConsole _console;
        private readonly Rectangle _availableArea;
        private readonly List<ConsoleMenuItem> _menuItems;
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
                this._menuItems = menuItems.ToList();
                this._selectedItem = this._menuItems.FirstOrDefault();
            }
            else
            {
                this._menuItems = new List<ConsoleMenuItem>();
            }
        }

        public void RenderAll()
        {
            int yIndex = 0;
            foreach (var item in this._menuItems)
            {
                this.RenderSingleItem(item, yIndex++);
            }
        }

        /// <summary>
        /// Activates menu.
        /// </summary>
        /// <param name="resetActiveItem">When TRUE, currently active element highlight will be removed.</param>
        /// <returns>Finally activated menu item.</returns>
        /// <remarks>Note - it auto-hides cursor for better interaction, but does not restore it back on exit</remarks>
        public ConsoleMenuItem Focus(bool resetActiveItem = false) // TODO: async?
        {
            this._console.HideCursor();
            this._selectedItem = this._activeItem ?? this._menuItems.FirstOrDefault();
            if (this._selectedItem != null)
            {
                this.RenderSingleItem(this._selectedItem, this._menuItems.IndexOf(this._selectedItem));
            }
            if (resetActiveItem)
            {
                var tempItem = this._activeItem;
                if (tempItem != null)
                {
                    this._activeItem = null;
                    this.RenderSingleItem(tempItem, this._menuItems.IndexOf(tempItem));
                }
            }

            ConsoleKey key = ConsoleKey.Escape;
            while (key != this._styling.SelectionKey)
            {
                key = this._console.ReadKey().Key;
                int? elementIndex = null;
                if (key == this._styling.MoveUpKey)
                {
                    elementIndex = this.FindItemAbove();
                }
                else if (key == this._styling.MoveDownKey)
                {
                    elementIndex = this.FindItemBelow();
                }
                else if (key == this._styling.MoveTopKey)
                {
                    elementIndex = this.FindFirstItem();
                }
                else if (key == this._styling.MoveBottomKey)
                {
                    elementIndex = this.FindLastItem();
                }

                if (elementIndex.HasValue)
                {
                    var currentItemIndex = this.FindSelectedItem();
                    if (currentItemIndex != elementIndex)
                    {
                        var tempItem = this._selectedItem;

                        this._selectedItem = this._menuItems[elementIndex.Value];

                        this.RenderSingleItem(this._selectedItem, elementIndex.Value);


                        if (currentItemIndex.HasValue)
                        {
                            this.RenderSingleItem(tempItem, currentItemIndex.Value);
                        }

                        this.OnItemChanged(new MenuItemChangedHandlerArgs(this._selectedItem, tempItem));
                    }
                }
            }

            this._activeItem = this._selectedItem;
            this.RenderSingleItem(this._activeItem, this._menuItems.IndexOf(this._activeItem));
            this._selectedItem = null;
            this.RenderAll();
            return this._activeItem;
        }

        private int? FindSelectedItem()
        {
            if (this._selectedItem == null)
            {
                return null;
            }

            return this._menuItems.IndexOf(this._selectedItem);
        }

        private int? FindLastItem() // enabled items only
        {
            for (int i = this._menuItems.Count - 1; i > 0; i--)
            {
                if (this._menuItems[i].Enabled)
                {
                    return i;
                }
            }

            return null;
        }

        private int? FindFirstItem() // enabled items only
        {
            for (int i = 0; i < this._menuItems.Count; i++)
            {
                if (this._menuItems[i].Enabled)
                {
                    return i;
                }
            }

            return null;
        }

        private int? FindItemBelow()
        {
            var selectedItemIndex = this.FindSelectedItem();
            if (selectedItemIndex.HasValue)
            {
                for (int i = selectedItemIndex.Value + 1; i < this._menuItems.Count; i++)
                {
                    if (this._menuItems[i].Enabled)
                    {
                        return i;
                    }
                }

                return selectedItemIndex; // no more
            }

            return null;
        }

        private int? FindItemAbove()
        {
            var selectedItemIndex = this.FindSelectedItem();
            if (selectedItemIndex.HasValue)
            {
                for (int i = selectedItemIndex.Value - 1; i >= 0; i--)
                {
                    if (this._menuItems[i].Enabled)
                    {
                        return i;
                    }
                }

                return selectedItemIndex; // no more
            }

            return null;
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

        private void RenderSingleItem(ConsoleMenuItem item, int yIndex)
        {
            // TODO: use Span<char> to optimize. Hopefully System.Console will receive such overload as well
            string caption = "###";
            if (item.Caption.Length <= this._availableArea.Width)
            {
                switch (this._styling.Alignment)
                {
                    case System.Windows.Forms.HorizontalAlignment.Left:
                    {
                        caption = item.Caption.PadRight(this._availableArea.Width);
                        break;
                    }
                    case System.Windows.Forms.HorizontalAlignment.Right:
                    {
                        caption = item.Caption.PadLeft(this._availableArea.Width);
                        break;
                    };
                    case System.Windows.Forms.HorizontalAlignment.Center:
                    {
                        int padLeft = (this._availableArea.Width - item.Caption.Length) / 2;
                        caption = item.Caption.PadLeft(padLeft + item.Caption.Length).PadRight(this._availableArea.Width);
                        break;
                    }
                }
            }
            else
            {
                caption = (this._styling.AddEllipsisOnOverflow)
                    ? item.Caption.Substring(0, this._availableArea.Width - 1) + ELLIPSIS_CHARACTER
                    : item.Caption.Substring(0, this._availableArea.Width);
            }

            this._console.RunAtomicOperations((c) =>
            {
                c.SetCursorPosition(this._availableArea.X, this._availableArea.Y + yIndex);
                c.WriteText(this.ChooseItemStyle(item), caption);
            });
        }
    }
}
