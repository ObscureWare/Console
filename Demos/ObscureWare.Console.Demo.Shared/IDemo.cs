namespace ObscureWare.Console.Demo.Shared
{
    using ObscureWare.Console.Root.Shared;

    using OsInfo;

    public interface IDemo
    {
        /// <summary>
        /// Demo name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Author of the demo
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Longer description of demo
        /// </summary>
        string Description { get; }

        /// <summary>
        /// If demo requires some specific system capabilities it might validate them here
        /// </summary>
        /// <param name="sysInfo"></param>
        /// <returns></returns>
        bool CanRun(OsVersion sysInfo);

        /// <summary>
        /// If SelfCreate, demo is responsible for creation of all ICosnole by itself.
        /// </summary>
        ConsoleSharing ConsoleSharing { get; }

        /// <summary>
        /// Specifies to which set this demo belongs to
        /// </summary>
        DemoSet Set { get; }

        /// <summary>
        /// Runs the demo
        /// </summary>
        /// <param name="console">Will be NULL if ConsoleSharing is SelfCreate</param>
        void Run(IConsole console);
    }
}
