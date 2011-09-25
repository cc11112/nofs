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
using org.apache.commons.collections;
using org.apache.commons.collections.collection;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.map
{

    /**
     * Decorates another <code>Map</code> to fix the size, preventing add/remove.
     * <p>
     * Any action that would change the size of the map is disallowed.
     * The put method is allowed to change the value associated with an existing
     * key however.
     * <p>
     * If trying to remove or clear the map, an UnsupportedOperationException is
     * thrown. If trying to put a new mapping into the map, an 
     * IllegalArgumentException is thrown. This is because the put method can 
     * succeed if the mapping's key already exists in the map, so the put method
     * is not always unsupported.
     * <p>
     * <strong>Note that FixedSizeMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. The simplest approach is to wrap this map
     * using {@link java.util.Collections#synchronizedMap(Map)}. This class may throw 
     * exceptions when accessed by concurrent threads without synchronization.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     * @author Paul Jack
     */
    [Serializable]
    public class FixedSizeMap
            : AbstractMapDecorator
            , java.util.Map<Object, Object>, BoundedMap, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 7450927208116179316L;

        /**
         * Factory method to create a fixed size map.
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        public static java.util.Map<Object, Object> decorate(java.util.Map<Object, Object> map)
        {
            return new FixedSizeMap(map);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        protected FixedSizeMap(java.util.Map<Object, Object> map)
            : base(map)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Write the map out using a custom routine.
         * 
         * @param out  the output stream
         * @throws IOException
         * @since Commons Collections 3.1
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeObject(map);
        }

        /**
         * Read the map in using a custom routine.
         * 
         * @param in  the input stream
         * @throws IOException
         * @throws ClassNotFoundException
         * @since Commons Collections 3.1
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            map = (java.util.Map<Object, Object>)inJ.readObject();
        }

        //-----------------------------------------------------------------------
        public override Object put(Object key, Object value)
        {
            if (map.containsKey(key) == false)
            {
                throw new java.lang.IllegalArgumentException("Cannot put new key/value pair - Map is fixed size");
            }
            return map.put(key, value);
        }

        public override void putAll(java.util.Map<Object, Object> mapToCopy)
        {
            for (java.util.Iterator<Object> it = mapToCopy.keySet().iterator(); it.hasNext(); )
            {
                if (mapToCopy.containsKey(it.next()) == false)
                {
                    throw new java.lang.IllegalArgumentException("Cannot put new key/value pair - Map is fixed size");
                }
            }
            map.putAll(mapToCopy);
        }

        public override void clear()
        {
            throw new java.lang.UnsupportedOperationException("Map is fixed size");
        }

        public override Object remove(Object key)
        {
            throw new java.lang.UnsupportedOperationException("Map is fixed size");
        }

        public new java.util.Set<Object> entrySet()
        {
            java.util.Set<Object> set = (java.util.Set<Object>)map.entrySet();
            // unmodifiable set will still allow modification via Map.Entry objects
            return UnmodifiableSet.decorate(set);
        }

        public override java.util.Set<Object> keySet()
        {
            java.util.Set<Object> set = map.keySet();
            return UnmodifiableSet.decorate(set);
        }

        public override java.util.Collection<Object> values()
        {
            java.util.Collection<Object> coll = map.values();
            return UnmodifiableCollection.decorate(coll);
        }

        public virtual bool isFull()
        {
            return true;
        }

        public virtual int maxSize()
        {
            return size();
        }

    }
}