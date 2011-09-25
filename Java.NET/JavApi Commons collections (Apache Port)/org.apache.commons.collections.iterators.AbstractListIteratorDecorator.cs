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

namespace org.apache.commons.collections.iterators
{

    /**
     * Provides basic behaviour for decorating a list iterator with extra functionality.
     * <p>
     * All methods are forwarded to the decorated list iterator.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Rodney Waldhoff
     * @author Stephen Colebourne
     */
    public class AbstractListIteratorDecorator : java.util.ListIterator<Object>
    {

        /** The iterator being decorated */
        protected readonly java.util.ListIterator<Object> iterator;

        //-----------------------------------------------------------------------
        /**
         * Constructor that decorates the specified iterator.
         *
         * @param iterator  the iterator to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        public AbstractListIteratorDecorator(java.util.ListIterator<Object> iterator)
            : base()
        {
            if (iterator == null)
            {
                throw new java.lang.IllegalArgumentException("ListIterator must not be null");
            }
            this.iterator = iterator;
        }

        /**
         * Gets the iterator being decorated.
         * 
         * @return the decorated iterator
         */
        protected virtual java.util.ListIterator<Object> getListIterator()
        {
            return iterator;
        }

        //-----------------------------------------------------------------------
        public virtual bool hasNext()
        {
            return iterator.hasNext();
        }

        public virtual Object next()
        {
            return iterator.next();
        }

        public virtual int nextIndex()
        {
            return iterator.nextIndex();
        }

        public virtual bool hasPrevious()
        {
            return iterator.hasPrevious();
        }

        public virtual Object previous()
        {
            return iterator.previous();
        }

        public virtual int previousIndex()
        {
            return iterator.previousIndex();
        }

        public virtual void remove()
        {
            iterator.remove();
        }

        public virtual void set(Object obj)
        {
            iterator.set(obj);
        }

        public virtual void add(Object obj)
        {
            iterator.add(obj);
        }

    }
}