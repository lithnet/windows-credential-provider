using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal sealed class RawCredentialEvents : IDisposable
    {
        private static readonly object RegistryLock = new object();
        private static readonly HashSet<RawCredentialEvents> Registry = new HashSet<RawCredentialEvents>();

        private readonly object observationLock = new object();
        private readonly int maximumVersion;
        private readonly RawQueryInterfaceDelegate queryInterfaceDelegate;
        private readonly RawReferenceDelegate addRefDelegate;
        private readonly RawReferenceDelegate releaseDelegate;
        private readonly RawCredentialEventFieldUIntDelegate fieldUIntStubDelegate;
        private readonly RawCredentialEventFieldPointerDelegate fieldPointerStubDelegate;
        private readonly RawCredentialEventFieldCheckboxDelegate fieldCheckboxStubDelegate;
        private readonly RawCredentialEventFieldPointerDelegate setFieldStringDelegate;
        private readonly RawCredentialEventNoArgumentsDelegate beginFieldUpdatesDelegate;
        private readonly RawCredentialEventNoArgumentsDelegate endFieldUpdatesDelegate;
        private readonly RawCredentialEventFieldUIntDelegate setFieldOptionsDelegate;
        private readonly RawCredentialEventOnCreatingWindowDelegate onCreatingWindowDelegate;
        private readonly RawCredentialEventBitmapBufferDelegate setFieldBitmapBufferDelegate;

        private IntPtr events1Vtable;
        private IntPtr events2Vtable;
        private IntPtr events3Vtable;
        private IntPtr events1Interface;
        private IntPtr events2Interface;
        private IntPtr events3Interface;
        private int referenceCount;
        private int addRefCallCount;
        private int releaseCallCount;
        private int events2QueryCount;
        private int events3QueryCount;
        private int setFieldStringCallCount;
        private int beginFieldUpdatesCallCount;
        private int endFieldUpdatesCallCount;
        private int setFieldOptionsCallCount;
        private int setFieldBitmapBufferCallCount;
        private int ownerReferenceReleased;
        private IntPtr lastCredential;
        private uint lastFieldId;
        private uint lastUIntValue;
        private string lastString;
        private byte[] lastBitmapBuffer;
        private bool bitmapBufferReturnValuePointerWasPresent;

        public RawCredentialEvents(int maximumVersion)
        {
            if (maximumVersion < 1 || maximumVersion > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumVersion));
            }

            this.maximumVersion = maximumVersion;
            this.referenceCount = 1;
            this.queryInterfaceDelegate = this.QueryInterface;
            this.addRefDelegate = this.AddRef;
            this.releaseDelegate = this.Release;
            this.fieldUIntStubDelegate = this.FieldUIntStub;
            this.fieldPointerStubDelegate = this.FieldPointerStub;
            this.fieldCheckboxStubDelegate = this.FieldCheckboxStub;
            this.setFieldStringDelegate = this.SetFieldString;
            this.beginFieldUpdatesDelegate = this.BeginFieldUpdates;
            this.endFieldUpdatesDelegate = this.EndFieldUpdates;
            this.setFieldOptionsDelegate = this.SetFieldOptions;
            this.onCreatingWindowDelegate = this.OnCreatingWindow;
            this.setFieldBitmapBufferDelegate = this.SetFieldBitmapBuffer;

            try
            {
                this.events1Vtable = this.CreateVtable(RawCredentialEventAbi.Events1SlotCount);
                this.events1Interface = this.CreateInterface(this.events1Vtable);

                if (maximumVersion >= 2)
                {
                    this.events2Vtable = this.CreateVtable(RawCredentialEventAbi.Events2SlotCount);
                    this.events2Interface = this.CreateInterface(this.events2Vtable);
                }

                if (maximumVersion >= 3)
                {
                    this.events3Vtable = this.CreateVtable(RawCredentialEventAbi.Events3SlotCount);
                    this.events3Interface = this.CreateInterface(this.events3Vtable);
                }

                lock (RegistryLock)
                {
                    Registry.Add(this);
                }
            }
            catch
            {
                this.FreeNativeMemory();
                throw;
            }
        }

        public IntPtr Events1Interface => this.events1Interface;

        public IntPtr Events2Interface => this.events2Interface;

        public IntPtr Events3Interface => this.events3Interface;

        public int CurrentReferenceCount => Volatile.Read(ref this.referenceCount);

        public int AddRefCallCount => Volatile.Read(ref this.addRefCallCount);

        public int ReleaseCallCount => Volatile.Read(ref this.releaseCallCount);

        public int Events2QueryCount => Volatile.Read(ref this.events2QueryCount);

        public int Events3QueryCount => Volatile.Read(ref this.events3QueryCount);

        public int SetFieldStringCallCount => Volatile.Read(ref this.setFieldStringCallCount);

        public int BeginFieldUpdatesCallCount => Volatile.Read(ref this.beginFieldUpdatesCallCount);

        public int EndFieldUpdatesCallCount => Volatile.Read(ref this.endFieldUpdatesCallCount);

        public int SetFieldOptionsCallCount => Volatile.Read(ref this.setFieldOptionsCallCount);

        public int SetFieldBitmapBufferCallCount => Volatile.Read(ref this.setFieldBitmapBufferCallCount);

        public IntPtr LastCredential
        {
            get
            {
                lock (this.observationLock)
                {
                    return this.lastCredential;
                }
            }
        }

        public uint LastFieldId
        {
            get
            {
                lock (this.observationLock)
                {
                    return this.lastFieldId;
                }
            }
        }

        public uint LastUIntValue
        {
            get
            {
                lock (this.observationLock)
                {
                    return this.lastUIntValue;
                }
            }
        }

        public string LastString
        {
            get
            {
                lock (this.observationLock)
                {
                    return this.lastString;
                }
            }
        }

        public byte[] LastBitmapBuffer
        {
            get
            {
                lock (this.observationLock)
                {
                    return this.lastBitmapBuffer == null ? null : (byte[])this.lastBitmapBuffer.Clone();
                }
            }
        }

        public bool BitmapBufferReturnValuePointerWasPresent
        {
            get
            {
                lock (this.observationLock)
                {
                    return this.bitmapBufferReturnValuePointerWasPresent;
                }
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref this.ownerReferenceReleased, 1) == 0)
            {
                this.Release(IntPtr.Zero);
            }
        }

        private IntPtr CreateVtable(int slotCount)
        {
            IntPtr vtable = Marshal.AllocHGlobal(checked(slotCount * IntPtr.Size));

            for (int slot = 0; slot < slotCount; slot++)
            {
                Marshal.WriteIntPtr(vtable, checked(slot * IntPtr.Size), IntPtr.Zero);
            }

            this.WriteMethod(vtable, 0, this.queryInterfaceDelegate);
            this.WriteMethod(vtable, 1, this.addRefDelegate);
            this.WriteMethod(vtable, 2, this.releaseDelegate);
            this.WriteMethod(vtable, 3, this.fieldUIntStubDelegate);
            this.WriteMethod(vtable, 4, this.fieldUIntStubDelegate);
            this.WriteMethod(vtable, RawCredentialEventAbi.SetFieldStringSlot, this.setFieldStringDelegate);
            this.WriteMethod(vtable, 6, this.fieldCheckboxStubDelegate);
            this.WriteMethod(vtable, 7, this.fieldPointerStubDelegate);
            this.WriteMethod(vtable, 8, this.fieldUIntStubDelegate);
            this.WriteMethod(vtable, 9, this.fieldUIntStubDelegate);
            this.WriteMethod(vtable, 10, this.fieldPointerStubDelegate);
            this.WriteMethod(vtable, 11, this.fieldUIntStubDelegate);
            this.WriteMethod(vtable, 12, this.onCreatingWindowDelegate);

            if (slotCount >= RawCredentialEventAbi.Events2SlotCount)
            {
                this.WriteMethod(vtable, RawCredentialEventAbi.BeginFieldUpdatesSlot, this.beginFieldUpdatesDelegate);
                this.WriteMethod(vtable, RawCredentialEventAbi.EndFieldUpdatesSlot, this.endFieldUpdatesDelegate);
                this.WriteMethod(vtable, RawCredentialEventAbi.SetFieldOptionsSlot, this.setFieldOptionsDelegate);
            }

            if (slotCount >= RawCredentialEventAbi.Events3SlotCount)
            {
                this.WriteMethod(vtable, RawCredentialEventAbi.SetFieldBitmapBufferSlot, this.setFieldBitmapBufferDelegate);
            }

            return vtable;
        }

        private IntPtr CreateInterface(IntPtr vtable)
        {
            IntPtr result = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(result, vtable);
            return result;
        }

        private void WriteMethod(IntPtr vtable, int slot, Delegate method)
        {
            Marshal.WriteIntPtr(vtable, checked(slot * IntPtr.Size), Marshal.GetFunctionPointerForDelegate(method));
        }

        private int QueryInterface(IntPtr instance, ref Guid interfaceId, out IntPtr interfacePointer)
        {
            interfacePointer = IntPtr.Zero;

            if (interfaceId == RawCredentialEventAbi.IUnknown || interfaceId == RawCredentialEventAbi.ICredentialProviderCredentialEvents)
            {
                interfacePointer = this.events1Interface;
            }
            else if (interfaceId == RawCredentialEventAbi.ICredentialProviderCredentialEvents2)
            {
                Interlocked.Increment(ref this.events2QueryCount);
                if (this.maximumVersion >= 2)
                {
                    interfacePointer = this.events2Interface;
                }
            }
            else if (interfaceId == RawCredentialEventAbi.ICredentialProviderCredentialEvents3)
            {
                Interlocked.Increment(ref this.events3QueryCount);
                if (this.maximumVersion >= 3)
                {
                    interfacePointer = this.events3Interface;
                }
            }

            if (interfacePointer == IntPtr.Zero)
            {
                return RawCredentialEventAbi.E_NOINTERFACE;
            }

            this.AddRef(instance);
            return CredentialProviderAbi.S_OK;
        }

        private uint AddRef(IntPtr instance)
        {
            Interlocked.Increment(ref this.addRefCallCount);
            return checked((uint)Interlocked.Increment(ref this.referenceCount));
        }

        private uint Release(IntPtr instance)
        {
            Interlocked.Increment(ref this.releaseCallCount);
            int result = Interlocked.Decrement(ref this.referenceCount);

            if (result == 0)
            {
                lock (RegistryLock)
                {
                    Registry.Remove(this);
                }

                this.FreeNativeMemory();
            }

            return checked((uint)result);
        }

        private int SetFieldString(IntPtr instance, IntPtr credential, uint fieldId, IntPtr value)
        {
            try
            {
                lock (this.observationLock)
                {
                    this.lastCredential = credential;
                    this.lastFieldId = fieldId;
                    this.lastString = value == IntPtr.Zero ? null : Marshal.PtrToStringUni(value);
                }

                Interlocked.Increment(ref this.setFieldStringCallCount);
                return CredentialProviderAbi.S_OK;
            }
            catch
            {
                return CredentialProviderAbi.E_FAIL;
            }
        }

        private int BeginFieldUpdates(IntPtr instance)
        {
            Interlocked.Increment(ref this.beginFieldUpdatesCallCount);
            return CredentialProviderAbi.S_OK;
        }

        private int EndFieldUpdates(IntPtr instance)
        {
            Interlocked.Increment(ref this.endFieldUpdatesCallCount);
            return CredentialProviderAbi.S_OK;
        }

        private int SetFieldOptions(IntPtr instance, IntPtr credential, uint fieldId, uint value)
        {
            lock (this.observationLock)
            {
                this.lastCredential = credential;
                this.lastFieldId = fieldId;
                this.lastUIntValue = value;
            }

            Interlocked.Increment(ref this.setFieldOptionsCallCount);
            return CredentialProviderAbi.S_OK;
        }

        private int SetFieldBitmapBuffer(IntPtr instance, IntPtr credential, uint fieldId, uint imageBufferSize, IntPtr imageBuffer, IntPtr returnValue)
        {
            try
            {
                if (returnValue == IntPtr.Zero)
                {
                    return RawCredentialEventAbi.E_POINTER;
                }

                byte[] buffer = new byte[checked((int)imageBufferSize)];
                if (buffer.Length > 0)
                {
                    Marshal.Copy(imageBuffer, buffer, 0, buffer.Length);
                }

                Marshal.WriteInt32(returnValue, CredentialProviderAbi.S_OK);

                lock (this.observationLock)
                {
                    this.lastCredential = credential;
                    this.lastFieldId = fieldId;
                    this.lastBitmapBuffer = buffer;
                    this.bitmapBufferReturnValuePointerWasPresent = true;
                }

                Interlocked.Increment(ref this.setFieldBitmapBufferCallCount);
                return CredentialProviderAbi.S_OK;
            }
            catch
            {
                return CredentialProviderAbi.E_FAIL;
            }
        }

        private int FieldUIntStub(IntPtr instance, IntPtr credential, uint fieldId, uint value)
        {
            return CredentialProviderAbi.S_OK;
        }

        private int FieldPointerStub(IntPtr instance, IntPtr credential, uint fieldId, IntPtr value)
        {
            return CredentialProviderAbi.S_OK;
        }

        private int FieldCheckboxStub(IntPtr instance, IntPtr credential, uint fieldId, int isChecked, IntPtr label)
        {
            return CredentialProviderAbi.S_OK;
        }

        private int OnCreatingWindow(IntPtr instance, out IntPtr ownerWindow)
        {
            ownerWindow = IntPtr.Zero;
            return CredentialProviderAbi.S_OK;
        }

        private void FreeNativeMemory()
        {
            this.FreeNativeMemory(ref this.events1Interface);
            this.FreeNativeMemory(ref this.events2Interface);
            this.FreeNativeMemory(ref this.events3Interface);
            this.FreeNativeMemory(ref this.events1Vtable);
            this.FreeNativeMemory(ref this.events2Vtable);
            this.FreeNativeMemory(ref this.events3Vtable);
        }

        private void FreeNativeMemory(ref IntPtr pointer)
        {
            if (pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pointer);
                pointer = IntPtr.Zero;
            }
        }
    }
}
