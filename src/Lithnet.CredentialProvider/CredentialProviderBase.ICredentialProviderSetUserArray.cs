using System;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider
{
    public abstract partial class CredentialProviderBase : ICredentialProviderSetUserArray
    {
        int ICredentialProviderSetUserArray.SetUserArray(ICredentialProviderUserArray users)
        {
            try
            {
                this.logger.LogTrace("SetUserArray");

                if (users.GetCount(out uint count) != HRESULT.S_OK)
                {
                    this.logger.LogTrace("ICredentialProviderUserArray.GetCount failed");
                    return HRESULT.S_FALSE;
                }

                this.logger.LogTrace($"SetUserArray called with {count} users");

                this.credentialProviderUsers = users;
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetUserArray failed");
                return HRESULT.E_FAIL;
            }
        }
    }
}