using System;
using System.Management.Automation;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    [Cmdlet(VerbsLifecycle.Unregister, "CredentialProvider", DefaultParameterSetName = "UnregisterByFileName")]
    public class UnregisterCredentialProviderCmdlet : PSCmdlet
    {
        [Parameter(ParameterSetName = "UnregisterByFileName")]
        public string File { get; set; }

        [Parameter(ParameterSetName = "UnregisterByClsid")]
        public Guid Clsid { get; set; }

        [Parameter(ParameterSetName = "UnregisterByProgId")]
        public string ProgId { get; set; }

        [Parameter]
        public SwitchParameter UnregisterCom { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "UnregisterByFileName")
            {
                if (!CredentialProviderRegistrationServices.IsManagedAssembly(this.File))
                {
                    throw new Exception("This tool cannot unregister managed assemblies by file name. You can unregister native assemblies using the CLSID or ProgID");
                }

                var assembly = CredentialProviderRegistrationServices.LoadAssembly(this.File);

                foreach (var type in CredentialProviderRegistrationServices.GetCredentialProviders(assembly))
                {
                    CredentialProviderRegistrationServices.UnregisterCredentialProvider(type, this.GetSwitchValue(this.UnregisterCom, nameof(this.UnregisterCom)));
                    this.WriteVerbose($"Unregistered credential provider {type.FullName}");
                }
            }
            else if (this.ParameterSetName == "UnregisterByClsid")
            {
                CredentialProviderRegistrationServices.UnregisterCredentialProvider(this.Clsid, this.GetSwitchValue(this.UnregisterCom, nameof(this.UnregisterCom)));
            }
            else if (this.ParameterSetName == "UnregisterByProgId")
            {
                var clsid = CredentialProviderRegistrationServices.GetClsidFromProgId(this.ProgId);
                CredentialProviderRegistrationServices.UnregisterCredentialProvider(clsid, this.GetSwitchValue(this.UnregisterCom, nameof(this.UnregisterCom)));
            }
        }

        protected bool GetSwitchValue(SwitchParameter parameter, string name)
        {
            if (this.MyInvocation.BoundParameters.ContainsKey(name))
            {
                return parameter.ToBool();
            }

            return false;
        }
    }
}
