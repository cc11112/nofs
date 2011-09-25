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

namespace org.apache.commons.collections
{

    /**
     * <p>A customized implementation of <code>java.util.TreeMap</code> designed
     * to operate in a multithreaded environment where the large majority of
     * method calls are read-only, instead of structural changes.  When operating
     * in "fast" mode, read calls are non-lock and write calls perform the
     * following steps:</p>
     * <ul>
     * <li>Clone the existing collection
     * <li>Perform the modification on the clone
     * <li>Replace the existing collection with the (modified) clone
     * </ul>
     * <p>When first created, objects of this class default to "slow" mode, where
     * all accesses of any type are lock but no cloning takes place.  This
     * is appropriate for initially populating the collection, followed by a switch
     * to "fast" mode (by calling <code>setFast(true)</code>) after initialization
     * is complete.</p>
     *
     * <p><strong>NOTE</strong>: If you are creating and accessing a
     * <code>TreeMap</code> only within a single thread, you should use
     * <code>java.util.TreeMap</code> directly (with no synchronization), for
     * maximum performance.</p>
     *
     * <p><strong>NOTE</strong>: <i>This class is not cross-platform.  
     * Using it may cause unexpected failures on some architectures.</i>
     * It suffers from the same problems as the double-checked locking idiom.  
     * In particular, the instruction that clones the internal collection and the 
     * instruction that sets the internal reference to the clone can be executed 
     * or perceived out-of-order.  This means that any read operation might fail 
     * unexpectedly, as it may be reading the state of the internal collection
     * before the internal collection is fully formed.
     * For more information on the double-checked locking idiom, see the
     * <a href="http://www.cs.umd.edu/~pugh/java/memoryModel/DoubleCheckedLocking.html">
     * Double-Checked Locking Idiom Is Broken Declaration</a>.</p>
     *
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author Craig R. McClanahan
     * @author Stephen Colebourne
     */
    public class FastTreeMap : java.util.TreeMap<Object, Object>
    {

        /**
         * The underlying map we are managing.
         */
        protected java.util.TreeMap<Object, Object> map = null;

        /**
         * Are we operating in "fast" mode?
         */
        protected bool fast = false;


        // Constructors
        // ----------------------------------------------------------------------

        /**
         * Construct a an empty map.
         */
        public FastTreeMap()
            : base()
        {
            this.map = new java.util.TreeMap<Object, Object>();
        }

        /**
         * Construct an empty map with the specified comparator.
         *
         * @param comparator  the comparator to use for ordering tree elements
         */
        public FastTreeMap(java.util.Comparator<Object> comparator)
            : base()
        {
            this.map = new java.util.TreeMap<Object, Object>(comparator);
        }

        /**
         * Construct a new map with the same mappings as the specified map,
         * sorted according to the keys's natural order
         *
         * @param map  the map whose mappings are to be copied
         */
        public FastTreeMap(java.util.Map<Object, Object> map)
            : base()
        {
            this.map = new java.util.TreeMap<Object, Object>(map);
        }

        /**
         * Construct a new map with the same mappings as the specified map,
         * sorted according to the same ordering
         *
         * @param map  the map whose mappings are to be copied
         */
        public FastTreeMap(java.util.SortedMap<Object, Object> map)
            : base()
        {
            this.map = new java.util.TreeMap<Object, Object>(map);
        }


        // Property access
        // ----------------------------------------------------------------------

        /**
         *  Returns true if this map is operating in fast mode.
         *
         *  @return true if this map is operating in fast mode
         */
        public bool getFast()
        {
            return (this.fast);
        }

        /**
         *  Sets whether this map is operating in fast mode.
         *
         *  @param fast true if this map should operate in fast mode
         */
        public void setFast(bool fast)
        {
            this.fast = fast;
        }


        // Map access
        // ----------------------------------------------------------------------
        // These methods can forward straight to the wrapped Map in 'fast' mode.
        // (because they are query methods)

