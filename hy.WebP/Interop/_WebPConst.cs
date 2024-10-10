using System;

namespace hy.WebP.Interop
{
    /* contant values not grouped by enums */
    internal static class _WebPConst
    {
        /*  MAJOR(8b) + MINOR(8b) */
        public const short WEBP_ENCODER_ABI_VERSION_140 = 0x020f;

        /*  MAJOR(8b) + MINOR(8b) */
        public const short WEBP_ENCODER_ABI_VERSION_120 = 0x020f;

        /* maximum width/height allowed (inclusive), in pixels */
        public const int WEBP_MAX_DIMENSION = 16383;
    }
}
