using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Exposes methods that enable the handling of a credential.
    /// </summary>
    /// <remarks>
    ///  ICredentialProviderCredential is implemented by outside parties providing a Logon UI or Credential UI prompting for user credentials. Enumeration of user tiles cannot be done without an implementation of this interface. 
    ///  </remarks>
    [Guid("63913A93-40C1-481A-818D-4072FF8C70CC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface ICredentialProviderCredential
    {
        /// <summary>
        /// Enables a credential to initiate events in the Logon UI or Credential UI through a callback interface. This method should be called before other methods in <see cref="ICredentialProviderCredential"/> interface.
        /// </summary>
        /// <param name="pcpce">An  <see cref="ICredentialProviderCredentialEvents"/> callback interface to be used as the notification mechanism.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int Advise([MarshalAs(UnmanagedType.Interface)][In] ICredentialProviderCredentialEvents pcpce);

        /// <summary>
        /// Used by the Logon UI or Credential UI to advise the credential that event callbacks are no longer accepted.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int UnAdvise();

        /// <summary>
        /// Called when a credential is selected. Enables the implementer to set logon characteristics.
        /// </summary>
        /// <param name="pbAutoLogon">When this method returns, contains TRUE if selection of the credential indicates that it should attempt to logon immediately and automatically, otherwise FALSE. For example, a credential provider that enumerates an account without a password may want to return this as true.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetSelected(out int pbAutoLogon);

        /// <summary>
        /// Called when a credential loses selection.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetDeselected();

        /// <summary>
        /// Retrieves the field state. The Logon UI and Credential UI use this to gain information about a field of a credential to display this information in the user tile.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field.</param>
        /// <param name="pcpfs">The credential provider field state. This indicates when the field should be displayed on the user tile.</param>
        /// <param name="pcpfis">The credential provider field interactive state. This indicates when the user can interact with the field.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetFieldState([In] uint dwFieldID, out FieldState pcpfs, out FieldInteractiveState pcpfis);

        /// <summary>
        /// Enables retrieval of text from a credential with a text field.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field.</param>
        /// <param name="ppsz">A pointer to a null-terminated Unicode string to return to the Logon UI or Credential UI.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetStringValue([In] uint dwFieldID, out IntPtr ppsz);

        /// <summary>
        /// Enables retrieval of bitmap data from a credential with a bitmap field.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field.</param>
        /// <param name="phbmp">Contains a pointer to the handle of the bitmap.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetBitmapValue([In] uint dwFieldID, out IntPtr phbmp);

        /// <summary>
        /// Retrieves the checkbox value.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field.</param>
        /// <param name="pbChecked">Indicates the state of the checkbox. TRUE indicates the checkbox is checked, otherwise FALSE.</param>
        /// <param name="ppszLabel">Points to the label on the checkbox.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetCheckboxValue([In] uint dwFieldID, out int pbChecked, [MarshalAs(UnmanagedType.LPWStr)] out string ppszLabel);

        /// <summary>
        /// Retrieves the identifier of a field that the submit button should be placed next to in the Logon UI. The Credential UI does not call this method.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field a submit button value is needed for.</param>
        /// <param name="pdwAdjacentTo">The field ID of the field that the submit button should be placed next to.</param>
        /// <remarks>Note to implementers: Do not return the field ID of a bitmap in this parameter. It is not good UI design to place the submit button next to a bitmap, and doing so can cause a failure in the Logon UI.</remarks>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetSubmitButtonValue([In] uint dwFieldID, out uint pdwAdjacentTo);

        /// <summary>
        /// Gets a count of the items in the specified combo box and designates which item should have initial selection.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the combo box to gather information about.</param>
        /// <param name="pcItems">A pointer to the number of items in the given combo box.</param>
        /// <param name="pdwSelectedItem">Contains a pointer to the item that receives initial selection.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetComboBoxValueCount([In] uint dwFieldID, out uint pcItems, out uint pdwSelectedItem);

        /// <summary>
        /// Gets the string label for a combo box entry at the given index.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the combo box to query.</param>
        /// <param name="dwItem">The index of the desired item.</param>
        /// <param name="ppszItem">A string value that receives the combo box label.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetComboBoxValueAt([In] uint dwFieldID, uint dwItem, [MarshalAs(UnmanagedType.LPWStr)] out string ppszItem);

        /// <summary>
        /// Enables a Logon UI or Credential UI to update the text for a CPFT_EDIT_TEXT fields as the user types in them.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field that needs to be updated.</param>
        /// <param name="psz">A pointer to a buffer containing the new text.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetStringValue([In] uint dwFieldID, [In] IntPtr psz);

        /// <summary>
        /// Enables a Logon UI and Credential UI to indicate that a checkbox value has changed.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field to update.</param>
        /// <param name="bChecked">Indicates the new value for the checkbox. TRUE means the checkbox should be checked, FALSE means unchecked.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetCheckboxValue([In] uint dwFieldID, [In] int bChecked);

        /// <summary>
        /// Enables a Logon UI and Credential UI to indicate that a combo box value has been selected.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the combo box that is affected.</param>
        /// <param name="dwSelectedItem">The specific item selected.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetComboBoxSelectedValue([In] uint dwFieldID, [In] uint dwSelectedItem);

        /// <summary>
        /// Enables the Logon UI and Credential UI to indicate that a link was clicked.
        /// </summary>
        /// <param name="dwFieldID">The identifier for the field clicked on.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int CommandLinkClicked([In] uint dwFieldID);

        /// <summary>
        /// Called in response to an attempt to submit this credential to the underlying authentication engine.
        /// </summary>
        /// <param name="pcpgsr">Indicates the success or failure of the attempt to serialize credentials.</param>
        /// <param name="pcpcs">A pointer to the credential. Depending on the result, there may be no valid credential.</param>
        /// <param name="ppszOptionalStatusText">A pointer to a Unicode string value that will be displayed by the Logon UI after serialization. May be NULL.</param>
        /// <param name="pcpsiOptionalStatusIcon">A pointer to an icon that will be displayed by the credential after the call to GetSerialization returns. This value can be NULL.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetSerialization(out SerializationResponse pcpgsr, out CredentialSerialization pcpcs, [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon);

        /// <summary>
        /// Translates a received error status code into the appropriate user-readable message. The Credential UI does not call this method.
        /// </summary>
        /// <param name="ntsStatus">The NTSTATUS value that reflects the return value of the Winlogon call to LsaLogonUser.</param>
        /// <param name="ntsSubstatus">The NTSTATUS value that reflects the value pointed to by the SubStatus parameter of LsaLogonUser when that function returns after being called by Winlogon.</param>
        /// <param name="ppszOptionalStatusText">A pointer to the error message that will be displayed to the user. May be NULL.</param>
        /// <param name="pcpsiOptionalStatusIcon">A pointer to an icon that will shown on the credential. May be NULL.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int ReportResult([In] int ntsStatus, [In] int ntsSubstatus, [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon);
    }
}
