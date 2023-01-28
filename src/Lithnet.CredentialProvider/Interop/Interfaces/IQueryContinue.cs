using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
	[Guid("7307055C-B24A-486B-9F25-163E597A28A9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IQueryContinue
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Error)]
		int QueryContinue();
	}
}
