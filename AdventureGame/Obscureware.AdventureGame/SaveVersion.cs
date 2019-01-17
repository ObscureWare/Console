namespace ObscureWare.AdventureGame
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public struct SaveVersion
    {
        public string Header;

        public string Version;

        public static SaveVersion Build()
        {

            return new SaveVersion
            {
                // ReSharper disable once StringLiteralTypo
                Header = @"AVSV",
                Version = typeof(SaveVersion).Assembly.GetName().Version.ToString()
            };
        }
    }
}