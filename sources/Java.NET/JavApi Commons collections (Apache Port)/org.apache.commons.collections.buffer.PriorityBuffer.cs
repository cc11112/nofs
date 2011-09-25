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
using org.apache.commons.collections;

namespace org.apache.commons.collections.buffer
{

    /**
     * Binary heap implementation of <code>Buffer</code> that provides for
     * removal based on <code>Comparator</code> ordering.
     * <p>
     * The removal order of a binary heap is based on either the natural sort
     * order of its elements or a specified {@link Comparator}.  The 
     * {@link #remove()} method always returns the first element as determined
     * by the sort order.  (The <code>ascendingOrder</code> flag in the constructors
     * can be used to reverse the sort order, in which case {@link #remove()}
     * will always remove the last element.)  The removal order is 
     * <i>not</i> the same as the order of iteration; elements are
     * returned by the iterator in no particular order.
     * <p>
     * The {@link #add(Object)} and {@link #remove()} operations perform
     * in logarithmic time.  The {@link #get()} operation performs in constant
     * time.  All other operations perform in linear time or worse.
     * <p>
     * Note that this implementation is not synchronized.  Use 
     * {@link org.apache.commons.collections.BufferUtils#synchronizedBuffer(Buffer)} or
     * {@link org.apache.commons.collections.buffer.SynchronizedBuffer#decorate(Buffer)}
     * to provide synchronized access to a <code>PriorityBuffer</code>:
     * <pre>
     * Buffer heap = SynchronizedBuffer.decorate(new PriorityBuffer());
     * </pre>
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.2.
     *
     * @since Commons Collections 3.0 (previously BinaryHeap v1.0)
     * @version $Revision$ $Date$
     * 
     * @author Peter Donald
     * @author Ram Chidambaram
     * @author Michael A. Smith
     * @author Paul Jack
     * @author Stephen Colebourne
     * @author Steve Phelps
     */
    [Serializable]
    public class PriorityBuffer : java.util.AbstractCollection<Object>
            , Buffer, java.io.Serializable
    {

        /** Serialization lock. */
        private static readonly long serialVersionUID = 6891186490470027896L;

        /**
         * The default capacity for the buffer.
         */
        private static readonly int DEFAULT_CAPACITY = 13;

        /**
         * The elements in this buffer.
         */
        protected Object[] elements;
        /**
         * The number of elements currently in this buffer.
         */
        protected int sizeJ;
        /**
         * If true, the first element as determined by the sort order will 
         * be returned.  If false, the last element as determined by the
         * sort order will be returned.
         */
        protected bool ascendingOrder;
        /**
         * The comparator used to order the elements
         */
        protected java.util.Comparator<Object> comparatorJ;

        //-----------------------------------------------------------------------
        /**
         * Constructs a new empty buffer that sorts in ascending order by the
         * natural order of the objects added.
         */
        public PriorityBuffer()
            : this(DEFAULT_CAPACITY, true, null)
        {
        }

        /**
         * Constructs a new empty buffer that sorts in ascending order using the
         * specified comparator.
         * 
         * @param comparator  the comparator used to order the elements,
         *  null means use natural order
         */
        public PriorityBuffer(java.util.Comparator<Object> comparator)
            : this(DEFAULT_CAPACITY, true, comparator)
        {
        }

        /**
         * Constructs a new empty buffer specifying the sort order and using the
         * natural order of the objects added.
         *
         * @param ascendingOrder  if <code>true</code> the heap is created as a 
         * minimum heap; otherwise, the heap is created as a maximum heap
         */
        public PriorityBuffer(bool ascendingOrder)
            : this(DEFAULT_CAPACITY, ascendingOrder, null)
        {
        }

        /**
         * Constructs a new empty buffer specifying the sort order and comparator.
         *
         * @param ascendingOrder  true to use the order imposed by the given 
         *   comparator; false to reverse that order
         * @param comparator  the comparator used to order the elements,
         *  null means use natural order
         */
        public PriorityBuffer(bool ascendingOrder, java.util.Comparator<Object> comparator)
            : this(DEFAULT_CAPACITY, ascendingOrder, comparator)
        {
        }

        /**
         * Constructs a new empty buffer that sorts in ascending order by the
         * natural order of the objects added, specifying an initial capacity.
         *  
         * @param capacity  the initial capacity for the buffer, greater than zero
         * @throws IllegalArgumentException if <code>capacity</code> is &lt;= <code>0</code>
         */
        public PriorityBuffer(int capacity)
            : this(capacity, true, null)
        {
        }

        /**
         * Constructs a new empty buffer that sorts in ascending order using the
         * specified comparator and initial capacity.
         *
         * @param capacity  the initial capacity for the buffer, greater than zero
         * @param comparator  the comparator used to order the elements,
         *  null means use natural order
         * @throws IllegalArgumentException if <code>capacity</code> is &lt;= <code>0</code>
         */
        public PriorityBuffer(int capacity, java.util.Comparator<Object> comparator)
            : this(capacity, true, comparator)
        {
        }

        /**
         * Constructs a new empty buffer that specifying initial capacity and
         * sort order, using the natural order of the objects added.
         *
         * @param capacity  the initial capacity for the buffer, greater than zero
         * @param ascendingOrder if <code>true</code> the heap is created as a 
         *  minimum heap; otherwise, the heap is created as a maximum heap.
         * @throws IllegalArgumentException if <code>capacity</code> is <code>&lt;= 0</code>
         */
        public PriorityBuffer(int capacity, bool ascendingOrder)
            : this(capacity, ascendingOrder, null)
        {
        }

        /**
         * Constructs a new empty buffer that specifying initial capacity,
         * sort order and comparator.
         *
         * @param capacity  the initial capacity for the buffer, greater than zero
         * @param ascendingOrder  true to use the order imposed by the given 
         *   comparator; false to reverse that order
         * @param comparator  the comparator used to order the elements,
         *  null means use natural order
         * @throws IllegalArgumentException if <code>capacity</code> is <code>&lt;= 0</code>
         */
        public PriorityBuffer(int capacity, bool ascendingOrder, java.util.Comparator<Object> comparator)
            : base()
        {
            if (capacity <= 0)
            {
                throw new java.lang.IllegalArgumentException("invalid capacity");
            }
            this.ascendingOrder = ascendingOrder;

            //+1 as 0 is noop
            this.elements = new Object[capacity + 1];
            this.comparatorJ = comparator;
        }

        //-----------------------------------------------------------------------
        /**
         * Checks whether the heap is ascending or descending order.
         * 
         * @return true if ascending order (a min heap)
         */
        public bool isAscendingOrder()
        {
            return ascendingOrder;
        }

        /**
         * Gets the comparator being used for this buffer, null is natural order.
         * 
         * @return the comparator in use, null is natural order
         */
        public java.util.Comparator<Object> comparator()
        {
            return comparatorJ;
        }

        //-----------------------------------------------------------------------
        /**
         * Returns the number of elements in this buffer.
         *
         * @return the number of elements in this buffer
         */
        public override int size()
        {
            return sizeJ;
        }

        /**
         * Clears all elements from the buffer.
         */
        public override void clear()
        {
            elements = new Object[elements.Length]; // for gc
            sizeJ = 0;
        }

        /**
         * Adds an element to the buffer.
         * <p>
         * The element added will be sorted according to the comparator in use.
         *
         * @param element  the element to be added
         * @return true always
         */
        public override bool add(Object element)
        {
            if (isAtCapacity())
            {
                grow();
            }
            // percolate element to it's place in tree
            if (ascendingOrder)
            {
                percolateUpMinHeap(element);
            }
            else
            {
                percolateUpMaxHeap(element);
            }
            return true;
        }

        /**
         * Gets the next element to be removed without actually removing it (peek).
         *
         * @return the next element
         * @throws BufferUnderflowException if the buffer is empty
         */
        public virtual Object get()
        {
            if (isEmpty())
            {
                throw new BufferUnderflowException();
            }
            else
            {
                return elements[1];
            }
        }

        /**
         * Gets and removes the next element (pop).
         *
         * @return the next element
         * @throws BufferUnderflowException if the buffer is empty
         */
        public virtual Object remove()
        {
            Object result = get();
            elements[1] = elements[sizeJ--];

            // set the unused element to 'null' so that the garbage collector
            // can free the object if not used anywhere else.(remove reference)
            elements[sizeJ + 1] = null;

            if (sizeJ != 0)
            {
                // percolate top element to it's place in tree
                if (ascendingOrder)
                {
                    percolateDownMinHeap(1);
                }
                else
                {
                    percolateDownMaxHeap(1);
                }
            }

            return result;
        }

        //-----------------------------------------------------------------------
        /**
         * Tests if the buffer is at capacity.
         *
         * @return <code>true</code> if buffer is full; <code>false</code> otherwise.
         */
        protected virtual bool isAtCapacity()
        {
            //+1 as element 0 is noop
            return elements.Length == sizeJ + 1;
        }


        /**
         * Percolates element down heap from the position given by the index.
         * <p>
         * Assumes it is a minimum heap.
         *
         * @param index the index for the element
         */
        protected virtual void percolateDownMinHeap(int index)
        {
            Object element = elements[index];
            int hole = index;

            while ((hole * 2) <= sizeJ)
            {
                int child = hole * 2;

                // if we have a right child and that child can not be percolated
                // up then move onto other child
                if (child != sizeJ && compare(elements[child + 1], elements[child]) < 0)
                {
                    child++;
                }

                // if we found resting place of bubble then terminate search
                if (compare(elements[child], element) >= 0)
                {
                    break;
                }

                elements[hole] = elements[child];
                hole = child;
            }

            elements[hole] = element;
        }

        /**
         * Percolates element down heap from the position given by the index.
         * <p>
         * Assumes it is a maximum heap.
         *
         * @param index the index of the element
         */
        protected virtual void percolateDownMaxHeap(int index)
        {
            Object element = elements[index];
            int hole = index;

            while ((hole * 2) <= sizeJ)
            {
                int child = hole * 2;

                // if we have a right child and that child can not be percolated
                // up then move onto other child
                if (child != sizeJ && compare(elements[child + 1], elements[child]) > 0)
                {
                    child++;
                }

                // if we found resting place of bubble then terminate search
                if (compare(elements[child], element) <= 0)
                {
                    break;
                }

                elements[hole] = elements[child];
                hole = child;
            }

            elements[hole] = element;
        }

        /**
         * Percolates element up heap from the position given by the index.
         * <p>
         * Assumes it is a minimum heap.
         *
         * @param index the index of the element to be percolated up
         */
        protected virtual void percolateUpMinHeap(int index)
        {
            int hole = index;
            Object element = elements[hole];
            while (hole > 1 && compare(element, elements[hole / 2]) < 0)
            {
                // save element that is being pushed down
                // as the element "bubble" is percolated up
                int next = hole / 2;
                elements[hole] = elements[next];
                hole = next;
            }
            elements[hole] = element;
        }

        /**
         * Percolates a new element up heap from the bottom.
         * <p>
         * Assumes it is a minimum heap.
         *
         * @param element the element
         */
        protected virtual void percolateUpMinHeap(Object element)
        {
            elements[++sizeJ] = element;
            percolateUpMinHeap(sizeJ);
        }

        /**
         * Percolates element up heap from from the position given by the index.
         * <p>
         * Assume it is a maximum heap.
         *
         * @param index the index of the element to be percolated up
         */
        protected virtual void percolateUpMaxHeap(int index)
        {
            int hole = index;
            Object element = elements[hole];

            while (hole > 1 && compare(element, elements[hole / 2]) > 0)
            {
                // save element that is being pushed down
                // as the element "bubble" is percolated up
                int next = hole / 2;
                elements[hole] = elements[next];
                hole = next;
            }

            elements[hole] = element;
        }

        /**
         * Percolates a new element up heap from the bottom.
         * <p>
         * Assume it is a maximum heap.
         *
         * @param element the element
         */
        protected virtual void percolateUpMaxHeap(Object element)
        {
            elements[++sizeJ] = element;
            percolateUpMaxHeap(sizeJ);
        }

        /**
         * Compares two objects using the comparator if specified, or the
         * natural order otherwise.
         * 
         * @param a  the first object
         * @param b  the second object
         * @return -ve if a less than b, 0 if they are equal, +ve if a greater than b
         */
        protected virtual int compare(Object a, Object b)
        {
            if (comparatorJ != null)
            {
                return comparatorJ.compare(a, b);
            }
            else
            {
                return ((java.lang.Comparable<Object>)a).compareTo(b);
            }
        }

        /**
         * Increases the size of the heap to support additional elements
         */
        protected virtual void grow()
        {
            Object[] array = new Object[elements.Length * 2];
            java.lang.SystemJ.arraycopy(elements, 0, array, 0, elements.Length);
            elements = array;
        }

        //-----------------------------------------------------------------------
        /**
         * Returns an iterator over this heap's elements.
         *
         * @return an iterator over this heap's elements
         */
        public override java.util.Iterator<Object> iterator()
        {
            return new IAC_PriorityBufferIterator(this);
        }

        /**
         * Returns a string representation of this heap.  The returned string
         * is similar to those produced by standard JDK collections.
         *
         * @return a string representation of this heap
         */
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.append("[ ");

            for (int i = 1; i < sizeJ + 1; i++)
            {
                if (i != 1)
                {
                    sb.append(", ");
                }
                sb.append(elements[i]);
            }

            sb.append(" ]");

            return sb.toString();
        }

