using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A5DA53F9-D475-4080-A120-910C4A739880")]
	[ComImport]
    internal interface ICredentialProviderFilter
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int Filter([In] UsageScenario cpus, [In] CredUIWinFlags dwFlags, IntPtr rgclsidProviders, IntPtr rgbAllow, [In] uint cProviders);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int UpdateRemoteCredential([In] ref CredentialSerialization pcpcsIn, ref CredentialSerialization pcpcsOut);
	}
}
