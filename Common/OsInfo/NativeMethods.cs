using System;
using System.Runtime.InteropServices;

namespace OsInfo
{
    internal static class NativeMethods
    {
        // ReSharper disable InconsistentNaming (PInvoke structures named accordingly to win.h definitions...)

        #region GET
        #region PRODUCT INFO
        [DllImport("Kernel32.dll")]
        internal static extern bool GetProductInfo(
            int osMajorVersion,
            int osMinorVersion,
            int spMajorVersion,
            int spMinorVersion,
            out int edition);
        #endregion PRODUCT INFO

        #region VERSION
        [DllImport("kernel32.dll")]
        public static extern bool GetVersionEx(ref NativeMethods.OSVERSIONINFOEX osVersionInfo);
        #endregion VERSION

        #region SYSTEMMETRICS
        [DllImport("user32")]
        public static extern int GetSystemMetrics(int nIndex);
        #endregion SYSTEMMETRICS

        #region SYSTEMINFO
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref NativeMethods.SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        public static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref NativeMethods.SYSTEM_INFO lpSystemInfo);
        #endregion SYSTEMINFO

        #endregion GET

        #region OSVERSIONINFOEX
        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }
        #endregion OSVERSIONINFOEX

        #region SYSTEM_INFO
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            internal NativeMethods._PROCESSOR_INFO_UNION uProcessorInfo;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort dwProcessorLevel;
            public ushort dwProcessorRevision;
        }
        #endregion SYSTEM_INFO

        #region _PROCESSOR_INFO_UNION
        [StructLayout(LayoutKind.Explicit)]
        public struct _PROCESSOR_INFO_UNION
        {
            [FieldOffset(0)]
            internal uint dwOemId;
            [FieldOffset(0)]
            internal ushort wProcessorArchitecture;
            [FieldOffset(2)]
            internal ushort wReserved;
        }
        #endregion _PROCESSOR_INFO_UNION

        #region 64 BIT OS DETECTION
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr LoadLibrary(string libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetProcAddress(IntPtr hwnd, string procedureName);
        #endregion 64 BIT OS DETECTION

        #region PRODUCT

        public const int PRODUCT_UNDEFINED = 0x00000000;
        public const int PRODUCT_ULTIMATE = 0x00000001;
        public const int PRODUCT_HOME_BASIC = 0x00000002;
        public const int PRODUCT_HOME_PREMIUM = 0x00000003;
        public const int PRODUCT_ENTERPRISE = 0x00000004;
        public const int PRODUCT_HOME_BASIC_N = 0x00000005;
        public const int PRODUCT_BUSINESS = 0x00000006;
        public const int PRODUCT_STANDARD_SERVER = 0x00000007;
        public const int PRODUCT_DATACENTER_SERVER = 0x00000008;
        public const int PRODUCT_SMALLBUSINESS_SERVER = 0x00000009;
        public const int PRODUCT_ENTERPRISE_SERVER = 0x0000000A;
        public const int PRODUCT_STARTER = 0x0000000B;
        public const int PRODUCT_DATACENTER_SERVER_CORE = 0x0000000C;
        public const int PRODUCT_STANDARD_SERVER_CORE = 0x0000000D;
        public const int PRODUCT_ENTERPRISE_SERVER_CORE = 0x0000000E;
        public const int PRODUCT_ENTERPRISE_SERVER_IA64 = 0x0000000F;
        public const int PRODUCT_BUSINESS_N = 0x00000010;
        public const int PRODUCT_WEB_SERVER = 0x00000011;
        public const int PRODUCT_CLUSTER_SERVER = 0x00000012;
        private const int PRODUCT_HOME_SERVER = 0x00000013;
        public const int PRODUCT_STORAGE_EXPRESS_SERVER = 0x00000014;
        public const int PRODUCT_STORAGE_STANDARD_SERVER = 0x00000015;
        public const int PRODUCT_STORAGE_WORKGROUP_SERVER = 0x00000016;
        public const int PRODUCT_STORAGE_ENTERPRISE_SERVER = 0x00000017;
        public const int PRODUCT_SERVER_FOR_SMALLBUSINESS = 0x00000018;
        public const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM = 0x00000019;
        public const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;
        public const int PRODUCT_ENTERPRISE_N = 0x0000001B;
        public const int PRODUCT_ULTIMATE_N = 0x0000001C;
        public const int PRODUCT_WEB_SERVER_CORE = 0x0000001D;
        public const int PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT = 0x0000001E;
        public const int PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY = 0x0000001F;
        public const int PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING = 0x00000020;
        public const int PRODUCT_SERVER_FOUNDATION = 0x00000021;
        public const int PRODUCT_HOME_PREMIUM_SERVER = 0x00000022;
        public const int PRODUCT_SERVER_FOR_SMALLBUSINESS_V = 0x00000023;
        public const int PRODUCT_STANDARD_SERVER_V = 0x00000024;
        public const int PRODUCT_DATACENTER_SERVER_V = 0x00000025;
        public const int PRODUCT_ENTERPRISE_SERVER_V = 0x00000026;
        public const int PRODUCT_DATACENTER_SERVER_CORE_V = 0x00000027;
        public const int PRODUCT_STANDARD_SERVER_CORE_V = 0x00000028;
        public const int PRODUCT_ENTERPRISE_SERVER_CORE_V = 0x00000029;
        public const int PRODUCT_HYPERV = 0x0000002A;
        public const int PRODUCT_STORAGE_EXPRESS_SERVER_CORE = 0x0000002B;
        public const int PRODUCT_STORAGE_STANDARD_SERVER_CORE = 0x0000002C;
        public const int PRODUCT_STORAGE_WORKGROUP_SERVER_CORE = 0x0000002D;
        public const int PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE = 0x0000002E;
        public const int PRODUCT_STARTER_N = 0x0000002F;
        public const int PRODUCT_PROFESSIONAL = 0x00000030;
        public const int PRODUCT_PROFESSIONAL_N = 0x00000031;
        public const int PRODUCT_SB_SOLUTION_SERVER = 0x00000032;
        public const int PRODUCT_SERVER_FOR_SB_SOLUTIONS = 0x00000033;
        public const int PRODUCT_STANDARD_SERVER_SOLUTIONS = 0x00000034;
        public const int PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE = 0x00000035;
        public const int PRODUCT_SB_SOLUTION_SERVER_EM = 0x00000036;
        public const int PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM = 0x00000037;
        public const int PRODUCT_SOLUTION_EMBEDDEDSERVER = 0x00000038;

        public const int PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE = 0x00000039;
        //private const int ???? = 0x0000003A;
        public const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT = 0x0000003B;

        public const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL = 0x0000003C;
        public const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC = 0x0000003D;
        public const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC = 0x0000003E;
        public const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE = 0x0000003F;
        public const int PRODUCT_CLUSTER_SERVER_V = 0x00000040;
        public const int PRODUCT_EMBEDDED = 0x00000041;
        public const int PRODUCT_STARTER_E = 0x00000042;
        public const int PRODUCT_HOME_BASIC_E = 0x00000043;
        public const int PRODUCT_HOME_PREMIUM_E = 0x00000044;
        public const int PRODUCT_PROFESSIONAL_E = 0x00000045;
        public const int PRODUCT_ENTERPRISE_E = 0x00000046;

        public const int PRODUCT_ULTIMATE_E = 0x00000047;
        //private const int PRODUCT_UNLICENSED = 0xABCDABCD;
        #endregion PRODUCT

        #region VERSIONS

        public const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        public const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        public const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        public const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        public const int VER_SUITE_PERSONAL = 512;
        public const int VER_SUITE_BLADE = 1024;
        #endregion VERSIONS
    }
}