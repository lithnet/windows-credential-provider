namespace Lithnet.CredentialProvider
{
    public enum ConsentUIElevationType
    {
        Unknown = 0,

        /// <summary>
        /// Automatic Admin Mode.
        /// This seems to be an instance where UAC creates a local, secondary
        /// account called '%username%_admin' which is used to elevate a process.
        /// </summary>
        AutomaticAdmin = 1,

        /// <summary>
        /// Prompt the user for consent (i.e. Yes or No)
        /// </summary>
        Consent = 2,

        /// <summary>
        /// Prompt the user for credentials
        /// </summary>
        Credentials = 3
    }
}