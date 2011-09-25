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

namespace biz.ritter.javapi.nio
{

    /// <summary>
    /// HeapByteBuffer, ReadWriteHeapByteBuffer and ReadOnlyHeapByteBuffer compose
    /// the implementation of array based byte buffers.
    /// <p>
    /// HeapByteBuffer implements all the shared readonly methods and is extended by
    /// the other two classes.
    /// </p>
    /// <p>
    /// All methods are marked final for runtime performance.
    /// </p>
    /// 
    /// </summary>
    /// <remarks>Class is ported from Apache Harmony project.</remarks>
    abstract class HeapByteBuffer : ByteBuffer {

        protected internal readonly byte[] backingArray;

        protected internal readonly int offset;

        internal HeapByteBuffer(byte[] backingArray)
            : this(backingArray, backingArray.Length, 0)
        {
        }

        protected internal HeapByteBuffer(int capacity) :this (new byte[capacity], capacity, 0) {
        }

        internal HeapByteBuffer(byte[] backingArray, int capacity, int offset) :base (capacity) {
            this.backingArray = backingArray;
            this.offset = offset;
            if (offset + capacity > backingArray.Length) {
                throw new java.lang.IndexOutOfBoundsException();
            }
        }

        public override ByteBuffer get(byte[] dest, int off, int len) {
            int length = dest.Length;
            if (off < 0 || len < 0 || (long) off + (long) len > length) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            if (len > remaining()) {
                throw new BufferUnderflowException();
            }
            java.lang.SystemJ.arraycopy(backingArray, offset + positionJ, dest, off, len);
            positionJ += len;
            return this;
        }

        public override byte get() {
            if (positionJ == limitJ) {
                throw new BufferUnderflowException();
            }
            return backingArray[offset + positionJ++];
        }

        public override byte get(int index) {
            if (index < 0 || index >= limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            return backingArray[offset + index];
        }


        public override int getInt() {
            int newPosition = positionJ + 4;
            if (newPosition > limitJ) {
                throw new BufferUnderflowException();
            }
            int result = loadInt(positionJ);
            positionJ = newPosition;
            return result;
        }

        public override int getInt(int index) {
            if (index < 0 || index + 4 > limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            return loadInt(index);
        }

        public override long getLong() {
            int newPosition = positionJ + 8;
            if (newPosition > limitJ) {
                throw new BufferUnderflowException();
            }
            long result = loadLong(positionJ);
            positionJ = newPosition;
            return result;
        }

        public override long getLong(int index) {
            if (index < 0 || index + 8 > limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            return loadLong(index);
        }

        public override short getShort() {
            int newPosition = positionJ + 2;
            if (newPosition > limitJ) {
                throw new BufferUnderflowException();
            }
            short result = loadShort(positionJ);
            positionJ = newPosition;
            return result;
        }

        public override short getShort(int index) {
            if (index < 0 || index + 2 > limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            return loadShort(index);
        }

        public override bool isDirect() {
            return false;
        }

        protected int loadInt(int index) {
            int baseOffset = offset + index;
            int bytes = 0;
            if (order() == ByteOrder.BIG_ENDIAN) {
                for (int i = 0; i < 4; i++) {
                    bytes = bytes << 8;
                    bytes = bytes | (backingArray[baseOffset + i] & 0xFF);
                }
            } else {
                for (int i = 3; i >= 0; i--) {
                    bytes = bytes << 8;
                    bytes = bytes | (backingArray[baseOffset + i] & 0xFF);
                }
            }
            return bytes;
        }

        protected virtual long loadLong(int index) {
            int baseOffset = offset + index;
            long bytes = 0;
            if (orderJ == ByteOrder.BIG_ENDIAN)
            {
                for (int i = 0; i < 8; i++) {
                    bytes = bytes << 8;
                    bytes = bytes | (backingArray[baseOffset + i] & 0xFF);
                }
            } else {
                for (int i = 7; i >= 0; i--) {
                    bytes = bytes << 8;
                    bytes = bytes | (backingArray[baseOffset + i] & 0xFF);
                }
            }
            return bytes;
        }

        protected virtual short loadShort(int index) {
            int baseOffset = offset + index;
            short bytes = 0;
            if (orderJ == ByteOrder.BIG_ENDIAN)
            {
                bytes = (short) (backingArray[baseOffset] << 8);
                bytes |= (short) (backingArray[baseOffset + 1] & 0xFF);
            } else {
                bytes = (short) (backingArray[baseOffset + 1] << 8);
                bytes |= (short) (backingArray[baseOffset] & 0xFF);
            }
            return bytes;
        }

        protected virtual void store(int index, int value) {
            int baseOffset = offset + index;
            if (orderJ == ByteOrder.BIG_ENDIAN)
            {
                for (int i = 3; i >= 0; i--) {
                    backingArray[baseOffset + i] = (byte) (value & 0xFF);
                    value = value >> 8;
                }
            } else {
                for (int i = 0; i <= 3; i++) {
                    backingArray[baseOffset + i] = (byte) (value & 0xFF);
                    value = value >> 8;
                }
            }
        }

        protected virtual void store(int index, long value) {
            int baseOffset = offset + index;
            if (orderJ == ByteOrder.BIG_ENDIAN)
            {
                for (int i = 7; i >= 0; i--) {
                    backingArray[baseOffset + i] = (byte) (value & 0xFF);
                    value = value >> 8;
                }
            } else {
                for (int i = 0; i <= 7; i++) {
                    backingArray[baseOffset + i] = (byte) (value & 0xFF);
                    value = value >> 8;
                }
            }
        }

        protected virtual void store(int index, short value) {
            int baseOffset = offset + index;
            if (orderJ == ByteOrder.BIG_ENDIAN)
            {
                backingArray[baseOffset] = (byte) ((value >> 8) & 0xFF);
                backingArray[baseOffset + 1] = (byte) (value & 0xFF);
            } else {
                backingArray[baseOffset + 1] = (byte) ((value >> 8) & 0xFF);
                backingArray[baseOffset] = (byte) (value & 0xFF);
            }
        }

        public override char getChar() {
            return (char) getShort();
        }

        public override char getChar(int index) {
            return (char) getShort(index);
        }

        public override ByteBuffer putChar(char value) {
            return putShort((short) value);
        }

        public override ByteBuffer putChar(int index, char value) {
            return putShort(index, (short) value);
        }

        public override bool hasArray()
        {
            return base.hasArray();
        }

        public override ByteBuffer asReadOnlyBuffer()
        {
            throw new NotImplementedException();
        }

        public override ByteBuffer compact()
        {
            throw new NotImplementedException();
        }

        public override ByteBuffer duplicate()
        {
            throw new NotImplementedException();
        }

        protected override byte[] protectedArray()
        {
            throw new NotImplementedException();
        }

        protected override int protectedArrayOffset()
        {
            throw new NotImplementedException();
        }

        public override ByteBuffer put(byte b)
        {
            throw new NotImplementedException();
        }

        public override ByteBuffer put(int index, byte b)
        {
            throw new NotImplementedException();
        }

        public override ByteBuffer slice()
        {
            throw new NotImplementedException();
        }

        public override bool isReadOnly()
        {
            throw new NotImplementedException();
        }
    }
}
