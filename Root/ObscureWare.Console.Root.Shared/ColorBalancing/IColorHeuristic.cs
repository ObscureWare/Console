namespace ObscureWare.Console.Root.Shared.ColorBalancing
{
    public interface IColorHeuristic
    {
        double CalculateDistance(uint color1, uint color2);
    }
}
