using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UTDemo
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(ref SYSTEM_INFO SystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandleW([MarshalAs(UnmanagedType.LPWStr), In] string fileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr), In] string fileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr), In] string procedureName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint GetModuleFileName(
          IntPtr hModule,
          [Out] StringBuilder lpFilename,
          [MarshalAs(UnmanagedType.U4), In] int nSize);
    }
}