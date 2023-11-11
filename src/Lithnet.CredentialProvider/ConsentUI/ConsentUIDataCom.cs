using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the data structure passed to consent UI when a user is trying to elevate a COM component. This is typically seen when a user presses a 'shield' icon in something like the file security dialog to elevate permissions.
    /// </summary>
    public class ConsentUIDataCom : ConsentUIData
    {
        /// <summary>
        /// The path to the COM component that is requesting elevation
        /// </summary>
        public string ComComponentPath { get; }

        /// <summary>
        /// The resource path to the image to display in consent UI
        /// </summary>
        public string ImageResourcePath { get; }

        /// <summary>
        /// The path to the process that is hosting the COM component
        /// </summary>
        public string ProcessPath { get; }

        /// <summary>
        /// A user friendly description of the type of operation that will be performed by the elevation
        /// </summary>
        public string OperationType { get; }

        /// <summary>
        /// The CLSID of the COM component
        /// </summary>
        public Guid ClsId { get; }

        internal ConsentUIDataCom(IntPtr pData, int expectedSize) : base(pData, expectedSize)
        {
            if (this.header.Type != ConsentUIType.Com)
            {
                throw new InvalidOperationException("The data structure is not of type COM");
            }

            var s = Marshal.PtrToStructure<ConsentUIStructureCom>(pData);

            this.ComComponentPath = this.GetStringValueIfValid(pData, (int)s.oComComponentPath);
            this.ImageResourcePath = this.GetStringValueIfValid(pData, (int)s.oImageResourcePath);
            this.ProcessPath = this.GetStringValueIfValid(pData, (int)s.oProcessPath);
            this.OperationType = this.GetStringValueIfValid(pData, (int)s.oOperationType);
            this.ClsId = s.Clsid;
        }
    }
}
