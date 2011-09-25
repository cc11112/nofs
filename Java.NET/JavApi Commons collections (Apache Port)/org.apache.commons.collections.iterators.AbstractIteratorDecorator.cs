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
     * Provides basic behaviour for decorating an iterator with extra functionality.
     * <p>
     * All methods are forwarded to the decorated iterator.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author James Strachan
     * @author Stephen Colebourne
     */
    public class AbstractIteratorDecorator : java.util.Iterator<Object>
    {

        /** The iterator being decorated */
        protected readonly java.util.Iterator<Object> iterator;

        //-----------------------------------------------------------------------
        /**
         * Constructor that decorates the specified iterator.
         *
         * @param iterator  the iterator to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        public AbstractIteratorDecorator(java.util.Iterator<Object> iterator)
            : base()
        {
            if (iterator == null)
            {
                throw new java.lang.IllegalArgumentException("Iterator must not be null");
            }
            this.iterator = iterator;
        }

        /**
         * Gets the iterator being decorated.
         * 
         * @return the decorated iterator
         */
        protected virtual java.util.Iterator<Object> getIterator()
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

        public virtual void remove()
        {
            iterator.remove();
        }

    }
}