using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int RawQueryInterfaceDelegate(IntPtr instance, ref Guid interfaceId, out IntPtr interfacePointer);
}
