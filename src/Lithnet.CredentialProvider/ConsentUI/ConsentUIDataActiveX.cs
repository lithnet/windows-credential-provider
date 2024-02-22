using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the data structure passed to consent UI when a user is trying to install an ActiveX control
    /// </summary>
    /// <remarks>This data structure is currently unknown and only the common header values are present</remarks>
    public class ConsentUIDataActiveX : ConsentUIData
    {
        internal ConsentUIDataActiveX(IntPtr pData, int expectedSize) : base(pData, expectedSize)
        {
            if (this.header.Type != ConsentUIType.ActiveX)
            {
                throw new InvalidOperationException("The data structure is not of type ActiveX");
            }
        }
    }
}
