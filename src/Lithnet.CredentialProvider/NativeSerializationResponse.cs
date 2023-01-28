namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// An object that represents a response to LogonUI/CredUI that includes serialized credentials, or the result of the attempt to obtain them.
    /// </summary>
    public class NativeSerializationResponse
    {
        /// <summary>
        /// Gets or sets the final HRESULT of the serialization operation
        /// </summary>
        public int HResult { get; set; }

        /// <summary>
        /// Gets or sets the status result of the serialization operation
        /// </summary>
        public SerializationResponse SerializationResponse { get; set; }

        /// <summary>
        /// Gets or sets the serialzied credential blob
        /// </summary>
        public CredentialSerialization SerializedCredentials { get; set; }

        /// <summary>
        /// Gets or sets the optional status text to display to the user
        /// </summary>
        public string OptionalStatusText { get; set; }

        /// <summary>
        /// Gets or sets the optional status icon to display to the user
        /// </summary>
        public StatusIcon OptionalStatusIcon { get; set; }
    }
}
