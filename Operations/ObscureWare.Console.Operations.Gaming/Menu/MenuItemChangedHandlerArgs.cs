namespace ObscureWare.Console.Operations.Gaming.Menu
{
    public class MenuItemChangedHandlerArgs
    {
        public MenuItemChangedHandlerArgs(ConsoleMenuItem activeItem, ConsoleMenuItem previousItem)
        {
            this.ActiveItem = activeItem;
            this.PreviousItem = previousItem;
        }

        public ConsoleMenuItem ActiveItem { get; private set; }

        public ConsoleMenuItem PreviousItem { get; private set; }
    }
}