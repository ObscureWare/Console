namespace ObscureWare.Console.Operations.Gaming.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;

    using Root.Shared;

    /// <summary>
    /// Renders and serves menu content, without frame
    /// </summary>
    public class ConsoleMenu
    {
        private readonly IConsole _console;
        private readonly Rectangle _availableArea;
        private readonly IEnumerable<ConsoleMenuItem> _menuItems;
        private readonly MenuStyles _styling;

        public ConsoleMenu(IConsole console, Rectangle availableArea, IEnumerable<ConsoleMenuItem> menuItems, MenuStyles styling)
        {
            this._console = console;
            this._availableArea = availableArea;
            this._menuItems = menuItems;
            this._styling = styling;
        }

        public void Render()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Activates menu
        /// </summary>
        /// <returns></returns>
        public async Task<ConsoleMenuItem> Activate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Raised when user moves through menu
        /// </summary>
        public event MenuItemChangedHandler ItemChanged;

        protected virtual void OnItemChanged(MenuItemChangedHandlerArgs args)
        {
            this.ItemChanged?.Invoke(this, args);
        }
    }
}
