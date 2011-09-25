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
using org.apache.commons.collections.iterators;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.list
{

    /**
     * Decorates a <code>List</code> to ensure that no duplicates are present
     * much like a <code>Set</code>.
     * <p>
     * The <code>List</code> interface makes certain assumptions/requirements.
     * This implementation breaks these in certain ways, but this is merely the
     * result of rejecting duplicates.
     * Each violation is explained in the method, but it should not affect you.
     * Bear in mind that Sets require immutable objects to function correctly.
     * <p>
     * The {@link org.apache.commons.collections.set.ListOrderedSet ListOrderedSet}
     * class provides an alternative approach, by wrapping an existing Set and
     * retaining insertion order in the iterator.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Matthew Hawthorne
     * @author Stephen Colebourne
     * @author Tom Dunham
     */
    [Serializable]
    public class SetUniqueList : AbstractSerializableListDecorator
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 7196982186153478694L;

        /**
         * Internal Set to maintain uniqueness.
         */
        protected readonly java.util.Set<Object> setJ;

        /**
         * Factory method to create a SetList using the supplied list to retain order.
         * <p>
         * If the list contains duplicates, these are removed (first indexed one kept).
         * A <code>HashSet</code> is used for the set behaviour.
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        public static SetUniqueList decorate(java.util.List<Object> list)
        {
            if (list == null)
            {
                throw new java.lang.IllegalArgumentException("List must not be null");
            }
            if (list.isEmpty())
            {
                return new SetUniqueList(list, new java.util.HashSet<Object>());
            }
            else
            {
                java.util.List<Object> temp = new java.util.ArrayList<Object>(list);
                list.clear();
                SetUniqueList sl = new SetUniqueList(list, new java.util.HashSet<Object>());
                sl.addAll(temp);
                return sl;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies) the List and specifies the set to use.
         * <p>
         * The set and list must both be correctly initialised to the same elements.
         * 
         * @param set  the set to decorate, must not be null
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if set or list is null
         */
        protected internal SetUniqueList(java.util.List<Object> list, java.util.Set<Object> set)
            : base(list)
        {
            if (set == null)
            {
                throw new java.lang.IllegalArgumentException("Set must not be null");
            }
            this.setJ = set;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets an unmodifiable view as a Set.
         * 
         * @return an unmodifiable set view
         */
        public virtual java.util.Set<Object> asSet()
        {
            return UnmodifiableSet.decorate(setJ);
        }

        //-----------------------------------------------------------------------
        /**
         * Adds an element to the list if it is not already present.
         * <p>
         * <i>(Violation)</i>
         * The <code>List</code> interface requires that this method returns
         * <code>true</code> always. However this class may return <code>false</code>
         * because of the <code>Set</code> behaviour.
         * 
         * @param object the object to add
         * @return true if object was added
         */
        public override bool add(Object obj)
        {
            // gets initial size
            int sizeBefore = size();

            // adds element if unique
            add(size(), obj);

            // compares sizes to detect if collection changed
            return (sizeBefore != size());
        }

        /**
         * Adds an element to a specific index in the list if it is not already present.
         * <p>
         * <i>(Violation)</i>
         * The <code>List</code> interface makes the assumption that the element is
         * always inserted. This may not happen with this implementation.
         * 
         * @param index  the index to insert at
         * @param object  the object to add
         */
        public override void add(int index, Object obj)
        {
            // adds element if it is not contained already
            if (setJ.contains(obj) == false)
            {
                base.add(index, obj);
                setJ.add(obj);
            }
        }

        /**
         * Adds an element to the end of the list if it is not already present.
         * <p>
         * <i>(Violation)</i>
         * The <code>List</code> interface makes the assumption that the element is
         * always inserted. This may not happen with this implementation.
         * 
         * @param coll  the collection to add
         */
        public override bool addAll(java.util.Collection<Object> coll)
        {
            return addAll(size(), coll);
        }

        /**
         * Adds a collection of objects to the end of the list avoiding duplicates.
         * <p>
         * Only elements that are not already in this list will be added, and
         * duplicates from the specified collection will be ignored.
         * <p>
         * <i>(Violation)</i>
         * The <code>List</code> interface makes the assumption that the elements
         * are always inserted. This may not happen with this implementation.
         * 
         * @param index  the index to insert at
         * @param coll  the collection to add in iterator order
         * @return true if this collection changed
         */
        public override bool addAll(int index, java.util.Collection<Object> coll)
        {
            // gets initial size
            int sizeBefore = size();

            // adds all elements
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                add(it.next());
            }

            // compares sizes to detect if collection changed
            return sizeBefore != size();
        }

        //-----------------------------------------------------------------------
        /**
         * Sets the value at the specified index avoiding duplicates.
         * <p>
         * The object is set into the specified index.
         * Afterwards, any previous duplicate is removed
         * If the object is not already in the list then a normal set occurs.
         * If it is present, then the old version is removed.
         * 
         * @param index  the index to insert at
         * @param object  the object to set
         * @return the previous object
         */
        public override Object set(int index, Object obj)
        {
            int pos = indexOf(obj);
            Object removed = base.set(index, obj);
            if (pos == -1 || pos == index)
            {
                return removed;
            }

            // the object is already in the uniq list
            // (and it hasn't been swapped with itself)
            base.remove(pos);  // remove the duplicate by index
            setJ.remove(removed);  // remove the item deleted by the set
            return removed;  // return the item deleted by the set
        }

        public override bool remove(Object obj)
        {
            bool result = base.remove(obj);
            setJ.remove(obj);
            return result;
        }

        public override Object remove(int index)
        {
            Object result = base.remove(index);
            setJ.remove(result);
            return result;
        }

        public override bool removeAll(java.util.Collection<Object> coll)
        {
            bool result = base.removeAll(coll);
            setJ.removeAll(coll);
            return result;
        }

        public override bool retainAll(java.util.Collection<Object> coll)
        {
            bool result = base.retainAll(coll);
            setJ.retainAll(coll);
            return result;
        }

        public override void clear()
        {
            base.clear();
            setJ.clear();
        }

        public override bool contains(Object obj)
        {
            return setJ.contains(obj);
        }

        public override bool containsAll(java.util.Collection<Object> coll)
        {
            return setJ.containsAll(coll);
        }

        public override java.util.Iterator<Object> iterator()
        {
            return new SetListIterator(base.iterator(), setJ);
        }

        public override java.util.ListIterator<Object> listIterator()
        {
            return new SetListListIterator(base.listIterator(), setJ);
        }

        public override java.util.ListIterator<Object> listIterator(int index)
        {
            return new SetListListIterator(base.listIterator(index), setJ);
        }

        public override java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            return new SetUniqueList(base.subList(fromIndex, toIndex), setJ);
        }

        //-----------------------------------------------------------------------
        /**
         * Inner class iterator.
         */
        internal class SetListIterator : AbstractIteratorDecorator
        {

            protected java.util.Set<Object> set;
            protected Object last = null;

            protected internal SetListIterator(java.util.Iterator<Object> it, java.util.Set<Object> set)
                : base(it)
            {
                this.set = set;
            }

            public override Object next()
            {
                last = base.next();
                return last;
            }

            public override void remove()
            {
                base.remove();
                set.remove(last);
                last = null;
            }
        }

        /**
         * Inner class iterator.
         */
        internal class SetListListIterator : AbstractListIteratorDecorator
        {

            protected java.util.Set<Object> setJ;
            protected Object last = null;

            protected internal SetListListIterator(java.util.ListIterator<Object> it, java.util.Set<Object> set)
                : base(it)
            {
                this.setJ = set;
            }

            public override Object next()
            {
                last = base.next();
                return last;
            }

            public override Object previous()
            {
                last = base.previous();
                return last;
            }

            public override void remove()
            {
                base.remove();
                setJ.remove(last);
                last = null;
            }

            public override void add(Object obj)
            {
                if (setJ.contains(obj) == false)
                {
                    base.add(obj);
                    setJ.add(obj);
                }
            }

            public override void set(Object obj)
            {
                throw new java.lang.UnsupportedOperationException("ListIterator does not support set");
            }
        }
    }
}