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

    /**
     * CharArrayBuffer, ReadWriteCharArrayBuffer and ReadOnlyCharArrayBuffer compose
     * the implementation of array based char buffers.
     * <p>
     * ReadWriteCharArrayBuffer extends CharArrayBuffer with all the write methods.
     * </p>
     * <p>
     * This class is marked final for runtime performance.
     * </p>
     * 
     */
    sealed class ReadWriteCharArrayBuffer : CharArrayBuffer {

        static ReadWriteCharArrayBuffer copy(CharArrayBuffer other, int markOfOther) {
            ReadWriteCharArrayBuffer buf = new ReadWriteCharArrayBuffer(other
                    .capacity(), other.backingArray, other.offset);
            buf.limitJ = other.limit();
            buf.positionJ = other.position();
            buf.markJ = markOfOther;
            return buf;
        }

        internal ReadWriteCharArrayBuffer(char[] array) : base (array){
        }

        internal ReadWriteCharArrayBuffer(int capacity) : base (capacity){
        }

        internal ReadWriteCharArrayBuffer(int capacity, char[] backingArray, int arrayOffset) : base (capacity,backingArray,arrayOffset){
        }

        public override CharBuffer asReadOnlyBuffer() {
            return ReadOnlyCharArrayBuffer.copy(this, markJ);
        }

        public override CharBuffer compact() {
            java.lang.SystemJ.arraycopy(backingArray, positionJ + offset, backingArray, offset,
                    remaining());
            positionJ = limitJ - positionJ;
            limitJ = capacityJ;
            markJ = UNSET_MARK;
            return this;
        }

        public override CharBuffer duplicate() {
            return copy(this, markJ);
        }

        public override bool isReadOnly() {
            return false;
        }

        public override char[] protectedArray() {
            return backingArray;
        }

        public override int protectedArrayOffset() {
            return offset;
        }

        public override bool protectedHasArray() {
            return true;
        }

        public override CharBuffer put(char c) {
            if (positionJ == limitJ) {
                throw new BufferOverflowException();
            }
            backingArray[offset + positionJ++] = c;
            return this;
        }

        public override CharBuffer put(int index, char c) {
            if (index < 0 || index >= limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            backingArray[offset + index] = c;
            return this;
        }

        public override CharBuffer put(char[] src, int off, int len) {
            int length = src.Length;
            if (off < 0 || len < 0 || (long) len + (long) off > length) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            if (len > remaining()) {
                throw new BufferOverflowException();
            }
            java.lang.SystemJ.arraycopy(src, off, backingArray, offset + positionJ, len);
            positionJ += len;
            return this;
        }

        public override CharBuffer slice() {
            return new ReadWriteCharArrayBuffer(remaining(), backingArray, offset
                    + positionJ);
        }

    }
}
