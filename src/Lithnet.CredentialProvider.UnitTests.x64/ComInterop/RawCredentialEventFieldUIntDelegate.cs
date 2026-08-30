using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int RawCredentialEventFieldUIntDelegate(IntPtr instance, IntPtr credential, uint fieldId, uint value);
}
