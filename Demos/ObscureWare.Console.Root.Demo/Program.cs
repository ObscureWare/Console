namespace ObscureWare.Console.Root.Demo
{
    using ObscureWare.Console.Root.Desktop;

    using OsInfo;

    using Console = System.Console;

    class Program
    {
        private const byte ESC = 0x1B;

        static void Main(string[] args)
        {

            Console.WriteLine("IsWindows10 => {0}", OsVersion.Win10SystemInfo.IsWindows10);
            Console.WriteLine("IsMobile => {0}", OsVersion.Win10SystemInfo.IsMobile);
            Console.WriteLine("HasAnniversaryUpdate => {0}", OsVersion.Win10SystemInfo.HasAnniversaryUpdate);
            Console.WriteLine("HasApril2018Update => {0}", OsVersion.Win10SystemInfo.HasApril2018Update);
            Console.WriteLine("HasCreatorsUpdate => {0}", OsVersion.Win10SystemInfo.HasCreatorsUpdate);
            Console.WriteLine("HasFallCreatorsUpdate => {0}", OsVersion.Win10SystemInfo.HasFallCreatorsUpdate);
            Console.WriteLine("HasNovemberUpdate => {0}", OsVersion.Win10SystemInfo.HasNovemberUpdate);
            Console.WriteLine("HasRedstone5Update => {0}", OsVersion.Win10SystemInfo.HasRedstone5Update);
            Console.WriteLine("IsThreshold1Version => {0}", OsVersion.Win10SystemInfo.IsThreshold1Version);


            // color-full tests
            var controller = new ConsoleController();
            var console = new SystemConsole(controller);
            Console.WriteLine("\x1b[31;32;33;34;35;36;101;102;103;104;105;106;107mThis text attempts to apply many colors in the same command. Note the colors are applied from left to right so only the right-most option of foreground cyan (SGR.36) and background bright white (SGR.107) is effective.\r\n");
        }
    }
}
