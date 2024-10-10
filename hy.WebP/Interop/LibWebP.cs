using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace hy.WebP.Interop
{
    internal class LibWebP
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)]string lpProcName);

        /* One-stop-shop call! No questions asked:
           Returns the size of the compressed data (pointed to by *output), or 0 if an error 
           occurred. The compressed data must be released by the caller using the call 
           'WebPFree(*output)'.  These functions compress using the lossy format, and the
           quality_factor can go from 0 (smaller output, lower quality) to 100 (best quality,
           larger output). */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Func_WebPGetEncoderVersion();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr Func_WebPEncodeBGR([In] IntPtr bgr, int width, int height, int stride, float quality_factor, ref IntPtr output);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr Func_WebPEncodeBGRA([In] IntPtr bgra, int width, int height, int stride, float quality_factor, ref IntPtr output);

        /* These functions are the equivalent of the above, but compressing in a lossless manner.
           Files are usually larger than lossy format, but will not suffer any compression loss */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr Func_WebPEncodeLosslessBGR([In] IntPtr bgr, int width, int height, int stride, ref IntPtr output);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr Func_WebPEncodeLosslessBGRA([In] IntPtr bgra, int width, int height, int stride, ref IntPtr output);

        /* Should always be called, to initialize a fresh WebPConfig structure before modification.
           Returns false in case of version mismatch. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Func_WebPConfigInitInternal(ref _WebPConfig config, _WebPPreset preset, float quality, int version);

        /* Activate the lossless compression mode with the desired efficiency level
           between 0 (fastest, lowest compression) and 9 (slower, best compression).
           A good default level is '6', providing a fair tradeoff between compression
           speed and final compressed size.
           This function will overwrite several fields from config: 'method', 'quality'
           and 'lossless'. Returns false in case of parameter error. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Func_WebPConfigLosslessPreset(ref _WebPConfig config, int level);

        /* Returns true if 'config' is non-NULL and all configuration parameters are
           within their valid ranges */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Func_WebPValidateConfig(ref _WebPConfig config);

        /* Should always be called, to initialize the structure. Returns false in case 
           of version mismatch. WebPPictureInit() must have succeeded before using the
           'picture' object.
           Note that, by default, use_argb is false and colorspace is WEBP_YUV420. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Func_WebPPictureInitInternal(ref _WebPPicture picture, int version);
        
        /* Main call */
        /* Main encoding call, after config and picture have been initialized.
           'picture' must be less than 16384x16384 in dimension (cf WEBP_MAX_DIMENSION),
           and the 'config' object must be a valid one.
           Returns false in case of error, true otherwise.
           In case of error, picture->error_code is updated accordingly.
           'picture' can hold the source samples in both YUV(A) or ARGB input, depending
           on the value of 'picture->use_argb'. It is highly recommended to use
           the former for lossy encoding, and the latter for lossless encoding
           (when config.lossless is true). Automatic conversion from one format to
           another is provided but they both incur some loss. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Func_WebPEncode([In] ref _WebPConfig config, ref _WebPPicture picture);

        /* Decodes WebP images pointed to by 'data' and returns RGBA samples, along
           with the dimensions in *width and *height. The ordering of samples in
           memory is A, R, G, B, A, R, G, B, ... in scan order (endian-independent).
           The returned pointer should be deleted calling WebPFree().
           Returns NULL in case of error. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte* Func_WebPDecodeBGRA(byte* data, UIntPtr data_size, ref int width, ref int height);

        /* Releases memory returned by the WebPDecode*() functions above. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void Func_WebPFree(byte* ptr);

        private string _fileName;
        private IntPtr _hModule;
        private _WebPLibVersion _libVersion;

        private Func_WebPConfigInitInternal _WebPConfigInitInternal;
        private Func_WebPConfigLosslessPreset _WebPConfigLosslessPreset;
        private Func_WebPValidateConfig _WebPValidateConfig;
        private Func_WebPPictureInitInternal _WebPPictureInitInternal;
        private Func_WebPEncode _WebPEncode;
        private Func_WebPDecodeBGRA _WebPDecodeBGRA;
        private Func_WebPFree _WebPFree;

        public LibWebP(string fileName, _WebPLibVersion libVersion)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            _fileName = fileName;
            _libVersion = libVersion;
            _hModule = LoadLibrary(fileName);
            if (_hModule == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            _WebPConfigInitInternal = GetProcAsDelegate<Func_WebPConfigInitInternal>("WebPConfigInitInternal");
            _WebPConfigLosslessPreset = GetProcAsDelegate<Func_WebPConfigLosslessPreset>("WebPConfigLosslessPreset");
            _WebPValidateConfig = GetProcAsDelegate<Func_WebPValidateConfig>("WebPValidateConfig");
            _WebPPictureInitInternal = GetProcAsDelegate<Func_WebPPictureInitInternal>("WebPPictureInitInternal");
            _WebPEncode = GetProcAsDelegate<Func_WebPEncode>("WebPEncode");
            _WebPDecodeBGRA = GetProcAsDelegate<Func_WebPDecodeBGRA>("WebPDecodeBGRA");
            _WebPFree = GetProcAsDelegate<Func_WebPFree>("WebPFree");
        }

        private IntPtr GetProcAddress(string procName)
        {
            IntPtr addr = GetProcAddress(_hModule, procName);
            if (addr == IntPtr.Zero)
                throw new EntryPointNotFoundException(
                    string.Format("Unable to find an entry point named '{0}' in module '{1}'", 
                    procName, _fileName));
            return addr;
        }

        private TProc GetProcAsDelegate<TProc>(string procName)
        {
            IntPtr addr = GetProcAddress(procName);
            return (TProc)(object)Marshal.GetDelegateForFunctionPointer(addr, typeof(TProc));
        }

        public int WebPConfigInit(ref _WebPConfig config, _WebPPreset preset, float quality)
        {
            int version;
            if (_libVersion == _WebPLibVersion.WEBP_VERSION_140)
                version = _WebPConst.WEBP_ENCODER_ABI_VERSION_140;
            else
                version = _WebPConst.WEBP_ENCODER_ABI_VERSION_120;
            return _WebPConfigInitInternal(ref config, preset, quality, version);
        }

        public int WebPConfigLosslessPreset(ref _WebPConfig config, int level)
        {
            return _WebPConfigLosslessPreset(ref config, level);
        }

        public int WebPValidateConfig(ref _WebPConfig config)
        {
            return _WebPValidateConfig(ref config);
        }

        public int WebPPictureInit(ref _WebPPicture picture)
        {
            int version;
            if (_libVersion == _WebPLibVersion.WEBP_VERSION_140)
                version = _WebPConst.WEBP_ENCODER_ABI_VERSION_140;
            else
                version = _WebPConst.WEBP_ENCODER_ABI_VERSION_120;
            return _WebPPictureInitInternal(ref picture, version);
        }

        public int WebPEncode([In] ref _WebPConfig config, ref _WebPPicture picture)
        {
            return _WebPEncode(ref config, ref picture);
        }

        public unsafe byte* WebPDecodeBGRA(byte* data, UIntPtr data_size, ref int width, ref int height)
        {
            return _WebPDecodeBGRA(data, data_size, ref width, ref height);
        }

        public unsafe void WebPFree(byte* ptr)
        {
            _WebPFree(ptr);
        }

        public string FileName
        {
            get { return _fileName; }
        }
    }
}
