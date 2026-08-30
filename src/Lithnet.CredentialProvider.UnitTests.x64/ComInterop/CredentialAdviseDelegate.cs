using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int CredentialAdviseDelegate(IntPtr instance, IntPtr credentialEvents);
}
