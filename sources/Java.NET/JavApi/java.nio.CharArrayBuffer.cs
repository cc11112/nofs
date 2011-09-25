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
     * CharArrayBuffer implements all the shared readonly methods and is extended by
     * the other two classes.
     * </p>
     * <p>
     * All methods are marked final for runtime performance.
     * </p>
     * 
     */
    abstract class CharArrayBuffer : CharBuffer {

        protected internal readonly char[] backingArray;

        protected internal readonly int offset;

        protected internal CharArrayBuffer(char[] array) :base (array.Length) {
            this.backingArray = array;
            this.offset = 0;
        }

        protected internal CharArrayBuffer(int capacity) : base (capacity){
            this.backingArray = new char[capacity];
            this.offset = 0;
        }

        protected internal CharArrayBuffer(int capacity, char[] backingArray, int offset) :base (capacity) {
            this.backingArray = backingArray;
            this.offset = offset;
        }

        public override char get() {
            if (positionJ == limitJ) {
                throw new BufferUnderflowException();
            }
            return backingArray[offset + positionJ++];
        }

        public override char get(int index) {
            if (index < 0 || index >= limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            return backingArray[offset + index];
        }

        public override CharBuffer get(char[] dest, int off, int len) {
            int length = dest.Length;
            if ((off < 0) || (len < 0) || (long) off + (long) len > length) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            if (len > remaining()) {
                throw new BufferUnderflowException();
            }
            java.lang.SystemJ.arraycopy(backingArray, offset + positionJ, dest, off, len);
            positionJ += len;
            return this;
        }

        public override sealed bool isDirect() {
            return false;
        }

        public override ByteOrder order() {
            return ByteOrder.nativeOrder();
        }

        public override java.lang.CharSequence subSequence(int start, int end) {
            if (start < 0 || end < start || end > remaining()) {
                throw new java.lang.IndexOutOfBoundsException();
            }

            CharBuffer result = duplicate();
            result.limit(positionJ + end);
            result.position(positionJ + start);
            return result;
        }

        public override String ToString() {
            return new String(backingArray, offset + positionJ, remaining());
        }
    }
}
