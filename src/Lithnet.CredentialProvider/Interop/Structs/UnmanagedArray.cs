using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct UnmanagedArray
	{
		public uint cElems;

		[ComConversionLoss]
		public IntPtr pElems;
	}
}
