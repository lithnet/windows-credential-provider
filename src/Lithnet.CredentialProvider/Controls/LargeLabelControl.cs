using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A text label that appears in a larger font size
    /// </summary>
    public class LargeLabelControl : ControlBase
    {
        /// <summary>
        /// Creates a new <c ref="LargeLabelControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public LargeLabelControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="LargeLabelControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public LargeLabelControl(string key, string label) : base(key, label, FieldType.LargeText) { }

        private LargeLabelControl(LargeLabelControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            return new LargeLabelControl(this);
        }
    }
}