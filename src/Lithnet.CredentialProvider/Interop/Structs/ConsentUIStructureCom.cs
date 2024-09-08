using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureCom
    {
        // 64 

        public IntPtr oOperationType; // 8 
        public IntPtr oComComponentPath; // 8

        // 64 + 16 == 80

        public IntPtr oImageResourcePath; // 8
        public IntPtr oProcessPath; // 8

        // 80 + 16 == 96

        public Guid Clsid; // 16

        // 96 + 16 == 112
    }
}
