using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// The base class of image-based controls
    /// </summary>
    public abstract class BitmapControl : ControlBase
    {
        private Bitmap bitmap;
        private Color backgroundColor;

        protected BitmapControl(string key, string label, bool isProviderLogo, Bitmap bitmap) :
            base(key, label, FieldType.TileImage, isProviderLogo ? Guid.Parse(CredProviderConstants.CPFG_CREDENTIAL_PROVIDER_LOGO) : Guid.Empty)
        {
            this.bitmap = bitmap;
            this.backgroundColor = Color.FromArgb(70, 70, 70);
        }

        protected BitmapControl(BitmapControl source) : base(source)
        {
            this.bitmap = source.bitmap;
            this.backgroundColor = source.backgroundColor;
        }

        /// <summary>
        /// Specifies the background color used to replace transparent pixels for <see cref="CredentialTile"/> and <see cref="CredentialTile2"/>. This defaults to #707070.
        /// </summary>
        /// <remarks>This property does not apply to <see cref="CredentialTile3"/>.</remarks>
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
        /// The image to be displayed
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
