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
using org.apache.commons.collections.keyvalue;

namespace org.apache.commons.collections.map
{

    /**
     * A <code>Map</code> implementation that holds a single item and is fixed size.
     * <p>
     * The single key/value pair is specified at creation.
     * The map is fixed size so any action that would change the size is disallowed.
     * However, the <code>put</code> or <code>setValue</code> methods can <i>change</i>
     * the value associated with the key.
     * <p>
     * If trying to remove or clear the map, an UnsupportedOperationException is thrown.
     * If trying to put a new mapping into the map, an  IllegalArgumentException is thrown.
     * The put method will only suceed if the key specified is the same as the 
     * singleton key.
     * <p>
     * The key and value can be obtained by:
     * <ul>
     * <li>normal Map methods and views
     * <li>the <code>MapIterator</code>, see {@link #mapIterator()}
     * <li>the <code>KeyValue</code> interface (just cast - no object creation)
     * </ul>
     * 
     * @since Commons Collections 3.1
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SingletonMap
            : OrderedMap, BoundedMap, KeyValue, java.io.Serializable, java.lang.Cloneable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -8931271118676803261L;

        /** Singleton key */
        private readonly Object key;
        /** Singleton value */
        private Object value;

        /**
         * Constructor that creates a map of <code>null</code> to <code>null</code>.
         */
        public SingletonMap()
            : base()
        {
            this.key = null;
        }

        /**
         * Constructor specifying the key and value.
         *
         * @param key  the key to use
         * @param value  the value to use
         */
        public SingletonMap(Object key, Object value)
            : base()
        {

            this.key = key;
            this.value = value;
        }

        /**
         * Constructor specifying the key and value as a <code>KeyValue</code>.
         *
         * @param keyValue  the key value pair to use
         */
        public SingletonMap(KeyValue keyValue)
            : base()
        {
            this.key = keyValue.getKey();
            this.value = keyValue.getValue();
        }

        /**
         * Constructor specifying the key and value as a <code>MapEntry</code>.
         *
         * @param mapEntry  the mapEntry to use
         */
        public SingletonMap(java.util.MapNS.Entry<Object, Object> mapEntry)
            : base()
        {

            this.key = mapEntry.getKey();
            this.value = mapEntry.getValue();
        }