        /**
         * Return the value to which this map maps the specified key.  Returns
         * <code>null</code> if the map contains no mapping for this key, or if
         * there is a mapping with a value of <code>null</code>.  Use the
         * <code>containsKey()</code> method to disambiguate these cases.
         *
         * @param key  the key whose value is to be returned
         * @return the value mapped to that key, or null
         */
        public override Object get(Object key)
        {
            if (fast)
            {
                return (map.get(key));
            }
            else
            {
                lock (map)
                {
                    return (map.get(key));
                }
            }
        }

        /**
         * Return the number of key-value mappings in this map.
         * 
         * @return the current size of the map
         */
        public override int size()
        {
            if (fast)
            {
                return (map.size());
            }
            else
            {
                lock (map)
                {
                    return (map.size());
                }
            }
        }

        /**
         * Return <code>true</code> if this map contains no mappings.
         * 
         * @return is the map currently empty
         */
        public override bool isEmpty()
        {
            if (fast)
            {
                return (map.isEmpty());
            }
            else
            {
                lock (map)
                {
                    return (map.isEmpty());
                }
            }
        }

        /**
         * Return <code>true</code> if this map contains a mapping for the
         * specified key.
         *
         * @param key  the key to be searched for
         * @return true if the map contains the key
         */
        public override bool containsKey(Object key)
        {
            if (fast)
            {
                return (map.containsKey(key));
            }
            else
            {
                lock (map)
                {
                    return (map.containsKey(key));
                }
            }
        }

        /**
         * Return <code>true</code> if this map contains one or more keys mapping
         * to the specified value.
         *
         * @param value  the value to be searched for
         * @return true if the map contains the value
         */
        public override bool containsValue(Object value)
        {
            if (fast)
            {
                return (map.containsValue(value));
            }
            else
            {
                lock (map)
                {
                    return (map.containsValue(value));
                }
            }
        }

        /**
         * Return the comparator used to order this map, or <code>null</code>
         * if this map uses its keys' natural order.
         * 
         * @return the comparator used to order the map, or null if natural order
         */
        public override java.util.Comparator<Object> comparator()
        {
            if (fast)
            {
                return (map.comparator());
            }
            else
            {
                lock (map)
                {
                    return (map.comparator());
                }
            }
        }

        /**
         * Return the first (lowest) key currently in this sorted map.
         * 
         * @return the first key in the map
         */
        public override Object firstKey()
        {
            if (fast)
            {
                return (map.firstKey());
            }
            else
            {
                lock (map)
                {
                    return (map.firstKey());
                }
            }
        }

        /**
         * Return the last (highest) key currently in this sorted map.
         * 
         * @return the last key in the map
         */
        public override Object lastKey()
        {
            if (fast)
            {
                return (map.lastKey());
            }
            else
            {
                lock (map)
                {
                    return (map.lastKey());
                }
            }
        }


        // Map modification
        // ----------------------------------------------------------------------
        // These methods perform special behaviour in 'fast' mode.
        // The map is cloned, updated and then assigned back.
        // See the comments at the top as to why this won't always work.

        /**
         * Associate the specified value with the specified key in this map.
         * If the map previously contained a mapping for this key, the old
         * value is replaced and returned.
         *
         * @param key  the key with which the value is to be associated
         * @param value  the value to be associated with this key
         * @return the value previously mapped to the key, or null
         */
        public override Object put(Object key, Object value)
        {
            if (fast)
            {
                lock (this)
                {
                    java.util.TreeMap<Object, Object> temp = (java.util.TreeMap<Object, Object>)map.clone();
                    Object result = temp.put(key, value);
                    map = temp;
                    return (result);
                }
            }
            else
            {
                lock (map)
                {
                    return (map.put(key, value));
                }
            }
        }

        /**
         * Copy all of the mappings from the specified map to this one, replacing
         * any mappings with the same keys.
         *
         * @param in  the map whose mappings are to be copied
         */
        public override void putAll(java.util.Map<Object, Object> inJ)
        {
            if (fast)
            {
                lock (this)
                {
                    java.util.TreeMap<Object, Object> temp = (java.util.TreeMap<Object, Object>)map.clone();
                    temp.putAll(inJ);
                    map = temp;
                }
            }
            else
            {
                lock (map)
                {
                    map.putAll(inJ);
                }
            }
        }

