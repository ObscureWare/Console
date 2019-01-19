namespace ObscureWare.AdventureGame.Interfaces
{
    public interface ISubstanceHolder : IDescribable
    {
        decimal Volume { get; set; }

        decimal Amount { get; set; }

        ISubstance Substance { get; set; }
    }
}