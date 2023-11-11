using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider.Samples
{
    public class TestCredentialProviderTile : CredentialTile2
    {
        private TextboxControl UsernameControl;
        private SecurePasswordTextboxControl PasswordControl;
        private SecurePasswordTextboxControl PasswordConfirmControl;
        private ComboboxControl ComboboxControl;
        private CheckboxControl CheckboxControl;
        private SmallLabelControl CheckboxStateControl;
        private SmallLabelControl ComboboxStateControl;

        private ICredentialProviderLogger logger = InternalLoggerFactory.Instance.CreateLogger<TestCredentialProviderTile>();

        public TestCredentialProviderTile(CredentialProviderBase credentialProvider) : base(credentialProvider)
        {
        }

        public TestCredentialProviderTile(CredentialProviderBase credentialProvider, CredentialProviderUser user) : base(credentialProvider, user)
        {
        }

        public string Username
        {
            get => UsernameControl.Text;
            set => UsernameControl.Text = value;
        }

        public SecureString Password
        {
            get => PasswordControl.Password;
            set => PasswordControl.Password = value;
        }

        public SecureString ConfirmPassword
        {
            get => PasswordConfirmControl.Password;
            set => PasswordConfirmControl.Password = value;
        }

        public string ComboboxSelectedItemLabel
        {
            get => ComboboxStateControl.Label;
            set => ComboboxStateControl.Label = value;
        }

        public bool IsChecked
        {
            get => CheckboxControl.IsChecked;
            set => CheckboxControl.IsChecked = value;
        }

        public string CheckboxStateLabel
        {
            get => CheckboxStateControl.Label;
            set => CheckboxStateControl.Label = value;
        }

        public override void Initialize()
        {
            if (UsageScenario == UsageScenario.ChangePassword)
            {
                PasswordConfirmControl = Controls.GetControl<SecurePasswordTextboxControl>(ControlKeys.ConfirmPassword);
            }

            PasswordControl = Controls.GetControl<SecurePasswordTextboxControl>(ControlKeys.Password);
            CheckboxControl = Controls.GetControl<CheckboxControl>(ControlKeys.Checkbox);
            CheckboxStateControl = Controls.GetControl<SmallLabelControl>(ControlKeys.LabelCheckboxValue);

            Controls.GetControl<CommandLinkControl>(ControlKeys.CommandLinkCheckboxValue).OnClick = () =>
            {
                CheckboxControl.IsChecked = !CheckboxControl.IsChecked;
            };

            CheckboxControl.PropertyChanged += CheckboxControl_PropertyChanged;

            UsernameControl = Controls.GetControl<TextboxControl>(ControlKeys.Username);
            Controls.GetControl<CommandLinkControl>(ControlKeys.CommandLinkUsername).OnClick = () =>
            {
                Username = Guid.NewGuid().ToString();
            };

            ComboboxStateControl = Controls.GetControl<SmallLabelControl>(ControlKeys.LabelComboboxSelectedItem);

            ComboboxControl = Controls.GetControl<ComboboxControl>(ControlKeys.Combobox);
            ComboboxControl.PropertyChanged += ComboboxControl_PropertyChanged;
            ComboboxControl.ComboBoxItems.Add("Item 1");
            ComboboxControl.ComboBoxItems.Add("Item 2");
            ComboboxControl.ComboBoxItems.Add("Item 3");
            ComboboxControl.SelectedItemIndex = 0;

            int count = 3;
            Controls.GetControl<CommandLinkControl>(ControlKeys.CommandLinkComboboxAdd).OnClick = () =>
            {
                ComboboxControl.ComboBoxItems.Add($"Item {++count}");
            };

            Controls.GetControl<CommandLinkControl>(ControlKeys.CommandLinkComboboxRemove).OnClick = () =>
            {
                if (ComboboxControl.ComboBoxItems.Count > 0)
                {
                    ComboboxControl.ComboBoxItems.Remove(ComboboxControl.ComboBoxItems[ComboboxControl.ComboBoxItems.Count - 1]);
                }
            };

            Username = User?.QualifiedUserName;
        }

        private void ComboboxControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ComboboxControl.SelectedItemIndex))
            {
                ComboboxSelectedItemLabel = $"Selected item: {(ComboboxControl.SelectedItem ?? "(none)")}";
            }
        }

        private void CheckboxControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckboxStateControl.Label = $"The check box is {(CheckboxControl.IsChecked ? "checked" : "not checked")}";
        }

        protected override CredentialResponseBase GetCredentials()
        {
            string username;
            string domain;

            if (Username.Contains("\\"))
            {
                domain = Username.Split('\\')[0];
                username = Username.Split('\\')[1];
            }
            else
            {
                username = Username;
                domain = Environment.MachineName;
            }

            var spassword = Controls.GetControl<SecurePasswordTextboxControl>(ControlKeys.Password).Password;

            if (!IsChecked)
            {
                logger.LogTrace("Performing secure login");

                return new CredentialResponseSecure()
                {
                    IsSuccess = true,
                    Password = spassword,
                    Domain = domain,
                    Username = username
                };
            }
            else
            {
                logger.LogTrace("Performing insecure login");
                string plainTextPassword = GetPlainTextPasswordFromSecureString(spassword);

                return new CredentialResponseInsecure()
                {
                    IsSuccess = true,
                    Password = plainTextPassword,
                    Domain = domain,
                    Username = username
                };
            }
        }

        private string GetPlainTextPasswordFromSecureString(SecureString s)
        {
            string plainTextPassword;
            IntPtr pPassword = IntPtr.Zero;
            try
            {
                pPassword = Marshal.SecureStringToGlobalAllocUnicode(s);
                plainTextPassword = Marshal.PtrToStringUni(pPassword);
            }
            finally
            {
                if (pPassword != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pPassword);
                }
            }

            return plainTextPassword;
        }
    }
}