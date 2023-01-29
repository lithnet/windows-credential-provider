using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    internal static class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Lithnet credential provider registration tool");

            var registerCommand = new Command("--register", "Registers the specified credential provider on this system");
            registerCommand.AddAlias("-r");
            var registerArgument = new Argument<FileInfo>("file name", "The name of the file to register");
            registerCommand.AddArgument(registerArgument);
            registerCommand.SetHandler((file) => RegisterAssembly(file), registerArgument);
            rootCommand.Add(registerCommand);


            var unregisterCommand = new Command("--unregister", "Unregisters the specified credential provider on this system");
            unregisterCommand.AddAlias("-u");
            var unregisterArgument = new Argument<FileInfo>("file name", "The name of the file to register");
            unregisterCommand.AddArgument(unregisterArgument);
            unregisterCommand.SetHandler((file) => UnregisterAssembly(file), unregisterArgument);
            rootCommand.Add(unregisterCommand);



            var enableCommand = new Command("--enable", "Enables the specified credential provider on this system");
            enableCommand.AddAlias("-e");
            var enableFileOption = new Option<FileInfo>("file", "The path to the credential provider to enable");

            var enableGuidOption = new Option<Guid>("providerid", "The GUID of the provider to enable");

            enableCommand.AddOption(enableFileOption);
            enableCommand.AddOption(enableGuidOption);
            enableCommand.SetHandler((file) => EnableProviders(file), enableFileOption);
            enableCommand.SetHandler((id) => EnableProviders(id), enableGuidOption);
            rootCommand.Add(enableCommand);


            //var unregisterOption = new Option<FileInfo>(
            //    name: "--unregister",
            //    description: "Unregisters the specified file as a credential provider on this system");

            //var enableOption = new Option<string>(
            //   name: "--enable",
            //   description: "Enables a credential provider on this system");

            //var disableOption = new Option<string>(
            //   name: "--disable",
            //   description: "Disables a credential provider on this system");


            //rootCommand.AddOption(registerOptions);
            //rootCommand.AddOption(unregisterOption);
            //rootCommand.AddOption(enableOption);
            //rootCommand.AddOption(disableOption);


            //rootCommand.SetHandler((file) => RegisterAssembly(file), registerOptions);
            //rootCommand.SetHandler((file) => UnregisterAssembly(file), unregisterOption);
            //rootCommand.SetHandler((file) => EnableProviders(file), enableOption);
            //rootCommand.SetHandler((file) => DisableProviders(file), disableOption);

            return await rootCommand.InvokeAsync(args);
        }

        private static void UnregisterAssembly(FileInfo file)
        {
            var assembly = LoadAssembly(file.FullName);

            foreach (var type in GetCredentialProviders(assembly))
            {
                CredentialProviderRegistrationServices.UnregisterCredentialProvider(type);
                Console.WriteLine($"Unregistered credential provider {type.Name}");
            }
        }

        private static void EnableProviders(FileInfo file)
        {
            var assembly = LoadAssembly(file.FullName);

            foreach (var type in GetCredentialProviders(assembly))
            {
                CredentialProviderRegistrationServices.EnableCredentialProvider(type);
                Console.WriteLine($"Enabled credential provider {type.FullName}");
            }
        }

        private static void EnableProviders(Guid id)
        {
            CredentialProviderRegistrationServices.EnableCredentialProvider(id);
            Console.WriteLine($"Enabled credential provider {id}");
        }

        private static void DisableProviders(FileInfo file)
        {
            var assembly = LoadAssembly(file.FullName);

            foreach (var type in GetCredentialProviders(assembly))
            {
                CredentialProviderRegistrationServices.DisableCredentialProvider(type);
                Console.WriteLine($"Disabled credential provider {type.FullName}");
            }
        }
        private static void DisableProviders(string id)
        {
            if (Guid.TryParse(id, out var providerId))
            {
                CredentialProviderRegistrationServices.DisableCredentialProvider(providerId);
                Console.WriteLine($"Disabled credential provider {providerId}");
            }

            var assembly = LoadAssembly(Path.GetFullPath(id));

            foreach (var type in GetCredentialProviders(assembly))
            {
                CredentialProviderRegistrationServices.DisableCredentialProvider(type);
                Console.WriteLine($"Disabled credential provider {type.FullName}");
            }
        }

        private static void RegisterAssembly(FileInfo file)
        {
            var assembly = LoadAssembly(file.FullName);

            foreach (var type in GetCredentialProviders(assembly))
            {
                CredentialProviderRegistrationServices.RegisterCredentialProvider(type);
                Console.WriteLine($"Registered credential provider {type.FullName}");
            }
        }

        private static IEnumerable<Type> GetCredentialProviders(Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(ifn => ifn.Name == "ICredentialProvider") && !t.IsAbstract && !t.IsInterface);
        }

        private static Assembly LoadAssembly(string assemblyPath)
        {
            string assemblyBasePath = Path.GetDirectoryName(assemblyPath);

            List<string> paths = new List<string>();
            paths.AddRange(Directory.GetFiles(Path.GetDirectoryName(assemblyPath)));
            paths.Add(typeof(object).Assembly.Location);
            paths.AddRange(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll"));

            var resolver = new PathAssemblyResolver(paths);
            MetadataLoadContext mlc = new MetadataLoadContext(resolver);
            return mlc.LoadFromAssemblyPath(assemblyPath);

            //AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (s, args) =>
            //{
            //    var name = new AssemblyName(args.Name);
            //    string assyPath = Path.Combine(assemblyBasePath, $"{name.Name}.dll");
            //    Trace.WriteLine($"Request for {args.Name}");

            //    if (File.Exists(assyPath))
            //    {
            //        Trace.WriteLine($"Found at {assyPath}");
            //        return Assembly.ReflectionOnlyLoadFrom(assyPath);
            //    }

            //    assyPath = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), $"{name.Name}.dll");

            //    if (File.Exists(assyPath))
            //    {
            //        Trace.WriteLine($"Found at {assyPath}");
            //        return Assembly.ReflectionOnlyLoadFrom(assyPath);
            //    }

            //    Trace.WriteLine($"Assembly {args.Name} not found");
            //    return null;

            //};

            //var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
        }
    }
}