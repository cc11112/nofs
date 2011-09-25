/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using System.IO;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.dotnet.util.wrapper
{
    /// <summary>
    /// This utility class wrap a .net framework System.IO.Stream instance into a Java java.io.InputStream.
    /// </summary>
    public sealed class InputStreamWrapper : java.io.InputStream
    {
        /// <summary>
        /// The base .net Stream instance
        /// </summary>
        private Stream delegateInstance;
        /// <summary>
        /// Construct new wrapper for given Stream instance
        /// </summary>
        /// <param name="dotNetStream">Stream to wrap</param>
        public InputStreamWrapper(Stream dotNetStream)
        {
            this.delegateInstance = dotNetStream;
        }
        public override int read() // throws IOException
        {
            return this.delegateInstance.ReadByte();
        }
        /*
         * Optimzed implementation for direct using System.IO.Stream method
         */
        public override int read(byte[] buffer, int beginOffset, int length)
        {
            return this.delegateInstance.Read(buffer, beginOffset, length);
        }
        public override void close()
        {
            this.delegateInstance.Close();
        }
        public override long skip(long n)
        {
            return this.delegateInstance.Seek(n, SeekOrigin.Current);
 	    }
    }
}
