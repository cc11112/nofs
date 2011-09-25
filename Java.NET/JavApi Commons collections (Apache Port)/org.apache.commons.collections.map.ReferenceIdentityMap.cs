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

namespace org.apache.commons.collections.map
{

    /**
     * A <code>Map</code> implementation that allows mappings to be
     * removed by the garbage collector and matches keys and values based
     * on <code>==</code> not <code>equals()</code>.
     * <p>
     * <p>
     * When you construct a <code>ReferenceIdentityMap</code>, you can specify what kind
     * of references are used to store the map's keys and values.
     * If non-hard references are used, then the garbage collector can remove
     * mappings if a key or value becomes unreachable, or if the JVM's memory is
     * running low. For information on how the different reference types behave,
     * see {@link Reference}.
     * <p>
     * Different types of references can be specified for keys and values.
     * The default constructor uses hard keys and soft values, providing a
     * memory-sensitive cache.
     * <p>
     * This map is similar to
     * {@link org.apache.commons.collections.map.ReferenceMap ReferenceMap}.
     * It differs in that keys and values in this class are compared using <code>==</code>.
     * <p>
     * This map will violate the detail of various Map and map view contracts.
     * As a general rule, don't compare this map to other maps.
     * <p>
     * This {@link java.util.Map Map} implementation does <i>not</i> allow null elements.
     * Attempting to add a null key or value to the map will raise a <code>NullPointerException</code>.
     * <p>
     * This implementation is not synchronized.
     * You can use {@link java.util.Collections#synchronizedMap} to 
     * provide synchronized access to a <code>ReferenceIdentityMap</code>.
     * Remember that synchronization will not stop the garbage collecter removing entries.
     * <p>
     * All the available iterators can be reset back to the start by casting to
     * <code>ResettableIterator</code> and calling <code>reset()</code>.
     * <p>
     * <strong>Note that ReferenceIdentityMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. The simplest approach is to wrap this map
     * using {@link java.util.Collections#synchronizedMap}. This class may throw 
     * exceptions when accessed by concurrent threads without synchronization.
     *
     * @see java.lang.ref.Reference
     * 
     * @since Commons Collections 3.0 (previously in main package v2.1)
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class ReferenceIdentityMap : AbstractReferenceMap, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -1266190134568365852L;

        /**
         * Constructs a new <code>ReferenceIdentityMap</code> that will
         * use hard references to keys and soft references to values.
         */
        public ReferenceIdentityMap()
            : base(HARD, SOFT, DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR, false)
        {
        }

        /**
         * Constructs a new <code>ReferenceIdentityMap</code> that will
         * use the specified types of references.
         *
         * @param keyType  the type of reference to use for keys;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param valueType  the type of reference to use for values;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         */
        public ReferenceIdentityMap(int keyType, int valueType)
            : base
                (keyType, valueType, DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR, false)
        {
        }

        /**
         * Constructs a new <code>ReferenceIdentityMap</code> that will
         * use the specified types of references.
         *
         * @param keyType  the type of reference to use for keys;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param valueType  the type of reference to use for values;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param purgeValues should the value be automatically purged when the 
         *   key is garbage collected 
         */
        public ReferenceIdentityMap(int keyType, int valueType, bool purgeValues) :
            base(keyType, valueType, DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR, purgeValues)
        {
        }

        /**
         * Constructs a new <code>ReferenceIdentityMap</code> with the
         * specified reference types, load factor and initial capacity.
         *
         * @param keyType  the type of reference to use for keys;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param valueType  the type of reference to use for values;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param capacity  the initial capacity for the map
         * @param loadFactor  the load factor for the map
         */
        public ReferenceIdentityMap(int keyType, int valueType, int capacity, float loadFactor)
            : base(keyType, valueType, capacity, loadFactor, false)
        {
        }

        /**
         * Constructs a new <code>ReferenceIdentityMap</code> with the
         * specified reference types, load factor and initial capacity.
         *
         * @param keyType  the type of reference to use for keys;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param valueType  the type of reference to use for values;
         *   must be {@link #HARD}, {@link #SOFT}, {@link #WEAK}
         * @param capacity  the initial capacity for the map
         * @param loadFactor  the load factor for the map
         * @param purgeValues  should the value be automatically purged when the 
         *   key is garbage collected 
         */
        public ReferenceIdentityMap(int keyType, int valueType, int capacity,
                            float loadFactor, bool purgeValues)
            : base(keyType, valueType, capacity, loadFactor, purgeValues)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the hash code for the key specified.
         * <p>
         * This implementation uses the identity hash code.
         * 
         * @param key  the key to get a hash code for
         * @return the hash code
         */
        protected override int hash(Object key)
        {
            return java.lang.SystemJ.identityHashCode(key);
        }

        /**
         * Gets the hash code for a MapEntry.
         * <p>
         * This implementation uses the identity hash code.
         * 
         * @param key  the key to get a hash code for, may be null
         * @param value  the value to get a hash code for, may be null
         * @return the hash code, as per the MapEntry specification
         */
        protected override int hashEntry(Object key, Object value)
        {
            return java.lang.SystemJ.identityHashCode(key) ^
                   java.lang.SystemJ.identityHashCode(value);
        }

        /**
         * Compares two keys for equals.
         * <p>
         * This implementation converts the key from the entry to a real reference
         * before comparison and uses <code>==</code>.
         * 
         * @param key1  the first key to compare passed in from outside
         * @param key2  the second key extracted from the entry via <code>entry.key</code>
         * @return true if equal by identity
         */
        protected override bool isEqualKey(Object key1, Object key2)
        {
            key2 = (keyType > HARD ? ((java.lang.refj.Reference<Object>)key2).get() : key2);
            return (key1 == key2);
        }

        /**
         * Compares two values for equals.
         * <p>
         * This implementation uses <code>==</code>.
         * 
         * @param value1  the first value to compare passed in from outside
         * @param value2  the second value extracted from the entry via <code>getValue()</code>
         * @return true if equal by identity
         */
        protected override bool isEqualValue(Object value1, Object value2)
        {
            return (value1 == value2);
        }

        //-----------------------------------------------------------------------
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