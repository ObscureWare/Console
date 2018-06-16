namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    /// <summary>
    /// Color heuristic calculation by Sebastian Gruchacz
    /// </summary>
    public class GruchenDefaultColorHeuristic : BaseGruchenColorHeuristic
    {
        // weights are reversed

        private const float COLOR_WEIGHT_HUE = 16.3f;

        private const float COLOR_WEIGHT_SATURATION = 25.3f;

        private const float COLOR_WEIGHT_BRIGHTNESS = 37.3f;

        private const float COLOR_WEIGHT_RED = 28.5f;

        private const float COLOR_WEIGHT_GREEN = 18.5f;

        private const float COLOR_WEIGHT_BLUE = 28.75f;

        private const float COLOR_PROPORTION = 0.2f; // 100f / 255f;


        public GruchenDefaultColorHeuristic() : base(
            COLOR_WEIGHT_HUE,
            COLOR_WEIGHT_SATURATION,
            COLOR_WEIGHT_BRIGHTNESS,
            COLOR_WEIGHT_RED,
            COLOR_WEIGHT_GREEN,
            COLOR_WEIGHT_BLUE,
            COLOR_PROPORTION)
        {

        }

        /// <inheritdoc />
        public override string Name => @"Default Gruchen's";
    }
}