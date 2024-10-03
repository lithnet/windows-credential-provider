using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Win32.SafeHandles;
using Windows.Win32.Foundation;
using Windows.Win32.System.Threading;
using NativeMethods = Windows.Win32.PInvoke;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// ConsentUIData is an abstract base class that represents all the different types of data structures that can be passed to the ConsentUI process for a UAC elevation prompt.
    /// The static members of the class can be used to retrieve the data structure passed to the ConsentUI process, or to determine if the current process is the ConsentUI process.
    /// The caller will be provided with one of the concrete implementations of this class, depending on the type of data structure that was passed to the ConsentUI process.
    /// Use the <see cref="ConsentUIData.ConsentUIType"/> property to determine the type of data structure and cast it to one of the concrete implementations.
    /// </summary>
    public abstract class ConsentUIData
    {
        private static bool? isConsentUI;
        private static ConsentUIData cachedInstance;
        private static ConsentUICommandLineArgs commandLineArgs;
        private protected ConsentUIStructureHeaderBase header;
        private readonly byte[] rawData;
        private static readonly Version v2HeaderOsVersion = new Version(10, 0, 26100, 0);
        internal static int v2HeaderSize = Marshal.SizeOf<ConsentUIStructureHeaderV2>();
        internal static int v1HeaderSize = Marshal.SizeOf<ConsentUIStructureHeaderV1>();
        internal static int HeaderSize { get; set; } = Environment.OSVersion.Version >= v2HeaderOsVersion ? v2HeaderSize : v1HeaderSize;

        /// <summary>
        /// Gets a value indicating the type of ConsentUI data structure
        /// </summary>
        public ConsentUIType Type => this.header.Type;

        /// <summary>
        /// Gets a value indicating how UAC has been told to fetch approval.
        /// In the case where a Credential Provider is initialised, this should always be `Credentials`.
        /// </summary>
        public ConsentUIPromptType PromptType => this.header.PromptType;

        /// <summary>
        /// Gets a handle to the window that was responsible for invoking the ConsentUI prompt
        /// </summary>
        public IntPtr HWnd => this.header.hWindow;

        /// <summary>
        /// Gets the reason why `consent.exe` was started in the first place. In other words,
        /// the type of action that led to an elevation request.
        /// </summary>
        public ConsentUIElevationReason ElevationReason => this.header.ElevationReason;

        /// <summary>
        /// A series of flags that AppInfo passes to ConsentUI to signify actions that need to
        /// take place on the UI side.
        /// This includes specifics around the UI that should be presented & signature verification settings.
        /// </summary>
        public ConsentUIFlags Flags => this.header.Flags;

        /// <summary>
        /// Gets the ID of the session where the ConsentUI prompt was originally invoked
        /// </summary>
        public int SessionId => this.header.sessionId;

        private protected ConsentUIData(IntPtr pData, int expectedSize)
        {
            this.rawData = GetRawBytes(pData, expectedSize);

            this.header = Marshal.PtrToStructure<ConsentUIStructureHeaderBase>(pData);

            if (this.header.Size != expectedSize)
            {
                throw new InvalidDataException($"The size of the data structure {this.header.Size} does not match the expected size {expectedSize}");
            }

            if (HeaderSize > this.header.Size)
            {
                throw new InvalidDataException($"The size of the data structure {this.header.Size} is less than the expected header size {HeaderSize}");
            }
        }

        public SafeHandle GetConsentMutex()
        {
            return DuplicateHandleInternal(this.header.hMutex);
        }

        /// <summary>
        /// Gets the Windows Identity from the original caller requesting elevation
        /// </summary>
        /// <returns>A WindowsIdentity object that represents the user requesting elevation</returns>
        /// <exception cref="Win32Exception">Thrown when the user's token could not be obtained from the session information</exception>
        public WindowsIdentity GetWindowsIdentity()
        {
            var duplicatedToken = DuplicateHandleInternal(this.header.hToken);
            return new WindowsIdentity(duplicatedToken.DangerousGetHandle());
        }

        /// <summary>
        /// Gets a raw byte array representing the ConsentUI data structure
        /// </summary>
        /// <returns>A byte array</returns>
        public byte[] GetRawData()
        {
            return this.rawData;
        }

        /// <summary>
        /// Gets a string value that is packed at the end of the data structure if the offset is valid
        /// </summary>
        /// <param name="pData">A pointer to the start of the data structure</param>
        /// <param name="offset">The position from the start of the data structure where the string starts</param>
        /// <returns>A string containing all characters from the given offset up to the first null character found</returns>
        private protected string GetStringValueIfValid(IntPtr pData, int offset)
        {
            if (offset > 0)
            {
                this.ThrowOnInvalidOffset(offset);
                return Marshal.PtrToStringUni(IntPtr.Add(pData, offset));
            }

            return null;
        }

        /// <summary>
        /// Throws an exception if the given offset is greater than the size of the data structure
        /// </summary>
        /// <param name="value">The value of the offset</param>
        /// <exception cref="InvalidDataException">Thrown when the value of the pointer is greater than the expected data size</exception>
        private protected void ThrowOnInvalidOffset(int value)
        {
            if (value >= this.header.Size)
            {
                throw new InvalidDataException($"Offset value {value} is greater than the expected data size {this.header.Size}");
            }
        }

        /// <summary>
        /// Creates a ConsentUIData object from a previously-obtained raw byte representation
        /// </summary>
        /// <param name="consentUIDataStructure">The raw bytes of a supported ConsentUI data structure</param>
        /// <returns>A ConsentUIData object</returns>
        public static ConsentUIData GetConsentUIData(byte[] consentUIDataStructure)
        {
            SafeHGlobalHandle pData = SafeHGlobalHandle.AllocHGlobal(consentUIDataStructure.Length);
            Marshal.Copy(consentUIDataStructure, 0, pData.ToIntPtr(), consentUIDataStructure.Length);
            return CreateInstance(pData.ToIntPtr(), consentUIDataStructure.Length);
        }

        /// <summary>
        /// Gets the data structure passed to the Consent UI process
        /// </summary>
        /// <returns>A ConsentUIData object</returns>
        public static ConsentUIData GetConsentUIData()
        {
            return GetConsentUIData(false);
        }

        /// <summary>
        /// Gets the data structure passed to the Consent UI process
        /// </summary>
        /// <param name="forceRefresh">Forces the data structure to be re-read from the process</param>
        /// <returns>A ConsentUIData object</returns>
        public static ConsentUIData GetConsentUIData(bool forceRefresh)
        {
            if (cachedInstance == null || forceRefresh)
            {
                var pData = GetConsentUIData(out int structSize);
                cachedInstance = ConsentUIData.CreateInstance(pData.ToIntPtr(), structSize);
            }

            return cachedInstance;
        }

        /// <summary>
        /// Gets a value indicating whether the current process is consent.exe, indicating that the provider is running inside an elevated UAC prompt
        /// </summary>
        /// <returns></returns>
        public static bool IsConsentUIParent()
        {
            if (isConsentUI == null)
            {
                var consentPath = Environment.ExpandEnvironmentVariables("%systemroot%\\system32\\consent.exe");
                var process = Process.GetCurrentProcess();
                var callingProcess = process.MainModule?.FileName;
                isConsentUI = string.Equals(callingProcess, consentPath, StringComparison.OrdinalIgnoreCase);
            }

            return isConsentUI.Value;
        }

        /// <summary>
        /// Gets the raw bytes of the ConsentUI data structure
        /// </summary>
        /// <returns>A byte array</returns>
        public static byte[] GetConsentUIDataRawBytes()
        {
            var pData = GetConsentUIData(out int structSize);
            return GetRawBytes(pData.ToIntPtr(), structSize);
        }

        /// <summary>
        /// Parses the command line of the consent.exe process to retrieve the data structure passed to it
        /// </summary>
        /// <param name="size">Returns the size of the data structure as reported in the command line arguments</param>
        /// <returns>A pointer to the newly created copy of the data structure</returns>
        /// <exception cref="InvalidOperationException">Thrown when either consent.exe is not the parent process</exception>
        /// <exception cref="ArgumentException">Throw when the arguments passed to consent.exe are invalid</exception>
        private static SafeHGlobalHandle GetConsentUIData(out int size)
        {
            if (!IsConsentUIParent())
            {
                throw new InvalidOperationException("The consent UI data can only be retrieved when consent.exe is the parent process");
            }

            commandLineArgs ??= GetConsentUICommandLineArgs();

            size = commandLineArgs.Size;
            return ReadMemoryFromProcess(commandLineArgs.AppInfoProcessId, commandLineArgs.Address, commandLineArgs.Size);
        }


        /// <summary>
        /// Extracts the command line arguments passed to the consent.exe process
        /// </summary>
        /// <returns>A ConsentUICommandLineArgs object containing the arguments parsed from the command line</returns>
        /// <exception cref="ArgumentException">Thrown when the arguments passed to consent.exe cannot be parsed or are of the incorrect number</exception>
        private static ConsentUICommandLineArgs GetConsentUICommandLineArgs()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length != 4)
            {
                throw new ArgumentException($"Unable to parse command line of consent.exe. The number of elements was incorrect\r\n{string.Join("\r\n", args)}");
            }

            if (!uint.TryParse(args[1], out var appInfoPid))
            {
                throw new ArgumentException($"Unable to parse command line of consent.exe. The expected first element was not an integer\r\n{string.Join("\r\n", args)}");
            }

            if (!int.TryParse(args[2], out var size))
            {
                throw new ArgumentException($"Unable to parse command line of consent.exe. The expected second element was not an integer\r\n{string.Join("\r\n", args)}");
            }

            if (!long.TryParse(args[3], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var address))
            {
                throw new ArgumentException($"Unable to parse command line of consent.exe. The expected third element was not an integer\r\n{string.Join("\r\n", args)}");
            }

            return new ConsentUICommandLineArgs
            {
                Address = address,
                AppInfoProcessId = appInfoPid,
                Size = size,
            };
        }

        /// <summary>
        /// Copies the memory from a raw pointer into a managed byte array
        /// </summary>
        /// <param name="pData">The pointer where the data copy must start</param>
        /// <param name="size">The number of bytes to copy</param>
        /// <returns>A copy of the raw memory returned as a managed byte array</returns>
        private static byte[] GetRawBytes(IntPtr pData, int size)
        {
            byte[] dataForExport = new byte[size];
            Marshal.Copy(pData, dataForExport, 0, size);
            return dataForExport;
        }

        /// <summary>
        /// Reads the memory from a specified process
        /// </summary>
        /// <param name="processId">The ID of the process</param>
        /// <param name="address">The memory address to read</param>
        /// <param name="size">The size of the data at the specified memory address</param>
        /// <returns>A handle to a copy of the process memory</returns>
        /// <exception cref="Win32Exception">Thrown when the process could not be opened or the memory address could not be read</exception>
        /// <exception cref="InvalidDataException">Thrown when the size of the copied structure did not equal the expected size as passed to the method</exception>
        private static SafeHGlobalHandle ReadMemoryFromProcess(uint processId, long address, int size)
        {
            SafeHGlobalHandle pData = SafeHGlobalHandle.AllocHGlobal(size);
            var pAddress = new IntPtr(address);

            SafeFileHandle hProcess = OpenProcessHandle(processId, PROCESS_ACCESS_RIGHTS.PROCESS_VM_READ);

            unsafe
            {
                nuint numberOfBytesRead = 0;

                if (!NativeMethods.ReadProcessMemory(hProcess, pAddress.ToPointer(), pData.ToIntPtr().ToPointer(), (nuint)size, &numberOfBytesRead))
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error, $"Unable to read memory from process {processId}");
                }

                if (numberOfBytesRead != (nuint)size)
                {
                    throw new InvalidDataException($"Bytes read from memory {numberOfBytesRead} was not the expected structure size {size}");
                }
            }

            return pData;
        }

        /// <summary>
        /// Opens a native handle to a process
        /// </summary>
        /// <param name="processId">The ID of the process</param>
        /// <param name="rights">The requested access rights</param>
        /// <returns>A safe handle to the process</returns>
        /// <exception cref="Win32Exception">Thrown when the process handle could not be obtained</exception>
        private static SafeFileHandle OpenProcessHandle(uint processId, PROCESS_ACCESS_RIGHTS rights)
        {
            var hProcess = NativeMethods.OpenProcess_SafeHandle(rights, false, processId);
            if (hProcess.IsInvalid)
            {
                int error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error, $"Unable to open process {processId}");
            }

            return hProcess;
        }

        /// <summary>
        /// Creates an instance of the appropriate subclass of ConsentUIData by reading the type from the data structure
        /// </summary>
        /// <param name="pData">A pointer to the data structure</param>
        /// <param name="expectedSize">The expected size of the data structure</param>
        /// <returns>A ConsentUIData object</returns>
        /// <exception cref="InvalidDataException">Thrown when the size of the expected data structure does not match the size reported in the structure itself</exception>
        private static ConsentUIData CreateInstance(IntPtr pData, int expectedSize)
        {
            var sizeReportedInStructure = Marshal.ReadInt32(pData, 0);

            if (sizeReportedInStructure != expectedSize)
            {
                throw new InvalidDataException($"The expected size {expectedSize} did not match the size reported by the structure {sizeReportedInStructure}");
            }

            var type = (ConsentUIType)Marshal.ReadInt32(pData, 4);

            return type switch
            {
                ConsentUIType.Exe => new ConsentUIDataExe(pData, sizeReportedInStructure),
                ConsentUIType.Msi => new ConsentUIDataMsi(pData, sizeReportedInStructure),
                ConsentUIType.Com => new ConsentUIDataCom(pData, sizeReportedInStructure),
                ConsentUIType.Msix => new ConsentUIDataMsix(pData, sizeReportedInStructure),
                ConsentUIType.ActiveX => new ConsentUIDataActiveX(pData, sizeReportedInStructure),
                ConsentUIType.CredCollect => new ConsentUIDataCredCollect(pData, sizeReportedInStructure),
                _ => throw new InvalidDataException("The ConsentUI data structure was for an unknown type"),
            };
        }

        /// <summary>
        /// Duplicates a handle passed in from the AppInfo service
        /// </summary>
        /// <param name="handle">The handle to duplicate</param>
        /// <returns>A duplicated reference to the handle</returns>
        /// <exception cref="Win32Exception">Thrown when the handle could not be duplicated</exception>
        private protected static SafeHandle DuplicateHandleInternal(IntPtr handle)
        {
            commandLineArgs ??= GetConsentUICommandLineArgs();

            var processHandle = OpenProcessHandle(commandLineArgs.AppInfoProcessId, PROCESS_ACCESS_RIGHTS.PROCESS_DUP_HANDLE);
            SafeFileHandle t = new(handle, false);

            if (!NativeMethods.DuplicateHandle(processHandle, t, Process.GetCurrentProcess().SafeHandle, out var duplicatedToken, 0, false, DUPLICATE_HANDLE_OPTIONS.DUPLICATE_SAME_ACCESS))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to duplicate the handle");
            }

            return duplicatedToken;
        }
    }
}