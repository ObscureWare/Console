namespace ObscureWare.AdventureGame.Interfaces
{
    public interface ISubstanceHolder : IDescribable
    {
        /// <summary>
        /// Volume of the container 
        /// </summary>
        decimal Volume { get; set; }

        /// <summary>
        /// Already taken space
        /// </summary>
        decimal Amount { get; set; }

        /// <summary>
        /// Type of the substance in container
        /// </summary>
        ISubstance Substance { get; set; }
    }
}