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
using System.Collections.Generic;
using System.Collections;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Implementation details:
    /// <code>
    ///  +-Class--------------+
    ///  |     Object         |----------------------------------------------------+
    ///  +--------------------+                                                    |
    ///           |            +-Interface-+                                       |
    ///         Java           | Map       |                                     .net
    ///           |            +-----------+                                       |
    ///  +-Class--------------+     |                                              |
    ///  |  AbstractMap       |-----+                                              |         
    ///  +--------------------+     |                       +-Interface---+        |         
    ///           |              JavApi                     | IDictionary |        |         
    ///           |                 |                       +-------------+        |
    ///  +--------------------+     |                              |      +-Class-----------+       
    ///  | HashMap            |     +------------------------------+------|    Dictionary   |
    ///  +--------------------+                                           +-----------------+
    /// </code>
    /// </remarks>
    public class AbstractMap<K, V> : System.Collections.Generic.Dictionary<K, V>, java.util.Map<K, V>
    {

        protected Set<K> keySetJ; 
        protected internal Collection<V> valuesCollection;

        /**
         * An immutable key-value mapping.
         * 
         * @param <K>
         *            the type of key
         * @param <V>
         *            the type of value
         * 
         * @since 1.6
         */
        [Serializable]
        public class SimpleImmutableEntry<K, V> : MapNS.Entry<K, V>,
                java.io.Serializable
        {

            private static readonly long serialVersionUID = 7138329143949025153L;

            private K key;

            private V value;

            /**
             * Constructs a new instance by key and value.
             * 
             * @param theKey
             *            the key
             * @param theValue
             *            the value
             */
            public SimpleImmutableEntry(K theKey, V theValue)
            {
                key = theKey;
                value = theValue;
            }

            /**
             * Constructs a new instance by an entry
             * 
             * @param entry
             *            the entry
             */
            public SimpleImmutableEntry(MapNS.Entry<K, V> entry)
            {
                key = entry.getKey();
                value = entry.getValue();
            }

            /**
             * {@inheritDoc}
             * 
             * @see java.util.Map.Entry#getKey()
             */
            public K getKey()
            {
                return key;
            }

            /**
             * {@inheritDoc}
             * 
             * @see java.util.Map.Entry#getValue()
             */
            public V getValue()
            {
                return value;
            }

            /**
             * Throws an UnsupportedOperationException.
             * 
             * @param object
             *            new value
             * @return (Does not)
             * @throws UnsupportedOperationException
             *             always
             * 
             * @see java.util.Map.Entry#setValue(java.lang.Object)
             */
            public V setValue(V obj)
            {
                throw new java.lang.UnsupportedOperationException();
            }

            /**
             * Answers whether the object is equal to this entry. This works across
             * all kinds of the Map.Entry interface.
             * 
             * @see java.lang.Object#equals(java.lang.Object)
             */
            public override bool Equals(Object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj is MapNS.Entry<Object, Object>)
                {
                    MapNS.Entry<Object, Object> entry = (MapNS.Entry<Object, Object>)obj;
                    return (key == null ? entry.getKey() == null : key.equals(entry
                            .getKey()))
                            && (value == null ? entry.getValue() == null : value
                                    .equals(entry.getValue()));
                }
                return false;
            }

            /**
             * Answers the hash code of this entry.
             * 
             * @see java.lang.Object#hashCode()
             */

            public override int GetHashCode()
            {
                return (key == null ? 0 : key.GetHashCode())
                        ^ (value == null ? 0 : value.GetHashCode());
            }

            /**
             * Answers a String representation of this entry.
             * 
             * @see java.lang.Object#toString()
             */

            public override String ToString()
            {
                return key + "=" + value; //$NON-NLS-1$
            }
        }

        public AbstractMap()
        {
        }

        #region java.util.Map<K,V>
        public virtual void clear()
        {
            this.Clear();
        }
        public virtual bool containsKey(Object key)
        {
            if (key is K)
            {
                return ContainsKey((K)key);
            }
            return false;
        }
        public virtual bool containsValue(Object value)
        {
            if (value is V) return ContainsValue((V)value);
            return false;
        }
        public virtual V get(Object key)
        {
            V returnValue = default(V);
            if (key is K) this.TryGetValue((K)key, out returnValue);
            return returnValue;
        }
        public virtual bool isEmpty()
        {
            return 0 == size();
        }
        public virtual V put(K key, V value)
        {
            V returnValue = this.get(key);
            this.Add(key, value);
            return returnValue;
        }
        public virtual V remove(Object key)
        {

            V returnValue = default(V);
            if (key is K)
            {
                returnValue = this.get(key);
                this.Remove((K)key);
            }
            return returnValue;
        }
        public virtual int size()
        {
            return this.Count;
        }
        public virtual void putAll(Map<K, V> map)
        {
            Iterator<K> it = map.keySet().iterator();
            while (it.hasNext())
            {
                K next = it.next();
                this.Add(next, map.get(next));
            }
        }
        public virtual Collection<V> values()
        {
            ArrayList<V> valueCollection = new ArrayList<V>(this.size());
            System.Collections.Generic.Dictionary<K, V>.ValueCollection dotNetCollection = this.Values;
            foreach (V value in dotNetCollection)
            {
                valueCollection.add(value);
            }
            return valueCollection;
        }
        public virtual Set<K> keySet()
        {
            DefaultSet<K> setCollection = new DefaultSet<K>();
            foreach (K key in this.Keys)
            {
                setCollection.add(key);
            }
            return setCollection;
        }
        public virtual Set<MapNS.Entry<K, V>> entrySet()
        {
            DefaultSet<MapNS.DefaultEntry<K, V>> setCollection = new DefaultSet<MapNS.DefaultEntry<K, V>>();
            foreach (K key in this.Keys)
            {
                V v = default(V);
                this.TryGetValue(key, out v);
                MapNS.DefaultEntry<K, V> e = new MapNS.DefaultEntry<K, V>(key, v);
                setCollection.add(e);
            }
            Set<MapNS.Entry<K, V>> returnValue = (Set<MapNS.Entry<K, V>>)setCollection;
            return returnValue;
        }
        #endregion
    }
    #region DefaultSet
    internal class DefaultSet<T> : AbstractSet<T>
    {
        private ArrayList<T> collection;
        public DefaultSet()
        {
            this.collection = new ArrayList<T>();
        }

        public override bool add(T obj)
        {
            if (this.contains(obj)) return false;
            return collection.add(obj);
        }
        public override bool addAll(Collection<T> c)
        {
            bool returnValue = false;
            Iterator<T> it = c.iterator();
            while (it.hasNext())
            {
                if (this.add(it.next()))
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }
        public override void clear()
        {
            this.collection.clear();
        }
        public override bool contains(object obj)
        {
            return this.collection.contains(obj);
        }
        public override bool containsAll(Collection<T> collection)
        {
            return this.collection.containsAll(collection);
        }
        public override bool isEmpty()
        {
            return this.collection.isEmpty();
        }
        public override bool remove(object obj)
        {
            return this.collection.remove(obj);
        }
        public override bool removeAll(Collection<T> collection)
        {
            return this.collection.removeAll(collection);
        }
        public override bool retainAll(Collection<T> collection)
        {
            return this.collection.retainAll(collection);
        }
        public override T[] toArray<T>(T[] contents)
        {
            return this.collection.toArray<T>(contents);
        }
        public override object[] toArray()
        {
            return this.collection.toArray();
        }
        public override int size()
        {
            return this.collection.size();
        }

        public override Iterator<T> iterator()
        {
            return this.collection.iterator();
        }
    }
    #endregion

    namespace MapNS
    {
        internal class DefaultEntry<K, V> : MapNS.Entry<K, V>
        {

            private V value;
            private readonly K key;

            public DefaultEntry(K newKey, V newValue)
            {
                this.key = newKey;
                this.setValue(newValue);
            }
            public V getValue()
            {
                return this.value;
            }
            public K getKey()
            {
                return this.key;
            }
            public V setValue(V newValue)
            {
                V oldValue = this.value;
                this.value = newValue;
                return oldValue;
            }
        }
    }
}