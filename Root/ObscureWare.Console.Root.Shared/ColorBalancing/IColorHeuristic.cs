namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    public interface IColorHeuristic
    {
        string Name { get; }

        double CalculateDistance(uint color1, uint color2);
    }
}
