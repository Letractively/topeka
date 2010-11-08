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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Topeka
{
    /// <summary>
    /// A Class that represents the response sent to the client
    /// </summary>
    public class Response
    {
        static Hashtable statusCodes;

        static Response()
        {
            statusCodes = new Hashtable();
            statusCodes.Add("200", "OK");
            statusCodes.Add("201", "CREATED");
            statusCodes.Add("202", "Accepted");
            statusCodes.Add("203", "Partial Information");
            statusCodes.Add("204", "No Response");
            statusCodes.Add("400", "Bad request");
            statusCodes.Add("401", "Unauthorized");
            statusCodes.Add("402", "PaymentRequired");
            statusCodes.Add("403", "Forbidden");
            statusCodes.Add("404", "Not found");
            statusCodes.Add("500", "Internal Error");
            statusCodes.Add("501", "Not implemented");
        }


        Socket socket;
        MemoryStream stream;
        internal Hashtable headers;
        
        Request request;
        internal string statusCode;
        ArrayList cookies;

        /// <summary>
        /// Prints a string to the client, it doesn't include newline
        /// </summary>
        /// <param name="whatToPrint">
        /// A string that represents what to print to the client
        /// </param>
        public void print(String whatToPrint)
        {
            stream.Write(Encoding.ASCII.GetBytes(whatToPrint), 0, Encoding.ASCII.GetBytes(whatToPrint).Length);
        }

        /// <summary>
        /// Prints a string to the client, including a newline
        /// </summary>
        /// <param name="whatToPrint">
        /// A string that represents what to print to the client
        /// </param>
        public void println(String whatToPrint)
        {
            whatToPrint = whatToPrint + "\r\n";
            print(whatToPrint);
        }

        /// <summary>
        /// Prints an array of Byte objects to the client, it can be used in situations where you need to send binary objects like images or files
        /// </summary>
        /// <param name="whatToPrint">The array of objects to be sent</param>
        public void print(Byte[] whatToPrint)
        {
            stream.Write(whatToPrint, 0, whatToPrint.Length);
        }


        void initializeMemoryStream()
        {
            this.stream = new MemoryStream();
        }

        /// <summary>
        /// Sets the header Content-Type to the specified
        /// </summary>
        /// <param name="type">The type of data to be sent in the response</param>
        public void setContentType(string type)
        {
            setHeader("Content-Type", type);
        }

        /// <summary>
        /// Set the header specified and replaces the value when the header is already setted
        /// </summary>
        /// <param name="header">The name of the header</param>
        /// <param name="value">The value to be set</param>
        public void setHeader(string header, string value)
        { 
            if (this.headers.ContainsKey(header)) {
            this.headers.Remove(header);
            }
            this.headers.Add(header, value);
        }

        /// <summary>
        /// Construct a Response object that can will be used to send data to the client
        /// </summary>
        /// <param name="socket">The socket connected, used to send data</param>
        /// <param name="request">The request received from the client</param>
        internal Response(ref Socket socket, Request request)
        {
            this.request = request;
            this.socket = socket;
            initializeMemoryStream();
            this.cookies = new ArrayList();
            this.statusCode = "200";
            headers = new Hashtable();
            headers.Add("Server", System.Environment.MachineName);
            headers.Add("Content-Type", "text/html");
            headers.Add("Accept-Ranges", "bytes");
        }

        /// <summary>
        /// Adds the Cookie object to the Cookie list that will be sent to the client
        /// </summary>
        /// <param name="cookie">The cookie object to be added</param>
        public void addCookie(Cookie cookie)
        {
            this.cookies.Add(cookie);
        }

        internal void flush()
        {
            SendHeader();
            send(stream.ToArray());
            initializeMemoryStream();
        }

        void send(Byte[] bytesToSend)
        {
            int bytes = 0;

            try
            {
                if (this.socket.Connected)
                {
                    bytes = this.socket.Send(bytesToSend, bytesToSend.Length, 0);
                    if (bytes == -1)
                    {
                        // Socket error
                    }
                    else
                    {
                        // I Think it's OK
                    }
                }
                else
                {
                    // Link broken
                }
            }
            catch (Exception e)
            {
                this.request.server.handleVerbosity(e);
            }
        }

        void SendHeader()
        {
            String sBuffer = "";
            sBuffer = sBuffer + request.m_HTTPVersion + " " + statusCode + " " + Response.statusCodes[statusCode] + "\r\n";
            IDictionaryEnumerator en = headers.GetEnumerator();
            while (en.MoveNext())
                sBuffer = sBuffer + en.Key + ": " + en.Value + "\r\n";
            for (int i = 0; i < this.cookies.Count; i++)
            {
                Cookie cookie = (Cookie)this.cookies[i];
                sBuffer = sBuffer + cookie.serialize() + "\r\n";
            }

            sBuffer = sBuffer + "Content-Length: " + this.stream.Length + "\r\n\r\n";
            Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);
            StringBuilder sb = new StringBuilder();
            sb.Append(System.Text.UTF8Encoding.UTF8.GetChars(bSendData));
            send(bSendData);
        }



    }

}
