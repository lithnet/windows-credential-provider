using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Provides common image behaviour for credential provider logo and user tile controls.
    /// </summary>
    public abstract class BitmapControl : ControlBase
    {
        private Bitmap bitmap;
        private Color backgroundColor;

        /// <summary>
        /// Initializes an image control.
        /// </summary>
        /// <param name="key">The unique key for the control.</param>
        /// <param name="label">The label associated with the control.</param>
        /// <param name="isProviderLogo"><see langword="true"/> to identify the image as the credential provider logo; otherwise, <see langword="false"/>.</param>
        /// <param name="bitmap">The initial image displayed by the control.</param>
        protected BitmapControl(string key, string label, bool isProviderLogo, Bitmap bitmap) :
            base(key, label, FieldType.TileImage, isProviderLogo ? Guid.Parse(CredProviderConstants.CPFG_CREDENTIAL_PROVIDER_LOGO) : Guid.Empty)
        {
            this.bitmap = bitmap;
            this.backgroundColor = Color.FromArgb(70, 70, 70);
        }

        /// <summary>
        /// Initializes an image control by copying an existing image control.
        /// </summary>
        /// <param name="source">The image control to copy.</param>
        protected BitmapControl(BitmapControl source) : base(source)
        {
            this.bitmap = source.bitmap;
            this.backgroundColor = source.backgroundColor;
        }

        /// <summary>
        /// Gets or sets the background color used when an image that contains transparency is displayed by a <see cref="CredentialTile"/> or <see cref="CredentialTile2"/>.
        /// </summary>
        /// <remarks>The default color is #464646. A <see cref="CredentialTile3"/> preserves the image's alpha channel and does not use this property.</remarks>
        public Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set
            {
                if (this.backgroundColor != value)
                {
                    this.backgroundColor = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the image displayed by the control.
        /// </summary>
        public Bitmap Bitmap
        {
            get { return this.bitmap; }
            set
            {

                if (this.bitmap != value)
                {
                    this.bitmap = value;

                    this.UpdateBitmap();

                    this.RaisePropertyChanged();
                }
            }
        }

        internal IntPtr GetHBitmap()
        {
            if (this.bitmap == null)
            {
                return IntPtr.Zero;
            }

            return this.Bitmap.GetHbitmap(this.BackgroundColor);
        }

        internal IntPtr GetBitmapBuffer(out uint size)
        {
            size = 0;

            if (this.bitmap == null)
            {
                return IntPtr.Zero;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                this.bitmap.Save(ms, ImageFormat.Png);
                var bitmapBytes = ms.ToArray();
                size = checked((uint)bitmapBytes.Length);
                IntPtr buffer = Marshal.AllocCoTaskMem(bitmapBytes.Length);
                Marshal.Copy(bitmapBytes, 0, buffer, bitmapBytes.Length);
                return buffer;
            }
        }

        private void UpdateBitmap()
        {
            if (this.Credential is ICredentialProviderCredential3 && this.Events is ICredentialProviderCredentialEvents3 events3)
            {
                IntPtr buffer = this.GetBitmapBuffer(out uint size);

                try
                {
                    events3.SetFieldBitmapBuffer(this.Credential, this.Id, size, buffer);
                }
                finally
                {
                    if (buffer != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(buffer);
                    }
                }

                return;
            }

            if (this.Events is ICredentialProviderCredentialEvents2 events2)
            {
                events2.SetFieldBitmap(this.Credential, this.Id, this.GetHBitmap());
            }
        }
    }
}
