using System;
using System.Runtime.InteropServices;

namespace hy.WebP.Interop
{
    /* From src/webp/encode.h */ 
    /* Compression parameters */
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct _WebPConfig
    {
        /* Lossless encoding (0=lossy(default), 1=lossless). */
        public int lossless;
        /* between 0 and 100. For lossy, 0 gives the smallest size and 100 the largest. 
           For lossless, this parameter is the amount of effort put into the 
           compression: 0 is the fastest but gives larger files compared to the slowest, 
           but best, 100. */
        public float quality;
        /* quality/speed trade-off (0=fast, 6=slower-better) */
        public int method;
        /* Hint for image type (lossless only for now). */
        public _WebPImageHint image_hint;
        /* if non-zero, set the desired target size in bytes. Takes precedence over 
           the 'compression' parameter. */
        public int target_size;
        /* if non-zero, specifies the minimal distortion to try to achieve. Takes 
           precedence over target_size. */
        public float target_PSNR;
        /* maximum number of segments to use, in [1..4] */
        public int segments;
        /* Spatial Noise Shaping. 0=off, 100=maximum. */
        public int sns_strength;
        /* range: [0 = off .. 100 = strongest] */
        public int filter_strength;
        /* range: [0 = off .. 7 = least sharp] */
        public int filter_sharpness;
        /* filtering type: 0 = simple, 1 = strong (only used if filter_strength > 0 or 
           autofilter > 0) */
        public int filter_type;
        /* Auto adjust filter's strength [0 = off, 1 = on] */
        public int autofilter;
        /* Algorithm for encoding the alpha plane (0 = none, 1 = compressed with WebP 
           lossless). Default is 1. */
        public int alpha_compression;
        /* Predictive filtering method for alpha plane. 0: none, 1: fast, 2: best. 
           Default if 1. */
        public int alpha_filtering;
        /* Between 0 (smallest size) and 100 (lossless).  Default is 100. */
        public int alpha_quality;
        /* number of entropy-analysis passes (in [1..10]). */
        public int pass;
        /* if true, export the compressed picture back. In-loop filtering is not applied. */
        public int show_compressed;
        /* preprocessing filter: 0=none, 1=segment-smooth, 2=pseudo-random dithering */
        public int preprocessing;
        /* log2(number of token partitions) in [0..3]. Default is set to 0 for easier 
           progressive decoding. */
        public int partitions;
        /* quality degradation allowed to fit the 512k limit on prediction modes coding 
           (0: no degradation, 100: maximum possible degradation). */
        public int partition_limit;
        /* If true, compression parameters will be remapped to better match the expected 
           output size from JPEG compression. Generally, the output size will be similar
           but the degradation will be lower.*/
        public int emulate_jpeg_size;
        /* If non-zero, try and use multi-threaded encoding. */
        public int thread_level;
        /* If set, reduce memory usage (but increase CPU use). */
        public int low_memory;
        /* Near lossless encoding [0 = max loss .. 100 = off (default)]. */
        public int near_lossless;
        /* if non-zero, preserve the exact RGB values under transparent area. Otherwise, 
           discard this invisible RGB information for better compression. The default
           value is 0. */
        public int exact;
        /* reserved for future lossless feature */
        public int use_delta_palette;
        /* if needed, use sharp (and slow) RGB->YUV conversion */
        public int use_sharp_yuv;
        /* minimum permissible quality factor */
        public int qmin;
        /* maximum permissible quality factor */
        public int qmax;
    }
}
