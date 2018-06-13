namespace ObscureWare.Console.Root.Demo
{
    using Demos.Interfaces;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Root.Desktop;

    using OsInfo;

    using Console = System.Console;

    public static class Program
    {
        private const byte ESC = 0x1B;

        private static void Main(string[] args)
        {
            var controller = new ConsoleController();
            var console = new SystemConsole(controller, new ConsoleStartConfiguration(ConsoleStartConfiguration.Colorfull)
            {
                DesiredRowWidth = 128 // for bars
            });

            console.PrintStatus("Virtual Console enabled", console.VirtualConsoleEnabled, StatusStyles.Default, StatusStyles.Default.SelectFlagStyle(console.VirtualConsoleEnabled));
            console.WriteLine();

            DemoRunner runner = new DemoRunner(new IDemo[]
            {
                new RainbowColorsDemo(), 
                new SysInfoDemo()
            }, 
            OsVersion.Info);

            runner.RunDemos(console);

            console.WaitBeforeQuit();
        }
    }
}
