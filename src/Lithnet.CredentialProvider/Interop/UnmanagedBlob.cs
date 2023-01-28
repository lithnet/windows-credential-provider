using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct UnmanagedBlob
	{
		public uint cbSize;

		[ComConversionLoss]
		public IntPtr pData;
	}
}
