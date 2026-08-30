using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal sealed class CredentialCallbackScenario : IDisposable
    {
        private readonly TestCredentialProviderUserArray users;
        private readonly ComInterfacePointer credential;
        private readonly CredentialAdviseDelegate advise;
        private readonly CredentialUnAdviseDelegate unAdvise;
        private bool advised;
        private bool disposed;

        public CredentialCallbackScenario(int maximumEventsVersion)
        {
            this.Provider = new AbiTestCredentialProvider3();
            this.users = new TestCredentialProviderUserArray();
            this.Events = new RawCredentialEvents(maximumEventsVersion);

            try
            {
                this.InitializeProvider();
                this.credential = this.CreateCredentialInterface();
                this.advise = this.credential.GetMethod<CredentialAdviseDelegate>(RawCredentialEventAbi.CredentialAdviseSlot);
                this.unAdvise = this.credential.GetMethod<CredentialUnAdviseDelegate>(RawCredentialEventAbi.CredentialUnAdviseSlot);
            }
            catch
            {
                this.credential?.Dispose();
                this.Events.Dispose();
                throw;
            }
        }

        public AbiTestCredentialProvider3 Provider { get; }

        public RawCredentialEvents Events { get; }

        public int Advise()
        {
            this.ThrowIfDisposed();
            int hresult = this.advise(this.credential.Value, this.Events.Events1Interface);
            if (hresult == CredentialProviderAbi.S_OK)
            {
                this.advised = true;
            }

            return hresult;
        }

        public int UnAdvise()
        {
            this.ThrowIfDisposed();
            int hresult = this.unAdvise(this.credential.Value);
            if (hresult == CredentialProviderAbi.S_OK)
            {
                this.advised = false;
            }

            return hresult;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            if (this.advised)
            {
                this.unAdvise(this.credential.Value);
                this.advised = false;
            }

            this.credential.Dispose();
            this.Events.Dispose();
            this.disposed = true;
            GC.KeepAlive(this.users);
            GC.KeepAlive(this.Provider);
        }

        private void InitializeProvider()
        {
            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(this.Provider, CredentialProviderAbi.ICredentialProvider))
            {
                SetUsageScenarioDelegate setUsageScenario = providerInterface.GetMethod<SetUsageScenarioDelegate>(CredentialProviderAbi.SetUsageScenarioSlot);
                this.ThrowIfFailed(setUsageScenario(providerInterface.Value, (int)UsageScenario.CredUI, 0), "SetUsageScenario");
            }

            using (ComInterfacePointer setUserArrayInterface = ComInterfacePointer.Create(this.Provider, CredentialProviderAbi.ICredentialProviderSetUserArray))
            using (ComInterfacePointer userArrayInterface = ComInterfacePointer.Create(this.users, CredentialProviderAbi.ICredentialProviderUserArray))
            {
                SetUserArrayDelegate setUserArray = setUserArrayInterface.GetMethod<SetUserArrayDelegate>(CredentialProviderAbi.SetUserArraySlot);
                this.ThrowIfFailed(setUserArray(setUserArrayInterface.Value, userArrayInterface.Value), "SetUserArray");
            }
        }

        private ComInterfacePointer CreateCredentialInterface()
        {
            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(this.Provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetCredentialCountDelegate getCredentialCount = providerInterface.GetMethod<GetCredentialCountDelegate>(CredentialProviderAbi.GetCredentialCountSlot);
                GetCredentialAtDelegate getCredentialAt = providerInterface.GetMethod<GetCredentialAtDelegate>(CredentialProviderAbi.GetCredentialAtSlot);

                this.ThrowIfFailed(getCredentialCount(providerInterface.Value, out uint count, out uint defaultCredential, out int autoLogonWithDefault), "GetCredentialCount");
                if (count != 1)
                {
                    throw new InvalidOperationException($"The callback ABI provider returned {count} credentials instead of one");
                }

                IntPtr credentialPointer = IntPtr.Zero;

                try
                {
                    this.ThrowIfFailed(getCredentialAt(providerInterface.Value, 0, out credentialPointer), "GetCredentialAt");
                    ComInterfacePointer result = ComInterfacePointer.TakeOwnership(credentialPointer);
                    credentialPointer = IntPtr.Zero;
                    return result;
                }
                finally
                {
                    if (credentialPointer != IntPtr.Zero)
                    {
                        Marshal.Release(credentialPointer);
                    }
                }
            }
        }

        private void ThrowIfFailed(int hresult, string operation)
        {
            if (hresult != CredentialProviderAbi.S_OK)
            {
                throw new COMException($"{operation} failed", hresult);
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(CredentialCallbackScenario));
            }
        }
    }
}
