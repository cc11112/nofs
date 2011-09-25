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
using java = biz.ritter.javapi;
namespace biz.ritter.javapi.lang.reflect
{
    /// <summary>
    /// Reflection type for working with arrays.
    /// </summary>
    public class Array
    {
        /// <summary>
        /// Compute the length of first dimension from given array
        /// </summary>
        /// <param name="array">array</param>
        /// <returns>size of first dimension</returns>
        /// <exception cref="java.lang.IllegalArgumentException">Throw an java.lang.IllegalArgumentException if given object is not an array.</exception> 
        /// <exception cref="java.lang.NullPointer">Throw an java.lang.NullPointerException if given object is not an array.</exception> 
        public static int getLength(Object array)
        {
            return cast(array).GetLength(0);
        }

        /// <summary>
        /// Get the object stored on given offset in array
        /// </summary>
        /// <param name="array">array</param>
        /// <param name="offset">zero based index</param>
        /// <returns>Object on index</returns>
        /// <exception cref="java.lang.IllegalArgumentException">Throw an java.lang.IllegalArgumentException if given object is not an array.</exception> 
        /// <exception cref="java.lang.NullPointer">Throw an java.lang.NullPointerException if given object is not an array.</exception> 
        public static Object get(Object array, int offset)
        {
            return cast(array).GetValue(offset);
        }

        /// <summary>
        /// Set the object stored on given offset in array
        /// </summary>
        /// <param name="array">array</param>
        /// <param name="offset">zero based index</param>
        /// <returns>Object on index</returns>
        /// <exception cref="java.lang.IllegalArgumentException">Throw an java.lang.IllegalArgumentException if given object is not an array.</exception> 
        /// <exception cref="java.lang.NullPointer">Throw an java.lang.NullPointerException if given object is not an array.</exception> 
        /// <exception cref="java.lang.ArrayIndexOutOfBoundsException">Throw an java.lang.ArrayIndexOutOfBoundsException if given index is lesser zero, equals length of array or greater length of array.</exception> 
        public static void set(Object array, int offset, Object newValue)
        {
            if (offset < 0 || offset >= getLength(array)) throw new java.lang.ArrayIndexOutOfBoundsException();
            cast(array).SetValue(newValue, offset);
        }

        /// <summary>
        /// Create new Array of type with giving length
        /// </summary>
        /// <param name="type">type / classe</param>
        /// <param name="length">size of array</param>
        /// <returns></returns>
        public static Object newInstance (Type type, int length)
        {
            return System.Array.CreateInstance(type, length);
        }
        /// <summary>
        /// Cast the given object to System.Array instance.
        /// </summary>
        /// <param name="array">the object</param>
        /// <returns>A System.Array instance of given object</returns>
        /// <exception cref="java.lang.IllegalArgumentException">Throw an java.lang.IllegalArgumentException if given object is not an array.</exception> 
        /// <exception cref="java.lang.NullPointer">Throw an java.lang.NullPointerException if given object is not an array.</exception> 
        internal static System.Array cast(Object array)
        {
            if (null == array) throw new java.lang.NullPointerException();
            if (array is System.Array)
            {
                System.Array instance = (System.Array)array;
                return instance;
            }
            else
            {
                throw new java.lang.IllegalArgumentException();
            }
        }
    }
}
