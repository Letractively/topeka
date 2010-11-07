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
using System.Globalization;

namespace Topeka
{
    /// <summary>
    /// A Class that represents a cookie
    /// </summary>
    public class Cookie
    {
        /*
        The date string is formatted as: 
        Wdy, DD-Mon-YYYY HH:MM:SS GMT 
        Set-Cookie: CUSTOMER=WILE_E_COYOTE; path=/; expires=Wednesday, 09-Nov-99 23:12:40 GMT
        */

        /// <summary>
        /// The name of the cookie
        /// </summary>
        public string name;
        /// <summary>
        /// The value of the cookie
        /// </summary>
        public string value;
        /// <summary>
        /// The expiration time, example "expires=Wednesday, 09-Nov-99 23:12:40 GMT"
        /// </summary>
        string expireTime;

        /// <summary>
        /// Creates a new Cookie object, setting the name and value from parameters
        /// </summary>
        /// <param name="name">The name of the Cookie</param>
        /// <param name="value">The value of the Cookie</param>
        public Cookie(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Sets the age of the cookie
        /// </summary>
        /// <param name="seconds">The seconds that the cookie will be active, the method converts the value in seconds to the corresponding format</param>
        public void setMaxAge(int seconds)
        {
            DateTime MyDate = DateTime.Now.AddSeconds(seconds);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime myDate = localZone.ToUniversalTime(MyDate);
            CultureInfo ci = CultureInfo.InvariantCulture;
            expireTime = myDate.ToString("ddd, dd-MMM-yy HH:mm:ss ",ci) + "GMT";
        }

        /// <summary>
        /// This method returns the string representation of the cookie that will be sent to the client
        /// </summary>
        /// <returns>The string representation of the cookie, in this format:
        /// Set-Cookie: CUSTOMER=WILE_E_COYOTE; path=/; expires=Wednesday, 09-Nov-99 23:12:40 GMT
        /// </returns>
        public string serialize()
        {
            if (this.expireTime == null)
                return "Set-Cookie: " + name + "=" + value + "; path=/";
            else
                return "Set-Cookie: " + name + "=" + value + "; path=/; expires=" + expireTime;
        }

    }
}
