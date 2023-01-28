using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ULargeInteger
	{
		public ulong QuadPart;
	}
}
