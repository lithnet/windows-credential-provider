namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A representation of a set of credentials to serialize back to LogonUI/CredUI. Consider using the <see cref="CredentialResponseSecure"/> class, which uses a SecureString for the password.
    /// </summary>
    public class CredentialResponseInsecure : CredentialResponseBase
    {
        /// <summary>
        /// The plain-text password of the user who's credentials are to be serialized
        /// </summary>
        public string Password { get; set; }
    }
}
