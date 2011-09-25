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
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    /**
     * A specialized {@link OutputStream} for class for writing content to an
     * (internal) byte array. As bytes are written to this stream, the byte array
     * may be expanded to hold more bytes. When the writing is considered to be
     * finished, a copy of the byte array can be requested from the class.
     * 
     * @see ByteArrayInputStream
     */
    public class ByteArrayOutputStream : OutputStream {
        /**
         * The byte array containing the bytes written.
         */
        protected byte[] buf;

        /**
         * The number of bytes written.
         */
        protected int count;

        /**
         * Constructs a new ByteArrayOutputStream with a default size of 32 bytes.
         * If more than 32 bytes are written to this instance, the underlying byte
         * array will expand.
         */
        public ByteArrayOutputStream() :base (){
            buf = new byte[32];
        }

        /**
         * Constructs a new {@code ByteArrayOutputStream} with a default size of
         * {@code size} bytes. If more than {@code size} bytes are written to this
         * instance, the underlying byte array will expand.
         * 
         * @param size
         *            initial size for the underlying byte array, must be
         *            non-negative.
         * @throws IllegalArgumentException
         *             if {@code size} < 0.
         */
        public ByteArrayOutputStream(int size) :base() {
            if (size >= 0) {
                buf = new byte[size];
            } else {
                throw new java.lang.IllegalArgumentException("size must be >= 0"); //$NON-NLS-1$
            }
        }

        /**
         * Closes this stream. This releases system resources used for this stream.
         * 
         * @throws IOException
         *             if an error occurs while attempting to close this stream.
         */
        
        public override void close(){// throws IOException {
            /**
             * Although the spec claims "A closed stream cannot perform output
             * operations and cannot be reopened.", this implementation must do
             * nothing.
             */
            base.close();
        }

        private void expand(int i) {
            /* Can the buffer handle @i more bytes, if not expand it */
            if (count + i <= buf.Length) {
                return;
            }

            byte[] newbuf = new byte[(count + i) * 2];
            java.lang.SystemJ.arraycopy(buf, 0, newbuf, 0, count);
            buf = newbuf;
        }

        /**
         * Resets this stream to the beginning of the underlying byte array. All
         * subsequent writes will overwrite any bytes previously stored in this
         * stream.
         */
        public virtual void reset() {
            lock (this){
                count = 0;
            }
        }

        /**
         * Returns the total number of bytes written to this stream so far.
         * 
         * @return the number of bytes written to this stream.
         */
        public virtual int size() {
            return count;
        }

        /**
         * Returns the contents of this ByteArrayOutputStream as a byte array. Any
         * changes made to the receiver after returning will not be reflected in the
         * byte array returned to the caller.
         * 
         * @return this stream's current contents as a byte array.
         */
        public virtual byte[] toByteArray() {
            lock (this) {
                byte[] newArray = new byte[count];
                java.lang.SystemJ.arraycopy(buf, 0, newArray, 0, count);
                return newArray;
            }
        }

        /**
         * Returns the contents of this ByteArrayOutputStream as a string. Any
         * changes made to the receiver after returning will not be reflected in the
         * string returned to the caller.
         * 
         * @return this stream's current contents as a string.
         */

        
        public override String ToString() {
            return new java.lang.StringJ(buf, 0, count).ToString();
        }

        /**
         * Returns the contents of this ByteArrayOutputStream as a string. Each byte
         * {@code b} in this stream is converted to a character {@code c} using the
         * following function:
         * {@code c == (char)(((hibyte & 0xff) << 8) | (b & 0xff))}. This method is
         * deprecated and either {@link #toString()} or {@link #toString(String)}
         * should be used.
         * 
         * @param hibyte
         *            the high byte of each resulting Unicode character.
         * @return this stream's current contents as a string with the high byte set
         *         to {@code hibyte}.
         * @deprecated Use {@link #toString()}.
         */
        [Obsolete]
        public String toString(int hibyte) {
            char[] newBuf = new char[size()];
            for (int i = 0; i < newBuf.Length; i++) {
                newBuf[i] = (char) (((hibyte & 0xff) << 8) | (buf[i] & 0xff));
            }
            return new String(newBuf);
        }

        /**
         * Returns the contents of this ByteArrayOutputStream as a string converted
         * according to the encoding declared in {@code enc}.
         * 
         * @param enc
         *            a string representing the encoding to use when translating
         *            this stream to a string.
         * @return this stream's current contents as an encoded string.
         * @throws UnsupportedEncodingException
         *             if the provided encoding is not supported.
         */
        public String toString(String enc) {//throws UnsupportedEncodingException {
            return new java.lang.StringJ(buf, 0, count, enc).ToString();
        }

        /**
         * Writes {@code count} bytes from the byte array {@code buffer} starting at
         * offset {@code index} to this stream.
         * 
         * @param buffer
         *            the buffer to be written.
         * @param offset
         *            the initial position in {@code buffer} to retrieve bytes.
         * @param len
         *            the number of bytes of {@code buffer} to write.
         * @throws NullPointerException
         *             if {@code buffer} is {@code null}.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code len < 0}, or if
         *             {@code offset + len} is greater than the length of
         *             {@code buffer}.
         */
        
        public override void write(byte[] buffer, int offset, int len) {
            lock (this) {
                // avoid int overflow
                if (offset < 0 || offset > buffer.Length || len < 0
                        || len > buffer.Length - offset) {
                    throw new java.lang.IndexOutOfBoundsException("Arguments out of bounds"); //$NON-NLS-1$
                }
                if (len == 0) {
                    return;
                }

                /* Expand if necessary */
                expand(len);
                java.lang.SystemJ.arraycopy(buffer, offset, buf, this.count, len);
                this.count += len;
            }
        }

        /**
         * Writes the specified byte {@code oneByte} to the OutputStream. Only the
         * low order byte of {@code oneByte} is written.
         * 
         * @param oneByte
         *            the byte to be written.
         */
        
        public override void write(int oneByte) {
            lock (this) {
                if (count == buf.Length) {
                    expand(1);
                }
                buf[count++] = (byte) oneByte;
            }
        }

        /**
         * Takes the contents of this stream and writes it to the output stream
         * {@code out}.
         * 
         * @param out
         *            an OutputStream on which to write the contents of this stream.
         * @throws IOException
         *             if an error occurs while writing to {@code out}.
         */
        public void writeTo(OutputStream outJ) {//throws IOException {
            outJ.write(buf, 0, count);
        }
    }
}
