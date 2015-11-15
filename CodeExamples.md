# For all examples you need to create a Server Object #
```
using Topeka;

namespace TestServer
{
    public class TestServer
    {
        public static void Main(string[] ar)
        {
            Server server = new Server(80);
            server.start();
        }
    }
}
```


# Simple Hello World #
Simply prints Hello World to the client, note that you need to create a subclass of **Servlet**
```
    public class helloWorld : Servlet
    {
        public override void doGet(Request request, Response response)
        {
            response.println("Hello World!");
        }
    }
```


# Download a File! #
This example reads a file from the filesystem and sends it as binary. The header "Content-disposition" is set to force the browser to present the download dialog instead of showing the file on the browser.
```
    public class printSomeFile : Servlet 
    {
        public override void doGet(Request request, Response response)
        {
            string filename = @"C:\Downloads\Readme.txt";
            // Create a FileStream
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            // Create a binary Reader to put the file content
            BinaryReader reader = new BinaryReader(fs);
            // Create the byte array
            byte[] bytes = new byte[fs.Length];
            int read;
            // Set content type to let the browser know what we are sending
            response.setContentType("application/octet-stream");
            // Create a header to force the browser download the file
            response.setHeader("Content-disposition", "attachment; filename=Readme.txt");
            // Transfer file contents to the byte array
            while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
            {
                response.print(bytes);
            }
            // Close the reader and file
            reader.Close();
            fs.Close();
        }
    }
```


# Embed an image on your project! #
This example reads an image that is embedded on your project and sends it to the browser. Please note that you will need to add the image first, and set the Build Action of the image to "Embedded Resource" to make it work.
```
    public class printEmbeddedImage : Servlet
    { 
        public override void doGet(Request request, Response response)
        {
            // Get image from assembly
            System.Drawing.Image image = (Image)Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TestServer.Topeka.png"));
            // Create the memory Stream
            MemoryStream imageStream = new MemoryStream();
            // Put the image in the stream. It is important to put the image format, if not the image will look weird
            image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
            // Create a byte array to put the stream
            byte[] imageContent = new Byte[imageStream.Length];
            // Rewind to position 0 of the stream
            imageStream.Position = 0;
            // Transfer the stream to the byte array
            imageStream.Read(imageContent, 0, (int)imageStream.Length);
            // Set the content type to allow the browser to know we are sending the image
            response.setContentType("image/png");
            // Print the image
            response.print(imageContent);
        }

    }
```

# Cookies made easy to handle sessions #
This example sets a cookie to last 10 seconds on the response, and then it reads it on the request. To make this test, you need to refresh the page within 10 seconds to see the cookie, otherwise the browser will not send back the cookie and you will not see it
```
    public class cookieTest : Servlet
    {
        public override void doGet(Request request, Response response)
        {
            response.println("<img src=\"Topeka.png\" border=0 /><br><br>");
            response.println("<font size=1 face=verdana>");
            response.println("<b>Cookie Test</b>");
            response.println("<br><br>");
            response.println("Current Cookies:");
            response.println("<br>");
            foreach (Cookie cookie in request.Cookies)
            {
            response.println("Cookie name: <b>"+cookie.name + "</b> - Value: <b>" + cookie.value+"</b>");
            }
            Cookie cookieS = new Cookie("Test Cookie", "Success");
            cookieS.setMaxAge(10);
            response.addCookie(cookieS);
        }

    }
```


# Overriding default page #
I named index to the default page, so with this example when you implement the index class you will handle the default page (http://localhost/)
```
    public class index : Servlet
    {
        public override void doGet(Request request, Response response)
        {
            response.println("Overriding default page");
        }

    }
```


# Custom Methods! #
Maybe you have one scenario when you need to have different methods that handle different situations (other that GET or POST). Topeka implemented a way to handle this using reflection just by setting the **"method"** parameter, so when you set the method parameter, the Servlet tries to invoke the method name specified as the parameter value.
```
    public class otherMethodTest : Servlet
    {
        public override void doGet(Request request, Response response)
        {
            response.println("<a href=\"otherMethodTest?method=clickButton\">Click me and you will see!</a>");
        }

        public void clickButton(Request request, Response response)
        {
            response.println("You clicked the button!");
        }
    }

```




# No Cache #
With this example we set the header Cache-Control to no-cache to tell the browser not to cache the content
```
    public class noCache : Servlet
    {
        public override void doGet(Request request, Response response)
        {
            response.setHeader("Cache-Control", "no-cache");
            response.println("This content is not cached by the browser");
        }
    }
```