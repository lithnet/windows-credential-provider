using System;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    public class CredentialProviderRegistrationData
    {
        public bool IsComRegistered { get; set; }

        public bool IsCredentialProviderRegistered { get; set; }

        public bool IsCredentalProviderEnabled { get; set; }

        public string CredentialProviderName { get; set; }  

        public Guid Clsid { get; set; }

        public string ProgId { get; set; }

        public string DllPath { get; set; }

        public DllType DllType { get; set; }
    }
}
