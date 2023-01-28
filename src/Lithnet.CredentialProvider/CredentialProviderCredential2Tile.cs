using System;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a 'v2' user credential tile that implements the functionality of <see cref="CredentialProviderCredential1Tile"/>, and includes support for personalized tile, where a single user tile is shown, with multiple logon options grouped within it. 
    /// </summary>
    /// <remarks>Inheriting from this class enables you to provide a v2 credential tile. V2 credential tiles were introduced in Windows 8. See the Microsoft documentation on ICredentialProviderCredential2 for more information</remarks>
    /// <inheritdoc/>
    public abstract class CredentialProviderCredential2Tile : CredentialProviderCredential1Tile, ICredentialProviderCredential2
    {
        public CredentialProviderUser User { get; }

        public override bool IsGenericTile => this.User == null;

        public GenericTileDisplayMode GenericTileDisplayMode { get; set; }

        protected CredentialProviderCredential2Tile(CredentialProviderBase credentialProvider) : this(credentialProvider, null) { }

        protected CredentialProviderCredential2Tile(CredentialProviderBase credentialProvider, CredentialProviderUser user) : base(credentialProvider)
        {
            this.User = user;
        }

        int ICredentialProviderCredential2.GetUserSid(out string sid)
        {
            sid = null;

            try
            {
                this.logger.LogTrace("GetUserSid");

                if (this.IsGenericTile)
                {
                    this.logger.LogTrace("GetUserSid: Tile is generic so returning no SID");
                    return this.GenericTileDisplayMode == GenericTileDisplayMode.DisplayUnderOtherUser ? HRESULT.S_FALSE : HRESULT.E_NOTIMPL;
                }

                return this.User.User.GetSid(out sid);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetUserSid failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProviderCredential2.SetSelected(out int pbAutoLogon)
        {
            return ((ICredentialProviderCredential)this).SetSelected(out pbAutoLogon);
        }

        int ICredentialProviderCredential2.SetDeselected()
        {
            return ((ICredentialProviderCredential)this).SetDeselected();
        }

        int ICredentialProviderCredential2.GetFieldState(uint dwFieldID, out FieldState pcpfs, out FieldInteractiveState pcpfis)
        {
            return ((ICredentialProviderCredential)this).GetFieldState(dwFieldID, out pcpfs, out pcpfis);
        }

        int ICredentialProviderCredential2.GetStringValue(uint dwFieldID, out IntPtr ppsz)
        {
            return ((ICredentialProviderCredential)this).GetStringValue(dwFieldID, out ppsz);
        }

        int ICredentialProviderCredential2.GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            return ((ICredentialProviderCredential)this).GetBitmapValue(dwFieldID, out phbmp);
        }

        int ICredentialProviderCredential2.GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            return ((ICredentialProviderCredential)this).GetCheckboxValue(dwFieldID, out pbChecked, out ppszLabel);
        }

        int ICredentialProviderCredential2.GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            return ((ICredentialProviderCredential)this).GetSubmitButtonValue(dwFieldID, out pdwAdjacentTo);
        }

        int ICredentialProviderCredential2.GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            return ((ICredentialProviderCredential)this).GetComboBoxValueCount(dwFieldID, out pcItems, out pdwSelectedItem);
        }

        int ICredentialProviderCredential2.GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            return ((ICredentialProviderCredential)this).GetComboBoxValueAt(dwFieldID, dwItem, out ppszItem);
        }

        int ICredentialProviderCredential2.SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            return ((ICredentialProviderCredential)this).SetCheckboxValue(dwFieldID, bChecked);
        }

        int ICredentialProviderCredential2.SetStringValue(uint dwFieldID, IntPtr psz)
        {
            return ((ICredentialProviderCredential)this).SetStringValue(dwFieldID, psz);
        }

        int ICredentialProviderCredential2.CommandLinkClicked(uint dwFieldID)
        {
            return ((ICredentialProviderCredential)this).CommandLinkClicked(dwFieldID);
        }

        int ICredentialProviderCredential2.SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            return ((ICredentialProviderCredential)this).SetComboBoxSelectedValue(dwFieldID, dwSelectedItem);
        }

        int ICredentialProviderCredential2.GetSerialization(out SerializationResponse pcpgsr, out CredentialSerialization pcpcs, out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon)
        {
            return ((ICredentialProviderCredential)this).GetSerialization(out pcpgsr, out pcpcs, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }

        int ICredentialProviderCredential2.UnAdvise()
        {
            return ((ICredentialProviderCredential)this).UnAdvise();
        }

        int ICredentialProviderCredential2.Advise(ICredentialProviderCredentialEvents pcpce)
        {
            return ((ICredentialProviderCredential)this).Advise(pcpce);
        }

        int ICredentialProviderCredential2.ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon)
        {
            return ((ICredentialProviderCredential)this).ReportResult(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }
    }
}
