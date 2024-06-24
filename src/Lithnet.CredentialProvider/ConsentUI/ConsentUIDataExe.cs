using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the data structure passed to consent UI when a user is trying to elevate an executable
    /// </summary>
    public class ConsentUIDataExe : ConsentUIData
    {
        /// <summary>
        /// A file handle pointing to the EXE in question
        /// </summary>
        private IntPtr hFile;

        /// <summary>
        /// The path to the process that the user has requested to be launched as an administrator
        /// </summary>
        public string ExecutablePath { get; }

        /// <summary>
        /// A currently unknown value. In most cases it seems to be the same as <see cref="ExecutablePath"/>
        /// </summary>
        public string Unknown1 { get; }

        /// <summary>
        /// The full command line, including arguments that will be used to launch the executable
        /// </summary>
        public string CommandLine { get; }

        /// <summary>
        /// A currently unknown parameter
        /// </summary>
        public string Unknown2 { get; }

        /// <summary>
        /// Gets a handle to the executable that the user has requested to be launched as an administrator
        /// </summary>
        /// <returns>A handle to the executable</returns>
        public SafeHandle GetExecutableHandle()
        {
            return DuplicateHandleFromInternal(this.hFile);
        }

        internal ConsentUIDataExe(IntPtr pData, int expectedSize) : base(pData, expectedSize)
        {
            if (this.header.Type != ConsentUIType.Exe)
            {
                throw new InvalidOperationException("The data structure is not of type EXE");
            }

            var s = Marshal.PtrToStructure<ConsentUIStructureExe>(pData);

            this.hFile = s.hFile;
            this.ExecutablePath = this.GetStringValueIfValid(pData, (int)s.oExecutablePath1);
            this.Unknown1 = this.GetStringValueIfValid(pData, (int)s.oExecutablePath2);
            this.CommandLine = this.GetStringValueIfValid(pData, (int)s.oCommandLine);
            this.Unknown2 = this.GetStringValueIfValid(pData, (int)s.oUnknown0);
        }
    }
}
