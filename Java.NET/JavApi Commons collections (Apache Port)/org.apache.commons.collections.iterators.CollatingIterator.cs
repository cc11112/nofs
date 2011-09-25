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
using org.apache.commons.collections.list;

namespace org.apache.commons.collections.iterators
{

    /**
     * Provides an ordered iteration over the elements contained in
     * a collection of ordered Iterators.
     * <p>
     * Given two ordered {@link Iterator} instances <code>A</code> and <code>B</code>,
     * the {@link #next} method on this iterator will return the lesser of 
     * <code>A.next()</code> and <code>B.next()</code>.
     *
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     * 
     * @author Rodney Waldhoff
     * @author Stephen Colebourne
     */
    public class CollatingIterator : java.util.Iterator<Object>
    {

        /** The {@link Comparator} used to evaluate order. */
        private java.util.Comparator<Object> comparator = null;

        /** The list of {@link Iterator}s to evaluate. */
        private java.util.ArrayList<Object> iterators = null;

        /** {@link Iterator#next Next} objects peeked from each iterator. */
        private java.util.ArrayList<Object> values = null;

        /** Whether or not each {@link #values} element has been set. */
        private java.util.BitSet valueSet = null;

        /** Index of the {@link #iterators iterator} from whom the last returned value was obtained. */
        private int lastReturned = -1;

        // Constructors
        // ----------------------------------------------------------------------
        /**
         * Constructs a new <code>CollatingIterator</code>.  Natural sort order
         * will be used, and child iterators will have to be manually added 
         * using the {@link #addIterator(Iterator)} method.
         */
        public CollatingIterator()
            : this(null, 2)
        {
        }

        /**
         * Constructs a new <code>CollatingIterator</code> that will used the
         * specified comparator for ordering.  Child iterators will have to be 
         * manually added using the {@link #addIterator(Iterator)} method.
         *
         * @param comp  the comparator to use to sort, or null to use natural sort order
         */
        public CollatingIterator(java.util.Comparator<Object> comp)
            : this(comp, 2)
        {
        }

        /**
         * Constructs a new <code>CollatingIterator</code> that will used the
         * specified comparator for ordering and have the specified initial
         * capacity.  Child iterators will have to be 
         * manually added using the {@link #addIterator(Iterator)} method.
         *
         * @param comp  the comparator to use to sort, or null to use natural sort order
         * @param initIterCapacity  the initial capacity for the internal list
         *    of child iterators
         */
        public CollatingIterator(java.util.Comparator<Object> comp, int initIterCapacity)
        {
            iterators = new java.util.ArrayList<Object>(initIterCapacity);
            setComparator(comp);
        }

        /**
         * Constructs a new <code>CollatingIterator</code> that will use the
         * specified comparator to provide ordered iteration over the two
         * given iterators.
         *
         * @param comp  the comparator to use to sort, or null to use natural sort order
         * @param a  the first child ordered iterator
         * @param b  the second child ordered iterator
         * @throws NullPointerException if either iterator is null
         */
        public CollatingIterator(java.util.Comparator<Object> comp, java.util.Iterator<Object> a, java.util.Iterator<Object> b) :
            this(comp, 2)
        {
            addIterator(a);
            addIterator(b);
        }

        /**
         * Constructs a new <code>CollatingIterator</code> that will use the
         * specified comparator to provide ordered iteration over the array
         * of iterators.
         *
         * @param comp  the comparator to use to sort, or null to use natural sort order
         * @param iterators  the array of iterators
         * @throws NullPointerException if iterators array is or contains null
         */
        public CollatingIterator(java.util.Comparator<Object> comp, java.util.Iterator<Object>[] iterators)
            : this(comp, iterators.Length)
        {
            for (int i = 0; i < iterators.Length; i++)
            {
                addIterator(iterators[i]);
            }
        }

        /**
         * Constructs a new <code>CollatingIterator</code> that will use the
         * specified comparator to provide ordered iteration over the collection
         * of iterators.
         *
         * @param comp  the comparator to use to sort, or null to use natural sort order
         * @param iterators  the collection of iterators
         * @throws NullPointerException if the iterators collection is or contains null
         * @throws ClassCastException if the iterators collection contains an
         *         element that's not an {@link Iterator}
         */
        public CollatingIterator(java.util.Comparator<Object> comp, java.util.Collection<Object> iterators) :
            this(comp, iterators.size())
        {
            for (java.util.Iterator<Object> it = iterators.iterator(); it.hasNext(); )
            {
                java.util.Iterator<Object> item = (java.util.Iterator<Object>)it.next();
                addIterator(item);
            }
        }

        // Public Methods
        // ----------------------------------------------------------------------
        /**
         * Adds the given {@link Iterator} to the iterators being collated.
         * 
         * @param iterator  the iterator to add to the collation, must not be null
         * @throws IllegalStateException if iteration has started
         * @throws NullPointerException if the iterator is null
         */
        public virtual void addIterator(java.util.Iterator<Object> iterator)
        {
            checkNotStarted();
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            iterators.add(iterator);
        }

        /**
         * Sets the iterator at the given index.
         * 
         * @param index  index of the Iterator to replace
         * @param iterator  Iterator to place at the given index
         * @throws IndexOutOfBoundsException if index &lt; 0 or index &gt; size()
         * @throws IllegalStateException if iteration has started
         * @throws NullPointerException if the iterator is null
         */
        public virtual void setIterator(int index, java.util.Iterator<Object> iterator)
        {
            checkNotStarted();
            if (iterator == null)
            {
                throw new java.lang.NullPointerException("Iterator must not be null");
            }
            iterators.set(index, iterator);
        }

