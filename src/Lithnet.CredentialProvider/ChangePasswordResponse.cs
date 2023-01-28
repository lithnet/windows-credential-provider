namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// The <c ref="ChangePasswordResponse"/> object is used to communicate the results of a password change operation to LogonUI
    /// </summary>
    public class ChangePasswordResponse
    {
        /// <summary>
        /// Gets or sets a value indicating if the password was successfully changed
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Optional. Gets or sets the icon to use when displaying the status text
        /// </summary>
        public StatusIcon StatusIcon { get; set; }

        /// <summary>
        /// Options. Details about the result of the password change operation
        /// </summary>
        public string StatusText { get; set; }
    }
}
