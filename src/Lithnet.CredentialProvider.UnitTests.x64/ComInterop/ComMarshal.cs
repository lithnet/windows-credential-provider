using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal static class ComMarshal
    {
        public static int QueryInterface(IntPtr unknown, Guid interfaceId, out IntPtr interfacePointer)
        {
#if NET9_0_OR_GREATER
            return Marshal.QueryInterface(unknown, in interfaceId, out interfacePointer);
#else
            return Marshal.QueryInterface(unknown, ref interfaceId, out interfacePointer);
#endif
        }
    }
}
