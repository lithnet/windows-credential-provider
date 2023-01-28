namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Declares the scenarios in which a credential provider is supported. A credential provider usage scenario (CPUS) enables the credential provider to provide distinct enumeration behavior and UI field setup across scenarios.
    /// </summary>
	public enum UsageScenario
    {
        /// <summary>
        /// No usage scenario has been set for the Credential Provider
        /// </summary>
		Invalid,

        /// <summary>
        /// Workstation logon or unlock. Credential providers that implement this scenario should be prepared to serialize credentials to the local authority for authentication. Starting in Windows 10, the CPUS_LOGON and CPUS_UNLOCK_WORKSTATION user scenarios have been combined.
        /// </summary>
		Logon,

        /// <summary>
        /// Workstation unlock. Credential providers that implement this scenario should be prepared to serialize credentials to the local authority for authentication. These credential providers also need to enumerate the currently logged-in user as the default tile.
        /// </summary>
        /// <remarks> Starting in Windows 10, the CPUS_LOGON and CPUS_UNLOCK_WORKSTATION user scenarios have been combined. This enables the system to support multiple users logging into a machine without creating and switching sessions unnecessarily. Any user on the machine can log into it once it has been locked without needing to back out of a current session and create a new one. Because of this, CPUS_LOGON can be used both for logging onto a system or when a workstation is unlocked. However, CPUS_LOGON cannot be used in all cases. Because of policy restrictions imposed by various systems, sometimes it is necessary for the user scenario to be CPUS_UNLOCK_WORKSTATION. Your credential provider should be robust enough to create the appropriate credential structure based on the scenario given to it. Windows will request the appropriate user scenario based on the situation. Some of the factors that impact whether or not a CPUS_UNLOCK_WORKSTATION scenario must be used include the following. Note that this is just a subset of possibilities.
        /// - The operating system of the device.
        /// - Whether this is a console or remote session.
        /// - Group policies such as hiding entry points for fast user switching, or interactive logon that does not display the user's last name.
        /// </remarks>
		UnlockWorkstation,

        /// <summary>
        /// Password change. This enables a credential provider to enumerate tiles in response to a user's request to change the password. Do not implement this scenario if you do not require some secret information from the user such as a password or PIN. These credential providers also need to enumerate the currently logged-in user as the default tile.
        /// </summary>
        ChangePassword,

        /// <summary>
        /// Credential UI. This scenario enables you to use credentials serialized by the credential provider to be used as authentication on remote machines. This is also the scenario used for over-the-shoulder prompting in User Access Control. This scenario uses a different instance of the credential provider than the one used for <c ref="Logon"/>, <c ref="UnlockWorkstation"/>, and <c ref="ChangePassword"/>, so the state of the credential provider cannot be maintained across the different scenarios.
        /// </summary>
		CredUI,

        /// <summary>
        /// Pre-Logon-Access Provider.
        /// </summary>
		PLAP
    }
}
