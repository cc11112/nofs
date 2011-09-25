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

namespace org.apache.commons.collections.iterators
{

    /** 
     * Decorates a list iterator such that it cannot be modified.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableListIterator : java.util.ListIterator<Object>, Unmodifiable
    {

        /** The iterator being decorated */
        private java.util.ListIterator<Object> iterator;

        //-----------------------------------------------------------------------
        /**
         * Decorates the specified iterator such that it cannot be modified.
         *
         * @param iterator  the iterator to decorate
         * @throws IllegalArgumentException if the iterator is null
         */
        public static java.util.ListIterator<Object> decorate(java.util.ListIterator<Object> iterator)
        {
            if (iterator == null)
            {
                throw new java.lang.IllegalArgumentException("ListIterator must not be null");
            }
            if (iterator is Unmodifiable)
            {
                return iterator;
            }
            return new UnmodifiableListIterator(iterator);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor.
         *
         * @param iterator  the iterator to decorate
         */
        private UnmodifiableListIterator(java.util.ListIterator<Object> iterator)
            : base()
        {
            this.iterator = iterator;
        }

        //-----------------------------------------------------------------------
        public bool hasNext()
        {
            return iterator.hasNext();
        }

        public Object next()
        {
            return iterator.next();
        }

        public int nextIndex()
        {
            return iterator.nextIndex();
        }

        public bool hasPrevious()
        {
            return iterator.hasPrevious();
        }

        public Object previous()
        {
            return iterator.previous();
        }

        public int previousIndex()
        {
            return iterator.previousIndex();
        }

        public void remove()
        {
            throw new java.lang.UnsupportedOperationException("remove() is not supported");
        }

        public void set(Object obj)
        {
            throw new java.lang.UnsupportedOperationException("set() is not supported");
        }

        public void add(Object obj)
        {
            throw new java.lang.UnsupportedOperationException("add() is not supported");
        }

    }
}