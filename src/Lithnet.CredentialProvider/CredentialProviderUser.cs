using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a user provided by the credential framework
    /// </summary>
    public class CredentialProviderUser
    {
        internal readonly ICredentialProviderUser User;
        private readonly ILogger logger;
        private string qualifiedUserName;
        private string sid;
        private string userName;
        private string displayName;
        private string logonStatus;
        private string providerId;

        internal CredentialProviderUser(ILoggerFactory loggerFactory, ICredentialProviderUser user)
        {
            this.User = user;
            this.logger = loggerFactory.CreateLogger<CredentialProviderUser>();
        }

        /// <summary>
        /// Gets the qualified username of the user. This name is used to pack an authentication buffer
        /// </summary>
        public string QualifiedUserName
        {
            get
            {
                if (this.qualifiedUserName == null)
                {
                    this.qualifiedUserName = this.TryGetValueOrDefault(PropertyKeys.PKEY_Identity_QualifiedUserName);
                }

                return this.qualifiedUserName;
            }
        }

        /// <summary>
        /// Gets the SID of the user
        /// </summary>
        public string Sid
        {
            get
            {
                if (this.sid == null)
                {
                    this.sid = this.TryGetValueOrDefault(PropertyKeys.PKEY_Identity_PrimarySid);
                }

                return this.sid;
            }
        }

        /// <summary>
        /// Gets the username of the user
        /// </summary>
        public string UserName
        {
            get
            {
                if (this.userName == null)
                {
                    this.userName = this.TryGetValueOrDefault(PropertyKeys.PKEY_Identity_UserName);
                }

                return this.userName;
            }
        }

        /// <summary>
        /// Gets the display name of the user
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (this.displayName == null)
                {
                    this.displayName = this.TryGetValueOrDefault(PropertyKeys.PKEY_Identity_DisplayName);
                }

                return this.displayName;
            }
        }

        /// <summary>
        /// Gets the logon status of the user. Does not apply to the CredUI scenario.
        /// </summary>
        /// <remarks>For example, "Signed-in", "Locked"</remarks>
        public string LogonStatus
        {
            get
            {
                if (this.logonStatus == null)
                {
                    this.logonStatus = this.TryGetValueOrDefault(PropertyKeys.PKEY_Identity_LogonStatusString);
                }

                return this.logonStatus;
            }
        }

        /// <summary>
        /// The user's provider ID
        /// </summary>
        public string ProviderID
        {
            get
            {
                if (this.providerId == null)
                {
                    this.providerId = this.TryGetValueOrDefault(PropertyKeys.PKEY_Identity_ProviderID);
                }

                return this.providerId;
            }
        }

        private string GetValue(PropertyKey key)
        {
            var result = this.User.GetStringValue(key, out var value);

            if (result != HRESULT.S_OK)
            {
                throw new COMException("Could not get user SID", result);
            }

            return value;
        }

        private string TryGetValueOrDefault(PropertyKey key)
        {
            try
            {
                return this.GetValue(key);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Unable to get property value for key {key.PropertyID}");
            }

            return null;
        }
    }
}
