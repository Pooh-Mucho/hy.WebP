// ==++==
// 
//    Copyright (c) 2017 Hui Yi <yi.webmaster@gmail.com>.  All rights reserved.
//   
//    The use and distribution terms for this software are contained in the file
//    named license.txt, which can be found in the root of this distribution.
//    By using this software in any fashion, you are agreeing to be bound by the
//    terms of this license.
//   
//    You must not remove this notice, or any other, from this software.
//
//    $Id: ImageArgbData.cs
//   
// ==--==

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;

using hy.WebP.Interop;

namespace hy.WebP
{
    /// <summary>
    /// The ImageArgbData class represents the ARGB data of an image.
    /// </summary>
    public class ImageArgbData
    {
        /// <summary>
        /// The ARGB data of the image, in 32-bit format as [B, G, R, A, B, G, R, A, ...]
        /// </summary>
        public readonly byte[] ArgbBytes;
        /// <summary>
        /// The stride width (or called scan width) of the ARGB data, in bytes.
        /// </summary>
        public readonly int Stride;
        /// <summary>
        /// The width of the image.
        /// </summary>
        public readonly int Width;
        /// <summary>
        /// The height of the image.
        /// </summary>
        public readonly int Height;

        internal ImageArgbData(int width, int height, int stride, byte[] argbBytes)
        {
            this.Width = width;
            this.Height = height;
            this.Stride = stride;
            this.ArgbBytes = argbBytes;
        }

        /// <summary>
        /// Create an ImageArgbData instance with the specified width and height.
        /// </summary>
        /// <param name="width"> The width of the image. </param>
        /// <param name="height"> The height of the image. </param>
        public ImageArgbData(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("image", width, "width cannot be 0");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("image", height, "height cannot be 0");
            if (width > _WebPConst.WEBP_MAX_DIMENSION)
                throw new ArgumentOutOfRangeException("image", width, "width cannot exceed WEBP_MAX_DIMENSION");
            if (height > _WebPConst.WEBP_MAX_DIMENSION)
                throw new ArgumentOutOfRangeException("image", height, "height cannot exceed WEBP_MAX_DIMENSION");

            this.Width = width;
            this.Height = height;
            this.Stride = width * 4;
            this.ArgbBytes = new byte[width * height * 4];
        }

        /// <summary>
        /// Get or set the color of the pixel at the specified position.
        /// </summary>
        /// <param name="x"> The x-coordinate of the pixel. </param>
        /// <param name="y"> The y-coordinate of the pixel. </param>
        /// <returns> The color of the pixel at the specified position. </returns>
        public unsafe Color this[int x, int y]
        {
            get
            {
                int width = Width;
                if (x < 0 || x >= width)
                    throw new ArgumentOutOfRangeException("x");
                if (y < 0 || y >= Height)
                    throw new ArgumentOutOfRangeException("y");
                int argb = *((int*)ArgbBytes[(y * width + x) * 4]);
                return Color.FromArgb(argb);
            }
            set
            {
                int width = Width;
                if (x < 0 || x >= width)
                    throw new ArgumentOutOfRangeException("x");
                if (y < 0 || y >= Height)
                    throw new ArgumentOutOfRangeException("y");
                uint argb = ((uint)value.A << 24) | ((uint)value.R << 16) | ((uint)value.G << 8) | ((uint)value.B);
                *((uint*)ArgbBytes[(y * width + x) * 4]) = argb;
            }
        }

        /// <summary>
        /// Convert the ImageArgbData to a bitmap, using Argb32 pixel format.
        /// </summary>
        /// <returns>The converted bitmap</returns>
        public unsafe Bitmap ToBitmap()
        {
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            BitmapData dat = bmp.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try {
                byte[] src = ArgbBytes;
                int srcIndex = 0;
                int srcStride = Width * 4;
                byte* dst = (byte*)dat.Scan0;
                int dstIndex = 0;
                int dstStride = dat.Stride;
                for (int y = 0; y < Height; y++) {
                    Marshal.Copy(src, srcIndex, new IntPtr(dst + dstIndex), Width * 4);
                    srcIndex += srcStride;
                    dstIndex += dstStride;
                }
            }
            finally {
                bmp.UnlockBits(dat);
            }
            return bmp;
        }

