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

#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
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