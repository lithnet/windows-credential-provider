using System;
using Lithnet.CredentialProvider.Interop;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests
{
    public class V1HeaderTests
    {
        [SetUp]
        public void Setup()
        {
            ConsentUIData.HeaderSize = ConsentUIData.v1HeaderSize;
        }

        [Test]
        public void TestExe()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V1\\dump-consent-exe-x64.dat");

            var d = new ConsentUIDataExe(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Exe, d.Type);
            Assert.AreEqual(ConsentUIFlags.InWindowsDirectory | ConsentUIFlags.SecureDesktop | ConsentUIFlags.Unknown3, d.Flags);
            Assert.AreEqual(3, d.SessionId);
            Assert.AreEqual((IntPtr)0x0000000000010128, d.HWnd);
            Assert.AreEqual(ConsentUIElevationReason.Request, d.ElevationReason);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", d.ExecutablePath);
            Assert.AreEqual("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", d.Unknown1);
            Assert.AreEqual("\"C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe\" ARGS1 ARGS2", d.CommandLine);
            Assert.AreEqual("", d.Unknown2);
        }

        [Test]
        public void TestMsi()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V1\\dump-consent-msi-x64.dat");

            var d = new ConsentUIDataMsi(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Msi, d.Type);
            Assert.AreEqual(ConsentUIFlags.InWindowsDirectory | ConsentUIFlags.SecureDesktop | ConsentUIFlags.AutoElevationOther, d.Flags);
            Assert.AreEqual(3, d.SessionId);
            Assert.AreEqual((IntPtr)0x0000000000230370, d.HWnd);
            Assert.AreEqual(ConsentUIElevationReason.Msi, d.ElevationReason);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual("7-Zip 23.01 (x64 edition)", d.ProductName);
            Assert.AreEqual(ConsentUIMsiAction.Install, d.Action);
            Assert.AreEqual("C:\\Windows\\Installer\\b733392.msi", d.ExecutionPath);
            Assert.AreEqual("1033", d.Locale);
            Assert.AreEqual("C:\\Users\\ryan2\\Downloads\\7z2301-x64.msi", d.OriginalMsi);
            Assert.AreEqual("7-Zip 23.01 (x64 edition)", d.ProductName);
            Assert.AreEqual("Igor Pavlov", d.Publisher);
            Assert.AreEqual("23.01.00.0", d.Version);
        }

        [Test]
        public void TestMsix()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V1\\dump-consent-msix-x64.dat");

            var d = new ConsentUIDataMsix(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Msix, d.Type);
            Assert.AreEqual( ConsentUIFlags.SecureDesktop | ConsentUIFlags.Unknown3, d.Flags);
            Assert.AreEqual(3, d.SessionId);
            Assert.AreEqual((IntPtr)0x0, d.HWnd);
            Assert.AreEqual(ConsentUIElevationReason.PackagedApp, d.ElevationReason);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual("\"C:\\Program Files\\WindowsApps\\Microsoft.MSIXPackagingTool_1.2023.807.0_x64__8wekyb3d8bbwe\\MsixPackageTool.exe\" ", d.CommandLine);
            Assert.AreEqual("C:\\Program Files\\WindowsApps\\Microsoft.MSIXPackagingTool_1.2023.807.0_x64__8wekyb3d8bbwe\\MsixPackageTool.exe", d.ExecutablePath);
            Assert.AreEqual("Microsoft.MSIXPackagingTool_8wekyb3d8bbwe!Msix.App", d.OtherName);
            Assert.AreEqual("Microsoft.MSIXPackagingTool_8wekyb3d8bbwe", d.PackageName);
        }

        [Test]
        public void TestCom()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V1\\dump-consent-com-x64.dat");

            var d = new ConsentUIDataCom(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Com, d.Type);
            Assert.AreEqual(ConsentUIFlags.SecureDesktop | ConsentUIFlags.AutoElevationOther | ConsentUIFlags.InWindowsDirectory, d.Flags);
            Assert.AreEqual(3, d.SessionId);
            Assert.AreEqual((IntPtr)0x00000000000903c2, d.HWnd);
            Assert.AreEqual(ConsentUIElevationReason.CLSID, d.ElevationReason);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual(new Guid("{3ad05575-8857-4850-9277-11b85bdb8e09}"), d.ClsId);
            Assert.AreEqual("C:\\Windows\\system32\\windows.storage.dll", d.ComComponentPath);
            Assert.AreEqual("", d.ImageResourcePath);
            Assert.AreEqual("File Operation", d.OperationType);
            Assert.AreEqual("C:\\Windows\\Explorer.EXE", d.ProcessPath);
        }
    }
}