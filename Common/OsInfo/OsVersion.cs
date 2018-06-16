namespace OsInfo
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    // based on https://www.codeproject.com/KB/miscctrl/OSVersionInfo.aspx
    // Added some redesign, cleanup, renaming, refactoring and extensions.
    // Also added cache / lazy accessors.

    /* Original remarks:

        http://www.codeproject.com/Articles/73000/Getting-Operating-System-Version-Info-Even-for-Win
        https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions

        Thanks to Member 7861383, Scott Vickery for the Windows 8.1 update and workaround.
        I have moved it to the beginning of the Name property, though...

        Thakts to Brisingr Aerowing for help with the Windows 10 adapatation

    */

    /// <summary>
    /// Provides detailed information about the host operating system.
    /// </summary>
    public class OsVersion
    {
        private static Lazy<Win10SystemVersion> _win10Version = new Lazy<Win10SystemVersion>(() => new Win10SystemVersion(Info.IsWindows10(), Info.BuildVersion));

        private static Lazy<OsVersion> _info = new Lazy<OsVersion>(() => new OsVersion());

        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

        /// <summary>
        /// Returns more info about Win10 updates
        /// </summary>
        public static Win10SystemVersion Win10SystemInfo => _win10Version.Value;

        /// <summary>
        /// System information
        /// </summary>
        public static OsVersion Info => _info.Value;

        /// <summary>
        /// For mocking singletons.
        /// </summary>
        /// <param name="testVersion"></param>
        /// <param name="testWin10Version"></param>
        public static void OverrideSingletonsWithTestingObjects(OsVersion testVersion = null,
            Win10SystemVersion testWin10Version = null)
        {
            if (testWin10Version != null)
            {
                _win10Version = new Lazy<Win10SystemVersion>(() => testWin10Version);
            }

            if (testVersion != null)
            {
                _info = new Lazy<OsVersion>(() => testVersion);
            }
        }

        private OsVersion()
        {

        }

        #region BITS
        /// <summary>
        /// Determines if the current application is 32 or 64-bit.
        /// </summary>
        public SoftwareArchitecture ApplicationBitness
        {
            get
            {
                SoftwareArchitecture pbits;

                // System.Collections.IDictionary test = Environment.GetEnvironmentVariables();

                switch (IntPtr.Size * 8)
                {
                    case 64:
                        pbits = SoftwareArchitecture.Bit64;
                        break;

                    case 32:
                        pbits = SoftwareArchitecture.Bit32;
                        break;

                    default:
                        pbits = SoftwareArchitecture.Unknown;
                        break;
                }

                return pbits;
            }
        }

        /// <summary>
        /// Determines if the current system / CPU are 32 or 64-bit.
        /// </summary>
        public SoftwareArchitecture SystemBitness
        {
            get
            {
                SoftwareArchitecture osbits;

                switch (IntPtr.Size * 8)
                {
                    case 64:
                        osbits = SoftwareArchitecture.Bit64;
                        break;

                    case 32:
                        osbits = Is32BitProcessOn64BitProcessor() ? SoftwareArchitecture.Bit64 : SoftwareArchitecture.Bit32;
                        break;

                    default:
                        osbits = SoftwareArchitecture.Unknown;
                        break;
                }

                return osbits;
            }
        }

        /// <summary>
        /// Determines if the current processor is 32 or 64-bit.
        /// </summary>
        public ProcessorArchitecture ProcessorBitness
        {
            get
            {
                ProcessorArchitecture pbits = ProcessorArchitecture.Unknown;

                try
                {
                    NativeMethods.SYSTEM_INFO lSystemInfo = new NativeMethods.SYSTEM_INFO();
                    NativeMethods.GetNativeSystemInfo(ref lSystemInfo);

                    switch (lSystemInfo.uProcessorInfo.wProcessorArchitecture)
                    {
                        case 9: // PROCESSOR_ARCHITECTURE_AMD64
                            pbits = ProcessorArchitecture.Bit64;
                            break;
                        case 6: // PROCESSOR_ARCHITECTURE_IA64
                            pbits = ProcessorArchitecture.Itanium64;
                            break;
                        case 0: // PROCESSOR_ARCHITECTURE_INTEL
                            pbits = ProcessorArchitecture.Bit32;
                            break;
                        default: // PROCESSOR_ARCHITECTURE_UNKNOWN
                            pbits = ProcessorArchitecture.Unknown;
                            break;
                    }
                }
                catch
                {
                    // Ignore
                }

                return pbits;
            }
        }
        #endregion BITS

        #region EDITION

        private string _sEdition;
        /// <summary>
        /// Gets the edition of the operating system running on this computer.
        /// </summary>
        public string Edition
        {
            get
            {
                if (_sEdition != null)
                {
                    return _sEdition;  //***** RETURN *****//
                }

                string edition = String.Empty;

                OperatingSystem osVersion = Environment.OSVersion;
                NativeMethods.OSVERSIONINFOEX osVersionInfo = new NativeMethods.OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVERSIONINFOEX));

                if (NativeMethods.GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;
                    byte productType = osVersionInfo.wProductType;
                    short suiteMask = osVersionInfo.wSuiteMask;

                    #region VERSION 4
                    if (majorVersion == 4)
                    {
                        if (productType == NativeMethods.VER_NT_WORKSTATION)
                        {
                            // Windows NT 4.0 Workstation
                            edition = "Workstation";
                        }
                        else if (productType == NativeMethods.VER_NT_SERVER)
                        {
                            if ((suiteMask & NativeMethods.VER_SUITE_ENTERPRISE) != 0)
                            {
                                // Windows NT 4.0 Server Enterprise
                                edition = "Enterprise Server";
                            }
                            else
                            {
                                // Windows NT 4.0 Server
                                edition = "Standard Server";
                            }
                        }
                    }
                    #endregion VERSION 4

                    #region VERSION 5
                    else if (majorVersion == 5)
                    {
                        if (productType == NativeMethods.VER_NT_WORKSTATION)
                        {
                            if ((suiteMask & NativeMethods.VER_SUITE_PERSONAL) != 0)
                            {
                                edition = "Home";
                            }
                            else
                            {
                                if (NativeMethods.GetSystemMetrics(86) == 0) // 86 == SM_TABLETPC
                                    edition = "Professional";
                                else
                                    edition = "Tablet Edition";
                            }
                        }
                        else if (productType == NativeMethods.VER_NT_SERVER)
                        {
                            if (minorVersion == 0)
                            {
                                if ((suiteMask & NativeMethods.VER_SUITE_DATACENTER) != 0)
                                {
                                    // Windows 2000 Datacenter Server
                                    edition = "Datacenter Server";
                                }
                                else if ((suiteMask & NativeMethods.VER_SUITE_ENTERPRISE) != 0)
                                {
                                    // Windows 2000 Advanced Server
                                    edition = "Advanced Server";
                                }
                                else
                                {
                                    // Windows 2000 Server
                                    edition = "Server";
                                }
                            }
                            else
                            {
                                if ((suiteMask & NativeMethods.VER_SUITE_DATACENTER) != 0)
                                {
                                    // Windows Server 2003 Datacenter Edition
                                    edition = "Datacenter";
                                }
                                else if ((suiteMask & NativeMethods.VER_SUITE_ENTERPRISE) != 0)
                                {
                                    // Windows Server 2003 Enterprise Edition
                                    edition = "Enterprise";
                                }
                                else if ((suiteMask & NativeMethods.VER_SUITE_BLADE) != 0)
                                {
                                    // Windows Server 2003 Web Edition
                                    edition = "Web Edition";
                                }
                                else
                                {
                                    // Windows Server 2003 Standard Edition
                                    edition = "Standard";
                                }
                            }
                        }
                    }
                    #endregion VERSION 5

                    #region VERSION 6
                    else if (majorVersion == 6)
                    {
                        int ed;
                        if (NativeMethods.GetProductInfo(majorVersion, minorVersion,
                            osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor,
                            out ed))
                        {
                            switch (ed)
                            {
                                case NativeMethods.PRODUCT_BUSINESS:
                                    edition = "Business";
                                    break;
                                case NativeMethods.PRODUCT_BUSINESS_N:
                                    edition = "Business N";
                                    break;
                                case NativeMethods.PRODUCT_CLUSTER_SERVER:
                                    edition = "HPC Edition";
                                    break;
                                case NativeMethods.PRODUCT_CLUSTER_SERVER_V:
                                    edition = "HPC Edition without Hyper-V";
                                    break;
                                case NativeMethods.PRODUCT_DATACENTER_SERVER:
                                    edition = "Datacenter Server";
                                    break;
                                case NativeMethods.PRODUCT_DATACENTER_SERVER_CORE:
                                    edition = "Datacenter Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_DATACENTER_SERVER_V:
                                    edition = "Datacenter Server without Hyper-V";
                                    break;
                                case NativeMethods.PRODUCT_DATACENTER_SERVER_CORE_V:
                                    edition = "Datacenter Server without Hyper-V (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_EMBEDDED:
                                    edition = "Embedded";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE:
                                    edition = "Enterprise";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_N:
                                    edition = "Enterprise N";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_E:
                                    edition = "Enterprise E";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_SERVER:
                                    edition = "Enterprise Server";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_SERVER_CORE:
                                    edition = "Enterprise Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_SERVER_CORE_V:
                                    edition = "Enterprise Server without Hyper-V (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_SERVER_IA64:
                                    edition = "Enterprise Server for Itanium-based Systems";
                                    break;
                                case NativeMethods.PRODUCT_ENTERPRISE_SERVER_V:
                                    edition = "Enterprise Server without Hyper-V";
                                    break;
                                case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT:
                                    edition = "Essential Business Server MGMT";
                                    break;
                                case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL:
                                    edition = "Essential Business Server ADDL";
                                    break;
                                case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC:
                                    edition = "Essential Business Server MGMTSVC";
                                    break;
                                case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC:
                                    edition = "Essential Business Server ADDLSVC";
                                    break;
                                case NativeMethods.PRODUCT_HOME_BASIC:
                                    edition = "Home Basic";
                                    break;
                                case NativeMethods.PRODUCT_HOME_BASIC_N:
                                    edition = "Home Basic N";
                                    break;
                                case NativeMethods.PRODUCT_HOME_BASIC_E:
                                    edition = "Home Basic E";
                                    break;
                                case NativeMethods.PRODUCT_HOME_PREMIUM:
                                    edition = "Home Premium";
                                    break;
                                case NativeMethods.PRODUCT_HOME_PREMIUM_N:
                                    edition = "Home Premium N";
                                    break;
                                case NativeMethods.PRODUCT_HOME_PREMIUM_E:
                                    edition = "Home Premium E";
                                    break;
                                case NativeMethods.PRODUCT_HOME_PREMIUM_SERVER:
                                    edition = "Home Premium Server";
                                    break;
                                case NativeMethods.PRODUCT_HYPERV:
                                    edition = "Microsoft Hyper-V Server";
                                    break;
                                case NativeMethods.PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT:
                                    edition = "Windows Essential Business Management Server";
                                    break;
                                case NativeMethods.PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING:
                                    edition = "Windows Essential Business Messaging Server";
                                    break;
                                case NativeMethods.PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY:
                                    edition = "Windows Essential Business Security Server";
                                    break;
                                case NativeMethods.PRODUCT_PROFESSIONAL:
                                    edition = "Professional";
                                    break;
                                case NativeMethods.PRODUCT_PROFESSIONAL_N:
                                    edition = "Professional N";
                                    break;
                                case NativeMethods.PRODUCT_PROFESSIONAL_E:
                                    edition = "Professional E";
                                    break;
                                case NativeMethods.PRODUCT_SB_SOLUTION_SERVER:
                                    edition = "SB Solution Server";
                                    break;
                                case NativeMethods.PRODUCT_SB_SOLUTION_SERVER_EM:
                                    edition = "SB Solution Server EM";
                                    break;
                                case NativeMethods.PRODUCT_SERVER_FOR_SB_SOLUTIONS:
                                    edition = "Server for SB Solutions";
                                    break;
                                case NativeMethods.PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM:
                                    edition = "Server for SB Solutions EM";
                                    break;
                                case NativeMethods.PRODUCT_SERVER_FOR_SMALLBUSINESS:
                                    edition = "Windows Essential Server Solutions";
                                    break;
                                case NativeMethods.PRODUCT_SERVER_FOR_SMALLBUSINESS_V:
                                    edition = "Windows Essential Server Solutions without Hyper-V";
                                    break;
                                case NativeMethods.PRODUCT_SERVER_FOUNDATION:
                                    edition = "Server Foundation";
                                    break;
                                case NativeMethods.PRODUCT_SMALLBUSINESS_SERVER:
                                    edition = "Windows Small Business Server";
                                    break;
                                case NativeMethods.PRODUCT_SMALLBUSINESS_SERVER_PREMIUM:
                                    edition = "Windows Small Business Server Premium";
                                    break;
                                case NativeMethods.PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE:
                                    edition = "Windows Small Business Server Premium (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_SOLUTION_EMBEDDEDSERVER:
                                    edition = "Solution Embedded Server";
                                    break;
                                case NativeMethods.PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE:
                                    edition = "Solution Embedded Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STANDARD_SERVER:
                                    edition = "Standard Server";
                                    break;
                                case NativeMethods.PRODUCT_STANDARD_SERVER_CORE:
                                    edition = "Standard Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STANDARD_SERVER_SOLUTIONS:
                                    edition = "Standard Server Solutions";
                                    break;
                                case NativeMethods.PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE:
                                    edition = "Standard Server Solutions (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STANDARD_SERVER_CORE_V:
                                    edition = "Standard Server without Hyper-V (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STANDARD_SERVER_V:
                                    edition = "Standard Server without Hyper-V";
                                    break;
                                case NativeMethods.PRODUCT_STARTER:
                                    edition = "Starter";
                                    break;
                                case NativeMethods.PRODUCT_STARTER_N:
                                    edition = "Starter N";
                                    break;
                                case NativeMethods.PRODUCT_STARTER_E:
                                    edition = "Starter E";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_ENTERPRISE_SERVER:
                                    edition = "Enterprise Storage Server";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE:
                                    edition = "Enterprise Storage Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_EXPRESS_SERVER:
                                    edition = "Express Storage Server";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_EXPRESS_SERVER_CORE:
                                    edition = "Express Storage Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_STANDARD_SERVER:
                                    edition = "Standard Storage Server";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_STANDARD_SERVER_CORE:
                                    edition = "Standard Storage Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_WORKGROUP_SERVER:
                                    edition = "Workgroup Storage Server";
                                    break;
                                case NativeMethods.PRODUCT_STORAGE_WORKGROUP_SERVER_CORE:
                                    edition = "Workgroup Storage Server (core installation)";
                                    break;
                                case NativeMethods.PRODUCT_UNDEFINED:
                                    edition = "Unknown product";
                                    break;
                                case NativeMethods.PRODUCT_ULTIMATE:
                                    edition = "Ultimate";
                                    break;
                                case NativeMethods.PRODUCT_ULTIMATE_N:
                                    edition = "Ultimate N";
                                    break;
                                case NativeMethods.PRODUCT_ULTIMATE_E:
                                    edition = "Ultimate E";
                                    break;
                                case NativeMethods.PRODUCT_WEB_SERVER:
                                    edition = "Web Server";
                                    break;
                                case NativeMethods.PRODUCT_WEB_SERVER_CORE:
                                    edition = "Web Server (core installation)";
                                    break;
                            }
                        }
                    }
                    #endregion VERSION 6
                }

                _sEdition = edition;
                return edition;
            }
        }

        #endregion EDITION

        #region NAME

        private string s_Name;
        /// <summary>
        /// Gets the name of the operating system running on this computer.
        /// </summary>
        public string Name
        {
            get
            {
                if (s_Name != null)
                    return s_Name;  //***** RETURN *****//

                string name = "unknown";

                OperatingSystem osVersion = Environment.OSVersion;
                NativeMethods.OSVERSIONINFOEX osVersionInfo = new NativeMethods.OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVERSIONINFOEX));

                if (NativeMethods.GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;

                    if (majorVersion == 6 && minorVersion == 2)
                    {
                        //The registry read workaround is by Scott Vickery. Thanks a lot for the help!

                        //http://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx

                        // For applications that have been manifested for Windows 8.1 & Windows 10. Applications not manifested for 8.1 or 10 will return the Windows 8 OS version value (6.2). 
                        // By reading the registry, we'll get the exact version - meaning we can even compare against  Win 8 and Win 8.1.
                        string exactVersion = RegistryHelpers.RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
                        if (!string.IsNullOrEmpty(exactVersion))
                        {
                            string[] splitResult = exactVersion.Split('.');
                            majorVersion = Convert.ToInt32(splitResult[0]);
                            minorVersion = Convert.ToInt32(splitResult[1]);
                        }
                        if (IsWindows10())
                        {
                            majorVersion = 10;
                            minorVersion = 0;
                        }
                    }

                    switch (osVersion.Platform)
                    {
                        case PlatformID.Win32S:
                            name = "Windows 3.1";
                            break;
                        case PlatformID.WinCE:
                            name = "Windows CE";
                            break;
                        case PlatformID.Win32Windows:
                            {
                                if (majorVersion == 4)
                                {
                                    string csdVersion = osVersionInfo.szCSDVersion;
                                    switch (minorVersion)
                                    {
                                        case 0:
                                            if (csdVersion == "B" || csdVersion == "C")
                                                name = "Windows 95 OSR2";
                                            else
                                                name = "Windows 95";
                                            break;
                                        case 10:
                                            if (csdVersion == "A")
                                                name = "Windows 98 Second Edition";
                                            else
                                                name = "Windows 98";
                                            break;
                                        case 90:
                                            name = "Windows Me";
                                            break;
                                    }
                                }
                                break;
                            }
                        case PlatformID.Win32NT:
                            {
                                byte productType = osVersionInfo.wProductType;

                                switch (majorVersion)
                                {
                                    case 3:
                                        name = "Windows NT 3.51";
                                        break;
                                    case 4:
                                        switch (productType)
                                        {
                                            case 1:
                                                name = "Windows NT 4.0";
                                                break;
                                            case 3:
                                                name = "Windows NT 4.0 Server";
                                                break;
                                        }
                                        break;
                                    case 5:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                name = "Windows 2000";
                                                break;
                                            case 1:
                                                name = "Windows XP";
                                                break;
                                            case 2:
                                                name = "Windows Server 2003";
                                                break;
                                        }
                                        break;
                                    case 6:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows Vista";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2008";
                                                        break;
                                                }
                                                break;

                                            case 1:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows 7";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2008 R2";
                                                        break;
                                                }
                                                break;
                                            case 2:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows 8";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2012";
                                                        break;
                                                }
                                                break;
                                            case 3:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows 8.1";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2012 R2";
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case 10:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                switch (productType)
                                                {
                                                    case 1:
                                                        name = "Windows 10";
                                                        break;
                                                    case 3:
                                                        name = "Windows Server 2016";
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            }
                    }
                }

                s_Name = name;
                return name;
            }
        }
        #endregion NAME


        #region SERVICE PACK
        /// <summary>
        /// Gets the service pack information of the operating system running on this computer.
        /// </summary>
        public string ServicePack
        {
            get
            {
                string servicePack = String.Empty;
                NativeMethods.OSVERSIONINFOEX osVersionInfo = new NativeMethods.OSVERSIONINFOEX();

                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVERSIONINFOEX));

                if (NativeMethods.GetVersionEx(ref osVersionInfo))
                {
                    servicePack = osVersionInfo.szCSDVersion;
                }

                return servicePack;
            }
        }
        #endregion SERVICE PACK

        #region VERSION

        /// <summary>
        /// Gets the build version number of the operating system running on this computer.
        /// </summary>
        public int BuildVersion => int.Parse(RegistryHelpers.RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuildNumber", "0"));

        /// <summary>
        /// Gets the full version string of the operating system running on this computer.
        /// </summary>
        public string VersionString
        {
            get
            {
                return Version.ToString();
            }
        }

        /// <summary>
        /// Gets the full version of the operating system running on this computer.
        /// </summary>
        public Version Version
        {
            get
            {
                return new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion);
            }
        }

        #region MAJOR

        /// <summary>
        /// Gets the major version number of the operating system running on this computer.
        /// </summary>
        public int MajorVersion
        {
            get
            {
                if (IsWindows10())
                {
                    return 10;
                }
                string exactVersion = RegistryHelpers.RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
                if (!string.IsNullOrEmpty(exactVersion))
                {
                    string[] splitVersion = exactVersion.Split('.');
                    return int.Parse(splitVersion[0]);
                }
                return Environment.OSVersion.Version.Major;
            }
        }

        #endregion MAJOR

        #region MINOR

        /// <summary>
        /// Gets the minor version number of the operating system running on this computer.
        /// </summary>
        public int MinorVersion
        {
            get
            {
                if (IsWindows10())
                {
                    return 0;
                }
                string exactVersion = RegistryHelpers.RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
                if (!string.IsNullOrEmpty(exactVersion))
                {
                    string[] splitVersion = exactVersion.Split('.');
                    return int.Parse(splitVersion[1]);
                }
                return Environment.OSVersion.Version.Minor;
            }
        }

        #endregion MINOR

        #region REVISION

        /// <summary>
        /// Gets the revision version number of the operating system running on this computer.
        /// </summary>
        public int RevisionVersion
        {
            get
            {
                if (IsWindows10())
                {
                    return 0;
                }
                return Environment.OSVersion.Version.Revision;
            }
        }

        #endregion REVISION

        #endregion VERSION

        #region 64 BIT OS DETECTION

        private IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            IntPtr handle = NativeMethods.LoadLibrary("kernel32");

            if (handle != IntPtr.Zero)
            {
                IntPtr fnPtr = NativeMethods.GetProcAddress(handle, "IsWow64Process");

                if (fnPtr != IntPtr.Zero)
                {
                    return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)fnPtr, typeof(IsWow64ProcessDelegate));
                }
            }

            return null;
        }

        private bool Is32BitProcessOn64BitProcessor()
        {
            IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

            if (fnDelegate == null)
            {
                return false;
            }

            bool isWow64;
            bool retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64);

            if (retVal == false)
            {
                return false;
            }

            return isWow64;
        }

        #endregion 64 BIT OS DETECTION

        #region Windows 10 Detection

        private bool IsWindows10()
        {
            string productName = RegistryHelpers.RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "");
            if (productName.StartsWith("Windows 10", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}