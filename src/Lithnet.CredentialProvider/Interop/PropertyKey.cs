using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct PropertyKey
	{
		public Guid FormatID;

		public uint PropertyID;

        public PropertyKey(string format, uint property)
        {
			FormatID = new Guid(format);
            PropertyID = property;
        }

        public PropertyKey(uint propertyId, 
            uint a,
            ushort b,
            ushort c,
            byte d,
            byte e,
            byte f,
            byte g,
            byte h,
            byte i,
            byte j,
            byte k)
        {
            PropertyID = propertyId;
            FormatID = new Guid(a, b, c, d, e, f, g, h, i, j, k);
        }
    }
}
