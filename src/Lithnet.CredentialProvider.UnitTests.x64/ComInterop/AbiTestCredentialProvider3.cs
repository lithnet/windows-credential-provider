using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2C87FFBB-6497-4D1A-8291-74B5DB3A0C3A")]
    internal sealed class AbiTestCredentialProvider3 : CredentialProviderBase
    {
        public AbiTestCredentialProvider3()
        {
            this.Field = new SmallLabelControl("message", "Callback ABI test");
            this.Logo = new CredentialProviderLogoControl("logo", "Callback ABI logo");
        }

        public SmallLabelControl Field { get; }

        public CredentialProviderLogoControl Logo { get; }

        public AbiTestCredentialTile3 Tile { get; private set; }

        public override bool IsUsageScenarioSupported(UsageScenario cpus, CredUIWinFlags dwFlags)
        {
            return cpus == UsageScenario.CredUI;
        }

        public override IEnumerable<ControlBase> GetControls(UsageScenario cpus)
        {
            return new ControlBase[] { this.Field, this.Logo };
        }

        public override bool ShouldIncludeUserTile(CredentialProviderUser user)
        {
            return false;
        }

        public override bool ShouldIncludeGenericTile()
        {
            return true;
        }

        public override CredentialTile CreateGenericTile()
        {
            this.Tile = new AbiTestCredentialTile3(this);
            return this.Tile;
        }

        public override CredentialTile2 CreateUserTile(CredentialProviderUser user)
        {
            return null;
        }
    }
}
