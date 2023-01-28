using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Provides an asynchronous callback mechanism used by a credential to notify it of state or text change events in the Logon UI or Credential UI.
    /// </summary>
	[Guid("FA6FA76B-66B7-4B11-95F1-86171118E816")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface ICredentialProviderCredentialEvents
    {
        /// <summary>
        /// Communicates to the Logon UI or Credential UI that a field state has changed and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing a field whose interactivity state is being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the field.</param>
        /// <param name="cpfs">The value from the CREDENTIAL_PROVIDER_FIELD_STATE enumeration that specifies the new field state.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldState([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] FieldState cpfs);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that the interactivity state of a field has changed and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing a field whose interactivity state is being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the field.</param>
        /// <param name="cpfis"></param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldInteractiveState([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] FieldInteractiveState cpfis);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that the string associated with a field has changed and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing a field whose interactivity state is being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the field.</param>
        /// <param name="psz">A pointer to the new string for the field.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldString([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] IntPtr psz);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that a checkbox field has changed and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing the checkbox field that is being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique field ID for the checkbox.</param>
        /// <param name="bChecked">The new state of the checkbox. TRUE indicates the checkbox should be checked, FALSE indicates it should not.</param>
        /// <param name="pszLabel">The new string for the checkbox label.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldCheckbox([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] int bChecked, [MarshalAs(UnmanagedType.LPWStr)][In] string pszLabel);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that a tile image field has changed and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing the tile image field that is being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the tile image field.</param>
        /// <param name="hbmp">The new tile image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldBitmap([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] IntPtr hbmp);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that the selected item in a combo box has changed and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing the combo box being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the combo box.</param>
        /// <param name="dwSelectedItem">The index of the item to select in the combo box.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldComboBoxSelectedItem([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwSelectedItem);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that an item should be deleted from a combo box and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing the combo box that needs to be updated. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the combo box.</param>
        /// <param name="dwItem">The index of the item that is deleted.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int DeleteFieldComboBoxItem([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwItem);

        /// <summary>
        /// Communicates to the Logon UI or Credential UI that a combo box needs an item appended and that the UI should be updated.
        /// </summary>
        /// <param name="pcpc">The credential containing the combo box that needs an item added. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the combo box.</param>
        /// <param name="pszItem">The string that will be appended to the combo box as a new option.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int AppendFieldComboBoxItem([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr)][In] string pszItem);

        /// <summary>
        /// Enables credentials to set the field that the submit button appears adjacent to.
        /// </summary>
        /// <param name="pcpc">The credential containing a field whose interactivity state is being set. This value should be set to this. See ICredentialProviderCredentialEvents for more information.</param>
        /// <param name="dwFieldID">The unique ID of the field.</param>
        /// <param name="dwAdjacentTo">The unique field ID of the field that the submit button should be adjacent to when this method completes.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetFieldSubmitButton([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwAdjacentTo);

        /// <summary>
        /// Called when the window is created. Enables credentials to retrieve the HWND of the parent window after Advise is called.
        /// </summary>
        /// <param name="phwndOwner">A pointer to the handle of the parent window.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int OnCreatingWindow(out IntPtr phwndOwner);
    }
}
