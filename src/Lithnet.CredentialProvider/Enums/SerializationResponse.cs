namespace Lithnet.CredentialProvider
{
	public enum SerializationResponse
	{
		/// <summary>
		///No credential was serialized because more information is needed. One example of this would be if a credential requires both a PIN and an answer to a secret question, but the user has only provided the PIN. This signals the caller should be given a chance to alter its response.
		/// </summary>
		NoCredentialNotFinished,

		/// <summary>
		/// The credential provider has not serialized a credential but has completed its work. This response has multiple meanings. It can mean that no credential was serialized and that the user should not try again. This response can also mean that no credential was submitted but the credential's work is complete. For example, in the Change Password scenario, this response implies success.
        /// </summary>
		NoCredentialFinished,

		/// <summary>
		/// A credential was serialized. This response implies that a serialization structure was passed back.
		/// </summary>
		ReturnCredentialFinished,

		/// <summary>
		/// The credential provider has not serialized a credential, but has completed its work. The difference between this value and <see cref="NoCredentialNotFinished"/> is that this flag will force the logon UI to return, which will call UnAdvise for all the credential providers.
		/// </summary>
		ReturnNoCredentialFinished
	}
}
