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
     * Adapter to make {@link Enumeration Enumeration} instances appear
     * to be {@link Iterator Iterator} instances.
     *
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author <a href="mailto:jstrachan@apache.org">James Strachan</a>
     * @author <a href="mailto:dlr@finemaltcoding.com">Daniel Rall</a>
     */
    public class EnumerationIterator : java.util.Iterator<Object>
    {

        /** The collection to remove elements from */
        private java.util.Collection<Object> collection;
        /** The enumeration being converted */
        private java.util.Enumeration<Object> enumeration;
        /** The last object retrieved */
        private Object last;

        // Constructors
        //-----------------------------------------------------------------------
        /**
         * Constructs a new <code>EnumerationIterator</code> that will not
         * function until {@link #setEnumeration(Enumeration)} is called.
         */
        public EnumerationIterator()
            : this(null, null)
        {
        }

        /**
         * Constructs a new <code>EnumerationIterator</code> that provides
         * an iterator view of the given enumeration.
         *
         * @param enumeration  the enumeration to use
         */
        public EnumerationIterator(java.util.Enumeration<Object> enumeration)
            : this(enumeration, null)
        {
        }

        /**
         * Constructs a new <code>EnumerationIterator</code> that will remove
         * elements from the specified collection.
         *
         * @param enumeration  the enumeration to use
         * @param collection  the collection to remove elements form
         */
        public EnumerationIterator(java.util.Enumeration<Object> enumeration, java.util.Collection<Object> collection)
            : base()
        {
            this.enumeration = enumeration;
            this.collection = collection;
            this.last = null;
        }

        // Iterator interface
        //-----------------------------------------------------------------------
        /**
         * Returns true if the underlying enumeration has more elements.
         *
         * @return true if the underlying enumeration has more elements
         * @throws NullPointerException  if the underlying enumeration is null
         */
        public bool hasNext()
        {
            return enumeration.hasMoreElements();
        }

        /**
         * Returns the next object from the enumeration.
         *
         * @return the next object from the enumeration
         * @throws NullPointerException if the enumeration is null
         */
        public Object next()
        {
            last = enumeration.nextElement();
            return last;
        }

        /**
         * Removes the last retrieved element if a collection is attached.
         * <p>
         * Functions if an associated <code>Collection</code> is known.
         * If so, the first occurrence of the last returned object from this
         * iterator will be removed from the collection.
         *
         * @exception IllegalStateException <code>next()</code> not called.
         * @exception UnsupportedOperationException if no associated collection
         */
        public void remove()
        {
            if (this.collection != null)
            {
                if (last != null)
                {
                    collection.remove(last);
                }
                else
                {
                    throw new java.lang.IllegalStateException("next() must have been called for remove() to function");
                }
            }
            else
            {
                throw new java.lang.UnsupportedOperationException("No Collection associated with this Iterator");
            }
        }

        // Properties
        //-----------------------------------------------------------------------
        /**
         * Returns the underlying enumeration.
         *
         * @return the underlying enumeration
         */
        public java.util.Enumeration<Object> getEnumeration()
        {
            return enumeration;
        }

        /**
         * Sets the underlying enumeration.
         *
         * @param enumeration  the new underlying enumeration
         */
        public void setEnumeration(java.util.Enumeration<Object> enumeration)
        {
            this.enumeration = enumeration;
        }

    }
}