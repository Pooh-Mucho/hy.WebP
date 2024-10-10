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
//    $Id: WebPDecodingException.cs
//   
// ==--==

using System;

namespace hy.WebP
{
    /// <summary>
    /// <para>Exception thrown when an error occurs during the decoding of a WebP image.</para>
    /// </summary>
    [Serializable]
    public class WebPDecodingException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the WebPDecodingException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WebPDecodingException(string message) : base(message) { }
    }
}
