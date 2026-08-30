using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int GetFieldDescriptorAtDelegate(IntPtr instance, uint index, out IntPtr descriptor);
}
