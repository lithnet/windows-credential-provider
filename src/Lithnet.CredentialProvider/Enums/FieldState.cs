namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Specifies the state of a single field in the Credential UI
    /// </summary>
	public enum FieldState
	{
        /// <summary>
        /// Do not show the field in any state. One example of this would be a password edit control that should not be displayed until the user authenticates a thumb print. Until the thumb print has been authenticated, the state of the password field would be CPFS_HIDDEN.
        /// </summary>
		Hidden,

        /// <summary>
        /// Show the field when in the selected state.
        /// </summary>
		DisplayInSelectedTile,

        /// <summary>
        /// Show the field when in the deselected state. This value is only valid for a UsageScenario set to CredUI.
        /// </summary>
		DisplayInDeselectedTile,

        /// <summary>
        /// Show the field both when the credential tile is selected and when it is not selected.
        /// </summary>
		DisplayInBoth
    }
}
