using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UTDemo
{
    internal static class LibraryMethods
    {
        public static IntPtr GetModuleHandle(string fileName)
        {
            IntPtr moduleHandleW = NativeMethods.GetModuleHandleW(fileName);
            if (!(moduleHandleW == IntPtr.Zero))
                return moduleHandleW;
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static IntPtr GetProcAddress(IntPtr hModule, string functionName)
        {
            IntPtr procAddress = NativeMethods.GetProcAddress(hModule, functionName);
            if (procAddress == IntPtr.Zero)
            {
                string moduleFileName = LibraryMethods.GetModuleFileName(hModule);
                throw new Exception(moduleFileName);
            }
            return procAddress;
        }

        public static T GetFunction<T>(IntPtr hModule, string functionName) where T : Delegate
        {
            return (T)Marshal.GetDelegateForFunctionPointer(LibraryMethods.GetProcAddress(hModule, functionName), typeof(T));
        }

        public static string GetModuleFileName(IntPtr hModule)
        {
            StringBuilder lpFilename = new StringBuilder((int)byte.MaxValue);
            if (NativeMethods.GetModuleFileName(hModule, lpFilename, lpFilename.Capacity) == 0U)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return lpFilename.ToString();
        }
    }
}
