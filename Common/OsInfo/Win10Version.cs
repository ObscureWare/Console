namespace OsInfo
{
    public class Win10Version
    {
        private readonly bool _isWindows10;
        private readonly int _minorVersion;

        internal Win10Version(bool isWindows10, int minorVersion)
        {
            _isWindows10 = isWindows10;
            _minorVersion = minorVersion;
        }

        public bool HasBaseVersion => _isWindows10 && _minorVersion >= 1507;

        public bool HasNovemberUpdate => _isWindows10 && _minorVersion >= 1511;

        public bool HasAnniversaryUpdate => _isWindows10 && _minorVersion >= 1607;

        public bool HasCreatorsUpdate => _isWindows10 && _minorVersion >= 1703;

        public bool HasFallUpdate => _isWindows10 && _minorVersion >= 1709;

        public bool HasApril2018Update => _isWindows10 && _minorVersion >= 1803;

        // TODO	Version 1809 an dmore

    }
}