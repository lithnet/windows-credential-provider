using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int GetFieldStateDelegate(IntPtr instance, uint fieldId, out int fieldState, out int interactiveState);
}
