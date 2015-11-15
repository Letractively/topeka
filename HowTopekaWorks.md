The starter Thread starts with a **TcpListener** on the specified port, then when a page is requested, it creates another Thread to handle communication. This Thread creates the corresponding **Request** and **Response** objects having properties and method to let the user handle the request properly.

# **The Servlet object** #
##  ##
The Servlet object is the base class for any page that you will like to serve. To start you will need to create a subclass of Servlet, and define the method doGet. This method is called when the browser requests the page with the "GET" method. To handle "POST" requests you can implement the method doPost, wich is defined as virtual, so you are not forced to implement. Both methods have two parameters: a Request object and a Response object. This classes have the required methods to let the user  handle the request.
##  ##

# **The Request object** #
##  ##
The Request object has all the information that the client sent (the web browser). This information is parsed on every request and is stored on local variables that can be accessed as read only properties.
##  ##

# **The Response object** #
##  ##
The Response object has all the methods to send data to the client (the web browser). This class can be used to send plain text or binary data. You have total control of the http headers that will be sent, so you can change the content type, length, cache settings and so on.
##  ##