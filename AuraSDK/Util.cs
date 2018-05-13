using System;
using System.Runtime.InteropServices;

namespace AuraSDKDotNet
{
    internal static class Util
    {
        /// <summary>
        /// Creates an array from a memory pointer populated by an external function.
        /// </summary>
        /// <param name="size">Size of the array to be retrieved</param>
        /// <param name="handler">Function that populates the memory space at the pointer</param>
        /// <returns></returns>
        internal static IntPtr[] ArrayFromPointer(int size, Action<IntPtr> handler)
        {
            IntPtr[] array = new IntPtr[size];
            IntPtr pointer = Marshal.AllocHGlobal(size);

            handler(pointer);

            Marshal.Copy(pointer, array, 0, size);
            Marshal.FreeHGlobal(pointer);

            return array;
        }
    }
}
