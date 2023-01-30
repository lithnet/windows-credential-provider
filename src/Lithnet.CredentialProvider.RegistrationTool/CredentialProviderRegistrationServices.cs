using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace Lithnet.CredentialProvider
{
    public static class CredentialProviderRegistrationServices
    {
        public static void UnregisterCredentialProvider(Type type)
        {
            DeleteCredentialProviderRegistration(type);

            if (IsFrameworkType(type))
            {
                UnregisterFrameworkAssembly(type);
            }
            else
            {
                UnregisterNetCoreAssembly(type);
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

            Registry.LocalMachine.DeleteSubKeyTree($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{comGuid:B}", false);
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

            Registry.LocalMachine.DeleteSubKeyTree($@"Software\Classes\CLSID\{comGuid:B}", false);
            Registry.LocalMachine.DeleteSubKeyTree($@"Software\Classes\{progId}", false);
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

            Registry.LocalMachine.DeleteSubKeyTree($@"Software\Classes\CLSID\{comGuid:B}", false);
            Registry.LocalMachine.DeleteSubKeyTree($@"Software\Classes\{progId}", false);
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
            var framework = type.Assembly.GetCustomAttributeValue("TargetFrameworkAttribute");
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
    }
}
