using System.Drawing;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// A control that displays the user's tile image
    /// </summary>
    public class UserTileControl : BitmapControl
    {
        /// <summary>
        /// Creates a new <c ref="UserTileControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public UserTileControl(string key) : this(key, null, null) { }

        /// <summary>
        /// Creates a new <c ref="UserTileControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public UserTileControl(string key, string label) : this(key, label, null) { }

        /// <summary>
        /// Creates a new <c ref="UserTileControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        /// <param name="bitmap">The bitmap to use as the user's tile image</param>
        public UserTileControl(string key, string label, Bitmap bitmap) : base(key, label, false, bitmap) { }

        private UserTileControl(UserTileControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            var clone = new UserTileControl(this);
            clone.Bitmap = this.Bitmap;
            clone.BackgroundColor = this.BackgroundColor;

            return clone;
        }
    }
}