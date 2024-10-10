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
//    $Id: WebPEncodingException.cs
//   
// ==--==

using System;

namespace hy.WebP
{
    /// <summary>
    /// <para>Exception thrown when an error occurs during the encoding of a WebP image.</para>
    /// </summary>
    [Serializable]
    public class WebPEncodingException: Exception
    {
        private int _errorCode;

        /// <summary>
        /// Initializes a new instance of the WebPEncodingException class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the WebP encoder.</param>
        public WebPEncodingException(int errorCode)
            : base(ErrorMessage(errorCode))
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the WebPEncodingException class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the WebP encoder.</param>
        /// <param name="message">The message that describes the error.</param>
        public WebPEncodingException(int errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// The error code returned by the WebP encoder.
        /// </summary>
        public int ErrorCode
        {
            get { return _errorCode; }
        }

        /// <summary>
        /// Check the error code and throw an exception if it is not 0(VP8_ENC_OK).
        /// </summary>
        /// <param name="errorCode">The error code returned by the WebP encoder.</param>
        public static void Check(int errorCode)
        {
            if (errorCode != (int)Interop._WebPEncodingError.VP8_ENC_OK)
                throw new WebPEncodingException(errorCode);
        }

        private static string ErrorMessage(int errorCode)
        {
            return ((Interop._WebPEncodingError)errorCode).ToString();
        }
    }
}
