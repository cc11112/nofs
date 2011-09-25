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
     * A thread safe version of the PriorityQueue.
     * Provides synchronized wrapper methods for all the methods 
     * defined in the PriorityQueue interface.
     *
     * @deprecated PriorityQueue is replaced by the Buffer interface, see buffer subpackage.
     *  Due to be removed in v4.0.
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author Ram Chidambaram
     */
    [Obsolete]
    public sealed class SynchronizedPriorityQueue : PriorityQueue
    {

        /**
         * The underlying priority queue.
         */
        private readonly PriorityQueue m_priorityQueue;

        /**
         * Constructs a new synchronized priority queue.
         *
         * @param priorityQueue  the priority queue to synchronize
         */
        public SynchronizedPriorityQueue(PriorityQueue priorityQueue)
        {
            m_priorityQueue = priorityQueue;
        }

        /**
         * Clear all elements from queue.
         */
        public void clear()
        {
            lock (this)
            {
                m_priorityQueue.clear();
            }
        }

        /**
         * Test if queue is empty.
         *
         * @return true if queue is empty else false.
         */
        public bool isEmpty()
        {
            lock (this)
            {
                return m_priorityQueue.isEmpty();
            }
        }

        /**
         * Insert an element into queue.
         *
         * @param element the element to be inserted
         */
        public void insert(Object element)
        {
            lock (this)
            {
                m_priorityQueue.insert(element);
            }
        }

        /**
         * Return element on top of heap but don't remove it.
         *
         * @return the element at top of heap
         * @throws NoSuchElementException if isEmpty() == true
         */
        public Object peek()
        {//throws NoSuchElementException {
            lock (this)
            {
                return m_priorityQueue.peek();
            }
        }

        /**
         * Return element on top of heap and remove it.
         *
         * @return the element at top of heap
         * @throws NoSuchElementException if isEmpty() == true
         */
        public Object pop()
        {//throws NoSuchElementException {
            lock (this)
            {
                return m_priorityQueue.pop();
            }
        }

        /**
         * Returns a string representation of the underlying queue.
         *
         * @return a string representation of the underlying queue
         */
        public override String ToString()
        {
            lock (this)
            {
                return m_priorityQueue.toString();
            }
        }

    }

}