using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureHeaderV1
    {
        public ConsentUIStructureHeaderBase BaseHeader;

        public IntPtr pReturnAddress; // 8

        // 64
    }
}
