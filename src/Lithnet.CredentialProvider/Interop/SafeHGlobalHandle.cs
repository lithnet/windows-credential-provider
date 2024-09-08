using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    internal sealed class SafeHGlobalHandle : IDisposable
    {
        /// <summary>
        /// Unmanaged pointer wrapped by this object
        /// </summary>
        IntPtr pointer;

        int size;

        SafeHGlobalHandle()
        {
            this.pointer = IntPtr.Zero;
        }

        SafeHGlobalHandle(IntPtr handle)
        {
            this.pointer = handle;
        }

        ~SafeHGlobalHandle()
        {
            this.Dispose();
        }

        public int Size => size;

        public static SafeHGlobalHandle InvalidHandle => new SafeHGlobalHandle(IntPtr.Zero);

        /// <summary>
        /// Operator to obtain the unmanaged pointer wrapped by the object. Note
        /// that the returned pointer is only valid for the lifetime of this
        /// object.
        /// </summary>
        /// <returns>Unmanaged pointer wrapped by the object</returns>
        public IntPtr ToIntPtr()
        {
            return this.pointer;
        }

        public void Dispose()
        {
            if (this.pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.pointer);
                this.pointer = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

        public static SafeHGlobalHandle AllocHGlobal(int cb)
        {
            if (cb < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cb), "The value of this argument must be non-negative");
            }

            SafeHGlobalHandle result = new SafeHGlobalHandle();
            result.pointer = Marshal.AllocHGlobal(cb);
            result.size = cb;
            return result;
        }
    }
}