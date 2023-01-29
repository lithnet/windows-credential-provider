using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lithnet.CredentialProvider.ModuleInit
{
    internal static class AssemblyResolver
    {
        private static string basePath;

        [ModuleInitializer]
        public static void AttachResolver()
        {
            Trace.WriteLine("Attaching assembly resolver");
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            basePath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);
            string assyPath = Path.Combine(basePath, $"{name.Name}.dll");
            Trace.WriteLine($"Request for {args.Name}");

            if (File.Exists(assyPath))
            {
                Trace.WriteLine($"Found at {assyPath}");
                return Assembly.Load(assyPath);
            }

            Trace.WriteLine($"Assembly {args.Name} not found");
            return null;
        }
    }
}