        /**
         * Constructor copying elements from another map.
         *
         * @param map  the map to copy, must be size 1
         * @throws NullPointerException if the map is null
         * @throws IllegalArgumentException if the size is not 1
         */
        public SingletonMap(java.util.Map<Object, Object> map)
        {
            if (map.size() != 1)
            {
                throw new java.lang.IllegalArgumentException("The map size must be 1");
            }
            java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)map.entrySet().iterator().next();
            this.key = entry.getKey();
            this.value = entry.getValue();
        }

        // KeyValue
        //-----------------------------------------------------------------------
        /**
         * Gets the key.
         *
         * @return the key 
         */
        public virtual Object getKey()
        {
            return key;
        }

        /**
         * Gets the value.
         *
         * @return the value
         */
        public virtual Object getValue()
        {
            return value;
        }

        /**
         * Sets the value.
         *
         * @param value  the new value to set
         * @return the old value
         */
        public virtual Object setValue(Object value)
        {
            Object old = this.value;
            this.value = value;
            return old;
        }

        // BoundedMap
        //-----------------------------------------------------------------------
        /**
         * Is the map currently full, always true.
         *
         * @return true always
         */
        public virtual bool isFull()
        {
            return true;
        }

        /**
         * Gets the maximum size of the map, always 1.
         * 
         * @return 1 always
         */
        public virtual int maxSize()
        {
            return 1;
        }

        // Map
        //-----------------------------------------------------------------------
        /**
         * Gets the value mapped to the key specified.
         * 
         * @param key  the key
         * @return the mapped value, null if no match
         */
        public virtual Object get(Object key)
        {
            if (isEqualKey(key))
            {
                return value;
            }
            return null;
        }

        /**
         * Gets the size of the map, always 1.
         * 
         * @return the size of 1
         */
        public virtual int size()
        {
            return 1;
        }

        /**
         * Checks whether the map is currently empty, which it never is.
         * 
         * @return false always
         */
        public virtual bool isEmpty()
        {
            return false;
        }

        //-----------------------------------------------------------------------
        /**
         * Checks whether the map contains the specified key.
         * 
         * @param key  the key to search for
         * @return true if the map contains the key
         */
        public virtual bool containsKey(Object key)
        {
            return (isEqualKey(key));
        }

        /**
         * Checks whether the map contains the specified value.
         * 
         * @param value  the value to search for
         * @return true if the map contains the key
         */
        public virtual bool containsValue(Object value)
        {
            return (isEqualValue(value));
        }

        //-----------------------------------------------------------------------
        /**
         * Puts a key-value mapping into this map where the key must match the existing key.
         * <p>
         * An IllegalArgumentException is thrown if the key does not match as the map
         * is fixed size.
         * 
         * @param key  the key to set, must be the key of the map
         * @param value  the value to set
         * @return the value previously mapped to this key, null if none
         * @throws IllegalArgumentException if the key does not match
         */
        public virtual Object put(Object key, Object value)
        {
            if (isEqualKey(key))
            {
                return setValue(value);
            }
            throw new java.lang.IllegalArgumentException("Cannot put new key/value pair - Map is fixed size singleton");
        }

        /**
         * Puts the values from the specified map into this map.
         * <p>
         * The map must be of size 0 or size 1.
         * If it is size 1, the key must match the key of this map otherwise an
         * IllegalArgumentException is thrown.
         * 
         * @param map  the map to add, must be size 0 or 1, and the key must match
         * @throws NullPointerException if the map is null
         * @throws IllegalArgumentException if the key does not match
         */
        public virtual void putAll(java.util.Map<Object, Object> map)
        {
            switch (map.size())
            {
                case 0:
                    return;

                case 1:
                    java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)map.entrySet().iterator().next();
                    put(entry.getKey(), entry.getValue());
                    return;

                default:
                    throw new java.lang.IllegalArgumentException("The map size must be 0 or 1");
            }
        }

        /**
         * Unsupported operation.
         * 
         * @param key  the mapping to remove
         * @return the value mapped to the removed key, null if key not in map
         * @throws UnsupportedOperationException always
         */
        public virtual Object remove(Object key)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        /**
         * Unsupported operation.
         */
        public virtual void clear()
        {
            throw new java.lang.UnsupportedOperationException();
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the entrySet view of the map.
         * Changes made via <code>setValue</code> affect this map.
         * To simply iterate through the entries, use {@link #mapIterator()}.
         * 
         * @return the entrySet view
         */
        public virtual java.util.Set<java.util.MapNS.Entry<Object, Object>> entrySet()
        {
            java.util.MapNS.Entry<Object, Object> entry = new TiedMapEntry(this, getKey());
            return java.util.Collections.singleton(entry);
        }

        /**
         * Gets the unmodifiable keySet view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the keys, use {@link #mapIterator()}.
         * 
         * @return the keySet view
         */
        public virtual java.util.Set<Object> keySet()
        {
            return java.util.Collections.singleton(key);
        }

        /**
         * Gets the unmodifiable values view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the values, use {@link #mapIterator()}.
         * 
         * @return the values view
         */
        public virtual java.util.Collection<Object> values()
        {
            return new SingletonValues(this);
        }

        /**
         * Gets an iterator over the map.
         * Changes made to the iterator using <code>setValue</code> affect this map.
         * The <code>remove</code> method is unsupported.
         * <p>
         * A MapIterator returns the keys in the map. It also provides convenient
         * methods to get the key and value, and set the value.
         * It avoids the need to create an entrySet/keySet/values object.
         * It also avoids creating the Map Entry object.
         * 
         * @return the map iterator
         */
        public virtual MapIterator mapIterator()
        {
            return new SingletonMapIterator(this);
        }

        // OrderedMap
        //-----------------------------------------------------------------------
        /**
         * Obtains an <code>OrderedMapIterator</code> over the map.
         * <p>
         * A ordered map iterator is an efficient way of iterating over maps
         * in both directions.
         * 
         * @return an ordered map iterator
         */
        public virtual OrderedMapIterator orderedMapIterator()
        {
            return new SingletonMapIterator(this);
        }

        /**
         * Gets the first (and only) key in the map.
         * 
         * @return the key
         */
        public virtual Object firstKey()
        {
            return getKey();
        }

        /**
         * Gets the last (and only) key in the map.
         * 
         * @return the key
         */
        public virtual Object lastKey()
        {
            return getKey();
        }

        /**
         * Gets the next key after the key specified, always null.
         * 
         * @param key  the next key
         * @return null always
         */
        public virtual Object nextKey(Object key)
        {
            return null;
        }

        /**
         * Gets the previous key before the key specified, always null.
         * 
         * @param key  the next key
         * @return null always
         */
        public virtual Object previousKey(Object key)
        {
            return null;
        }

        //-----------------------------------------------------------------------
        /**
         * Compares the specified key to the stored key.
         * 
         * @param key  the key to compare
         * @return true if equal
         */
        protected virtual bool isEqualKey(Object key)
        {
            return (key == null ? getKey() == null : key.equals(getKey()));
        }

        /**
         * Compares the specified value to the stored value.
         * 
         * @param value  the value to compare
         * @return true if equal
         */
        protected virtual bool isEqualValue(Object value)
        {
            return (value == null ? getValue() == null : value.equals(getValue()));
        }

        //-----------------------------------------------------------------------
        /**
         * SingletonMapIterator.
         */
        internal class SingletonMapIterator : OrderedMapIterator, ResettableIterator
        {
            private readonly SingletonMap parent;
            private bool hasNextJ = true;
            private bool canGetSet = false;

            internal SingletonMapIterator(SingletonMap parent)
                : base()
            {
                this.parent = parent;
            }

            public virtual bool hasNext()
            {
                return hasNextJ;
            }

            public virtual Object next()
            {
                if (hasNextJ == false)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_NEXT_ENTRY);
                }
                hasNextJ = false;
                canGetSet = true;
                return parent.getKey();
            }

            public virtual bool hasPrevious()
            {
                return (hasNextJ == false);
            }

            public virtual Object previous()
            {
                if (hasNextJ == true)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_PREVIOUS_ENTRY);
                }
                hasNextJ = true;
                return parent.getKey();
            }

            public virtual void remove()
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public virtual Object getKey()
            {
                if (canGetSet == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETKEY_INVALID);
                }
                return parent.getKey();
            }

            public virtual Object getValue()
            {
                if (canGetSet == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETVALUE_INVALID);
                }
                return parent.getValue();
            }

            public virtual Object setValue(Object value)
            {
                if (canGetSet == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.SETVALUE_INVALID);
                }
                return parent.setValue(value);
            }

            public virtual void reset()
            {
                hasNextJ = true;
            }

            public override String ToString()
            {
                if (hasNextJ)
                {
                    return "Iterator[]";
                }
                else
                {
                    return "Iterator[" + getKey() + "=" + getValue() + "]";
                }
            }
        }

        /**
         * Values implementation for the SingletonMap.
         * This class is needed as values is a view that must update as the map updates.
         */
        [Serializable]
        internal class SingletonValues : java.util.AbstractSet<Object>, java.io.Serializable
        {
            private static readonly long serialVersionUID = -3689524741863047872L;
            private readonly SingletonMap parent;

            internal SingletonValues(SingletonMap parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return 1;
            }
            public override bool isEmpty()
            {
                return false;
            }
            public override bool contains(Object obj)
            {
                return parent.containsValue(obj);
            }
            public override void clear()
            {
                throw new java.lang.UnsupportedOperationException();
            }
            public override java.util.Iterator<Object> iterator()
            {
                return new SingletonIterator(parent.getValue(), false);
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Clones the map without cloning the key or value.
         *
         * @return a shallow clone
         */
        public virtual Object clone()
        {
            try
            {
                SingletonMap cloned = (SingletonMap)base.MemberwiseClone();
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
            if (obj is java.util.Map<Object, Object> == false)
            {
                return false;
            }
            java.util.Map<Object, Object> other = (java.util.Map<Object, Object>)obj;
            if (other.size() != 1)
            {
                return false;
            }
            java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)other.entrySet().iterator().next();
            return isEqualKey(entry.getKey()) && isEqualValue(entry.getValue());
        }

        /**
         * Gets the standard Map hashCode.
         * 
         * @return the hash code defined in the Map interface
         */
        public override int GetHashCode()
        {
            return (getKey() == null ? 0 : getKey().GetHashCode()) ^
                   (getValue() == null ? 0 : getValue().GetHashCode());
        }

        /**
         * Gets the map as a String.
         * 
         * @return a string version of the map
         */
        public override String ToString()
        {
            return new System.Text.StringBuilder()
                .Append('{')
                .Append((getKey() == this ? "(this Map)" : getKey()))
                .Append('=')
                .Append((getValue() == this ? "(this Map)" : getValue()))
                .Append('}')
                .ToString();
        }

    }
}