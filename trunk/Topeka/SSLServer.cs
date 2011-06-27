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
using System.Threading;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Web;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Topeka
{
    /// <summary>
    /// This class represents the server itself, it is the base to create the listeners that handles the requests
    /// </summary>
    public class SSLServer : Server
    {
        
        X509Certificate serverCertificate;

        /// <summary>
        /// Returns the X509Certificate associated with this server
        /// </summary>
        public X509Certificate getCertificate()
        {
            return this.serverCertificate;
        }

        /// <summary>
        /// Creates the server object, setting the custom port received as parameter
        /// </summary>
        /// <param name="port">The port to listen</param>
        /// <param name="certificateFile">The absolute path to the certificate file</param>
        public SSLServer(int port, string certificateFile)
        {
            serverCertificate = X509Certificate.CreateFromCertFile(certificateFile);
            this.ssl = true;
            ipAddress = IPAddress.Any;
            initializeVariables();
            this.port = port;
        }

        /// <summary>
        /// Creates the server object, setting the custom port received as parameter
        /// </summary>
        /// <param name="port">The port to listen</param>
        /// <param name="certificate">An X509Certificate object previously created</param>
        public SSLServer(int port, X509Certificate certificate)
        {
            serverCertificate = certificate;
            this.ssl = true;
            ipAddress = IPAddress.Any;
            initializeVariables();
            this.port = port;
        }

        /// <summary>
        /// Creates the server object, setting the custom port and IPAddress received as parameter
        /// </summary>
        /// <param name="port">The port to listen</param>
        /// <param name="ipAddress">The IPAddress to listen</param>
        /// <param name="certificateFile">The absolute path to the certificate file</param>
        public SSLServer(int port, string certificateFile,  IPAddress ipAddress)
        {
            serverCertificate = X509Certificate.CreateFromCertFile(certificateFile);
            this.ipAddress = ipAddress;
            this.ssl = true;
            initializeVariables();
            this.port = port;
        }

    }





}
