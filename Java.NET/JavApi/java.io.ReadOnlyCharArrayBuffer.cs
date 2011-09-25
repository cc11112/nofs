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
     * ReadOnlyCharArrayBuffer extends CharArrayBuffer with all the write methods
     * throwing read only exception.
     * </p>
     * <p>
     * This class is marked final for runtime performance.
     * </p>
     * 
     */
    sealed class ReadOnlyCharArrayBuffer : CharArrayBuffer {

        internal static ReadOnlyCharArrayBuffer copy(CharArrayBuffer other, int markOfOther) {
            ReadOnlyCharArrayBuffer buf = new ReadOnlyCharArrayBuffer(other
                    .capacity(), other.backingArray, other.offset);
            buf.limitJ = other.limit();
            buf.positionJ = other.position();
            buf.markJ = markOfOther;
            return buf;
        }

        internal ReadOnlyCharArrayBuffer(int capacity, char[] backingArray, int arrayOffset) : base (capacity,backingArray,arrayOffset){
        }

        public override CharBuffer asReadOnlyBuffer() {
            return duplicate();
        }

        public override CharBuffer compact() {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer duplicate() {
            return copy(this, markJ);
        }

        public override bool isReadOnly() {
            return true;
        }

        public override char[] protectedArray() {
            throw new ReadOnlyBufferException();
        }

        public override int protectedArrayOffset() {
            throw new ReadOnlyBufferException();
        }

        public override bool protectedHasArray() {
            return false;
        }

        public override CharBuffer put(char c) {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer put(int index, char c) {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer put(char[] src, int off, int len) {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer put(CharBuffer src) {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer put(String src, int start, int end) {
            if ((start < 0) || (end < 0)
                    || (long) start + (long) end > src.length()) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer slice() {
            return new ReadOnlyCharArrayBuffer(remaining(), backingArray, offset
                    + positionJ);
        }
    }
}
