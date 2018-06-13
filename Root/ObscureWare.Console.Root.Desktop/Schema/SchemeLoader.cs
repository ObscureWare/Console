namespace ObscureWare.Console.Root.Desktop.Schema
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SchemeLoader
    {
        private static readonly string ExeDirectory = System.IO.Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;

        private static readonly ISchemeParser[] Parsers =
        {
            new IniSchemeParser(),
            new XmlSchemeParser()
        };

        private static readonly string[] SupportedExtensions;
        private static readonly string[] FoldersToScan;
        private static readonly List<string> CustomFolders = new List<string>();

        static SchemeLoader()
        {
            SupportedExtensions = Parsers
                .SelectMany(p => p.GetSupportedExtensions())
                .Distinct()
                .ToArray();

            FoldersToScan = new[]
            {
                Path.Combine(ExeDirectory, "schemes"),
                ExeDirectory,
                @"./schemes/",
                @"./"
            };
        }

        public static void AddCustomFolder(string path)
        {
            if (!CustomFolders.Contains(path))
            {
                CustomFolders.Add(path);
            }
        }

        /// <summary>
        /// This scans predefined locations looking for scheme files (*.ini, *.itermcolors, etc) looking for one that is named as given <param name="schemeName"></param>
        /// </summary>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global (Public API)
        public static ColorScheme TryLoadingScheme(string schemeName)
        {
            // first check if this is direct file
            if (schemeName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                // not a valid path or folder
                return null;
            }

            if (schemeName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                // not valid file name - must be a path.
                return TryLoadingSchemeFromFile(schemeName);
            }

            string[] itemsToScan;
            if (Path.HasExtension(schemeName))
            {
                // look only for specific files
                itemsToScan = FoldersToScan.Concat(CustomFolders)
                    .Select(f => Path.Combine(f, schemeName))
                    .ToArray();
            }
            else
            {
                itemsToScan = FoldersToScan.Concat(CustomFolders)
                    .Select(f => Path.Combine(f, schemeName))
                    .SelectMany(p => SupportedExtensions.Select(e => p + e))
                    .ToArray();
            }

            return (from path in itemsToScan
                    where File.Exists(path)
                    let parser = SelectParser(path)
                    where parser != null
                    select parser.ParseScheme(path, throwExceptions: true))
                   .FirstOrDefault();
        }

        /// <summary>
        /// This is expected to load scheme directly from given file. No folder scanning.
        /// </summary>
        /// <param name="schemeFilePathName"></param>
        /// <returns></returns>
        /// <remarks>It's up to the caller to specify correct extension in the file name. And that the name is correct.</remarks>
        // ReSharper disable once MemberCanBePrivate.Global (Public API)
        public static ColorScheme TryLoadingSchemeFromFile(string schemeFilePathName)
        {
            var file = new FileInfo(schemeFilePathName);
            if (!file.Exists)
            {
                return null;
            }

            var parser = SelectParser(file.FullName);

            return parser?.ParseScheme(file.FullName, throwExceptions: true);
        }

        private static ISchemeParser SelectParser(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            return Parsers.FirstOrDefault(p => p.CanProcess(extension));
        }

        public static IEnumerable<ColorScheme> LoadAllFromFolder(string folder)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            if (dir.Exists)
            {
                var files = dir.GetFiles("*", SearchOption.TopDirectoryOnly); // TODO: improve with usage of SupportedExtensions
                foreach (var file in files)
                {
                    var scheme = TryLoadingSchemeFromFile(file.FullName);
                    if (scheme != null)
                    {
                        yield return scheme;
                    }
                }
            }
        }
    }
}
