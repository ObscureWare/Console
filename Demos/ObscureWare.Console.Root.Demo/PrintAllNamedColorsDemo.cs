namespace ObscureWare.Console.Root.Demo
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;

    using Console.Demo.Shared;

    using Shared;

    internal class PrintAllNamedColorsDemo : IDemo
    {
        /// <inheritdoc />
        public string Name { get; } = "Named colors printout";

        /// <inheritdoc />
        public string Author { get; } = @"Sebastian Gruchacz";

        /// <inheritdoc />
        public string Description { get; } = "Demonstrates how all named colors will be presented using default color balancer and current console mode.";

        /// <inheritdoc />
        public bool CanRun()
        {
            return true; // can run on both simulated and virtual, just better ;-)
        }

        /// <inheritdoc />
        public ConsoleDemoSharing ConsoleSharing { get; } = ConsoleDemoSharing.CanShare;

        /// <inheritdoc />
        public DemoSet Set { get; } = DemoSet.Root;

        public void Run(IConsole console)
        {
            var props = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Color));

            var colorBalancer = ColorBalancer.Default;

            foreach (var propertyInfo in props)
            {
                Color c = (Color)propertyInfo.GetValue(null);
                ConsoleColor cc = colorBalancer.FindClosestColor(c);

                console.WriteLine(new ConsoleFontColor(c, Color.Black),
                    string.Format("{0,-25} {1,-18} #{2,-8:X}", propertyInfo.Name, Enum.GetName(typeof(ConsoleColor), cc), c.ToArgb()));
            }

            console.WaitForNextPage();
            console.Clear();
        }
    }
}