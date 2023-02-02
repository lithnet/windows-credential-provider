using System;
using System.Runtime.InteropServices;
using System.Security;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A masked password text box, with a Password property that provides the password as a secure string
    /// </summary>
    public class SecurePasswordTextboxControl : ControlBase
    {
        private SecureString password;

        /// <summary>
        /// Creates a new <c ref="SecurePasswordTextboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public SecurePasswordTextboxControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="SecurePasswordTextboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public SecurePasswordTextboxControl(string key, string label) : base(key, label, FieldType.PasswordText) { }

        /// <summary>
        /// Gets or sets the password value as a secure string
        /// </summary>
        public SecureString Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    if (this.password?.Length > 0)
                    {
                        var ptr = Marshal.SecureStringToCoTaskMemUnicode(this.password);
                        this.logger.LogWarningDebug($"0x:{ptr.ToString("X16")} - CONTROL: Created ptr for outgoing SetFieldString");
                        this.Events?.SetFieldString(this.Credential, this.Id, ptr);
                    }
                    else
                    {
                        this.Events?.SetFieldString(this.Credential, this.Id, IntPtr.Zero);
                    }

                    this.RaisePropertyChanged(nameof(this.Password));
                }
            }
        }

        internal void SetPasswordInternal(SecureString s)
        {
            this.password = s;
            this.RaisePropertyChanged(nameof(this.Password));
        }

        private SecurePasswordTextboxControl(SecurePasswordTextboxControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            var clone = new SecurePasswordTextboxControl(this);
            clone.password = this.password;
            return clone;
        }
    }
}