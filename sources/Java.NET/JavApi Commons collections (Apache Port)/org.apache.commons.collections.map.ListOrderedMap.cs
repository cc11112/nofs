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
using org.apache.commons.collections.list;

namespace org.apache.commons.collections.map
{

    /**
     * Decorates a <code>Map</code> to ensure that the order of addition is retained
     * using a <code>List</code> to maintain order.
     * <p>
     * The order will be used via the iterators and toArray methods on the views.
     * The order is also returned by the <code>MapIterator</code>.
     * The <code>orderedMapIterator()</code> method accesses an iterator that can
     * iterate both forwards and backwards through the map.
     * In addition, non-interface methods are provided to access the map by index.
     * <p>
     * If an object is added to the Map for a second time, it will remain in the
     * original position in the iteration.
     * <p>
     * <strong>Note that ListOrderedMap is not synchronized and is not thread-safe.</strong>
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
     * @author Henri Yandell
     * @author Stephen Colebourne
     * @author Matt Benson
     */
    [Serializable]
    public class ListOrderedMap
            : AbstractMapDecorator
            , OrderedMap, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 2728177751851003750L;

        /** Internal list to hold the sequence of objects */
        protected readonly java.util.List<Object> insertOrder = new java.util.ArrayList<Object>();

        /**
         * Factory method to create an ordered map.
         * <p>
         * An <code>ArrayList</code> is used to retain order.
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        public static OrderedMap decorate(java.util.Map<Object, Object> map)
        {
            return new ListOrderedMap(map);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructs a new empty <code>ListOrderedMap</code> that decorates
         * a <code>HashMap</code>.
         * 
         * @since Commons Collections 3.1
         */
        public ListOrderedMap() :
            this(new java.util.HashMap<Object, Object>())
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        protected ListOrderedMap(java.util.Map<Object, Object> map) :
            base(map)
        {
            insertOrder.addAll(getMap().keySet());
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

        // Implement OrderedMap
        //-----------------------------------------------------------------------
        public virtual MapIterator mapIterator()
        {
            return orderedMapIterator();
        }

        public virtual OrderedMapIterator orderedMapIterator()
        {
            return new ListOrderedMapIterator(this);
        }

        /**
         * Gets the first key in this map by insert order.
         *
         * @return the first key currently in this map
         * @throws NoSuchElementException if this map is empty
         */
        public virtual Object firstKey()
        {
            if (size() == 0)
            {
                throw new java.util.NoSuchElementException("Map is empty");
            }
            return insertOrder.get(0);
        }

        /**
         * Gets the last key in this map by insert order.
         *
         * @return the last key currently in this map
         * @throws NoSuchElementException if this map is empty
         */
        public virtual Object lastKey()
        {
            if (size() == 0)
            {
                throw new java.util.NoSuchElementException("Map is empty");
            }
            return insertOrder.get(size() - 1);
        }

        /**
         * Gets the next key to the one specified using insert order.
         * This method performs a list search to find the key and is O(n).
         * 
         * @param key  the key to find previous for
         * @return the next key, null if no match or at start
         */
        public virtual Object nextKey(Object key)
        {
            int index = insertOrder.indexOf(key);
            if (index >= 0 && index < size() - 1)
            {
                return insertOrder.get(index + 1);
            }
            return null;
        }

        /**
         * Gets the previous key to the one specified using insert order.
         * This method performs a list search to find the key and is O(n).
         * 
         * @param key  the key to find previous for
         * @return the previous key, null if no match or at start
         */
        public virtual Object previousKey(Object key)
        {
            int index = insertOrder.indexOf(key);
            if (index > 0)
            {
                return insertOrder.get(index - 1);
            }
            return null;
        }

        //-----------------------------------------------------------------------
        public override Object put(Object key, Object value)
        {
            if (getMap().containsKey(key))
            {
                // re-adding doesn't change order
                return getMap().put(key, value);
            }
            else
            {
                // first add, so add to both map and list
                Object result = getMap().put(key, value);
                insertOrder.add(key);
                return result;
            }
        }

        public override void putAll(java.util.Map<Object, Object> map)
        {
            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = it.next();
                put(entry.getKey(), entry.getValue());
            }
        }

