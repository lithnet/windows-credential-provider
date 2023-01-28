using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A native credential serialization structure used to pass logon information back to LogonUI/CredUI
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CredentialSerialization
    {
        /// <summary>
        /// The unique identifier of the authentication package. This parameter is required when calling LsaLogonUser. In a Credential UI scenario, this value is set before a serialization is sent through SetSerialization. This is the same as the authentication package value returned by LsaLookupAuthenticationPackage. Content providers can use this parameter to determine if they are able to return credentials for this authentication package. Developers who write their own authentication package may supply their own value.
        /// </summary>
        public uint AuthenticationPackage;

        /// <summary>
        /// The CLSID of the credential provider. Credential providers assign their own CLSID to this member during serialization. Credential UI ignores this member.
        /// </summary>
        public Guid ProviderClassGuid;

        /// <summary>
        /// The size, in bytes, of the credential pointed to by <see cref="SerializationData"/>.
        /// </summary>
        public uint SerializationSize;

        /// <summary>
        /// A pointer to an array of bytes containing serialized credential information. The exact format of this data depends on the authentication package targeted by a credential provider.
        /// </summary>
        public IntPtr SerializationData;
    }
}
