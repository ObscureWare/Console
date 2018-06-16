namespace ObscureWare.Console.Operations.Demo
{
    using System;
    using System.Drawing;

    using Console.Demo.Shared;

    using Implementation;

    using Root.Shared;

    using TestShared;

    internal class CommandLineDemo : IDemo
    {
        /// <inheritdoc />
        public string Name { get; } = "Command-line";

        /// <inheritdoc />
        public string Author { get; } = @"Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = @"(pending) simulation of simple command line (without actual commands and command engine).";

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
            console.WriteLine("Starting command line simulator (entry line only, no real commands). Type 'exit' `command` to stop it.");
            console.WriteLine("There are also some fake auto-completion suggestions starting with 'a*' and 's'. Use (Shit)+Tab to test them.");

            var promptConsoleColor = new ConsoleFontColor(Color.LightSkyBlue, Color.Black);
            var cmdColor = new ConsoleFontColor(Color.LightGray, Color.Black);
            var retypeColor = new ConsoleFontColor(Color.Lime, Color.Black);

            VirtualEntryLine cmdSimulator = new VirtualEntryLine(console, new FakeClipBoard(), cmdColor);

            var fakeAutocompleter = new TestingAutoCompleter("abcd", "aabbdd", "asddhhhs","nbbdbd", "sdsdsds", "sddsdssfdf", "abc", "sdessse");
            // TODO: replace with dynamic auto-completer that does not ignore letter already enetered
            string cmd = "";
            while (!string.Equals(cmd, "EXIT", StringComparison.OrdinalIgnoreCase))
            {
                console.WriteText(promptConsoleColor, "c:\\");
                cmd = cmdSimulator.GetUserEntry(fakeAutocompleter);
                console.WriteLine();
                console.WriteLine(retypeColor, cmd);
                console.WriteLine();
            }
        }
    }
}