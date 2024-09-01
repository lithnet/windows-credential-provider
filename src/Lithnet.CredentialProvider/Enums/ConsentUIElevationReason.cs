namespace Lithnet.CredentialProvider
{
    public enum ConsentUIElevationReason
    {
        /// <summary>
        /// Application Compatibility
        /// e.g. "Run this program as an Administrator" explicitly configured
        ///
        /// > "The AppCompat database stores information in the application
        /// >  compatibility fix entries for an application."
        /// </summary>
        AppCompatExplicit = 0,

        /// <summary>
        /// Application Compatibility
        /// e.g. "Run this program as an Administrator" set via Windows heuristics
        ///
        /// > "The AppCompat database stores information in the application
        /// >  compatibility fix entries for an application."
        /// </summary>
        AppCompatHeuristic = 1,

        /// <summary>
        /// Application manifest
        /// > "The Fusion database stores information from application
        /// >  manifests that describe the applications. The manifest schema
        /// >  is updated to add a new requested execution level field."
        ///
        /// See also: https://learn.microsoft.com/en-us/windows/win32/sbscs/application-manifests#trustinfo
        /// </summary>
        Fusion = 2,

        /// <summary>
        /// Automatically detected Windows Installer package (e.g. an EXE installer)
        /// > "Installer detection detects setup files, which helps prevent installations
        /// >  from being run without the user's knowledge and consent."
        /// </summary>
        Installer = 3,

        /// <summary>
        /// COM elevation action
        /// </summary>
        CLSID = 4,

        /// <summary>
        /// Windows Installer package installation (MSI)
        /// </summary>
        Msi = 5,

        /// <summary>
        /// "Run as Administrator..." (e.g. manual UAC)
        /// </summary>
        Request = 6,
        
        /// <summary>
        /// ActiveX Installer Service (AXIS)
        /// </summary>
        AxIS = 7,
        
        /// <summary>
        /// Packaged Applications (MSIX / APPX)
        /// </summary>
        PackagedApp = 8,
        
        /// <summary>
        /// Unknown
        /// </summary>
        NumReasons = 9,
    }
}