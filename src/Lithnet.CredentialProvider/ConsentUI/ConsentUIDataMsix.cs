using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the data structure passed to consent UI when a user is trying to elevate an MSIX package
    /// </summary>
    public class ConsentUIDataMsix : ConsentUIData
    {
        /// <summary>
        /// The path to the package being installed
        /// </summary>
        public string ExecutablePath { get; }

        /// <summary>
        /// The name of the package being installed
        /// </summary>
        public string PackageName { get; }

        /// <summary>
        /// The full command line, including any arguments used to launch the installer
        /// </summary>
        public string CommandLine { get; }

        /// <summary>
        /// A currently unknown parameter
        /// </summary>
        public string OtherName { get; }

        internal ConsentUIDataMsix(IntPtr pData, int expectedSize) : base(pData, expectedSize)
        {
            if (this.header.Type != ConsentUIType.Msix)
            {
                throw new InvalidOperationException("The data structure is not of type MSIX");
            }

            var s = Marshal.PtrToStructure<ConsentUIStructureMsix>(pData + HeaderSize);

            this.ExecutablePath = this.GetStringValueIfValid(pData, (int)s.oExecutablePath);
            this.PackageName = this.GetStringValueIfValid(pData, (int)s.oPackageName);
            this.CommandLine = this.GetStringValueIfValid(pData, (int)s.oCommandLine);
            this.OtherName = this.GetStringValueIfValid(pData, (int)s.oOtherName);
        }
    }
}
