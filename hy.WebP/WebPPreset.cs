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
//    $Id: WebPPreset.cs
//   
// ==--==

using System;
using hy.WebP.Interop;

namespace hy.WebP
{
    /// <summary>
    /// <para>Enumerate predefined settings for WebPConfig, depending on the type of source picture.</para>
    /// <para>Works only for lossy encoding mode.</para>
    /// </summary>
    public enum WebPPreset : int
    {
        /// <summary>
        /// Default preset
        /// </summary>
        Default = _WebPPreset.WEBP_PRESET_DEFAULT,
        /// <summary>
        /// Digital picture, like portrait, inner shot
        /// </summary>
        Picture = _WebPPreset.WEBP_PRESET_PICTURE,
        /// <summary>
        /// Outdoor photograph, with natural lighting
        /// </summary>
        Photo = _WebPPreset.WEBP_PRESET_PHOTO,
        /// <summary>
        /// Hand or line drawing, with high-contrast details
        /// </summary>
        Drawing = _WebPPreset.WEBP_PRESET_DRAWING,
        /// <summary>
        /// Small-sized colorful image
        /// </summary>
        Icon = _WebPPreset.WEBP_PRESET_ICON,
        /// <summary>
        /// Text-like image
        /// </summary>
        Text = _WebPPreset.WEBP_PRESET_TEXT,
    }
}
