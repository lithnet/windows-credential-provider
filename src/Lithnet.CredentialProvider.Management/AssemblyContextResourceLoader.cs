using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    internal static class AssemblyContextResourceLoader
    {
        private static readonly DependencyAssemblyLoadContext dependencyLoadContext = new DependencyAssemblyLoadContext();

        public static Assembly LoadIntoAlc(Stream stream)
        {
            return dependencyLoadContext.LoadFromStream(stream);
        }

        private class DependencyAssemblyLoadContext : AssemblyLoadContext
        {
            protected override Assembly Load(AssemblyName assemblyName)
            {
                return null;
            }
        }
    }
}
