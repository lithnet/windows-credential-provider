using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents the data structure passed to consent UI in a yet unknown scenario
    /// </summary>
    /// <remarks>This data structure is currently unknown and only the common header values are present</remarks>
    public class ConsentUIDataCredCollect : ConsentUIData
    {
        internal ConsentUIDataCredCollect(IntPtr pData, int expectedSize) : base(pData, expectedSize)
        {
            if (this.header.Type != ConsentUIType.CredCollect)
            {
                throw new InvalidOperationException("The data structure is not of type CredCollect");
            }
        }
    }
}
