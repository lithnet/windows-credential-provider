using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Exposes methods that enable the handling of a credential.
    /// </summary>
    /// <remarks>ICredentialProviderCredential is implemented by outside parties providing a Logon UI or Credential UI prompting for user credentials. Enumeration of user tiles cannot be done without an implementation of this interface. </remarks>
    [Guid("64A5010E-4363-41F8-9738-19045C20DABC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface ICredentialProviderCredential3 : ICredentialProviderCredential2
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

        /// <inheritdoc cref="ICredentialProviderCredential2.GetUserSid(out string)"/>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetUserSid([MarshalAs(UnmanagedType.LPWStr)] out string sid);

        /// <summary>
        /// Gets a buffer containing the image data
        /// </summary>
        /// <param name="fieldID">The field ID to get</param>
        /// <param name="pImageBufferSize">The size of the bitmap buffer</param>
        /// <param name="ppImageBuffer">A pointer to the bitmap buffer</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetBitmapBufferValue([In] uint fieldID, [Out] out uint pImageBufferSize, [Out] out IntPtr ppImageBuffer);
    }
}