        /**
         * Remove any mapping for this key, and return any previously
         * mapped value.
         *
         * @param key  the key whose mapping is to be removed
         * @return the value removed, or null
         */
        public override Object remove(Object key)
        {
            if (fast)
            {
                lock (this)
                {
                    java.util.TreeMap<Object, Object> temp = (java.util.TreeMap<Object, Object>)map.clone();
                    Object result = temp.remove(key);
                    map = temp;
                    return (result);
                }
            }
            else
            {
                lock (map)
                {
                    return (map.remove(key));
                }
            }
        }

        /**
         * Remove all mappings from this map.
         */
        public override void clear()
        {
            if (fast)
            {
                lock (this)
                {
                    map = new java.util.TreeMap<Object, Object>();
                }
            }
            else
            {
                lock (map)
                {
                    map.clear();
                }
            }
        }


        // Basic object methods
        // ----------------------------------------------------------------------

        /**
         * Compare the specified object with this list for equality.  This
         * implementation uses exactly the code that is used to define the
         * list equals function in the documentation for the
         * <code>Map.equals</code> method.
         *
         * @param o  the object to be compared to this list
         * @return true if the two maps are equal
         */
        public override bool Equals(Object o)
        {
            // Simple tests that require no synchronization
            if (o == this)
            {
                return (true);
            }
            else if (!(o is java.util.Map<Object, Object>))
            {
                return (false);
            }
            java.util.Map<Object, Object> mo = (java.util.Map<Object, Object>)o;

            // Compare the two maps for equality
            if (fast)
            {
                if (mo.size() != map.size())
                {
                    return (false);
                }
                java.util.Iterator<java.util.MapNS.Entry<Object, Object>> i = map.entrySet().iterator();
                while (i.hasNext())
                {
                    java.util.MapNS.Entry<Object, Object> e = (java.util.MapNS.Entry<Object, Object>)i.next();
                    Object key = e.getKey();
                    Object value = e.getValue();
                    if (value == null)
                    {
                        if (!(mo.get(key) == null && mo.containsKey(key)))
                        {
                            return (false);
                        }
                    }
                    else
                    {
                        if (!value.equals(mo.get(key)))
                        {
                            return (false);
                        }
                    }
                }
                return (true);
            }
            else
            {
                lock (map)
                {
                    if (mo.size() != map.size())
                    {
                        return (false);
                    }
                    java.util.Iterator<java.util.MapNS.Entry<Object, Object>> i = map.entrySet().iterator();
                    while (i.hasNext())
                    {
                        java.util.MapNS.Entry<Object, Object> e = (java.util.MapNS.Entry<Object, Object>)i.next();
                        Object key = e.getKey();
                        Object value = e.getValue();
                        if (value == null)
                        {
                            if (!(mo.get(key) == null && mo.containsKey(key)))
                            {
                                return (false);
                            }
                        }
                        else
                        {
                            if (!value.equals(mo.get(key)))
                            {
                                return (false);
                            }
                        }
                    }
                    return (true);
                }
            }
        }

        /**
         * Return the hash code value for this map.  This implementation uses
         * exactly the code that is used to define the list hash function in the
         * documentation for the <code>Map.hashCode</code> method.
         * 
         * @return a suitable integer hash code
         */
        public override int GetHashCode()
        {
            if (fast)
            {
                int h = 0;
                java.util.Iterator<java.util.MapNS.Entry<Object, Object>> i = map.entrySet().iterator();
                while (i.hasNext())
                {
                    h += i.next().GetHashCode();
                }
                return (h);
            }
            else
            {
                lock (map)
                {
                    int h = 0;
                    java.util.Iterator<java.util.MapNS.Entry<Object, Object>> i = map.entrySet().iterator();
                    while (i.hasNext())
                    {
                        h += i.next().GetHashCode();
                    }
                    return (h);
                }
            }
        }

        /**
         * Return a shallow copy of this <code>FastTreeMap</code> instance.
         * The keys and values themselves are not copied.
         * 
         * @return a clone of this map
         */
        public override Object clone()
        {
            FastTreeMap results = null;
            if (fast)
            {
                results = new FastTreeMap(map);
            }
            else
            {
                lock (map)
                {
                    results = new FastTreeMap(map);
                }
            }
            results.setFast(getFast());
            return (results);
        }


