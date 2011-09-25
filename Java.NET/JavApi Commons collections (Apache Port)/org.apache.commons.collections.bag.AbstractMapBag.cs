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
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.bag
{

    /**
     * Abstract implementation of the {@link Bag} interface to simplify the creation
     * of subclass implementations.
     * <p>
     * Subclasses specify a Map implementation to use as the internal storage.
     * The map will be used to map bag elements to a number; the number represents
     * the number of occurrences of that element in the bag.
     *
     * @since Commons Collections 3.0 (previously DefaultMapBag v2.0)
     * @version $Revision$ $Date$
     * 
     * @author Chuck Burdick
     * @author Michael A. Smith
     * @author Stephen Colebourne
     * @author Janek Bogucki
     * @author Steve Clark
     */
    [Serializable]
    public abstract class AbstractMapBag : Bag
    {

        /** The map to use to store the data */
        [NonSerialized]
        private java.util.Map<Object, Object> map;
        /** The current total size of the bag */
        private int sizeJ;
        /** The modification count for fail fast iterators */
        [NonSerialized]
        private int modCount;
        /** The modification count for fail fast iterators */
        [NonSerialized]
        private java.util.Set<Object> uniqueSetJ;

        /**
         * Constructor needed for subclass serialisation.
         * 
         */
        protected AbstractMapBag()
            : base()
        {
        }

        /**
         * Constructor that assigns the specified Map as the backing store.
         * The map must be empty and non-null.
         * 
         * @param map  the map to assign
         */
        protected AbstractMapBag(java.util.Map<Object, Object> map)
            : base()
        {

            this.map = map;
        }

        /**
         * Utility method for implementations to access the map that backs
         * this bag. Not intended for interactive use outside of subclasses.
         * 
         * @return the map being used by the Bag
         */
        protected virtual java.util.Map<Object, Object> getMap()
        {
            return map;
        }

        //-----------------------------------------------------------------------
        /**
         * Returns the number of elements in this bag.
         *
         * @return current size of the bag
         */
        public virtual int size()
        {
            return sizeJ;
        }

        /**
         * Returns true if the underlying map is empty.
         *
         * @return true if bag is empty
         */
        public virtual bool isEmpty()
        {
            return map.isEmpty();
        }

        /**
         * Returns the number of occurrence of the given element in this bag
         * by looking up its count in the underlying map.
         *
         * @param object  the object to search for
         * @return the number of occurrences of the object, zero if not found
         */
        public virtual int getCount(Object obj)
        {
            MutableInteger count = (MutableInteger)map.get(obj);
            if (count != null)
            {
                return count.value;
            }
            return 0;
        }

        //-----------------------------------------------------------------------
        /**
         * Determines if the bag contains the given element by checking if the
         * underlying map contains the element as a key.
         *
         * @param object  the object to search for
         * @return true if the bag contains the given element
         */
        public virtual bool contains(Object obj)
        {
            return map.containsKey(obj);
        }

        /**
         * Determines if the bag contains the given elements.
         * 
         * @param coll  the collection to check against
         * @return <code>true</code> if the Bag contains all the collection
         */
        public virtual bool containsAll(java.util.Collection<Object> coll)
        {
            if (coll is Bag)
            {
                return containsAll((Bag)coll);
            }
            return containsAll(new HashBag(coll));
        }

        /**
         * Returns <code>true</code> if the bag contains all elements in
         * the given collection, respecting cardinality.
         * 
         * @param other  the bag to check against
         * @return <code>true</code> if the Bag contains all the collection
         */
        bool containsAll(Bag other)
        {
            bool result = true;
            java.util.Iterator<Object> it = other.uniqueSet().iterator();
            while (it.hasNext())
            {
                Object current = it.next();
                bool contains = getCount(current) >= other.getCount(current);
                result = result && contains;
            }
            return result;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets an iterator over the bag elements.
         * Elements present in the Bag more than once will be returned repeatedly.
         * 
         * @return the iterator
         */
        public virtual java.util.Iterator<Object> iterator()
        {
            return new BagIterator(this);
        }

        /**
         * Inner class iterator for the Bag.
         */
        private class BagIterator : java.util.Iterator<Object>
        {
            private AbstractMapBag parent;
            private java.util.Iterator<java.util.MapNS.Entry<Object, Object>> entryIterator;
            private java.util.MapNS.Entry<Object, Object> current;
            private int itemCount;
            private readonly int mods;
            private bool canRemove;

            /**
             * Constructor.
             * 
             * @param parent  the parent bag
             */
            public BagIterator(AbstractMapBag parent)
            {
                this.parent = parent;
                this.entryIterator = parent.map.entrySet().iterator();
                this.current = null;
                this.mods = parent.modCount;
                this.canRemove = false;
            }

            public bool hasNext()
            {
                return (itemCount > 0 || entryIterator.hasNext());
            }

            public Object next()
            {
                if (parent.modCount != mods)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                if (itemCount == 0)
                {
                    current = (java.util.MapNS.Entry<Object, Object>)entryIterator.next();
                    itemCount = ((MutableInteger)current.getValue()).value;
                }
                canRemove = true;
                itemCount--;
                return current.getKey();
            }

            public void remove()
            {
                if (parent.modCount != mods)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                if (canRemove == false)
                {
                    throw new java.lang.IllegalStateException();
                }
                MutableInteger mut = (MutableInteger)current.getValue();
                if (mut.value > 1)
                {
                    mut.value--;
                }
                else
                {
                    entryIterator.remove();
                }
                parent.sizeJ--;
                canRemove = false;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Adds a new element to the bag, incrementing its count in the underlying map.
         *
         * @param object  the object to add
         * @return <code>true</code> if the object was not already in the <code>uniqueSet</code>
         */
        public virtual bool add(Object obj)
        {
            return add(obj, 1);
        }

        /**
         * Adds a new element to the bag, incrementing its count in the map.
         *
         * @param object  the object to search for
         * @param nCopies  the number of copies to add
         * @return <code>true</code> if the object was not already in the <code>uniqueSet</code>
         */
        public virtual bool add(Object obj, int nCopies)
        {
            modCount++;
            if (nCopies > 0)
            {
                MutableInteger mut = (MutableInteger)map.get(obj);
                sizeJ += nCopies;
                if (mut == null)
                {
                    map.put(obj, new MutableInteger(nCopies));
                    return true;
                }
                else
                {
                    mut.value += nCopies;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /**
         * Invokes {@link #add(Object)} for each element in the given collection.
         *
         * @param coll  the collection to add
         * @return <code>true</code> if this call changed the bag
         */
        public virtual bool addAll(java.util.Collection<Object> coll)
        {
            bool changed = false;
            java.util.Iterator<Object> i = coll.iterator();
            while (i.hasNext())
            {
                bool added = add(i.next());
                changed = changed || added;
            }
            return changed;
        }

        //-----------------------------------------------------------------------
        /**
         * Clears the bag by clearing the underlying map.
         */
        public virtual void clear()
        {
            modCount++;
            map.clear();
            sizeJ = 0;
        }

        /**
         * Removes all copies of the specified object from the bag.
         * 
         * @param object  the object to remove
         * @return true if the bag changed
         */
        public virtual bool remove(Object obj)
        {
            MutableInteger mut = (MutableInteger)map.get(obj);
            if (mut == null)
            {
                return false;
            }
            modCount++;
            map.remove(obj);
            sizeJ -= mut.value;
            return true;
        }

        /**
         * Removes a specified number of copies of an object from the bag.
         * 
         * @param object  the object to remove
         * @param nCopies  the number of copies to remove
         * @return true if the bag changed
         */
        public virtual bool remove(Object obj, int nCopies)
        {
            MutableInteger mut = (MutableInteger)map.get(obj);
            if (mut == null)
            {
                return false;
            }
            if (nCopies <= 0)
            {
                return false;
            }
            modCount++;
            if (nCopies < mut.value)
            {
                mut.value -= nCopies;
                sizeJ -= nCopies;
            }
            else
            {
                map.remove(obj);
                sizeJ -= mut.value;
            }
            return true;
        }

        /**
         * Removes objects from the bag according to their count in the specified collection.
         * 
         * @param coll  the collection to use
         * @return true if the bag changed
         */
        public virtual bool removeAll(java.util.Collection<Object> coll)
        {
            bool result = false;
            if (coll != null)
            {
                java.util.Iterator<Object> i = coll.iterator();
                while (i.hasNext())
                {
                    bool changed = remove(i.next(), 1);
                    result = result || changed;
                }
            }
            return result;
        }

        /**
         * Remove any members of the bag that are not in the given
         * bag, respecting cardinality.
         *
         * @param coll  the collection to retain
         * @return true if this call changed the collection
         */
        public virtual bool retainAll(java.util.Collection<Object> coll)
        {
            if (coll is Bag)
            {
                return retainAll((Bag)coll);
            }
            return retainAll(new HashBag(coll));
        }

        /**
         * Remove any members of the bag that are not in the given
         * bag, respecting cardinality.
         * @see #retainAll(Collection)
         * 
         * @param other  the bag to retain
         * @return <code>true</code> if this call changed the collection
         */
        bool retainAll(Bag other)
        {
            bool result = false;
            Bag excess = new HashBag();
            java.util.Iterator<Object> i = uniqueSet().iterator();
            while (i.hasNext())
            {
                Object current = i.next();
                int myCount = getCount(current);
                int otherCount = other.getCount(current);
                if (1 <= otherCount && otherCount <= myCount)
                {
                    excess.add(current, myCount - otherCount);
                }
                else
                {
                    excess.add(current, myCount);
                }
            }
            if (!excess.isEmpty())
            {
                result = removeAll(excess);
            }
            return result;
        }

        //-----------------------------------------------------------------------
        /**
         * Mutable integer class for storing the data.
         */
        protected class MutableInteger
        {
            /** The value of this mutable. */
            protected internal int value;

            /**
             * Constructor.
             * @param value  the initial value
             */
            internal MutableInteger(int value)
            {
                this.value = value;
            }

            public override bool Equals(Object obj)
            {
                if (obj is MutableInteger == false)
                {
                    return false;
                }
                return ((MutableInteger)obj).value == value;
            }

            public override int GetHashCode()
            {
                return value;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Returns an array of all of this bag's elements.
         *
         * @return an array of all of this bag's elements
         */
        public virtual Object[] toArray()
        {
            Object[] result = new Object[size()];
            int i = 0;
            java.util.Iterator<Object> it = map.keySet().iterator();
            while (it.hasNext())
            {
                Object current = it.next();
                for (int index = getCount(current); index > 0; index--)
                {
                    result[i++] = current;
                }
            }
            return result;
        }

        /**
         * Returns an array of all of this bag's elements.
         *
         * @param array  the array to populate
         * @return an array of all of this bag's elements
         */
        public virtual Object[] toArray<Object>(Object[] array)
        {
            int size = this.size();
            if (array.Length < size)
            {
                array = new Object[sizeJ];
            }

            int i = 0;
            java.util.Iterator<Object> it = (java.util.Iterator<Object>)map.keySet().iterator();
            while (it.hasNext())
            {
                Object current = it.next();
                for (int index = getCount(current); index > 0; index--)
                {
                    array[i++] = current;
                }
            }
            if (array.Length > size)
            {
                array[size] = default(Object);
            }
            return array;
        }

        /**
         * Returns an unmodifiable view of the underlying map's key set.
         *
         * @return the set of unique elements in this bag
         */
        public virtual java.util.Set<Object> uniqueSet()
        {
            if (uniqueSetJ == null)
            {
                uniqueSetJ = UnmodifiableSet.decorate(map.keySet());
            }
            return uniqueSetJ;
        }

        //-----------------------------------------------------------------------
        /**
         * Write the map out using a custom routine.
         * @param out  the output stream
         * @throws IOException
         */
        protected virtual void doWriteObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.writeInt(map.size());
            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)it.next();
                outJ.writeObject(entry.getKey());
                outJ.writeInt(((MutableInteger)entry.getValue()).value);
            }
        }

        /**
         * Read the map in using a custom routine.
         * @param map  the map to use
         * @param in  the input stream
         * @throws IOException
         * @throws ClassNotFoundException
         */
        protected virtual void doReadObject(java.util.Map<Object, Object> map, java.io.ObjectInputStream inJ)
        {// throws IOException, ClassNotFoundException {
            this.map = map;
            int entrySize = inJ.readInt();
            for (int i = 0; i < entrySize; i++)
            {
                Object obj = inJ.readObject();
                int count = inJ.readInt();
                map.put(obj, new MutableInteger(count));
                sizeJ += count;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Compares this Bag to another.
         * This Bag equals another Bag if it contains the same number of occurrences of
         * the same elements.
         * 
         * @param object  the Bag to compare to
         * @return true if equal
         */
        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj is Bag == false)
            {
                return false;
            }
            Bag other = (Bag)obj;
            if (other.size() != size())
            {
                return false;
            }
            for (java.util.Iterator<Object> it = map.keySet().iterator(); it.hasNext(); )
            {
                Object element = it.next();
                if (other.getCount(element) != getCount(element))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Gets a hash code for the Bag compatible with the definition of equals.
         * The hash code is defined as the sum total of a hash code for each element.
         * The per element hash code is defined as
         * <code>(e==null ? 0 : e.hashCode()) ^ noOccurances)</code>.
         * This hash code is compatible with the Set interface.
         * 
         * @return the hash code of the Bag
         */
        public override int GetHashCode()
        {
            int total = 0;
            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)it.next();
                Object element = entry.getKey();
                MutableInteger count = (MutableInteger)entry.getValue();
                total += (element == null ? 0 : element.GetHashCode()) ^ count.value;
            }
            return total;
        }

        /**
         * Implement a toString() method suitable for debugging.
         * 
         * @return a debugging toString
         */
        public override String ToString()
        {
            if (size() == 0)
            {
                return "[]";
            }
            System.Text.StringBuilder buf = new System.Text.StringBuilder();
            buf.append('[');
            java.util.Iterator<Object> it = uniqueSet().iterator();
            while (it.hasNext())
            {
                Object current = it.next();
                int count = getCount(current);
                buf.append(count);
                buf.append(':');
                buf.append(current);
                if (it.hasNext())
                {
                    buf.append(',');
                }
            }
            buf.append(']');
            return buf.toString();
        }

    }
}