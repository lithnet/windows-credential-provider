namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A base class used to provide credentials for serialization
    /// </summary>
    public abstract class CredentialResponseBase
    {
        /// <summary>
        /// Gets or sets a value indicating if the credentials were sucessfully obtained
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// An optional status icon
        /// </summary>
        public StatusIcon StatusIcon { get; set; }

        /// <summary>
        /// An optional status message describing any errors that occurred
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// The domain of the user who's credentials are to be serialized
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// The username of the user who's credentials are to be serialized
        /// </summary>
        public string Username { get; set; }
    }
}
