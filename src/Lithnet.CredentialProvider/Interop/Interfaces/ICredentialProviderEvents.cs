using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Provides an asynchronous callback mechanism used by a credential provider to notify it of changes in the list of credentials or their fields.
    /// </summary>
    /// <remarks>
    /// An implementation of ICredentialProviderEvents is provided for use by outside parties implementing a credential provider. 
    /// </remarks>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("34201E5A-A787-41A3-A5A4-BD6DCF2A854E")]
	[ComImport]
    internal interface ICredentialProviderEvents
	{
        /// <summary>
        /// Signals the Logon UI or Credential UI that the enumerated list of credentials has changed. This happens when the number of credentials change, the individual credentials change, or the number of fields available change. This is an asynchronous method.
        /// </summary>
        /// <param name="upAdviseContext">A pointer to an integer that uniquely identifies which credential provider has requested re-enumeration. The credential provider should pass back the interface pointer it received from Advise in this parameter.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int CredentialsChanged(IntPtr upAdviseContext);
	}
}
