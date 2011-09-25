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
    /// Provide factory service of buffer classes.
    /// <p>
    /// Since all buffer impl classes are package private (except DirectByteBuffer),
    /// this factory is the only entrance to access buffer functions from outside of
    /// the impl package.
    /// </p>
    /// </summary>
    /// <remarks>Class is ported from Apache Harmony project.</remarks>
    sealed class BufferFactory
    {

        /// <summary>
        /// Returns a new byte buffer based on the specified byte array.
        /// 
        /// @param array
        ///            The byte array
        /// @return A new byte buffer based on the specified byte array.
        /// </summary>
        public static ByteBuffer newByteBuffer(byte [] array) {
            return new ReadWriteHeapByteBuffer(array);
        }

        /// <summary>
        /// Returns a new array based byte buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based byte buffer with the specified capacity.
        /// </summary>
        public static ByteBuffer newByteBuffer(int capacity) {
            return new ReadWriteHeapByteBuffer(capacity);
        }

        /// <summary>
        /// Returns a new char buffer based on the specified char array.
        /// 
        /// @param array
        ///            The char array
        /// @return A new char buffer based on the specified char array.
        /// </summary>
        public static CharBuffer newCharBuffer(char [] array) {
            return new ReadWriteCharArrayBuffer(array);
        }

        /// <summary>
        /// Returns a new readonly char buffer based on the specified char sequence.
        /// 
        /// @param chseq
        ///            The char sequence
        /// @return A new readonly char buffer based on the specified char sequence.
        /// </summary>
        public static CharBuffer newCharBuffer(java.lang.CharSequence chseq)
        {
            return new CharSequenceAdapter(chseq);
        }

        /// <summary>
        /// Returns a new array based char buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based char buffer with the specified capacity.
        /// </summary>
        public static CharBuffer newCharBuffer(int capacity) {
            return new ReadWriteCharArrayBuffer(capacity);
        }

/**
        /// <summary>
        /// Returns a new direct byte buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new direct byte buffer with the specified capacity.
        /// </summary>
        public static ByteBuffer newDirectByteBuffer(int capacity) {
            return new ReadWriteDirectByteBuffer(capacity);
        }

        /// <summary>
        /// Returns a new double buffer based on the specified double array.
        /// 
        /// @param array
        ///            The double array
        /// @return A new double buffer based on the specified double array.
        /// </summary>
        public static DoubleBuffer newDoubleBuffer(double array[]) {
            return new ReadWriteDoubleArrayBuffer(array);
        }

        /// <summary>
        /// Returns a new array based double buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based double buffer with the specified capacity.
        /// </summary>
        public static DoubleBuffer newDoubleBuffer(int capacity) {
            return new ReadWriteDoubleArrayBuffer(capacity);
        }

        /// <summary>
        /// Returns a new float buffer based on the specified float array.
        /// 
        /// @param array
        ///            The float array
        /// @return A new float buffer based on the specified float array.
        /// </summary>
        public static FloatBuffer newFloatBuffer(float array[]) {
            return new ReadWriteFloatArrayBuffer(array);
        }

        /// <summary>
        /// Returns a new array based float buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based float buffer with the specified capacity.
        /// </summary>
        public static FloatBuffer newFloatBuffer(int capacity) {
            return new ReadWriteFloatArrayBuffer(capacity);
        }

        /// <summary>
        /// Returns a new array based int buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based int buffer with the specified capacity.
        /// </summary>
        public static IntBuffer newIntBuffer(int capacity) {
            return new ReadWriteIntArrayBuffer(capacity);
        }

        /// <summary>
        /// Returns a new int buffer based on the specified int array.
        /// 
        /// @param array
        ///            The int array
        /// @return A new int buffer based on the specified int array.
        /// </summary>
        public static IntBuffer newIntBuffer(int array[]) {
            return new ReadWriteIntArrayBuffer(array);
        }

        /// <summary>
        /// Returns a new array based long buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based long buffer with the specified capacity.
        /// </summary>
        public static LongBuffer newLongBuffer(int capacity) {
            return new ReadWriteLongArrayBuffer(capacity);
        }

        /// <summary>
        /// Returns a new long buffer based on the specified long array.
        /// 
        /// @param array
        ///            The long array
        /// @return A new long buffer based on the specified long array.
        /// </summary>
        public static LongBuffer newLongBuffer(long array[]) {
            return new ReadWriteLongArrayBuffer(array);
        }

        /// <summary>
        /// Returns a new array based short buffer with the specified capacity.
        /// 
        /// @param capacity
        ///            The capacity of the new buffer
        /// @return A new array based short buffer with the specified capacity.
        /// </summary>
        public static ShortBuffer newShortBuffer(int capacity) {
            return new ReadWriteShortArrayBuffer(capacity);
        }

        /// <summary>
        /// Returns a new short buffer based on the specified short array.
        /// 
        /// @param array
        ///            The short array
        /// @return A new short buffer based on the specified short array.
        /// </summary>
        public static ShortBuffer newShortBuffer(short array[]) {
            return new ReadWriteShortArrayBuffer(array);
        }
*/
    }
}
