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
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_APPCOMPAT_EXPLICIT (ole32.dll)`
        /// </summary>
        AppCompatExplicit = 0,

        /// <summary>
        /// Application Compatibility
        /// e.g. "Run this program as an Administrator" set via Windows heuristics
        ///
        /// > "The AppCompat database stores information in the application
        /// >  compatibility fix entries for an application."
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_APPCOMPAT_HEURISTIC (ole32.dll)`
        /// </summary>
        AppCompatHeuristic = 1,

        /// <summary>
        /// Application manifest
        /// > "The Fusion database stores information from application
        /// >  manifests that describe the applications. The manifest schema
        /// >  is updated to add a new requested execution level field."
        /// See also: https://learn.microsoft.com/en-us/windows/win32/sbscs/application-manifests#trustinfo
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_FUSION (ole32.dll)`
        /// </summary>
        Fusion = 2,

        /// <summary>
        /// Automatically detected Windows Installer package (e.g. an EXE installer)
        /// > "Installer detection detects setup files, which helps prevent installations
        /// >  from being run without the user's knowledge and consent."
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_INSTALLER (ole32.dll)`
        /// </summary>
        Installer = 3,

        /// <summary>
        /// COM elevation action
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_CLSID (ole32.dll)`
        /// </summary>
        Clsid = 4,

        /// <summary>
        /// Windows Installer package installation (MSI)
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_MSI (ole32.dll)`
        /// </summary>
        Msi = 5,

        /// <summary>
        /// "Run as Administrator..." (e.g. manual UAC)
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_REQUEST (ole32.dll)`
        /// </summary>
        Request = 6,
        
        /// <summary>
        /// ActiveX Installer Service (AXIS)
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_AXIS (ole32.dll)`
        /// </summary>
        Axis = 7,
        
        /// <summary>
        /// Packaged Applications (MSIX / APPX)
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_PACKAGED_APP (ole32.dll)`
        /// </summary>
        Msix = 8,
        
        /// <summary>
        /// Unknown
        ///
        /// `ELEVATION_REASON.ELEVATION_REASON_NUM_REASONS (ole32.dll)`
        /// </summary>
        NumReasons = 9,
    }
}