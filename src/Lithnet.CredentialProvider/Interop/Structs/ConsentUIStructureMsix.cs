using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureMsix
    {
        public ConsentUIStructureHeader Header;

        // 64 

        public IntPtr oExecutablePath; // 8 
        public IntPtr oCommandLine; // 8

        // 64 + 16 == 80

        public IntPtr oPackageName; // 8
        public IntPtr oOtherName; // 8

        // 80 + 16 == 96

        public int ProcessId; // 4
        // Padding (x64) // 4

        // 96 + 4 + 4 = 104
    }
}
