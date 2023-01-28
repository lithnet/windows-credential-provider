using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct KerberosInteractiveUnlockLogon
    {
        public KerbLogonSubmitType SubmitType;
        public LsaStringUni LogonDomainName;
        public LsaStringUni Username;
        public LsaStringUni Password;

        public long LoginId;
    }
}
