namespace ObscureWare.Console.Demo.Shared
{
    using ObscureWare.Console.Root.Shared;

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
        /// <returns></returns>
        bool CanRun();

        /// <summary>
        /// If SelfCreate, demo is responsible for creation of all ICosnole by itself.
        /// </summary>
        ConsoleDemoSharing ConsoleSharing { get; }

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