        // Sub map views
        // ----------------------------------------------------------------------

        /**
         * Return a view of the portion of this map whose keys are strictly
         * less than the specified key.
         *
         * @param key Key higher than any in the returned map
         * @return a head map
         */
        public override java.util.SortedMap<Object, Object> headMap(Object key)
        {
            if (fast)
            {
                return (map.headMap(key));
            }
            else
            {
                lock (map)
                {
                    return (map.headMap(key));
                }
            }
        }

        /**
         * Return a view of the portion of this map whose keys are in the
         * range fromKey (inclusive) to toKey (exclusive).
         *
         * @param fromKey Lower limit of keys for the returned map
         * @param toKey Upper limit of keys for the returned map
         * @return a sub map
         */
        public override java.util.SortedMap<Object, Object> subMap(Object fromKey, Object toKey)
        {
            if (fast)
            {
                return (map.subMap(fromKey, toKey));
            }
            else
            {
                lock (map)
                {
                    return (map.subMap(fromKey, toKey));
                }
            }
        }

        /**
         * Return a view of the portion of this map whose keys are greater than
         * or equal to the specified key.
         *
         * @param key Key less than or equal to any in the returned map
         * @return a tail map
         */
        public override java.util.SortedMap<Object, Object> tailMap(Object key)
        {
            if (fast)
            {
                return (map.tailMap(key));
            }
            else
            {
                lock (map)
                {
                    return (map.tailMap(key));
                }
            }
        }


        // Map views
        // ----------------------------------------------------------------------

        /**
         * Return a collection view of the mappings contained in this map.  Each
         * element in the returned collection is a <code>Map.Entry</code>.
         */
        public override java.util.Set<java.util.MapNS.Entry<Object, Object>> entrySet()
        {
            return (java.util.Set<java.util.MapNS.Entry<Object, Object>>)new EntrySet(this);
        }

        /**
         * Return a set view of the keys contained in this map.
         */
        public override java.util.Set<Object> keySet()
        {
            return new KeySet(this);
        }

        /**
         * Return a collection view of the values contained in this map.
         */
        public override java.util.Collection<Object> values()
        {
            return new Values(this);
        }

        // Map view inner classes
        // ----------------------------------------------------------------------

        /**
         * Abstract collection implementation shared by keySet(), values() and entrySet().
         */
        private abstract class CollectionView : java.util.Collection<Object>
        {
            protected internal readonly FastTreeMap root;
            public CollectionView(FastTreeMap root)
            {
                this.root = root;
            }

            protected abstract java.util.Collection<Object> get(java.util.Map<Object, Object> map);
            protected abstract Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry);


            public virtual void clear()
            {
                if (root.fast)
                {
                    lock (root)
                    {
                        root.map = new java.util.TreeMap<Object, Object>();
                    }
                }
                else
                {
                    lock (root.map)
                    {
                        get(root.map).clear();
                    }
                }
            }

