using System.Security;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A representation of a set of credentials to serialize back to LogonUI/CredUI.
    /// </summary>
    public class CredentialResponseSecure : CredentialResponseBase
    {
        /// <summary>
        /// The password of the user who's credentials are to be serialized
        /// </summary>
        public SecureString Password { get; set; }
    }
}
