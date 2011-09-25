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

namespace org.apache.commons.collections.buffer
{

    /**
     * The BoundedFifoBuffer is a very efficient implementation of
     * <code>Buffer</code> that is of a fixed size.
     * <p>
     * The removal order of a <code>BoundedFifoBuffer</code> is based on the 
     * insertion order; elements are removed in the same order in which they
     * were added.  The iteration order is the same as the removal order.
     * <p>
     * The {@link #add(Object)}, {@link #remove()} and {@link #get()} operations
     * all perform in constant time.  All other operations perform in linear
     * time or worse.
     * <p>
     * Note that this implementation is not synchronized.  The following can be
     * used to provide synchronized access to your <code>BoundedFifoBuffer</code>:
     * <pre>
     *   Buffer fifo = BufferUtils.synchronizedBuffer(new BoundedFifoBuffer());
     * </pre>
     * <p>
     * This buffer prevents null objects from being added.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0 (previously in main package v2.1)
     * @version $Revision$ $Date$
     * 
     * @author Avalon
     * @author Berin Loritsch
     * @author Paul Jack
     * @author Stephen Colebourne
     * @author Herve Quiroz
     */
    [Serializable]
    public class BoundedFifoBuffer : java.util.AbstractCollection<Object>, Buffer, BoundedCollection, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 5603722811189451017L;

        /** Underlying storage array */
        [NonSerialized]
        internal Object[] elements;

        /** Array index of first (oldest) buffer element */
        [NonSerialized]
        internal int start = 0;

        /** 
         * Index mod maxElements of the array position following the last buffer
         * element.  Buffer elements start at elements[start] and "wrap around"
         * elements[maxElements-1], ending at elements[decrement(end)].  
         * For example, elements = {c,a,b}, start=1, end=1 corresponds to 
         * the buffer [a,b,c].
         */
        [NonSerialized]
        internal int end = 0;

        /** Flag to indicate if the buffer is currently full. */
        [NonSerialized]
        internal bool full = false;

        /** Capacity of the buffer */
        internal readonly int maxElements;

        /**
         * Constructs a new <code>BoundedFifoBuffer</code> big enough to hold
         * 32 elements.
         */
        public BoundedFifoBuffer()
            : this(32)
        {
        }

        /**
         * Constructs a new <code>BoundedFifoBuffer</code> big enough to hold
         * the specified number of elements.
         *
         * @param size  the maximum number of elements for this fifo
         * @throws IllegalArgumentException  if the size is less than 1
         */
        public BoundedFifoBuffer(int size)
        {
            if (size <= 0)
            {
                throw new java.lang.IllegalArgumentException("The size must be greater than 0");
            }
            elements = new Object[size];
            maxElements = elements.Length;
        }

        /**
         * Constructs a new <code>BoundedFifoBuffer</code> big enough to hold all
         * of the elements in the specified collection. That collection's
         * elements will also be added to the buffer.
         *
         * @param coll  the collection whose elements to add, may not be null
         * @throws NullPointerException if the collection is null
         */
        public BoundedFifoBuffer(java.util.Collection<Object> coll)
            : this(coll.size())
        {
            addAll(coll);
        }

        //-----------------------------------------------------------------------
        /**
         * Write the buffer out using a custom routine.
         * 
         * @param out  the output stream
         * @throws IOException
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {// throws IOException {
            outJ.defaultWriteObject();
            outJ.writeInt(size());
            for (java.util.Iterator<Object> it = iterator(); it.hasNext(); )
            {
                outJ.writeObject(it.next());
            }
        }

        /**
         * Read the buffer in using a custom routine.
         * 
         * @param in  the input stream
         * @throws IOException
         * @throws ClassNotFoundException
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            elements = new Object[maxElements];
            int size = inJ.readInt();
            for (int i = 0; i < size; i++)
            {
                elements[i] = inJ.readObject();
            }
            start = 0;
            full = (size == maxElements);
            if (full)
            {
                end = 0;
            }
            else
            {
                end = size;
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Returns the number of elements stored in the buffer.
         *
         * @return this buffer's size
         */
        public override int size()
        {
            int size = 0;

            if (end < start)
            {
                size = maxElements - start + end;
            }
            else if (end == start)
            {
                size = (full ? maxElements : 0);
            }
            else
            {
                size = end - start;
            }

            return size;
        }

        /**
         * Returns true if this buffer is empty; false otherwise.
         *
         * @return true if this buffer is empty
         */
        public override bool isEmpty()
        {
            return size() == 0;
        }

