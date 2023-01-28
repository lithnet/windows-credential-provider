using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct PropertyValue
    {
        public ushort vt;

        public byte wReserved1;

        public byte wReserved2;

        public uint wReserved3;

        public InnerPropertyValue Value;
    }
}
