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
     * Provides an implementation of an empty iterator.
     *
     * @since Commons Collections 3.1
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractEmptyIterator
    {

        /**
         * Constructor.
         */
        protected AbstractEmptyIterator()
            : base()
        {
        }

        public bool hasNext()
        {
            return false;
        }

        public Object next()
        {
            throw new java.util.NoSuchElementException("Iterator contains no elements");
        }

        public bool hasPrevious()
        {
            return false;
        }

        public Object previous()
        {
            throw new java.util.NoSuchElementException("Iterator contains no elements");
        }

        public int nextIndex()
        {
            return 0;
        }

        public int previousIndex()
        {
            return -1;
        }

        public void add(Object obj)
        {
            throw new java.lang.UnsupportedOperationException("add() not supported for empty Iterator");
        }

        public void set(Object obj)
        {
            throw new java.lang.IllegalStateException("Iterator contains no elements");
        }

        public void remove()
        {
            throw new java.lang.IllegalStateException("Iterator contains no elements");
        }

        public Object getKey()
        {
            throw new java.lang.IllegalStateException("Iterator contains no elements");
        }

        public Object getValue()
        {
            throw new java.lang.IllegalStateException("Iterator contains no elements");
        }

        public Object setValue(Object value)
        {
            throw new java.lang.IllegalStateException("Iterator contains no elements");
        }

        public void reset()
        {
            // do nothing
        }

    }
}