using System;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal static class CredentialProviderAbi
    {
        public static readonly Guid ICredentialProvider = new Guid("D27C3481-5A1C-45B2-8AAA-C20EBBE8229E");

        public static readonly Guid ICredentialProviderCredential = new Guid("63913A93-40C1-481A-818D-4072FF8C70CC");

        public static readonly Guid ICredentialProviderCredential2 = new Guid("FD672C54-40EA-4D6E-9B49-CFB1A7507BD7");

        public static readonly Guid ICredentialProviderSetUserArray = new Guid("095C1484-1C0C-4388-9C6D-500E61BF84BD");

        public static readonly Guid ICredentialProviderUserArray = new Guid("90C119AE-0F18-4520-A1F1-114366A40FE8");

        public const int SetUsageScenarioSlot = 3;

        public const int GetFieldDescriptorCountSlot = 7;

        public const int GetFieldDescriptorAtSlot = 8;

        public const int GetCredentialCountSlot = 9;

        public const int GetCredentialAtSlot = 10;

        public const int SetUserArraySlot = 3;

        public const int UserArrayGetCountSlot = 5;

        public const int SetSelectedSlot = 5;

        public const int SetDeselectedSlot = 6;

        public const int GetFieldStateSlot = 7;

        public const int GetStringValueSlot = 8;

        public const int GetUserSidSlot = 20;

        public const int SmallTextFieldType = 2;

        public const int DisplayInSelectedTileFieldState = 1;

        public const int NoInteractiveFieldState = 0;

        public const uint NoDefaultCredential = 0xFFFFFFFF;

        public const int S_OK = 0;

        public const int S_FALSE = 1;

        public const int E_FAIL = unchecked((int)0x80004005);

        public const int E_INVALIDARG = unchecked((int)0x80070057);

        public const int E_NOTIMPL = unchecked((int)0x80004001);
    }
}
