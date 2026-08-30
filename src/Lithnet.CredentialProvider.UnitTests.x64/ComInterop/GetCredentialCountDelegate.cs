using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int GetCredentialCountDelegate(IntPtr instance, out uint count, out uint defaultCredential, out int autoLogonWithDefault);
}
