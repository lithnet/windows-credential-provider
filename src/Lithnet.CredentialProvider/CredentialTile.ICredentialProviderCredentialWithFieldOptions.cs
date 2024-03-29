﻿using System;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    public abstract partial class CredentialTile : ICredentialProviderCredentialWithFieldOptions
    {
        int ICredentialProviderCredentialWithFieldOptions.GetFieldOptions(uint dwFieldID, out FieldOptions options)
        {
            options = FieldOptions.None;

            try
            {
                this.logger.LogTrace($"GetFieldOptions: field {dwFieldID}");

                if (this.Controls.TryGetControl(dwFieldID, out var instance))
                {
                    options = instance.Options;

                    this.logger.LogTrace($"GetFieldOptions on [{instance}] returning options {instance.Options}");
                    return HRESULT.S_OK;
                }

                this.logger.LogError($"GetFieldOptions failed to find a field match for {dwFieldID}");

                return HRESULT.E_FAIL;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetFieldOptions failed");
                return HRESULT.E_FAIL;
            }
        }
    }
}
