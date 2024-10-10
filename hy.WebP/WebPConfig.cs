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
//    $Id: WebPConfig.cs
//   
// ==--==

using System;

using hy.WebP.Interop;

namespace hy.WebP
{
    /// <summary>
    /// <para>The WebPConfig structure is used for configuration and control of the WebP encoding process.</para>
    /// <para>Instances of this structure are created with the CreateConfig, CreateConfigPreset and CreateConfigLossless methods of the WebPCodec class.</para>
    /// </summary>
    public sealed class WebPConfig
    {
        internal _WebPConfig _inner;

        internal WebPConfig(_WebPConfig inner)
        {
            _inner = inner;
        }
        /// <summary>
        /// Lossless encoding Mode. false=lossy(default), true=lossless.
        /// </summary>
        public bool Lossless
        {
            get { return _inner.lossless != 0; }
        }

        /// <summary>
        /// <para>Quaility between 0 and 100.</para>
        /// <para>For lossy, 0 gives the smallest size and 100 the largest. </para>
        /// <para>For lossless, this parameter is the amount of effort put into the compression:
        /// 0 is the fastest but gives larger files compared to the slowest, but best, 100.</para>
        /// </summary>
        /// <remarks> Use parameter level of WebPCodec.CreateConfigLossless to create a lossless configuration.</remarks>
        public float Quaility
        {
            get { return _inner.quality; }
        }

        /// <summary>
        /// Quality/Speed trade-off (0=fast, 6=slower-better).
        /// </summary>
        public int Method
        {
            get { return _inner.method; }
            set
            {
                if (value < 0 || value > 6)
                    throw new ArgumentOutOfRangeException("Method");
                _inner.method = value;
            }
        }

        /// <summary>
        /// Hint for image type (lossless only for now).
        /// </summary>
        public WebPImageHint ImageHint
        {
            get { return (WebPImageHint)_inner.image_hint; }
            set
            {
                switch (value) {
                    case WebPImageHint.Default: break;
                    case WebPImageHint.Picture: break;
                    case WebPImageHint.Photo: break;
                    case WebPImageHint.Graph: break;
                    default: throw new ArgumentException("Invalid image hint", "ImageHint");
                }
                _inner.image_hint = (_WebPImageHint)value;
            }
        }

        /// <summary>
        /// Spatial Noise Shaping. 0=off, 100=maximum.
        /// </summary>
        public int SNSStrength
        {
            get { return _inner.sns_strength; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("SNSStrength");
                _inner.sns_strength = value;
            }
        }

        /// <summary>
        /// FilterStrength only works if lossy mode is enabled.
        /// range: [0 = off .. 100 = strongest] 
        /// </summary>
        public int FilterStrength
        {
            get { return _inner.filter_strength; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("FilterStrength");
                _inner.filter_strength = value;
            }
        }

        /// <summary>
        /// FilterSharpness only works if lossy mode is enabled.
        /// range: [0 = off .. 7 = least sharp].
        /// </summary>
        public int FilterSharpness
        {
            get { return _inner.filter_sharpness; }
            set
            {
                if (value < 0 || value > 7)
                    throw new ArgumentOutOfRangeException("FilterSharpness");
                _inner.filter_sharpness = value;
            }
        }

        /// <summary>
        /// Filtering type: 0 = simple, 1 = strong (only used if filter_strength > 0 or autofilter enabled).
        /// </summary>
        public int FilterType
        {
            get { return _inner.filter_type; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("FilterType");
                _inner.filter_type = value;
            }
        }

        /// <summary>
        /// Auto adjust filter's strength.
        /// </summary>
        public bool AutoFilter
        {
            get { return _inner.autofilter != 0; }
            set { _inner.autofilter = value ? 1 : 0; }
        }

        /// <summary>
        /// Algorithm for encoding the alpha plane (0 = none, 1 = compressed with WebP lossless). Default is 1. 
        /// </summary>
        public bool AlphaCompression
        {
            get { return _inner.alpha_compression != 0; }
            set { _inner.alpha_compression = value ? 1 : 0; }
        }

        /// <summary>
        /// Predictive filtering method for alpha plane. 0: none, 1: fast, 2: best. Default if 1. 
        /// </summary>
        public int AlphaFiltering
        {
            get { return _inner.alpha_filtering; }
            set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentOutOfRangeException("AlphaFiltering");
                _inner.alpha_filtering = value;
            }
        }

        /// <summary>
        /// Between 0 (smallest size) and 100 (lossless).  Default is 100. 
        /// </summary>
        public int AlphaQuality
        {
            get { return _inner.alpha_quality; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("AlphaQuality");
                _inner.alpha_quality = value;
            }
        }

        /// <summary>
        /// Number of entropy-analysis passes (in [1..10]).
        /// </summary>
        public int Pass
        {
            get { return _inner.pass; }
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentOutOfRangeException("Pass");
                _inner.pass = value;
            }
        }

        /// <summary>
        /// If true, export the compressed picture back. In-loop filtering is not applied. 
        /// </summary>
        public bool ShowCompressed
        {
            get { return _inner.show_compressed != 0; }
            set { _inner.show_compressed = value ? 1 : 0; }
        }

        /// <summary>
        /// If true, try and use multi-threaded encoding. 
        /// </summary>
        public bool Multithreading
        {
            get { return _inner.thread_level != 0; }
            set { _inner.thread_level = value ? 1 : 0; }
        }

        /// <summary>
        /// Near lossless encoding [0 = max loss .. 100 = off (default)]. Works only in lossless mode.
        /// The smaller the value, the smaller the file size and the lower the quality.
        /// </summary>
        public int NearLossless
        {
            get { return _inner.near_lossless; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("NearLossless");
                _inner.near_lossless = value;
            }
        }

        /// <summary>
        /// If needed, use sharp (and slow) RGB->YUV conversion.
        /// </summary>
        public bool UseSharpYuv
        {
            get { return _inner.use_sharp_yuv != 0; }
            set { _inner.use_sharp_yuv = value ? 1 : 0; }
        }

        /// <summary>
        /// Minimum permissible quality factor
        /// </summary>
        public int QMin
        {
            get { return _inner.qmin; }
            set { _inner.qmin = value; }
        }

        /// <summary>
        /// Maximum permissible quality factor
        /// </summary>
        public int QMax
        {
            get { return _inner.qmax; }
            set { _inner.qmax = value; }
        }

        /// <summary>
        /// If non-zero, specifies the minimal distortion to try to achieve. Takes precedence over target_size.
        /// Works only in lossy mode.
        /// </summary>
        public float TargetPSNR
        {
            get { return _inner.target_PSNR; }
            set { _inner.target_PSNR = value; }
        }

    }
}
