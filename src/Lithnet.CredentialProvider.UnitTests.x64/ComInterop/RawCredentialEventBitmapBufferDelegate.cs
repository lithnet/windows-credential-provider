using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    // The trailing returnValue pointer characterizes the current CLR projection without PreserveSig.
    // It does not establish the native ABI of the undocumented Windows Events3 interface.
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int RawCredentialEventBitmapBufferDelegate(IntPtr instance, IntPtr credential, uint fieldId, uint imageBufferSize, IntPtr imageBuffer, IntPtr returnValue);
}
