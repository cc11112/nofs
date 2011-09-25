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
using System.Security.Cryptography;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util.zip
{

    ///<summary>
    ///The Adler32 class is used to compute a Adler32 checksum from data provided as
    ///input value.
    ///</summary>
    ///<seealso cref="http://en.wikipedia.org/wiki/Adler32#Example_implementation"/>
    ///<remarks>
    ///The Adler32 class compute a Adler32 checksum from data provided as input value.
    ///It implements the Wikipedia source code sample as C# code.
    ///If big array is used, you can become out of memory.
    ///</remarks>
    public class Adler32 : HashAlgorithm, java.util.zip.Checksum
    {

        /// <summary>
        /// The modulo factor for Adler32
        /// </summary>
        private static readonly int MOD_ADLER32 = 65521;
        /// <summary>
        /// The content for compute checksum
        /// </summary>
        private byte[] content;
        /// <summary>
        /// The last checksum
        /// </summary>
        private long checksum;

        /// <summary>
        /// Construct a new Adler32 instance.
        /// </summary>
        public Adler32()
        {
            this.Initialize();
        }
        /// <summary>
        /// Return the Adler32 checksum
        /// </summary>
        /// <returns>Adler32 checksum</returns>
        public long getValue()
        {
            return this.checksum;
        }

        /**
            * Resets the Adler32 checksum to it initial state.
            */
        public void reset()
        {
            this.Initialize();
        }

        /**
            * Updates this checksum with the byte value provided as integer.
            *
            * @param val
            *            represents the byte to update the checksum.
            */
        public void update(int val)
        {
            this.update(new byte[] { (byte)val });
        }

        /**
            * Updates this checksum with the bytes contained in buffer {@code buf}.
            *
            * @param buf
            *            the buffer holding the data to update the checksum with.
            */
        public void update(byte[] buf)
        {
            this.update(buf, 0, buf.Length);
        }

        /**
            * Updates this checksum with n bytes of data obtained from buffer {@code
            * buf}, starting at offset {@code off}.
            *
            * @param buf
            *            the buffer to update the checksum.
            * @param off
            *            the offset in {@code buf} to obtain data from.
            * @param nbytes
            *            the number of bytes to read from {@code buf}.
            */
        public void update(byte[] buf, int off, int nbytes)
        {
            this.HashCore(buf, off, nbytes);
        }

        // see System.Security.Cryptography.HashAlgorithm
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            byte[] newContent = new byte[this.content.Length + cbSize];
            java.lang.SystemJ.arraycopy(content, 0, newContent, 0, content.Length);
            java.lang.SystemJ.arraycopy(array, ibStart, newContent, content.Length, cbSize);
            this.content = newContent;
            this.compute();
        }

        // see System.Security.Cryptography.HashAlgorithm
        protected override byte[] HashFinal()
        {
            return (""+this.checksum).getBytes();
        }

        // see System.Security.Cryptography.HashAlgorithm
        public override void Initialize()
        {
            content = new byte[0];
            this.compute();
        }

        /// <summary>
        /// Compute the checksum
        /// </summary>
        protected void compute()
        {
            long a = 1;
            long b = 0;
            foreach (byte next in this.content)
            {
                a = (a + next) % MOD_ADLER32;
                b = (b + a) % MOD_ADLER32;
            }
            this.checksum = (b << 16) | a;
        }
    }
}
