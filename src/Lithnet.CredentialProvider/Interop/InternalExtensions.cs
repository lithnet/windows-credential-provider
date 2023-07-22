using System;
using System.Diagnostics;
using System.Security;

namespace Lithnet.CredentialProvider.Interop
{
    internal static class InternalExtensions
    {
        [Conditional("DEBUG")]
        internal static void LogWarningDebug(this ILogger logger, string message)
        {
            logger.LogWarning(message);
        }

        internal static unsafe int Wcslen(this IntPtr addr)
        {
            const int maxLength = int.MaxValue;

            var c = (char*)(addr);

            for (int i = 0; i < maxLength; i++)
            {
                if (c[i] == '\0')
                {
                    return i;
                }
            }

            throw new ArgumentException("End of string not found");
        }

        internal static unsafe SecureString IntPtrToSecureString(this IntPtr psz)
        {
            var length = psz.Wcslen();
            var charArray = (char*)psz;
            return new SecureString(charArray, length);
        }

        internal static string GetUserName(this ICredentialProviderUser user)
        {
            user.GetStringValue(PropertyKeys.PKEY_Identity_UserName, out var value);
            return value;
        }

        internal static string GetQualifiedUserName(this ICredentialProviderUser user)
        {
            user.GetStringValue(PropertyKeys.PKEY_Identity_QualifiedUserName, out var value);
            return value;
        }

        internal static string GetDisplayName(this ICredentialProviderUser user)
        {
            user.GetStringValue(PropertyKeys.PKEY_Identity_DisplayName, out var value);
            return value;
        }

        internal static string GetLogonStatus(this ICredentialProviderUser user)
        {
            user.GetStringValue(PropertyKeys.PKEY_Identity_LogonStatusString, out var value);
            return value;
        }

        internal static string GetPrimarySid(this ICredentialProviderUser user)
        {
            user.GetStringValue(PropertyKeys.PKEY_Identity_PrimarySid, out var value);
            return value;
        }

        internal static string GetProviderID(this ICredentialProviderUser user)
        {
            user.GetStringValue(PropertyKeys.PKEY_Identity_ProviderID, out var value);
            return value;
        }
    }
}