        public override Object remove(Object key)
        {
            Object result = getMap().remove(key);
            insertOrder.remove(key);
            return result;
        }

        public override void clear()
        {
            getMap().clear();
            insertOrder.clear();
        }

        //-----------------------------------------------------------------------
        /**
         * Gets a view over the keys in the map.
         * <p>
         * The Collection will be ordered by object insertion into the map.
         *
         * @see #keyList()
         * @return the fully modifiable collection view over the keys
         */
        public override java.util.Set<Object> keySet()
        {
            return new KeySetView(this);
        }

        /**
         * Gets a view over the keys in the map as a List.
         * <p>
         * The List will be ordered by object insertion into the map.
         * The List is unmodifiable.
         *
         * @see #keySet()
         * @return the unmodifiable list view over the keys
         * @since Commons Collections 3.2
         */
        public virtual java.util.List<Object> keyList()
        {
            return UnmodifiableList.decorate(insertOrder);
        }

        /**
         * Gets a view over the values in the map.
         * <p>
         * The Collection will be ordered by object insertion into the map.
         * <p>
         * From Commons Collections 3.2, this Collection can be cast
         * to a list, see {@link #valueList()}
         *
         * @see #valueList()
         * @return the fully modifiable collection view over the values
         */
        public override java.util.Collection<Object> values()
        {
            return new ValuesView(this);
        }

        /**
         * Gets a view over the values in the map as a List.
         * <p>
         * The List will be ordered by object insertion into the map.
         * The List supports remove and set, but does not support add.
         *
         * @see #values()
         * @return the partially modifiable list view over the values
         * @since Commons Collections 3.2
         */
        public virtual java.util.List<Object> valueList()
        {
            return new ValuesView(this);
        }

        /**
         * Gets a view over the entries in the map.
         * <p>
         * The Set will be ordered by object insertion into the map.
         *
         * @return the fully modifiable set view over the entries
         */
        public new java.util.Set<Object> entrySet()
        {
            return new EntrySetView(this, this.insertOrder);
        }

