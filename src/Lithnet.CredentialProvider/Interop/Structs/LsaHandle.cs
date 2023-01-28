using System;
using System.ComponentModel;

namespace Lithnet.CredentialProvider.Interop
{
    internal struct LsaHandle : IDisposable
    {
        public IntPtr Handle;

        public LsaHandle(IntPtr handle)
        {
            this.Handle = handle;
        }

        public static implicit operator LsaHandle(IntPtr handle) => new LsaHandle(handle);
        public static implicit operator IntPtr(LsaHandle handle) => handle.Handle;

        public static LsaHandle ConnectUntrusted()
        {
            var result = PInvoke.LsaConnectUntrusted(out var handle);

            if (result != 0)
            {
                throw new Win32Exception((int)result);
            }

            return handle;
        }

        public bool IsValid()
        {
            return this.Handle != IntPtr.Zero;
        }

        public void Dispose()
        {
            PInvoke.LsaDeregisterLogonProcess(this.Handle);
        }
    }
}
