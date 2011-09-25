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
     * <p>A customized implementation of <code>java.util.HashMap</code> designed
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
     * <code>HashMap</code> only within a single thread, you should use
     * <code>java.util.HashMap</code> directly (with no synchronization), for
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
    public class FastHashMap : java.util.HashMap<Object, Object>
    {

        /**
         * The underlyingjava.util.Map<Object,Object>we are managing.
         */
        protected java.util.HashMap<Object, Object> map = null;

        /**
         * Are we currently operating in "fast" mode?
         */
        protected bool fast = false;

        // Constructors
        // ----------------------------------------------------------------------

        /**
         * Construct an empty map.
         */
        public FastHashMap()
            : base()
        {
            this.map = new java.util.HashMap<Object, Object>();
        }

        /**
         * Construct an emptyjava.util.Map<Object,Object>with the specified capacity.
         *
         * @param capacity  the initial capacity of the empty map
         */
        public FastHashMap(int capacity)
            : base()
        {
            this.map = new java.util.HashMap<Object, Object>(capacity);
        }

        /**
         * Construct an emptyjava.util.Map<Object,Object>with the specified capacity and load factor.
         *
         * @param capacity  the initial capacity of the empty map
         * @param factor  the load factor of the new map
         */
        public FastHashMap(int capacity, float factor)
            : base()
        {
            this.map = new java.util.HashMap<Object, Object>(capacity, factor);
        }

        /**
         * Construct a newjava.util.Map<Object,Object>with the same mappings as the specified map.
         *
         * @paramjava.util.Map<Object,Object> thejava.util.Map<Object,Object>whose mappings are to be copied
         */
        public FastHashMap(java.util.Map<Object, Object> map)
            : base()
        {
            this.map = new java.util.HashMap<Object, Object>(map);
        }


        // Property access
        // ----------------------------------------------------------------------

        /**
         *  Returns true if thisjava.util.Map<Object,Object>is operating in fast mode.
         *
         *  @return true if thisjava.util.Map<Object,Object>is operating in fast mode
         */
        public bool getFast()
        {
            return (this.fast);
        }

        /**
         *  Sets whether thisjava.util.Map<Object,Object>is operating in fast mode.
         *
         *  @param fast true if thisjava.util.Map<Object,Object>should operate in fast mode
         */
        public void setFast(bool fast)
        {
            this.fast = fast;
        }


        //java.util.Map<Object,Object>access
        // ----------------------------------------------------------------------
        // These methods can forward straight to the wrappedjava.util.Map<Object,Object>in 'fast' mode.
        // (because they are query methods)

        /**
         * Return the value to which thisjava.util.Map<Object,Object>maps the specified key.  Returns
         * <code>null</code> if thejava.util.Map<Object,Object>contains no mapping for this key, or if
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
         * Return <code>true</code> if thisjava.util.Map<Object,Object>contains no mappings.
         * 
         * @return is thejava.util.Map<Object,Object>currently empty
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
         * Return <code>true</code> if thisjava.util.Map<Object,Object>contains a mapping for the
         * specified key.
         *
         * @param key  the key to be searched for
         * @return true if thejava.util.Map<Object,Object>contains the key
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
         * Return <code>true</code> if thisjava.util.Map<Object,Object>contains one or more keys mapping
         * to the specified value.
         *
         * @param value  the value to be searched for
         * @return true if thejava.util.Map<Object,Object>contains the value
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

        //java.util.Map<Object,Object>modification
        // ----------------------------------------------------------------------
        // These methods perform special behaviour in 'fast' mode.
        // Thejava.util.Map<Object,Object>is cloned, updated and then assigned back.
        // See the comments at the top as to why this won't always work.

        /**
         * Associate the specified value with the specified key in this map.
         * If thejava.util.Map<Object,Object>previously contained a mapping for this key, the old
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
                    java.util.HashMap<Object, Object> temp = (java.util.HashMap<Object, Object>)map.clone();
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
         * Copy all of the mappings from the specifiedjava.util.Map<Object,Object>to this one, replacing
         * any mappings with the same keys.
         *
         * @param in  thejava.util.Map<Object,Object>whose mappings are to be copied
         */
        public override void putAll(java.util.Map<Object, Object> inJ)
        {
            if (fast)
            {
                lock (this)
                {
                    java.util.HashMap<Object, Object> temp = (java.util.HashMap<Object, Object>)map.clone();
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
                    java.util.HashMap<Object, Object> temp = (java.util.HashMap<Object, Object>)map.clone();
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
                    map = new java.util.HashMap<Object, Object>();
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
                    java.util.MapNS.Entry<Object, Object> e = i.next();
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
                        java.util.MapNS.Entry<Object, Object> e = i.next();
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
         * @return suitable integer hash code
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
         * Return a shallow copy of this <code>FastHashMap</code> instance.
         * The keys and values themselves are not copied.
         * 
         * @return a clone of this map
         */
        public Object clone()
        {
            FastHashMap results = null;
            if (fast)
            {
                results = new FastHashMap(map);
            }
            else
            {
                lock (map)
                {
                    results = new FastHashMap(map);
                }
            }
            results.setFast(getFast());
            return (results);
        }

        //java.util.Map<Object,Object>views
        // ----------------------------------------------------------------------

        /**
         * Return a collection view of the mappings contained in this map.  Each
         * element in the returned collection is a <code>Map.Entry</code>.
         */
        public new java.util.Set<Object> entrySet()
        {
            return new EntrySet(this);
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

        //java.util.Map<Object,Object>view inner classes
        // ----------------------------------------------------------------------

        /**
         * Abstract collection implementation shared by keySet(), values() and entrySet().
         */
        private abstract class CollectionView : java.util.Collection<Object>
        {

            private FastHashMap root;

            public CollectionView(FastHashMap root)
            {
                this.root = root;
            }

            protected abstract java.util.Collection<Object> get(java.util.Map<Object, Object> map);
            protected internal abstract Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry);


            public virtual void clear()
            {
                if (root.fast)
                {
                    lock (root)
                    {
                        root.map = new java.util.HashMap<Object, Object>();
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
                        java.util.HashMap<Object, Object> temp = (java.util.HashMap<Object, Object>)root.map.clone();
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
                        java.util.HashMap<Object, Object> temp = (java.util.HashMap<Object, Object>)root.map.clone();
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
                        java.util.HashMap<Object, Object> temp = (java.util.HashMap<Object, Object>)root.map.clone();
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

            public virtual bool add(Object o)
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public virtual bool addAll(java.util.Collection<Object> c)
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public virtual java.util.Iterator<Object> iterator()
            {
                return new CollectionViewIterator(this);
            }

            private class CollectionViewIterator : java.util.Iterator<Object>
            {
                private readonly FastHashMap root;
                private readonly CollectionView view;
                private java.util.Map<Object, Object> expected;
                private java.util.MapNS.Entry<Object, Object> lastReturned = null;
                private java.util.Iterator<java.util.MapNS.Entry<Object, Object>> iterator;

                public CollectionViewIterator(CollectionView view)
                {
                    this.root = view.root;
                    this.view = view;
                    this.expected = root.map;
                    this.iterator = expected.entrySet().iterator();
                }

                public bool hasNext()
                {
                    if (expected != root.map)
                    {
                        throw new java.util.ConcurrentModificationException();
                    }
                    return iterator.hasNext();
                }

                public Object next()
                {
                    if (expected != root.map)
                    {
                        throw new java.util.ConcurrentModificationException();
                    }
                    lastReturned = (java.util.MapNS.Entry<Object, Object>)iterator.next();
                    return view.iteratorNext(lastReturned);
                }

                public void remove()
                {
                    if (lastReturned == null)
                    {
                        throw new java.lang.IllegalStateException();
                    }
                    if (root.fast)
                    {
                        lock (root)
                        {
                            if (expected != root.map)
                            {
                                throw new java.util.ConcurrentModificationException();
                            }
                            root.remove(lastReturned.getKey());
                            lastReturned = null;
                            expected = root.map;
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
         * Set implementation over the keys of the FastHashMap
         */
        private class KeySet : CollectionView, java.util.Set<Object>
        {
            public KeySet (FastHashMap fhm) : base(fhm){}

            protected override java.util.Collection<Object> get(java.util.Map<Object, Object> map)
            {
                return map.keySet();
            }

            protected internal override Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry)
            {
                return entry.getKey();
            }

        }

        /**
         * Collection implementation over the values of the FastHashMap
         */
        private new class Values : CollectionView
        {

            public Values(FastHashMap fhm) : base(fhm) { }
            protected override java.util.Collection<Object> get(java.util.Map<Object, Object> map)
            {
                return map.values();
            }

            protected internal override Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry)
            {
                return entry.getValue();
            }
        }

        /**
         * Set implementation over the entries of the FastHashMap
         */
        private class EntrySet : CollectionView, java.util.Set<Object>
        {
            private FastHashMap root;
            public EntrySet(FastHashMap root)
                : base(root)
            {
                this.root = root;
            }

            protected override java.util.Collection<Object> get(java.util.Map<Object, Object> map)
            {
                return (java.util.Collection<Object>)root.map.entrySet();
            }

            protected internal override Object iteratorNext(java.util.MapNS.Entry<Object, Object> entry)
            {
                return entry;
            }

        }

    }
}