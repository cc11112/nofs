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
     * This class wraps a char sequence to be a char buffer.
     * <p>
     * Implementation notice:
     * <ul>
     * <li>Char sequence based buffer is always readonly.</li>
     * </ul>
     * </p>
     * 
     */
    sealed class CharSequenceAdapter : CharBuffer {

        internal static CharSequenceAdapter copy(CharSequenceAdapter other) {
            CharSequenceAdapter buf = new CharSequenceAdapter(other.sequence);
            buf.limitJ = other.limitJ;
            buf.positionJ = other.positionJ;
            buf.markJ = other.markJ;
            return buf;
        }

        internal readonly java.lang.CharSequence sequence;

        internal CharSequenceAdapter(java.lang.CharSequence chseq) : base (chseq.length()){
            sequence = chseq;
        }

        public override CharBuffer asReadOnlyBuffer() {
            return duplicate();
        }

        public override CharBuffer compact() {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer duplicate() {
            return copy(this);
        }

        public override char get() {
            if (positionJ == limitJ) {
                throw new BufferUnderflowException();
            }
            return sequence.charAt(positionJ++);
        }

        public override char get(int index) {
            if (index < 0 || index >= limitJ) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            return sequence.charAt(index);
        }

        public override CharBuffer get(char[] dest, int off, int len) {
            int length = dest.Length;
            if ((off < 0) || (len < 0) || (long) off + (long) len > length) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            if (len > remaining()) {
                throw new BufferUnderflowException();
            }
            int newPosition = positionJ + len;
            sequence.toString().getChars(positionJ, newPosition, dest, off);
            positionJ = newPosition;
            return this;
        }

        public override bool isDirect() {
            return false;
        }

        public override bool isReadOnly() {
            return true;
        }

        public override ByteOrder order() {
            return ByteOrder.nativeOrder();
        }

        public override char[] protectedArray() {
            throw new java.lang.UnsupportedOperationException();
        }

        public override int protectedArrayOffset() {
            throw new java.lang.UnsupportedOperationException();
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
            if ((off < 0) || (len < 0) || (long) off + (long) len > src.Length) {
                throw new java.lang.IndexOutOfBoundsException();
            }

            if (len > remaining()) {
                throw new BufferOverflowException();
            }

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
            return new CharSequenceAdapter(sequence.subSequence(positionJ, limitJ));
        }

        
        public override java.lang.CharSequence subSequence(int start, int end) {
            if (end < start || start < 0 || end > remaining()) {
                throw new java.lang.IndexOutOfBoundsException();
            }

            CharSequenceAdapter result = copy(this);
            result.positionJ = positionJ + start;
            result.limitJ = positionJ + end;
            return result;
        }
    }
}
