using System;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    internal abstract partial class CredentialTile3 : ICredentialProviderCredential3
    {
        int ICredentialProviderCredential3.GetBitmapBufferValue(uint dwFieldID, out uint pImageBufferSize, out IntPtr ppImageBuffer)
        {
            this.logger.LogTrace($"Called GetBitmapBufferValue {dwFieldID}");

            ppImageBuffer = IntPtr.Zero;
            pImageBufferSize = 0;

            try
            {
                this.logger.LogTrace($"GetBitmapBufferValue: field {dwFieldID}");

                if (this.Controls.TryGetControl<BitmapControl>(dwFieldID, FieldType.TileImage, out var instance))
                {
                    var hbitmap = instance.GetBitmapBuffer(out pImageBufferSize);
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

        int ICredentialProviderCredential3.GetUserSid(out string sid)
        {
            return ((ICredentialProviderCredential2)this).GetUserSid(out sid);
        }

        int ICredentialProviderCredential3.SetSelected(out int pbAutoLogon)
        {
            return ((ICredentialProviderCredential)this).SetSelected(out pbAutoLogon);
        }

        int ICredentialProviderCredential3.SetDeselected()
        {
            return ((ICredentialProviderCredential)this).SetDeselected();
        }

        int ICredentialProviderCredential3.GetFieldState(uint dwFieldID, out FieldState pcpfs, out FieldInteractiveState pcpfis)
        {
            return ((ICredentialProviderCredential)this).GetFieldState(dwFieldID, out pcpfs, out pcpfis);
        }

        int ICredentialProviderCredential3.GetStringValue(uint dwFieldID, out IntPtr ppsz)
        {
            return ((ICredentialProviderCredential)this).GetStringValue(dwFieldID, out ppsz);
        }

        int ICredentialProviderCredential3.GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            return ((ICredentialProviderCredential)this).GetBitmapValue(dwFieldID, out phbmp);
        }

        int ICredentialProviderCredential3.GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            return ((ICredentialProviderCredential)this).GetCheckboxValue(dwFieldID, out pbChecked, out ppszLabel);
        }

        int ICredentialProviderCredential3.GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            return ((ICredentialProviderCredential)this).GetSubmitButtonValue(dwFieldID, out pdwAdjacentTo);
        }

        int ICredentialProviderCredential3.GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            return ((ICredentialProviderCredential)this).GetComboBoxValueCount(dwFieldID, out pcItems, out pdwSelectedItem);
        }

        int ICredentialProviderCredential3.GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            return ((ICredentialProviderCredential)this).GetComboBoxValueAt(dwFieldID, dwItem, out ppszItem);
        }

        int ICredentialProviderCredential3.SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            return ((ICredentialProviderCredential)this).SetCheckboxValue(dwFieldID, bChecked);
        }

        int ICredentialProviderCredential3.SetStringValue(uint dwFieldID, IntPtr psz)
        {
            return ((ICredentialProviderCredential)this).SetStringValue(dwFieldID, psz);
        }

        int ICredentialProviderCredential3.CommandLinkClicked(uint dwFieldID)
        {
            return ((ICredentialProviderCredential)this).CommandLinkClicked(dwFieldID);
        }

        int ICredentialProviderCredential3.SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            return ((ICredentialProviderCredential)this).SetComboBoxSelectedValue(dwFieldID, dwSelectedItem);
        }

        int ICredentialProviderCredential3.GetSerialization(out SerializationResponse pcpgsr, out CredentialSerialization pcpcs, out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon)
        {
            return ((ICredentialProviderCredential)this).GetSerialization(out pcpgsr, out pcpcs, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }

        int ICredentialProviderCredential3.UnAdvise()
        {
            return ((ICredentialProviderCredential)this).UnAdvise();
        }

        int ICredentialProviderCredential3.Advise(ICredentialProviderCredentialEvents pcpce)
        {
            return ((ICredentialProviderCredential)this).Advise(pcpce);
        }

        int ICredentialProviderCredential3.ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon)
        {
            return ((ICredentialProviderCredential)this).ReportResult(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }
    }
}
