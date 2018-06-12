namespace ObscureWare.Console.Root.Demo
{
    using Console.Demo.Shared;

    using Demos.Interfaces;

    using OsInfo;

    using Shared;

    public class SysInfoDemo : IDemo
    {
        /// <inheritdoc />
        public string Name { get; } = "Sys-Info";

        /// <inheritdoc />
        public string Author { get; } = "Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = "Demonstrates system version capabilities plus StatusStyle helper usage.";

        /// <inheritdoc />
        public bool CanRun(OsVersion sysInfo)
        {
            return true; // no limits
        }

        /// <inheritdoc />
        public ConsoleDemoSharing ConsoleSharing { get; } = ConsoleDemoSharing.CanShare;

        /// <inheritdoc />
        public DemoSet Set { get; } = DemoSet.Root;

        /// <inheritdoc />
        public void Run(IConsole console)
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

            console.WaitForNextPage();
        }
    }
}