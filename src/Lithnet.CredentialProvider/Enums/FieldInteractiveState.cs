namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Describes the state of a field and how it a user can interact with it. Fields can be displayed by a credential provider in a variety of different interactive states.
    /// </summary>
	public enum FieldInteractiveState
	{
        /// <summary>
        /// The field can be edited if the field type supports editing. It also contains none of the other available interactive states.
        /// </summary>
		None,

        /// <summary>
        /// Reserved and not used.
        /// </summary>
		ReadOnly,

        /// <summary>
        /// The field is disabled. The user can see it but not interact with it. This support was added starting with Windows 10.
        /// </summary>
		Disabled,

        /// <summary>
        /// Credential providers use this field interactive state to indicate that the field should receive initial keyboard focus. This interactive state may not be specified for field types that the user cannot edit. If several editable fields specify this state, the last of them based on dwIndex order receives focus. On systems before Windows 10, it was the first of editable fields based on dwIndex order. This field interactive state is obeyed only during initial enumeration.
        /// </summary>
		Focused
    }
}
