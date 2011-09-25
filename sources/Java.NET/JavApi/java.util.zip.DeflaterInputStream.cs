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

namespace biz.ritter.javapi.util.zip
{

    /**
     * An inputstream filter to compress data by the compressing format of Deflate.
     */
    public class DeflaterInputStream : java.io.FilterInputStream {

        /**
         * The Deflater used by this DeflaterInputStream
         */
        protected readonly Deflater def;

        /**
         * The internal input data buffer used by this DeflaterInputStream.
         */
        protected readonly byte[] buf;

        private static readonly int defaultsize = 1024;

        private const int EOF = -1;

        private bool closed = false;

        private bool availableJ = true;

        /**
         * Constructs a DeflaterInputStream with the default Deflater and internal
         * input buffer length.
         * 
         * @param in
         *            the InputStream that the DeflaterInputStream reads data from.
         */
        public DeflaterInputStream(java.io.InputStream inJ) :this(inJ, new Deflater(), defaultsize){
        }

        /**
         * Constructs a DeflaterInputStream with a specified Deflater and the
         * default internal input buffer length.
         * 
         * @param in
         *            the InputStream that the DeflaterInputStream reads data from.
         * @param defl
         *            an specifed Deflater used to compress data.
         */
        public DeflaterInputStream(java.io.InputStream inJ, Deflater defl) :this(inJ, defl, defaultsize){
        }

        /**
         * Constructs a DeflaterInputStream with a specified Deflater and input
         * buffer length.
         * 
         * @param in
         *            the InputStream that the DeflaterInputStream reads data from.
         * @param defl
         *            a specifed Deflater used to compress data.
         * @param bufLen
         *            the buffer length of the internal input data buffer.
         */
        public DeflaterInputStream(java.io.InputStream inJ, Deflater defl, int bufLen) :base (inJ){
            if (null == inJ || null == defl) {
                throw new java.lang.NullPointerException();
            }
            if (bufLen <= 0) {
                throw new java.lang.IllegalArgumentException();
            }
            def = defl;
            buf = new byte[bufLen];
        }

        /**
         * Closes the underlying input stream and discards any remaining uncompressed
         * data.
         */
        public override void close(){// throws IOException {
            closed = true;
            def.end();
            inJ.close();
        }

        /**
         * Reads a byte from the compressed input stream.
         * 
         * @return the byte or -1 if the end of the compressed input stream has been
         *         reached.
         */
        
        public override int read() {//throws IOException {
            byte[] result = new byte[1];

            // EOF
            if (read(result, 0, 1) == EOF) {
                return EOF;
            }

            int r = result[0];
            if (r < 0) {
                r += 256;
            }
            return r;
        }

        /**
         * Reads compressed data into a byte buffer.
         * 
         * @param b
         *            the byte buffer that compressed data will be read into.
         * @param off
         *            the offset in the byte buffer where compressed data will start
         *            to be read into.
         * @param len
         *            the length of the compressed data that is expected to read.
         * @return the number of bytes read or -1 if the end of the compressed input
         *         stream has been reached.
         */
        
        public override int read(byte[] b, int off, int len) {//throws IOException {
            checkClosed();
            if (null == b) {
                throw new java.lang.NullPointerException();
            }
            if (off < 0 || len < 0 || len > b.Length - off) {
                throw new java.lang.IndexOutOfBoundsException();
            }

            if (!availableJ) {
                return EOF;
            }

            int count = 0;

            while (count < len && !def.finished()) {
                if (def.needsInput()) {
                    // read data from input stream
                    int readed = inJ.read(buf);
                    if (EOF == readed) {
                        // gets to the end of the input stream
                        def.finish();
                    } else {
                        def.setInput(buf, 0, readed);
                    }
                }
                // gets compressed data from def
                int readedJ = def.deflate(buf, 0, java.lang.Math.min(buf.Length, len - count));
                if (EOF == readedJ) {
                    break;
                }
                java.lang.SystemJ.arraycopy(buf, 0, b, off + count, readedJ);
                count += readedJ;
            }
            if (0 == count) {
                count = EOF;
                availableJ = false;
            }
            return count;
        }

        /**
         * Skips n bytes from the DeflateInputStream.
         * 
         * @param n
         *            the bytes to skipped. If n is greater than Integer.MAX_VALUE,
         *            the DeflateInputStream tries to skip Integer.MAX_VALUE bytes.
         * @return the number of bytes actually skipped.
         */
        
        public override long skip(long n){// throws IOException {
            if (n < 0) {
                throw new java.lang.IllegalArgumentException();
            }
            checkClosed();

            int length = (int) (n > java.lang.Integer.MAX_VALUE ? java.lang.Integer.MAX_VALUE : n);
            byte[] buffer = new byte[defaultsize > length ? length : defaultsize];
            int skipped = 0;
            int count = 0;
            while (skipped < length
                    && (count = read(buffer, 0,
                            (length - skipped > buffer.Length) ? buffer.Length
                                    : length - skipped)) >= 0) {
                skipped += count;
            }
            return skipped;
        }

        /**
         * Returns 1 to denote there is data to read while 0 if no data is
         * available. The returned value does not denote how many bytes that can be
         * read.
         * 
         * @return 1 to denote there is data to read while 0 if no data is
         *         available.
         */
        
        public override int available() {//throws IOException {
            checkClosed();
            if (availableJ) {
                return 1;
            }
            return 0;
        }

        /**
         * Denotes whether this inputstream support mark()/reset() operation. Always
         * returns false since the two operaions are not supported in
         * DeflaterInputStream.
         * 
         * @return always returns false.
         */
        
        public override bool markSupported() {
            return false;
        }

        /**
         * Not supported in DeflaterInputStream and just does nothing.
         * 
         * @param limit
         *            maximum number of bytes that can be read before the mark
         *            becomes invalid.
         */
        
        public override void mark(int limit) {
            // do nothing
        }

        /**
         * Not supported in DeflaterInputStream and just throws IOException.
         */
        
        public override void reset(){// throws IOException {
            throw new java.io.IOException();
        }

        private void checkClosed() {//throws IOException {
            if (closed) {
                throw new java.io.IOException();
            }
        }
    }
}
