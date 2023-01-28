using System;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Explicit, Pack = 8, Size = 8)]
    internal struct InnerPropertyValue
    {
        [FieldOffset(0)]
        public sbyte cVal;

        [FieldOffset(0)]
        public byte bVal;

        [FieldOffset(0)]
        public short iVal;

        [FieldOffset(0)]
        public ushort uiVal;

        [FieldOffset(0)]
        public int lVal;

        [FieldOffset(0)]
        public uint ulVal;

        [FieldOffset(0)]
        public int intVal;

        [FieldOffset(0)]
        public uint uintVal;

        [FieldOffset(0)]
        public LargeInteger hVal;

        [FieldOffset(0)]
        public ULargeInteger uhVal;

        [FieldOffset(0)]
        public float fltVal;

        [FieldOffset(0)]
        public double dblVal;

        [FieldOffset(0)]
        public short boolVal;

        [FieldOffset(0)]
        public short __OBSOLETE__VARIANT_BOOL;

        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.Error)]
        public int scode;

        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.Currency)]
        public decimal cyVal;

        [FieldOffset(0)]
        public DateTime date;

        [FieldOffset(0)]
        public System.Runtime.InteropServices.ComTypes.FILETIME filetime;

        [FieldOffset(0)]
        public UnmanagedBlob bstrblobVal;

        [FieldOffset(0)]
        public UnmanagedBlob blob;

        [FieldOffset(0)]
        public UnmanagedArray cac;

        [FieldOffset(0)]
        public UnmanagedArray caub;

        [FieldOffset(0)]
        public UnmanagedArray cai;

        [FieldOffset(0)]
        public UnmanagedArray caui;

        [FieldOffset(0)]
        public UnmanagedArray cal;

        [FieldOffset(0)]
        public UnmanagedArray caul;

        [FieldOffset(0)]
        public UnmanagedArray caflt;

        [FieldOffset(0)]
        public UnmanagedArray cadbl;

        [FieldOffset(0)]
        public UnmanagedArray cabool;

        [FieldOffset(0)]
        public UnmanagedArray cascode;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pcVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pbVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr piVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr puiVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr plVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pulVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pintVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr puintVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pfltVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pdblVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pboolVal;

        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pscode;
    }
}