        //-----------------------------------------------------------------------
        /**
         * Returns the Map as a string.
         * 
         * @return the Map as a String
         */
        public override String ToString()
        {
            if (isEmpty())
            {
                return "{}";
            }
            java.lang.StringBuffer buf = new java.lang.StringBuffer();
            buf.append('{');
            bool first = true;
            java.util.Iterator<Object> it = entrySet().iterator();
            while (it.hasNext())
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)it.next();
                Object key = entry.getKey();
                Object value = entry.getValue();
                if (first)
                {
                    first = false;
                }
                else
                {
                    buf.append(", ");
                }
                buf.append(key == this ? "(this Map)" : key);
                buf.append('=');
                buf.append(value == this ? "(this Map)" : value);
            }
            buf.append('}');
            return buf.toString();
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the key at the specified index.
         * 
         * @param index  the index to retrieve
         * @return the key at the specified index
         * @throws IndexOutOfBoundsException if the index is invalid
         */
        public virtual Object get(int index)
        {
            return insertOrder.get(index);
        }

        /**
         * Gets the value at the specified index.
         * 
         * @param index  the index to retrieve
         * @return the key at the specified index
         * @throws IndexOutOfBoundsException if the index is invalid
         */
        public virtual Object getValue(int index)
        {
            return get(insertOrder.get(index));
        }

        /**
         * Gets the index of the specified key.
         * 
         * @param key  the key to find the index of
         * @return the index, or -1 if not found
         */
        public virtual int indexOf(Object key)
        {
            return insertOrder.indexOf(key);
        }

        /**
         * Sets the value at the specified index.
         *
         * @param index  the index of the value to set
         * @return the previous value at that index
         * @throws IndexOutOfBoundsException if the index is invalid
         * @since Commons Collections 3.2
         */
        public virtual Object setValue(int index, Object value)
        {
            Object key = insertOrder.get(index);
            return put(key, value);
        }

        /**
         * Puts a key-value mapping into the map at the specified index.
         * <p>
         * If the map already contains the key, then the original mapping
         * is removed and the new mapping added at the specified index.
         * The remove may change the effect of the index. The index is
         * always calculated relative to the original state of the map.
         * <p>
         * Thus the steps are: (1) remove the existing key-value mapping,
         * then (2) insert the new key-value mapping at the position it
         * would have been inserted had the remove not ocurred.
         *
         * @param index  the index at which the mapping should be inserted
         * @param key  the key
         * @param value  the value
         * @return the value previously mapped to the key
         * @throws IndexOutOfBoundsException if the index is out of range
         * @since Commons Collections 3.2
         */
        public virtual Object put(int index, Object key, Object value)
        {
            java.util.Map<Object, Object> m = getMap();
            if (m.containsKey(key))
            {
                Object result = m.remove(key);
                int pos = insertOrder.indexOf(key);
                insertOrder.remove(pos);
                if (pos < index)
                {
                    index--;
                }
                insertOrder.add(index, key);
                m.put(key, value);
                return result;
            }
            else
            {
                insertOrder.add(index, key);
                m.put(key, value);
                return null;
            }
        }

        /**
         * Removes the element at the specified index.
         *
         * @param index  the index of the object to remove
         * @return the removed value, or <code>null</code> if none existed
         * @throws IndexOutOfBoundsException if the index is invalid
         */
        public Object remove(int index)
        {
            return remove(get(index));
        }

        /**
         * Gets an unmodifiable List view of the keys which changes as the map changes.
         * <p>
         * The returned list is unmodifiable because changes to the values of
         * the list (using {@link java.util.ListIterator#set(Object)}) will
         * effectively remove the value from the list and reinsert that value at
         * the end of the list, which is an unexpected side effect of changing the
         * value of a list.  This occurs because changing the key, changes when the
         * mapping is added to the map and thus where it appears in the list.
         * <p>
         * An alternative to this method is to use the better named
         * {@link #keyList()} or {@link #keySet()}.
         *
         * @see #keyList()
         * @see #keySet()
         * @return The ordered list of keys.  
         */
        public virtual java.util.List<Object> asList()
        {
            return keyList();
        }

        //-----------------------------------------------------------------------
        class ValuesView : java.util.AbstractList<Object>
        {
            private readonly ListOrderedMap parent;

            internal ValuesView(ListOrderedMap parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return this.parent.size();
            }

            public override bool contains(Object value)
            {
                return this.parent.containsValue(value);
            }

            public override void clear()
            {
                this.parent.clear();
            }

            public override java.util.Iterator<Object> iterator()
            {
                return new IAC_Iterator(parent.entrySet().iterator());
            }

            public override Object get(int index)
            {
                return this.parent.getValue(index);
            }

            public override Object set(int index, Object value)
            {
                return this.parent.setValue(index, value);
            }

            public override Object remove(int index)
            {
                return this.parent.remove(index);
            }
        }

        class IAC_Iterator : AbstractIteratorDecorator
        {
            public IAC_Iterator(java.util.Iterator<Object> toDecorate)
                : base(toDecorate)
            {
            }
            public override Object next()
            {
                //! todo (this line is expected): return iterator.next().getValue();
                Object obj = iterator.next();
                Object result = obj.GetType().GetMethod("getValue").Invoke(obj, null);
                return result;
            }
        }
        //-----------------------------------------------------------------------
        class KeySetView : java.util.AbstractSet<Object>
        {
            private readonly ListOrderedMap parent;

            internal KeySetView(ListOrderedMap parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return this.parent.size();
            }

            public override bool contains(Object value)
            {
                return this.parent.containsKey(value);
            }

            public override void clear()
            {
                this.parent.clear();
            }

            public override java.util.Iterator<Object> iterator()
            {
                return new IAC_Iterator(parent.entrySet().iterator());
            }
        }

        //-----------------------------------------------------------------------    
        class EntrySetView : java.util.AbstractSet<Object>
        {
            private readonly ListOrderedMap parent;
            private readonly java.util.List<Object> insertOrder;
            private java.util.Set<java.util.MapNS.Entry<Object, Object>> entrySet;

            public EntrySetView(ListOrderedMap parent, java.util.List<Object> insertOrder)
                : base()
            {
                this.parent = parent;
                this.insertOrder = insertOrder;
            }

            private java.util.Set<Object> getEntrySet()
            {
                if (entrySet == null)
                {
                    entrySet = parent.getMap().entrySet();
                }
                return (java.util.Set<Object>)entrySet;
            }

            public override int size()
            {
                return this.parent.size();
            }
            public override bool isEmpty()
            {
                return this.parent.isEmpty();
            }

            public override bool contains(Object obj)
            {
                return getEntrySet().contains(obj);
            }

            public override bool containsAll(java.util.Collection<Object> coll)
            {
                return getEntrySet().containsAll(coll);
            }

            public override bool remove(Object obj)
            {
                if (obj is java.util.MapNS.Entry<Object, Object> == false)
                {
                    return false;
                }
                if (getEntrySet().contains(obj))
                {
                    Object key = ((java.util.MapNS.Entry<Object, Object>)obj).getKey();
                    parent.remove(key);
                    return true;
                }
                return false;
            }

            public override void clear()
            {
                this.parent.clear();
            }

            public override bool Equals(Object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                return getEntrySet().equals(obj);
            }

            public override int GetHashCode()
            {
                return getEntrySet().GetHashCode();
            }

            public String toString()
            {
                return getEntrySet().toString();
            }

            public override java.util.Iterator<Object> iterator()
            {
                return new ListOrderedIterator(parent, insertOrder);
            }
        }

        //-----------------------------------------------------------------------
        class ListOrderedIterator : AbstractIteratorDecorator
        {
            private readonly ListOrderedMap parent;
            private Object last = null;

            internal ListOrderedIterator(ListOrderedMap parent, java.util.List<Object> insertOrder)
                : base(insertOrder.iterator())
            {
                this.parent = parent;
            }

            public override Object next()
            {
                last = base.next();
                return new ListOrderedMapEntry(parent, last);
            }

            public override void remove()
            {
                base.remove();
                parent.getMap().remove(last);
            }
        }

        //-----------------------------------------------------------------------
        class ListOrderedMapEntry : AbstractMapEntry
        {
            private readonly ListOrderedMap parent;

            internal ListOrderedMapEntry(ListOrderedMap parent, Object key)
                : base(key, null)
            {
                this.parent = parent;
            }

            public override Object getValue()
            {
                return parent.get(key);
            }

            public override Object setValue(Object value)
            {
                return parent.getMap().put(key, value);
            }
        }

        //-----------------------------------------------------------------------
        class ListOrderedMapIterator : OrderedMapIterator, ResettableIterator
        {
            private readonly ListOrderedMap parent;
            private java.util.ListIterator<Object> iterator;
            private Object last = null;
            private bool readable = false;

            internal ListOrderedMapIterator(ListOrderedMap parent)
                : base()
            {
                this.parent = parent;
                this.iterator = parent.insertOrder.listIterator();
            }

            public virtual bool hasNext()
            {
                return iterator.hasNext();
            }

            public virtual Object next()
            {
                last = iterator.next();
                readable = true;
                return last;
            }

            public virtual bool hasPrevious()
            {
                return iterator.hasPrevious();
            }

            public virtual Object previous()
            {
                last = iterator.previous();
                readable = true;
                return last;
            }

            public virtual void remove()
            {
                if (readable == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.REMOVE_INVALID);
                }
                iterator.remove();
                parent.map.remove(last);
                readable = false;
            }

            public virtual Object getKey()
            {
                if (readable == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETKEY_INVALID);
                }
                return last;
            }

            public virtual Object getValue()
            {
                if (readable == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETVALUE_INVALID);
                }
                return parent.get(last);
            }

            public virtual Object setValue(Object value)
            {
                if (readable == false)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.SETVALUE_INVALID);
                }
                return parent.map.put(last, value);
            }

            public virtual void reset()
            {
                iterator = parent.insertOrder.listIterator();
                last = null;
                readable = false;
            }

            public override String ToString()
            {
                if (readable == true)
                {
                    return "Iterator[" + getKey() + "=" + getValue() + "]";
                }
                else
                {
                    return "Iterator[]";
                }
            }
        }

    }
}