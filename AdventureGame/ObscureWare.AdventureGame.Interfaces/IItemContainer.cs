namespace ObscureWare.AdventureGame.Interfaces
{
    using System.Collections.Generic;

    public interface IItemContainer
    {
        IList<IItem> Items { get; }
    }
}