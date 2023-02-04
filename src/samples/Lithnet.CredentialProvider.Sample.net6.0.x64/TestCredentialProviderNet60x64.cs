using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider.Samples
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Lithnet.CredentialProvider.Sample.net6.0.x64")]
    [Guid("4cd12d80-9259-4f38-94dc-1828080ad9ff")]
    public class TestCredentialProviderNet60x64 : CredentialProviderBase
    {
        private static readonly ILogger logger = InternalLogger.LoggerFactory.CreateLogger<TestCredentialProviderNet60x64>();

        protected override ILoggerFactory GetLoggerFactory()
        {
            return InternalLogger.LoggerFactory;
        }

        public override IEnumerable<ControlBase> GetControls(UsageScenario cpus)
        {
            var password = new SecurePasswordTextboxControl(ControlKeys.Password, "Password");

            if (cpus == UsageScenario.ChangePassword)
            {
                var confirmPassword = new SecurePasswordTextboxControl(ControlKeys.ConfirmPassword, "Confirm password");
                yield return new TextboxControl(ControlKeys.Username, "Username");
                yield return password;
                yield return confirmPassword;
                yield return new SubmitButtonControl(ControlKeys.ButtonSubmit, "Submit", confirmPassword);
            }
            else
            {
                yield return new CredentialProviderLabelControl(ControlKeys.LabelCredentialProvider, "Login with showcase credential provider");

                var image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Lithnet.CredentialProvider.Sample.net6.0.x64.Resources.TileIcon.png"));

                yield return new CredentialProviderLogoControl(ControlKeys.ImageCredentialProvider, "Credential provider logo", image);
                yield return new CredentialProviderLogoControl(ControlKeys.ImageUserTile, "User tile image", image);

                yield return new LargeLabelControl(ControlKeys.LabelLargeHeading, "The is our showcase credential provider");
                yield return new SmallLabelControl(ControlKeys.LabelSmallHeading, "Let's see what we can do");

                yield return new CheckboxControl(ControlKeys.Checkbox, "A checkbox");
                yield return new SmallLabelControl(ControlKeys.LabelCheckboxValue, "The check box is currently unchecked");
                yield return new CommandLinkControl(ControlKeys.CommandLinkCheckboxValue, "Click this link to change the check box value in code behind");

                yield return new ComboboxControl(ControlKeys.Combobox, "Items to choose from:");
                yield return new SmallLabelControl(ControlKeys.LabelComboboxSelectedItem, "This is the currently selected item: <none>");
                yield return new CommandLinkControl(ControlKeys.CommandLinkComboboxAdd, "Add a random item to the combo box");
                yield return new CommandLinkControl(ControlKeys.CommandLinkComboboxRemove, "Remove the last item from the combo box");

                yield return new TextboxControl(ControlKeys.Username, "Username");
                yield return new CommandLinkControl(ControlKeys.CommandLinkUsername, "Click this link to generate a random username");

                yield return password;
                yield return new SubmitButtonControl(ControlKeys.ButtonSubmit, "Submit", password);
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

        public override CredentialTile CreateGenericTile()
        {
            return new TestCredentialProviderTile(this);
        }

        public override CredentialTile2 CreateUserTile(CredentialProviderUser user)
        {
            return new TestCredentialProviderTile(this, user);
        }
    }
}
