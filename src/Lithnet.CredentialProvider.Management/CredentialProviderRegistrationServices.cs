using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Lithnet.CredentialProvider.RegistrationTool
{
    public static class CredentialProviderRegistrationServices
    {
        public static bool IsManagedAssembly(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var peReader = new PEReader(fs))
                {
                    if (!peReader.HasMetadata)
                    {
                        return false;
                    }

                    MetadataReader reader = peReader.GetMetadataReader();
                    return reader.IsAssembly;
                }
            }
        }

        public static IEnumerable<CredentialProviderRegistrationData> GetCredentalProviders()
        {
            var cpKeys = Registry.LocalMachine.OpenSubKey($@"Software\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers");
            foreach (var clsid in cpKeys.GetSubKeyNames())
            {
                if (Guid.TryParse(clsid, out Guid result))
                {
                    yield return GetCredentialProvider(result);
                }
            }
        }

        public static CredentialProviderRegistrationData GetCredentialProvider(Type type)
        {
            var comGuid = GetComGuid(type);
            return GetCredentialProvider(comGuid);
        }

        public static CredentialProviderRegistrationData GetCredentialProvider(string progId)
        {
            var clsid = GetClsidFromProgId(progId);
            return GetCredentialProvider(clsid);
        }

        public static CredentialProviderRegistrationData GetCredentialProvider(Guid clsid)
        {
            CredentialProviderRegistrationData data = new CredentialProviderRegistrationData();

            data.Clsid = clsid;

            var clsidKey = Registry.ClassesRoot.OpenSubKey($@"CLSID\{clsid:B}");
            if (clsidKey != null)
            {
                var inprocKey = clsidKey.OpenSubKey("InprocServer32");
                data.IsComRegistered = inprocKey != null;

                if (data.IsComRegistered)
                {
                    var coreLib = inprocKey.GetValue(string.Empty) as string;

                    if (string.IsNullOrWhiteSpace(coreLib))
                    {
                        data.IsComRegistered = false;
                    }
                    else
                    {
                        if (string.Equals(coreLib, "mscoree.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DllType = DllType.NetFramework;
                            data.DllPath = inprocKey.GetValue("CodeBase") as string;
                        }
                        else if (coreLib.EndsWith(".comhost.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DllType = DllType.NetCore;
                            var i = coreLib.IndexOf(".comhost.dll", StringComparison.OrdinalIgnoreCase);
                            data.DllPath = coreLib.Substring(0, i) + ".dll";
                        }
                        else
                        {
                            data.DllType = DllType.Native;
                            data.DllPath = coreLib;
                        }
                    }
                }
            }

            data.ProgId = Registry.ClassesRoot.OpenSubKey($@"CLSID\{clsid:B}\ProgId")?.GetValue(string.Empty) as string;

            var cpkey = Registry.LocalMachine.OpenSubKey($@"Software\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{clsid:B}");
            data.IsCredentialProviderRegistered = cpkey != null;

            if (data.IsCredentialProviderRegistered)
            {
                int? disabled = cpkey.GetValue("Disabled", 0) as int?;
                data.IsCredentalProviderEnabled = disabled == null || disabled == 0;
                data.CredentialProviderName = cpkey.GetValue(string.Empty) as string;
            }

            return data;
        }

        public static void UnregisterCredentialProvider(Type type, bool unregisterCom)
        {
            DeleteCredentialProviderRegistration(type);

            if (unregisterCom)
            {
                if (IsFrameworkType(type))
                {
                    UnregisterFrameworkAssembly(type);
                }
                else
                {
                    UnregisterNetCoreAssembly(type);
                }
            }
        }

        public static void UnregisterCredentialProvider(Guid clsid, bool unregisterCom)
        {
            DeleteCredentialProviderRegistration(clsid);

            if (unregisterCom)
            {
                UnregisterClass(clsid);

            }
        }

        public static void RegisterCredentialProvider(Type type)
        {
            CreateCredentialProviderRegistration(type);

            if (IsFrameworkType(type))
            {
                RegisterFrameworkAssembly(type);
            }
            else
            {
                RegisterNetCoreAssembly(type);
            }
        }

        public static void DisableCredentialProvider(Type type)
        {
            var comGuid = GetComGuid(type);
            DisableCredentialProvider(comGuid);
        }

        public static void DisableCredentialProvider(Guid comGuid)
        {
            var key = Registry.LocalMachine.OpenSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{comGuid:B}", true);
            key?.SetValue("Disabled", 1);
        }

        public static void EnableCredentialProvider(string progId)
        {
            var clsid = GetClsidFromProgId(progId);
            EnableCredentialProvider(clsid);
        }

        public static void DisableCredentialProvider(string progId)
        {
            var clsid = GetClsidFromProgId(progId);
            DisableCredentialProvider(clsid);
        }

        public static void EnableCredentialProvider(Guid comGuid)
        {
            var key = Registry.LocalMachine.OpenSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{comGuid:B}", true);
            key?.SetValue("Disabled", 0);
        }

        public static void EnableCredentialProvider(Type type)
        {
            var comGuid = GetComGuid(type);
            EnableCredentialProvider(comGuid);
        }

        private static void CreateCredentialProviderRegistration(Type t)
        {
            var comGuid = GetComGuid(t);
            var typeName = GetTypeFullName(t);

            var key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{comGuid:B}", true);
            key.SetValue(null, typeName);
        }

        private static void DeleteCredentialProviderRegistration(Type t)
        {
            var comGuid = GetComGuid(t);
            DeleteCredentialProviderRegistration(comGuid);
        }

        private static void DeleteCredentialProviderRegistration(Guid clsid)
        {
            Registry.LocalMachine.DeleteSubKeyTree($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{clsid:B}", false);
        }

        private static void RegisterNetCoreAssembly(Type t)
        {
            var comGuid = GetComGuid(t);
            var typeName = GetTypeFullName(t);
            var progId = GetComProgId(t);
            var assemblyLocation = GetTypeAssemblyLocation(t);

            var dir = Path.GetDirectoryName(assemblyLocation);
            var assemblyFile = Path.GetFileNameWithoutExtension(assemblyLocation);
            var comHostLocation = Path.Combine(dir, assemblyFile + ".comhost.dll");

            var rootClsid = Registry.LocalMachine.CreateSubKey($@"Software\Classes\CLSID\{comGuid:B}", true);
            rootClsid.SetValue(null, "CoreCLR COMHost Server");

            var inprocKey = rootClsid.CreateSubKey("InprocServer32", true);
            inprocKey.SetValue(null, comHostLocation);
            inprocKey.SetValue("ThreadingModel", "Both");

            var progIdKey = rootClsid.CreateSubKey("ProgId", true);
            progIdKey.SetValue(null, progId);

            var progIdRoot = Registry.LocalMachine.CreateSubKey($@"Software\Classes\{progId}", true);
            progIdRoot.SetValue(null, typeName);
            var progIdSubKey = progIdRoot.CreateSubKey("CLSID");
            progIdSubKey.SetValue(null, comGuid.ToString("B"));
        }

        private static void UnregisterNetCoreAssembly(Type t)
        {
            var comGuid = GetComGuid(t);
            var progId = GetComProgId(t);

            UnregisterClass(comGuid, progId);
        }

        private static void UnregisterClass(Guid? clsid, string progId)
        {
            if (clsid != null)
            {
                Registry.LocalMachine.DeleteSubKeyTree($@"Software\Classes\CLSID\{clsid}", false);
            }

            if (!string.IsNullOrWhiteSpace(progId))
            {
                Registry.LocalMachine.DeleteSubKeyTree($@"Software\Classes\{progId}", false);
            }
        }

        private static void RegisterFrameworkAssembly(Type t)
        {
            var comGuid = GetComGuid(t);
            var typeName = GetTypeFullName(t);
            var progId = GetComProgId(t);

            var rootClsid = Registry.LocalMachine.CreateSubKey($@"Software\Classes\CLSID\{comGuid:B}", true);
            rootClsid.SetValue(null, typeName);

            rootClsid.CreateSubKey("Implemented Categories");
            rootClsid.CreateSubKey(@"Implemented Categories\{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");

            var inprocKey = rootClsid.CreateSubKey("InprocServer32", true);
            inprocKey.SetValue(null, "mscoree.dll");
            inprocKey.SetValue("ThreadingModel", "Both");
            inprocKey.SetValue("Class", typeName);
            inprocKey.SetValue("RuntimeVersion", "v4.0.30319");
            inprocKey.SetValue("Assembly", GetTypeAssemblyName(t));
            inprocKey.SetValue("CodeBase", GetTypeAssemblyLocation(t));

            var progIdKey = rootClsid.CreateSubKey("ProgId", true);
            progIdKey.SetValue(null, progId);

            var progIdRoot = Registry.LocalMachine.CreateSubKey($@"Software\Classes\{progId}", true);
            progIdRoot.SetValue(null, typeName);
            var progIdSubKey = progIdRoot.CreateSubKey("CLSID");
            progIdSubKey.SetValue(null, comGuid.ToString("B"));
        }

        private static void UnregisterFrameworkAssembly(Type t)
        {
            var comGuid = GetComGuid(t);
            var progId = GetComProgId(t);

            UnregisterClass(comGuid, progId);
        }

        public static void UnregisterClass(Guid clsid)
        {
            string progid = null;

            try
            {
                progid = GetProgIdFromClasid(clsid);
            }
            catch (NotFoundException) { }

            UnregisterClass(clsid, progid);
        }

        public static void UnregisterClass(string progId)
        {
            Guid? clsid = null;
            try
            {
                clsid = GetClsidFromProgId(progId);
            }
            catch (NotFoundException) { }

            UnregisterClass(clsid, progId);
        }

        public static Guid GetClsidFromProgId(string progId)
        {
            var value = Registry.ClassesRoot.OpenSubKey($@"{progId}\CLSID")?.GetValue(string.Empty) as string;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ClsidNotFoundException($"The clsid for ProgId was not found {progId}");
            }

            return Guid.Parse(value);
        }

        public static string GetProgIdFromClasid(Guid clsid)
        {
            var value = Registry.ClassesRoot.OpenSubKey($@"CLSID\{clsid:B}\ProgId")?.GetValue(string.Empty) as string;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ProgIdNotFoundException($"The ProgId for clsid was not found {clsid}");
            }

            return value;
        }

        private static string GetTypeAssemblyLocation(Type type)
        {
            return type.Assembly.Location;
        }

        private static string GetTypeAssemblyName(Type type)
        {
            return type.Assembly.FullName;
        }

        private static string GetTypeClassName(Type type)
        {
            return type.Name;
        }

        private static string GetTypeFullName(Type type)
        {
            return type.FullName;
        }

        private static Guid GetComGuid(Type type)
        {
            var typeId = type.GetCustomAttributeValue("GuidAttribute");

            if (typeId == null)
            {
                throw new ArgumentException($"The type {type.Name} does not have the Guid attribute present");
            }

            return new Guid(typeId);
        }

        private static string GetComProgId(Type type)
        {
            var typeId = type.GetCustomAttributeValue("ProgIdAttribute");

            if (typeId == null)
            {
                throw new ArgumentException($"The type {type.Name} does not have the ProgId attribute present");
            }

            return typeId;
        }

        private static bool IsFrameworkType(Type type)
        {
            return IsFrameworkAssembly(type.Assembly);
        }

        private static bool IsFrameworkAssembly(Assembly assembly)
        {
            var framework = assembly.GetCustomAttributeValue("TargetFrameworkAttribute");
            return framework.StartsWith(".NETFramework");
        }


        private static string GetCustomAttributeValue(this Type type, string attributeName)
        {
            var cads = type.GetCustomAttributesData();
            foreach (CustomAttributeData cad in cads.Where(a => a.AttributeType.Name == attributeName))
            {
                return cad.ConstructorArguments.FirstOrDefault().Value as string;
            }

            return String.Empty;
        }

        private static string GetCustomAttributeValue(this Assembly assembly, string attributeName)
        {
            foreach (CustomAttributeData cad in assembly.GetCustomAttributesData().Where(a => a.AttributeType.Name == attributeName))
            {
                return cad.ConstructorArguments.FirstOrDefault().Value as string;
            }

            return String.Empty;
        }

        public static IEnumerable<Type> GetCredentialProviders(Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(ifn => ifn.Name == "ICredentialProvider") && !t.IsAbstract && !t.IsInterface);
        }

        public static Assembly LoadAssembly(string assemblyPath)
        {
            string assemblyBasePath = Path.GetDirectoryName(assemblyPath);

            List<string> paths = new List<string>();
            paths.AddRange(Directory.GetFiles(assemblyBasePath));
            paths.Add(typeof(object).Assembly.Location);

            // needs to be the .net fx or net core fx location
            paths.AddRange(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll"));

            var resolver = new PathAssemblyResolver(paths);
            MetadataLoadContext mlc = new MetadataLoadContext(resolver);
            return mlc.LoadFromAssemblyPath(assemblyPath);
        }
    }
}
