using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Specifies the Windows Installer action described by Consent UI data.
    /// </summary>
    public enum ConsentUIMsiAction : uint
    {
        /// <summary>
        /// Installs a Windows Installer package.
        /// </summary>
        Install = 0,

        /// <summary>
        /// Uninstalls a Windows Installer package.
        /// </summary>
        Uninstall = 1,

        /// <summary>
        /// Updates or repairs an installed Windows Installer package.
        /// </summary>
        Update = 2
    }
}
