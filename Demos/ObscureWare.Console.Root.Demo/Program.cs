namespace ObscureWare.Console.Root.Demo
{
    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Root.Desktop;
    using ObscureWare.Console.Root.Shared;

    using OsInfo;

    using Console = System.Console;

    class Program
    {
        private const byte ESC = 0x1B;

        static void Main(string[] args)
        {
            var controller = new ConsoleController();
            var console = new SystemConsole(controller, ConsoleStartConfiguration.Colorfull);

            PrintOsVersionDemo(console);

            console.WaitForNextPage();


            // color-full tests


            Console.WriteLine("\x1b[31;32;33;34;35;36;101;102;103;104;105;106;107mThis text attempts to apply many colors in the same command. Note the colors are applied from left to right so only the right-most option of foreground cyan (SGR.36) and background bright white (SGR.107) is effective.\r\n");




            console.WaitBeforeQuit();
        }

        private static void PrintOsVersionDemo(IConsole console)
        {
            StatusStyles statusStyles = StatusStyles.Default;
            console.PrintStatus("Windows version", OsVersion.Info.VersionString, statusStyles, StatusStyle.Info);
            console.PrintStatus("App bitness", OsVersion.Info.ApplicationBitness, statusStyles, StatusStyle.Info);
            console.PrintStatus("OS bitness", OsVersion.Info.SystemBitness, statusStyles, StatusStyle.Info);
            console.PrintStatus("CPU bitness", OsVersion.Info.ProcessorBitness, statusStyles, StatusStyle.Info);

            console.WriteLine();

            console.PrintStatus("IsWindows10", OsVersion.Win10SystemInfo.IsWindows10, statusStyles, StatusStyle.Info);
            console.PrintStatus("IsMobile", OsVersion.Win10SystemInfo.IsMobile, statusStyles, StatusStyle.Info);
            console.PrintStatus("HasAnniversaryUpdate", OsVersion.Win10SystemInfo.HasAnniversaryUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasAnniversaryUpdate));
            console.PrintStatus("HasApril2018Update", OsVersion.Win10SystemInfo.HasApril2018Update, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasApril2018Update));
            console.PrintStatus("HasCreatorsUpdate", OsVersion.Win10SystemInfo.HasCreatorsUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasCreatorsUpdate));
            console.PrintStatus("HasFallCreatorsUpdate", OsVersion.Win10SystemInfo.HasFallCreatorsUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasFallCreatorsUpdate));
            console.PrintStatus("HasNovemberUpdate", OsVersion.Win10SystemInfo.HasNovemberUpdate, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasNovemberUpdate));
            console.PrintStatus("HasRedstone5Update", OsVersion.Win10SystemInfo.HasRedstone5Update, statusStyles,
                statusStyles.SelectFlagStyle(OsVersion.Win10SystemInfo.HasRedstone5Update));
            console.PrintStatus("IsThreshold1Version", OsVersion.Win10SystemInfo.IsThreshold1Version, statusStyles,
                statusStyles.SelectFlagStyle(!OsVersion.Win10SystemInfo.IsThreshold1Version));
        }
    }


}
