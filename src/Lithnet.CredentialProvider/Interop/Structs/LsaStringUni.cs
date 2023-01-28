using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LsaStringUni : IDisposable
    {
        public ushort Length;
        public ushort MaxLength;
        public IntPtr Buffer;

        public LsaStringUni(string value)
        {
            this.Length = (ushort)(value.Length * sizeof(ushort));
            this.MaxLength = this.Length;
            this.Buffer = Marshal.StringToHGlobalUni(value);
        }

        public static implicit operator LsaStringUni(string value) => new LsaStringUni(value);
        public static implicit operator string(LsaStringUni value) => value.ToString();

        public override string ToString()
        {
            return Marshal.PtrToStringUni(this.Buffer) ?? string.Empty;
        }
        public void Dispose()
        {
            Marshal.FreeHGlobal(this.Buffer);
        }
    }
}
