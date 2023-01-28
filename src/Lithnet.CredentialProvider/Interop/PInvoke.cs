using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Lithnet.CredentialProvider.Interop
{
    internal static class PInvoke
    {
        [DllImport("secur32.dll", SetLastError = false)]
        public static extern uint LsaConnectUntrusted(out IntPtr LsaHandle);

        [DllImport("secur32.dll", SetLastError = false)]
        public static extern IntPtr LsaDeregisterLogonProcess([In] IntPtr handle);

        [DllImport("secur32.dll", SetLastError = false)]
        public static extern uint LsaLookupAuthenticationPackage(IntPtr lsaHandle, ref LsaString packageName, out uint authenticationPackage);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern uint LsaNtStatusToWinError(uint status);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetComputerName(StringBuilder buffer, ref uint size);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static string GetComputerName(uint bufferSize = 25)
        {
            var buffer = new StringBuilder((int)bufferSize);

            if (GetComputerName(buffer, ref bufferSize))
            {
                return buffer.ToString();
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static uint LookupAuthenticationPackage(LsaString packageName)
        {
            using (var handle = LsaHandle.ConnectUntrusted())
            {
                var result = LsaLookupAuthenticationPackage(handle, ref packageName, out var package);

                if (result != 0)
                {
                    throw new Win32Exception((int)result);
                }

                return package;
            }
        }

        [MethodImplAttribute(MethodImplOptions.NoOptimization)]
        public static unsafe void SecureZeroMemory(IntPtr ptr, uint length)
        {
            for (int i = 0; i < length; i++)
            {
                *((byte*)ptr + i) = 0;
            }
        }
    }
}
