namespace ObscureWare.AdventureGame.Interfaces
{
    public interface ICreature : IDescribable
    {
        IItemContainer Inventory { get; }

        // descriptor record or code only?

        // IItemContainer Loot { get; }

        // location

        // type (human, animal, etc...)
    }
}