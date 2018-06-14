namespace ObscureWare.Console.Root.Desktop.ColorBalancing
{
    /// <summary>
    /// By MS
    /// </summary>
    public class NearestNeighborRgbColorHeuristic : BaseMicrosoftColorHeuristic
    {
        /// <inheritdoc />
        public override double CalculateDistance(uint color1, uint color2)
        {
            return Distance(RGB(color1), RGB(color2));
        }
    }
}