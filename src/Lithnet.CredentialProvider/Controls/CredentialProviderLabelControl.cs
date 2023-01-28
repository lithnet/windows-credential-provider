namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a control that provides the credential UI with the name of this credential provider
    /// </summary>
    public class CredentialProviderLabelControl : SmallLabelControl
    {
        /// <summary>
        /// Creates a new <c ref="CredentialProviderLabelControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public CredentialProviderLabelControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="CredentialProviderLabelControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public CredentialProviderLabelControl(string key, string label) : base(key, label, true)
        {
            this.State = FieldState.DisplayInDeselectedTile;
        }

        private CredentialProviderLabelControl(CredentialProviderLabelControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            return new CredentialProviderLabelControl(this);
        }
    }
}