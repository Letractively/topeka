using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topeka;
using System.Reflection;
using System.Diagnostics;

namespace TestServer
{
    class Program : Servlet
    {
        static void Main(string[] args)
        {
            Server server = new Server(2000);
            server.addAssembly(Assembly.GetExecutingAssembly(), "TestServer");
            server.addAssembly(Assembly.Load("TestLibrary"), "TestLibrary.web");
            server.start();
            Process.Start("http://localhost:2000");
        }

        public override void doGet(Request request, Response response)
        {
            response.println("Hello TestServer");
        }
    }
}
