namespace ObscureWare.Console.Demo.Shared
{
    using Demos.Interfaces;

    internal class DemoItem
    {
        public IDemo Demo { get; set; }
        public int Number { get; set; }
        public string DisplayTitle { get; set; }
        public string[] DescriptionRows { get; set; }
        public bool Enabled { get; set; }
    }
}