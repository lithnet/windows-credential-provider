using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider.Samples
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Lithnet.CredentialProvider.Sample.Core.x64")]
    [Guid("4cd12d80-9259-4f38-94dc-1828080ad9ff")]
    public class TestCredentialProviderCoreX64 : CredentialProviderBase
    {
        private static readonly ICredentialProviderLogger logger = InternalLoggerFactory.Instance.CreateLogger<TestCredentialProviderCoreX64>();

        protected override ICredentialProviderLoggerFactory GetLoggerFactory()
        {
            return InternalLoggerFactory.Instance;
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

                var providerLogo = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Lithnet.CredentialProvider.Sample.Core.x64.Resources.TileIcon.png"));
                var transparentUserTile = CreateTransparentUserTile();

                yield return new CredentialProviderLogoControl(ControlKeys.ImageCredentialProvider, "Credential provider logo", providerLogo);
                yield return new UserTileControl(ControlKeys.ImageUserTile, "Transparent user tile image", transparentUserTile);

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

        private static Bitmap CreateTransparentUserTile()
        {
            // CredentialTile3 preserves the alpha channel in this image. CredentialTile and CredentialTile2 render it against the control's BackgroundColor.
            Bitmap image = new Bitmap(128, 128, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(image))
            using (SolidBrush shadow = new SolidBrush(Color.FromArgb(96, 0, 0, 0)))
            using (SolidBrush foreground = new SolidBrush(Color.FromArgb(255, 38, 132, 255)))
            using (Pen outline = new Pen(Color.White, 5))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);
                graphics.FillEllipse(shadow, 28, 30, 88, 88);
                graphics.FillEllipse(foreground, 12, 12, 88, 88);
                graphics.DrawEllipse(outline, 12, 12, 88, 88);
            }

            return image;
        }
    }
}
