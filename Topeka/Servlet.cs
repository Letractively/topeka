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
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Globalization;

namespace Topeka
{
    /// <summary>
    /// This class represent a servlet. To use this class you can create a subclass and override the methods doGet and doPost to handle requests
    /// </summary>
    public abstract class Servlet
    {
        static List<Type> servlets;

        /// <summary>
        /// This method searches any servlet subclass within the assemblies and returns the class Type
        /// </summary>
        /// <param name="servlet">The servlet subclass to be searched</param>
        /// <returns>Returns the Type of the servlet subclass</returns>
        internal static Type getServlet(string servlet)
        {
            foreach (Type type in servlets)
            {
                if (type.Name == servlet) return type;
            }
            return null;
        }

        static Servlet() {
            servlets = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(Type.GetType("Topeka.Servlet")))
                    {
                        servlets.Add(type);
                    }
                
                }
            
            }
   
        }

        /// <summary>
        /// The abstract method that needs to be overriden to handle GET requests
        /// </summary>
        /// <param name="request">The Request object containing information about the request received from the client</param>
        /// <param name="response">The Response object that will be used to send data to the client</param>
        public abstract void doGet(Request request, Response response);

        /// <summary>
        /// The virtual method that needs to be overriden to handle POST requests
        /// </summary>
        /// <param name="request">The Request object containing information about the request received from the client</param>
        /// <param name="response">The Response object that will be used to send data to the client</param>
        public virtual void doPost(Request request, Response response)
        {
            doGet(request, response);
        }

        /// <summary>
        /// This method is called when the server detects a parameter "method" in the list of parameters
        /// Example: Users?method=AddUser will try to invoke the method AddUser from the class Users 
        /// </summary>
        /// <param name="type">The type of the Servlet subclass</param>
        /// <param name="method">The method name, in string format</param>
        /// <param name="request">The Request object used to handle this request</param>
        /// <param name="response">The Response object used to handle this request</param>
        internal void invokeMethod(Type type, string method, Request request, Response response)
        {
            object ibaseObject = Activator.CreateInstance(type);
            // Create the parameter list
            object[] arguments = new object[] { request, response };
            object result;
            // Dynamically Invoke the Object
            result = type.InvokeMember(method,
                                     BindingFlags.Default | BindingFlags.InvokeMethod,
                                     null,
                                     ibaseObject,
                                     arguments);

        }


        internal static void doGetIndex(Request request, Response response)
        {
            response.println("<img src=\"Topeka.png\" border=0 /><br>");
            response.println("<font size=1 face=verdana>");
            response.println("A lightweight multi-threaded free and easy to use Web Server<br>");
            response.println("Programmed by <b>Juanchi</b> @ 2010<br>");
            response.println("Server Started: <b>" + request.server.startTime + "</b></br>");
            response.println("Server Version: <b>"+Assembly.GetExecutingAssembly().GetName().Version.ToString()+"</b><br>");
            response.println("</font>");
        }

        internal static void doGetFavicon(Request request, Response response)
        {
            printResource(Assembly.GetExecutingAssembly(), "Topeka.img.favicon.ico", "ico", request, response);
        }

        internal static void doGetTopekaLogo(Request request, Response response)
        {
            printResource(Assembly.GetExecutingAssembly(), "Topeka.img.Topeka.png", "png", request, response);
        }

        internal static Stream getResourceStream(Assembly assembly, string resource)
        {
            return assembly.GetManifestResourceStream(resource);
        }

        internal static ResourceAssembly findResourceStream(Server server, string resource)
        {
            ResourceAssembly toReturn = null;
            foreach (ResourceAssembly resource_assembly in server.resources)
            {
                if (resource_assembly.assembly.GetManifestResourceStream(resource_assembly.assembly_namespace + "." + resource) != null) return resource_assembly;
            }
            return toReturn;
        }


        internal static void printResource(Assembly assembly, string resource, string extension, Request request, Response response)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);
            response.setContentType(Response.getMimeType(extension));
            BinaryReader reader = new BinaryReader(stream);
            byte[] bytes = new byte[stream.Length];
                int read;
                while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
                {
                    response.print(bytes);
                }
                reader.Close();
        }

    }
}
