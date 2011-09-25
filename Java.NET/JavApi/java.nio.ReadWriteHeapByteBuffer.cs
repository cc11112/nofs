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
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.nio
{

    /// <summary>
    /// HeapByteBuffer, ReadWriteHeapByteBuffer and ReadOnlyHeapByteBuffer compose
    /// the implementation of array based byte buffers.
    /// <p>
    /// ReadWriteHeapByteBuffer extends HeapByteBuffer with all the write methods.
    /// </p>
    /// <p>
    /// This class is marked final for runtime performance.
    /// </p>
    /// 
    /// </summary>
    /// <remarks>Class is ported from Apache Harmony project.</remarks>
    sealed class ReadWriteHeapByteBuffer : HeapByteBuffer
    {

        static ReadWriteHeapByteBuffer copy(HeapByteBuffer other, int markOfOther) {
            ReadWriteHeapByteBuffer buf = new ReadWriteHeapByteBuffer(
                    other.backingArray, other.capacity(), other.offset);
            buf.limitJ = other.limit();
            buf.positionJ = other.position();
            buf.markJ = markOfOther;
            buf.order(other.order());
            return buf;
        }

        internal ReadWriteHeapByteBuffer(byte[] backingArray) : base (backingArray){}

        internal ReadWriteHeapByteBuffer(int capacity) : base (capacity){}

        internal ReadWriteHeapByteBuffer(byte[] backingArray, int capacity, int arrayOffset) : base (backingArray,capacity,arrayOffset){}

        public override ByteBuffer asReadOnlyBuffer() {
            return ReadOnlyHeapByteBuffer.copy(this, markJ);
        }

        public override ByteBuffer compact() {
            java.lang.SystemJ.arraycopy(backingArray, positionJ + offset, backingArray, offset, remaining());
            positionJ = limitJ - positionJ;
            limitJ = capacityJ;
            markJ = UNSET_MARK;
            return this;
        }

        public override ByteBuffer duplicate() {
            return copy(this, markJ);
        }

        public override bool isReadOnly() {
            return false;
        }

        protected override byte[] protectedArray() {
            return backingArray;
        }

        protected override int protectedArrayOffset() {
            return offset;
        }

        protected override bool protectedHasArray() {
            return true;
        }

        public override ByteBuffer put(byte b) {
            if (positionJ == limitJ) {
                throw new BufferOverflowException();
            }
            backingArray[offset + positionJ++] = b;
            return this;
        }

        public override ByteBuffer put(int index, byte b) {
            if (index < 0 || index >= limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            backingArray[offset + index] = b;
            return this;
        }

        public override bool isDirect()
        {
            return base.isDirect();
        }
        public override bool hasArray()
        {
            return base.hasArray();
        }

    public override ByteBuffer putShort(int index, short value) {
        if (index < 0 || (long) index + 2 > limitJ) {
            throw new java.lang.IndexOutOfBoundsException();
        }
        store(index, value);
        return this;
    }

    public override ByteBuffer putShort(short value) {
        int newPosition = positionJ + 2;
        if (newPosition > limitJ) {
            throw new BufferOverflowException();
        }
        store(positionJ, value);
        positionJ = newPosition;
        return this;
    }

    }
}
