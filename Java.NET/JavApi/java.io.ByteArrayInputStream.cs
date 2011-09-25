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
 */

using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    /**
     * A specialized {@link InputStream } for reading the contents of a byte array.
     * 
     * @see ByteArrayOutputStream
     */
    public class ByteArrayInputStream : InputStream {
        /**
         * The {@code byte} array containing the bytes to stream over.
         */
        protected internal byte[] buf;

        /**
         * The current position within the byte array.
         */
        protected internal int pos;

        /**
         * The current mark position. Initially set to 0 or the <code>offset</code>
         * parameter within the constructor.
         */
        protected int markJ;

        /**
         * The total number of bytes initially available in the byte array
         * {@code buf}.
         */
        protected int countJ;

        /**
         * Constructs a new {@code ByteArrayInputStream} on the byte array
         * {@code buf}.
         * 
         * @param buf
         *            the byte array to stream over.
         */
        public ByteArrayInputStream(byte[] buf) {
            this.markJ = 0;
            this.buf = buf;
            this.countJ = buf.Length;
        }

        /**
         * Constructs a new {@code ByteArrayInputStream} on the byte array
         * {@code buf} with the initial position set to {@code offset} and the
         * number of bytes available set to {@code offset} + {@code length}.
         * 
         * @param buf
         *            the byte array to stream over.
         * @param offset
         *            the initial position in {@code buf} to start streaming from.
         * @param length
         *            the number of bytes available for streaming.
         */
        public ByteArrayInputStream(byte[] buf, int offset, int length) {
            this.buf = buf;
            pos = offset;
            markJ = offset;
            countJ = offset + length > buf.Length ? buf.Length : offset + length;
        }

        /**
         * Returns the number of bytes that are available before this stream will
         * block. This method returns the number of bytes yet to be read from the
         * source byte array.
         * 
         * @return the number of bytes available before blocking.
         */
        public override int available() {
            lock (this) {
                return countJ - pos;
            }
        }

        /**
         * Closes this stream and frees resources associated with this stream.
         * 
         * @throws IOException
         *             if an I/O error occurs while closing this stream.
         */
        
        public override void close(){ //throws IOException {
            // Do nothing on close, this matches JDK behaviour.
        }

        /**
         * Sets a mark position in this ByteArrayInputStream. The parameter
         * {@code readlimit} is ignored. Sending {@code reset()} will reposition the
         * stream back to the marked position.
         * 
         * @param readlimit
         *            ignored.
         * @see #markSupported()
         * @see #reset()
         */
        
        public override void mark(int readlimit) {
            lock (this) {
                markJ = pos;
            }
        }

        /**
         * Indicates whether this stream supports the {@code mark()} and
         * {@code reset()} methods. Returns {@code true} since this class supports
         * these methods.
         * 
         * @return always {@code true}.
         * @see #mark(int)
         * @see #reset()
         */
        
        public override bool markSupported() {
            return true;
        }

        /**
         * Reads a single byte from the source byte array and returns it as an
         * integer in the range from 0 to 255. Returns -1 if the end of the source
         * array has been reached.
         * 
         * @return the byte read or -1 if the end of this stream has been reached.
         */
        
        public override int read() {
            lock (this) {
                return pos < countJ ? buf[pos++] & 0xFF : -1;
            }
        }

        /**
         * Reads at most {@code len} bytes from this stream and stores
         * them in byte array {@code b} starting at {@code offset}. This
         * implementation reads bytes from the source byte array.
         * 
         * @param b
         *            the byte array in which to store the bytes read.
         * @param offset
         *            the initial position in {@code b} to store the bytes read from
         *            this stream.
         * @param length
         *            the maximum number of bytes to store in {@code b}.
         * @return the number of bytes actually read or -1 if no bytes were read and
         *         the end of the stream was encountered.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code length < 0}, or if
         *             {@code offset + length} is greater than the size of
         *             {@code b}.
         * @throws NullPointerException
         *             if {@code b} is {@code null}.
         */
        
        public override int read(byte[] b, int offset, int length) {
            lock (this) {
                if (b == null) {
                    throw new java.lang.NullPointerException();
                }
                // avoid int overflow
                if (offset < 0 || offset > b.Length || length < 0
                        || length > b.Length - offset) {
                    throw new java.lang.IndexOutOfBoundsException();
                }
                // Are there any bytes available?
                if (this.pos >= this.countJ) {
                    return -1;
                }
                if (length == 0) {
                    return 0;
                }

                int copylen = this.countJ - pos < length ? this.countJ - pos : length;
                java.lang.SystemJ.arraycopy(buf, pos, b, offset, copylen);
                pos += copylen;
                return copylen;
            }
        }

        /**
         * Resets this stream to the last marked location. This implementation
         * resets the position to either the marked position, the start position
         * supplied in the constructor or 0 if neither has been provided.
         *
         * @see #mark(int)
         */
        
        public override void reset() {
            lock (this) {
                pos = markJ;
            }
        }

        /**
         * Skips {@code count} number of bytes in this InputStream. Subsequent
         * {@code read()}s will not return these bytes unless {@code reset()} is
         * used. This implementation skips {@code count} number of bytes in the
         * target stream. It does nothing and returns 0 if {@code n} is negative.
         * 
         * @param n
         *            the number of bytes to skip.
         * @return the number of bytes actually skipped.
         */
        
        public override long skip(long n) {
            lock (this) {
                if (n <= 0) {
                    return 0;
                }
                int temp = pos;
                pos = this.countJ - pos < n ? this.countJ : (int) (pos + n);
                return pos - temp;
            }
        }
    }
}
