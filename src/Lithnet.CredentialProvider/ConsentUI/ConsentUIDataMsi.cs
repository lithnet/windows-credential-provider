﻿using System;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the data structure passed to consent UI when a user is trying to elevate an MSI installer
    /// </summary>
    public class ConsentUIDataMsi : ConsentUIData
    {
        /// <summary>
        /// The name of the product being installed
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// The version of the product being installed
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// The locale of the product being installed
        /// </summary>
        public string Locale { get; }

        /// <summary>
        /// The publisher of the product being installed
        /// </summary>
        public string Publisher { get; }

        /// <summary>
        /// The path to the MSI installer
        /// </summary>
        public string ExecutionPath { get; }

        /// <summary>
        /// The path to the original MSI file launched by the user
        /// </summary>
        public string OriginalMsi { get; }

        /// <summary>
        /// A currently unknown parameter
        /// </summary>
        public string Unknown1 { get; }

        /// <summary>
        /// A currently unknown parameter
        /// </summary>
        public string Unknown2 { get; }

        internal ConsentUIDataMsi(IntPtr pData, int expectedSize) : base(pData, expectedSize)
        {
            if (this.header.Type != ConsentUIType.Msi)
            {
                throw new InvalidOperationException("The data structure is not of type MSI");
            }

            var s = Marshal.PtrToStructure<ConsentUIStructureMsi>(pData);

            this.ProductName = this.GetStringValueIfValid(pData, (int)s.oProductName);
            this.Version = this.GetStringValueIfValid(pData, (int)s.oVersion);
            this.Locale = this.GetStringValueIfValid(pData, (int)s.oLocale);
            this.Publisher = this.GetStringValueIfValid(pData, (int)s.oPublisher);
            this.ExecutionPath = this.GetStringValueIfValid(pData,   (int)s.oExecutionPath);
            this.OriginalMsi = this.GetStringValueIfValid(pData,    (int)s.oOriginalMsi);
            this.Unknown1 = this.GetStringValueIfValid(pData, (int)s.oUnknown1);
            this.Unknown2 = this.GetStringValueIfValid(pData, (int)s.oUnknown2);
        }
    }
}