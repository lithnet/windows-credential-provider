using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.Loader;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    public class ModuleInitializer : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        public static bool IsFullFramework => typeof(object).Assembly.FullName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase);
        private Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        public void OnImport()
        {
            Trace.WriteLine($"Initializing PowerShell module loaded in {(IsFullFramework ? "netfx" : "netcore")}");

            this.PreloadAssemblies();
            this.HookResolvers();
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            this.UnhookResolvers();
        }

        private void PreloadAssemblies()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            foreach (string resource in executingAssembly.GetManifestResourceNames().Where(n => n.EndsWith(".dll")))
            {
                using (var stream = executingAssembly.GetManifestResourceStream(resource))
                {
                    if (stream == null)
                    {
                        continue;
                    }

                    try
                    {
                        Trace.WriteLine("Preloading assembly: " + resource);

                        if (IsFullFramework)
                        {
                            var bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            this.assemblies.Add(resource, Assembly.Load(bytes));
                        }
                        else
                        {
                            this.assemblies.Add(resource, AssemblyContextResourceLoader.LoadIntoAlc(stream));
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Failed to load: {0}\r\n", resource, ex.ToString());
                    }
                }
            }
        }

        private void HookResolvers()
        {
            AppDomain.CurrentDomain.AssemblyResolve += this.ResolveAssembly;

            if (!IsFullFramework)
            {
                this.HookAssemblyLoadContextResolver();
            }
        }

        private void UnhookResolvers()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= this.ResolveAssembly;

            if (!IsFullFramework)
            {
                this.UnhookAssemblyLoadContextResolver();
            }
        }

        private void HookAssemblyLoadContextResolver()
        {
            AssemblyLoadContext.Default.Resolving += this.ResolveAssembly;
        }

        private void UnhookAssemblyLoadContextResolver()
        {
            AssemblyLoadContext.Default.Resolving += this.ResolveAssembly;
        }

        private Assembly ResolveAssembly(object s, ResolveEventArgs e)
        {
            var assemblyName = new AssemblyName(e.Name);
            return this.ResolveAssemblyFromCache(assemblyName);
        }

        private Assembly ResolveAssembly(AssemblyLoadContext defaultAlc, AssemblyName assemblyName)
        {
            return this.ResolveAssemblyFromCache(assemblyName);
        }

        private Assembly ResolveAssemblyFromCache(AssemblyName assemblyName)
        {
            var path = string.Format("{0}.dll", assemblyName.Name);

            if (this.assemblies.ContainsKey(path))
            {
                return this.assemblies[path];
            }

            return null;
        }
    }
}
