namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Specifies how Consent UI obtains approval for an elevation request.
    /// </summary>
    public enum ConsentUIPromptType
    {
        /// <summary>
        /// The prompt type is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Uses the Consent UI automatic administrator mode.
        /// </summary>
        AutomaticAdmin = 1,

        /// <summary>
        /// Requests consent from an administrator.
        /// </summary>
        Consent = 2,

        /// <summary>
        /// Requests administrator credentials.
        /// </summary>
        Credentials = 3
    }
}
