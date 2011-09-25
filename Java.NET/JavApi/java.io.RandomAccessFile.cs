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
using System.IO;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    public class RandomAccessFile : Closeable //,DataInput,DataOutput
    {
        protected FileStream delegateInstance;
        public RandomAccessFile(File f, String mode) : this (f.ToString(), mode){}
        public RandomAccessFile(String file, String mode) {
            /* mode:
             * r -> FileAccess.Read
             * rw -> FileAccess.ReadWrite
             */
            if (!mode.StartsWith("r")) throw new java.lang.IllegalArgumentException ();
            FileAccess fa = FileAccess.Read;
            if (mode.StartsWith("rw")) fa = FileAccess.ReadWrite;
            
            delegateInstance = new FileStream(file,fa == FileAccess.ReadWrite ? FileMode.OpenOrCreate : FileMode.Open,fa);
        }

        public virtual int read()
        {
            return this.delegateInstance.ReadByte();
        }
        public virtual void close()
        {
            this.delegateInstance.Close();
        }
        /**
         * Reads bytes from this file into {@code buffer}. Blocks until {@code
         * buffer.length} number of bytes have been read, the end of the file is
         * reached or an exception is thrown.
         * 
         * @param buffer
         *            the buffer to read bytes into.
         * @throws EOFException
         *             if the end of this file is detected.
         * @throws IOException
         *             if this file is closed or another I/O error occurs.
         * @throws NullPointerException
         *             if {@code buffer} is {@code null}.
         */
        public void readFully(byte[] buffer) {//throws IOException {
            readFully(buffer, 0, buffer.Length);
        }

        /**
         * Read bytes from this file into {@code buffer} starting at offset {@code
         * offset}. This method blocks until {@code count} number of bytes have been
         * read.
         * 
         * @param buffer
         *            the buffer to read bytes into.
         * @param offset
         *            the initial position in {@code buffer} to store the bytes read
         *            from this file.
         * @param count
         *            the maximum number of bytes to store in {@code buffer}.
         * @throws EOFException
         *             if the end of this file is detected.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code count < 0}, or if {@code
         *             offset + count} is greater than the length of {@code buffer}.
         * @throws IOException
         *             if this file is closed or another I/O error occurs.
         * @throws NullPointerException
         *             if {@code buffer} is {@code null}.
         */
        public void readFully(byte[] buffer, int offset, int count) {
                //throws IOException {
            if (buffer == null) {
                throw new java.lang.NullPointerException("buffer is null"); //$NON-NLS-1$
            }
            // avoid int overflow
            if (offset < 0 || offset > buffer.Length || count < 0
                    || count > buffer.Length - offset) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            while (count > 0) {
                int result = this.delegateInstance.Read(buffer, offset, count);
                if (result < 0) {
                    throw new EOFException();
                }
                offset += result;
                count -= result;
            }
        }

        /**
         * Returns the length of this file in bytes.
         * 
         * @return the file's length in bytes.
         * @throws IOException
         *             if this file is closed or some other I/O error occurs.
         */
        public long length() {//throws IOException {
            openCheck();
            return this.delegateInstance.Length;//fileSystem.size(fd.descriptor);
        }


        /**
         * Checks to see if the file is currently open. Returns silently if it is,
         * and throws an exception if it is not.
         * 
         * @throws IOException
         *             the receiver is closed.
         */
        private void openCheck() {//throws IOException {
            lock (this.delegateInstance) {
                if (this.delegateInstance.SafeFileHandle.IsClosed) { //if (fd.descriptor < 0) {
                    throw new IOException();
                }
            }
        }

        /**
         * Skips over {@code count} bytes in this file. Less than {@code count}
         * bytes are skipped if the end of the file is reached or an exception is
         * thrown during the operation. Nothing is done if {@code count} is
         * negative.
         * 
         * @param count
         *            the number of bytes to skip.
         * @return the number of bytes actually skipped.
         * @throws IOException
         *             if this file is closed or another I/O error occurs.
         */
        public int skipBytes(int count) {//throws IOException {
            if (count > 0) {
                long currentPos = this.delegateInstance.Position, eof = length();
                int newCount = (int) ((currentPos + count > eof) ? eof - currentPos
                        : count);
                seek(currentPos + newCount);
                return newCount;
            }
            return 0;
        }
        /**
         * Moves this file's file pointer to a new position, from where following
         * {@code read}, {@code write} or {@code skip} operations are done. The
         * position may be greater than the current length of the file, but the
         * file's length will only change if the moving of the pointer is followed
         * by a {@code write} operation.
         * 
         * @param pos
         *            the new file pointer position.
         * @throws IOException
         *             if this file is closed, {@code pos < 0} or another I/O error
         *             occurs.
         */
        public void seek(long pos) {//throws IOException {
            if (pos < 0) {
                // seek position is negative
                throw new IOException("seek position is negative"); //$NON-NLS-1$
            }
            openCheck();
            lock (this.delegateInstance) {
                this.delegateInstance.Seek(this.delegateInstance.Position, SeekOrigin.Begin);
                //fileSystem.seek(fd.descriptor, pos, IFileSystem.SEEK_SET);
            }
        }

        /**
         * Gets the current position within this file. All reads and
         * writes take place at the current file pointer position.
         * 
         * @return the current offset in bytes from the beginning of the file.
         * 
         * @throws IOException
         *             if an error occurs while getting the file pointer of this
         *             file.
         */
        public long getFilePointer() {//throws IOException {
            openCheck();
            return this.delegateInstance.Position;
            //return fileSystem.seek(fd.descriptor, 0L, IFileSystem.SEEK_CUR);
        }

        /**
         * Reads at most {@code count} bytes from the current position in this file
         * and stores them in the byte array {@code buffer} starting at {@code
         * offset}. Blocks until {@code count} bytes have been read, the end of the
         * file is reached or an exception is thrown.
         * 
         * @param buffer
         *            the array in which to store the bytes read from this file.
         * @param offset
         *            the initial position in {@code buffer} to store the bytes read
         *            from this file.
         * @param count
         *            the maximum number of bytes to store in {@code buffer}.
         * @return the number of bytes actually read or -1 if the end of the stream
         *         has been reached.
         * @throws IndexOutOfBoundsException
         *             if {@code offset < 0} or {@code count < 0}, or if {@code
         *             offset + count} is greater than the size of {@code buffer}.
         * @throws IOException
         *             if this file is closed or another I/O error occurs.
         */
        public int read(byte[] buffer, int offset, int count){// throws IOException {
            // have to have four comparisions to not miss integer overflow cases
            if (count > buffer.Length - offset || count < 0 || offset < 0) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            if (0 == count) {
                return 0;
            }
            openCheck();
            lock (this.delegateInstance) {
                return this.delegateInstance.Read (buffer, offset, count);
            }
        }
    }
}
