using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [ComVisible(true)]
    [Guid("90C119AE-0F18-4520-A1F1-114366A40FE8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestCredentialProviderUserArray
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetProviderFilter(ref Guid providerToFilterTo);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetAccountOptions(out int accountOptions);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetCount(out uint userCount);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetAt(uint userIndex, out IntPtr user);
    }
}
