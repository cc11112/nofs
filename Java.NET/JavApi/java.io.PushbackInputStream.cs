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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    // First class code fully copied from hand, and change to C# syntax by hand - so extending framework to work: NullPointerException, SystemJ.arraycopy, ArrayStoreException (Time: ~ 10 min)
    ///<summary>
    /// Wraps an existing {@link InputStream} and adds functionality to "push back"
    /// bytes that have been read, so that they can be read again. Parsers may find
    /// this useful. The number of bytes which may be pushed back can be specified
    /// during construction. If the buffer of pushed back bytes is empty, bytes are
    /// read from the underlying input stream.
    /// </summary>
    /// <remarks>Class is ported from Apache Harmony project.</remarks>
    public class PushbackInputStream : FilterInputStream
    {
        /**
         * The buffer that contains pushed-back bytes.
         */
        protected byte[] buf;

        /**
         * The current position within {@code buf}. A value equal to
         * {@code buf.length} indicates that no bytes are available. A value of 0
         * indicates that the buffer is full.
         */
        protected int pos;

        /**
         * Constructs a new {@code PushbackInputStream} with the specified input
         * stream as source. The size of the pushback buffer is set to the default
         * value of 1 byte.
         * 
         * @param in
         *            the source input stream.
         */
        public PushbackInputStream(InputStream inJ) : base (inJ){
            buf = (inJ == null) ? null : new byte[1];
            pos = 1;
        }

        /**
         * Constructs a new {@code PushbackInputStream} with {@code in} as source
         * input stream. The size of the pushback buffer is set to {@code size}.
         * 
         * @param in
         *            the source input stream.
         * @param size
         *            the size of the pushback buffer.
         * @throws IllegalArgumentException
         *             if {@code size} is negative.
         */
        public PushbackInputStream(InputStream inJ, int size) : base (inJ) {
            if (size <= 0) {
                throw new java.lang.IllegalArgumentException("size : "+size); //Messages.getString("luni.A3") $NON-NLS-1$
            }
            buf = (inJ == null) ? null : new byte[size];
            pos = size;
        }

        /**
         * Returns the number of bytes that are available before this stream will
         * block. This is the sum of the bytes available in the pushback buffer and
         * those available from the source stream.
         *
         * @return the number of bytes available before blocking.
         * @throws IOException
         *             if this stream is closed or an I/O error occurs in the source
         *             stream.
         */
        public override int available() {//throws IOException {
            if (buf == null) {
                throw new IOException();
            }
            return buf.Length - pos + inJ.available();
        }

        /**
         * Closes this stream. This implementation closes the source stream
         * and releases the pushback buffer.
         *
         * @throws IOException
         *             if an error occurs while closing this stream.
         */

        public override void close(){// throws IOException {
            if (inJ != null) {
                inJ.close();
                inJ = null;
                buf = null;
            }
        }

        /**
         * Indicates whether this stream supports the {@code mark(int)} and
         * {@code reset()} methods. {@code PushbackInputStream} does not support
         * them, so it returns {@code false}.
         * 
         * @return always {@code false}.
         * @see #mark(int)
         * @see #reset()
         */
        public override bool markSupported() {
            return false;
        }

        /**
         * Reads a single byte from this stream and returns it as an integer in the
         * range from 0 to 255. If the pushback buffer does not contain any
         * available bytes then a byte from the source input stream is returned.
         * Blocks until one byte has been read, the end of the source stream is
         * detected or an exception is thrown.
         *
         * @return the byte read or -1 if the end of the source stream has been
         *         reached.
         * @throws IOException
         *             if this stream is closed or an I/O error occurs while reading
         *             from this stream.
         */
        public override int read(){// throws IOException {
            if (buf == null) {
                throw new IOException();
            }
            // Is there a pushback byte available?
            if (pos < buf.Length) {
                return (buf[pos++] & 0xFF);
            }
            // Assume read() in the InputStream will return low-order byte or -1
            // if end of stream.
            return inJ.read();
        }

        /**
         * Reads at most {@code length} bytes from this stream and stores them in
         * the byte array {@code buffer} starting at {@code offset}. Bytes are read
         * from the pushback buffer first, then from the source stream if more bytes
         * are required. Blocks until {@code count} bytes have been read, the end of
         * the source stream is detected or an exception is thrown.
         * 
         * @param buffer
         *            the array in which to store the bytes read from this stream.
         * @param offset
         *            the initial position in {@code buffer} to store the bytes read
         *            from this stream.
         * @param length
         *            the maximum number of bytes to store in {@code buffer}.
         * @return the number of bytes read or -1 if the end of the source stream
         *         has been reached.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code length < 0}, or if
         *             {@code offset + length} is greater than the length of
         *             {@code buffer}.
         * @throws IOException
         *             if this stream is closed or another I/O error occurs while
         *             reading from this stream.
         * @throws NullPointerException
         *             if {@code buffer} is {@code null}.
         */
        public override int read(byte[] buffer, int offset, int length){// throws IOException {
            if (buf == null) {
                // luni.24=Stream is closed
                throw new IOException("Stream is closed"); //$NON-NLS-1$
            }
            // Force buffer null check first!
            if (offset > buffer.Length || offset < 0) {
                // luni.12=Offset out of bounds \: {0}
                throw new java.lang.ArrayIndexOutOfBoundsException("Offset out of bounds :"+offset); //$NON-NLS-1$
            }
            if (length < 0 || length > buffer.Length - offset) {
                // luni.18=Length out of bounds \: {0}
                throw new java.lang.ArrayIndexOutOfBoundsException("Length out of bounds :"+ length); //$NON-NLS-1$
            }

            int copiedBytes = 0, copyLength = 0, newOffset = offset;
            // Are there pushback bytes available?
            if (pos < buf.Length) {
                copyLength = (buf.Length - pos >= length) ? length : buf.Length - pos;
                java.lang.SystemJ.arraycopy(buf, pos, buffer, newOffset, copyLength);
                newOffset += copyLength;
                copiedBytes += copyLength;
                // Use up the bytes in the local buffer
                pos += copyLength;
            }
            // Have we copied enough?
            if (copyLength == length) {
                return length;
            }
            int inCopied = inJ.read(buffer, newOffset, length - copiedBytes);
            if (inCopied > 0) {
                return inCopied + copiedBytes;
            }
            if (copiedBytes == 0) {
                return inCopied;
            }
            return copiedBytes;
        }

        /**
         * Skips {@code count} bytes in this stream. This implementation skips bytes
         * in the pushback buffer first and then in the source stream if necessary.
         * 
         * @param count
         *            the number of bytes to skip.
         * @return the number of bytes actually skipped.
         * @throws IOException
         *             if this stream is closed or another I/O error occurs.
         */
        public override long skip(long count) {//throws IOException {
            if (inJ == null) {
                throw new IOException("Stream is closed"); //Messages.getString("luni.24") $NON-NLS-1$
            }
            if (count <= 0) {
                return 0;
            }
            long numSkipped = 0;
            if (pos < buf.Length) {
                numSkipped += (count < buf.Length - pos) ? count : buf.Length - pos;
                pos += (int) numSkipped;
            }
            if (numSkipped < count) {
                numSkipped += inJ.skip(count - numSkipped);
            }
            return numSkipped;
        }

        /**
         * Pushes all the bytes in {@code buffer} back to this stream. The bytes are
         * pushed back in such a way that the next byte read from this stream is
         * buffer[0], then buffer[1] and so on.
         * <p>
         * If this stream's internal pushback buffer cannot store the entire
         * contents of {@code buffer}, an {@code IOException} is thrown. Parts of
         * {@code buffer} may have already been copied to the pushback buffer when
         * the exception is thrown.
         *
         * @param buffer
         *            the buffer containing the bytes to push back to this stream.
         * @throws IOException
         *             if the free space in the internal pushback buffer is not
         *             sufficient to store the contents of {@code buffer}.
         */
        public virtual void unread(byte[] buffer) {//throws IOException {
            unread(buffer, 0, buffer.Length);
        }

        /**
         * Pushes a subset of the bytes in {@code buffer} back to this stream. The
         * subset is defined by the start position {@code offset} within
         * {@code buffer} and the number of bytes specified by {@code length}. The
         * bytes are pushed back in such a way that the next byte read from this
         * stream is {@code buffer[offset]}, then {@code buffer[1]} and so on.
         * <p>
         * If this stream's internal pushback buffer cannot store the selected
         * subset of {@code buffer}, an {@code IOException} is thrown. Parts of
         * {@code buffer} may have already been copied to the pushback buffer when
         * the exception is thrown.
         * 
         * @param buffer
         *            the buffer containing the bytes to push back to this stream.
         * @param offset
         *            the index of the first byte in {@code buffer} to push back.
         * @param length
         *            the number of bytes to push back.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code length < 0}, or if
         *             {@code offset + length} is greater than the length of
         *             {@code buffer}.
         * @throws IOException
         *             if the free space in the internal pushback buffer is not
         *             sufficient to store the selected contents of {@code buffer}.
         */
        public virtual void unread(byte[] buffer, int offset, int length){
                //throws IOException {
            if (length > pos) {
                // luni.D3=Pushback buffer full
                throw new IOException("Pushback buffer full"); //$NON-NLS-1$
            }
            if (offset > buffer.Length || offset < 0) {
                // luni.12=Offset out of bounds \: {0}
                throw new java.lang.ArrayIndexOutOfBoundsException("Offset out of bounds : "+offset); //$NON-NLS-1$
            }
            if (length < 0 || length > buffer.Length - offset) {
                // luni.18=Length out of bounds \: {0}
                throw new java.lang.ArrayIndexOutOfBoundsException("Length out of bounds :"+ length); //$NON-NLS-1$
            }
            if (buf == null) {
                // luni.24=Stream is closed
                throw new IOException("Stream is closed"); //$NON-NLS-1$
            }

            java.lang.SystemJ.arraycopy(buffer, offset, buf, pos - length, length);
            pos = pos - length;
        }

        /**
         * Pushes the specified byte {@code oneByte} back to this stream. Only the
         * least significant byte of the integer {@code oneByte} is pushed back.
         * This is done in such a way that the next byte read from this stream is
         * {@code (byte) oneByte}.
         * <p>
         * If this stream's internal pushback buffer cannot store the byte, an
         * {@code IOException} is thrown.
         *
         * @param oneByte
         *            the byte to push back to this stream.
         * @throws IOException
         *             if this stream is closed or the internal pushback buffer is
         *             full.
         */
        public virtual void unread(int oneByte){// throws IOException {
            if (buf == null) {
                throw new IOException();
            }
            if (pos == 0) {
                throw new IOException(); //$NON-NLS-1$
            }
            buf[--pos] = (byte) oneByte;
        }

        /**
         * Marks the current position in this stream. Setting a mark is not
         * supported in this class; this implementation does nothing.
         * 
         * @param readlimit
         *            the number of bytes that can be read from this stream before
         *            the mark is invalidated; this parameter is ignored.
         */
        public override void mark(int readlimit) {
            return;
        }

        /**
         * Resets this stream to the last marked position. Resetting the stream is
         * not supported in this class; this implementation always throws an
         * {@code IOException}.
         * 
         * @throws IOException
         *             if this method is called.
         */
        public override void reset() {//throws IOException {
            throw new java.io.IOException();
        }
    }
}
