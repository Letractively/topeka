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
        static Hashtable mimeTypes;
        static int bufferSize = 65536; // 64k

        static Response()
        {
            statusCodes = new Hashtable();
            statusCodes.Add("200", "OK");
            statusCodes.Add("201", "CREATED");
            statusCodes.Add("202", "Accepted");
            statusCodes.Add("203", "Partial Information");
            statusCodes.Add("204", "No Response");
            statusCodes.Add("206", "Partial content");
            statusCodes.Add("400", "Bad request");
            statusCodes.Add("401", "Unauthorized");
            statusCodes.Add("402", "PaymentRequired");
            statusCodes.Add("403", "Forbidden");
            statusCodes.Add("404", "Not found");
            statusCodes.Add("500", "Internal Error");
            statusCodes.Add("501", "Not implemented");

            // The MIME Types were taken from w3schools.com 
            mimeTypes = new Hashtable();
            mimeTypes.Add("323", "text/h323");
            mimeTypes.Add("acx", "application/internet-property-stream");
            mimeTypes.Add("ai", "application/postscript");
            mimeTypes.Add("aif", "audio/x-aiff");
            mimeTypes.Add("aifc", "audio/x-aiff");
            mimeTypes.Add("aiff", "audio/x-aiff");
            mimeTypes.Add("asf", "video/x-ms-asf");
            mimeTypes.Add("asr", "video/x-ms-asf");
            mimeTypes.Add("asx", "video/x-ms-asf");
            mimeTypes.Add("au", "audio/basic");
            mimeTypes.Add("avi", "video/x-msvideo");
            mimeTypes.Add("axs", "application/olescript");
            mimeTypes.Add("bas", "text/plain");
            mimeTypes.Add("bcpio", "application/x-bcpio");
            mimeTypes.Add("bin", "application/octet-stream");
            mimeTypes.Add("bmp", "image/bmp");
            mimeTypes.Add("c", "text/plain");
            mimeTypes.Add("cat", "application/vnd.ms-pkiseccat");
            mimeTypes.Add("cdf", "application/x-cdf");
            mimeTypes.Add("cer", "application/x-x509-ca-cert");
            mimeTypes.Add("class", "application/octet-stream");
            mimeTypes.Add("clp", "application/x-msclip");
            mimeTypes.Add("cmx", "image/x-cmx");
            mimeTypes.Add("cod", "image/cis-cod");
            mimeTypes.Add("cpio", "application/x-cpio");
            mimeTypes.Add("crd", "application/x-mscardfile");
            mimeTypes.Add("crl", "application/pkix-crl");
            mimeTypes.Add("crt", "application/x-x509-ca-cert");
            mimeTypes.Add("csh", "application/x-csh");
            mimeTypes.Add("css", "text/css");
            mimeTypes.Add("dcr", "application/x-director");
            mimeTypes.Add("der", "application/x-x509-ca-cert");
            mimeTypes.Add("dir", "application/x-director");
            mimeTypes.Add("dll", "application/x-msdownload");
            mimeTypes.Add("dms", "application/octet-stream");
            mimeTypes.Add("doc", "application/msword");
            mimeTypes.Add("dot", "application/msword");
            mimeTypes.Add("dvi", "application/x-dvi");
            mimeTypes.Add("dxr", "application/x-director");
            mimeTypes.Add("eps", "application/postscript");
            mimeTypes.Add("etx", "text/x-setext");
            mimeTypes.Add("evy", "application/envoy");
            mimeTypes.Add("exe", "application/octet-stream");
            mimeTypes.Add("fif", "application/fractals");
            mimeTypes.Add("flr", "x-world/x-vrml");
            mimeTypes.Add("gif", "image/gif");
            mimeTypes.Add("gtar", "application/x-gtar");
            mimeTypes.Add("gz", "application/x-gzip");
            mimeTypes.Add("h", "text/plain");
            mimeTypes.Add("hdf", "application/x-hdf");
            mimeTypes.Add("hlp", "application/winhlp");
            mimeTypes.Add("hqx", "application/mac-binhex40");
            mimeTypes.Add("hta", "application/hta");
            mimeTypes.Add("htc", "text/x-component");
            mimeTypes.Add("htm", "text/html");
            mimeTypes.Add("html", "text/html");
            mimeTypes.Add("htt", "text/webviewhtml");
            mimeTypes.Add("ico", "image/x-icon");
            mimeTypes.Add("ief", "image/ief");
            mimeTypes.Add("iii", "application/x-iphone");
            mimeTypes.Add("ins", "application/x-internet-signup");
            mimeTypes.Add("isp", "application/x-internet-signup");
            mimeTypes.Add("jfif", "image/pipeg");
            mimeTypes.Add("jpe", "image/jpeg");
            mimeTypes.Add("jpeg", "image/jpeg");
            mimeTypes.Add("jpg", "image/jpeg");
            mimeTypes.Add("js", "application/x-javascript");
            mimeTypes.Add("latex", "application/x-latex");
            mimeTypes.Add("lha", "application/octet-stream");
            mimeTypes.Add("lsf", "video/x-la-asf");
            mimeTypes.Add("lsx", "video/x-la-asf");
            mimeTypes.Add("lzh", "application/octet-stream");
            mimeTypes.Add("m13", "application/x-msmediaview");
            mimeTypes.Add("m14", "application/x-msmediaview");
            mimeTypes.Add("m3u", "audio/x-mpegurl");
            mimeTypes.Add("man", "application/x-troff-man");
            mimeTypes.Add("mdb", "application/x-msaccess");
            mimeTypes.Add("me", "application/x-troff-me");
            mimeTypes.Add("mht", "message/rfc822");
            mimeTypes.Add("mhtml", "message/rfc822");
            mimeTypes.Add("mid", "audio/mid");
            mimeTypes.Add("mny", "application/x-msmoney");
            mimeTypes.Add("mov", "video/quicktime");
            mimeTypes.Add("movie", "video/x-sgi-movie");
            mimeTypes.Add("mp2", "video/mpeg");
            mimeTypes.Add("mp3", "audio/mpeg");
            mimeTypes.Add("mpa", "video/mpeg");
            mimeTypes.Add("mpe", "video/mpeg");
            mimeTypes.Add("mpeg", "video/mpeg");
            mimeTypes.Add("mpg", "video/mpeg");
            mimeTypes.Add("mpp", "application/vnd.ms-project");
            mimeTypes.Add("mpv2", "video/mpeg");
            mimeTypes.Add("ms", "application/x-troff-ms");
            mimeTypes.Add("mvb", "application/x-msmediaview");
            mimeTypes.Add("nws", "message/rfc822");
            mimeTypes.Add("oda", "application/oda");
            mimeTypes.Add("p10", "application/pkcs10");
            mimeTypes.Add("p12", "application/x-pkcs12");
            mimeTypes.Add("p7b", "application/x-pkcs7-certificates");
            mimeTypes.Add("p7c", "application/x-pkcs7-mime");
            mimeTypes.Add("p7m", "application/x-pkcs7-mime");
            mimeTypes.Add("p7r", "application/x-pkcs7-certreqresp");
            mimeTypes.Add("p7s", "application/x-pkcs7-signature");
            mimeTypes.Add("pbm", "image/x-portable-bitmap");
            mimeTypes.Add("pdf", "application/pdf");
            mimeTypes.Add("pfx", "application/x-pkcs12");
            mimeTypes.Add("pgm", "image/x-portable-graymap");
            mimeTypes.Add("pko", "application/ynd.ms-pkipko");
            mimeTypes.Add("pma", "application/x-perfmon");
            mimeTypes.Add("pmc", "application/x-perfmon");
            mimeTypes.Add("pml", "application/x-perfmon");
            mimeTypes.Add("pmr", "application/x-perfmon");
            mimeTypes.Add("pmw", "application/x-perfmon");
            mimeTypes.Add("pnm", "image/x-portable-anymap");
            mimeTypes.Add("pot,", "application/vnd.ms-powerpoint");
            mimeTypes.Add("ppm", "image/x-portable-pixmap");
            mimeTypes.Add("pps", "application/vnd.ms-powerpoint");
            mimeTypes.Add("ppt", "application/vnd.ms-powerpoint");
            mimeTypes.Add("prf", "application/pics-rules");
            mimeTypes.Add("ps", "application/postscript");
            mimeTypes.Add("pub", "application/x-mspublisher");
            mimeTypes.Add("qt", "video/quicktime");
            mimeTypes.Add("ra", "audio/x-pn-realaudio");
            mimeTypes.Add("ram", "audio/x-pn-realaudio");
            mimeTypes.Add("ras", "image/x-cmu-raster");
            mimeTypes.Add("rgb", "image/x-rgb");
            mimeTypes.Add("rmi", "audio/mid");
            mimeTypes.Add("roff", "application/x-troff");
            mimeTypes.Add("rtf", "application/rtf");
            mimeTypes.Add("rtx", "text/richtext");
            mimeTypes.Add("scd", "application/x-msschedule");
            mimeTypes.Add("sct", "text/scriptlet");
            mimeTypes.Add("setpay", "application/set-payment-initiation");
            mimeTypes.Add("setreg", "application/set-registration-initiation");
            mimeTypes.Add("sh", "application/x-sh");
            mimeTypes.Add("shar", "application/x-shar");
            mimeTypes.Add("sit", "application/x-stuffit");
            mimeTypes.Add("snd", "audio/basic");
            mimeTypes.Add("spc", "application/x-pkcs7-certificates");
            mimeTypes.Add("spl", "application/futuresplash");
            mimeTypes.Add("src", "application/x-wais-source");
            mimeTypes.Add("sst", "application/vnd.ms-pkicertstore");
            mimeTypes.Add("stl", "application/vnd.ms-pkistl");
            mimeTypes.Add("stm", "text/html");
            mimeTypes.Add("svg", "image/svg+xml");
            mimeTypes.Add("sv4cpio", "application/x-sv4cpio");
            mimeTypes.Add("sv4crc", "application/x-sv4crc");
            mimeTypes.Add("swf", "application/x-shockwave-flash");
            mimeTypes.Add("t", "application/x-troff");
            mimeTypes.Add("tar", "application/x-tar");
            mimeTypes.Add("tcl", "application/x-tcl");
            mimeTypes.Add("tex", "application/x-tex");
            mimeTypes.Add("texi", "application/x-texinfo");
            mimeTypes.Add("texinfo", "application/x-texinfo");
            mimeTypes.Add("tgz", "application/x-compressed");
            mimeTypes.Add("tif", "image/tiff");
            mimeTypes.Add("tiff", "image/tiff");
            mimeTypes.Add("tr", "application/x-troff");
            mimeTypes.Add("trm", "application/x-msterminal");
            mimeTypes.Add("tsv", "text/tab-separated-values");
            mimeTypes.Add("txt", "text/plain");
            mimeTypes.Add("uls", "text/iuls");
            mimeTypes.Add("ustar", "application/x-ustar");
            mimeTypes.Add("vcf", "text/x-vcard");
            mimeTypes.Add("vrml", "x-world/x-vrml");
            mimeTypes.Add("wav", "audio/x-wav");
            mimeTypes.Add("wcm", "application/vnd.ms-works");
            mimeTypes.Add("wdb", "application/vnd.ms-works");
            mimeTypes.Add("wks", "application/vnd.ms-works");
            mimeTypes.Add("wmf", "application/x-msmetafile");
            mimeTypes.Add("wps", "application/vnd.ms-works");
            mimeTypes.Add("wri", "application/x-mswrite");
            mimeTypes.Add("wrl", "x-world/x-vrml");
            mimeTypes.Add("wrz", "x-world/x-vrml");
            mimeTypes.Add("xaf", "x-world/x-vrml");
            mimeTypes.Add("xbm", "image/x-xbitmap");
            mimeTypes.Add("xla", "application/vnd.ms-excel");
            mimeTypes.Add("xlc", "application/vnd.ms-excel");
            mimeTypes.Add("xlm", "application/vnd.ms-excel");
            mimeTypes.Add("xls", "application/vnd.ms-excel");
            mimeTypes.Add("xlt", "application/vnd.ms-excel");
            mimeTypes.Add("xlw", "application/vnd.ms-excel");
            mimeTypes.Add("xof", "x-world/x-vrml");
            mimeTypes.Add("xpm", "image/x-xpixmap");
            mimeTypes.Add("xwd", "image/x-xwindowdump");
            mimeTypes.Add("z", "application/x-compress");
            mimeTypes.Add("zip", "application/zip");

        }


        Socket socket;
        MemoryStream stream;
        internal Hashtable headers;
        internal bool alreadyFlushed;
        internal long contentLength;

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

        internal static string getMimeType(string extension)
        {
            if (mimeTypes.ContainsKey(extension)) return (string)mimeTypes[extension];
            else return "application/octet-stream";
        }

        long getTotalLength()
        {
            long length = 0;
            foreach (Range range in request.Ranges)
            {
                length += range._lastByte - range._firstByte;
            }
            return length;
        }

        /// <summary>
        /// Prints a file to the client, the MIME type is sent if the extension is known, this method uses a buffer to read the file before sending it to the socket.
        /// If the socket is closed, then it stops reading the file. Use this method when you need to transfer large files that cannot be held in a memory stream.
        /// </summary>
        /// <param name="fileName">The filename to send to the client</param>
        /// <param name="forceDownload">Set this flag to true when you want the browser force the download instead of displaying it</param>
        public void printFile(string fileName, bool forceDownload)
        {
            if (File.Exists(fileName))
            {
                // Create a FileStream
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                this.setContentType(Response.getMimeType(Path.GetExtension(fileName).Substring(1)));
                string attachment = "";
                if (forceDownload) attachment = "attachment; "; // Create a header to force the browser download the file
                this.setHeader("Content-disposition", attachment + "filename=" + HTMLHelper.encode(Path.GetFileName(fileName)));
                if (request.Ranges.Count > 0)
                {
                    foreach (Range range in request.Ranges)
                    {
                        if (range._lastByte == 0) range._lastByte = fs.Length;
                        headers.Add("Content-Range", range._firstByte + "-" + (range._lastByte - 1) + "/" + fs.Length);
                    }
                    this.contentLength = getTotalLength();
                    this.statusCode = "206";
                }
                else this.contentLength = fs.Length;

                SendHeader();

                if (request.Ranges.Count > 0)
                {
                    foreach (Range range in request.Ranges)
                    {
                        transferByteRange(fs, range._firstByte, range._lastByte);
                    }
                }
                else
                    transferByteRange(fs, 0, fs.Length);

                // Close the file
                fs.Close();
                // Set the AlreadyFlushed flag
                alreadyFlushed = true;
            }
            else
            {
                statusCode = "404";
                println("<font face='verdana'>Resource <b>\"" + request.Page + "\"</b> not found.</font>");
            }
        }

        void transferByteRange(FileStream fs, long startPos, long endPos)
        {
            // Create the byte array with the buffer size defined
            byte[] bytes = new byte[bufferSize];
            int read = bufferSize;
            long bytesRemaining = endPos - startPos;

            // Sets the position to the defined
            fs.Position = startPos;

            // Transfer file contents to the client
            while ((read = fs.Read(bytes, 0, bytes.Length)) != 0)
            {
                if (read > bytesRemaining)
                {
                    byte[] finalBytes = new byte[bytesRemaining];
                    Array.Copy(bytes, finalBytes, bytesRemaining);
                    if (this.socket.Connected) this.send(finalBytes);
                    break;
                }
                else if (read < bytes.Length)
                {
                    byte[] finalBytes = new byte[read];
                    Array.Copy(bytes, finalBytes, read);
                    if (this.socket.Connected) this.send(finalBytes);
                    break;
                }
                else
                {
                    if (this.socket.Connected) this.send(bytes);
                    else break;
                }
                bytesRemaining -= read;
            }
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
            headers.Add("Cache-Control", "no-cache");
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
            if (!alreadyFlushed)
            {
                this.contentLength = this.stream.Length;
                SendHeader();
                send(stream.ToArray());
            }
            alreadyFlushed = true;
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

            sBuffer = sBuffer + "Content-Length: " + contentLength + "\r\n\r\n";
            Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);
            StringBuilder sb = new StringBuilder();
            sb.Append(System.Text.UTF8Encoding.UTF8.GetChars(bSendData));
            send(bSendData);
        }



    }

}
