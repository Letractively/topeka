This library is intended for users that want to embed a web server to an existing project. It has a similar sintax to java servlets framework, but simplified. It is programmed in **c#** using .net framework 2.0. It is also compatible with linux using **mono**

**The very basic and simple Hello World using Topeka**
```
using Topeka;

    public class testPage : Servlet
    {
        public static void Main(string[] ar)
        {
            // Instantiate a server object
            Server server = new Server(8080);
            server.start();
        }
        // Override Servlet method doGet to handle requests
        public override void doGet(Request request, Response response)
        {
            // Print Hello World to the client
            response.println("Hello World!");
        }
    }
```

Then when you start the server you could open a browser and point to http://localhost:8080/testPage and you will get the response from the Server.