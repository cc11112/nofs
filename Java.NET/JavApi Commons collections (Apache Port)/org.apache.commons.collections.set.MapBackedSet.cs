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

namespace org.apache.commons.collections.set
{

    /**
     * Decorates a <code>Map</code> to obtain <code>Set</code> behaviour.
     * <p>
     * This class is used to create a <code>Set</code> with the same properties as
     * the key set of any map. Thus, a ReferenceSet can be created by wrapping a
     * <code>ReferenceMap</code> in an instance of this class.
     * <p>
     * Most map implementation can be used to create a set by passing in dummy values.
     * Exceptions include <code>BidiMap</code> implementations, as they require unique values.
     *
     * @since Commons Collections 3.1
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class MapBackedSet : java.util.Set<Object>, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 6723912213766056587L;

        /** The map being used as the backing store */
        private readonly java.util.Map<Object, Object> map;
        /** The dummyValue to use */
        private readonly Object dummyValue;

        /**
         * Factory method to create a set from a map.
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        public static java.util.Set<Object> decorate(java.util.Map<Object, Object> map)
        {
            return decorate(map, null);
        }

        /**
         * Factory method to create a set from a map.
         * 
         * @param map  the map to decorate, must not be null
         * @param dummyValue  the dummy value to use
         * @throws IllegalArgumentException if map is null
         */
        public static java.util.Set<Object> decorate(java.util.Map<Object, Object> map, Object dummyValue)
        {
            if (map == null)
            {
                throw new java.lang.IllegalArgumentException("The map must not be null");
            }
            return new MapBackedSet(map, dummyValue);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @param dummyValue  the dummy value to use
         * @throws IllegalArgumentException if map is null
         */
        private MapBackedSet(java.util.Map<Object, Object> map, Object dummyValue)
            : base()
        {
            this.map = map;
            this.dummyValue = dummyValue;
        }

        //-----------------------------------------------------------------------
        public int size()
        {
            return map.size();
        }

        public bool isEmpty()
        {
            return map.isEmpty();
        }

        public java.util.Iterator<Object> iterator()
        {
            return map.keySet().iterator();
        }

        public bool contains(Object obj)
        {
            return map.containsKey(obj);
        }

        public bool containsAll(java.util.Collection<Object> coll)
        {
            return map.keySet().containsAll(coll);
        }

        public bool add(Object obj)
        {
            int size = map.size();
            map.put(obj, dummyValue);
            return (map.size() != size);
        }

        public bool addAll(java.util.Collection<Object> coll)
        {
            int size = map.size();
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                Object obj = it.next();
                map.put(obj, dummyValue);
            }
            return (map.size() != size);
        }

        public bool remove(Object obj)
        {
            int size = map.size();
            map.remove(obj);
            return (map.size() != size);
        }

        public bool removeAll(java.util.Collection<Object> coll)
        {
            return map.keySet().removeAll(coll);
        }

        public bool retainAll(java.util.Collection<Object> coll)
        {
            return map.keySet().retainAll(coll);
        }

        public void clear()
        {
            map.clear();
        }

        public Object[] toArray()
        {
            return map.keySet().toArray();
        }

        public Object[] toArray<Object>(Object[] array)
        {
            return map.keySet().toArray(array);
        }

        public override bool Equals(Object obj)
        {
            return map.keySet().equals(obj);
        }

        public override int GetHashCode()
        {
            return map.keySet().GetHashCode();
        }

    }
}