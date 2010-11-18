using System;
using System.Collections.Generic;
using System.Text;

namespace Topeka
{
    /// <summary>
    /// A Class that represents a Range
    /// </summary>
    internal class Range
    {
        internal long _firstByte;
        /// <summary>
        /// The First Byte of the Range
        /// </summary>
        public long FirstByte
        { get { return _firstByte; } }

        internal long _lastByte;
        /// <summary>
        /// The Last Byte of the Range
        /// </summary>
        public long LastByte
        { get { return _lastByte; } }

        internal Range(long firstByte, long lastByte)
        { 
            this._firstByte = firstByte;
            this._lastByte = lastByte;
        }

        internal Range(string input)
        {
            this._firstByte = 0;
            this._lastByte = 0;

            Tokenizer values = new Tokenizer(input, "-");
            try { this._firstByte = (long)long.Parse(values.nextToken()); }
            catch (Exception) { }
            try { this._lastByte = (long)long.Parse(values.nextToken()); }
            catch (Exception) { }
        }

    }
}
