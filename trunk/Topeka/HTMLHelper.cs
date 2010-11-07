/*
topeka - A lightweight embeddable multi-threaded free and easy to use Web Server library

Project: http://code.google.com/p/topeka/
Developer's Home: http://www.juanchi.com.ar

Copyright (c) 2010 Juanchi (juanchi@juanchi.com.ar)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Topeka
{
    /// <summary>
    /// This class is used to help in HTML encodings
    /// </summary>
    public class HTMLHelper
    {
        /// <summary>
        /// Encodes the input string using the UrlEncode method
        /// </summary>
        /// <param name="input">The string to be encoded</param>
        /// <returns>The encoded string</returns>
        public static string encode(string input)
        {
            return System.Web.HttpUtility.UrlEncode(input);
        }

        /// <summary>
        /// Decodes the input string using the UrlDecode method
        /// </summary>
        /// <param name="input">The string to be decoded</param>
        /// <returns>The decoded string</returns>
        public static string decode(string input)
        {
            return System.Web.HttpUtility.UrlDecode(input);
        }

    }
}
