using System;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a control that renders as a clickable text link in the credential UI
    /// </summary>
    public class CommandLinkControl : ControlBase
    {
        /// <summary>
        /// Creates a new <c ref="CommandLinkControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        public CommandLinkControl(string key) : this(key, null) { }

        /// <summary>
        /// Creates a new <c ref="CommandLinkControl"/> control
        /// </summary>
        /// <param name="key">The unique key for this control</param>
        /// <param name="label">The label associated with the control</param>
        public CommandLinkControl(string key, string label) : base(key, label, FieldType.CommandLink) { }

        private CommandLinkControl(CommandLinkControl source) : base(source) { }

        /// <summary>
        /// Gets or sets an action to take when the link is clicked by the user
        /// </summary>
        public Action OnClick { get; set; }

        internal override ControlBase Clone()
        {
            var clone = new CommandLinkControl(this);
            clone.OnClick = this.OnClick;
            return clone;
        }
    }
}