namespace ObscureWare.AdventureGame.Interfaces
{
    public interface IItem : IDescribable
    {
        int Quantity { get; set; }
    }
}