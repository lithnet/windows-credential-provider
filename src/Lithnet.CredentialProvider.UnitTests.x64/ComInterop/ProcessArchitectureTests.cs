using System;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [TestFixture]
    public class ProcessArchitectureTests
    {
        [Test]
        public void TestHostUsesRequestedArchitecture()
        {
#if TEST_X86
            const string expectedArchitecture = "x86";
#elif TEST_X64
            const string expectedArchitecture = "AMD64";
#elif TEST_ARM64
            const string expectedArchitecture = "ARM64";
#else
#error A test process architecture must be defined by the project.
#endif

            string actualArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");

            Assert.That(actualArchitecture, Is.EqualTo(expectedArchitecture).IgnoreCase);
        }
    }
}
