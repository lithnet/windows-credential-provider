using System;

namespace Lithnet.CredentialProvider
{
    [Flags]
    public enum ConsentUIFlags
    {
        SkipSignatureVerification = 0x01,

        /// <summary>
        /// Indicates to ConsentUI that it needs to switch to the Secure Desktop
        /// </summary>
        SecureDesktop = 0x02,

        Unknown1 = 0x04,
        Unknown2 = 0x08,
        Unknown3 = 0x10,

        /// <summary>
        /// This flag seems to cause ConsentUI to 
        /// skip all signature verification related code.
        /// </summary>
        SkipVerification = 0x20,

        /// <summary>
        /// Indicates that the executable file is contained within a Windows directory.
        /// As all the executables in System32, etc. are unsigned, ConsentUI
        /// uses this to toggle catalog verification, if required.
        /// </summary>
        InWindowsDirectory = 0x40,

        /// <summary>
        /// Seems to indicates to ConsentUI that automatic elevation should occur,
        /// and that the executable is in a safe Windows location
        /// </summary>
        AutoElevationWindows = 0x80,

        /// <summary>
        /// Like `AutoElevationWindows`, this seems indicates to ConsentUI that
        /// automatic elevation should occur, but that further verification
        /// inside ConsentUI should occur.
        /// </summary>
        AutoElevationOther = 0x100,

        Unknown4 = 0x200,

        /// <summary>
        /// ConsentUI uses this flag to determine if it should pass
        /// SIF_BASE_VERIFICATION | SIF_AUTHENTICODE_SIGNED to WTGetSignatureInfo
        /// </summary>
        PerformBaseVerification = 0x400,

        /// <summary>
        /// Indicates that the publisher is untrusted - this is what seems to trigger
        /// an AMSI scan (i.e., SmartScreen)
        /// </summary>
        UntrustedPublisher = 0x800,

        /// <summary>
        /// This flag seems to cause ConsentUI to skip all elevation-related code and exit.
        /// </summary>
        BlockElevation = 0x1000,

        /// <summary>
        /// Corresponds to `ConsentUIPromptType.AutomaticAdmin`
        /// This seems to be an instance where UAC creates a local, secondary
        /// account called '%username%_admin' which is used to elevate a process.
        /// </summary>
        AutomaticAdminMode = 0x2000
    }
}
