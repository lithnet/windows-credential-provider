using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Lithnet.CredentialProvider.Interop
{
    internal class CredentialSerializer
    {
        private readonly ILogger logger;

        public CredentialSerializer(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<CredentialSerializer>();
        }

        public CredentialSerialization GenerateCredentialSerialization(string domain, string username, SecureString password, bool isWorkstationUnlock, Guid providerId)
        {
            var authPackage = PInvoke.LookupAuthenticationPackage(CredProviderConstants.NEGOSSP_NAME_A);
            var pData = this.SerializeKerbLogon(domain, username, password, isWorkstationUnlock ? KerbLogonSubmitType.WorkstationUnlockLogon : KerbLogonSubmitType.InteractiveLogon, out int size);
            this.logger.LogWarningDebug($"0x{pData.ToString("X16")} - Serializer: Password got packed into ");

            return new CredentialSerialization()
            {
                AuthenticationPackage = authPackage,
                SerializationData = pData,
                ProviderClassGuid = providerId,
                SerializationSize = (uint)size
            };
        }

        public CredentialSerialization GenerateCredentialSerialization(string domain, string username, string password, bool isWorkstationUnlock, Guid providerId)
        {
            var authPackage = PInvoke.LookupAuthenticationPackage(CredProviderConstants.NEGOSSP_NAME_A);
            var pData = this.SerializeKerbLogon(domain, username, password, isWorkstationUnlock ? KerbLogonSubmitType.WorkstationUnlockLogon : KerbLogonSubmitType.InteractiveLogon, out int size);
            this.logger.LogWarningDebug($"0x{pData.ToString("X16")}: Password got packed");

            return new CredentialSerialization()
            {
                AuthenticationPackage = authPackage,
                SerializationData = pData,
                ProviderClassGuid = providerId,
                SerializationSize = (uint)size
            };
        }

        private unsafe IntPtr SerializeKerbLogon(string domain, string username, string password, KerbLogonSubmitType type, out int size)
        {
            size = sizeof(KerberosInteractiveUnlockLogon) +
                        Encoding.Unicode.GetMaxByteCount(domain.Length) +
                        Encoding.Unicode.GetMaxByteCount(username.Length) +
                        Encoding.Unicode.GetMaxByteCount(password.Length);

            IntPtr pBuffer = Marshal.AllocCoTaskMem(size);
            byte* buffer = (byte*)pBuffer;

            var logon = (KerberosInteractiveUnlockLogon*)buffer;

            logon->SubmitType = type;
            logon->LogonDomainName.MaxLength = (ushort)Encoding.Unicode.GetMaxByteCount(domain.Length);
            logon->LogonDomainName.Length = (ushort)(domain.Length * sizeof(char));
            logon->LogonDomainName.Buffer = (IntPtr)sizeof(KerberosInteractiveUnlockLogon);
            fixed (char* domainBuffer = domain)
            {
                Encoding.Unicode.GetBytes(domainBuffer, domain.Length,
                    buffer + logon->LogonDomainName.Buffer.ToInt64(), logon->LogonDomainName.Length);
            }

            logon->Username.Length = (ushort)(username.Length * sizeof(char));
            logon->Username.MaxLength = (ushort)Encoding.Unicode.GetMaxByteCount(username.Length);
            logon->Username.Buffer = (IntPtr)(sizeof(KerberosInteractiveUnlockLogon) + logon->LogonDomainName.MaxLength);
            fixed (char* usernameBuffer = username)
            {
                Encoding.Unicode.GetBytes(usernameBuffer, username.Length,
                    buffer + logon->Username.Buffer.ToInt64(), logon->Username.Length);
            }

            logon->Password.MaxLength = (ushort)Encoding.Unicode.GetMaxByteCount(password.Length);
            logon->Password.Length = (ushort)(password.Length * sizeof(char));
            logon->Password.Buffer = (IntPtr)(sizeof(KerberosInteractiveUnlockLogon) + logon->LogonDomainName.MaxLength + logon->Username.MaxLength);
            fixed (char* passwordBuffer = password)
            {
                Encoding.Unicode.GetBytes(passwordBuffer, password.Length,
                    buffer + logon->Password.Buffer.ToInt64(), logon->Password.Length);
            }

            return pBuffer;
        }

        private unsafe IntPtr SerializeKerbLogon(string domain, string username, SecureString password, KerbLogonSubmitType type, out int size)
        {
            size = sizeof(KerberosInteractiveUnlockLogon) +
                        Encoding.Unicode.GetMaxByteCount(domain.Length) +
                        Encoding.Unicode.GetMaxByteCount(username.Length) +
                        Encoding.Unicode.GetMaxByteCount(password.Length);


            IntPtr pBuffer = Marshal.AllocCoTaskMem(size);
            byte* buffer = (byte*)pBuffer;

            var logon = (KerberosInteractiveUnlockLogon*)buffer;

            logon->SubmitType = type;
            logon->LogonDomainName.MaxLength = (ushort)Encoding.Unicode.GetMaxByteCount(domain.Length);
            logon->LogonDomainName.Length = (ushort)(domain.Length * sizeof(char));
            logon->LogonDomainName.Buffer = (IntPtr)sizeof(KerberosInteractiveUnlockLogon);
            fixed (char* domainBuffer = domain)
            {
                Encoding.Unicode.GetBytes(domainBuffer, domain.Length,
                    buffer + logon->LogonDomainName.Buffer.ToInt64(), logon->LogonDomainName.Length);
            }

            logon->Username.Length = (ushort)(username.Length * sizeof(char));
            logon->Username.MaxLength = (ushort)Encoding.Unicode.GetMaxByteCount(username.Length);
            logon->Username.Buffer = (IntPtr)(sizeof(KerberosInteractiveUnlockLogon) + logon->LogonDomainName.MaxLength);
            fixed (char* usernameBuffer = username)
            {
                Encoding.Unicode.GetBytes(usernameBuffer, username.Length,
                    buffer + logon->Username.Buffer.ToInt64(), logon->Username.Length);
            }

            logon->Password.MaxLength = (ushort)Encoding.Unicode.GetMaxByteCount(password.Length);
            logon->Password.Length = (ushort)(password.Length * sizeof(char));
            logon->Password.Buffer = (IntPtr)(sizeof(KerberosInteractiveUnlockLogon) + logon->LogonDomainName.MaxLength + logon->Username.MaxLength);

            IntPtr buff = IntPtr.Zero;

            try
            {
                buff = Marshal.SecureStringToCoTaskMemUnicode(password);
                this.logger.LogWarningDebug($"0x{buff.ToString("X16")} - Serializer: Unprotected password");
                IntPtr targetPositionToCopyTo = (IntPtr)(buffer + logon->Password.Buffer.ToInt64());

                Buffer.MemoryCopy(buff.ToPointer(), targetPositionToCopyTo.ToPointer(), logon->Password.Length, password.Length * sizeof(char));

                this.logger.LogWarningDebug($"0x{targetPositionToCopyTo.ToString("X16")} - Serializer: Copied unprotected password into LSA string buffer");
            }
            finally
            {
                if (buff != IntPtr.Zero)
                {
                    Marshal.ZeroFreeCoTaskMemUnicode(buff);
                    this.logger.LogWarningDebug($"0x{buff.ToString("X16")} - Serializer: Freed Unprotected password");
                }
            }

            this.logger.LogWarningDebug($"0x{((IntPtr)buffer).ToString("X16")} - Serializer: Put password");
            return pBuffer;
        }
    }
}
