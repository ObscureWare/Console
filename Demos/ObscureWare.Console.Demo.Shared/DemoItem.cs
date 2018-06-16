namespace ObscureWare.Console.Demo.Components
{
    using Shared;

    internal class DemoItem
    {
        public IDemo Demo { get; set; }
        public int Number { get; set; }
        public string DisplayTitle { get; set; }
        public string[] DescriptionRows { get; set; }
        public bool Enabled { get; set; }
    }
}