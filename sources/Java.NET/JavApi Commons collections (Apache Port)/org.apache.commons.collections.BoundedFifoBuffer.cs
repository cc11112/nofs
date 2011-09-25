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

namespace org.apache.commons.collections
{

    /**
     * <strong>Basties note:</strong> reimplemented deprecated class as decorator for new class.<p/>
     * 
     * The BoundedFifoBuffer is a very efficient implementation of
     * Buffer that does not alter the size of the buffer at runtime.
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
     *
     * @deprecated Moved to buffer subpackage. Due to be removed in v4.0.
     * @since 2.1
     * @version $Revision$ $Date$
     * 
     * @author Avalon
     * @author Berin Loritsch
     * @author Paul Jack
     * @author Stephen Colebourne
     * @author Herve Quiroz
     */
    public class BoundedFifoBuffer : org.apache.commons.collections.buffer.BoundedFifoBuffer
    {// extends AbstractCollection implements Buffer, BoundedCollection {

        /**
         * Constructs a new <code>BoundedFifoBuffer</code> big enough to hold
         * 32 elements.
         */
        public BoundedFifoBuffer()
            : base(32)
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
            : base(size)
        {
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
            : base(coll)
        {
        }

        /**
         * Returns the number of elements stored in the buffer.
         *
         * @return this buffer's size
         */
        public override int size()
        {
            return base.size();
        }

        /**
         * Returns true if this buffer is empty; false otherwise.
         *
         * @return true if this buffer is empty
         */
        public override bool isEmpty()
        {
            return base.isEmpty();
        }

        /**
         * Returns true if this collection is full and no new elements can be added.
         *
         * @return <code>true</code> if the collection is full
         */
        public override bool isFull()
        {
            return base.isFull();
        }

        /**
         * Gets the maximum size of the collection (the bound).
         *
         * @return the maximum number of elements the collection can hold
         */
        public override int maxSize()
        {
            return base.maxSize();
        }

        /**
         * Clears this buffer.
         */
        public override void clear()
        {
            base.clear();
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
            return base.add(element);
        }

        /**
         * Returns the least recently inserted element in this buffer.
         *
         * @return the least recently inserted element
         * @throws BufferUnderflowException  if the buffer is empty
         */
        public override Object get()
        {
            return base.get();
        }

        /**
         * Removes the least recently inserted element from this buffer.
         *
         * @return the least recently inserted element
         * @throws BufferUnderflowException  if the buffer is empty
         */
        public override Object remove()
        {
            return base.remove();
        }

        /**
         * Increments the internal index.
         * 
         * @param index  the index to increment
         * @return the updated index
         */
        internal override int increment(int index)
        {
            return base.increment(index);
        }

        /**
         * Decrements the internal index.
         * 
         * @param index  the index to decrement
         * @return the updated index
         */
        internal override int decrement(int index)
        {
            return base.decrement(index);
        }

        /**
         * Returns an iterator over this buffer's elements.
         *
         * @return an iterator over this buffer's elements
         */
        public override java.util.Iterator<Object> iterator()
        {
            return base.iterator();
        }

    }
}
