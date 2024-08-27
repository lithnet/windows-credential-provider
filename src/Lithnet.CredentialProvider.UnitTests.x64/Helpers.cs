using System.IO;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests
{
    internal static class Helpers
    {
        public static SafeHGlobalHandle ReadFile(string filename)
        {
            var data = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, filename));
            var p = SafeHGlobalHandle.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, p.ToIntPtr(), data.Length);

            return p;
        }
    }
}
