using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Extends the ICredentialProviderCredential interface by adding a method that retrieves the security identifier (SID) of a user. The credential is associated with that user and can be grouped under the user's tile.
    /// </summary>
    /// <remarks><para>This class is required for creating a V2 credential provider. V2 credential providers provide a personalized log on experience for the user. This occurs by the credential provider telling the Logon UI what sign in options are available for a user. It is recommended that new credential providers should be V2 credential providers.</para>
    /// <para>In order to create an ICredentialProviderCredential2 instance, a valid SID needs to be returned by the GetUserSid function.Valid is defined by the returned SID being for one of the users currently enumerated by the Logon UI.</para>
    /// <para>A useful tool for getting the available users and determining which ones you want to associate with is the ICredentialProviderUserArray object. This object contains a list of ICredentialProviderUser objects that can be queried to gain information about the users that will be enumerated.For example you could gain the user's SID or username using GetStringValue with a passed in parameter of PKEY_Identity_PrimarySid or PKEY_Identity_USerName respectively. You can even filter the results using SetProviderFilter to only display a subset of available users.</para>
    /// <para>Using the ICredentialProviderUserArray is optional, but it is a convenient way to get the necessary information to make valid SID values.In order to get a list of users that will be enumerated by the Logon UI, implement the ICredentialProviderSetUserArray interface to get the ICredentialProviderUserArray object from SetUserArray.Logon UI calls SetUserArray before GetCredentialCount, so the ICredentialProviderUserArray object is ready when a credential provider is about to return credentials.</para>
    /// <para>A V2 credential provider is represented by an icon displayed underneath the "Sign-in options" link.In order to provide an icon for your credential provider, define a CREDENTIAL_PROVIDER_FIELD_TYPE of CPFT_TILE_IMAGE in the credential itself.Then make sure the guidFieldType of the CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR is set to CPFG_CREDENTIAL_PROVIDER_LOGO.The recommended size for an icon is 72 by 72 pixels.</para>
    /// <para>Similar to specifying an icon for your credential provider, you can also specify a text string to identify your credential provider.This string appears in a pop-up window when a user hovers over the icon.To do this, define a CREDENTIAL_PROVIDER_FIELD_TYPE of CPFT_SMALL_TEXT in the credential itself.Then make sure the guidFieldType of the CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR is set to CPFG_CREDENTIAL_PROVIDER_LABEL.This string should supplement the credential provider icon described above and be descriptive enough that users understand what it is. For example, the picture password provider's description is "Picture Password".</para></remarks>
    [Guid("FD672C54-40EA-4D6E-9B49-CFB1A7507BD7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface ICredentialProviderCredential2 : ICredentialProviderCredential
    {
        /// <inheritdoc cref="ICredentialProviderCredential.Advise(ICredentialProviderCredentialEvents)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int Advise([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredentialEvents pcpce);

        /// <inheritdoc cref="ICredentialProviderCredential.UnAdvise"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int UnAdvise();

        /// <inheritdoc cref="ICredentialProviderCredential.SetSelected(out int)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetSelected(out int pbAutoLogon);

        /// <inheritdoc cref="ICredentialProviderCredential.SetDeselected"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetDeselected();

        /// <inheritdoc cref="ICredentialProviderCredential.GetFieldState(uint, out FieldState, out FieldInteractiveState)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetFieldState([In] uint dwFieldID, out FieldState pcpfs, out FieldInteractiveState pcpfis);

        /// <inheritdoc cref="ICredentialProviderCredential.GetStringValue(uint, out IntPtr)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetStringValue([In] uint dwFieldID, out IntPtr ppsz);

        /// <inheritdoc cref="ICredentialProviderCredential.GetBitmapValue(uint, out IntPtr)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetBitmapValue([In] uint dwFieldID, out IntPtr phbmp);

        /// <inheritdoc cref="ICredentialProviderCredential.GetCheckboxValue(uint, out int, out string)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetCheckboxValue([In] uint dwFieldID, out int pbChecked, [MarshalAs(UnmanagedType.LPWStr)] out string ppszLabel);

        /// <inheritdoc cref="ICredentialProviderCredential.GetSubmitButtonValue(uint, out uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetSubmitButtonValue([In] uint dwFieldID, out uint pdwAdjacentTo);

        /// <inheritdoc cref="ICredentialProviderCredential.GetComboBoxValueCount(uint, out uint, out uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetComboBoxValueCount([In] uint dwFieldID, out uint pcItems, out uint pdwSelectedItem);

        /// <inheritdoc cref="ICredentialProviderCredential.GetComboBoxValueAt(uint, uint, out string)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetComboBoxValueAt([In] uint dwFieldID, uint dwItem, [MarshalAs(UnmanagedType.LPWStr)] out string ppszItem);

        /// <inheritdoc cref="ICredentialProviderCredential.SetStringValue(uint, IntPtr)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetStringValue([In] uint dwFieldID, [In] IntPtr psz);

        /// <inheritdoc cref="ICredentialProviderCredential.SetCheckboxValue(uint, int)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetCheckboxValue([In] uint dwFieldID, [In] int bChecked);

        /// <inheritdoc cref="ICredentialProviderCredential.SetComboBoxSelectedValue(uint, uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetComboBoxSelectedValue([In] uint dwFieldID, [In] uint dwSelectedItem);

        /// <inheritdoc cref="ICredentialProviderCredential.CommandLinkClicked(uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int CommandLinkClicked([In] uint dwFieldID);

        /// <inheritdoc cref="ICredentialProviderCredential.GetSerialization(out SerializationResponse, out CredentialSerialization, out string, out StatusIcon)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetSerialization(out SerializationResponse pcpgsr, out CredentialSerialization pcpcs, [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon);

        /// <inheritdoc cref="ICredentialProviderCredential.ReportResult(int, int, out string, out StatusIcon)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int ReportResult([In] int ntsStatus, [In] int ntsSubstatus, [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon);

        /// <summary>
        /// Retrieves the security identifier (SID) of the user that is associated with this credential.
        /// </summary>
        /// <param name="sid">The address of a pointer to a buffer that, when this method returns successfully, receives the user's SID.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <remarks>The Logon UI will use the returned SID from this method to associate the credential tile with a user tile. To associate the credential with the "Other user" user tile in the Logon UI, this method should return S_FALSE and a null SID. The "Other user" tile is normally only valid when the PC is joined to a domain.</remarks>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetUserSid([MarshalAs(UnmanagedType.LPWStr)] out string sid);
    }
}
