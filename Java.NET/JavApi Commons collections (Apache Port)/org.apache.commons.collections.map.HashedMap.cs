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

namespace org.apache.commons.collections.map
{

    /**
     * A <code>Map</code> implementation that is a general purpose alternative
     * to <code>HashMap</code>.
     * <p>
     * This implementation improves on the JDK1.4 HashMap by adding the 
     * {@link org.apache.commons.collections.MapIterator MapIterator}
     * functionality and many methods for subclassing.
     * <p>
     * <strong>Note that HashedMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. The simplest approach is to wrap this map
     * using {@link java.util.Collections#synchronizedMap(Map)}. This class may throw 
     * exceptions when accessed by concurrent threads without synchronization.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public class HashedMap
            : AbstractHashedMap, java.io.Serializable, java.lang.Cloneable
    {

        /** Serialisation version */
        private static readonly long serialVersionUID = -1788199231038721040L;

        /**
         * Constructs a new empty map with default size and load factor.
         */
        public HashedMap()
            : base(DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR, DEFAULT_THRESHOLD)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity. 
         *
         * @param initialCapacity  the initial capacity
         * @throws IllegalArgumentException if the initial capacity is less than one
         */
        public HashedMap(int initialCapacity)
            : base(initialCapacity)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity and
         * load factor. 
         *
         * @param initialCapacity  the initial capacity
         * @param loadFactor  the load factor
         * @throws IllegalArgumentException if the initial capacity is less than one
         * @throws IllegalArgumentException if the load factor is less than zero
         */
        public HashedMap(int initialCapacity, float loadFactor)
            : base(initialCapacity, loadFactor)
        {
        }

        /**
         * Constructor copying elements from another map.
         *
         * @param map  the map to copy
         * @throws NullPointerException if the map is null
         */
        public HashedMap(java.util.Map<Object, Object> map)
            : base(map)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Clones the map without cloning the keys or values.
         *
         * @return a shallow clone
         */
        protected override Object clone()
        {
            return base.clone();
        }
        Object java.lang.Cloneable.clone()
        {
            return this.clone();
        }

        /**
         * Write the map out using a custom routine.
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            doWriteObject(outJ);
        }

        /**
         * Read the map in using a custom routine.
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            doReadObject(inJ);
        }

    }
}