namespace ObscureWare.Console.Root.Demo
{
    using ObscureWare.Console.Demo.Components;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Root.Desktop;

    public static class Program
    {
        private static void Main(string[] args)
        {
            var controller = new ConsoleController();
            var console = new SystemConsole(controller, new ConsoleStartConfiguration(ConsoleStartConfiguration.Colorfull)
            {
                DesiredRowWidth = 128 // for bars
            });

            DemoRunner runner = new DemoRunner(new IDemo[]
            {
                new RainbowColorsDemo(),
                new SysInfoDemo(),
                new ColorSchemesDemo(),
                new ColorHeuriticsDemo(), 
            }); // TODO: MEF

            runner.RunDemos(console);
        }
    }
}
