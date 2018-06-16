namespace ObscureWare.Console.Operations.Demo
{
    using System.Linq;

    using Console.Demo.Shared;

    using ObscureWare.Tests.Common;

    using Root.Shared;

    using Shared;

    internal class TextSplittingDemo : IDemo
    {
        /// <inheritdoc />
        public string Name { get; } = "Text splitting";

        /// <inheritdoc />
        public string Author { get; } = @"Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = "Demonstrates effects of text splitting routines. (pending functionality, pending demo, need not fake samples).";

        /// <inheritdoc />
        public bool CanRun()
        {
            return true;
        }

        /// <inheritdoc />
        public ConsoleDemoSharing ConsoleSharing { get; } = ConsoleDemoSharing.CanShare;

        /// <inheritdoc />
        public DemoSet Set { get; } = DemoSet.Operations;

        public void Run(IConsole console)
        {
            for (int i = 0; i < 20; i++)
            {
                string str = TestTools.AlphaSentence.BuildRandomStringFrom(20, 50);
                string[] split = str.SplitTextToFit(15).ToArray();

                console.WriteLine(str + " => " + string.Join("|", split));
            }

            console.WaitForNextPage();
        }
    }
}