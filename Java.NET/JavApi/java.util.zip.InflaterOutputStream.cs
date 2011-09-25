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
     * An ouput stream filter to decompress data compressed in the format of
     * Deflater.
     */
    public class InflaterOutputStream : java.io.FilterOutputStream {

        /**
         * The inflater used by InflaterOutputStream to decompress data.
         */
        protected readonly Inflater inf;

        /**
         * The internal output buffer.
         */
        protected readonly byte[] buf;

        private bool closed = false;

        private static readonly int DEFAULT_BUFFER_SIZE = 1024;

        /**
         * Constructs an InflaterOutputStream with the default Inflater and internal
         * output buffer size.
         * 
         * @param out
         *            the output stream that InflaterOutputStream will write
         *            compressed data into.
         */
        public InflaterOutputStream(java.io.OutputStream outJ) :this(outJ, new Inflater()){
        }

        /**
         * Constructs an InflaterOutputStream with the specifed Inflater and the
         * default internal output buffer size.
         * 
         * @param out
         *            the output stream that InflaterOutputStream will write
         *            compressed data into.
         * @param infl
         *            the Inflater used by the InflaterOutputStream to decompress
         *            data.
         */
        public InflaterOutputStream(java.io.OutputStream outJ, Inflater infl) :this(outJ, infl, DEFAULT_BUFFER_SIZE){
        }

        /**
         * Constructs an InflaterOutputStream with the specifed Inflater and
         * internal output buffer size.
         * 
         * @param out
         *            the output stream that InflaterOutputStream will write
         *            compressed data into.
         * @param infl
         *            the Inflater used by the InflaterOutputStream to decompress
         *            data.
         * @param bufLen the size of the internal output buffer.
         */
        public InflaterOutputStream(java.io.OutputStream outJ, Inflater infl, int bufLen) :base(outJ){
            if (null == outJ || null == infl) {
                throw new java.lang.NullPointerException();
            }
            if (bufLen <= 0) {
                throw new java.lang.IllegalArgumentException();
            }
            inf = infl;
            buf = new byte[bufLen];
        }

        /**
         * Writes remaining data into the output stream and closes the underling
         * output stream data.
         */
        public override void close() {//throws IOException {
            if (!closed) {
                finish();
                inf.end();
                outJ.close();
                closed = true;
            }
        }

        /**
         * Flushes the output stream.
         */
        
        public override void flush(){// throws IOException {
            finish();
            outJ.flush();
        }

        /**
         * Finishes writing current uncompressed data into the InflaterOutputStream
         * but not closes it.
         * 
         * @throws IOException
         *             if the stream has been closed or some I/O error occurs.
         */
        public void finish() {// throws IOException {
            checkClosed();
            write();
        }

        /**
         * Writes a bit to the uncompressing output stream.
         * 
         * @param b
         *            the bit to write to the uncompressing output stream.
         * @throws IOException
         *             if the stream has been closed or some I/O error occurs.
         * @throws ZipException
         *             if a zip exception occurs.
         */
        
        public override void write(int b) {//throws IOException, ZipException {
            write(new byte[] { (byte) b }, 0, 1);
        }

        /**
         * Writes a bit to the uncompressing output stream.
         * 
         * @param b
         *            the byte array to write to the uncompressing output stream.
         * @param off
         *            the offset in the byte array where the data is first to be
         *            uncompressed.
         * @param len
         *            the number of the bytes to be uncompressed.
         * @throws IOException
         *             if the stream has been closed or some I/O error occurs.
         * @throws ZipException
         *             if a zip exception occurs.
         * @throws NullPointerException
         *             if the byte array is null.
         * @throws IndexOutOfBoundsException
         *             if the off less than zero or len less than zero or off + len
         *             is greater than the byte array length.
         */
        
        public override void write(byte[] b, int off, int len) //throws IOException,
        {//ZipException {
            checkClosed();
            if (null == b) {
                throw new java.lang.NullPointerException();
            }
            if (off < 0 || len < 0 || len > b.Length - off) {
                throw new java.lang.IndexOutOfBoundsException();
            }

            inf.setInput(b, off, len);
            write();
        }

        private void write() {//throws IOException, ZipException {
            int inflated = 0;
            try {
                while ((inflated = inf.inflate(buf)) > 0) {
                    outJ.write(buf, 0, inflated);
                }
            } catch (DataFormatException e) {
                throw new ZipException();
            }
        }

        private void checkClosed(){// throws IOException {
            if (closed) {
                throw new java.io.IOException();
            }
        }
    }
}
