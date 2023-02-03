using System;
using System.Management.Automation;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    [Cmdlet(VerbsLifecycle.Enable, "CredentialProvider", DefaultParameterSetName = "EnableByFileName")]
    public class EnableCredentialProviderCmdlet : PSCmdlet
    {
        [Parameter(ParameterSetName = "EnableByFileName")]
        public string File { get; set; }

        [Parameter(ParameterSetName = "EnableByClsid")]
        public Guid Clsid { get; set; }

        [Parameter(ParameterSetName = "EnableByProgId")]
        public string ProgId { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "EnableByFileName")
            {
                if (!CredentialProviderRegistrationServices.IsManagedAssembly(this.File))
                {
                    throw new System.Exception("This tool cannot enable managed assemblies by file name. You can enable native assemblies using the CLSID or ProgID");
                }

                var assembly = CredentialProviderRegistrationServices.LoadAssembly(this.File);

                foreach (var type in CredentialProviderRegistrationServices.GetCredentialProviders(assembly))
                {
                    CredentialProviderRegistrationServices.EnableCredentialProvider(type);
                    this.WriteVerbose($"Enabled credential provider {type.FullName}");
                }
            }
            else if (this.ParameterSetName == "EnableByClsid")
            {
                CredentialProviderRegistrationServices.EnableCredentialProvider(this.Clsid);
            }
            else if (this.ParameterSetName == "EnableByProgId")
            {
                var clsid = CredentialProviderRegistrationServices.GetClsidFromProgId(this.ProgId);
                CredentialProviderRegistrationServices.EnableCredentialProvider(clsid);
            }
        }
    }
}
