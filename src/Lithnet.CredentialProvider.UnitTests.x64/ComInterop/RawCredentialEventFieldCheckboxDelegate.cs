using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate int RawCredentialEventFieldCheckboxDelegate(IntPtr instance, IntPtr credential, uint fieldId, int isChecked, IntPtr label);
}
