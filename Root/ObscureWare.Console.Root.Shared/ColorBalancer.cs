namespace ObscureWare.Console.Root.Shared
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using ObscureWare.Console.Root.Desktop.Schema;
    using ObscureWare.Console.Root.Shared.ColorBalancing;

    public class ColorBalancer : IDisposable
    {
        private readonly ConcurrentDictionary<Color, ConsoleColor> _knownMappings = new ConcurrentDictionary<Color, ConsoleColor>();
        private readonly ColorScheme _scheme;
        private readonly IColorHeuristic _heuristics;
        private bool _disposed = false;

        public ColorBalancer(ColorScheme scheme, IColorHeuristic heuristics)
        {
            this._scheme = scheme;
            this._heuristics = heuristics;
        }

        public void Dispose()
        {
            _knownMappings.Clear();
            this._disposed = true;
        }

        /// <summary>
        /// Default balancer
        /// </summary>
        public static ColorBalancer Default => new ColorBalancer(ColorScheme.Default, new GruchenDefaultColorHeuristic());

        /// <summary>
        /// Tries to find the closest match for given RGB color among current set of colors used by System.Console
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns> 
        public ConsoleColor FindClosestColor(Color color)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(ColorBalancer));
            }

            ConsoleColor cc;
            if (this._knownMappings.TryGetValue(color, out cc))
            {
                return cc;
            }

            var index = this._scheme.colorTable
                .Select((c, idx) => Tuple.Create<uint, int>(c, idx))
                .OrderBy(kp => this._heuristics.CalculateDistance((uint)color.ToArgb(), kp.Item1))
                .First().Item2;
            cc = (ConsoleColor)index;

            this._knownMappings.TryAdd(color, cc);
            return cc;
        }

        /// <summary>
        /// Returns actual ARGB color stored at console enumerated colors.
        /// </summary>
        /// <param name="cc">Enumeration-index in console colors</param>
        /// <returns>ARGB color.</returns>
        public Color GetCurrentConsoleColor(ConsoleColor cc)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(ColorBalancer));
            }

            return Color.FromArgb((int)this._scheme.colorTable[(int)cc]);
        }

        //public CloseColorFinder GetDefault()
        //{
        //    return new CloseColorFinder(GetDefaultDefinitions().ToArray());
        //}

        //private IEnumerable<KeyValuePair<ConsoleColor, Color>> GetDefaultDefinitions()
        //{
        //    // TODO: Provide switch to turn old colors set (now being alternate...)
        //    return this._defaultSet;
        //}
    }
}