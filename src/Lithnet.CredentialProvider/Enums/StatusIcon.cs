namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Indicates which status icon should be displayed.
    /// </summary>
    /// <remarks>
    /// CREDENTIAL_PROVIDER_STATUS_ICON is not used starting in Windows 10.
    /// As part of ReportResult, a credential provider may specify a status icon to display.It is important to not that only Logon UI calls ReportResult. Credential UI does not.
    /// </remarks>
	public enum StatusIcon
    {
        /// <summary>
        /// No icon indicated.
        /// </summary>
		None,

        /// <summary>
        /// Display the error icon.
        /// </summary>
		Error,

        /// <summary>
        /// Display the warning icon.
        /// </summary>
		Warning,

        /// <summary>
        /// Reserved
        /// </summary>
		Success
    }
}
