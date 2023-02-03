using System;
using System.Management.Automation;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    [Cmdlet(VerbsLifecycle.Disable, "CredentialProvider", DefaultParameterSetName = "DisableByFileName")]
    public class DisableCredentialProviderCmdlet : PSCmdlet
    {
        [Parameter(ParameterSetName = "DisableByFileName")]
        public string File { get; set; }

        [Parameter(ParameterSetName = "DisableByClsid")]
        public Guid Clsid { get; set; }

        [Parameter(ParameterSetName = "DisableByProgId")]
        public string ProgId { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "DisableByFileName")
            {
                if (!CredentialProviderRegistrationServices.IsManagedAssembly(this.File))
                {
                    throw new System.Exception("This tool cannot disable managed assemblies by file name. You can disable native assemblies using the CLSID or ProgID");
                }
                var assembly = CredentialProviderRegistrationServices.LoadAssembly(this.File);

                foreach (var type in CredentialProviderRegistrationServices.GetCredentialProviders(assembly))
                {
                    CredentialProviderRegistrationServices.DisableCredentialProvider(type);
                    this.WriteVerbose($"Disabled credential provider {type.FullName}");
                }
            }
            else if (this.ParameterSetName == "DisableByClsid")
            {
                CredentialProviderRegistrationServices.DisableCredentialProvider(this.Clsid);
            }
            else if (this.ParameterSetName == "DisableByProgId")
            {
                var clsid = CredentialProviderRegistrationServices.GetClsidFromProgId(this.ProgId);
                CredentialProviderRegistrationServices.DisableCredentialProvider(clsid);
            }
        }
    }
}
