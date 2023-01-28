using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Extends the ICredentialProviderCredentialEvents interface by adding methods that enable batch updating of fields in theLogon UI or Credential UI.
    /// </summary>
    /// <remarks>
    /// <para>In Windows 7 and Windows Vista, many credential providers used ICredentialProviderEvents::CredentialsChanged to update UI. While this works, it causes a re-enumeration of all the credentials from the calling credential provider. The processing of this event can, under some circumstances, lead to flashing or focus changes in the UI due to this re-enumeration. Therefore, using ICredentialProviderEvents::CredentialsChanged solely for UI updates is discouraged. The new recommendation is as follows:</para>
    /// <para>
    /// Use ICredentialProviderEvents::CredentialsChanged only if a credential provider needs to do automatically logon a user or change the number of credentials it is enumerating.
    ///Use ICredentialProviderCredentialEvents2 to update a credential provider's UI.
    /// </para>
    /// <para>ICredentialProviderCredentialEvents2 includes all of the methods inherited from ICredentialProviderCredentialEvents.This includes all of the inherited methods except OnCreatingWindow.</para>
    /// <para>When interacting with a background thread, the use of ICredentialProviderCredentialEvents2 is similar to the use of ICredentialProviderCredentialEvents, in that proper inter-thread communication methods must be used.</para>
    /// </remarks>
    [Guid("B53C00B6-9922-4B78-B1F4-DDFE774DC39B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface ICredentialProviderCredentialEvents2 : ICredentialProviderCredentialEvents
    {
        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldState(ICredentialProviderCredential, uint, FieldState)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldState([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] FieldState cpfs);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldInteractiveState(ICredentialProviderCredential, uint, FieldInteractiveState)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldInteractiveState([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] FieldInteractiveState cpfis);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldString(ICredentialProviderCredential, uint, IntPtr)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldString([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] IntPtr psz);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldCheckbox(ICredentialProviderCredential, uint, int, string)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldCheckbox([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] int bChecked, [MarshalAs(UnmanagedType.LPWStr)][In] string pszLabel);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldBitmap(ICredentialProviderCredential, uint, IntPtr)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldBitmap([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] IntPtr hbmp);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldComboBoxSelectedItem(ICredentialProviderCredential, uint, uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldComboBoxSelectedItem([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwSelectedItem);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.DeleteFieldComboBoxItem(ICredentialProviderCredential, uint, uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int DeleteFieldComboBoxItem([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwItem);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.AppendFieldComboBoxItem(ICredentialProviderCredential, uint, string)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int AppendFieldComboBoxItem([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr)][In] string pszItem);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.SetFieldSubmitButton(ICredentialProviderCredential, uint, uint)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldSubmitButton([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwAdjacentTo);

        ///<inheritdoc cref="ICredentialProviderCredentialEvents.OnCreatingWindow(out IntPtr)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int OnCreatingWindow(out IntPtr phwndOwner);

        /// <summary>
        /// Starts a batch update to fields in the logon or credential UI.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int BeginFieldUpdates();

        /// <summary>
        /// Finishes and commits the batch updates started by BeginFieldUpdates.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int EndFieldUpdates();

        /// <summary>
        /// Specifies whether a specified field in the logon or credential UI should display a "password reveal" glyph or is expected to receive an e-mail address.
        /// </summary>
        /// <param name="credential">An ICredentialProviderCredential interface pointer to the credential object.</param>
        /// <param name="fieldID">The ID of the field in the logon or credential UI for which this option applies.</param>
        /// <param name="options">One or more of the CREDENTIAL_PROVIDER_CREDENTIAL_FIELD_OPTIONS values, which specify the field options.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldOptions([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential credential, [In] uint fieldID, [In] FieldOptions options);
    }
}
