using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureExe
    {
        public ConsentUIStructureHeader Header;

        // 64 

        public IntPtr hFile; // 8 
        public IntPtr oExecutablePath1; // 8

        // 64 + 16 = 80

        public IntPtr oExecutablePath2; // 8
        public IntPtr oCommandLine; // 8

        // 80 + 16 == 96

        public IntPtr oUnknown0; // 8
        public int ProcessId; // 4
        // Padding (x64)  // 4

        // 96 + 12 (+ 4) = 112
    }
}
