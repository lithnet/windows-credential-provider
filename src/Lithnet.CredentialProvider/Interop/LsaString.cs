using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LsaString : IDisposable
    {
        public ushort Length;
        public ushort MaxLength;
        public IntPtr Buffer;

        public LsaString(string value)
        {
            this.Length = (ushort)value.Length;
            this.MaxLength = this.Length;
            this.Buffer = Marshal.StringToHGlobalAnsi(value);
        }

        public static implicit operator LsaString(string value) => new LsaString(value);
        public static implicit operator string(LsaString value) => value.ToString();

        public override string ToString()
        {
            return Marshal.PtrToStringAnsi(this.Buffer) ?? string.Empty;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(this.Buffer);
        }
    }
}
