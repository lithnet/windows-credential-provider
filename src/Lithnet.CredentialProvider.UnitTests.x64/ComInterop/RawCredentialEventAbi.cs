using System;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal static class RawCredentialEventAbi
    {
        public static readonly Guid IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        public static readonly Guid ICredentialProviderCredentialEvents = new Guid("FA6FA76B-66B7-4B11-95F1-86171118E816");

        public static readonly Guid ICredentialProviderCredentialEvents2 = new Guid("B53C00B6-9922-4B78-B1F4-DDFE774DC39B");

        public static readonly Guid ICredentialProviderCredentialEvents3 = new Guid("2D8DEEB8-1322-4973-8DF9-B282F2468290");

        public const int CredentialAdviseSlot = 3;

        public const int CredentialUnAdviseSlot = 4;

        public const int SetFieldStringSlot = 5;

        public const int BeginFieldUpdatesSlot = 13;

        public const int EndFieldUpdatesSlot = 14;

        public const int SetFieldOptionsSlot = 15;

        public const int SetFieldBitmapBufferSlot = 16;

        public const int Events1SlotCount = 13;

        public const int Events2SlotCount = 16;

        public const int Events3SlotCount = 17;

        public const int E_NOINTERFACE = unchecked((int)0x80004002);

        public const int E_POINTER = unchecked((int)0x80004003);
    }
}
