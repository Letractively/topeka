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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Topeka
{   
    /// <summary>
    /// This class is used to handle requests from clients.
    /// It creates a new thread to handle the request.
    /// </summary>
    public class requestHandler
    {
        Socket mySocket;
        Server server;

        /// <summary>
        /// Creates a requestHandler object and starts the thread for handling the request
        /// </summary>
        /// <param name="socket">The Socket connected</param>
        /// <param name="server">The Server Object</param>
        public requestHandler(Socket socket, Server server)
        {
            mySocket = socket;
            this.server = server;
            Thread thread = new Thread(startHandling);
            thread.Start();
        }


        private void startHandling()
        {
            try
            {
                Tokenizer ip = new Tokenizer((string)mySocket.RemoteEndPoint.ToString(), ":");

                // Receive max 16k from the client
                Byte[] receiveBuffer = new Byte[16384];

                int i = mySocket.Receive(receiveBuffer, receiveBuffer.Length, 0);

                // Convert the data received to UTF8
                string stringBuffer = Encoding.UTF8.GetString(receiveBuffer);

                // Create the request object
                Request request = new Request(stringBuffer, server);

                try
                {
                    request.m_ClientIP = ip.nextToken();
                }
                catch (Exception e)
                {
                    request.server.handleVerbosity(e);
                }


                Response response = new Response(ref mySocket, request);

                Servlet instance;

                string servlet=request.Class;

                if (servlet == "") servlet = "index";

                server.handleVerbosity("Client requested " + servlet);

                    Type type = Servlet.getServlet(servlet);


                    if (type != null)
                    {
                        try
                        {
                            instance = (Servlet)Activator.CreateInstance(type);
                            try
                            {
                                if (request.getParameter("method") != null) instance.invokeMethod(type, request.getParameter("method"), request, response);
                                else if (request.Method == "GET") instance.doGet(request, response);
                                else if (request.Method == "POST") instance.doPost(request, response);
                            }
                            catch (Exception) { }

                        }
                        catch (Exception e)
                        {
                            response.statusCode = "404";
                            response.println("Resource <b>\"" + request.Class + "\"</b> not found.");
                            request.server.handleVerbosity("Resource " + request.Class + " not found.");
                            request.server.handleVerbosity(e);
                        }
                    }
                    else if (servlet == "index") 
                    {
                        Servlet.doGetIndex(request, response);
                    }
                    else if (servlet.ToUpper() == "FAVICON.ICO")
                    {
                        Servlet.doGetFavicon(request, response);
                    }
                    else if (servlet.ToUpper() == "TOPEKA.PNG")
                    {
                        Servlet.doGetTopekaLogo(request, response);
                    }

                response.flush();
                request = null;
                response = null;
            }
            catch (Exception e)
            {
                server.handleVerbosity(e);
            }
            mySocket.Close();
        }
    }
}
