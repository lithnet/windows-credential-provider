using System.Drawing;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a control that provides the credential UI with the logo of this credential provider
    /// </summary>
    public class CredentialProviderLogoControl : BitmapControl
    {
        /// <summary>
        /// Creates a new <c ref="CredentialProviderLogoControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public CredentialProviderLogoControl(string key) : this(key, null, null) { }

        /// <summary>
        /// Creates a new <c ref="CredentialProviderLogoControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control. This value is not displayed to the user</param>
        public CredentialProviderLogoControl(string key, string label) : this(key, label, null) { }

        /// <summary>
        /// Creates a new <c ref="CredentialProviderLogoControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        /// <param name="bitmap">The bitmap to use as the logo</param>
        public CredentialProviderLogoControl(string key, string label, Bitmap bitmap) : base(key, label, true, bitmap)
        {
            this.State = FieldState.DisplayInDeselectedTile;
        }

        private CredentialProviderLogoControl(CredentialProviderLogoControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            var clone = new CredentialProviderLogoControl(this);
            clone.Bitmap = this.Bitmap;
            clone.BackgroundColor = this.BackgroundColor;
            return clone;
        }
    }
}