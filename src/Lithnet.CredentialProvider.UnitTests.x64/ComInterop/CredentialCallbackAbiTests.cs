using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    [TestFixture]
    [NonParallelizable]
    [Apartment(ApartmentState.STA)]
    public class CredentialCallbackAbiTests
    {
        [Test]
        public void RawEventInterfacesPreserveQueryInterfaceIdentityAndReferenceCounts()
        {
            RawCredentialEvents events = new RawCredentialEvents(3);

            try
            {
                AssertIUnknownIdentity(events, events.Events1Interface);
                AssertIUnknownIdentity(events, events.Events2Interface);
                AssertIUnknownIdentity(events, events.Events3Interface);

                RawQueryInterfaceDelegate queryInterface = GetRawMethod<RawQueryInterfaceDelegate>(events.Events1Interface, 0);
                Guid events2Id = RawCredentialEventAbi.ICredentialProviderCredentialEvents2;
                int beforeEvents2Query = events.CurrentReferenceCount;

                Assert.That(queryInterface(events.Events1Interface, ref events2Id, out IntPtr events2Pointer), Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(events2Pointer, Is.EqualTo(events.Events2Interface));
                Assert.That(events.CurrentReferenceCount, Is.EqualTo(beforeEvents2Query + 1));
                ReleaseRaw(events2Pointer);
                Assert.That(events.CurrentReferenceCount, Is.EqualTo(beforeEvents2Query));

                Guid unsupportedId = new Guid("757DA58F-742C-4840-9692-10A9D6392E93");
                int beforeUnsupportedQuery = events.CurrentReferenceCount;

                Assert.That(queryInterface(events.Events1Interface, ref unsupportedId, out IntPtr unsupportedPointer), Is.EqualTo(RawCredentialEventAbi.E_NOINTERFACE));
                Assert.That(unsupportedPointer, Is.EqualTo(IntPtr.Zero));
                Assert.That(events.CurrentReferenceCount, Is.EqualTo(beforeUnsupportedQuery));
            }
            finally
            {
                events.Dispose();
            }

            CollectComWrappers();
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(0));
        }

        [Test]
        public void FailedAdviseReleasesCallbackState()
        {
            RawCredentialEvents events = ExerciseFailedAdvise();

            CollectComWrappers();
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(0));
        }

        [Test]
        public void RepeatedAdviseReplacesTheExistingCallbackReference()
        {
            RawCredentialEvents events;

            using (CredentialCallbackScenario scenario = new CredentialCallbackScenario(2))
            {
                events = scenario.Events;

                Assert.That(scenario.Advise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterFirstAdvise = events.CurrentReferenceCount;

                Assert.That(scenario.Advise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterSecondAdvise = events.CurrentReferenceCount;

                Assert.That(afterSecondAdvise, Is.EqualTo(afterFirstAdvise));
                Assert.That(scenario.UnAdvise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                Assert.That(events.CurrentReferenceCount, Is.LessThan(afterSecondAdvise));
            }

            CollectComWrappers();
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(0));
        }

        private static RawCredentialEvents ExerciseFailedAdvise()
        {
            RawCredentialEvents events;

            using (CredentialCallbackScenario scenario = new CredentialCallbackScenario(2))
            {
                events = scenario.Events;
                scenario.Provider.Tile.ThrowOnLoad = true;
                int releaseCallsBeforeAdvise = events.ReleaseCallCount;

                Assert.That(scenario.Advise(), Is.EqualTo(CredentialProviderAbi.E_FAIL));
                Assert.That(events.ReleaseCallCount, Is.GreaterThan(releaseCallsBeforeAdvise));

                scenario.Provider.Field.Label = "No callback after failed Advise";
                Assert.That(events.SetFieldStringCallCount, Is.EqualTo(0));
            }

            return events;
        }

        [Test]
        public void Events1SetFieldStringPreservesSlotArgumentsAndLifetimePhases()
        {
            RawCredentialEvents events;

            using (CredentialCallbackScenario scenario = new CredentialCallbackScenario(1))
            {
                events = scenario.Events;
                int beforeAdvise = scenario.Events.CurrentReferenceCount;

                Assert.That(scenario.Advise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterAdvise = scenario.Events.CurrentReferenceCount;

                SmallLabelControl field = scenario.Provider.Tile.Controls.GetControl<SmallLabelControl>("message");
                field.Label = "Updated through Events1";
                int duringUpdate = scenario.Events.CurrentReferenceCount;

                Assert.Multiple(() =>
                {
                    Assert.That(scenario.Events.SetFieldStringCallCount, Is.EqualTo(1));
                    Assert.That(scenario.Events.LastCredential, Is.Not.EqualTo(IntPtr.Zero));
                    Assert.That(scenario.Events.LastFieldId, Is.EqualTo(field.Id));
                    Assert.That(scenario.Events.LastString, Is.EqualTo("Updated through Events1"));
                    Assert.That(scenario.Events.Events2QueryCount, Is.GreaterThan(0));
                    Assert.That(scenario.Events.Events3QueryCount, Is.EqualTo(0));
                });

                int releaseCallsBeforeUnAdvise = scenario.Events.ReleaseCallCount;
                Assert.That(scenario.UnAdvise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterUnAdvise = scenario.Events.CurrentReferenceCount;

                AssertLifetimePhases(scenario.Events, beforeAdvise, afterAdvise, duringUpdate, afterUnAdvise, releaseCallsBeforeUnAdvise);
            }

            CollectComWrappers();
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(0));
        }

        [Test]
        public void Events2FieldUpdatesPreserveSlotsArgumentsAndLifetimePhases()
        {
            RawCredentialEvents events;

            using (CredentialCallbackScenario scenario = new CredentialCallbackScenario(2))
            {
                events = scenario.Events;
                int beforeAdvise = scenario.Events.CurrentReferenceCount;

                Assert.That(scenario.Advise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterAdvise = scenario.Events.CurrentReferenceCount;

                SmallLabelControl field = scenario.Provider.Tile.Controls.GetControl<SmallLabelControl>("message");
                scenario.Provider.Tile.BeginBulkFieldUpdate();
                field.Options = FieldOptions.Email;
                scenario.Provider.Tile.EndBulkFieldUpdate();
                int duringUpdate = scenario.Events.CurrentReferenceCount;

                Assert.Multiple(() =>
                {
                    Assert.That(scenario.Events.BeginFieldUpdatesCallCount, Is.EqualTo(1));
                    Assert.That(scenario.Events.EndFieldUpdatesCallCount, Is.EqualTo(1));
                    Assert.That(scenario.Events.SetFieldOptionsCallCount, Is.EqualTo(1));
                    Assert.That(scenario.Events.LastCredential, Is.Not.EqualTo(IntPtr.Zero));
                    Assert.That(scenario.Events.LastFieldId, Is.EqualTo(field.Id));
                    Assert.That(scenario.Events.LastUIntValue, Is.EqualTo((uint)FieldOptions.Email));
                    Assert.That(scenario.Events.Events2QueryCount, Is.GreaterThan(0));
                    Assert.That(scenario.Events.Events3QueryCount, Is.EqualTo(0));
                });

                int releaseCallsBeforeUnAdvise = scenario.Events.ReleaseCallCount;
                Assert.That(scenario.UnAdvise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterUnAdvise = scenario.Events.CurrentReferenceCount;

                AssertLifetimePhases(scenario.Events, beforeAdvise, afterAdvise, duringUpdate, afterUnAdvise, releaseCallsBeforeUnAdvise);
            }

            CollectComWrappers();
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(0));
        }

        [Test]
        public void Events3BitmapBufferPreservesSlotArgumentsCallShapeAndLifetimePhases()
        {
            RawCredentialEvents events = ExerciseEvents3BitmapBuffer();

            CollectComWrappers();
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(0));
        }

        private static RawCredentialEvents ExerciseEvents3BitmapBuffer()
        {
            RawCredentialEvents events;

            using (CredentialCallbackScenario scenario = new CredentialCallbackScenario(3))
            {
                events = scenario.Events;
                int beforeAdvise = scenario.Events.CurrentReferenceCount;

                Assert.That(scenario.Advise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterAdvise = scenario.Events.CurrentReferenceCount;

                CredentialProviderLogoControl logo = scenario.Provider.Tile.Controls.GetControl<CredentialProviderLogoControl>("logo");
                using (Bitmap bitmap = new Bitmap(2, 2))
                {
                    bitmap.SetPixel(0, 0, Color.Transparent);
                    bitmap.SetPixel(1, 0, Color.Red);
                    logo.Bitmap = bitmap;
                }

                int duringUpdate = scenario.Events.CurrentReferenceCount;
                byte[] buffer = scenario.Events.LastBitmapBuffer;

                Assert.Multiple(() =>
                {
                    Assert.That(scenario.Events.SetFieldBitmapBufferCallCount, Is.EqualTo(1));
                    Assert.That(scenario.Events.LastCredential, Is.Not.EqualTo(IntPtr.Zero));
                    Assert.That(scenario.Events.LastFieldId, Is.EqualTo(logo.Id));
                    Assert.That(buffer, Is.Not.Null);
                    Assert.That(buffer.Length, Is.GreaterThan(8));
                    Assert.That(buffer[0], Is.EqualTo(0x89));
                    Assert.That(buffer[1], Is.EqualTo(0x50));
                    Assert.That(buffer[2], Is.EqualTo(0x4E));
                    Assert.That(buffer[3], Is.EqualTo(0x47));
                    Assert.That(buffer[4], Is.EqualTo(0x0D));
                    Assert.That(buffer[5], Is.EqualTo(0x0A));
                    Assert.That(buffer[6], Is.EqualTo(0x1A));
                    Assert.That(buffer[7], Is.EqualTo(0x0A));
                    Assert.That(scenario.Events.BitmapBufferReturnValuePointerWasPresent, Is.True);
                    Assert.That(scenario.Events.Events2QueryCount, Is.GreaterThan(0));
                    Assert.That(scenario.Events.Events3QueryCount, Is.GreaterThan(0));
                });

                int releaseCallsBeforeUnAdvise = scenario.Events.ReleaseCallCount;
                Assert.That(scenario.UnAdvise(), Is.EqualTo(CredentialProviderAbi.S_OK));
                int afterUnAdvise = scenario.Events.CurrentReferenceCount;

                AssertLifetimePhases(scenario.Events, beforeAdvise, afterAdvise, duringUpdate, afterUnAdvise, releaseCallsBeforeUnAdvise);
            }

            return events;
        }

        private static void AssertLifetimePhases(RawCredentialEvents events, int beforeAdvise, int afterAdvise, int duringUpdate, int afterUnAdvise, int releaseCallsBeforeUnAdvise)
        {
            TestContext.WriteLine($"Callback reference counts: before Advise={beforeAdvise}, after Advise={afterAdvise}, during update={duringUpdate}, after UnAdvise={afterUnAdvise}; AddRef calls={events.AddRefCallCount}, Release calls={events.ReleaseCallCount}");

            Assert.Multiple(() =>
            {
                Assert.That(beforeAdvise, Is.EqualTo(1));
                Assert.That(afterAdvise, Is.GreaterThan(beforeAdvise));
                Assert.That(duringUpdate, Is.GreaterThanOrEqualTo(afterAdvise));
                Assert.That(afterUnAdvise, Is.LessThan(duringUpdate));
                Assert.That(afterUnAdvise, Is.GreaterThanOrEqualTo(beforeAdvise));
                Assert.That(events.ReleaseCallCount, Is.GreaterThan(releaseCallsBeforeUnAdvise));
            });
        }

        private static void AssertIUnknownIdentity(RawCredentialEvents events, IntPtr source)
        {
            RawQueryInterfaceDelegate queryInterface = GetRawMethod<RawQueryInterfaceDelegate>(source, 0);
            Guid unknownId = RawCredentialEventAbi.IUnknown;
            int beforeQuery = events.CurrentReferenceCount;

            Assert.That(queryInterface(source, ref unknownId, out IntPtr unknown), Is.EqualTo(CredentialProviderAbi.S_OK));
            Assert.That(unknown, Is.EqualTo(events.Events1Interface));
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(beforeQuery + 1));

            ReleaseRaw(unknown);
            Assert.That(events.CurrentReferenceCount, Is.EqualTo(beforeQuery));
        }

        private static TDelegate GetRawMethod<TDelegate>(IntPtr interfacePointer, int slot) where TDelegate : class
        {
            IntPtr vtable = Marshal.ReadIntPtr(interfacePointer);
            IntPtr method = Marshal.ReadIntPtr(vtable, checked(slot * IntPtr.Size));
            return (TDelegate)(object)Marshal.GetDelegateForFunctionPointer(method, typeof(TDelegate));
        }

        private static void ReleaseRaw(IntPtr interfacePointer)
        {
            RawReferenceDelegate release = GetRawMethod<RawReferenceDelegate>(interfacePointer, 2);
            release(interfacePointer);
        }

        private static void CollectComWrappers()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
