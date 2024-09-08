using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureHeaderV2
    {
        public ConsentUIStructureHeaderBase BaseHeader;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x81)]
        public byte[] unknown1;

        public IntPtr pReturnAddress; // 8
    }
}