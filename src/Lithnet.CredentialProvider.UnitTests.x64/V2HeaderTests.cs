using System;
using Lithnet.CredentialProvider.Interop;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests
{
    public class V2HeaderTests
    {

        [SetUp]
        public void Setup()
        {
            ConsentUIData.HeaderSize = ConsentUIData.v2HeaderSize;
        }

        [Test]
        public void TestExe()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V2\\dump-consent-exe-x64.dat");

            ConsentUIDataExe d = new ConsentUIDataExe(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Exe, d.Type);
            Assert.AreEqual(ConsentUIFlags.InWindowsDirectory | ConsentUIFlags.SecureDesktop | ConsentUIFlags.Unknown3, d.Flags);
            Assert.AreEqual(3, d.SessionId);
            Assert.AreEqual((IntPtr)0x0, d.HWnd); // this seems odd
            Assert.AreEqual((ConsentUIElevationType)6, d.ElevationType);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", d.ExecutablePath);
            Assert.AreEqual("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", d.Unknown1);
            Assert.AreEqual("\"C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe\" ", d.CommandLine);
            Assert.AreEqual("", d.Unknown2);
        }

        [Test]
        public void TestMsi()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V2\\dump-consent-msi-x64.dat");

            var d = new ConsentUIDataMsi(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Msi, d.Type);
            Assert.AreEqual(ConsentUIFlags.InWindowsDirectory | ConsentUIFlags.SecureDesktop | ConsentUIFlags.AutoElevationOther, d.Flags);
            Assert.AreEqual(2, d.SessionId);
            Assert.AreEqual((IntPtr)0x00000000001e033a, d.HWnd);
            Assert.AreEqual((ConsentUIElevationType)5, d.ElevationType);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual("7-Zip 24.08 (x64 edition)", d.ProductName);
            Assert.AreEqual(ConsentUIMsiAction.Install, d.Action);
            Assert.AreEqual("C:\\WINDOWS\\Installer\\58ca98.msi", d.ExecutionPath);
            Assert.AreEqual("1033", d.Locale);
            Assert.AreEqual("C:\\Users\\testuser\\Downloads\\7z2408-x64.msi", d.OriginalMsi);
            Assert.AreEqual("7-Zip 24.08 (x64 edition)", d.ProductName);
            Assert.AreEqual("Igor Pavlov", d.Publisher);
            Assert.AreEqual("24.08.00.0", d.Version);
        }

        [Test]
        public void TestMsix()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V2\\dump-consent-msix-x64.dat");

            var d = new ConsentUIDataMsix(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Msix, d.Type);
            Assert.AreEqual(ConsentUIFlags.SecureDesktop | ConsentUIFlags.Unknown3, d.Flags);
            Assert.AreEqual(2, d.SessionId);
            Assert.AreEqual((IntPtr)0x0, d.HWnd);
            Assert.AreEqual((ConsentUIElevationType)8, d.ElevationType);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual("\"C:\\Program Files\\WindowsApps\\Microsoft.MSIXPackagingTool_1.2024.405.0_x64__8wekyb3d8bbwe\\MsixPackageTool.exe\" ", d.CommandLine);
            Assert.AreEqual("C:\\Program Files\\WindowsApps\\Microsoft.MSIXPackagingTool_1.2024.405.0_x64__8wekyb3d8bbwe\\MsixPackageTool.exe", d.ExecutablePath);
            Assert.AreEqual("Microsoft.MSIXPackagingTool_8wekyb3d8bbwe!Msix.App", d.OtherName);
            Assert.AreEqual("Microsoft.MSIXPackagingTool_8wekyb3d8bbwe", d.PackageName);
        }

        [Test]
        public void TestCom()
        {
            SafeHGlobalHandle p = Helpers.ReadFile($"Samples\\V2\\dump-consent-com-x64.dat");

            var d = new ConsentUIDataCom(p.ToIntPtr(), p.Size);

            Assert.AreEqual(ConsentUIType.Com, d.Type);
            Assert.AreEqual(ConsentUIFlags.SecureDesktop | ConsentUIFlags.AutoElevationOther | ConsentUIFlags.InWindowsDirectory, d.Flags);
            Assert.AreEqual(2, d.SessionId);
            Assert.AreEqual((IntPtr)0x00000000000d021e, d.HWnd);
            Assert.AreEqual((ConsentUIElevationType)4, d.ElevationType);
            Assert.AreEqual(ConsentUIPromptType.Credentials, d.PromptType);
            Assert.AreEqual(new Guid("{3ad05575-8857-4850-9277-11b85bdb8e09}"), d.ClsId);
            Assert.AreEqual("C:\\WINDOWS\\system32\\windows.storage.dll", d.ComComponentPath);
            Assert.AreEqual("", d.ImageResourcePath);
            Assert.AreEqual("File Operation", d.OperationType);
            Assert.AreEqual("C:\\WINDOWS\\Explorer.EXE", d.ProcessPath);
        }
    }
}