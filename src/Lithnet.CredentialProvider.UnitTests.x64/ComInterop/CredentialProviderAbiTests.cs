using System;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [TestFixture]
    [NonParallelizable]
    [Apartment(ApartmentState.STA)]
    public class CredentialProviderAbiTests
    {
        [Test]
        public void CredentialProviderInterfaceCanBeQueried()
        {
            var provider = new AbiTestCredentialProvider();
            IntPtr unknown = Marshal.GetIUnknownForObject(provider);
            IntPtr providerInterface = IntPtr.Zero;

            try
            {
                int hresult = ComMarshal.QueryInterface(unknown, CredentialProviderAbi.ICredentialProvider, out providerInterface);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(providerInterface, Is.Not.EqualTo(IntPtr.Zero));
            }
            finally
            {
                if (providerInterface != IntPtr.Zero)
                {
                    Marshal.Release(providerInterface);
                }

                Marshal.Release(unknown);
                GC.KeepAlive(provider);
            }
        }

        [Test]
        public void SetUsageScenarioPreservesHResultAndArguments()
        {
            var provider = new AbiTestCredentialProvider();

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                SetUsageScenarioDelegate setUsageScenario = providerInterface.GetMethod<SetUsageScenarioDelegate>(CredentialProviderAbi.SetUsageScenarioSlot);

                int hresult = setUsageScenario(providerInterface.Value, (int)UsageScenario.CredUI, (uint)CredUIWinFlags.CREDUIWIN_SECURE_PROMPT);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(provider.UsageScenario, Is.EqualTo(UsageScenario.CredUI));
                Assert.That(provider.CredUIFlags, Is.EqualTo(CredUIWinFlags.CREDUIWIN_SECURE_PROMPT));

                hresult = setUsageScenario(providerInterface.Value, (int)UsageScenario.Logon, 0);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.E_NOTIMPL));
                Assert.That(provider.UsageScenario, Is.EqualTo(UsageScenario.Logon));
                Assert.That(provider.CredUIFlags, Is.EqualTo((CredUIWinFlags)0));
            }

            GC.KeepAlive(provider);
        }

        [Test]
        public void GetFieldDescriptorCountReturnsProviderControlCount()
        {
            var provider = new AbiTestCredentialProvider();

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetFieldDescriptorCountDelegate getFieldDescriptorCount = providerInterface.GetMethod<GetFieldDescriptorCountDelegate>(CredentialProviderAbi.GetFieldDescriptorCountSlot);

                int hresult = getFieldDescriptorCount(providerInterface.Value, out uint count);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(count, Is.EqualTo(1));
            }

            GC.KeepAlive(provider);
        }

        [Test]
        public void GetFieldDescriptorAtReturnsWindowsSdkLayout()
        {
            var provider = new AbiTestCredentialProvider();

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetFieldDescriptorCountDelegate getFieldDescriptorCount = providerInterface.GetMethod<GetFieldDescriptorCountDelegate>(CredentialProviderAbi.GetFieldDescriptorCountSlot);
                GetFieldDescriptorAtDelegate getFieldDescriptorAt = providerInterface.GetMethod<GetFieldDescriptorAtDelegate>(CredentialProviderAbi.GetFieldDescriptorAtSlot);

                Assert.That(getFieldDescriptorCount(providerInterface.Value, out uint count), Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(count, Is.EqualTo(1));

                IntPtr descriptorPointer = IntPtr.Zero;
                IntPtr labelPointer = IntPtr.Zero;

                try
                {
                    int hresult = getFieldDescriptorAt(providerInterface.Value, 0, out descriptorPointer);

                    Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                    Assert.That(descriptorPointer, Is.Not.EqualTo(IntPtr.Zero));

                    NativeCredentialProviderFieldDescriptor descriptor = Marshal.PtrToStructure<NativeCredentialProviderFieldDescriptor>(descriptorPointer);
                    labelPointer = descriptor.Label;

                    Assert.That(descriptor.FieldId, Is.EqualTo(provider.Field.Id));
                    Assert.That(descriptor.FieldType, Is.EqualTo(CredentialProviderAbi.SmallTextFieldType));
                    Assert.That(Marshal.PtrToStringUni(labelPointer), Is.EqualTo("COM ABI test"));
                    Assert.That(descriptor.FieldTypeGuid, Is.EqualTo(Guid.Empty));
                }
                finally
                {
                    if (labelPointer != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(labelPointer);
                    }

                    if (descriptorPointer != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(descriptorPointer);
                    }
                }
            }

            GC.KeepAlive(provider);
        }

        [Test]
        public void GetFieldDescriptorAtRejectsInvalidIndex()
        {
            var provider = new AbiTestCredentialProvider();

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetFieldDescriptorCountDelegate getFieldDescriptorCount = providerInterface.GetMethod<GetFieldDescriptorCountDelegate>(CredentialProviderAbi.GetFieldDescriptorCountSlot);
                GetFieldDescriptorAtDelegate getFieldDescriptorAt = providerInterface.GetMethod<GetFieldDescriptorAtDelegate>(CredentialProviderAbi.GetFieldDescriptorAtSlot);

                Assert.That(getFieldDescriptorCount(providerInterface.Value, out uint count), Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(count, Is.EqualTo(1));

                int hresult = getFieldDescriptorAt(providerInterface.Value, count, out IntPtr descriptorPointer);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.E_INVALIDARG));
                Assert.That(descriptorPointer, Is.EqualTo(IntPtr.Zero));
            }

            GC.KeepAlive(provider);
        }

        [Test]
        public void CredentialProviderSetUserArrayInterfaceCanBeQueried()
        {
            var provider = new AbiTestCredentialProvider();
            IntPtr unknown = Marshal.GetIUnknownForObject(provider);
            IntPtr setUserArrayInterface = IntPtr.Zero;

            try
            {
                int hresult = ComMarshal.QueryInterface(unknown, CredentialProviderAbi.ICredentialProviderSetUserArray, out setUserArrayInterface);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(setUserArrayInterface, Is.Not.EqualTo(IntPtr.Zero));
            }
            finally
            {
                if (setUserArrayInterface != IntPtr.Zero)
                {
                    Marshal.Release(setUserArrayInterface);
                }

                Marshal.Release(unknown);
                GC.KeepAlive(provider);
            }
        }

        [Test]
        public void TestUserArrayUsesWindowsSdkGetCountSlot()
        {
            var users = new TestCredentialProviderUserArray();

            using (ComInterfacePointer userArrayInterface = ComInterfacePointer.Create(users, CredentialProviderAbi.ICredentialProviderUserArray))
            {
                GetUserCountDelegate getCount = userArrayInterface.GetMethod<GetUserCountDelegate>(CredentialProviderAbi.UserArrayGetCountSlot);

                int hresult = getCount(userArrayInterface.Value, out uint count);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(count, Is.EqualTo(0));
                Assert.That(users.GetCountCallCount, Is.EqualTo(1));
            }

            GC.KeepAlive(users);
        }

        [Test]
        public void SetUserArrayInvokesWindowsSdkUserArrayGetCount()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            SetEmptyUserArray(provider, users);

            Assert.That(users.GetCountCallCount, Is.EqualTo(1));

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void GetCredentialCountReturnsGenericTileWithoutDefault()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            SetEmptyUserArray(provider, users);

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetCredentialCountDelegate getCredentialCount = providerInterface.GetMethod<GetCredentialCountDelegate>(CredentialProviderAbi.GetCredentialCountSlot);

                int hresult = getCredentialCount(providerInterface.Value, out uint count, out uint defaultCredential, out int autoLogonWithDefault);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(count, Is.EqualTo(1));
                Assert.That(defaultCredential, Is.EqualTo(CredentialProviderAbi.NoDefaultCredential));
                Assert.That(autoLogonWithDefault, Is.EqualTo(0));
                Assert.That(users.GetCountCallCount, Is.EqualTo(2));
                Assert.That(provider.Tile, Is.Not.Null);
            }

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void GetCredentialAtRejectsInvalidIndex()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            SetEmptyUserArray(provider, users);

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetCredentialCountDelegate getCredentialCount = providerInterface.GetMethod<GetCredentialCountDelegate>(CredentialProviderAbi.GetCredentialCountSlot);
                GetCredentialAtDelegate getCredentialAt = providerInterface.GetMethod<GetCredentialAtDelegate>(CredentialProviderAbi.GetCredentialAtSlot);

                Assert.That(getCredentialCount(providerInterface.Value, out uint count, out uint defaultCredential, out int autoLogonWithDefault), Is.EqualTo(CredentialProviderAbi.S_OK));

                int hresult = getCredentialAt(providerInterface.Value, count, out IntPtr credential);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.E_FAIL));
                Assert.That(credential, Is.EqualTo(IntPtr.Zero));
            }

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void CredentialV1SelectionMethodsPreserveStateAndHResults()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            using (ComInterfacePointer credential = CreateCredentialInterface(provider, users))
            {
                SetSelectedDelegate setSelected = credential.GetMethod<SetSelectedDelegate>(CredentialProviderAbi.SetSelectedSlot);
                SetDeselectedDelegate setDeselected = credential.GetMethod<SetDeselectedDelegate>(CredentialProviderAbi.SetDeselectedSlot);

                int hresult = setSelected(credential.Value, out int autoLogon);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(autoLogon, Is.EqualTo(0));
                Assert.That(provider.Tile.IsSelected, Is.True);

                hresult = setDeselected(credential.Value);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(provider.Tile.IsSelected, Is.False);
            }

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void CredentialV1GetFieldStateUsesWindowsSdkEnumValues()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            using (ComInterfacePointer credential = CreateCredentialInterface(provider, users))
            {
                GetFieldStateDelegate getFieldState = credential.GetMethod<GetFieldStateDelegate>(CredentialProviderAbi.GetFieldStateSlot);

                int hresult = getFieldState(credential.Value, provider.Field.Id, out int fieldState, out int interactiveState);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(fieldState, Is.EqualTo(CredentialProviderAbi.DisplayInSelectedTileFieldState));
                Assert.That(interactiveState, Is.EqualTo(CredentialProviderAbi.NoInteractiveFieldState));
            }

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void CredentialV1GetStringValueReturnsComTaskMemory()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            using (ComInterfacePointer credential = CreateCredentialInterface(provider, users))
            {
                GetStringValueDelegate getStringValue = credential.GetMethod<GetStringValueDelegate>(CredentialProviderAbi.GetStringValueSlot);
                IntPtr value = IntPtr.Zero;

                try
                {
                    int hresult = getStringValue(credential.Value, provider.Field.Id, out value);

                    Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                    Assert.That(value, Is.Not.EqualTo(IntPtr.Zero));
                    Assert.That(Marshal.PtrToStringUni(value), Is.EqualTo("COM ABI test"));
                }
                finally
                {
                    if (value != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(value);
                    }
                }
            }

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void CredentialV2GetUserSidUsesInheritedVtableOrder()
        {
            var provider = new AbiTestCredentialProvider();
            var users = new TestCredentialProviderUserArray();

            using (ComInterfacePointer credential = CreateCredentialInterface(provider, users))
            {
                IntPtr credential2Pointer = IntPtr.Zero;

                try
                {
                    int hresult = ComMarshal.QueryInterface(credential.Value, CredentialProviderAbi.ICredentialProviderCredential2, out credential2Pointer);

                    Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                    Assert.That(credential2Pointer, Is.Not.EqualTo(IntPtr.Zero));

                    using (ComInterfacePointer credential2 = ComInterfacePointer.TakeOwnership(credential2Pointer))
                    {
                        credential2Pointer = IntPtr.Zero;
                        GetUserSidDelegate getUserSid = credential2.GetMethod<GetUserSidDelegate>(CredentialProviderAbi.GetUserSidSlot);
                        IntPtr sid = IntPtr.Zero;

                        try
                        {
                            hresult = getUserSid(credential2.Value, out sid);

                            Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_FALSE));
                            Assert.That(sid, Is.EqualTo(IntPtr.Zero));
                        }
                        finally
                        {
                            if (sid != IntPtr.Zero)
                            {
                                Marshal.FreeCoTaskMem(sid);
                            }
                        }
                    }
                }
                finally
                {
                    if (credential2Pointer != IntPtr.Zero)
                    {
                        Marshal.Release(credential2Pointer);
                    }
                }
            }

            GC.KeepAlive(users);
            GC.KeepAlive(provider);
        }

        [Test]
        public void FieldDescriptorDeclarationMatchesWindowsSdkSize()
        {
            int expectedSize = IntPtr.Size == 4 ? 28 : 32;

            Assert.That(Marshal.SizeOf<NativeCredentialProviderFieldDescriptor>(), Is.EqualTo(expectedSize));
        }

        private static void SetEmptyUserArray(AbiTestCredentialProvider provider, TestCredentialProviderUserArray users)
        {
            using (ComInterfacePointer setUserArrayInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProviderSetUserArray))
            using (ComInterfacePointer userArrayInterface = ComInterfacePointer.Create(users, CredentialProviderAbi.ICredentialProviderUserArray))
            {
                SetUserArrayDelegate setUserArray = setUserArrayInterface.GetMethod<SetUserArrayDelegate>(CredentialProviderAbi.SetUserArraySlot);

                int hresult = setUserArray(setUserArrayInterface.Value, userArrayInterface.Value);

                Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
            }
        }

        private static ComInterfacePointer CreateCredentialInterface(AbiTestCredentialProvider provider, TestCredentialProviderUserArray users)
        {
            SetEmptyUserArray(provider, users);

            using (ComInterfacePointer providerInterface = ComInterfacePointer.Create(provider, CredentialProviderAbi.ICredentialProvider))
            {
                GetCredentialCountDelegate getCredentialCount = providerInterface.GetMethod<GetCredentialCountDelegate>(CredentialProviderAbi.GetCredentialCountSlot);
                GetCredentialAtDelegate getCredentialAt = providerInterface.GetMethod<GetCredentialAtDelegate>(CredentialProviderAbi.GetCredentialAtSlot);

                Assert.That(getCredentialCount(providerInterface.Value, out uint count, out uint defaultCredential, out int autoLogonWithDefault), Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(count, Is.EqualTo(1));

                IntPtr credential = IntPtr.Zero;

                try
                {
                    int hresult = getCredentialAt(providerInterface.Value, 0, out credential);

                    Assert.That(hresult, Is.EqualTo(CredentialProviderAbi.S_OK));
                    Assert.That(credential, Is.Not.EqualTo(IntPtr.Zero));

                    ComInterfacePointer result = ComInterfacePointer.TakeOwnership(credential);
                    credential = IntPtr.Zero;
                    return result;
                }
                finally
                {
                    if (credential != IntPtr.Zero)
                    {
                        Marshal.Release(credential);
                    }
                }
            }
        }
    }
}
