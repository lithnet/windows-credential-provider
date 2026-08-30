using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal sealed class ComInterfacePointer : IDisposable
    {
        private IntPtr value;

        private ComInterfacePointer(IntPtr value)
        {
            this.value = value;
        }

        public IntPtr Value
        {
            get
            {
                if (this.value == IntPtr.Zero)
                {
                    throw new ObjectDisposedException(nameof(ComInterfacePointer));
                }

                return this.value;
            }
        }

        public static ComInterfacePointer Create(object instance, Guid interfaceId)
        {
            IntPtr unknown = Marshal.GetIUnknownForObject(instance);

            try
            {
                int hresult = ComMarshal.QueryInterface(unknown, interfaceId, out IntPtr interfacePointer);
                if (hresult != CredentialProviderAbi.S_OK)
                {
                    if (interfacePointer != IntPtr.Zero)
                    {
                        Marshal.Release(interfacePointer);
                    }

                    throw new COMException("The COM interface was not available", hresult);
                }

                return new ComInterfacePointer(interfacePointer);
            }
            finally
            {
                Marshal.Release(unknown);
            }
        }

        public static ComInterfacePointer TakeOwnership(IntPtr value)
        {
            if (value == IntPtr.Zero)
            {
                throw new ArgumentException("The COM interface pointer cannot be zero", nameof(value));
            }

            return new ComInterfacePointer(value);
        }

        public TDelegate GetMethod<TDelegate>(int slot) where TDelegate : class
        {
            IntPtr vtable = Marshal.ReadIntPtr(this.Value);
            IntPtr method = Marshal.ReadIntPtr(vtable, checked(slot * IntPtr.Size));
            return (TDelegate)(object)Marshal.GetDelegateForFunctionPointer(method, typeof(TDelegate));
        }

        public void Dispose()
        {
            if (this.value != IntPtr.Zero)
            {
                Marshal.Release(this.value);
                this.value = IntPtr.Zero;
            }
        }
    }
}
