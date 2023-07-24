using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    public partial class CredentialTile : ICredentialProviderCredential
    {
        int ICredentialProviderCredential.Advise(ICredentialProviderCredentialEvents pcpce)
        {
            try
            {
                this.logger.LogTrace("Advise");

                if (pcpce != null)
                {
                    this.Controls.AssignEvents(pcpce);
                    this.events = pcpce;
                    this.events2 = pcpce as ICredentialProviderCredentialEvents2;

                    var intPtr = Marshal.GetIUnknownForObject(pcpce);
                    Marshal.AddRef(intPtr);
                }

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Advise failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.UnAdvise()
        {
            try
            {
                this.logger.LogTrace("Unadvise");

                if (this.events != null)
                {
                    this.Controls.UnassignEvents();
                    var intPtr = Marshal.GetIUnknownForObject(this.events);
                    Marshal.Release(intPtr);
                    this.events = null;
                }

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "UnAdvise failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.SetSelected(out int pbAutoLogon)
        {
            pbAutoLogon = 0;

            try
            {
                this.logger.LogTrace("SetSelected");
                this.IsSelected = true;
                pbAutoLogon = this.IsAutoLogon ? 1 : 0;
                this.OnSelected();
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetSelected failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.SetDeselected()
        {
            try
            {
                this.logger.LogTrace("SetDeselected");
                this.IsSelected = false;
                this.OnDeselected();
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetDeselected failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetFieldState(uint dwFieldID, out FieldState pcpfs, out FieldInteractiveState pcpfis)
        {
            pcpfis = FieldInteractiveState.None;
            pcpfs = FieldState.Hidden;

            try
            {
                this.logger.LogTrace($"GetFieldState: field {dwFieldID}");

                if (this.Controls.TryGetControl(dwFieldID, out var instance))
                {
                    pcpfs = instance.State;
                    pcpfis = instance.InteractiveState;

                    this.logger.LogTrace($"GetFieldState on [{instance}] returning state {instance.State} and interactive state {instance.InteractiveState}");
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetFieldState failed to find a field match for {dwFieldID}");


                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetFieldState failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetStringValue(uint dwFieldID, out IntPtr ppsz)
        {
            ppsz = IntPtr.Zero;

            try
            {
                this.logger.LogTrace($"GetStringValue: field {dwFieldID}");

                if (this.Controls.TryGetControl(dwFieldID, out var instance))
                {
                    if (instance is TextboxControl t)
                    {
                        ppsz = Marshal.StringToCoTaskMemUni(t.Text);
                        return HRESULT.S_OK;
                    }

                    if (instance is SecurePasswordTextboxControl p)
                    {
                        if (p.Password == null || p.Password.Length == 0)
                        {
                            ppsz = IntPtr.Zero;
                        }
                        else
                        {
                            ppsz = Marshal.SecureStringToCoTaskMemUnicode(p.Password);
                            this.logger.LogWarningDebug($"0x{ppsz.ToString("X16")} - Put password for outbound GetStringValue");
                        }

                        return HRESULT.S_OK;
                    }

                    if (instance is InsecurePasswordTextboxControl i)
                    {
                        if (i.Password == null || i.Password.Length == 0)
                        {
                            ppsz = IntPtr.Zero;
                        }
                        else
                        {
                            ppsz = Marshal.StringToCoTaskMemUni(i.Password);
                            this.logger.LogWarningDebug($"0x{ppsz.ToString("X16")} - Put password for outbound GetStringValue");
                        }

                        return HRESULT.S_OK;
                    }

                    ppsz = Marshal.StringToCoTaskMemUni(instance.Label);
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetStringValue failed to find a field match for {dwFieldID}");

                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetStringValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            phbmp = IntPtr.Zero;

            try
            {
                this.logger.LogTrace($"GetBitmapValue: field {dwFieldID}");

                if (this.Controls.TryGetControl<BitmapControl>(dwFieldID, FieldType.TileImage, out var instance))
                {
                    phbmp = instance.GetHBitmap();
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetBitmapValue was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetBitmapValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            pbChecked = 0;
            ppszLabel = null;

            try
            {
                this.logger.LogTrace($"GetCheckboxValue: field {dwFieldID}");


                if (this.Controls.TryGetControl<CheckboxControl>(dwFieldID, FieldType.CheckBox, out var instance))
                {
                    ppszLabel = instance.Label;
                    pbChecked = instance.IsChecked ? 1 : 0;
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetCheckboxValue was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetCheckboxValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            pdwAdjacentTo = 0;

            try
            {
                this.logger.LogTrace($"GetSubmitButtonValue: field {dwFieldID}");

                if (this.Controls.TryGetControl<SubmitButtonControl>(dwFieldID, FieldType.Submit, out var instance))
                {
                    pdwAdjacentTo = instance.AdjacentToId;
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetSubmitButtonValue was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetSubmitButtonValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            pcItems = 0;
            pdwSelectedItem = 0;

            try
            {
                this.logger.LogTrace($"GetComboBoxValueCount: field {dwFieldID}");

                if (this.Controls.TryGetControl<ComboboxControl>(dwFieldID, FieldType.ComboBox, out var instance))
                {
                    pcItems = (uint)instance.ComboBoxItems.Count;
                    pdwSelectedItem = (uint)instance.SelectedItemIndex;
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetComboBoxValueCount was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetComboBoxValueCount failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            ppszItem = null;

            try
            {
                this.logger.LogTrace($"GetComboBoxValueAt: field {dwFieldID}");
                if (this.Controls.TryGetControl<ComboboxControl>(dwFieldID, FieldType.ComboBox, out var instance))
                {
                    if (dwItem < instance.ComboBoxItems.Count)
                    {
                        ppszItem = instance.ComboBoxItems[(int)dwItem];
                        return HRESULT.S_OK;
                    }
                    else
                    {
                        return HRESULT.E_FAIL;
                    }
                }

                this.logger.LogError($"GetComboBoxValueAt was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetComboBoxValueAt failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.SetStringValue(uint dwFieldID, IntPtr psz)
        {
            try
            {
                this.logger.LogWarningDebug($"0x{psz.ToString("X16")} - Incoming SetStringValue: field {dwFieldID}");

                if (this.Controls.TryGetControl(dwFieldID, out var instance))
                {
                    if (instance.Type == FieldType.EditText && instance is TextboxControl b)
                    {
                        b.SetTextInternal(Marshal.PtrToStringUni(psz));
                        return HRESULT.S_OK;
                    }

                    if (instance.Type == FieldType.PasswordText && instance is SecurePasswordTextboxControl p)
                    {
                        this.logger.LogWarningDebug($"0x{psz.ToString("X16")} - Incoming password in SetStringValue");
                        p.SetPasswordInternal(psz.IntPtrToSecureString());
                        PInvoke.SecureZeroMemory(psz, (uint)(psz.Wcslen() * 2));
                        return HRESULT.S_OK;
                    }

                    if (instance.Type == FieldType.PasswordText && instance is InsecurePasswordTextboxControl i)
                    {
                        this.logger.LogWarningDebug($"0x{psz.ToString("X16")} - Incoming password in SetStringValue");
                        i.SetPasswordInternal(Marshal.PtrToStringUni(psz));
                        PInvoke.SecureZeroMemory(psz, (uint)(psz.Wcslen() * 2));
                        return HRESULT.S_OK;
                    }
                }

                this.logger.LogError($"SetStringValue was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetStringValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            try
            {
                this.logger.LogTrace($"SetCheckboxValue: field {dwFieldID}");

                if (this.Controls.TryGetControl<CheckboxControl>(dwFieldID, FieldType.CheckBox, out var instance))
                {
                    instance.SetIsCheckedInternal(bChecked != 0);
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"SetCheckboxValue was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetCheckboxValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            try
            {
                this.logger.LogTrace($"SetComboBoxSelectedValue: field {dwFieldID}");

                if (this.Controls.TryGetControl<ComboboxControl>(dwFieldID, FieldType.ComboBox, out var instance))
                {
                    instance.SetComboboxSelectedItemIndexInternal((unchecked((int)dwSelectedItem)));
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"SetComboBoxSelectedValue was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetComboBoxSelectedValue failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.CommandLinkClicked(uint dwFieldID)
        {
            try
            {
                this.logger.LogTrace($"CommandLinkClicked: field {dwFieldID}");

                if (this.Controls.TryGetControl<CommandLinkControl>(dwFieldID, FieldType.CommandLink, out var instance))
                {
                    instance.OnClick?.Invoke();
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"CommandLinkClicked was incorrectly called on field {instance}");
                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CommandLinkClicked failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.GetSerialization(out SerializationResponse pcpgsr, out CredentialSerialization pcpcs, out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon)
        {
            pcpgsr = SerializationResponse.NoCredentialNotFinished;
            pcpcs = default;
            ppszOptionalStatusText = null;
            pcpsiOptionalStatusIcon = StatusIcon.None;

            try
            {
                this.logger.LogTrace($"GetSerialization called");
                NativeSerializationResponse response = this.OnGetSerialization();

                pcpgsr = response.SerializationResponse;
                pcpcs = response.SerializedCredentials;
                ppszOptionalStatusText = response.OptionalStatusText;
                pcpsiOptionalStatusIcon = response.OptionalStatusIcon;

                this.logger.LogTrace($"GetSerialization is returning {pcpgsr}, with auth provider {pcpcs.AuthenticationPackage}, status icon '{pcpsiOptionalStatusIcon}' and status text '{ppszOptionalStatusText}'");

                return response.HResult;
            }
            catch (Exception ex)
            {
                ppszOptionalStatusText = "Unexpected error";
                pcpsiOptionalStatusIcon = StatusIcon.Error;
                this.logger.LogError(ex, "GetSerialization failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential.ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon)
        {
            ppszOptionalStatusText = null;
            pcpsiOptionalStatusIcon = StatusIcon.None;

            try
            {
                this.logger.LogTrace($"ReportResult");

                this.OnLogonStatusReported(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);

                if (ppszOptionalStatusText == null && pcpsiOptionalStatusIcon == StatusIcon.None)
                {
                    pcpsiOptionalStatusIcon = 0;
                    return HRESULT.E_NOTIMPL;
                }

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "ReportResult failed");
                return HRESULT.E_FAIL;
            }
        }
    }
}
