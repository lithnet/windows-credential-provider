using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Provides customization options for a single field in a logon or credential UI
    /// </summary>
	[Flags]
    public enum FieldOptions
    {
        /// <summary>
        /// Default. Don't show the "password reveal" glyph, and use the standard on-screen keyboard layout.
        /// </summary>
		None = 0,

        /// <summary>
        /// Display the "password reveal" glyph in a password entry box. When this glyph is held down by the user, the entry in the password box is shown in plain text. The glyph is shown here:
        /// </summary>
		PasswordReveal = 1,

        /// <summary>
        /// The field will contain an e-mail address. The on-screen keyboard should be optimized for that input (showing the .com and @ keys on the primary keyboard layout). This option is used with Microsoft account credentials.
        /// </summary>
		Email = 2,

        /// <summary>
        /// When enabled, the touch keyboard will be automatically invoked. This should be set only on the CPFG_CREDENTIAL_PROVIDER_LOGO field.
        /// </summary>
		TouchKeyboardAutoInvoke = 4,

        /// <summary>
        /// The field will only allow numerals to be entered. The on-screen keyboard should be optimized for that input (showing only a number keypad on the primary keyboard layout). This should be set only on the CPFT_PASSWORD_TEXT field
        /// </summary>
		NumbersOnly = 8,

        /// <summary>
        /// Show the English keyboard.
        /// </summary>
		ShowEnglishKeyboard = 16
    }
}
