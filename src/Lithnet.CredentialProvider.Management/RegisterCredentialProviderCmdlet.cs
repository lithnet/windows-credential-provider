using System.Management.Automation;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    [Cmdlet(VerbsLifecycle.Register, "CredentialProvider")]
    public class RegisterCredentialProviderCmdlet : PSCmdlet
    {
        [Parameter]
        public string File { get; set; }

        protected override void ProcessRecord()
        {
            if (!CredentialProviderRegistrationServices.IsManagedAssembly(this.File))
            {
                throw new System.Exception("This tool can only register managed assemblies");
            }

            var assembly = CredentialProviderRegistrationServices.LoadAssembly(this.File);

            foreach (var type in CredentialProviderRegistrationServices.GetCredentialProviders(assembly))
            {
                CredentialProviderRegistrationServices.RegisterCredentialProvider(type);
                this.WriteVerbose($"Registered credential provider {type.FullName}");
            }
        }
    }
}
