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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    [biz.ritter.develop.ReleaseNote(jVersion = 1.0, port = 1)]
    public abstract class InputStream : Closeable
    {
        private const int END_OF_STREAM = -1;
        private static byte[] skipBuffer;
        private static readonly int SKIP_BUFFER_SIZE = 2048;

        public abstract int read(); // throws IOException
        public virtual int read(byte[] buffer)
        {
            return this.read(buffer, 0, buffer.Length);
        }
        public virtual int read (byte [] buffer, int beginOffset, int length) {
            if (0 > beginOffset || beginOffset + length > buffer.Length || length < 0)
            {
                throw new java.lang.IndexOutOfBoundsException();
            }
            int input = this.read();
            if (END_OF_STREAM == input)
            {
                return END_OF_STREAM;
            }
            else
            {
                int readedByteCount = 1;
                buffer[beginOffset] = (byte)input;
                try
                {
                    for (; readedByteCount < length; readedByteCount++)
                    {
                        input = read();
                        if (END_OF_STREAM == input) break;
                        else
                        {
                            buffer[beginOffset + readedByteCount] = (byte)input;
                        }
                    }
                }
                catch (java.io.IOException ignored) /* catch, because the need to return readed byte count*/{ }
                return readedByteCount;
            }
        }

        public virtual void close() { }
        public virtual int available()
        {
            return 0;
        }
        public virtual long skip(long n)
        {
            long remaining = n;
            int nr;
            if (skipBuffer == null) skipBuffer = new byte[SKIP_BUFFER_SIZE];

            byte[] localSkipBuffer = skipBuffer;

            if (n <= 0)
            {
                return 0;
            }

            while (remaining > 0)
            {
                nr = read(localSkipBuffer, 0,
                      (int)java.lang.Math.min(SKIP_BUFFER_SIZE, remaining));
                if (nr < 0)
                {
                    break;
                }
                remaining -= nr;
            }
            return n - remaining;
        }
        public virtual void mark(int readlimit) { lock (this) { } }
        public virtual void reset()
        {
            throw new IOException("Mark / reset not supported");
        }
        public virtual bool markSupported()
        {
            return false;
        }
    }
}
