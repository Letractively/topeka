﻿/*
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


namespace Topeka
{
    /// <summary>
    /// This class represents the server itself, it is the base to create the listeners that handles the requests
    /// </summary>
    public class Server
    {

        internal TcpListener myListener;
        internal int port;
        internal IPAddress ipAddress;
        internal string startTime;
        internal int verboseLevel;
        internal int logMaxSize;
        internal string logFileName;
        internal string rootPath;
        internal bool stopRequested;
        internal bool ssl;
        internal ManualResetEvent signalThread;
        internal List<ResourceAssembly> resources;

        /// <summary>
        /// Sets the Verbosity level to print to a console or file
        /// If set to 0, no messages will be printed
        /// If set to 1, Information messages will be printed
        /// If set to 2, Information and Exceptions will be printed
        /// </summary>
        /// <param name="verboseLevel">The verbose level to set</param>
        public void setVerbosityLevel(int verboseLevel)
        {
            this.verboseLevel = verboseLevel;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Server()
        { 
        }

        /// <summary>
        /// Returns true if the server is SSL-Enabled
        /// </summary>
        public bool isSSL()
        {
            return this.ssl;
        }



        /// <summary>
        /// Sets the Root Path to handle static pages. 
        /// If not set, the server will not handle static pages
        /// </summary>
        /// <param name="rootPath">The Path where you want to serve. It must be an absolute path.</param>
        public void setRootPath(string rootPath)
        {
            if (rootPath.EndsWith(Path.DirectorySeparatorChar.ToString())) this.rootPath = rootPath;
            else this.rootPath = rootPath + Path.DirectorySeparatorChar.ToString();
        }

        /// <summary>
        /// Add an Assembly to read static pages.
        /// To read a static page from an assembly you must embed that page as embedded resource (in object properties)
        /// </summary>
        /// <param name="assembly">The Assembly where the static pages are stored in as resources</param>
        /// <param name="assembly_namespace">The Namespace in wich the resources are stored.</param>
        public void addAssembly(Assembly assembly, string assembly_namespace)
        {
            ResourceAssembly resource = new ResourceAssembly(assembly, assembly_namespace);
            resources.Add(resource);
        }




        /// <summary>
        /// Sets the file name of the log
        /// </summary>
        /// <param name="fileName">A string with the full path to the file name</param>
        public void setLogFileName(string fileName)
        {
            this.logFileName = fileName;        
        }

        /// <summary>
        /// Sets the IPAddress to listen to
        /// </summary>
        /// <param name="ipAddress">The IPAddress of the network interface you want to listen to</param>
        public void setIPAddress(IPAddress ipAddress)
        {
            this.ipAddress = ipAddress;
        }


        /// <summary>
        /// Starts the server, this method creates the first listening thread
        /// </summary>
        public void start()
        {
            try
            {
                myListener = new TcpListener(ipAddress, port);
                myListener.Start();
                Thread th = new Thread(new System.Threading.ThreadStart(AcceptSocket));
                th.Start();
                this.handleVerbosity("Web server is now running...");
                startTime = addDate("");
            }
            catch (Exception e)
            {
                this.handleVerbosity(e);
            }
        
        }


        internal void handleVerbosity(string message)
        {
            if (verboseLevel > 0)
                printToConsoleOrFile(addDate(message));
        }

        internal void printToConsoleOrFile(string text)
        {
            try
            {
                if (logFileName != null) writeLog(text);
            }
            catch (Exception) { }
            if (logFileName == null) Console.WriteLine(text);
        }

        internal void handleVerbosity(Exception e)
        {
            if (verboseLevel > 1)
                {
                    StringBuilder st = new StringBuilder();
                    st.Append(addDate(e.Message));
                    st.Append(e.StackTrace);
                    printToConsoleOrFile(st.ToString());
                }
        }

        internal string addDate(string text)
        {
            string datetime = "";
            try
            {
                DateTime date1 = System.DateTime.Now;
                CultureInfo ci = CultureInfo.InvariantCulture;
                datetime = date1.ToString("ddd MMM dd HH:mm:ss.FFF yyyy", ci);
            }
            catch (Exception) { }
            return datetime + " - " + text;
        }

        internal void writeLog(string text)
        {
            lock (this)
            {
                FileInfo f = new FileInfo(this.logFileName);
                    if (f.Exists)
                    {
                        if (f.Length > logMaxSize)
                        {
                            try
                            {
                                FileInfo f_old = new FileInfo(this.logFileName + ".old");
                                f_old.Delete();
                                f.MoveTo(this.logFileName + ".old");
                            }
                            catch (Exception) { }
                        }
                    }
                    else 
                    {
                        // File does not exist
                    }

                StreamWriter SW;
                SW = File.AppendText(this.logFileName);
                SW.WriteLine(text);
                SW.Close();
            }

        }


        /// <summary>
        /// Stops the current Server, closes the listener
        /// </summary>
        public void stop()
        {
            stopRequested = true;
            try
            {
                if (this.myListener.Server.Connected)
                {
                    // 5 Seconds to allow socket finalization
                    this.myListener.Server.Close(5);
                    // Stop the tcp listener
                    this.myListener.Stop();
                }
            }
            catch (Exception)
            {
            
            }
        }

        internal void initializeVariables()
        {
            logMaxSize = 20971520;
            verboseLevel = 0;
            logFileName = null;
            rootPath = null;
            stopRequested = false;
            signalThread = new ManualResetEvent(false);
            resources = new List<ResourceAssembly>();
        }

        /// <summary>
        /// Creates the server object, setting the custom port received as parameter
        /// </summary>
        /// <param name="port">The port to listen</param>
        public Server(int port)
        {
            ipAddress = IPAddress.Any;
            initializeVariables();
            this.port = port;
        }

        /// <summary>
        /// Creates the server object, setting the custom port and IPAddress received as parameter
        /// </summary>
        /// <param name="port">The port to listen</param>
        /// <param name="ipAddress">The IPAddress to listen</param>
        public Server(int port, IPAddress ipAddress)
        {
            this.ipAddress = ipAddress;
            initializeVariables();
            this.port = port;
        }

       

        void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                TcpListener listener = (TcpListener)ar.AsyncState;
                TcpClient client = listener.EndAcceptTcpClient(ar);
                requestHandler handler = new requestHandler(client, this);
                signalThread.Set();
            }
            catch (Exception e)
            {
                handleVerbosity(e);
            }
        }

        void AcceptSocket()
        {
            while (true)
            {
                signalThread.Reset();
                try
                {
                    myListener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallBack), myListener);
                }
                catch (Exception e)
                {
                    handleVerbosity(e);
                }
                signalThread.WaitOne();
                if (stopRequested) break;
            }

        }
    }





}
