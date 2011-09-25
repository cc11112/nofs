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
     * This class provides an implementation of {@code FilterOutputStream} that
     * compresses data using the <i>DEFLATE</i> algorithm. Basically it wraps the
     * {@code Deflater} class and takes care of the buffering.
     *
     * @see Deflater
     */
    public class DeflaterOutputStream : java.io.FilterOutputStream
    {
        internal const int BUF_SIZE = 512;

        /**
         * The buffer for the data to be written to.
         */
        protected byte[] buf;

        /**
         * The deflater used.
         */
        protected Deflater def;

        protected internal bool done = false;

        /**
         * This constructor lets you pass the {@code Deflater} specifying the
         * compression algorithm.
         *
         * @param os
         *            is the {@code OutputStream} where to write the compressed data
         *            to.
         * @param def
         *            is the specific {@code Deflater} that is used to compress
         *            data.
         */
        public DeflaterOutputStream(java.io.OutputStream os, Deflater def) :this(os, def, BUF_SIZE){
        }

        /**
         * This is the most basic constructor. You only need to pass the {@code
         * OutputStream} to which the compressed data shall be written to. The
         * default settings for the {@code Deflater} and internal buffer are used.
         * In particular the {@code Deflater} produces a ZLIB header in the output
         * stream.
         *
         * @param os
         *            is the OutputStream where to write the compressed data to.
         */
        public DeflaterOutputStream(java.io.OutputStream os) : this(os, new Deflater(), BUF_SIZE){
        }

        /**
         * This constructor lets you specify both the compression algorithm as well
         * as the internal buffer size to be used.
         *
         * @param os
         *            is the {@code OutputStream} where to write the compressed data
         *            to.
         * @param def
         *            is the specific {@code Deflater} that will be used to compress
         *            data.
         * @param bsize
         *            is the size to be used for the internal buffer.
         */
        public DeflaterOutputStream(java.io.OutputStream os, Deflater def, int bsize) : base(os) {
            if (os == null || def == null) {
                throw new java.lang.NullPointerException();
            }
            if (bsize <= 0) {
                throw new java.lang.IllegalArgumentException();
            }
            this.def = def;
            buf = new byte[bsize];
        }

        /**
         * Compress the data in the input buffer and write it to the underlying
         * stream.
         *
         * @throws IOException
         *             If an error occurs during deflation.
         */
        protected void deflate() {//throws IOException {
            int x = 0;
            do {
                x = def.deflate(buf);
                outJ.write(buf, 0, x);
            } while (!def.needsInput());
        }

        /**
         * Writes any unwritten compressed data to the underlying stream, the closes
         * all underlying streams. This stream can no longer be used after close()
         * has been called.
         *
         * @throws IOException
         *             If an error occurs while closing the data compression
         *             process.
         */
        
        public override void close(){// throws IOException {
            if (!def.finished()) {
                finish();
            }
            def.end();
            outJ.close();
        }

        /**
         * Writes any unwritten data to the underlying stream. Does not close the
         * stream.
         *
         * @throws IOException
         *             If an error occurs.
         */
        public virtual void finish(){// throws IOException {
            if (done) {
                return;
            }
            def.finish();
            int x = 0;
            while (!def.finished()) {
                if (def.needsInput()) {
                    def.setInput(buf, 0, 0);
                }
                x = def.deflate(buf);
                outJ.write(buf, 0, x);
            }
            done = true;
        }

        
        public override void write(int i) {//throws IOException {
            byte[] b = new byte[1];
            b[0] = (byte) i;
            write(b, 0, 1);
        }

        /**
         * Compresses {@code nbytes} of data from {@code buf} starting at
         * {@code off} and writes it to the underlying stream.
         *
         * @param buffer
         *            the buffer of data to compress.
         * @param off
         *            offset in buffer to extract data from.
         * @param nbytes
         *            the number of bytes of data to read from the buffer.
         * @throws IOException
         *             If an error occurs during writing.
         */
        
        public override void write(byte[] buffer, int off, int nbytes) {//throws IOException {
            if (done) {
                throw new java.io.IOException("attempt to write after finish"); //$NON-NLS-1$
            }
            // avoid int overflow, check null buf
            if (off <= buffer.Length && nbytes >= 0 && off >= 0
                    && buffer.Length - off >= nbytes) {
                if (!def.needsInput()) {
                    throw new java.io.IOException();
                }
                def.setInput(buffer, off, nbytes);
                deflate();
            } else {
                throw new java.lang.ArrayIndexOutOfBoundsException();
            }
        }
    }
}
