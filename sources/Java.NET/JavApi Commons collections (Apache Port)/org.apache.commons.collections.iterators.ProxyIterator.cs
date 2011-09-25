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
     * A Proxy {@link Iterator Iterator} which delegates its methods to a proxy instance.
     *
     * @deprecated Use AbstractIteratorDecorator. Will be removed in v4.0
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author James Strachan
     */
    public class ProxyIterator : java.util.Iterator<Object>
    {

        /** Holds value of property iterator. */
        private java.util.Iterator<Object> iterator;

        // Constructors
        //-------------------------------------------------------------------------

        /**
         * Constructs a new <code>ProxyIterator</code> that will not function
         * until {@link #setIterator(Iterator)} is called.
         */
        public ProxyIterator()
            : base()
        {
        }

        /**
         * Constructs a new <code>ProxyIterator</code> that will use the
         * given iterator.
         *
         * @param iterator  the underlying iterator
         */
        public ProxyIterator(java.util.Iterator<Object> iterator)
            : base()
        {
            this.iterator = iterator;
        }

        // Iterator interface
        //-------------------------------------------------------------------------

        /**
         *  Returns true if the underlying iterator has more elements.
         *
         *  @return true if the underlying iterator has more elements
         */
        public bool hasNext()
        {
            return getIterator().hasNext();
        }

        /**
         *  Returns the next element from the underlying iterator.
         *
         *  @return the next element from the underlying iterator
         *  @throws java.util.NoSuchElementException  if the underlying iterator 
         *    raises it because it has no more elements
         */
        public Object next()
        {
            return getIterator().next();
        }

        /**
         *  Removes the last returned element from the collection that spawned
         *  the underlying iterator.
         */
        public void remove()
        {
            getIterator().remove();
        }

        // Properties
        //-------------------------------------------------------------------------
        /** Getter for property iterator.
         * @return Value of property iterator.
         */
        public java.util.Iterator<Object> getIterator()
        {
            return iterator;
        }
        /** Setter for property iterator.
         * @param iterator New value of property iterator.
         */
        public void setIterator(java.util.Iterator<Object> iterator)
        {
            this.iterator = iterator;
        }
    }
}