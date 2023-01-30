using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Lithnet.CredentialProvider.ModuleInit
{
    internal static class AssemblyResolver
    {
        private static string basePath;

        [ModuleInitializer]
        public static void AttachResolver()
        {
            Trace.WriteLine($"Loaded assembly {Assembly.GetExecutingAssembly().Location}");
            Trace.WriteLine("Attaching assembly resolver");
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            basePath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Trace.WriteLine("First chance exception in credential provider");
            Trace.WriteLine((e.Exception)?.ToString() ?? "No exception details provided");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("Unhandled exception in credential provider");
            Trace.WriteLine((e.ExceptionObject as Exception)?.ToString() ?? "No exception details provided");
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);
            Trace.WriteLine($"Request for {args.Name}");

#if NETFRAMEWORK
            if (name.Name.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
            {
                string gacPath = $@"C:\Windows\Microsoft.NET\assembly\GAC_MSIL\{name.Name}\v4.0_4.0.0.0__b03f5f7f11d50a3a\{name.Name}.dll";

                if (File.Exists(gacPath))
                {
                    Trace.WriteLine($"System assembly found at {gacPath}");
                    return Assembly.LoadFrom(gacPath);
                }
            }
#endif

            string assyPath = Path.Combine(basePath, $"{name.Name}.dll");


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