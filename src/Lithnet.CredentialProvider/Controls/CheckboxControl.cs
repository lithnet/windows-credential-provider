using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A control that renders a check box in the credential UI
    /// </summary>
    public class CheckboxControl : ControlBase
    {
        private bool isChecked;

        /// <summary>
        /// Creates a new <c ref="CheckboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public CheckboxControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="CheckboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public CheckboxControl(string key, string label) : base(key, label, FieldType.CheckBox) { }

        /// <summary>
        /// Gets or sets a value indicating if the checkbox is currently checked
        /// </summary>
        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.Events?.SetFieldCheckbox(this.Credential, this.Id, value ? 1 : 0, this.Label);
                    this.RaisePropertyChanged();
                }
            }
        }

        internal void SetIsCheckedInternal(bool value)
        {
            this.isChecked = value;
            this.RaisePropertyChanged(nameof(this.IsChecked));
        }

        private CheckboxControl(CheckboxControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            var clone = new CheckboxControl(this);
            clone.IsChecked = this.IsChecked;
            return clone;
        }
    }
}