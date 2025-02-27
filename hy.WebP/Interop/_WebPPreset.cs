﻿using System;

namespace hy.WebP.Interop
{
    /* Enumerate some predefined settings for WebPConfig, depending on the type
       of source picture. These presets are used when calling WebPConfigPreset(). */
    internal enum _WebPPreset : int
    {
        WEBP_PRESET_DEFAULT = 0,  /* WEBP_PRESET_DEFAULT -> 0 */
        WEBP_PRESET_PICTURE,      /* digital picture, like portrait, inner shot */
        WEBP_PRESET_PHOTO,        /* outdoor photograph, with natural lighting */
        WEBP_PRESET_DRAWING,      /* hand or line drawing, with high-contrast details */
        WEBP_PRESET_ICON,         /* small-sized colorful images */
        WEBP_PRESET_TEXT          /* text-like */
    }
}
