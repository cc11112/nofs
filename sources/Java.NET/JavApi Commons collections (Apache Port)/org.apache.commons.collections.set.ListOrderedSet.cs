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
 */

using System;
using java = biz.ritter.javapi;
using org.apache.commons.collections.iterators;
using org.apache.commons.collections.list;

namespace org.apache.commons.collections.set
{
    /**
     * Decorates another <code>Set</code> to ensure that the order of addition
     * is retained and used by the iterator.
     * <p>
     * If an object is added to the set for a second time, it will remain in the
     * original position in the iteration.
     * The order can be observed from the set via the iterator or toArray methods.
     * <p>
     * The ListOrderedSet also has various useful direct methods. These include many
     * from <code>List</code>, such as <code>get(int)</code>, <code>remove(int)</code>
     * and <code>indexOf(int)</code>. An unmodifiable <code>List</code> view of 
     * the set can be obtained via <code>asList()</code>.
     * <p>
     * This class cannot implement the <code>List</code> interface directly as
     * various interface methods (notably equals/hashCode) are incompatable with a set.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     * @author Henning P. Schmiedehausen
     */
    [Serializable]
    public class ListOrderedSet : AbstractSerializableSetDecorator, java.util.Set<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -228664372470420141L;

        /** Internal list to hold the sequence of objects */
        protected readonly java.util.List<Object> setOrder;

        /**
         * Factory method to create an ordered set specifying the list and set to use.
         * <p>
         * The list and set must both be empty.
         * 
         * @param set  the set to decorate, must be empty and not null
         * @param list  the list to decorate, must be empty and not null
         * @throws IllegalArgumentException if set or list is null
         * @throws IllegalArgumentException if either the set or list is not empty
         * @since Commons Collections 3.1
         */
        public static ListOrderedSet decorate(java.util.Set<Object> set, java.util.List<Object> list)
        {
            if (set == null)
            {
                throw new java.lang.IllegalArgumentException("Set must not be null");
            }
            if (list == null)
            {
                throw new java.lang.IllegalArgumentException("List must not be null");
            }
            if (set.size() > 0 || list.size() > 0)
            {
                throw new java.lang.IllegalArgumentException("Set and List must be empty");
            }
            return new ListOrderedSet(set, list);
        }

        /**
         * Factory method to create an ordered set.
         * <p>
         * An <code>ArrayList</code> is used to retain order.
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        public static ListOrderedSet decorate(java.util.Set<Object> set)
        {
            return new ListOrderedSet(set);
        }

        /**
         * Factory method to create an ordered set using the supplied list to retain order.
         * <p>
         * A <code>HashSet</code> is used for the set behaviour.
         * <p>
         * NOTE: If the list contains duplicates, the duplicates are removed,
         * altering the specified list.
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        public static ListOrderedSet decorate(java.util.List<Object> list)
        {
            if (list == null)
            {
                throw new java.lang.IllegalArgumentException("List must not be null");
            }
            java.util.Set<Object> set = new java.util.HashSet<Object>(list);
            list.retainAll(set);

            return new ListOrderedSet(set, list);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructs a new empty <code>ListOrderedSet</code> using
         * a <code>HashSet</code> and an <code>ArrayList</code> internally.
         * 
         * @since Commons Collections 3.1
         */
        public ListOrderedSet()
            : base(new java.util.HashSet<Object>())
        {
            setOrder = new java.util.ArrayList<Object>();
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected ListOrderedSet(java.util.Set<Object> set)
            : base(set)
        {
            setOrder = new java.util.ArrayList<Object>(set);
        }

        /**
         * Constructor that wraps (not copies) the Set and specifies the list to use.
         * <p>
         * The set and list must both be correctly initialised to the same elements.
         * 
         * @param set  the set to decorate, must not be null
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if set or list is null
         */
        protected ListOrderedSet(java.util.Set<Object> set, java.util.List<Object> list)
            : base(set)
        {
            if (list == null)
            {
                throw new java.lang.IllegalArgumentException("List must not be null");
            }
            setOrder = list;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets an unmodifiable view of the order of the Set.
         * 
         * @return an unmodifiable list view
         */
        public virtual java.util.List<Object> asList()
        {
            return UnmodifiableList.decorate(setOrder);
        }

        //-----------------------------------------------------------------------
        public override void clear()
        {
            collection.clear();
            setOrder.clear();
        }

        public override java.util.Iterator<Object> iterator()
        {
            return new OrderedSetIterator(setOrder.iterator(), collection);
        }

        public override bool add(Object obj)
        {
            if (collection.contains(obj))
            {
                // re-adding doesn't change order
                return collection.add(obj);
            }
            else
            {
                // first add, so add to both set and list
                bool result = collection.add(obj);
                setOrder.add(obj);
                return result;
            }
        }

        public override bool addAll(java.util.Collection<Object> coll)
        {
            bool result = false;
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                Object obj = it.next();
                result = result | add(obj);
            }
            return result;
        }

        public override bool remove(Object obj)
        {
            bool result = collection.remove(obj);
            setOrder.remove(obj);
            return result;
        }

        public override bool removeAll(java.util.Collection<Object> coll)
        {
            bool result = false;
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                Object obj = it.next();
                result = result | remove(obj);
            }
            return result;
        }

        public override bool retainAll(java.util.Collection<Object> coll)
        {
            bool result = collection.retainAll(coll);
            if (result == false)
            {
                return false;
            }
            else if (collection.size() == 0)
            {
                setOrder.clear();
            }
            else
            {
                for (java.util.Iterator<Object> it = setOrder.iterator(); it.hasNext(); )
                {
                    Object obj = it.next();
                    if (collection.contains(obj) == false)
                    {
                        it.remove();
                    }
                }
            }
            return result;
        }

        public override Object[] toArray()
        {
            return setOrder.toArray();
        }

        public override Object[] toArray<Object>(Object[] a)
        {
            return setOrder.toArray(a);
        }

        //-----------------------------------------------------------------------
        public virtual Object get(int index)
        {
            return setOrder.get(index);
        }

        public virtual int indexOf(Object obj)
        {
            return setOrder.indexOf(obj);
        }

        public virtual void add(int index, Object obj)
        {
            if (contains(obj) == false)
            {
                collection.add(obj);
                setOrder.add(index, obj);
            }
        }

        public virtual bool addAll(int index, java.util.Collection<Object> coll)
        {
            bool changed = false;
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                Object obj = it.next();
                if (contains(obj) == false)
                {
                    collection.add(obj);
                    setOrder.add(index, obj);
                    index++;
                    changed = true;
                }
            }
            return changed;
        }

        public virtual Object remove(int index)
        {
            Object obj = setOrder.remove(index);
            remove(obj);
            return obj;
        }

        /**
         * Uses the underlying List's toString so that order is achieved. 
         * This means that the decorated Set's toString is not used, so 
         * any custom toStrings will be ignored. 
         */
        // Fortunately List.toString and Set.toString look the same
        public override String ToString()
        {
            return setOrder.toString();
        }

        //-----------------------------------------------------------------------
        /**
         * Internal iterator handle remove.
         */
        internal class OrderedSetIterator : AbstractIteratorDecorator
        {

            /** Object we iterate on */
            protected readonly java.util.Collection<Object> set;
            /** Last object retrieved */
            protected Object last;

            internal OrderedSetIterator(java.util.Iterator<Object> iterator, java.util.Collection<Object> set)
                : base(iterator)
            {
                this.set = set;
            }

            public override Object next()
            {
                last = iterator.next();
                return last;
            }

            public override void remove()
            {
                set.remove(last);
                iterator.remove();
                last = null;
            }
        }
    }
}