﻿using System;
using System.Collections.Generic;
using System.Text;

namespace hy.WebP.Interop
{
    /* chroma sampling color spaces */
    internal enum _WebPEncCSP
    {
        WEBP_YUV420 = 0,        /* 4:2:0 */
        WEBP_YUV420A = 4,       /* alpha channel variant */
        WEBP_CSP_UV_MASK = 3,   /* bit-mask to get the UV sampling factors */
        WEBP_CSP_ALPHA_BIT = 4  /* bit that is set if alpha is present */
    }
}
