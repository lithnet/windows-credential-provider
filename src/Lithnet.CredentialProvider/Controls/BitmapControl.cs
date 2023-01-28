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

        protected BitmapControl(BitmapControl source) : base(source) { }

        /// <summary>
        /// Specifies the background color that should replace any transparent elements of the image. This defaults to #707070
        /// </summary>
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

                    if (this.Events is ICredentialProviderCredentialEvents3 e)
                    {
                        var buffer = this.GetBitmapBuffer(out uint size);
                        e.SetFieldBitmapBuffer(this.Credential, this.Id, size, buffer);
                    }

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
            var hbitmap = this.GetHBitmap();

            if (hbitmap == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            var image = Bitmap.FromHbitmap(hbitmap);

            IntPtr buffer = IntPtr.Zero;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                var bitmapBytes = ms.ToArray();
                size = (uint)bitmapBytes.Length;
                buffer = Marshal.AllocCoTaskMem(bitmapBytes.Length);
                Marshal.Copy(bitmapBytes, 0, buffer, bitmapBytes.Length);
            }

            return buffer;
        }
    }
}