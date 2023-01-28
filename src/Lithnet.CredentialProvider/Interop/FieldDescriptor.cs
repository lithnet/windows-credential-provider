using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FieldDescriptor
	{
		public uint FieldID;

		public FieldType FieldType;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string Label;

        public Guid FieldTypeGuid;
	}
}