        /// <summary>
        /// Create the ImageArgbData from a file. The maximum supported image size is 16382x16383.
        /// </summary>
        /// <param name="filePath"> The path of the file. </param>
        /// <returns> The ARGB data of the image </returns>
        public static ImageArgbData FromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 8192)) {
                using (Image image = Image.FromStream(stream, false, false)) {
                    return ImageArgbData.FromImage(image);
                }
            }
        }

        /// <summary>
        /// Create the ImageArgbData from an image. The maximum supported image size is 16382x16383.
        /// </summary>
        /// <param name="image"> The image to dump ARGB data from. </param>
        /// <returns> The ARGB data of the image </returns>
        public unsafe static ImageArgbData FromImage(Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            Size size = image.Size;
            if (size.Width <= 0)
                throw new ArgumentOutOfRangeException("image", size, "image.Width cannot be 0");
            if (size.Height <= 0)
                throw new ArgumentOutOfRangeException("image", size, "image.Height cannot be 0");
            if (size.Width > _WebPConst.WEBP_MAX_DIMENSION)
                throw new ArgumentOutOfRangeException("image", size, "image.Width cannot exceed WEBP_MAX_DIMENSION");
            if (size.Height > _WebPConst.WEBP_MAX_DIMENSION)
                throw new ArgumentOutOfRangeException("image", size, "image.Height cannot exceed WEBP_MAX_DIMENSION");

            if (image is Bitmap) {
                return FromBitmap((Bitmap)image, new Rectangle(0, 0, size.Width, size.Height));
            }
            using (Bitmap bmp = CreateBitmap(image, new Rectangle(0, 0, size.Width, size.Height))) {
                return FromBitmap(bmp, new Rectangle(0, 0, size.Width, size.Height));
            }
        }

        /// <summary>
        /// Create the ImageArgbData from an image, with the specified clip rectangle. The maximum supported clip size is 16382x16383.
        /// </summary>
        /// <param name="image"> The image to dump ARGB data from. </param>
        /// <param name="clipRect"> The clip rectangle </param>
        /// <returns> The ARGB data of the image </returns>
        public static ImageArgbData FromImage(Image image, Rectangle clipRect)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            Size size = image.Size;
            if (clipRect.Left < 0 || clipRect.Left > size.Width)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "Invalid clipRect.Left");
            if (clipRect.Right < 0 || clipRect.Left > size.Width)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "Invalid clipRect.Right");
            if (clipRect.Top < 0 || clipRect.Top > size.Height)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "Invalid clipRect.Top");
            if (clipRect.Bottom < 0 || clipRect.Bottom > size.Height)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "Invalid clipRect.Bottom");
            if (clipRect.Width <= 0)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "Invalid clipRect.Width");
            if (clipRect.Height <= 0)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "Invalid clipRect.Height");
            if (clipRect.Width > _WebPConst.WEBP_MAX_DIMENSION)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "clipRect.Width cannot exceed WEBP_MAX_DIMENSION");
            if (clipRect.Height > _WebPConst.WEBP_MAX_DIMENSION)
                throw new ArgumentOutOfRangeException("clipRect", clipRect, "clipRect.Height cannot exceed WEBP_MAX_DIMENSION");

            if (image is Bitmap) {
                return FromBitmap((Bitmap)image, clipRect);
            }
            using (Bitmap bmp = CreateBitmap(image, clipRect)) {
                return FromBitmap(bmp, new Rectangle(0, 0, clipRect.Width, clipRect.Height));
            }
        }

        internal static Bitmap CreateBitmap(Image image, Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                g.CompositingMode = CompositingMode.SourceCopy;
                g.Clear(Color.Transparent);
                if (rect.X == 0 && rect.Y == 0 && rect.Size == image.Size) {
                    g.DrawImage(image, 0, 0);
                }
                else {
                    g.DrawImage(image, 0, 0, rect, GraphicsUnit.Pixel);
                }
            }
            return bmp;
        }

        private static ImageArgbData FromBitmap(Bitmap bmp, Rectangle rect)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb) {
                BitmapData bmpDat = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                try {
                    return FromFormat24bppRgb(bmpDat);
                }
                finally {
                    bmp.UnlockBits(bmpDat);
                }
            }
            else {
                BitmapData bmpDat = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try {
                    return FromFormat32bppArgb(bmpDat);
                }
                finally {
                    bmp.UnlockBits(bmpDat);
                }
            }
        }

        internal static unsafe ImageArgbData FromFormat24bppRgb(BitmapData data)
        {
            int width = data.Width;
            int height = data.Height;
            int srcStride = data.Stride;
            int dstStride = width * 4;

            ImageArgbData result = new ImageArgbData(width, height, dstStride, new byte[width * height * 4]);

            fixed (byte* pDst = result.ArgbBytes) {
                byte* pSrc = (byte*)data.Scan0;
                for (int y = 0; y < height; y++) {
                    byte* srcLine = (byte*)(pSrc + y * srcStride);
                    uint* dstLine = (uint*)(pDst + y * dstStride);
                    for (int x = 0; x < width; x++) {
                        *dstLine = ((uint)srcLine[0]) | ((uint)srcLine[1] << 8) | ((uint)srcLine[2] << 16) | 0xff000000;
                        dstLine += 1;
                        srcLine += 3;
                    }
                }
            }

            return result;
        }

        internal static unsafe ImageArgbData FromFormat32bppArgb(BitmapData data)
        {
            int width = data.Width;
            int height = data.Height;
            int srcStride = data.Stride;
            int dstStride = width * 4;

            ImageArgbData result = new ImageArgbData(width, height, dstStride, new byte[width*height*4]);

            fixed (byte* pDst = result.ArgbBytes) {
                byte* pSrc = (byte*)data.Scan0;
                for (int y = 0; y < height; y++) {
                    uint* srcLine = (uint*)(pSrc + y * srcStride);
                    uint* dstLine = (uint*)(pDst + y * dstStride);
                    for (int x = 0; x < width; x++) {
                        *dstLine++ = *srcLine++;
                    }
                }
            }

            return result;
        }

    }
}
