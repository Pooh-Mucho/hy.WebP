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
//    $Id: WebPCodec.cs
//   
// ==--==

using System;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using hy.WebP.Interop;

namespace hy.WebP
{
    /// <summary>
    /// The WebPCodec class provides methods to encode and decode WebP images, and PSNR calculation for image quality comparison.
    /// </summary>
    public static class WebPCodec
    {
        // WebP file header. The first 12 bytes of a WebP file should be:
        //   'R' 'I' 'F' 'F' * * * * 'W' 'E' 'B' 'P'
        private static readonly byte[] RiffHeader = Encoding.ASCII.GetBytes("RIFF");
        private static readonly byte[] WebpFourCC = Encoding.ASCII.GetBytes("WEBP");
        private static readonly int WebPFileHeaderSize = 12;

        private static LibWebP _lib;

        /// <summary>
        /// The handle to the native libwebp library.
        /// </summary>
        private static LibWebP Lib
        {
            get
            {
                if (_lib == null) {
                    lock (typeof(WebPCodec)) {
                        if (_lib == null) {
                            string dir = Path.GetDirectoryName(typeof(WebPCodec).Assembly.Location);
                            if (IntPtr.Size == 4) {
                                string fileName140 = Path.Combine(dir, "libwebp140_x86.dll");
                                if (File.Exists(fileName140)) {
                                    _lib = new LibWebP(fileName140, _WebPLibVersion.WEBP_VERSION_140);
                                    return _lib;
                                }
                                string fileName120 = Path.Combine(dir, "libwebp120_x86.dll");
                                if (File.Exists(fileName120)) {
                                    _lib = new LibWebP(fileName120, _WebPLibVersion.WEBP_VERSION_120);
                                    return _lib;
                                }
                                try {
                                    File.WriteAllBytes(fileName140, Resources.libwebp140_x86);
                                    _lib = new LibWebP(fileName140, _WebPLibVersion.WEBP_VERSION_140);
                                    return _lib;
                                }
                                catch { }
                                string tempFile = Path.Combine(Path.GetTempPath(), "libwebp140_x86.dll");
                                File.WriteAllBytes(tempFile, Resources.libwebp140_x86);
                                _lib = new LibWebP(tempFile, _WebPLibVersion.WEBP_VERSION_140);
                                return _lib;
                            }
                            else {
                                string fileName140 = Path.Combine(dir, "libwebp140_x64.dll");
                                if (File.Exists(fileName140)) {
                                    _lib = new LibWebP(fileName140, _WebPLibVersion.WEBP_VERSION_140);
                                    return _lib;
                                }
                                string fileName120 = Path.Combine(dir, "libwebp120_x64.dll");
                                if (File.Exists(fileName120)) {
                                    _lib = new LibWebP(fileName120, _WebPLibVersion.WEBP_VERSION_120);
                                    return _lib;
                                }
                                try {
                                    File.WriteAllBytes(fileName140, Resources.libwebp140_x64);
                                    _lib = new LibWebP(fileName140, _WebPLibVersion.WEBP_VERSION_140);
                                    return _lib;
                                }
                                catch { }
                                string tempFile = Path.Combine(Path.GetTempPath(), "libwebp140_x64.dll");
                                File.WriteAllBytes(tempFile, Resources.libwebp140_x64);
                                _lib = new LibWebP(tempFile, _WebPLibVersion.WEBP_VERSION_140);
                                return _lib;
                            }
                        }
                    }
                }
                return _lib;
            }
        }

