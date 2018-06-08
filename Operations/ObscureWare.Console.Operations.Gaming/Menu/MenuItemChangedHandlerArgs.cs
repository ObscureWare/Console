namespace ObscureWare.Console.Operations.Controls.Menu
{
    public class MenuItemChangedHandlerArgs
    {
        public MenuItemChangedHandlerArgs(ConsoleMenuItem activeItem, ConsoleMenuItem previousItem)
        {
            this.ActiveItem = activeItem;
            this.PreviousItem = previousItem;
        }

        public ConsoleMenuItem ActiveItem { get; internal set; }

        public ConsoleMenuItem PreviousItem { get; internal set; }
    }
}