using System.Drawing;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a control that displays the user's tile image.
    /// </summary>
    /// <remarks>See <see cref="BitmapControl.BackgroundColor"/> for the image transparency behaviour of each credential tile version.</remarks>
    public class UserTileControl : BitmapControl
    {
        /// <summary>
        /// Creates a new <see cref="UserTileControl"/> control.
        /// </summary>
        /// <param name="key">The unique key for this control.</param>
        public UserTileControl(string key) : this(key, null, null) { }

        /// <summary>
        /// Creates a new <see cref="UserTileControl"/> control.
        /// </summary>
        /// <param name="key">The unique key for this control.</param>
        /// <param name="label">The label associated with the control.</param>
        public UserTileControl(string key, string label) : this(key, label, null) { }

        /// <summary>
        /// Creates a new <see cref="UserTileControl"/> control.
        /// </summary>
        /// <param name="key">The unique key for this control.</param>
        /// <param name="label">The label associated with the control.</param>
        /// <param name="bitmap">The bitmap to use as the user's tile image.</param>
        public UserTileControl(string key, string label, Bitmap bitmap) : base(key, label, false, bitmap) { }

        private UserTileControl(UserTileControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            return new UserTileControl(this);
        }
    }
}
