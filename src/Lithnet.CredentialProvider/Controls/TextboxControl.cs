using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A text box control used to capture use input
    /// </summary>
    public class TextboxControl : ControlBase
    {
        private string text;

        private TextboxControl(TextboxControl source) : base(source) { }

        /// <summary>
        /// Creates a new <c ref="TextboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public TextboxControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="TextboxControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public TextboxControl(string key, string label) : base(key, label, FieldType.EditText)
        {
        }

        /// <summary>
        /// Gets or sets the text value of the text box
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.Events?.SetFieldString(this.Credential, this.Id, Marshal.StringToCoTaskMemUni(value));
                    this.RaisePropertyChanged();
                }
            }
        }

        internal void SetTextInternal(string text)
        {
            this.text = text;
            this.RaisePropertyChanged(nameof(this.Text));
        }

        internal override ControlBase Clone()
        {
            var clone = new TextboxControl(this);
            clone.Text = this.Text;

            return clone;
        }
    }
}