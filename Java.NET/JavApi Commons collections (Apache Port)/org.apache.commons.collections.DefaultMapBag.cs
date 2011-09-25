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
using System.Text;
using java = biz.ritter.javapi;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections
{

    /**
     * A skeletal implementation of the {@link Bag}
     * interface to minimize the effort required for target implementations.
     * Subclasses need only to call <code>setMap(Map)</code> in their constructor 
     * (or invoke the Map constructor) specifying a map instance that will be used
     * to store the contents of the bag.
     * <p>
     * The map will be used to map bag elements to a number; the number represents
     * the number of occurrences of that element in the bag.
     *
     * @deprecated Moved to bag subpackage as AbstractMapBag. Due to be removed in v4.0.
     * @since Commons Collections 2.0
     * @version $Revision$ $Date$
     * 
     * @author Chuck Burdick
     * @author Michael A. Smith
     * @author Stephen Colebourne
     * @author Janek Bogucki
     */
    [Obsolete]
    public abstract class DefaultMapBag : Bag
    {
        private java.util.Map<Object, Object> _map = null;
        private int _total = 0;
        private int _mods = 0;

        /**
         * No-argument constructor.  
         * Subclasses should invoke <code>setMap(Map)</code> in
         * their constructors.
         */
        public DefaultMapBag()
        {
        }

        /**
         * Constructor that assigns the specified Map as the backing store.
         * The map must be empty.
         * 
         * @param map  the map to assign
         */
        protected DefaultMapBag(java.util.Map<Object, Object> map)
        {
            setMap(map);
        }

        /**
         * Adds a new element to the bag by incrementing its count in the 
         * underlying map.
         *
         * @param object  the object to add
         * @return <code>true</code> if the object was not already in the <code>uniqueSet</code>
         */
        public virtual bool add(Object obj)
        {
            return add(obj, 1);
        }

        /**
         * Adds a new element to the bag by incrementing its count in the map.
         *
         * @param object  the object to search for
         * @param nCopies  the number of copies to add
         * @return <code>true</code> if the object was not already in the <code>uniqueSet</code>
         */
        public virtual bool add(Object obj, int nCopies)
        {
            _mods++;
            if (nCopies > 0)
            {
                int count = (nCopies + getCount(obj));
                _map.put(obj, new java.lang.Integer(count));
                _total += nCopies;
                return (count == nCopies);
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

        /**
         * Clears the bag by clearing the underlying map.
         */
        public virtual void clear()
        {
            _mods++;
            _map.clear();
            _total = 0;
        }

        /**
         * Determines if the bag contains the given element by checking if the
         * underlying map contains the element as a key.
         *
         * @param object  the object to search for
         * @return true if the bag contains the given element
         */
        public virtual bool contains(Object obj)
        {
            return _map.containsKey(obj);
        }

        /**
         * Determines if the bag contains the given elements.
         * 
         * @param coll  the collection to check against
         * @return <code>true</code> if the Bag contains all the collection
         */
        public virtual bool containsAll(java.util.Collection<Object> coll)
        {
            return containsAll(new HashBag(coll));
        }

        /**
         * Returns <code>true</code> if the bag contains all elements in
         * the given collection, respecting cardinality.
         * 
         * @param other  the bag to check against
         * @return <code>true</code> if the Bag contains all the collection
         */
        public virtual bool containsAll(Bag other)
        {
            bool result = true;
            java.util.Iterator<Object> i = other.uniqueSet().iterator();
            while (i.hasNext())
            {
                Object current = i.next();
                bool contains = getCount(current) >= other.getCount(current);
                result = result && contains;
            }
            return result;
        }

        /**
         * Returns true if the given object is not null, has the precise type 
         * of this bag, and contains the same number of occurrences of all the
         * same elements.
         *
         * @param object  the object to test for equality
         * @return true if that object equals this bag
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
            for (java.util.Iterator<Object> it = _map.keySet().iterator(); it.hasNext(); )
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
         * Returns the hash code of the underlying map.
         *
         * @return the hash code of the underlying map
         */
        public override int GetHashCode()
        {
            return _map.GetHashCode();
        }

        /**
         * Returns true if the underlying map is empty.
         *
         * @return true if there are no elements in this bag
         */
        public virtual bool isEmpty()
        {
            return _map.isEmpty();
        }

        public virtual java.util.Iterator<Object> iterator()
        {
            return new BagIterator(this, extractList().iterator());
        }

        public virtual bool remove(Object obj)
        {
            return remove(obj, getCount(obj));
        }

        public virtual bool remove(Object obj, int nCopies)
        {
            _mods++;
            bool result = false;
            int count = getCount(obj);
            if (nCopies <= 0)
            {
                result = false;
            }
            else if (count > nCopies)
            {
                _map.put(obj, new java.lang.Integer(count - nCopies));
                result = true;
                _total -= nCopies;
            }
            else
            { // count > 0 && count <= i  
                // need to remove all
                result = (_map.remove(obj) != null);
                _total -= count;
            }
            return result;
        }

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
        public virtual bool retainAll(Bag other)
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

        /**
         * Returns an array of all of this bag's elements.
         *
         * @return an array of all of this bag's elements
         */
        public virtual Object[] toArray()
        {
            return extractList().toArray();
        }

        /**
         * Returns an array of all of this bag's elements.
         *
         * @param array  the array to populate
         * @return an array of all of this bag's elements
         */
        public virtual Object[] toArray<Object>(Object[] array)
        {
            return extractList().toArray(array);
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
            int result = 0;
            java.lang.Integer count = MapUtils.getInteger(_map, obj);
            if (count != null)
            {
                result = count.intValue();
            }
            return result;
        }

        /**
         * Returns an unmodifiable view of the underlying map's key set.
         *
         * @return the set of unique elements in this bag
         */
        public virtual java.util.Set<Object> uniqueSet()
        {
            return UnmodifiableSet.decorate(_map.keySet());
        }

        /**
         * Returns the number of elements in this bag.
         *
         * @return the number of elements in this bag
         */
        public virtual int size()
        {
            return _total;
        }

        /**
         * Actually walks the bag to make sure the count is correct and
         * resets the running total
         * 
         * @return the current total size
         */
        protected virtual int calcTotalSize()
        {
            _total = extractList().size();
            return _total;
        }

        /**
         * Utility method for implementations to set the map that backs
         * this bag. Not intended for interactive use outside of
         * subclasses.
         */
        protected virtual void setMap(java.util.Map<Object, Object> map)
        {
            if (map == null || map.isEmpty() == false)
            {
                throw new java.lang.IllegalArgumentException("The map must be non-null and empty");
            }
            _map = map;
        }

        /**
         * Utility method for implementations to access the map that backs
         * this bag. Not intended for interactive use outside of
         * subclasses.
         */
        protected virtual java.util.Map<Object, Object> getMap()
        {
            return _map;
        }

        /**
         * Create a list for use in iteration, etc.
         */
        private java.util.List<Object> extractList()
        {
            java.util.ArrayList<Object> result = new java.util.ArrayList<Object>();
            java.util.Iterator<Object> i = uniqueSet().iterator();
            while (i.hasNext())
            {
                Object current = i.next();
                for (int index = getCount(current); index > 0; index--)
                {
                    result.add(current);
                }
            }
            return result;
        }

        /**
         * Return number of modifications for iterator.
         * 
         * @return the modification count
         */
        internal virtual int modCount()
        {
            return _mods;
        }

        /**
         * Implement a toString() method suitable for debugging.
         * 
         * @return a debugging toString
         */
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.append("[");
            java.util.Iterator<Object> i = uniqueSet().iterator();
            while (i.hasNext())
            {
                Object current = i.next();
                int count = getCount(current);
                buf.append(count);
                buf.append(":");
                buf.append(current);
                if (i.hasNext())
                {
                    buf.append(",");
                }
            }
            buf.append("]");
            return buf.toString();
        }

    }
    internal class BagIterator : java.util.Iterator<Object>
    {
        private DefaultMapBag _parent = null;
        private java.util.Iterator<Object> _support = null;
        private Object _current = null;
        private int _mods = 0;

        public BagIterator(DefaultMapBag parent, java.util.Iterator<Object> support)
        {
            _parent = parent;
            _support = support;
            _current = null;
            _mods = parent.modCount();
        }

        public virtual bool hasNext()
        {
            return _support.hasNext();
        }

        public virtual Object next()
        {
            if (_parent.modCount() != _mods)
            {
                throw new java.util.ConcurrentModificationException();
            }
            _current = _support.next();
            return _current;
        }

        public virtual void remove()
        {
            if (_parent.modCount() != _mods)
            {
                throw new java.util.ConcurrentModificationException();
            }
            _support.remove();
            _parent.remove(_current, 1);
            _mods++;
        }
    }

}