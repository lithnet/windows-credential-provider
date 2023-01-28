using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    /// <summary>
    /// Provides a method that enables the credential provider framework to determine whether you've made a customization to a field's option in a logon or credential UI.
    /// </summary>
    /// <remarks>
    /// Implement this interface if your credential provider overrides the default field options through ICredentialProviderCredentialEvents2::SetFieldOptions. This enables the credential provider framework to determine the field options that you've specified . 
    /// </remarks>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("DBC6FB30-C843-49E3-A645-573E6F39446A")]
	[ComImport]
    internal interface ICredentialProviderCredentialWithFieldOptions
	{
        /// <summary>
        /// Retrieves the current option set for a specified field in a logon or credential UI. Called by the credential provider framework.
        /// </summary>
        /// <param name="fieldID">The ID of the field in the logon or credential UI.</param>
        /// <param name="options">A pointer to an CREDENTIAL_PROVIDER_CREDENTIAL_FIELD_OPTIONS value that, when this method returns successfully, receives one or more flags that specify the current options for the field.</param>
        /// <remarks>Provides a method that enables the credential provider framework to determine whether you've made a customization to a field's option in a logon or credential UI.</remarks>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int GetFieldOptions([In] uint fieldID, out FieldOptions options);
	}
}
