using System;
using System.Runtime.InteropServices;

namespace AuraSDKDotNet
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        internal static extern bool FreeLibrary(IntPtr dllHandle);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);
    }
}
