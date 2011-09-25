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
     * The {@code CheckedOutputStream} class is used to maintain a running checksum
     * of all data written to a stream. The purpose of this checksum is to establish
     * data integrity, by publishing the checksum to other parties wanting to read
     * the non corrupted data.
     */
    public class CheckedOutputStream : java.io.FilterOutputStream {

        private readonly Checksum check;

        /**
         * Constructs a new {@code CheckedOutputStream} on {@code OutputStream}
         * {@code os}. The checksum is calculated using the algorithm implemented
         * by {@code csum}.
         *
         * @param os
         *            the output stream to calculate checksum for.
         * @param cs
         *            an entity implementing the checksum algorithm.
         */
        public CheckedOutputStream(java.io.OutputStream os, Checksum cs) :base (os) {
            check = cs;
        }

        /**
         * Returns the checksum calculated on the stream read so far.
         *
         * @return the updated checksum.
         */
        public Checksum getChecksum() {
            return check;
        }

        /**
         * Writes the specified byte to the underlying stream. The checksum is
         * updated with {@code val}.
         *
         * @param val
         *            the data value to written to the output stream.
         * @throws IOException
         *             if an IO error has occurred.
         */
        public override void write(int val) {//throws IOException {
            outJ.write(val);
            check.update(val);
        }

        /**
         * Writes n bytes of data from {@code buf} starting at offset {@code off} to
         * the underlying stream. The checksum is updated with the bytes written.
         *
         * @param buf
         *            data written to the output stream.
         * @param off
         *            the offset to start reading the data from {@code buf} written
         *            to the output stream.
         * @param nbytes
         *            number of bytes to write to the output stream.
         * @throws IOException
         *             if an IO error has occurred.
         */
        public override void write(byte[] buf, int off, int nbytes) {//throws IOException {
            outJ.write(buf, off, nbytes);
            check.update(buf, off, nbytes);
        }
    }
}