        /**
         * Gets the list of Iterators (unmodifiable).
         * 
         * @return the unmodifiable list of iterators added
         */
        public virtual java.util.List<Object> getIterators()
        {
            return UnmodifiableList.decorate(iterators);
        }

        /**
         * Gets the {@link Comparator} by which collatation occurs.
         */
        public virtual java.util.Comparator<Object> getComparator()
        {
            return comparator;
        }

        /**
         * Sets the {@link Comparator} by which collation occurs.
         * 
         * @throws IllegalStateException if iteration has started
         */
        public virtual void setComparator(java.util.Comparator<Object> comp)
        {
            checkNotStarted();
            comparator = comp;
        }

        // Iterator Methods
        // -------------------------------------------------------------------
        /**
         * Returns <code>true</code> if any child iterator has remaining elements.
         *
         * @return true if this iterator has remaining elements
         */
        public virtual bool hasNext()
        {
            start();
            return anyValueSet(valueSet) || anyHasNext(iterators);
        }

        /**
         * Returns the next ordered element from a child iterator.
         *
         * @return the next ordered element
         * @throws NoSuchElementException if no child iterator has any more elements
         */
        public virtual Object next()
        {// throws NoSuchElementException {
            if (hasNext() == false)
            {
                throw new java.util.NoSuchElementException();
            }
            int leastIndex = least();
            if (leastIndex == -1)
            {
                throw new java.util.NoSuchElementException();
            }
            else
            {
                Object val = values.get(leastIndex);
                clear(leastIndex);
                lastReturned = leastIndex;
                return val;
            }
        }

        /**
         * Removes the last returned element from the child iterator that 
         * produced it.
         *
         * @throws IllegalStateException if there is no last returned element,
         *  or if the last returned element has already been removed
         */
        public virtual void remove()
        {
            if (lastReturned == -1)
            {
                throw new java.lang.IllegalStateException("No value can be removed at present");
            }
            java.util.Iterator<Object> it = (java.util.Iterator<Object>)(iterators.get(lastReturned));
            it.remove();
        }

        // Private Methods
        // -------------------------------------------------------------------
        /** 
         * Initializes the collating state if it hasn't been already.
         */
        private void start()
        {
            if (values == null)
            {
                values = new java.util.ArrayList<Object>(iterators.size());
                valueSet = new java.util.BitSet(iterators.size());
                for (int i = 0; i < iterators.size(); i++)
                {
                    values.add(null);
                    valueSet.clear(i);
                }
            }
        }

        /** 
         * Sets the {@link #values} and {@link #valueSet} attributes 
         * at position <i>i</i> to the next value of the 
         * {@link #iterators iterator} at position <i>i</i>, or 
         * clear them if the <i>i</i><sup>th</sup> iterator
         * has no next value.
         *
         * @return <tt>false</tt> iff there was no value to set
         */
        private bool set(int i)
        {
            java.util.Iterator<Object> it = (java.util.Iterator<Object>)(iterators.get(i));
            if (it.hasNext())
            {
                values.set(i, it.next());
                valueSet.set(i);
                return true;
            }
            else
            {
                values.set(i, null);
                valueSet.clear(i);
                return false;
            }
        }

        /** 
         * Clears the {@link #values} and {@link #valueSet} attributes 
         * at position <i>i</i>.
         */
        private void clear(int i)
        {
            values.set(i, null);
            valueSet.clear(i);
        }

        /** 
         * Throws {@link IllegalStateException} if iteration has started 
         * via {@link #start}.
         * 
         * @throws IllegalStateException if iteration started
         */
        private void checkNotStarted()
        {//throws IllegalStateException {
            if (values != null)
            {
                throw new java.lang.IllegalStateException("Can't do that after next or hasNext has been called.");
            }
        }

        /** 
         * Returns the index of the least element in {@link #values},
         * {@link #set(int) setting} any uninitialized values.
         * 
         * @throws IllegalStateException
         */
        private int least()
        {
            int leastIndex = -1;
            Object leastObject = null;
            for (int i = 0; i < values.size(); i++)
            {
                if (valueSet.get(i) == false)
                {
                    set(i);
                }
                if (valueSet.get(i))
                {
                    if (leastIndex == -1)
                    {
                        leastIndex = i;
                        leastObject = values.get(i);
                    }
                    else
                    {
                        Object curObject = values.get(i);
                        if (comparator.compare(curObject, leastObject) < 0)
                        {
                            leastObject = curObject;
                            leastIndex = i;
                        }
                    }
                }
            }
            return leastIndex;
        }

        /**
         * Returns <code>true</code> iff any bit in the given set is 
         * <code>true</code>.
         */
        private bool anyValueSet(java.util.BitSet set)
        {
            for (int i = 0; i < set.size(); i++)
            {
                if (set.get(i))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Returns <code>true</code> iff any {@link Iterator} 
         * in the given list has a next value.
         */
        private bool anyHasNext(java.util.ArrayList<Object> iters)
        {
            for (int i = 0; i < iters.size(); i++)
            {
                java.util.Iterator<Object> it = (java.util.Iterator<Object>)iters.get(i);
                if (it.hasNext())
                {
                    return true;
                }
            }
            return false;
        }

    }
}