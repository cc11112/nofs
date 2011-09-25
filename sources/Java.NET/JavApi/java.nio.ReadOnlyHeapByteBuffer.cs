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

/**
 * HeapByteBuffer, ReadWriteHeapByteBuffer and ReadOnlyHeapByteBuffer compose
 * the implementation of array based byte buffers.
 * <p>
 * ReadOnlyHeapByteBuffer extends HeapByteBuffer with all the write methods
 * throwing read only exception.
 * </p>
 * <p>
 * This class is marked final for runtime performance.
 * </p>
 * 
 */
sealed class ReadOnlyHeapByteBuffer : HeapByteBuffer {

    internal static ReadOnlyHeapByteBuffer copy(HeapByteBuffer other, int markOfOther) {
        ReadOnlyHeapByteBuffer buf = new ReadOnlyHeapByteBuffer(
                other.backingArray, other.capacity(), other.offset);
        buf.limitJ = other.limit();
        buf.positionJ = other.position();
        buf.markJ = markOfOther;
        buf.order(other.order());
        return buf;
    }

    internal ReadOnlyHeapByteBuffer(byte[] backingArray, int capacity, int arrayOffset) : base (backingArray, capacity, arrayOffset){
    }

    public override ByteBuffer asReadOnlyBuffer() {
        return copy(this, markJ);
    }

    public override ByteBuffer compact() {
        throw new ReadOnlyBufferException();
    }

    public override ByteBuffer duplicate() {
        return copy(this, markJ);
    }

    public override bool isReadOnly() {
        return true;
    }

    protected override byte[] protectedArray() {
        throw new ReadOnlyBufferException();
    }

    protected override int protectedArrayOffset() {
        throw new ReadOnlyBufferException();
    }

    protected override bool protectedHasArray() {
        return false;
    }

    public override ByteBuffer put(byte b) {
        throw new ReadOnlyBufferException();
    }

    public override ByteBuffer put(int index, byte b) {
        throw new ReadOnlyBufferException();
    }



    public override ByteBuffer putShort(int index, short value) {
        throw new ReadOnlyBufferException();
    }

    public override ByteBuffer putShort(short value) {
        throw new ReadOnlyBufferException();
    }

    public override ByteBuffer slice() {
        ReadOnlyHeapByteBuffer slice = new ReadOnlyHeapByteBuffer(backingArray,
                remaining(), offset + positionJ);
        slice.orderJ = orderJ;
        return slice;
    }
}
}
