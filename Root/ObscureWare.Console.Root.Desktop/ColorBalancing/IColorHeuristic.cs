namespace ObscureWare.Console.Root.Desktop.ColorBalancing
{
    public interface IColorHeuristic
    {
        double CalculateDistance(uint color1, uint color2);
    }
}
