using System;
using System.Runtime.InteropServices;

namespace DinkToPdf.Callbacks
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void StringCallback(IntPtr converter, [MarshalAs(UnmanagedType.LPStr)] string str);
}