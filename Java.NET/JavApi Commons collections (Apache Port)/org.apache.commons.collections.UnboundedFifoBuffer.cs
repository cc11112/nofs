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
     * UnboundedFifoBuffer is a very efficient buffer implementation.
     * According to performance testing, it exhibits a constant access time, but it
     * also outperforms ArrayList when used for the same purpose.
     * <p>
     * The removal order of an <code>UnboundedFifoBuffer</code> is based on the insertion
     * order; elements are removed in the same order in which they were added.
     * The iteration order is the same as the removal order.
     * <p>
     * The {@link #remove()} and {@link #get()} operations perform in constant time.
     * The {@link #add(Object)} operation performs in amortized constant time.  All
     * other operations perform in linear time or worse.
     * <p>
     * Note that this implementation is not synchronized.  The following can be
     * used to provide synchronized access to your <code>UnboundedFifoBuffer</code>:
     * <pre>
     *   Buffer fifo = BufferUtils.synchronizedBuffer(new UnboundedFifoBuffer());
     * </pre>
     * <p>
     * This buffer prevents null objects from being added.
     * 
     * @deprecated Moved to buffer subpackage. Due to be removed in v4.0.
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     *
     * @author Avalon
     * @author Federico Barbieri
     * @author Berin Loritsch
     * @author Paul Jack
     * @author Stephen Colebourne
     * @author Andreas Schlosser
     */
    [Obsolete]
    public class UnboundedFifoBuffer : java.util.AbstractCollection<Object>, Buffer
    {

        protected internal Object[] m_buffer;
        protected internal int m_head;
        protected internal int m_tail;

        /**
         * Constructs an UnboundedFifoBuffer with the default number of elements.
         * It is exactly the same as performing the following:
         *
         * <pre>
         *   new UnboundedFifoBuffer(32);
         * </pre>
         */
        public UnboundedFifoBuffer()
            : this(32)
        {
        }

        /**
         * Constructs an UnboundedFifoBuffer with the specified number of elements.
         * The integer must be a positive integer.
         * 
         * @param initialSize  the initial size of the buffer
         * @throws IllegalArgumentException  if the size is less than 1
         */
        public UnboundedFifoBuffer(int initialSize)
        {
            if (initialSize <= 0)
            {
                throw new java.lang.IllegalArgumentException("The size must be greater than 0");
            }
            m_buffer = new Object[initialSize + 1];
            m_head = 0;
            m_tail = 0;
        }

        /**
         * Returns the number of elements stored in the buffer.
         *
         * @return this buffer's size
         */
        public override int size()
        {
            int size = 0;

            if (m_tail < m_head)
            {
                size = m_buffer.Length - m_head + m_tail;
            }
            else
            {
                size = m_tail - m_head;
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
            return (size() == 0);
        }

        /**
         * Adds the given element to this buffer.
         *
         * @param obj  the element to add
         * @return true, always
         * @throws NullPointerException  if the given element is null
         * @throws BufferOverflowException  if this buffer is full
         */
        public override bool add(Object obj)
        {
            if (obj == null)
            {
                throw new java.lang.NullPointerException("Attempted to add null object to buffer");
            }

            if (size() + 1 >= m_buffer.Length)
            {
                Object[] tmp = new Object[((m_buffer.Length - 1) * 2) + 1];

                int j = 0;
                for (int i = m_head; i != m_tail; )
                {
                    tmp[j] = m_buffer[i];
                    m_buffer[i] = null;

                    j++;
                    i++;
                    if (i == m_buffer.Length)
                    {
                        i = 0;
                    }
                }

                m_buffer = tmp;
                m_head = 0;
                m_tail = j;
            }

            m_buffer[m_tail] = obj;
            m_tail++;
            if (m_tail >= m_buffer.Length)
            {
                m_tail = 0;
            }
            return true;
        }

        /**
         * Returns the next object in the buffer.
         *
         * @return the next object in the buffer
         * @throws BufferUnderflowException  if this buffer is empty
         */
        public virtual Object get()
        {
            if (isEmpty())
            {
                throw new BufferUnderflowException("The buffer is already empty");
            }

            return m_buffer[m_head];
        }

        /**
         * Removes the next object from the buffer
         *
         * @return the removed object
         * @throws BufferUnderflowException  if this buffer is empty
         */
        public virtual Object remove()
        {
            if (isEmpty())
            {
                throw new BufferUnderflowException("The buffer is already empty");
            }

            Object element = m_buffer[m_head];

            if (null != element)
            {
                m_buffer[m_head] = null;

                m_head++;
                if (m_head >= m_buffer.Length)
                {
                    m_head = 0;
                }
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
            if (index >= m_buffer.Length)
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
                index = m_buffer.Length - 1;
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
            return new IAC_UnboundedFifoBufferIterator(this);
        }

    }
    internal class IAC_UnboundedFifoBufferIterator : java.util.Iterator<Object>
    {
        private UnboundedFifoBuffer root;

        internal IAC_UnboundedFifoBufferIterator(UnboundedFifoBuffer root)
        {
            this.root = root;
            index = root.m_head;
        }

        private int index;
        private int lastReturnedIndex = -1;

        public virtual bool hasNext()
        {
            return index != root.m_tail;

        }

        public virtual Object next()
        {
            if (!hasNext())
                throw new java.util.NoSuchElementException();
            lastReturnedIndex = index;
            index = root.increment(index);
            return root.m_buffer[lastReturnedIndex];
        }

        public virtual void remove()
        {
            if (lastReturnedIndex == -1)
                throw new java.lang.IllegalStateException();

            // First element can be removed quickly
            if (lastReturnedIndex == root.m_head)
            {
                root.remove();
                lastReturnedIndex = -1;
                return;
            }

            // Other elements require us to shift the subsequent elements
            int i = root.increment(lastReturnedIndex);
            while (i != root.m_tail)
            {
                root.m_buffer[root.decrement(i)] = root.m_buffer[i];
                i = root.increment(i);
            }

            lastReturnedIndex = -1;
            root.m_tail = root.decrement(root.m_tail);
            root.m_buffer[root.m_tail] = null;
            index = root.decrement(index);
        }

    }
}
