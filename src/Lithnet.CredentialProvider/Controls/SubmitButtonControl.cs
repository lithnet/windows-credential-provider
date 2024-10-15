using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// The submit button for the login form. This button is ignored when the credential provider is invoked via CredUI. Note, there can only be one submit button.
    /// </summary>
    public class SubmitButtonControl : ControlBase
    {
        private ControlBase adjacentToControl;

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
            this.State = FieldState.DisplayInSelectedTile;
            this.adjacentToControl = adjacentToControl;
        }

        /// <summary>
        /// Gets or sets the control that the button is adjacent to
        /// </summary>
        public ControlBase AdjacentToControl
        {
            get
            {
                return this.adjacentToControl;
            }
            set
            {
                this.adjacentToControl = value;
                this.Events?.SetFieldSubmitButton(this.Credential, this.Id, this.adjacentToControl.Id);
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the unique ID of the control that the button is adjacent to
        /// </summary>
        public string AdjacentToKey => this.adjacentToControl?.Key;

        internal uint AdjacentToId => this.adjacentToControl.Id;

        private SubmitButtonControl(SubmitButtonControl source) : base(source) { }

        internal override ControlBase Clone()
        {
            var clone = new SubmitButtonControl(this);
            clone.adjacentToControl = this.adjacentToControl;

            return clone;
        }
    }
}