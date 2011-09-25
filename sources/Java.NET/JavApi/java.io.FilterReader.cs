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
     * Wraps an existing {@link Reader} and performs some transformation on the
     * input data while it is being read. Transformations can be anything from a
     * simple byte-wise filtering input data to an on-the-fly compression or
     * decompression of the underlying reader. Readers that wrap another reader and
     * provide some additional functionality on top of it usually inherit from this
     * class.
     * 
     * @see FilterWriter
     */
    public abstract class FilterReader : Reader {

        /**
         * The target Reader which is being filtered.
         */
        protected Reader inJ;

        /**
         * Constructs a new FilterReader on the Reader {@code in}.
         * 
         * @param in
         *            The non-null Reader to filter reads on.
         */
        protected FilterReader(Reader inJ) : base (inJ) {
            this.inJ = inJ;
        }

        /**
         * Closes this reader. This implementation closes the filtered reader.
         * 
         * @throws IOException
         *             if an error occurs while closing this reader.
         */
        public override void close() //throws IOException 
        {
            lock (lockJ) {
                inJ.close();
            }
        }

        /**
         * Sets a mark position in this reader. The parameter {@code readlimit}
         * indicates how many bytes can be read before the mark is invalidated.
         * Sending {@code reset()} will reposition this reader back to the marked
         * position, provided that {@code readlimit} has not been surpassed.
         * <p>
         * This implementation sets a mark in the filtered reader.
         * 
         * @param readlimit
         *            the number of bytes that can be read from this reader before
         *            the mark is invalidated.
         * @throws IOException
         *             if an error occurs while marking this reader.
         * @see #markSupported()
         * @see #reset()
         */
        public override void mark(int readlimit) //throws IOException 
        {
            lock (lockJ) {
                inJ.mark(readlimit);
            }
        }

        /**
         * Indicates whether this reader supports {@code mark()} and {@code reset()}.
         * This implementation returns whether the filtered reader supports marking.
         * 
         * @return {@code true} if {@code mark()} and {@code reset()} are supported
         *         by the filtered reader, {@code false} otherwise.
         * @see #mark(int)
         * @see #reset()
         * @see #skip(long)
         */
        public override bool markSupported() {
            lock (lockJ){
                return inJ.markSupported();
            }
        }

        /**
         * Reads a single character from the filtered reader and returns it as an
         * integer with the two higher-order bytes set to 0. Returns -1 if the end
         * of the filtered reader has been reached.
         * 
         * @return The character read or -1 if the end of the filtered reader has
         *         been reached.
         * @throws IOException
         *             if an error occurs while reading from this reader.
         */
        public override int read()// throws IOException 
        {
            lock (lockJ){
                return inJ.read();
            }
        }

        /**
         * Reads at most {@code count} characters from the filtered reader and stores them
         * in the byte array {@code buffer} starting at {@code offset}. Returns the
         * number of characters actually read or -1 if no characters were read and
         * the end of the filtered reader was encountered.
         * 
         * @param buffer
         *            the char array in which to store the characters read.
         * @param offset
         *            the initial position in {@code buffer} to store the characters
         *            read from this reader.
         * @param count
         *            the maximum number of characters to store in {@code buffer}.
         * @return the number of characters actually read or -1 if the end of the
         *         filtered reader has been reached while reading.
         * @throws IOException
         *             if an error occurs while reading from this reader.
         */
        public override int read(char[] buffer, int offset, int count) //throws IOException 
        {
            lock (lockJ){
                return inJ.read(buffer, offset, count);
            }
        }

        /**
         * Indicates whether this reader is ready to be read without blocking. If
         * the result is {@code true}, the next {@code read()} will not block. If
         * the result is {@code false}, this reader may or may not block when
         * {@code read()} is sent.
         * 
         * @return {@code true} if this reader will not block when {@code read()}
         *         is called, {@code false} if unknown or blocking will occur.
         * @throws IOException
         *             if the reader is closed or some other I/O error occurs.
         */
        public override bool ready() //throws IOException 
        {
            lock (lockJ){
                return inJ.ready();
            }
        }

        /**
         * Resets this reader's position to the last marked location. Invocations of
         * {@code read()} and {@code skip()} will occur from this new location. If
         * this reader was not marked, the behavior depends on the implementation of
         * {@code reset()} in the Reader subclass that is filtered by this reader.
         * The default behavior for Reader is to throw an {@code IOException}.
         * 
         * @throws IOException
         *             if a problem occurred or the filtered reader does not support
         *             {@code mark()} and {@code reset()}.
         * @see #mark(int)
         * @see #markSupported()
         */
        public override void reset() //throws IOException 
        {
            lock (lockJ){
                inJ.reset();
            }
        }

        /**
         * Skips {@code count} characters in this reader. Subsequent {@code read()}'s
         * will not return these characters unless {@code reset()} is used. The
         * default implementation is to skip characters in the filtered reader.
         * 
         * @param count
         *            the maximum number of characters to skip.
         * @return the number of characters actually skipped.
         * @throws IOException
         *             if the filtered reader is closed or some other I/O error
         *             occurs.
         * @see #mark(int)
         * @see #markSupported()
         * @see #reset()
         */
        public override long skip(long count) //throws IOException 
        {
            lock (lockJ){
                return inJ.skip(count);
            }
        }
    }

}
