using System;
using System.Management.Automation;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    [Cmdlet(VerbsCommon.Get, "CredentialProvider", DefaultParameterSetName = "None")]
    public class GetCredentialProviderCmdlet : PSCmdlet
    {
        [Parameter(ParameterSetName = "GetByFileName")]
        public string File { get; set; }

        [Parameter(ParameterSetName = "GetByClsid")]
        public Guid Clsid { get; set; }

        [Parameter(ParameterSetName = "GetByProgId")]
        public string ProgId { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "None")
            {
                foreach (var item in CredentialProviderRegistrationServices.GetCredentalProviders())
                {
                    this.WriteObject(item);
                }
            }
            else if (this.ParameterSetName == "GetByFileName")
            {
                var assembly = CredentialProviderRegistrationServices.LoadAssembly(this.File);

                foreach (var type in CredentialProviderRegistrationServices.GetCredentialProviders(assembly))
                {
                    CredentialProviderRegistrationServices.GetCredentialProvider(type);
                    this.WriteVerbose($"Got credential provider {type.FullName}");
                }
            }
            else if (this.ParameterSetName == "GetByClsid")
            {
                CredentialProviderRegistrationServices.GetCredentialProvider(this.Clsid);
            }
            else if (this.ParameterSetName == "GetByProgId")
            {
                var clsid = CredentialProviderRegistrationServices.GetClsidFromProgId(this.ProgId);
                CredentialProviderRegistrationServices.GetCredentialProvider(clsid);
            }
        }
    }
}
