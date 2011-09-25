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
     * A class for turning a byte stream into a character stream. Data read from the
     * source input stream is converted into characters by either a default or a
     * provided character converter. The default encoding is taken from the
     * "file.encoding" system property. {@code InputStreamReader} contains a buffer
     * of bytes read from the source stream and converts these into characters as
     * needed. The buffer size is 8K.
     * 
     * @see OutputStreamWriter
     */
    public class InputStreamReader : Reader {
        private InputStream inJ;

        private static readonly int BUFFER_SIZE = 8192;

        private bool endOfInput = false;

        protected internal java.nio.charset.CharsetDecoder decoder;

        protected internal java.nio.ByteBuffer bytes = java.nio.ByteBuffer.allocate(BUFFER_SIZE);

        /**
         * Constructs a new {@code InputStreamReader} on the {@link InputStream}
         * {@code in}. This constructor sets the character converter to the encoding
         * specified in the "file.encoding" property and falls back to ISO 8859_1
         * (ISO-Latin-1) if the property doesn't exist.
         * 
         * @param in
         *            the input stream from which to read characters.
         */
        public InputStreamReader(InputStream inJ) : base (inJ){
            this.inJ = inJ;
            /*String encoding = AccessController
                    .doPrivileged(new PriviAction<String>(
                            "file.encoding", "ISO8859_1")); //$NON-NLS-1$//$NON-NLS-2$*/
            String encoding = java.lang.SystemJ.getProperty("file.encoding", "ISO8859_1"); 
            decoder = java.nio.charset.Charset.forName(encoding).newDecoder().onMalformedInput(
                    java.nio.charset.CodingErrorAction.REPLACE).onUnmappableCharacter(
                    java.nio.charset.CodingErrorAction.REPLACE);
            bytes.limit(0);
        }

        /**
         * Constructs a new InputStreamReader on the InputStream {@code in}. The
         * character converter that is used to decode bytes into characters is
         * identified by name by {@code enc}. If the encoding cannot be found, an
         * UnsupportedEncodingException error is thrown.
         * 
         * @param in
         *            the InputStream from which to read characters.
         * @param enc
         *            identifies the character converter to use.
         * @throws NullPointerException
         *             if {@code enc} is {@code null}.
         * @throws UnsupportedEncodingException
         *             if the encoding specified by {@code enc} cannot be found.
         */
        public InputStreamReader(InputStream inJ, String enc) : base (inJ){
                //throws UnsupportedEncodingException {
            if (enc == null) {
                throw new java.lang.NullPointerException();
            }
            this.inJ = inJ;
            try {
                decoder = java.nio.charset.Charset.forName(enc).newDecoder().onMalformedInput(
                        java.nio.charset.CodingErrorAction.REPLACE).onUnmappableCharacter(
                        java.nio.charset.CodingErrorAction.REPLACE);
            } catch (java.lang.IllegalArgumentException e) {
                throw (UnsupportedEncodingException)
                        new UnsupportedEncodingException(enc).initCause(e);
            }
            bytes.limit(0);
        }

        /**
         * Constructs a new InputStreamReader on the InputStream {@code in} and
         * CharsetDecoder {@code dec}.
         * 
         * @param in
         *            the source InputStream from which to read characters.
         * @param dec
         *            the CharsetDecoder used by the character conversion.
         */
        public InputStreamReader(InputStream inJ, java.nio.charset.CharsetDecoder dec) : base(inJ){
            dec.averageCharsPerByte();
            this.inJ = inJ;
            decoder = dec;
            bytes.limit(0);
        }

        /**
         * Constructs a new InputStreamReader on the InputStream {@code in} and
         * Charset {@code charset}.
         * 
         * @param in
         *            the source InputStream from which to read characters.
         * @param charset
         *            the Charset that defines the character converter
         */
        public InputStreamReader(InputStream inJ, java.nio.charset.Charset charset) : base(inJ) {
            this.inJ = inJ;
            decoder = charset.newDecoder().onMalformedInput(
                    java.nio.charset.CodingErrorAction.REPLACE).onUnmappableCharacter(
                    java.nio.charset.CodingErrorAction.REPLACE);
            bytes.limit(0);
        }

        /**
         * Closes this reader. This implementation closes the source InputStream and
         * releases all local storage.
         * 
         * @throws IOException
         *             if an error occurs attempting to close this reader.
         */
        
        public override void close(){// throws IOException {
            lock (this.lockJ) {
                decoder = null;
                if (inJ != null) {
                    inJ.close();
                    inJ = null;
                }
            }
        }

        /**
         * Returns the name of the encoding used to convert bytes into characters.
         * The value {@code null} is returned if this reader has been closed.
         * 
         * @return the name of the character converter or {@code null} if this
         *         reader is closed.
         */
        public virtual String getEncoding() {
            if (!isOpen()) {
                return null;
            }
            return decoder.charset().name();
        }

        /**
         * Reads a single character from this reader and returns it as an integer
         * with the two higher-order bytes set to 0. Returns -1 if the end of the
         * reader has been reached. The byte value is either obtained from
         * converting bytes in this reader's buffer or by first filling the buffer
         * from the source InputStream and then reading from the buffer.
         * 
         * @return the character read or -1 if the end of the reader has been
         *         reached.
         * @throws IOException
         *             if this reader is closed or some other I/O error occurs.
         */
        public override int read() {//throws IOException {
            lock (lockJ) {
                if (!isOpen()) {
                    // luni.BA=InputStreamReader is closed.
                    throw new IOException("InputStreamReader is closed."); //$NON-NLS-1$
                }

                char[] buf = new char[1];
                return read(buf, 0, 1) != -1 ? buf[0] : -1;
            }
        }

        /**
         * Reads at most {@code length} characters from this reader and stores them
         * at position {@code offset} in the character array {@code buf}. Returns
         * the number of characters actually read or -1 if the end of the reader has
         * been reached. The bytes are either obtained from converting bytes in this
         * reader's buffer or by first filling the buffer from the source
         * InputStream and then reading from the buffer.
         * 
         * @param buf
         *            the array to store the characters read.
         * @param offset
         *            the initial position in {@code buf} to store the characters
         *            read from this reader.
         * @param length
         *            the maximum number of characters to read.
         * @return the number of characters read or -1 if the end of the reader has
         *         been reached.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code length < 0}, or if
         *             {@code offset + length} is greater than the length of
         *             {@code buf}.
         * @throws IOException
         *             if this reader is closed or some other I/O error occurs.
         */
        public override int read(char[] buf, int offset, int length){// throws IOException {
            lock (lockJ) {
                if (!isOpen()) {
                    // luni.BA=InputStreamReader is closed.
                    throw new IOException("InputStreamReader is closed."); //$NON-NLS-1$
                }
                if (offset < 0 || offset > buf.Length - length || length < 0) {
                    throw new java.lang.IndexOutOfBoundsException();
                }
                if (length == 0) {
                    return 0;
                }

                java.nio.CharBuffer outJ = java.nio.CharBuffer.wrap(buf, offset, length);
                java.nio.charset.CoderResult result = java.nio.charset.CoderResult.UNDERFLOW;

                // bytes.remaining() indicates number of bytes in buffer
                // when 1-st time entered, it'll be equal to zero
                bool needInput = !bytes.hasRemaining();

                while (outJ.hasRemaining()) {
                    // fill the buffer if needed
                    if (needInput) {
                        try {
                            if ((inJ.available() == 0) 
                                && (outJ.position() > offset)) {
                                // we could return the result without blocking read
                                break;
                            }
                        } catch (IOException e) {
                            // available didn't work so just try the read
                        }

                        int to_read = bytes.capacity() - bytes.limit();
                        int off = bytes.arrayOffset() + bytes.limit();
                        int was_red = inJ.read((byte[])bytes.array(), off, to_read);

                        if (was_red == -1) {
                            endOfInput = true;
                            break;
                        } else if (was_red == 0) {
                            break;
                        }
                        bytes.limit(bytes.limit() + was_red);
                        needInput = false;
                    }

                    // decode bytes
                    result = decoder.decode(bytes, outJ, false);

                    if (result.isUnderflow()) {
                        // compact the buffer if no space left
                        if (bytes.limit() == bytes.capacity()) {
                            bytes.compact();
                            bytes.limit(bytes.position());
                            bytes.position(0);
                        }
                        needInput = true;
                    } else {
                        break;
                    }
                }

                if (result == java.nio.charset.CoderResult.UNDERFLOW && endOfInput) {
                    result = decoder.decode(bytes, outJ, true);
                    decoder.flush(outJ);
                    decoder.reset();
                }
                if (result.isMalformed()) {
                    throw new java.nio.charset.MalformedInputException(result.length());
                } else if (result.isUnmappable()) {
                    throw new java.nio.charset.UnmappableCharacterException(result.length());
                }

                return outJ.position() - offset == 0 ? -1 : outJ.position() - offset;
            }
        }

        /*
         * Answer a boolean indicating whether or not this InputStreamReader is
         * open.
         */
        private bool isOpen() {
            return inJ != null;
        }

        /**
         * Indicates whether this reader is ready to be read without blocking. If
         * the result is {@code true}, the next {@code read()} will not block. If
         * the result is {@code false} then this reader may or may not block when
         * {@code read()} is called. This implementation returns {@code true} if
         * there are bytes available in the buffer or the source stream has bytes
         * available.
         * 
         * @return {@code true} if the receiver will not block when {@code read()}
         *         is called, {@code false} if unknown or blocking will occur.
         * @throws IOException
         *             if this reader is closed or some other I/O error occurs.
         */
        
        public override bool ready(){// throws IOException {
            lock (lockJ) {
                if (inJ == null) {
                    // luni.BA=InputStreamReader is closed.
                    throw new IOException("InputStreamReader is closed."); //$NON-NLS-1$
                }
                try {
                    return bytes.hasRemaining() || inJ.available() > 0;
                } catch (IOException e) {
                    return false;
                }
            }
        }
    }
}
