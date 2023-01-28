using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lithnet.CredentialProvider.Interop
{
    
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("9387928B-AC75-4BF9-8AB2-2B93C4A55290")]
    [ComImport]
    internal interface IConnectableCredentialProviderCredential : ICredentialProviderCredential
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int Advise([MarshalAs(UnmanagedType.Interface)] [In] ICredentialProviderCredentialEvents pcpce);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int UnAdvise();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetSelected(out int pbAutoLogon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetDeselected();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetFieldState([In] uint dwFieldID, out FieldState pcpfs, out FieldInteractiveState pcpfis);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetStringValue([In] uint dwFieldID, out IntPtr ppsz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetBitmapValue([In] uint dwFieldID, out IntPtr phbmp);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetCheckboxValue([In] uint dwFieldID, out int pbChecked, [MarshalAs(UnmanagedType.LPWStr)] out string ppszLabel);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetSubmitButtonValue([In] uint dwFieldID, out uint pdwAdjacentTo);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetComboBoxValueCount([In] uint dwFieldID, out uint pcItems, out uint pdwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetComboBoxValueAt([In] uint dwFieldID, uint dwItem, [MarshalAs(UnmanagedType.LPWStr)] out string ppszItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetStringValue([In] uint dwFieldID, [In] IntPtr psz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetCheckboxValue([In] uint dwFieldID, [In] int bChecked);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int SetComboBoxSelectedValue([In] uint dwFieldID, [In] uint dwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int CommandLinkClicked([In] uint dwFieldID);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int GetSerialization(out SerializationResponse pcpgsr, out CredentialSerialization pcpcs, [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        new int ReportResult([In] int ntsStatus, [In] int ntsSubstatus, [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText, out StatusIcon pcpsiOptionalStatusIcon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int Connect([MarshalAs(UnmanagedType.Interface)] [In] IQueryContinueWithStatus pqcws);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
        [return: MarshalAs(UnmanagedType.Error)]
        int Disconnect();
    }
}
