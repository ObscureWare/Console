namespace OsInfo
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Windows_10_version_history</remarks>
    public class Win10SystemVersion
    {
        private readonly bool _isWindows10;
        private readonly int _buildNumber;
        private readonly bool _isMobile;

        internal Win10SystemVersion(bool isWindows10, int buildNumber)
        {
            _isWindows10 = isWindows10;
            _buildNumber = buildNumber;
            _isMobile = Environment.OSVersion.Platform == PlatformID.WinCE;
        }

        public bool IsWindows10 => _isWindows10;

        public bool IsThreshold1Version => _isWindows10 && !_isMobile && _buildNumber < 10240;

        public bool HasNovemberUpdate => _isWindows10 && ((!_isMobile && _buildNumber > 10240) || _isMobile);

        public bool HasAnniversaryUpdate => _isWindows10 && ((!_isMobile && _buildNumber > 10586) || (_isMobile && _buildNumber > 10586));

        public bool HasCreatorsUpdate => _isWindows10 && ((!_isMobile && _buildNumber > 14393) || (_isMobile && _buildNumber > 14393));

        public bool HasFallCreatorsUpdate => _isWindows10 && ((!_isMobile && _buildNumber > 15063) || (_isMobile && _buildNumber > 15063));

        public bool HasApril2018Update => _isWindows10 && ((!_isMobile && _buildNumber > 16299) || (_isMobile && _buildNumber > 15254)); // TODO: update when changed

        public bool HasRedstone5Update => _isWindows10 && ((!_isMobile && _buildNumber > 17134)); // TODO: update when changed

        // TODO	Version 1809 and more

    }
}