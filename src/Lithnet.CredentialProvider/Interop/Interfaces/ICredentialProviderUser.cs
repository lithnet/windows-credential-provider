using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Provides methods used to retrieve certain properties of an individual user included in a logon or credential UI.
    /// </summary>
    /// <remarks>Windows 8 introduces the ability to group credential providers by user. The logon UI can display a set of users rather than a set of multiple credential providers for each user. Selecting a user then displays the individual credential provider options associated with that user.</remarks>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("13793285-3EA6-40FD-B420-15F47DA41FBB")]
    [ComImport]
    internal interface ICredentialProviderUser
    {
        /// <summary>
        /// Retrieves the user's security identifier (SID).
        /// </summary>
        /// <param name="sid">The address of a pointer to a buffer that, when this method returns successfully, receives the user's SID. It is the responsibility of the caller to free this resource by calling the CoTaskMemFree function.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetSid([MarshalAs(UnmanagedType.LPWStr)] out string sid);

        /// <summary>
        /// Retrieves string properties from the ICredentialProviderUser object based on the input value.
        /// </summary>
        /// <param name="providerID"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetProviderID(out Guid providerID);

        /// <summary>
        /// One of the following values that specify the property to retrieve.
        /// </summary>
        /// <param name="key">One of the following values that specify the property to retrieve - PKEY_Identity_DisplayName, PKEY_Identity_LogonStatusString, PKEY_Identity_PrimarySid, PKEY_Identity_ProviderID, PKEY_Identity_QualifiedUserName or KEY_Identity_UserName </param>
        /// <param name="stringValue">The address of a pointer to a buffer that, when this method returns successfully, receives the requested string.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetStringValue([In] ref PropertyKey key, [MarshalAs(UnmanagedType.LPWStr)] out string stringValue);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetValue([In] ref PropertyKey key, out PropertyValue value);
    }
}
