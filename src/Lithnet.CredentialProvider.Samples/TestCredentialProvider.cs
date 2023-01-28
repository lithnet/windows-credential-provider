using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider.Samples
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Lithnet.CredentialProvider.Samples.TestCredentialProvider")]
    [Guid("1A3993B6-EB2B-44BB-A788-7AB1711DFF16")]
    public class TestCredentialProvider : CredentialProviderBase
    {
        private static readonly ILogger logger = Program.LoggerFactory.CreateLogger<TestCredentialProvider>();

        public TestCredentialProvider()
        {
        }

        public override ILoggerFactory GetLoggerFactory()
        {
            return Program.LoggerFactory;
        }

        public override IEnumerable<ControlBase> GetControls(UsageScenario cpus)
        {

            var password = new SecurePasswordTextboxControl(TestCredentialProviderControlKeys.Password, "Password");

            if (cpus == UsageScenario.ChangePassword)
            {
                var confirmPassword = new SecurePasswordTextboxControl(TestCredentialProviderControlKeys.ConfirmPassword, "Confirm password");
                yield return new TextboxControl(TestCredentialProviderControlKeys.Username, "Username");
                yield return password;
                yield return confirmPassword;
                yield return new SubmitButtonControl(TestCredentialProviderControlKeys.ButtonSubmit, "Submit", confirmPassword);
            }
            else
            {
                yield return new CredentialProviderLabelControl(TestCredentialProviderControlKeys.LabelCredentialProvider, "Login with showcase credential provider");
                yield return new CredentialProviderLogoControl(TestCredentialProviderControlKeys.ImageCredentialProvider, "Credential provider logo", Resources.TileIcon);
                yield return new CredentialProviderLogoControl(TestCredentialProviderControlKeys.ImageUserTile, "User tile image", Resources.TileIcon);

                yield return new LargeLabelControl(TestCredentialProviderControlKeys.LabelLargeHeading, "The is our showcase credential provider");
                yield return new SmallLabelControl(TestCredentialProviderControlKeys.LabelSmallHeading, "Let's see what we can do");

                yield return new CheckboxControl(TestCredentialProviderControlKeys.Checkbox, "A checkbox");
                yield return new SmallLabelControl(TestCredentialProviderControlKeys.LabelCheckboxValue, "The check box is currently unchecked");
                yield return new CommandLinkControl(TestCredentialProviderControlKeys.CommandLinkCheckboxValue, "Click this link to change the check box value in code behind");

                yield return new ComboboxControl(TestCredentialProviderControlKeys.Combobox, "Items to choose from:");
                yield return new SmallLabelControl(TestCredentialProviderControlKeys.LabelComboboxSelectedItem, "This is the currently selected item: <none>");
                yield return new CommandLinkControl(TestCredentialProviderControlKeys.CommandLinkComboboxAdd, "Add a random item to the combo box");
                yield return new CommandLinkControl(TestCredentialProviderControlKeys.CommandLinkComboboxRemove, "Remove the last item from the combo box");

                yield return new TextboxControl(TestCredentialProviderControlKeys.Username, "Username");
                yield return new CommandLinkControl(TestCredentialProviderControlKeys.CommandLinkUsername, "Click this link to generate a random username");

                yield return password;
                yield return new SubmitButtonControl(TestCredentialProviderControlKeys.ButtonSubmit, "Submit", password);
            }
        }

        public override bool IsUsageScenarioSupported(UsageScenario cpus, CredUIWinFlags dwFlags)
        {
            switch (cpus)
            {
                case UsageScenario.Logon:
                case UsageScenario.UnlockWorkstation:
                case UsageScenario.CredUI:
                case UsageScenario.ChangePassword:
                    return true;

                default:
                    return false;
            }
        }

        public override bool ShouldIncludeUserTile(CredentialProviderUser user)
        {
            return true;
        }

        public override bool ShouldIncludeGenericTile()
        {
            return true;
        }

        public override CredentialProviderCredential1Tile CreateGenericTile()
        {
            return new TestCredentialProviderTile(this);
        }

        public override CredentialProviderCredential1Tile CreateUserTile(CredentialProviderUser user)
        {
            return new TestCredentialProviderTile(this, user);
        }
    }
}
