using System;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A text label that appears in a smaller font size
    /// </summary>
    public class SmallLabelControl : ControlBase
    {
        /// <summary>
        /// Creates a new <c ref="SmallLabelControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public SmallLabelControl(string key) : this(key, null, false) { }

        /// <summary>
        /// Creates a new <c ref="SmallLabelControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public SmallLabelControl(string key, string label) : this(key, label, false) { }

        private protected SmallLabelControl(SmallLabelControl source) : base(source) { }

        private protected SmallLabelControl(string key, string label, bool isProviderLabel) :
            base(key, label, FieldType.SmallText, isProviderLabel ? Guid.Parse(CredProviderConstants.CPFG_CREDENTIAL_PROVIDER_LABEL) : Guid.Empty)
        {
            if (isProviderLabel)
            {
                this.State = FieldState.DisplayInDeselectedTile;
            }
        }

        internal override ControlBase Clone()
        {
            return new SmallLabelControl(this);
        }
    }
}