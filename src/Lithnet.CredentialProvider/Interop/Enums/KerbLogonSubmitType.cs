namespace Lithnet.CredentialProvider.Interop
{
    internal enum KerbLogonSubmitType : uint
    {
        InteractiveLogon = 2,
        SmartCardLogon = 6,
        WorkstationUnlockLogon = 7,
        SmartCardUnlockLogon = 8,
        ProxyLogon = 9,
        TicketLogon = 10,
        TicketUnlockLogon = 11,
        S4ULogon = 12,
        CertificateLogon = 13,
        CertificateS4ULogon = 14,
        CertificateUnlockLogon = 15,
        NoElevationLogon = 83,
        LuidLogon = 84,
    }
}