        /**
         * Returns true if this collection is full and no new elements can be added.
         *
         * @return <code>true</code> if the collection is full
         */
        public virtual bool isFull()
        {
            return size() == maxElements;
        }

        /**
         * Gets the maximum size of the collection (the bound).
         *
         * @return the maximum number of elements the collection can hold
         */
        public virtual int maxSize()
        {
            return maxElements;
        }

        /**
         * Clears this buffer.
         */
        public override void clear()
        {
            full = false;
            start = 0;
            end = 0;
            java.util.Arrays<Object>.fill(elements, null);
        }

        /**
         * Adds the given element to this buffer.
         *
         * @param element  the element to add
         * @return true, always
         * @throws NullPointerException  if the given element is null
         * @throws BufferOverflowException  if this buffer is full
         */
        public override bool add(Object element)
        {
            if (null == element)
            {
                throw new java.lang.NullPointerException("Attempted to add null object to buffer");
            }

            if (full)
            {
                throw new BufferOverflowException("The buffer cannot hold more than " + maxElements + " objects.");
            }

            elements[end++] = element;

            if (end >= maxElements)
            {
                end = 0;
            }

            if (end == start)
            {
                full = true;
            }

            return true;
        }

        /**
         * Returns the least recently inserted element in this buffer.
         *
         * @return the least recently inserted element
         * @throws BufferUnderflowException  if the buffer is empty
         */
        public virtual Object get()
        {
            if (isEmpty())
            {
                throw new BufferUnderflowException("The buffer is already empty");
            }

            return elements[start];
        }

        /**
         * Removes the least recently inserted element from this buffer.
         *
         * @return the least recently inserted element
         * @throws BufferUnderflowException  if the buffer is empty
         */
        public virtual Object remove()
        {
            if (isEmpty())
            {
                throw new BufferUnderflowException("The buffer is already empty");
            }

            Object element = elements[start];

            if (null != element)
            {
                elements[start++] = null;

                if (start >= maxElements)
                {
                    start = 0;
                }

                full = false;
            }

            return element;
        }

        /**
         * Increments the internal index.
         * 
         * @param index  the index to increment
         * @return the updated index
         */
        internal virtual int increment(int index)
        {
            index++;
            if (index >= maxElements)
            {
                index = 0;
            }
            return index;
        }

        /**
         * Decrements the internal index.
         * 
         * @param index  the index to decrement
         * @return the updated index
         */
        internal virtual int decrement(int index)
        {
            index--;
            if (index < 0)
            {
                index = maxElements - 1;
            }
            return index;
        }

        /**
         * Returns an iterator over this buffer's elements.
         *
         * @return an iterator over this buffer's elements
         */
        public override java.util.Iterator<Object> iterator()
        {
            return new IAC_BoundedFifoBufferIterator(this);
        }


    }
    internal class IAC_BoundedFifoBufferIterator : java.util.Iterator<Object>
    {
        private readonly BoundedFifoBuffer root;
        private int index;
        private int lastReturnedIndex = -1;
        private bool isFirst;

        public IAC_BoundedFifoBufferIterator(BoundedFifoBuffer bfb)
        {
            root = bfb;
            index = root.start;
            isFirst = root.full;
        }

        public virtual void remove()
        {
            if (lastReturnedIndex == -1)
            {
                throw new java.lang.IllegalStateException();
            }

            // First element can be removed quickly
            if (lastReturnedIndex == root.start)
            {
                root.remove();
                lastReturnedIndex = -1;
                return;
            }

            int pos = lastReturnedIndex + 1;
            if (root.start < lastReturnedIndex && pos < root.end)
            {
                // shift in one part
                java.lang.SystemJ.arraycopy(root.elements, pos, root.elements,
                        lastReturnedIndex, root.end - pos);
            }
            else
            {
                // Other elements require us to shift the subsequent elements
                while (pos != root.end)
                {
                    if (pos >= root.maxElements)
                    {
                        root.elements[pos - 1] = root.elements[0];
                        pos = 0;
                    }
                    else
                    {
                        root.elements[root.decrement(pos)] = root.elements[pos];
                        pos = root.increment(pos);
                    }
                }
            }

            lastReturnedIndex = -1;
            root.end = root.decrement(root.end);
            root.elements[root.end] = null;
            root.full = false;
            index = root.decrement(index);
        }

        public virtual bool hasNext()
        {
            return isFirst || (index != root.end);

        }

        public virtual Object next()
        {
            if (!hasNext())
            {
                throw new java.util.NoSuchElementException();
            }
            isFirst = false;
            lastReturnedIndex = index;
            index = root.increment(index);
            return root.elements[lastReturnedIndex];
        }
    }
}