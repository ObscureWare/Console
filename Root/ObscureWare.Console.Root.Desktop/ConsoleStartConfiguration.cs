// ReSharper disable MemberCanBePrivate.Global (This is public API)
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

        /// <summary>
        /// If specified - application will try locating and loading specific scheme with colors to be used in 16-colors mode.
        /// </summary>
        /// <remarks>It looks for both INI (*.ini) and XML (*.itermcolors) configurations in following folders:
        /// - current folder
        /// - sub-folder "schemes" of current folder
        /// - folder of executing assembly
        /// - sub-folder "schemes" of where executing assembly is located
        /// - treats given name as absolute path to the scheme file.
        /// Given order is default to color-tool (https://github.com/Microsoft/console/blob/master/tools/ColorTool), yet it might be changed to exact opposite as we feel it to be more intuitive and precise.</remarks>
        public string ColorSchemeName { get; set; } = null;

        #region Static definitions

        /// <summary>
        /// Default, 16-colors console
        /// </summary>
        public static ConsoleStartConfiguration Default => new ConsoleStartConfiguration(runFullScreen: false, mode: ConsoleMode.Buffered, tryVirtualConsole: false);

        /// <summary>
        /// Size and behavior default, yet 24-bit color enabled (if available)
        /// </summary>
        public static ConsoleStartConfiguration Colorfull => new ConsoleStartConfiguration(runFullScreen: false, mode: ConsoleMode.Buffered, tryVirtualConsole: true);

        /// <summary>
        /// Non-wrapping, "non-buffered" single screen console with 24-bit color enabled (if available)
        /// </summary>
        /// <remarks>"Non-buffered" means that there is no off-screen buffer outside console window - it just has the same size and location.</remarks>
        public static ConsoleStartConfiguration Gaming => new ConsoleStartConfiguration(runFullScreen: false, mode: ConsoleMode.SingleScreenNoWrapping, tryVirtualConsole: true);

        /// <summary>
        /// Full-screen, non-wrapping, "non-buffered" single screen console with 24-bit color enabled (if available)
        /// </summary>
        /// <remarks>"Non-buffered" means that there is no off-screen buffer outside console window - it just has the same size and location.</remarks>
        public static ConsoleStartConfiguration GamingFulLScreen => new ConsoleStartConfiguration(runFullScreen: true, mode: ConsoleMode.SingleScreenNoWrapping, tryVirtualConsole: true);

        #endregion Static definitions
    }
}