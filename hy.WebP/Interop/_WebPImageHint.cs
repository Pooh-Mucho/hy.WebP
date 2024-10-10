using System;

namespace hy.WebP.Interop
{
    /* Hint for image type (lossless only for now). */
    internal enum _WebPImageHint: int
    {
        WEBP_HINT_DEFAULT = 0,  /* WEBP_HINT_DEFAULT -> 0 */
        WEBP_HINT_PICTURE,      /* digital picture, like portrait, inner shot */
        WEBP_HINT_PHOTO,        /* outdoor photograph, with natural lighting */
        WEBP_HINT_GRAPH,        /* Discrete tone image (graph, map-tile etc). */
        WEBP_HINT_LAST
    }
}