        internal class IAC_PriorityBufferIterator : java.util.Iterator<Object>
        {

            private PriorityBuffer root;
            public IAC_PriorityBufferIterator(PriorityBuffer root)
            {
                this.root = root;
            }

            private int index = 1;
            private int lastReturnedIndex = -1;

            public bool hasNext()
            {
                return index <= this.root.sizeJ;
            }

            public Object next()
            {
                if (!hasNext())
                {
                    throw new java.util.NoSuchElementException();
                }
                lastReturnedIndex = index;
                index++;
                return this.root.elements[lastReturnedIndex];
            }

            public void remove()
            {
                if (lastReturnedIndex == -1)
                {
                    throw new java.lang.IllegalStateException();
                }
                this.root.elements[lastReturnedIndex] = this.root.elements[this.root.sizeJ];
                this.root.elements[this.root.sizeJ] = null;
                this.root.sizeJ--;
                if (this.root.sizeJ != 0 && lastReturnedIndex <= this.root.sizeJ)
                {
                    int compareToParent = 0;
                    if (lastReturnedIndex > 1)
                    {
                        compareToParent = this.root.compare(this.root.elements[lastReturnedIndex],
                            this.root.elements[lastReturnedIndex / 2]);
                    }
                    if (this.root.ascendingOrder)
                    {
                        if (lastReturnedIndex > 1 && compareToParent < 0)
                        {
                            this.root.percolateUpMinHeap(lastReturnedIndex);
                        }
                        else
                        {
                            this.root.percolateDownMinHeap(lastReturnedIndex);
                        }
                    }
                    else
                    {  // max heap
                        if (lastReturnedIndex > 1 && compareToParent > 0)
                        {
                            this.root.percolateUpMaxHeap(lastReturnedIndex);
                        }
                        else
                        {
                            this.root.percolateDownMaxHeap(lastReturnedIndex);
                        }
                    }
                }
                index--;
                lastReturnedIndex = -1;
            }
        };
    }
}