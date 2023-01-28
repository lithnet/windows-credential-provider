using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [Guid("2D8DEEB8-1322-4973-8DF9-B282F2468290")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface ICredentialProviderCredentialEvents3 : ICredentialProviderCredentialEvents2
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

        ///<inheritdoc cref="ICredentialProviderCredentialEvents2.BeginFieldUpdates"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int BeginFieldUpdates();

        ///<inheritdoc cref="ICredentialProviderCredentialEvents2.EndFieldUpdates"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int EndFieldUpdates();

        ///<inheritdoc cref="ICredentialProviderCredentialEvents2.SetFieldOptions(ICredentialProviderCredential, uint, FieldOptions)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetFieldOptions([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential credential, [In] uint fieldID, [In] FieldOptions options);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that a bitmap field should be updated
        /// </summary>
        /// <param name="pcpc">An ICredentialProviderCredential interface pointer to the credential object.</param>
        /// <param name="fieldID">The ID of the field to update</param>
        /// <param name="imageBufferSize">The image buffer size</param>
        /// <param name="pImageBuffer">A pointer to the image buffer</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        int SetFieldBitmapBuffer([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, uint fieldID, uint imageBufferSize, IntPtr pImageBuffer);
    }
}
