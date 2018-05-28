namespace ObscureWare.Console.Root.Desktop
{
    using System.Windows.Forms;

    using ObscureWare.Console.Root.Shared;

    public class ConsoleStartConfiguration
    {
        public ConsoleStartConfiguration(
            bool runFullScreen = false,
            ConsoleMode mode = ConsoleMode.Buffered,
            bool tryVirtualConsole = true,
            Screen startScreen = null,
            uint desiredRowWidth = 0,
            uint desiredRowCount = 0,
            uint desiredBufferRowCount = 0)
        {
            RunFullScreen = runFullScreen;
            Mode = mode;
            TryVirtualConsole = tryVirtualConsole;
            DesiredRowWidth = desiredRowWidth;
            DesiredRowCount = desiredRowCount;
            DesiredBufferRowCount = desiredBufferRowCount;
            StartScreen = startScreen ?? Screen.PrimaryScreen;
        }

        /// <summary>
        /// Specifies, on which screen console window shall be placed.
        /// </summary>
        public Screen StartScreen { get; }

        /// <summary>
        /// Specifies, whether console shall be run full-screen
        /// </summary>
        public bool RunFullScreen { get; }

        /// <summary>
        /// Specifies mode in which console shall run
        /// </summary>
        public ConsoleMode Mode { get; }

        /// <summary>
        /// Specifies, whether console shall try entering virtual mode, enabling 24bit colors and more.
        /// </summary>
        public bool TryVirtualConsole { get; }

        /// <summary>
        /// Ignored when RunFullScreen == TRUE. When == ZERO - use defaults.
        /// </summary>
        public uint DesiredRowWidth { get; }

        /// <summary>
        /// Ignored when RunFullScreen == TRUE. When == ZERO - use defaults.
        /// </summary>
        public uint DesiredRowCount { get; }

        /// <summary>
        /// Used only when Mode == ConsoleMode.Buffered
        /// </summary>
        public uint DesiredBufferRowCount { get; }


        #region Static definitions

        public static ConsoleStartConfiguration Default => new ConsoleStartConfiguration(false, ConsoleMode.Buffered, false);

        public static ConsoleStartConfiguration Colorfull => new ConsoleStartConfiguration(false, ConsoleMode.Buffered, true);

        public static ConsoleStartConfiguration Gaming => new ConsoleStartConfiguration(false, ConsoleMode.SingleScreenNoWrapping, true);

        public static ConsoleStartConfiguration GamingFulLScreen => new ConsoleStartConfiguration(true, ConsoleMode.SingleScreenNoWrapping, true);

        #endregion Static definitions
    }
}