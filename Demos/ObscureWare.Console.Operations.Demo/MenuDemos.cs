namespace ObscureWare.Console.Operations.Demo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;

    using ObscureWare.Console.Demo.Shared;
    using ObscureWare.Console.Operations.Controls.Menu;
    using ObscureWare.Console.Operations.Implementation;
    using ObscureWare.Console.Root.Shared;

    using FrameStyle = ObscureWare.Console.Operations.Interfaces.Styles.FrameStyle;

    internal static class MenuDemos
    {
        public static void MenuDemo(IConsole console)
        {
            console.Clear();
            var exitGuid = new Guid(@"a7725515-7f82-4c18-9c36-343003bdf20d");

            var menuItems = new ConsoleMenuItem[]
            {
                new ConsoleMenuItem
                {
                    Enabled = true,
                    Caption = "menu item number one",
                    Code = Guid.NewGuid()
                },
                new ConsoleMenuItem
                {
                    Enabled = true,
                    Caption = "menu item number two",
                    Code = Guid.NewGuid()
                },
                new ConsoleMenuItem
                {
                    Enabled = false,
                    Caption = "menu item number three",
                    Code = Guid.NewGuid()
                },
                new ConsoleMenuItem
                {
                    Enabled = true,
                    Caption = "menu item number four with very long description",
                    Code = Guid.NewGuid()
                },
                new ConsoleMenuItem
                {
                    Enabled = false,
                    Caption = "menu item number five",
                    Code = Guid.NewGuid()
                },
                new ConsoleMenuItem
                {
                    Enabled = true,
                    Caption = "menu item number six",
                    Code = Guid.NewGuid()
                },
                new ConsoleMenuItem
                {
                    Enabled = true,
                    Caption = "Exit menu DEMO",
                    Code = exitGuid
                }
            };

            var menuStyling = new MenuStyles
            {
                ActiveItem = new ConsoleFontColor(Color.Red, Color.Black),
                DisabledItem = new ConsoleFontColor(Color.Gray, Color.Black),
                NormalItem = new ConsoleFontColor(Color.WhiteSmoke, Color.Black),
                SelectedItem = new ConsoleFontColor(Color.Black, Color.LightGray),
                Alignment = TextAlign.Center
            };

            var aConsole = new AtomicConsole(console);

            int menuStartY = 6;
            int menuStartX = 5;
            int menuContentWidth = 25;

            var frameColors = new ConsoleFontColor(Color.Yellow, Color.Black);
            var boxInnerColors = new ConsoleFontColor(Color.WhiteSmoke, Color.Black);
            var frameStyle = new FrameStyle(frameColors, boxInnerColors, @"┌─┐││└─┘", ' ');

            var ops = new ConsoleOperations(aConsole);
            ops.WriteTextBox(new Rectangle(menuStartX - 1, menuStartY - 1, menuContentWidth + 2, menuItems.Length + 2), "", frameStyle);

            var menu = new ConsoleMenu(aConsole, new Rectangle(menuStartX, menuStartY, menuContentWidth, 0), menuItems, menuStyling);
            menu.RenderAll();

            ConsoleMenuItem result = null;
            while (result == null || result.Code != exitGuid)
            {
                result = menu.Focus(resetActiveItem: true);

                aConsole.CleanLineSync(0, Color.Black);
                aConsole.RunAtomicOperations(ac =>
                    {
                        aConsole.SetCursorPosition(0, 0);
                        aConsole.PrintColorfullText(
                            new KeyValuePair<ConsoleFontColor, string>(new ConsoleFontColor(Color.BlanchedAlmond, Color.Black), @"Selected menu: "),
                            new KeyValuePair<ConsoleFontColor, string>(new ConsoleFontColor(Color.Gold, Color.Black), result?.Caption ?? "#NULL#")
                            );
                    });
                Thread.Sleep(1000);
            }

            aConsole.SetCursorPosition(0, menuStartY + menuItems.Length + 1);
            aConsole.ShowCursor();

            aConsole.WaitForNextPage();
        }
    }
}