using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Lithnet.CredentialProvider.UnitTests
{
    public class BitmapControlTests
    {
        [Test]
        public void TransparentBufferPreservesAlphaChannel()
        {
            using (Bitmap source = new Bitmap(2, 1, PixelFormat.Format32bppArgb))
            {
                source.SetPixel(0, 0, Color.FromArgb(0, 10, 20, 30));
                source.SetPixel(1, 0, Color.FromArgb(128, 40, 50, 60));

                var control = new UserTileControl("image", "Image", source);

                byte[] bytes = GetBitmapBuffer(control);

                Assert.That(bytes, Has.Length.GreaterThan(8));
                Assert.That(bytes[0], Is.EqualTo(0x89));
                Assert.That(bytes[1], Is.EqualTo(0x50));
                Assert.That(bytes[2], Is.EqualTo(0x4e));
                Assert.That(bytes[3], Is.EqualTo(0x47));

                using (MemoryStream stream = new MemoryStream(bytes))
                using (Bitmap decoded = new Bitmap(stream))
                {
                    Assert.That(decoded.GetPixel(0, 0).A, Is.EqualTo(0));
                    Assert.That(decoded.GetPixel(1, 0).A, Is.EqualTo(128));
                    Assert.That(decoded.GetPixel(1, 0).R, Is.EqualTo(40));
                    Assert.That(decoded.GetPixel(1, 0).G, Is.EqualTo(50));
                    Assert.That(decoded.GetPixel(1, 0).B, Is.EqualTo(60));
                }
            }
        }

        [Test]
        public void BitmapBufferDoesNotApplyConfiguredBackgroundColor()
        {
            using (Bitmap source = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
            {
                source.SetPixel(0, 0, Color.Transparent);

                var control = new UserTileControl("image", "Image", source)
                {
                    BackgroundColor = Color.FromArgb(12, 34, 56)
                };

                byte[] bytes = GetBitmapBuffer(control);

                using (MemoryStream stream = new MemoryStream(bytes))
                using (Bitmap decoded = new Bitmap(stream))
                {
                    Assert.That(decoded.GetPixel(0, 0).A, Is.EqualTo(0));
                }
            }
        }

        [Test]
        public void CloneCopiesBitmapAndBackgroundColor()
        {
            using (Bitmap source = new Bitmap(1, 1))
            {
                var control = new UserTileControl("image", "Image", source)
                {
                    BackgroundColor = Color.CornflowerBlue
                };

                var clone = (UserTileControl)control.Clone();

                Assert.That(clone.Bitmap, Is.SameAs(source));
                Assert.That(clone.BackgroundColor, Is.EqualTo(Color.CornflowerBlue));
            }
        }

        private static byte[] GetBitmapBuffer(BitmapControl control)
        {
            IntPtr buffer = control.GetBitmapBuffer(out uint size);

            try
            {
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, checked((int)size));
                return bytes;
            }
            finally
            {
                Marshal.FreeCoTaskMem(buffer);
            }
        }
    }
}
