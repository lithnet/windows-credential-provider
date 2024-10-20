﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    public abstract partial class CredentialProviderBase : ICredentialProvider
    {
        private const uint CREDENTIAL_PROVIDER_NO_DEFAULT = 0xFFFFFFFF;

        int ICredentialProvider.SetUsageScenario(UsageScenario cpus, CredUIWinFlags dwFlags)
        {
            try
            {
                this.logger.LogTrace($"SetUsageScenario: Usage: {cpus} flags: {dwFlags}");

                this.UsageScenario = cpus;
                this.CredUIFlags = dwFlags;

                if (this.IsUsageScenarioSupported(cpus, (CredUIWinFlags)dwFlags))
                {
                    return HRESULT.S_OK;
                }
                else
                {
                    return HRESULT.E_NOTIMPL;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetUsageScenario failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.SetSerialization(ref CredentialSerialization pcpcs)
        {
            try
            {
                this.InboundSerialization = pcpcs;
                this.OnSetSerialization(pcpcs);
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SetSerialization failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.Advise(ICredentialProviderEvents pcpe, IntPtr upAdviseContext)
        {
            try
            {
                this.logger.LogTrace($"Advise: {upAdviseContext}");
                if (pcpe != null)
                {
                    this.credentialProviderEventsAdviseContext = upAdviseContext;
                    this.CredentialProviderEvents = pcpe;
                    var intPtr = Marshal.GetIUnknownForObject(pcpe);
                    Marshal.AddRef(intPtr);
                }

                this.OnLoad();

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Advise failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.UnAdvise()
        {
            try
            {
                this.logger.LogTrace($"Unadvise");

                if (this.CredentialProviderEvents != null)
                {
                    var intPtr = Marshal.GetIUnknownForObject(this.CredentialProviderEvents);
                    Marshal.Release(intPtr);
                    this.CredentialProviderEvents = null;
                    this.credentialProviderEventsAdviseContext = IntPtr.Zero;
                }

                this.OnUnload();

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "UnAdvise failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.GetFieldDescriptorCount(out uint pdwCount)
        {
            pdwCount = 0;

            try
            {
                this.logger.LogTrace("GetFieldDescriptorCount");
                this.BuildControls();
                this.Controls.Lock();

                pdwCount = (uint)this.Controls.Count;
                this.logger.LogTrace($"GetFieldDescriptorCount return {pdwCount}");

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetFieldDescriptorCount failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.GetFieldDescriptorAt(uint dwIndex, out IntPtr ppcpfd)
        {
            ppcpfd = IntPtr.Zero;

            try
            {
                this.logger.LogTrace($"GetFieldDescriptorAt {dwIndex}");

                if (dwIndex >= this.Controls.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                var item = this.Controls[(int)dwIndex].GetDescriptor();
                var size = Marshal.SizeOf<FieldDescriptor>();
                var ptr = Marshal.AllocCoTaskMem(size);
                Marshal.StructureToPtr<FieldDescriptor>(item, ptr, false);
                ppcpfd = ptr;

                this.logger.LogTrace($"GetFieldDescriptorAt {dwIndex}: returning {item}");

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetFieldDescriptorAt failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.GetCredentialCount(out uint pdwCount, out uint pdwDefault, out int pbAutoLogonWithDefault)
        {
            pdwCount = 0;
            pdwDefault = CREDENTIAL_PROVIDER_NO_DEFAULT;
            pbAutoLogonWithDefault = 0;

            try
            {
                this.logger.LogTrace($"GetCredentialCount");

                if (this.Tiles == null)
                {
                    this.logger.LogTrace($"GetCredentialCount is enumerating tiles");
                    this.SetupTiles();
                    this.logger.LogTrace($"{this.Tiles.Count} tiles enumerated. {this.Tiles.Count(t => t.IsGenericTile)} generic and {this.tiles.Count(t => !t.IsGenericTile)} personalized");
                }

                this.notifyOnTileCollectionChange = true;

                var defaultTile = this.DefaultTile;

                if (defaultTile != null)
                {
                    var index = this.tiles.IndexOf(defaultTile);

                    if (index >= 0)
                    {
                        pdwDefault = (uint)index;
                        pbAutoLogonWithDefault = this.DefaultTileAutoLogon ? 1 : 0;
                    }
                }

                pdwCount = (uint)this.Tiles.Count;
                this.logger.LogTrace($"GetCredentialCount returning pdwCount: {pdwCount}, pdwDefault: {pdwDefault}, pbAutoLogonWithDefault: {pbAutoLogonWithDefault}");

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetCredentialCount failed");
                return HRESULT.E_FAIL;
            }
        }

        int ICredentialProvider.GetCredentialAt(uint dwIndex, out ICredentialProviderCredential ppcpc)
        {
            ppcpc = null;

            try
            {
                this.logger.LogTrace($"GetCredentialAt {dwIndex}");

                var tile = this.Tiles[(int)dwIndex];

                ppcpc = (ICredentialProviderCredential)tile;
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "GetCredentialAt failed");
                return HRESULT.E_FAIL;
            }
        }
    }
}
