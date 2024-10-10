using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace hy.WebP.Interop
{
    /* Signature for output function. Should return true if writing was successful.
       data/data_size is the segment of data to write, and 'picture' is for
       reference (and so one can make use of picture->custom_ptr) */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int _WebPWriterFunction(IntPtr data, UIntPtr data_size, ref _WebPPicture picture);

    /* Progress hook, called from time to time to report progress. It can return
       false to request an abort of the encoding process, or true otherwise if
       everything is OK */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int _WebPProgressHook(int percent, [In]ref _WebPPicture picture);

    /* WebPMemoryWrite: a special WebPWriterFunction that writes to memory using
       the following WebPMemoryWriter object (to be set as a custom_ptr). */
    //internal unsafe struct _WebPMemoryWriter
    //{
    //    public byte* mem;          /* final buffer (of size 'max_size', larger than 'size'). */
    //    public UIntPtr size;       /* final size */
    //    public UIntPtr max_size ;  /* total capacity */
    //    public uint pad_1, pad_2;  /* padding for later use */
    //}


    /* Main exchange structure (input samples, output bytes, statistics)
       Once WebPPictureInit() has been called, it's ok to make all the INPUT fields
       (use_argb, y/u/v, argb, ...) point to user-owned data, even if
       WebPPictureAlloc() has been called. Depending on the value use_argb,
       it's guaranteed that either *argb or *y/*u/*v content will be kept untouched. */
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal unsafe struct _WebPPicture
    {
        /* INPUT */
        /* Main flag for encoder selecting between ARGB or YUV input.  It is recommended
           to use ARGB input (*argb, argb_stride) for lossless compression, and YUV 
           input (*y, *u, *v, etc.) for lossy compression since these are the respective
           native colorspace for these formats. */
        public int use_argb;
        /* YUV input (mostly used for input to lossy compression) */
        /* colorspace: should be YUV420 for now (=Y'CbCr). */
        public _WebPEncCSP colorspace;
        /* dimensions (less or equal to WEBP_MAX_DIMENSION) */
        public int width, height;
        /* pointers to luma/chroma planes. */
        public byte* y; byte* u; byte* v;
        /* luma/chroma strides. */
        public int y_stride, uv_stride;
        /* pointer to the alpha plane */
        public byte* a;
        /* stride of the alpha plane */
        public int a_stride;
        /* padding for later use */
        public uint pad1_1;
        public uint pad1_2;

        /* ARGB input (mostly used for input to lossless compression) */
        /* Pointer to argb (32 bit) plane. */
        public uint* argb;
        /* This is stride in pixels units, not bytes. */
        public int argb_stride;
        /* padding for later use */
        public uint pad2_1, pad2_2, pad2_3;

        /* OUTPUT */
        /* Byte-emission hook, to store compressed bytes as they are ready. */
        /* can be NULL */
        public _WebPWriterFunction writer;
        /* can be used by the writer. */
        public void* custom_ptr;
        /* map for extra information (only for lossy compression mode) */
        /* 1: intra type, 2: segment, 3: quant, 4: intra-16 prediction mode,
           5: chroma prediction mode, 6: bit cost, 7: distortion */
        public int extra_info_type;
        /* if not NULL, points to an array of size 
           ((width + 15) / 16) * ((height + 15) / 16) that will be filled with 
           a macroblock map, depending on extra_info_type. */
        public byte* extra_info;

        /* STATS AND REPORTS */
        /* Pointer to side statistics (updated only if not NULL) */
        public IntPtr* stats;
        /* Error code for the latest error encountered during encoding */
        public _WebPEncodingError error_code;

        /* If not NULL, report progress during encoding. */
        public _WebPProgressHook progress_hook;
        /* this field is free to be set to any value and used during callbacks
           (like progress-report e.g.). */
        public void* user_data;
        /* padding for later use */
        public uint pad3_1, pad3_2, pad3_3;
        /* Unused for now */
        public uint* pad4;
        public uint* pad5;
        public uint pad6_1, pad6_2, pad6_3, pad6_4, pad6_5, pad6_6, pad6_7, pad6_8;

        /* PRIVATE FIELDS */
        /* row chunk of memory for yuva planes */
        public void* memory_;
        /* and for argb too. */
        public void* memory_argb_;
        /* padding for later use */
        public void* pad7_1;
        public void* pad7_2;
    }
}
