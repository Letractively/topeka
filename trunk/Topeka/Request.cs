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
using System.Collections;
using System.Text;

namespace Topeka
{
    /// <summary>
    /// A Class that represents the request from the client
    /// </summary>
    public class Request
    {
        String sBuffer;
        internal Server server;

        private string m_Method;
        /// <summary>
        /// This Property represent the type of request (GET or POST).
        /// </summary>
        public string Method
        { 
            get{return m_Method;}
        }

        private string m_Authorization;
        /// <summary>
        /// Authentication credentials for HTTP authentication
        /// </summary>
        public string Authorization
        {
            get { return m_Authorization; }
        }

        private string m_Path;
        /// <summary>
        /// The complete path of the request received from the client
        /// </summary>
        public string Path
        { get { return m_Path; } }

        internal string m_HTTPVersion;
        /// <summary>
        /// The version of the HTTP (example HTTP/1.1)
        /// </summary>
        public string HTTPVersion
        {get{return m_HTTPVersion;}}

        private string m_Accept;
        /// <summary>
        /// Content-Types that are acceptable
        /// </summary>
        public string Accept
        {get{return  m_Accept;}}

        private string m_AcceptLanguage;
        /// <summary>
        /// Acceptable languages for response
        /// </summary>
        public string AcceptLanguage
        {get{return m_AcceptLanguage;}}

        private string m_AcceptEncoding;
        /// <summary>
        /// Acceptable encodings
        /// </summary>
        public string AcceptEncoding
        {get{return m_AcceptEncoding;}}

        private string m_UserAgent;
        /// <summary>
        /// The user agent string of the user agent
        /// </summary>
        public string UserAgent
        {get{return m_UserAgent;}}

        private string m_Host;
        /// <summary>
        /// The Client Host connected
        /// </summary>
        public string Host
        {get{return m_Host;}}

        private string m_Connection;
        /// <summary>
        /// What type of connection the user-agent would prefer
        /// </summary>
        public string Connection
        {get{return m_Connection;}}

        internal string m_ClientIP;
        /// <summary>
        /// The Client IP connected 
        /// </summary>
        public string ClientIP
        {get{return m_ClientIP;}}

        private ArrayList m_Cookies;
        /// <summary>
        /// An array of Cookie Objects previously sent by the server with Set-Cookie
        /// </summary>
        public ArrayList Cookies
        {get{return m_Cookies;}}

        Hashtable parameters;
        internal String Class;

        string substract(string temp)
        {
            Tokenizer toki = new Tokenizer(temp, ": ");
            toki.nextToken();
            string toReturn = toki.nextToken();
            return toReturn;
        }

        /// <summary>
        /// Returns the parameter specified from the request
        /// </summary>
        /// <param name="parameter">The parameter to be returned</param>
        /// <returns>The value for the parameter specified, null if there is no parameter</returns>
        public string getParameter(string parameter)
        {
            return (string)parameters[parameter];
        }

        void parseParameters(string parameters_tokenized)
        {
            Tokenizer parameters_token = new Tokenizer(parameters_tokenized, "&");
            while (parameters_token.hasMoreTokens())
            {
                Tokenizer tempi = new Tokenizer(parameters_token.nextToken(), "=");
                parameters.Add(tempi.nextToken(), HTMLHelper.decode(tempi.nextToken()));
            }
        }

        /// <summary>
        /// Construct a Request object that contains the information received from the client
        /// </summary>
        /// <param name="sBuffer">The buffer received from the client</param>
        /// <param name="server">The server that handled the request</param>
        internal Request(String sBuffer, Server server)
        {
            this.server = server;
            this.m_ClientIP = "";
            this.sBuffer = sBuffer;
            this.m_Cookies = new ArrayList();
            this.parameters = new Hashtable();

            Tokenizer toki = new Tokenizer(sBuffer, "\r\n");
            Tokenizer fullRequest = new Tokenizer(toki.nextToken(), " ");
            m_Method = fullRequest.nextToken();
            m_Path = fullRequest.nextToken();
            Tokenizer path = new Tokenizer(m_Path.Substring(1), "?");
            Class = path.nextToken();
            parseParameters(path.nextToken());

            m_HTTPVersion = fullRequest.nextToken();

            while (toki.hasMoreTokens())
            {
                string temp = toki.nextToken();
                if (temp != null)
                {
                    if (temp.IndexOf("Accept:") == 0) m_Accept = substract(temp);
                    else if (temp.IndexOf("Accept-Language:") == 0) m_AcceptLanguage = substract(temp);
                    else if (temp.IndexOf("Accept-Encoding:") == 0) m_AcceptEncoding = substract(temp);
                    else if (temp.IndexOf("User-Agent:") == 0) m_UserAgent = substract(temp);
                    else if (temp.IndexOf("Host:") == 0) m_Host = substract(temp);
                    else if (temp.IndexOf("Connection:") == 0) m_Connection = substract(temp);
                    else if (temp.IndexOf("Authorization:") == 0) m_Authorization = substract(temp);
                    else if (temp.IndexOf("Cookie:") == 0)
                    {
                        string cookiesTemp = substract(temp);
                        Tokenizer cookiesToki = new Tokenizer(cookiesTemp, "; ");
                        while (cookiesToki.hasMoreTokens())
                        {
                            string cookieTemp = cookiesToki.nextToken();
                            Tokenizer inCookie = new Tokenizer(cookieTemp, "=");
                            this.m_Cookies.Add(new Cookie(inCookie.nextToken(), inCookie.nextToken()));
                        }
                    }
                    else if (temp == "")
                    {
                        if (Method == "POST")
                        {
                            parseParameters(toki.nextToken());
                        }
                    }
                }
            }



        }
    }

}
