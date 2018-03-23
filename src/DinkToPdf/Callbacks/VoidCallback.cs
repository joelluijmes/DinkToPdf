using System;
using System.Runtime.InteropServices;

namespace DinkToPdf.Callbacks
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}