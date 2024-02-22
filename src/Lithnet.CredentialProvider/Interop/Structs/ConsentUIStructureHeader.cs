using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ConsentUIStructureHeader
    {
        public int Size; // 4
        public ConsentUIType Type; // 4
        public ConsentUIPromptType PromptType; // 4
        // padding on x64 - 4

        // 16 

        public IntPtr hWindow; // 8
        public IntPtr hToken; // 8

        // 32

        public ConsentUIElevationType ElevationType; // 4
        public int sessionId; // 4
        public IntPtr hMutex; // 8

        // 48+

        public ConsentUIFlags Flags; // 4
        public int unknown0; // 4 
        public IntPtr pReturnAddress; // 8

        // 64
    }
}
