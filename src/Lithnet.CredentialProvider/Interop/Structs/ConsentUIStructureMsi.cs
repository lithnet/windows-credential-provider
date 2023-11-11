using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureMsi
    {
        public ConsentUIStructureHeader Header;

        // 64 

        public IntPtr hUnknown1; // 8 
        public IntPtr oProductName; // 8

        // 64 + 16 == 80

        public IntPtr oVersion; // 8
        public IntPtr oLocale; // 8

        // 80 + 16 == 96

        public IntPtr oPublisher; // 8
        public IntPtr oExecutionPath; // 8

        // 96 + 16 == 112

        public IntPtr oOriginalMsi; // 8
        public IntPtr hUnknown2; // 8

        // 96 + 16 == 128

        public IntPtr oUnknown1; // 8
        public IntPtr oUnknown2; // 8

        // 128 + 16 == 144
    }
}
