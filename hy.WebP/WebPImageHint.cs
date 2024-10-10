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
//    $Id: WebPImageHint.cs
//   
// ==--==

using System;

using hy.WebP.Interop;

namespace hy.WebP
{
    /// <summary>
    /// Image characteristics hint for the underlying encoder (lossless only for now).
    /// </summary>
    public enum WebPImageHint : int
    {
        /// <summary>
        /// Default preset.
        /// </summary>
        Default = _WebPImageHint.WEBP_HINT_DEFAULT,
        /// <summary>
        /// Digital picture, like portrait, inner shot
        /// </summary>
        Picture = _WebPImageHint.WEBP_HINT_PICTURE,
        /// <summary>
        /// Outdoor photograph, with natural lighting
        /// </summary>
        Photo = _WebPImageHint.WEBP_HINT_PHOTO,
        /// <summary>
        /// Discrete tone image (graph, map-tile etc)
        /// </summary>
        Graph = _WebPImageHint.WEBP_HINT_GRAPH
    }
}
