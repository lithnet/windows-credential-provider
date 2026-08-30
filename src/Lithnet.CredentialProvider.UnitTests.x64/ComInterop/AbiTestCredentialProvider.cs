using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2A83F3F8-A46C-4104-8FFB-BD9979279450")]
    internal sealed class AbiTestCredentialProvider : CredentialProviderBase
    {
        public AbiTestCredentialProvider()
        {
            this.Field = new SmallLabelControl("message", "COM ABI test");
        }

        public SmallLabelControl Field { get; }

        public AbiTestCredentialTile2 Tile { get; private set; }

        public override bool IsUsageScenarioSupported(UsageScenario cpus, CredUIWinFlags dwFlags)
        {
            return cpus == UsageScenario.CredUI;
        }

        public override IEnumerable<ControlBase> GetControls(UsageScenario cpus)
        {
            return new ControlBase[] { this.Field };
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
            this.Tile = new AbiTestCredentialTile2(this);
            return this.Tile;
        }

        public override CredentialTile2 CreateUserTile(CredentialProviderUser user)
        {
            return null;
        }
    }
}
