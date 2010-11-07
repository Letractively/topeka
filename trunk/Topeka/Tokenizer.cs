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

namespace Topeka
{
    /// <summary>
    /// A Class used to cut strings based on delimiter chars
    /// </summary>
    public class Tokenizer
    {
        String input;
        String token;
        String temp;

        /// <summary>
        /// Creates a Tokenizer object based on a string
        /// <param name="input">The string to be tokenized</param>
        /// <param name="token">The token string that is used as delimiter</param>
        /// </summary>
        public Tokenizer(String input, String token)
        {
            this.input = input;
            this.token = token;
            this.temp = new string(input.ToCharArray());
        }

        /// <summary>
        /// Call this method to get the next string, then it automatically moves the cursor to the next token
        /// </summary>
        /// <returns>Return a string representing the current token</returns>
        public string nextToken()
        {
            String toReturn = null;
            if (this.temp.IndexOf(this.token) >= 0)
            {
                toReturn = this.temp.Substring(0, this.temp.IndexOf(this.token));
                this.temp = this.temp.Substring(this.temp.IndexOf(this.token) + token.Length, this.temp.Length - this.temp.IndexOf(this.token) - token.Length);
            }
            else
            {
                toReturn = new string(temp.ToCharArray());
                temp = "";
            }
            return toReturn;
        }

        /// <summary>
        /// Call this method to know if there are tokens left
        /// </summary>
        /// <returns>Return true if there are more tokens left, false if there are no more tokens</returns>
        public bool hasMoreTokens()
        {
            if (this.temp.Length > 0) return true;
            else return false;
        }

        /// <summary>
        /// Moves the cursor to the first token
        /// </summary>
        public void firstToken()
        {
            this.temp = new string(input.ToCharArray());
        }


    }
}
