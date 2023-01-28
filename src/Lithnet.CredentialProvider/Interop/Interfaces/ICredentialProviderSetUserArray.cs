using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Provides a method that enables a credential provider to receive the set of users that will be shown in the logon or credential UI.
    /// </summary>
    /// <remarks>
    ///  Implement this interface for credential providers that have a need to know which users will appear in the logon or credential UI. 
    /// </remarks>
	[Guid("095C1484-1C0C-4388-9C6D-500E61BF84BD")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICredentialProviderSetUserArray
	{
        /// <summary>
        /// Called by the system during the initialization of a logon or credential UI to retrieve the set of users to show in that UI.
        /// </summary>
        /// <param name="users">A pointer to an array object that contains a set of ICredentialProviderUser objects, each representing a user that will appear in the logon or credential UI. This array enables the credential provider to enumerate and query each of the user objects for their SID, their associated credential provider's ID, various forms of the user name, and their logon status string.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int SetUserArray([MarshalAs(UnmanagedType.Interface)] [In] ICredentialProviderUserArray users);
	}
}
