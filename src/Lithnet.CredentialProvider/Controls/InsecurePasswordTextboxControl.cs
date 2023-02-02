using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A masked password text box, with a Password property that provides the password in plain text
    /// </summary>
    public class InsecurePasswordTextboxControl : ControlBase
    {
        private string password;

        /// <summary>
        /// Creates a new <c ref="InsecurePasswordTextboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public InsecurePasswordTextboxControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="InsecurePasswordTextboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public InsecurePasswordTextboxControl(string key, string label) : base(key, label, FieldType.PasswordText) { }

        private InsecurePasswordTextboxControl(InsecurePasswordTextboxControl source) : base(source) { }

        /// <summary>
        /// Gets or sets the password value in plain text
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    if (this.password?.Length > 0)
                    {
                        var ptr = Marshal.StringToCoTaskMemUni(this.password);
                        this.logger.LogWarningDebug($"0x:{ptr.ToString("X16")} - Created ptr for outgoing SetFieldString");
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

        internal void SetPasswordInternal(string s)
        {
            this.password = s;
            this.RaisePropertyChanged(nameof(this.Password));
        }

        internal override ControlBase Clone()
        {
            var clone = new InsecurePasswordTextboxControl(this);
            clone.password = this.password;
            return clone;
        }
    }
}