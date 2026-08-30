using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeCredentialProviderFieldDescriptor
    {
        public uint FieldId;

        public int FieldType;

        public IntPtr Label;

        public Guid FieldTypeGuid;
    }
}
