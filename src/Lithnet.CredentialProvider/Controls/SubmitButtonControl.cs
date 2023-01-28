using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// The submit button for the login form. This button is ignored when the credential provider is invoked via CredUI. Note, there can only be one submit button.
    /// </summary>
    public class SubmitButtonControl : ControlBase
    {
        /// <summary>
        /// Creates a new <c ref="SubmitButtonControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="adjacentToControl">The control that the submit button should appear adjacent to</param>
        public SubmitButtonControl(string key, ControlBase adjacentToControl) : this(key, null, adjacentToControl) { }

        /// <summary>
        /// Creates a new <c ref="SubmitButtonControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        /// <param name="adjacentToControl">The control that the submit button should appear adjacent to</param>
        public SubmitButtonControl(string key, string label, ControlBase adjacentToControl) : base(key, label, FieldType.Submit)
        {
            this.AdjacentToId = adjacentToControl.Id;
            this.AdjacentToKey = adjacentToControl.Key;
            this.State = FieldState.DisplayInSelectedTile;
        }

        /// <summary>
        /// Gets the unique ID of the control that the button is adjacent to
        /// </summary>
        public string AdjacentToKey { get; }

        internal uint AdjacentToId { get; private set; }

        private SubmitButtonControl(SubmitButtonControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            var clone = new SubmitButtonControl(this);
            clone.AdjacentToId = this.AdjacentToId;

            return clone;
        }
    }
}