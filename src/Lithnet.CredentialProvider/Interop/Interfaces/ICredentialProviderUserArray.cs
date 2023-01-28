using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Represents the set of users that will appear in the logon or credential UI. This information enables the credential provider to enumerate the set to retrieve property information about each user to populate fields or filter the set.
    /// </summary>
    /// <remarks>This object is provided by the Windows credential provider framework to your credential provider through the ICredentialProviderSetUserArray::SetUserArray method. Ownership of this object remains with the credential provider framework.</remarks>
	[Guid("90C119AE-0F18-4520-A1F1-114366A40FE8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICredentialProviderUserArray
	{
        /// <summary>
        /// Limits the set of users in the array to either local accounts or Microsoft accounts.
        /// </summary>
        /// <param name="guidProviderToFilterTo">Set this parameter to Identity_LocalUserProvider for the local accounts credential provider; otherwise set it to the GUID of the Microsoft account provider.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int SetProviderFilter([In] ref Guid guidProviderToFilterTo);

        /// <summary>
        /// Retrieves a value that indicates whether the "Other user" tile for local or Microsoft accounts is shown in the logon or credential UI. This information can be used by a credential provider to show the same behavior as the password or Microsoft account provider.
        /// </summary>
        /// <param name="credentialProviderAccountOptions">A pointer to a value that, when this method returns successfully, receives one or more flags that specify which empty tiles are shown by the logon or credential UI.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int GetAccountOptions(out AccountOptions credentialProviderAccountOptions);

        /// <summary>
        /// Retrieves the number of ICredentialProviderUser objects in the user array.
        /// </summary>
        /// <param name="userCount">A pointer to a value that, when this method returns successfully, receives the number of users in the array.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int GetCount(out uint userCount);

        /// <summary>
        /// Retrieves a specified user from the array.
        /// </summary>
        /// <param name="userIndex">The 0-based array index of the user. The size of the array can be obtained through the GetCount method.</param>
        /// <param name="user">The address of a pointer to an object that, when this method returns successfully, represents the specified user. It is the responsibility of the caller to free this object when it is no longer needed by calling its Release method.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int GetAt([In] uint userIndex, [MarshalAs(UnmanagedType.Interface)] out ICredentialProviderUser user);
	}
}
