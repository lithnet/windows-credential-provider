using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    // The in-process call also requires the callback to implement the parameter's managed interface type.
    // ITestCredentialProviderUserArray keeps the COM contract under test independent of that adapter.
    internal sealed class TestCredentialProviderUserArray : ITestCredentialProviderUserArray, ICredentialProviderUserArray
    {
        public int GetCountCallCount { get; private set; }

        public int SetProviderFilter(ref Guid providerToFilterTo)
        {
            return CredentialProviderAbi.E_NOTIMPL;
        }

        public int GetAccountOptions(out int accountOptions)
        {
            accountOptions = 0;
            return CredentialProviderAbi.S_OK;
        }

        public int GetCount(out uint userCount)
        {
            this.GetCountCallCount++;
            userCount = 0;
            return CredentialProviderAbi.S_OK;
        }

        public int GetAt(uint userIndex, out IntPtr user)
        {
            user = IntPtr.Zero;
            return CredentialProviderAbi.E_INVALIDARG;
        }

        int ICredentialProviderUserArray.GetAccountOptions(out AccountOptions accountOptions)
        {
            accountOptions = AccountOptions.None;
            return CredentialProviderAbi.S_OK;
        }

        int ICredentialProviderUserArray.GetAt(uint userIndex, out ICredentialProviderUser user)
        {
            user = null;
            return CredentialProviderAbi.E_INVALIDARG;
        }
    }
}