            public virtual bool remove(Object o)
            {
                if (root.fast)
                {
                    lock (root)
                    {
                        java.util.TreeMap<Object, Object> temp = (java.util.TreeMap<Object, Object>)root.map.clone();
                        bool r = get(temp).remove(o);
                        root.map = temp;
                        return r;
                    }
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).remove(o);
                    }
                }
            }

            public virtual bool removeAll(java.util.Collection<Object> o)
            {
                if (root.fast)
                {
                    lock (root)
                    {
                        java.util.TreeMap<Object, Object> temp = (java.util.TreeMap<Object, Object>)root.map.clone();
                        bool r = get(temp).removeAll(o);
                        root.map = temp;
                        return r;
                    }
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).removeAll(o);
                    }
                }
            }

            public virtual bool retainAll(java.util.Collection<Object> o)
            {
                if (root.fast)
                {
                    lock (root)
                    {
                        java.util.TreeMap<Object, Object> temp = (java.util.TreeMap<Object, Object>)root.map.clone();
                        bool r = get(temp).retainAll(o);
                        root.map = temp;
                        return r;
                    }
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).retainAll(o);
                    }
                }
            }

            public virtual int size()
            {
                if (root.fast)
                {
                    return get(root.map).size();
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).size();
                    }
                }
            }


            public virtual bool isEmpty()
            {
                if (root.fast)
                {
                    return get(root.map).isEmpty();
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).isEmpty();
                    }
                }
            }

            public virtual bool contains(Object o)
            {
                if (root.fast)
                {
                    return get(root.map).contains(o);
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).contains(o);
                    }
                }
            }

            public virtual bool containsAll(java.util.Collection<Object> o)
            {
                if (root.fast)
                {
                    return get(root.map).containsAll(o);
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).containsAll(o);
                    }
                }
            }

            public virtual Object[] toArray<Object>(Object[] o)
            {
                if (root.fast)
                {
                    return get(root.map).toArray(o);
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).toArray(o);
                    }
                }
            }

            public virtual Object[] toArray()
            {
                if (root.fast)
                {
                    return get(root.map).toArray();
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).toArray();
                    }
                }
            }


            public override bool Equals(Object o)
            {
                if (o == this) return true;
                if (root.fast)
                {
                    return get(root.map).equals(o);
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).equals(o);
                    }
                }
            }

            public override int GetHashCode()
            {
                if (root.fast)
                {
                    return get(root.map).GetHashCode();
                }
                else
                {
                    lock (root.map)
                    {
                        return get(root.map).GetHashCode();
                    }
                }
            }

            public bool add(Object o)
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public bool addAll(java.util.Collection<Object> c)
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public java.util.Iterator<Object> iterator()
            {
                return new CollectionViewIterator(this);
            }

            private class CollectionViewIterator : java.util.Iterator<Object>
            {

                private CollectionView root;

                private java.util.Map<Object, Object> expected;
                private java.util.MapNS.Entry<Object, Object> lastReturned = null;
                private java.util.Iterator<java.util.MapNS.Entry<Object, Object>> iterator;

                public CollectionViewIterator(CollectionView root)
                {
                    this.root = root;
                    this.expected = root.root.map;
                    this.iterator = expected.entrySet().iterator();
                }

                public bool hasNext()
                {
                    if (expected != root.root.map)
                    {
                        throw new java.util.ConcurrentModificationException();
                    }
                    return iterator.hasNext();
                }

                public Object next()
                {
                    if (expected != root.root.map)
                    {
                        throw new java.util.ConcurrentModificationException();
                    }
                    lastReturned = (java.util.MapNS.Entry<Object, Object>)iterator.next();
                    return root.iteratorNext(lastReturned);
                }

                public void remove()
                {
                    if (lastReturned == null)
                    {
                        throw new java.lang.IllegalStateException();
                    }
                    if (root.root.fast)
                    {
                        lock (root)
                        {
                            if (expected != root.root.map)
                            {
                                throw new java.util.ConcurrentModificationException();
                            }
                            root.remove(lastReturned.getKey());
                            lastReturned = null;
                            expected = root.root.map;
                        }
                    }
                    else
                    {
                        iterator.remove();
                        lastReturned = null;
                    }
                }
            }
        }

        /**
         * Set implementation over the keys of the FastTreeMap
         */
        private class KeySet : CollectionView, java.util.Set<Object>
        {

            public KeySet(FastTreeMap ftm) : base(ftm) { }
            protected override java.util.Collection<Object> get(java.util.Map<Object, Object> map)
            {
                return map.keySet();
            }

            protected override Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry)
            {
                return entry.getKey();
            }

        }

        /**
         * Collection implementation over the values of the FastTreeMap
         */
        private new class Values : CollectionView
        {

            public Values(FastTreeMap ftm) : base(ftm) { }
            protected override java.util.Collection<Object> get(java.util.Map<Object, Object> map)
            {
                return map.values();
            }

            protected override Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry)
            {
                return entry.getValue();
            }
        }

        /**
         * Set implementation over the entries of the FastTreeMap
         */
        private class EntrySet : CollectionView, java.util.Set<Object>
        {

            public EntrySet (FastTreeMap ftm) : base(ftm){}

            protected override java.util.Collection<Object> get(java.util.Map<Object, Object> map)
            {
                return (java.util.Collection<Object>) root.map.entrySet();
            }


            protected override Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry)
            {
                return entry;
            }

        }

    }
}