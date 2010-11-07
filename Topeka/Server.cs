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

namespace Topeka
{
    /// <summary>
    /// This class represents the server itself, it is the base to create the listeners that handles the requests
    /// </summary>
    public class Server
    {
        
        TcpListener myListener;
        private int port;
        internal string startTime;
        int verboseLevel;
        int logMaxSize;
        string logFileName;

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
        /// Sets the file name of the log
        /// </summary>
        /// <param name="fileName">A string with the full path to the file name</param>
        public void setLogFileName(string fileName)
        {
            this.logFileName = fileName;        
        }

        /// <summary>
        /// Starts the server, this method creates the first listening thread
        /// </summary>
        public void start()
        {
            try
            {
                myListener = new TcpListener(IPAddress.Any, port);
                myListener.Start();
                Thread th = new Thread(new System.Threading.ThreadStart(StartListen));
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

        void initializeLogVariables()
        {
            logMaxSize = 20971520;
            verboseLevel = 0;
            logFileName = null;
        }

        /// <summary>
        /// Creates the server object, setting the custom port received as parameter
        /// </summary>
        /// <param name="port">The port to listen</param>
        public Server(int port)
        {
            initializeLogVariables();
            this.port = port;
        }


        void StartListen()
        {

            while (true)
            {
            Start:
                try
                {
                    Socket socket = myListener.AcceptSocket();

                    if (socket.Connected)
                    {
                        requestHandler handler = new requestHandler(socket,this);
                    }
                    else
                        return;
                }
                catch (Exception e)
                {
                    this.handleVerbosity(e);
                    goto Start;
                }
            }

        }
    }





}
