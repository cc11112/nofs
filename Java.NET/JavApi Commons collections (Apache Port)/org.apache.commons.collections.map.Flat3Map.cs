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
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.map
{

    /**
     * A <code>Map</code> implementation that stores data in simple fields until
     * the size is greater than 3.
     * <p>
     * This map is designed for performance and can outstrip HashMap.
     * It also has good garbage collection characteristics.
     * <ul>
     * <li>Optimised for operation at size 3 or less.
     * <li>Still works well once size 3 exceeded.
     * <li>Gets at size 3 or less are about 0-10% faster than HashMap,
     * <li>Puts at size 3 or less are over 4 times faster than HashMap.
     * <li>Performance 5% slower than HashMap once size 3 exceeded once.
     * </ul>
     * The design uses two distinct modes of operation - flat and delegate.
     * While the map is size 3 or less, operations map straight onto fields using
     * switch statements. Once size 4 is reached, the map switches to delegate mode
     * and only switches back when cleared. In delegate mode, all operations are
     * forwarded straight to a HashMap resulting in the 5% performance loss.
     * <p>
     * The performance gains on puts are due to not needing to create a Map Entry
     * object. This is a large saving not only in performance but in garbage collection.
     * <p>
     * Whilst in flat mode this map is also easy for the garbage collector to dispatch.
     * This is because it contains no complex objects or arrays which slow the progress.
     * <p>
     * Do not use <code>Flat3Map</code> if the size is likely to grow beyond 3.
     * <p>
     * <strong>Note that Flat3Map is not synchronized and is not thread-safe.</strong>
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
    public class Flat3Map : IterableMap, java.io.Serializable, java.lang.Cloneable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -6701087419741928296L;

        /** The size of the map, used while in flat mode */
        [NonSerialized]
        private int sizeJ;
        /** Hash, used while in flat mode */
        [NonSerialized]
        private int hash1;
        /** Hash, used while in flat mode */
        [NonSerialized]
        private int hash2;
        /** Hash, used while in flat mode */
        [NonSerialized]
        private int hash3;
        /** Key, used while in flat mode */
        [NonSerialized]
        private Object key1;
        /** Key, used while in flat mode */
        [NonSerialized]
        private Object key2;
        /** Key, used while in flat mode */
        [NonSerialized]
        private Object key3;
        /** Value, used while in flat mode */
        [NonSerialized]
        private Object value1;
        /** Value, used while in flat mode */
        [NonSerialized]
        private Object value2;
        /** Value, used while in flat mode */
        [NonSerialized]
        private Object value3;
        /** Map, used while in delegate mode */
        [NonSerialized]
        private AbstractHashedMap delegateMap;

        /**
         * Constructor.
         */
        public Flat3Map()
            : base()
        {
        }

        /**
         * Constructor copying elements from another map.
         *
         * @param map  the map to copy
         * @throws NullPointerException if the map is null
         */
        public Flat3Map(java.util.Map<Object, Object> map)
            : base()
        {
            putAll(map);
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the value mapped to the key specified.
         * 
         * @param key  the key
         * @return the mapped value, null if no match
         */
        public Object get(Object key)
        {
            if (delegateMap != null)
            {
                return delegateMap.get(key);
            }
            if (key == null)
            {
                switch (sizeJ)
                {
                    // drop through
                    case 3:
                        if (key3 == null) return value3;
                        if (key2 == null) return value2;
                        if (key1 == null) return value1;
                        break;
                    case 2:
                        if (key2 == null) return value2;
                        if (key1 == null) return value1;
                        break;
                    case 1:
                        if (key1 == null) return value1;
                        break;
                }
            }
            else
            {
                if (sizeJ > 0)
                {
                    int hashCode = key.GetHashCode();
                    switch (sizeJ)
                    {
                        // drop through
                        case 3:
                            if (hash3 == hashCode && key.equals(key3)) return value3;
                            if (hash2 == hashCode && key.equals(key2)) return value2;
                            if (hash1 == hashCode && key.equals(key1)) return value1;
                            break;
                        case 2:
                            if (hash2 == hashCode && key.equals(key2)) return value2;
                            if (hash1 == hashCode && key.equals(key1)) return value1;
                            break;
                        case 1:
                            if (hash1 == hashCode && key.equals(key1)) return value1;
                            break;
                    }
                }
            }
            return null;
        }

        /**
         * Gets the size of the map.
         * 
         * @return the size
         */
        public int size()
        {
            if (delegateMap != null)
            {
                return delegateMap.size();
            }
            return sizeJ;
        }

        /**
         * Checks whether the map is currently empty.
         * 
         * @return true if the map is currently size zero
         */
        public bool isEmpty()
        {
            return (size() == 0);
        }

        //-----------------------------------------------------------------------
        /**
         * Checks whether the map contains the specified key.
         * 
         * @param key  the key to search for
         * @return true if the map contains the key
         */
        public bool containsKey(Object key)
        {
            if (delegateMap != null)
            {
                return delegateMap.containsKey(key);
            }
            if (key == null)
            {
                switch (sizeJ)
                {  // drop through
                    case 3:
                        if (key3 == null) return true;
                        if (key2 == null) return true;
                        if (key1 == null) return true;
                        break;
                    case 2:
                        if (key2 == null) return true;
                        if (key1 == null) return true;
                        break;
                    case 1:
                        if (key1 == null) return true;
                        break;
                }
            }
            else
            {
                if (sizeJ > 0)
                {
                    int hashCode = key.GetHashCode();
                    switch (sizeJ)
                    {  // drop through
                        case 3:
                            if (hash3 == hashCode && key.equals(key3)) return true;
                            if (hash2 == hashCode && key.equals(key2)) return true;
                            if (hash1 == hashCode && key.equals(key1)) return true;
                            break;
                        case 2:
                            if (hash2 == hashCode && key.equals(key2)) return true;
                            if (hash1 == hashCode && key.equals(key1)) return true;
                            break;
                        case 1:
                            if (hash1 == hashCode && key.equals(key1)) return true;
                            break;
                    }
                }
            }
            return false;
        }

        /**
         * Checks whether the map contains the specified value.
         * 
         * @param value  the value to search for
         * @return true if the map contains the key
         */
        public bool containsValue(Object value)
        {
            if (delegateMap != null)
            {
                return delegateMap.containsValue(value);
            }
            if (value == null)
            {  // drop through
                switch (sizeJ)
                {
                    case 3:
                        if (value3 == null) return true;
                        if (value2 == null) return true;
                        if (value1 == null) return true;
                        break;
                    case 2:
                        if (value2 == null) return true;
                        if (value1 == null) return true;
                        break;
                    case 1:
                        if (value1 == null) return true;
                        break;
                }
            }
            else
            {
                switch (sizeJ)
                {  // drop through
                    case 3:
                        if (value.equals(value3)) return true;
                        if (value.equals(value2)) return true;
                        if (value.equals(value1)) return true;
                        break;
                    case 2:
                        if (value.equals(value2)) return true;
                        if (value.equals(value1)) return true;
                        break;
                    case 1:
                        if (value.equals(value1)) return true;
                        break;
                }
            }
            return false;
        }

        //-----------------------------------------------------------------------
        /**
         * Puts a key-value mapping into this map.
         * 
         * @param key  the key to add
         * @param value  the value to add
         * @return the value previously mapped to this key, null if none
         */
        public Object put(Object key, Object value)
        {
            if (delegateMap != null)
            {
                return delegateMap.put(key, value);
            }
            // change existing mapping
            if (key == null)
            {
                switch (sizeJ)
                {  // drop through
                    case 3:
                        if (key3 == null)
                        {
                            Object old = value3;
                            value3 = value;
                            return old;
                        }
                        if (key2 == null)
                        {
                            Object old = value2;
                            value2 = value;
                            return old;
                        }
                        if (key1 == null)
                        {
                            Object old = value1;
                            value1 = value;
                            return old;
                        }
                        break;
                    case 2:
                        if (key2 == null)
                        {
                            Object old = value2;
                            value2 = value;
                            return old;
                        }
                        if (key1 == null)
                        {
                            Object old = value1;
                            value1 = value;
                            return old;
                        }
                        break;
                    case 1:
                        if (key1 == null)
                        {
                            Object old = value1;
                            value1 = value;
                            return old;
                        }
                        break;
                }
            }
            else
            {
                if (sizeJ > 0)
                {
                    int hashCode = key.GetHashCode();
                    switch (sizeJ)
                    {  // drop through
                        case 3:
                            if (hash3 == hashCode && key.equals(key3))
                            {
                                Object old = value3;
                                value3 = value;
                                return old;
                            }
                            if (hash2 == hashCode && key.equals(key2))
                            {
                                Object old = value2;
                                value2 = value;
                                return old;
                            }
                            if (hash1 == hashCode && key.equals(key1))
                            {
                                Object old = value1;
                                value1 = value;
                                return old;
                            }
                            break;
                        case 2:
                            if (hash2 == hashCode && key.equals(key2))
                            {
                                Object old = value2;
                                value2 = value;
                                return old;
                            }
                            if (hash1 == hashCode && key.equals(key1))
                            {
                                Object old = value1;
                                value1 = value;
                                return old;
                            }
                            break;
                        case 1:
                            if (hash1 == hashCode && key.equals(key1))
                            {
                                Object old = value1;
                                value1 = value;
                                return old;
                            }
                            break;
                    }
                }
            }

            // add new mapping
            switch (sizeJ)
            {
                default:
                    convertToMap();
                    delegateMap.put(key, value);
                    return null;
                case 2:
                    hash3 = (key == null ? 0 : key.GetHashCode());
                    key3 = key;
                    value3 = value;
                    break;
                case 1:
                    hash2 = (key == null ? 0 : key.GetHashCode());
                    key2 = key;
                    value2 = value;
                    break;
                case 0:
                    hash1 = (key == null ? 0 : key.GetHashCode());
                    key1 = key;
                    value1 = value;
                    break;
            }
            sizeJ++;
            return null;
        }

        /**
         * Puts all the values from the specified map into this map.
         * 
         * @param map  the map to add
         * @throws NullPointerException if the map is null
         */
        public void putAll(java.util.Map<Object, Object> map)
        {
            int size = map.size();
            if (size == 0)
            {
                return;
            }
            if (delegateMap != null)
            {
                delegateMap.putAll(map);
                return;
            }
            if (size < 4)
            {
                for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
                {
                    java.util.MapNS.Entry<Object, Object> entry = it.next();
                    put(entry.getKey(), entry.getValue());
                }
            }
            else
            {
                convertToMap();
                delegateMap.putAll(map);
            }
        }

        /**
         * Converts the flat map data to a map.
         */
        private void convertToMap()
        {
            delegateMap = createDelegateMap();
            switch (sizeJ)
            {  // drop through
                case 3:
                    delegateMap.put(key3, value3);
                    delegateMap.put(key2, value2);
                    delegateMap.put(key1, value1);
                    break;
                case 2:
                    delegateMap.put(key2, value2);
                    delegateMap.put(key1, value1);
                    break;
                case 1:
                    delegateMap.put(key1, value1);
                    break;
            }

            sizeJ = 0;
            hash1 = hash2 = hash3 = 0;
            key1 = key2 = key3 = null;
            value1 = value2 = value3 = null;
        }

        /**
         * Create an instance of the map used for storage when in delegation mode.
         * <p>
         * This can be overridden by subclasses to provide a different map implementation.
         * Not every AbstractHashedMap is suitable, identity and reference based maps
         * would be poor choices.
         *
         * @return a new AbstractHashedMap or subclass
         * @since Commons Collections 3.1
         */
        protected AbstractHashedMap createDelegateMap()
        {
            return new HashedMap();
        }

        /**
         * Removes the specified mapping from this map.
         * 
         * @param key  the mapping to remove
         * @return the value mapped to the removed key, null if key not in map
         */
        public Object remove(Object key)
        {
            if (delegateMap != null)
            {
                return delegateMap.remove(key);
            }
            if (sizeJ == 0)
            {
                return null;
            }
            if (key == null)
            {
                switch (sizeJ)
                {  // drop through
                    case 3:
                        if (key3 == null)
                        {
                            Object old = value3;
                            hash3 = 0;
                            key3 = null;
                            value3 = null;
                            sizeJ = 2;
                            return old;
                        }
                        if (key2 == null)
                        {
                            Object old = value3;
                            hash2 = hash3;
                            key2 = key3;
                            value2 = value3;
                            hash3 = 0;
                            key3 = null;
                            value3 = null;
                            sizeJ = 2;
                            return old;
                        }
                        if (key1 == null)
                        {
                            Object old = value3;
                            hash1 = hash3;
                            key1 = key3;
                            value1 = value3;
                            hash3 = 0;
                            key3 = null;
                            value3 = null;
                            sizeJ = 2;
                            return old;
                        }
                        return null;
                    case 2:
                        if (key2 == null)
                        {
                            Object old = value2;
                            hash2 = 0;
                            key2 = null;
                            value2 = null;
                            sizeJ = 1;
                            return old;
                        }
                        if (key1 == null)
                        {
                            Object old = value2;
                            hash1 = hash2;
                            key1 = key2;
                            value1 = value2;
                            hash2 = 0;
                            key2 = null;
                            value2 = null;
                            sizeJ = 1;
                            return old;
                        }
                        return null;
                    case 1:
                        if (key1 == null)
                        {
                            Object old = value1;
                            hash1 = 0;
                            key1 = null;
                            value1 = null;
                            sizeJ = 0;
                            return old;
                        }
                        break;
                }
            }
            else
            {
                if (sizeJ > 0)
                {
                    int hashCode = key.GetHashCode();
                    switch (sizeJ)
                    {  // drop through
                        case 3:
                            if (hash3 == hashCode && key.equals(key3))
                            {
                                Object old = value3;
                                hash3 = 0;
                                key3 = null;
                                value3 = null;
                                sizeJ = 2;
                                return old;
                            }
                            if (hash2 == hashCode && key.equals(key2))
                            {
                                Object old = value3;
                                hash2 = hash3;
                                key2 = key3;
                                value2 = value3;
                                hash3 = 0;
                                key3 = null;
                                value3 = null;
                                sizeJ = 2;
                                return old;
                            }
                            if (hash1 == hashCode && key.equals(key1))
                            {
                                Object old = value3;
                                hash1 = hash3;
                                key1 = key3;
                                value1 = value3;
                                hash3 = 0;
                                key3 = null;
                                value3 = null;
                                sizeJ = 2;
                                return old;
                            }
                            return null;
                        case 2:
                            if (hash2 == hashCode && key.equals(key2))
                            {
                                Object old = value2;
                                hash2 = 0;
                                key2 = null;
                                value2 = null;
                                sizeJ = 1;
                                return old;
                            }
                            if (hash1 == hashCode && key.equals(key1))
                            {
                                Object old = value2;
                                hash1 = hash2;
                                key1 = key2;
                                value1 = value2;
                                hash2 = 0;
                                key2 = null;
                                value2 = null;
                                sizeJ = 1;
                                return old;
                            }
                            return null;
                        case 1:
                            if (hash1 == hashCode && key.equals(key1))
                            {
                                Object old = value1;
                                hash1 = 0;
                                key1 = null;
                                value1 = null;
                                sizeJ = 0;
                                return old;
                            }
                            break;
                    }
                }
            }
            return null;
        }

        /**
         * Clears the map, resetting the size to zero and nullifying references
         * to avoid garbage collection issues.
         */
        public void clear()
        {
            if (delegateMap != null)
            {
                delegateMap.clear();  // should aid gc
                delegateMap = null;  // switch back to flat mode
            }
            else
            {
                sizeJ = 0;
                hash1 = hash2 = hash3 = 0;
                key1 = key2 = key3 = null;
                value1 = value2 = value3 = null;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Gets an iterator over the map.
         * Changes made to the iterator affect this map.
         * <p>
         * A MapIterator returns the keys in the map. It also provides convenient
         * methods to get the key and value, and set the value.
         * It avoids the need to create an entrySet/keySet/values object.
         * It also avoids creating the Map Entry object.
         * 
         * @return the map iterator
         */
        public MapIterator mapIterator()
        {
            if (delegateMap != null)
            {
                return delegateMap.mapIterator();
            }
            if (sizeJ == 0)
            {
                return EmptyMapIterator.INSTANCE;
            }
            return new FlatMapIterator(this);
        }

        /**
         * FlatMapIterator
         */
        internal class FlatMapIterator : MapIterator, ResettableIterator
        {
            private readonly Flat3Map parent;
            private int nextIndex = 0;
            private bool canRemove = false;

            internal FlatMapIterator(Flat3Map parent)
                : base()
            {
                this.parent = parent;
            }

            public bool hasNext()
            {
                return (nextIndex < parent.sizeJ);
            }

            public Object next()
            {
                if (hasNext() == false)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_NEXT_ENTRY);
                }
                canRemove = true;
                nextIndex++;
                return getKey();
            }

            public void remove()
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.REMOVE_INVALID);
                }
                parent.remove(getKey());
                nextIndex--;
                canRemove = false;
            }

            public Object getKey()
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETKEY_INVALID);
                }
                switch (nextIndex)
                {
                    case 3:
                        return parent.key3;
                    case 2:
                        return parent.key2;
                    case 1:
                        return parent.key1;
                }
                throw new java.lang.IllegalStateException("Invalid map index");
            }

            public Object getValue()
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETVALUE_INVALID);
                }
                switch (nextIndex)
                {
                    case 3:
                        return parent.value3;
                    case 2:
                        return parent.value2;
                    case 1:
                        return parent.value1;
                }
                throw new java.lang.IllegalStateException("Invalid map index");
            }

            public Object setValue(Object value)
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.SETVALUE_INVALID);
                }
                Object old = getValue();
                switch (nextIndex)
                {
                    case 3:
                        parent.value3 = value;
                        parent.value2 = value;
                        parent.value1 = value;
                        break;
                    case 2:
                        parent.value2 = value;
                        parent.value1 = value;
                        break;
                    case 1:
                        parent.value1 = value;
                        break;
                }
                return old;
            }

            public void reset()
            {
                nextIndex = 0;
                canRemove = false;
            }

            public String toString()
            {
                if (canRemove)
                {
                    return "Iterator[" + getKey() + "=" + getValue() + "]";
                }
                else
                {
                    return "Iterator[]";
                }
            }
        }

        /**
         * Gets the entrySet view of the map.
         * Changes made to the view affect this map.
         * The Map Entry is not an independent object and changes as the 
         * iterator progresses.
         * To simply iterate through the entries, use {@link #mapIterator()}.
         * 
         * @return the entrySet view
         */
        public java.util.Set<java.util.MapNS.Entry<Object,Object>> entrySet()
        {
            if (delegateMap != null)
            {
                return (java.util.Set<java.util.MapNS.Entry<Object, Object>>)delegateMap.entrySet();
            }
            return (java.util.Set<java.util.MapNS.Entry<Object,Object>>)new EntrySet(this);
        }

        /**
         * EntrySet
         */
        class EntrySet : java.util.AbstractSet<Object>
        {
            private readonly Flat3Map parent;

            internal EntrySet(Flat3Map parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return parent.size();
            }

            public override void clear()
            {
                parent.clear();
            }

            public override bool remove(Object obj)
            {
                if (obj is java.util.MapNS.Entry<Object, Object> == false)
                {
                    return false;
                }
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)obj;
                Object key = entry.getKey();
                bool result = parent.containsKey(key);
                parent.remove(key);
                return result;
            }

            public override java.util.Iterator<Object> iterator()
            {
                if (parent.delegateMap != null)
                {
                    return parent.delegateMap.entrySet().iterator();
                }
                if (parent.size() == 0)
                {
                    return EmptyIterator.INSTANCE;
                }
                return new EntrySetIterator(parent);
            }
        }

        /**
         * EntrySetIterator and MapEntry
         */
        class EntrySetIterator : java.util.Iterator<Object>, java.util.MapNS.Entry<Object, Object>
        {
            private readonly Flat3Map parent;
            private int nextIndex = 0;
            private bool canRemove = false;

            protected internal EntrySetIterator(Flat3Map parent)
                : base()
            {
                this.parent = parent;
            }

            public virtual bool hasNext()
            {
                return (nextIndex < parent.sizeJ);
            }

            public virtual Object next()
            {
                if (hasNext() == false)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_NEXT_ENTRY);
                }
                canRemove = true;
                nextIndex++;
                return this;
            }

            public virtual void remove()
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.REMOVE_INVALID);
                }
                parent.remove(getKey());
                nextIndex--;
                canRemove = false;
            }

            public virtual Object getKey()
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETKEY_INVALID);
                }
                switch (nextIndex)
                {
                    case 3:
                        return parent.key3;
                    case 2:
                        return parent.key2;
                    case 1:
                        return parent.key1;
                }
                throw new java.lang.IllegalStateException("Invalid map index");
            }

            public virtual Object getValue()
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETVALUE_INVALID);
                }
                switch (nextIndex)
                {
                    case 3:
                        return parent.value3;
                    case 2:
                        return parent.value2;
                    case 1:
                        return parent.value1;
                }
                throw new java.lang.IllegalStateException("Invalid map index");
            }

            public virtual Object setValue(Object value)
            {
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.SETVALUE_INVALID);
                }
                Object old = getValue();
                switch (nextIndex)
                {
                    case 3:
                        parent.value3 = value;
                        parent.value2 = value;
                        parent.value1 = value;
                        break;
                    case 2:
                        parent.value2 = value;
                        parent.value1 = value;
                        break;
                    case 1:
                        parent.value1 = value;
                        break;
                }
                return old;
            }

            public override bool Equals(Object obj)
            {
                if (canRemove == false)
                {
                    return false;
                }
                if (obj is java.util.MapNS.Entry<Object, Object> == false)
                {
                    return false;
                }
                java.util.MapNS.Entry<Object, Object> other = (java.util.MapNS.Entry<Object, Object>)obj;
                Object key = getKey();
                Object value = getValue();
                return (key == null ? other.getKey() == null : key.equals(other.getKey())) &&
                       (value == null ? other.getValue() == null : value.equals(other.getValue()));
            }

            public override int GetHashCode()
            {
                if (canRemove == false)
                {
                    return 0;
                }
                Object key = getKey();
                Object value = getValue();
                return (key == null ? 0 : key.GetHashCode()) ^
                       (value == null ? 0 : value.GetHashCode());
            }

            public override String ToString()
            {
                if (canRemove)
                {
                    return getKey() + "=" + getValue();
                }
                else
                {
                    return "";
                }
            }
        }

        /**
         * Gets the keySet view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the keys, use {@link #mapIterator()}.
         * 
         * @return the keySet view
         */
        public virtual java.util.Set<Object> keySet()
        {
            if (delegateMap != null)
            {
                return delegateMap.keySet();
            }
            return new KeySet(this);
        }

        /**
         * KeySet
         */
        class KeySet : java.util.AbstractSet<Object>
        {
            private readonly Flat3Map parent;

            internal KeySet(Flat3Map parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return parent.size();
            }

            public override void clear()
            {
                parent.clear();
            }

            public override bool contains(Object key)
            {
                return parent.containsKey(key);
            }

            public override bool remove(Object key)
            {
                bool result = parent.containsKey(key);
                parent.remove(key);
                return result;
            }

            public override java.util.Iterator<Object> iterator()
            {
                if (parent.delegateMap != null)
                {
                    return parent.delegateMap.keySet().iterator();
                }
                if (parent.size() == 0)
                {
                    return EmptyIterator.INSTANCE;
                }
                return new KeySetIterator(parent);
            }
        }

        /**
         * KeySetIterator
         */
        class KeySetIterator : EntrySetIterator
        {

            internal KeySetIterator(Flat3Map parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                base.next();
                return getKey();
            }
        }

        /**
         * Gets the values view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the values, use {@link #mapIterator()}.
         * 
         * @return the values view
         */
        public java.util.Collection<Object> values()
        {
            if (delegateMap != null)
            {
                return delegateMap.values();
            }
            return new Values(this);
        }

        /**
         * Values
         */
        class Values : java.util.AbstractCollection<Object>
        {
            private readonly Flat3Map parent;

            internal Values(Flat3Map parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return parent.size();
            }

            public override void clear()
            {
                parent.clear();
            }

            public override bool contains(Object value)
            {
                return parent.containsValue(value);
            }

            public override java.util.Iterator<Object> iterator()
            {
                if (parent.delegateMap != null)
                {
                    return parent.delegateMap.values().iterator();
                }
                if (parent.size() == 0)
                {
                    return EmptyIterator.INSTANCE;
                }
                return new ValuesIterator(parent);
            }
        }

        /**
         * ValuesIterator
         */
        class ValuesIterator : EntrySetIterator
        {

            internal ValuesIterator(Flat3Map parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                base.next();
                return getValue();
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Write the map out using a custom routine.
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeInt(size());
            for (MapIterator it = mapIterator(); it.hasNext(); )
            {
                outJ.writeObject(it.next());  // key
                outJ.writeObject(it.getValue());  // value
            }
        }

        /**
         * Read the map in using a custom routine.
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            int count = inJ.readInt();
            if (count > 3)
            {
                delegateMap = createDelegateMap();
            }
            for (int i = count; i > 0; i--)
            {
                put(inJ.readObject(), inJ.readObject());
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Clones the map without cloning the keys or values.
         *
         * @return a shallow clone
         * @since Commons Collections 3.1
         */
        public Object clone()
        {
            try
            {
                Flat3Map cloned = (Flat3Map)base.MemberwiseClone();
                if (cloned.delegateMap != null)
                {
                    cloned.delegateMap = (HashedMap)cloned.delegateMap.clone();
                }
                return cloned;
            }
            catch (java.lang.CloneNotSupportedException)
            {
                throw new java.lang.InternalError();
            }
        }

        /**
         * Compares this map with another.
         * 
         * @param obj  the object to compare to
         * @return true if equal
         */
        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (delegateMap != null)
            {
                return delegateMap.equals(obj);
            }
            if (obj is java.util.Map<Object, Object> == false)
            {
                return false;
            }
            java.util.Map<Object, Object> other = (java.util.Map<Object, Object>)obj;
            if (sizeJ != other.size())
            {
                return false;
            }
            if (sizeJ > 0)
            {
                Object otherValue = null;
                switch (sizeJ)
                {  // drop through
                    case 3:
                        if (other.containsKey(key3) == false)
                        {
                            return false;
                        }
                        otherValue = other.get(key3);
                        if (value3 == null ? otherValue != null : !value3.equals(otherValue))
                        {
                            return false;
                        }
                        if (other.containsKey(key2) == false)
                        {
                            return false;
                        }
                        otherValue = other.get(key2);
                        if (value2 == null ? otherValue != null : !value2.equals(otherValue))
                        {
                            return false;
                        }
                        if (other.containsKey(key1) == false)
                        {
                            return false;
                        }
                        otherValue = other.get(key1);
                        if (value1 == null ? otherValue != null : !value1.equals(otherValue))
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (other.containsKey(key2) == false)
                        {
                            return false;
                        }
                        otherValue = other.get(key2);
                        if (value2 == null ? otherValue != null : !value2.equals(otherValue))
                        {
                            return false;
                        }
                        if (other.containsKey(key1) == false)
                        {
                            return false;
                        }
                        otherValue = other.get(key1);
                        if (value1 == null ? otherValue != null : !value1.equals(otherValue))
                        {
                            return false;
                        }
                        break;
                    case 1:
                        if (other.containsKey(key1) == false)
                        {
                            return false;
                        }
                        otherValue = other.get(key1);
                        if (value1 == null ? otherValue != null : !value1.equals(otherValue))
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        /**
         * Gets the standard Map hashCode.
         * 
         * @return the hash code defined in the Map interface
         */
        public override int GetHashCode()
        {
            if (delegateMap != null)
            {
                return delegateMap.GetHashCode();
            }
            int total = 0;
            switch (sizeJ)
            {  // drop through
                case 3:
                    total += (hash3 ^ (value3 == null ? 0 : value3.GetHashCode()));
                    total += (hash2 ^ (value2 == null ? 0 : value2.GetHashCode()));
                    total += (hash1 ^ (value1 == null ? 0 : value1.GetHashCode()));
                    break;
                case 2:
                    total += (hash2 ^ (value2 == null ? 0 : value2.GetHashCode()));
                    total += (hash1 ^ (value1 == null ? 0 : value1.GetHashCode()));
                    break;
                case 1:
                    total += (hash1 ^ (value1 == null ? 0 : value1.GetHashCode()));
                    break;
            }
            return total;
        }

        /**
         * Gets the map as a String.
         * 
         * @return a string version of the map
         */
        public String toString()
        {
            if (delegateMap != null)
            {
                return delegateMap.toString();
            }
            if (sizeJ == 0)
            {
                return "{}";
            }
            java.lang.StringBuffer buf = new java.lang.StringBuffer(128);
            buf.append('{');
            switch (sizeJ)
            {  // drop through
                case 3:
                    buf.append((key3 == this ? "(this Map)" : key3));
                    buf.append('=');
                    buf.append((value3 == this ? "(this Map)" : value3));
                    buf.append(',');
                    buf.append((key2 == this ? "(this Map)" : key2));
                    buf.append('=');
                    buf.append((value2 == this ? "(this Map)" : value2));
                    buf.append(',');
                    buf.append((key1 == this ? "(this Map)" : key1));
                    buf.append('=');
                    buf.append((value1 == this ? "(this Map)" : value1));
                    break;
                case 2:
                    buf.append((key2 == this ? "(this Map)" : key2));
                    buf.append('=');
                    buf.append((value2 == this ? "(this Map)" : value2));
                    buf.append(',');
                    buf.append((key1 == this ? "(this Map)" : key1));
                    buf.append('=');
                    buf.append((value1 == this ? "(this Map)" : value1));
                    break;
                case 1:
                    buf.append((key1 == this ? "(this Map)" : key1));
                    buf.append('=');
                    buf.append((value1 == this ? "(this Map)" : value1));
                    break;
            }
            buf.append('}');
            return buf.toString();
        }

    }
}