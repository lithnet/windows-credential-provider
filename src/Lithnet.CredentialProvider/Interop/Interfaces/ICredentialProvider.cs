using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("D27C3481-5A1C-45B2-8AAA-C20EBBE8229E")]
    [ComImport]
    internal interface ICredentialProvider
    {
        /// <summary>
        /// Defines the scenarios for which the credential provider is valid. Called whenever the credential provider is initialized.
        /// </summary>
        /// <param name="cpus">The scenario the credential provider has been created in. This is the usage scenario that needs to be supported. See the Remarks for more information.</param>
        /// <param name="dwFlags">A value that affects the behavior of the credential provider. This value can be a bitwise-OR combination of one or more of the following values defined in Wincred.h. See CredUIPromptForWindowsCredentials for more information.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetUsageScenario([In] UsageScenario cpus, [In] CredUIWinFlags dwFlags);


        /// <summary>
        /// Sets the serialization characteristics of the credential provider.
        /// </summary>
        /// <param name="pcpcs">A CredentialSerialization structure that stores the serialization characteristics of the credential provider</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetSerialization([In] ref CredentialSerialization pcpcs);


        /// <summary>
        /// Allows a credential provider to initiate events in the Logon UI or Credential UI through a callback interface.
        /// </summary>
        /// <param name="pcpe">An ICredentialProviderEvents callback interface to be used as the notification mechanism.</param>
        /// <param name="upAdviseContext">A pointer to an integer that uniquely identifies which credential provider has requested re-enumeration.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int Advise([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderEvents pcpe, IntPtr upAdviseContext);

        /// <summary>
        /// Used by the Logon UI or Credential UI to advise the credential provider that event callbacks are no longer accepted.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int UnAdvise();

        /// <summary>
        /// Retrieves the count of fields in the needed to display this provider's credentials.
        /// </summary>
        /// <param name="pdwCount">The field count.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetFieldDescriptorCount(out uint pdwCount);

        /// <summary>
        /// Gets metadata that describes a specified field.
        /// </summary>
        /// <param name="dwIndex">The zero-based index of the field for which the information should be retrieved.</param>
        /// <param name="ppcpfd">A pointer to a CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR structure which receives the information about the field.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetFieldDescriptorAt([In] uint dwIndex, out IntPtr ppcpfd);


        /// <summary>
        /// Gets the number of available credentials under this credential provider.
        /// </summary>
        /// <param name="pdwCount">A DWORD value that receives the count of credentials.</param>
        /// <param name="pdwDefault">A DWORD value that receives the index of the credential to be used as the default. If no default value has been set, this value should be set to CREDENTIAL_PROVIDER_NO_DEFAULT.</param>
        /// <param name="pbAutoLogonWithDefault">A BOOL value indicating if the default credential identified by pdwDefault should be used for an auto logon attempt. An auto logon attempt means the Logon UI or Credential UI will immediately call GetSerialization on the provider's default tile.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetCredentialCount(out uint pdwCount, out uint pdwDefault, out int pbAutoLogonWithDefault);


        /// <summary>
        /// Gets a specific credential.
        /// </summary>
        /// <param name="dwIndex">The zero-based index of the credential within the set of credentials enumerated for this credential provider.</param>
        /// <param name="ppcpc">An ICredentialProviderCredential instance representing the credential.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetCredentialAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out ICredentialProviderCredential ppcpc);
    }
}
