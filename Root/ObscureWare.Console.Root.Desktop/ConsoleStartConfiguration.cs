namespace ObscureWare.Console.Root.Desktop
{
    using System.Windows.Forms;

    using Shared;

    public class ConsoleStartConfiguration
    {
        public ConsoleStartConfiguration(
            bool runFullScreen = false,
            ConsoleMode mode = ConsoleMode.Buffered,
            bool tryVirtualConsole = true,
            Screen startScreen = null,
            uint desiredRowWidth = 0,
            uint desiredRowCount = 0,
            uint desiredBufferWidth = 0,
            uint desiredBufferRowCount = 0)
        {
            this.RunFullScreen = runFullScreen;
            this.Mode = mode;
            this.TryVirtualConsole = tryVirtualConsole;
            this.DesiredRowWidth = desiredRowWidth;
            this.DesiredRowCount = desiredRowCount;
            this.DesiredBufferRowWidth = desiredBufferWidth;
            this.DesiredBufferRowCount = desiredBufferRowCount;
            this.StartScreen = startScreen ?? Screen.PrimaryScreen;
        }

        public ConsoleStartConfiguration(ConsoleStartConfiguration baseConfiguration)
        {
            this.RunFullScreen = baseConfiguration.RunFullScreen;
            this.Mode = baseConfiguration.Mode;
            this.TryVirtualConsole = baseConfiguration.TryVirtualConsole;
            this.DesiredRowWidth = baseConfiguration.DesiredRowWidth;
            this.DesiredRowCount = baseConfiguration.DesiredRowCount;
            this.DesiredBufferRowCount = baseConfiguration.DesiredBufferRowCount;
            this.DesiredBufferRowCount = baseConfiguration.DesiredBufferRowCount;
            this.StartScreen = baseConfiguration.StartScreen;
        }

        /// <summary>
        /// Specifies, on which screen console window shall be placed.
        /// </summary>
        public Screen StartScreen { get; set; }

        /// <summary>
        /// Specifies, whether console shall be run full-screen
        /// </summary>
        public bool RunFullScreen { get; set; }

        /// <summary>
        /// Specifies mode in which console shall run
        /// </summary>
        public ConsoleMode Mode { get; set; }

        /// <summary>
        /// Specifies, whether console shall try entering virtual mode, enabling 24bit colors and more.
        /// </summary>
        public bool TryVirtualConsole { get; set; }

        /// <summary>
        /// Ignored when RunFullScreen == TRUE. When == ZERO - use defaults.
        /// </summary>
        public uint DesiredRowWidth { get; set; }

        /// <summary>
        /// Ignored when RunFullScreen == TRUE. When == ZERO - use defaults.
        /// </summary>
        public uint DesiredRowCount { get; set; }

        /// <summary>
        /// Used only when Mode == ConsoleMode.Buffered
        /// </summary>
        public uint DesiredBufferRowCount { get; set; }

        /// <summary>
        /// Used only when Mode == ConsoleMode.Buffered
        /// </summary>
        public uint DesiredBufferRowWidth { get; set; }


        #region Static definitions

        public static ConsoleStartConfiguration Default => new ConsoleStartConfiguration(false, ConsoleMode.Buffered, false);

        public static ConsoleStartConfiguration Colorfull => new ConsoleStartConfiguration(false, ConsoleMode.Buffered, true);

        public static ConsoleStartConfiguration Gaming => new ConsoleStartConfiguration(false, ConsoleMode.SingleScreenNoWrapping, true);

        public static ConsoleStartConfiguration GamingFulLScreen => new ConsoleStartConfiguration(true, ConsoleMode.SingleScreenNoWrapping, true);
        

        #endregion Static definitions
    }
}