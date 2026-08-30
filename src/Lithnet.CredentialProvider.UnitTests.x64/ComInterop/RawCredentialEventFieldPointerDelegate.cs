using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int RawCredentialEventFieldPointerDelegate(IntPtr instance, IntPtr credential, uint fieldId, IntPtr value);
}
