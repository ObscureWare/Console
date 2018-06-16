namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    /// <summary>
    /// By MS
    /// </summary>
    public class NearestNeighborHsvColorHeuristic : BaseMicrosoftColorHeuristic
    {
        /// <inheritdoc />
        public override string Name => @"MS Nearest Neighbor HSV";

        /// <inheritdoc />
        public override double CalculateDistance(uint color1, uint color2)
        {
            return Distance(HSV(color1), HSV(color2));
        }
    }
}