        /// <summary>
        /// Check if the file header bytes contains a valid webp file header.
        /// </summary>
        /// <param name="bytes"> The byte array containing the file header bytes, at least 12 bytes length.</param>
        /// <returns> Return true if bytes contains a valid webp header, false otherwise.</returns>
        public static bool IsWebPFileHeader(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (bytes.Length < WebPFileHeaderSize)
                return false;
            for (int i = 0; i < 4; i++) {
                if (bytes[i] != RiffHeader[i])
                    return false;
            }
            for (int i = 8; i < 12; i++) {
                if (bytes[i] != WebpFourCC[i - 8])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Create a WebPConfig object with the specified quality and preset WEBP_PRESET_DEFAULT.
        /// </summary>
        /// <param name="quality"> The quality factor, between 0.0 and 100.0 </param>
        /// <returns> A WebPConfig object with the specified quality and WEBP_PRESET_DEFAULT preset </returns>
        public static WebPConfig CreateConfig(float quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality", quality, "Invalid quality");
            Interop._WebPConfig inner = new _WebPConfig();
            inner.lossless = 0;
            int err = Lib.WebPConfigInit(ref inner, _WebPPreset.WEBP_PRESET_DEFAULT, quality);
            if (err == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION mismatch");
            inner.method = 6;
            inner.filter_sharpness = 0;
            inner.filter_strength = 100;
            inner.use_sharp_yuv = 1;
            inner.pass = 3;
            return new WebPConfig(inner);
        }

        /// <summary>
        /// Create a WebPConfig object with the specified quality, and preset.
        /// </summary>
        /// <param name="preset"> The preset to use </param>
        /// <param name="quality"> The quality factor, between 0.0 and 100.0 </param>
        /// <returns> A WebPConfig object with the specified quality and preset </returns>
        /// <seealso cref="WebPPreset"/>
        public static WebPConfig CreateConfigPreset(WebPPreset preset, float quality)
        {
            switch (preset) {
                case WebPPreset.Default: break;
                case WebPPreset.Picture: break;
                case WebPPreset.Photo: break;
                case WebPPreset.Drawing: break;
                case WebPPreset.Text: break;
                default: throw new ArgumentException("Invalid preset", "preset");
            }
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality", quality, "Invalid quality");
            Interop._WebPConfig inner = new _WebPConfig();
            inner.lossless = 0;
            int err = Lib.WebPConfigInit(ref inner, _WebPPreset.WEBP_PRESET_DEFAULT, quality);
            if (err == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION mismatch");
            inner.method = 6;
            inner.filter_sharpness = 0;
            inner.filter_strength = 100;
            inner.use_sharp_yuv = 1;
            inner.pass = 3;
            return new WebPConfig(inner);
        }


        /// <summary>
        /// Create a WebPConfig object with lossless compression mode and the specified efficiency level.
        /// This function will overwrite several fields from config: 'method', 'quality' and 'lossless'.
        /// </summary>
        /// <param name="level"> 
        /// The efficiency level between 0 (fastest, lowest compression) and 9 (slower, best compression).
        /// A good default level is '6', providing a fair tradeoff between compression speed and final compressed size.
        /// </param>
        /// <returns> A WebPConfig object with lossless compression mode and the specified level </returns>
        public static WebPConfig CreateConfigLossless(int level)
        {
            if (level < 0 || level > 9)
                throw new ArgumentOutOfRangeException("level");
            Interop._WebPConfig inner = new _WebPConfig();
            int err = Lib.WebPConfigInit(ref inner, _WebPPreset.WEBP_PRESET_DEFAULT, 100);
            if (err == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION mismatch");
            inner.lossless = 1;
            err = Lib.WebPConfigLosslessPreset(ref inner, level);
            if (err == 0)
                throw new WebPEncodingException(-2, "Lossless parameter error");
            inner.method = 3;
            inner.use_sharp_yuv = 1;
            return new WebPConfig(inner);
        }

        /// <summary>
        /// Create a WebPConfig object with lossless compression mode.
        /// </summary>
        /// <returns> A WebPConfig object with lossless compression mode and the specified level </returns>
        public static WebPConfig CreateConfigLossless()
        {
            Interop._WebPConfig inner = new _WebPConfig();
            int err = Lib.WebPConfigInit(ref inner, _WebPPreset.WEBP_PRESET_DEFAULT, 100);
            if (err == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION mismatch");
            inner.lossless = 1;
            err = Lib.WebPConfigLosslessPreset(ref inner, 9);
            if (err == 0)
                throw new WebPEncodingException(-2, "Lossless parameter error");
            inner.method = 3;
            inner.use_sharp_yuv = 1;
            return new WebPConfig(inner);
        }

        /// <summary>
        /// Create a WebPConfig object with lossless compression mode(NearLosses = 99).
        /// </summary>
        /// <returns> A WebPConfig object with lossless compression mode and the specified level </returns>
        public static WebPConfig CreateConfigNearLossless()
        {
            Interop._WebPConfig inner = new _WebPConfig();
            int err = Lib.WebPConfigInit(ref inner, _WebPPreset.WEBP_PRESET_DEFAULT, 100);
            if (err == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION mismatch");
            inner.lossless = 1;
            err = Lib.WebPConfigLosslessPreset(ref inner, 9);
            if (err == 0)
                throw new WebPEncodingException(-2, "Lossless parameter error");
            inner.method = 3;
            inner.use_sharp_yuv = 1;
            inner.near_lossless = 99;
            return new WebPConfig(inner);
        }

        /// <summary>
        /// Dump the ARGB data from the image. The maximum supported image size is 16382x16383.
        /// </summary>
        /// <param name="image"> The image to dump ARGB data from. </param>
        /// <returns> The ARGB data of the image </returns>
        public unsafe static ImageArgbData DumpArgbData(Image image)
        {
            return ImageArgbData.FromImage(image);
        }

        /// <summary>
        /// Dump the ARGB data from the image, with the specified clip rectangle. The maximum supported clipRect size is 16382x16383.
        /// </summary>
        /// <param name="image"> The image to dump ARGB data from. </param>
        /// <param name="clipRect"> The clip rectangle </param>
        /// <returns> The ARGB data of the image </returns>
        public unsafe static ImageArgbData DumpArgbData(Image image, Rectangle clipRect)
        {
            return ImageArgbData.FromImage(image, clipRect);
        }

        /// <summary>
        /// Encode the image bytes to WebP format, using the specified configuration.
        /// </summary>
        /// <param name="config"> The configuration to use </param>
        /// <param name="image"> The image bytes to encode. Supported formats are PNG, JPEG, BMP, GIF, TIFF, and ICO. </param>
        /// <returns> The encoded WebP image bytes </returns>
        public static byte[] Encode(WebPConfig config, byte[] image)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (image == null || image.Length == 0)
                throw new ArgumentNullException("image");
            using (Image imageObj = Image.FromStream(new MemoryStream(image), false, false)) {
                return Encode(config, imageObj);
            }
        }

        /// <summary>
        /// Encode the image to WebP format, using the specified configuration.
        /// </summary>
        /// <param name="config"> The configuration to use </param>
        /// <param name="argbData"> The ARGB data of the image </param>
        /// <returns> The encoded WebP image bytes </returns>
        public static unsafe byte[] Encode(WebPConfig config, ImageArgbData argbData)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (argbData == null)
                throw new ArgumentNullException("argbData");
            if (Lib.WebPValidateConfig(ref config._inner) == 0)
                throw new WebPEncodingException(-3, "Invalid WebPConfig parameters");

            _WebPPicture picture = new _WebPPicture();
            if (Lib.WebPPictureInit(ref picture) == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION");

            picture.use_argb = 1;
            picture.width = argbData.Width;
            picture.height = argbData.Height;
            picture.argb_stride = argbData.Width;
            fixed (byte* p = argbData.ArgbBytes) {
                picture.argb = (uint*)p;
                ResultWriter writer = new ResultWriter();
                picture.writer = writer.WriterFunction;
                Lib.WebPEncode(ref config._inner, ref picture);
                WebPEncodingException.Check((int)picture.error_code);
                return writer.Result;
            }
        }

        /// <summary>
        /// Encode the image to WebP format, using the specified configuration.
        /// </summary>
        /// <param name="config"> The configuration to use </param>
        /// <param name="image"> The image to encode </param>
        /// <returns> The encoded WebP image bytes </returns>
        public unsafe static byte[] Encode(WebPConfig config, Image image)
        {
            if (config == null)
                throw new ArgumentNullException("config");
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
            if (Lib.WebPValidateConfig(ref config._inner) == 0)
                throw new WebPEncodingException(-3, "Invalid WebPConfig parameters");

            _WebPPicture picture = new _WebPPicture();
            if (Lib.WebPPictureInit(ref picture) == 0)
                throw new WebPEncodingException(-1, "WEBP_ENCODER_ABI_VERSION");

            Bitmap bmp;
            if (image is Bitmap) {
                bmp = (Bitmap)image;
            }
            else {
                bmp = ImageArgbData.CreateBitmap(image, new Rectangle(0, 0, size.Width, size.Height));
            }

            picture.use_argb = 1;
            picture.width = size.Width;
            picture.height = size.Height;
            ResultWriter writer = new ResultWriter();
            picture.writer = writer.WriterFunction;

            try {
                BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, size.Width, size.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try {
                    if (bmpDat.Stride == size.Width * 4) {
                        picture.argb = (uint*)bmpDat.Scan0;
                        picture.argb_stride = size.Width;
                        Lib.WebPEncode(ref config._inner, ref picture);
                        WebPEncodingException.Check((int)picture.error_code);
                        return writer.Result;
                    }
                    else {
                        ImageArgbData argbData = ImageArgbData.FromFormat32bppArgb(bmpDat);
                        fixed (byte* p = argbData.ArgbBytes) {
                            picture.argb = (uint*)p;
                            picture.argb_stride = argbData.Width;
                            Lib.WebPEncode(ref config._inner, ref picture);
                            WebPEncodingException.Check((int)picture.error_code);
                            return writer.Result;
                        }
                    }
                }
                finally {
                    bmp.UnlockBits(bmpDat);
                }
            }
            finally {
                if (bmp != image)
                    bmp.Dispose();
            }
        }

        /// <summary>
        /// Decode the WebP image bytes to a Bitmap object. The pixel format of the bitmap is Format32bppArgb.
        /// </summary>
        /// <param name="webpBytes"> The WebP image bytes </param>
        /// <returns> The decoded Bitmap object </returns>
        public unsafe static Bitmap Decode(byte[] webpBytes)
        {
            if (webpBytes == null || webpBytes.Length == 0)
                throw new ArgumentNullException("webpBytes");
            int width = 0, height = 0;
            fixed (byte* p = webpBytes) {
                byte* argb = Lib.WebPDecodeBGRA(p, new UIntPtr((uint)webpBytes.Length), ref width, ref height);
                if (argb == null)
                    throw new WebPDecodingException("Bad webp format");
                try {
                    Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    try {
                        byte* scan0 = (byte*)bmpDat.Scan0;
                        for (int y = 0; y < height; y++) {
                            uint* srcLine = (uint*)argb + y * width;
                            uint* dstLine = (uint*)(scan0 + y * bmpDat.Stride);
                            for (int x = 0; x < width; x++) {
                                *dstLine++ = *srcLine++;
                            }
                        }
                    }
                    finally {
                        bmp.UnlockBits(bmpDat);
                    }
                    return bmp;
                }
                finally {
                    Lib.WebPFree(argb);
                }
            }
        }

        /// <summary>
        /// Decode the WebP image bytes to a ImageArgbData object.
        /// </summary>
        /// <param name="webpBytes"> The WebP image bytes </param>
        /// <returns> The decoded ImageArgbData object </returns>
        public unsafe static ImageArgbData DecodeToArgb(byte[] webpBytes)
        {
            if (webpBytes == null || webpBytes.Length == 0)
                throw new ArgumentNullException("webpBytes");
            int width = 0, height = 0;
            fixed (byte* p = webpBytes) {
                byte* src = Lib.WebPDecodeBGRA(p, new UIntPtr((uint)webpBytes.Length), ref width, ref height);
                if (src == null)
                    throw new WebPDecodingException("Bad webp format");
                try {
                    ImageArgbData image = new ImageArgbData(width, height, width * 4, new byte[width * height * 4]);
                    Marshal.Copy(new IntPtr(src), image.ArgbBytes, 0, image.ArgbBytes.Length);
                    return image;
                }
                finally {
                    Lib.WebPFree(src);
                }
            }
        }

        /// <summary>
        /// Calculate the Mean Squared Error (MSE) between two images. Only the RGB channels are compared.
        /// See: https://en.wikipedia.org/wiki/Peak_signal-to-noise_ratio
        /// </summary>
        /// <param name="I"> The first image </param>
        /// <param name="K"> The second image </param>
        /// <returns> The Mean Squared Error (MSE) </returns>
        public static double MSE(ImageArgbData I, ImageArgbData K)
        {
            if (I == null)
                throw new ArgumentNullException("I");
            if (K == null)
                throw new ArgumentNullException("K");
            if (I.Width != K.Width)
                throw new ArgumentException("Size of image I and K are not equal");
            if (I.Height != K.Height)
                throw new ArgumentException("Size of image I and K are not equal");
            long sum = 0;
            int width = I.Width, height = I.Height;
            byte[] iBuf = I.ArgbBytes, kBuf = K.ArgbBytes;
            for (int y = 0; y < height; y++) {
                int iIndex = y * I.Stride;
                int kIndex = y * K.Stride;
                for (int x = 0; x < width; x++) {
                    int deltaB = (int)iBuf[iIndex] - (int)kBuf[kIndex];
                    int deltaG = (int)iBuf[iIndex + 1] - (int)kBuf[kIndex + 1];
                    int deltaR = (int)iBuf[iIndex + 2] - (int)kBuf[kIndex + 2];

                    sum += (long)(deltaB * deltaB + deltaG * deltaG + deltaR * deltaR);

                    iIndex += 4;
                    kIndex += 4;
                }
            }

            return (double)sum / ((double)width * (double)height);
        }

        /// <summary>
        /// <para>Calculate the Peak Signal-to-Noise Ratio (PSNR) from the Mean Squared Error (MSE).</para>
        /// <para>The PSNR is defined as: 20 * log10(max) - 10 * log10(MSE)</para>
        /// <para>Higher PSNR means higher image quality:</para>
        /// <list type="bullet">
        /// <item><description>PSNR &gt;= 40dB: Great</description> </item>
        /// <item><description>30dB &lt;= PSNR &lt; 40dB: Good</description> </item>
        /// <item><description>20dB &lt;= PSNR &lt; 30dB: Medium</description> </item>
        /// <item><description>PSNR &lt; 20dB: Poor</description> </item>
        /// </list>
        /// <para>See: https://en.wikipedia.org/wiki/Peak_signal-to-noise_ratio </para>
        /// </summary>
        /// <param name="mse"> The Mean Squared Error (MSE) </param>
        /// <returns> The Peak Signal-to-Noise Ratio (PSNR) </returns>
        public static double PSNR(double mse)
        {
            if (mse < 0)
                throw new ArgumentException("mse should >= 0", "mse");
            // psnr = 10 * log10((max * max)/mse) 
            //      = 20 * log10(max / sqrt(mse))
            //      = 20 * log10(max) - 10 * log10(mse)
            if (mse < 0.000000001) /* Avoid Div/0 Error */
                mse = 0.000000001;
            // 20*Log10(255) = 48.1308
            double psnr = 20 * Math.Log10(255) - 10 * Math.Log10(mse);
            return psnr;
        }

        /// <summary>
        /// <para>Calculate the Peak Signal-to-Noise Ratio (PSNR) between two images.</para>
        /// <para>The PSNR is defined as: 20 * log10(max) - 10 * log10(MSE)</para>
        /// <para>The MSE is defined as: the sum over three channels (RGB) of the squared difference divided by width * height * 3.</para>
        /// <para>Higher PSNR means higher image quality:</para>
        /// <list type="bullet">
        /// <item><description>PSNR &gt;= 40dB: Great</description> </item>
        /// <item><description>30dB &lt;= PSNR &lt; 40dB: Good</description> </item>
        /// <item><description>20dB &lt;= PSNR &lt; 30dB: Medium</description> </item>
        /// <item><description>PSNR &lt; 20dB: Poor</description> </item>
        /// </list>
        /// <para>See: https://en.wikipedia.org/wiki/Peak_signal-to-noise_ratio </para>
        /// </summary>
        /// <param name="I"> The first image </param>
        /// <param name="K"> The second image </param>
        /// <returns> The Peak Signal-to-Noise Ratio (PSNR) </returns>
        public static double PSNR(ImageArgbData I, ImageArgbData K)
        {
            if (I == null)
                throw new ArgumentNullException("I");
            if (K == null)
                throw new ArgumentNullException("K");
            if (I.Width != K.Width)
                throw new ArgumentException("Size of image I and K are not equal");
            if (I.Height != K.Height)
                throw new ArgumentException("Size of image I and K are not equal");
            double mse = MSE(I, K);
            return PSNR(mse);
        }

        /// <summary>
        /// <para>Calculate the Peak Signal-to-Noise Ratio (PSNR) between two images.</para>
        /// <para>The PSNR is defined as: 20 * log10(max) - 10 * log10(MSE)</para>
        /// <para>The MSE is defined as: the sum over three channels (RGB) of the squared difference divided by width * height * 3.</para>
        /// <para>Higher PSNR means higher image quality:</para>
        /// <list type="bullet">
        /// <item><description>PSNR &gt;= 40dB: Great</description> </item>
        /// <item><description>30dB &lt;= PSNR &lt; 40dB: Good</description> </item>
        /// <item><description>20dB &lt;= PSNR &lt; 30dB: Medium</description> </item>
        /// <item><description>PSNR &lt; 20dB: Poor</description> </item>
        /// </list>
        /// <para>See: https://en.wikipedia.org/wiki/Peak_signal-to-noise_ratio </para>
        /// </summary>
        /// <param name="I"> The first image </param>
        /// <param name="K"> The second image </param>
        /// <returns> The Peak Signal-to-Noise Ratio (PSNR) </returns>
        public static double PSNR(Image I, Image K)
        {
            if (I == null)
                throw new ArgumentNullException("I");
            if (K == null)
                throw new ArgumentNullException("K");
            if (I.Size != K.Size)
                throw new ArgumentException("Size of image I and K are not equal");

            ImageArgbData argbI = DumpArgbData(I);
            ImageArgbData argbK = DumpArgbData(K);
            return PSNR(argbI, argbK);
        }

        /// <summary>
        /// The ResultWriter class is used to write the encoded WebP image bytes to a memory stream.
        /// The WriterFunction is used as the unmanaged WebPWriterFunction callback function for native WebPEncode.
        /// </summary>
        private class ResultWriter
        {
            private MemoryStream _stream;
            private byte[] _buffer;
            private Interop._WebPWriterFunction _writerFunction;

            public ResultWriter()
            {
                _stream = new MemoryStream(1024 * 64);
                _buffer = new byte[1024];
                _writerFunction = Callback;
            }

            private int Callback([In]IntPtr data, UIntPtr data_size, [In]ref _WebPPicture picture)
            {
                int remain = (int)data_size.ToUInt64();
                while (remain > 0) {
                    int step = _buffer.Length;
                    if (step > remain)
                        step = remain;
                    Marshal.Copy(data, _buffer, 0, step);
                    _stream.Write(_buffer, 0, step);
                    data = new IntPtr(data.ToInt64() + step);
                    remain -= step;
                }
                return 1;
            }

            public Interop._WebPWriterFunction WriterFunction
            {
                get { return _writerFunction; }
            }

            public byte[] Result
            {
                get { return _stream.ToArray(); }
            }
        }

    }

}
